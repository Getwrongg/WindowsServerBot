using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TradingPlatform
{
    public class BitMexApi
    {
        Global Settings = Global.Instance;

        #region Variables
        private const string domain = "https://www.bitmex.com";
        private string apiKey;
        private string apiSecret;
        private int rateLimit;
        #endregion Variables

        //sets default variables for bitmex access
        public void setBitmex(int aRateLimit, string ApiKey, string ApiSecret)
        {
            //rateLimit = 5000;
            apiKey = ApiKey;
            apiSecret = ApiSecret;
            rateLimit = aRateLimit;
        }

        //get user details
        public void GetUser()
        {
            var param = new Dictionary<string, string>();
            string test = Query("GET", "/user", param, true);
            Console.WriteLine(test);
        }

        //request current wallet balance
        public List<string> GetWallet()
        {
            List<string> list = new List<string>();
            var param = new Dictionary<string, string>();
            string test = Query("GET", "/user/wallet", param, true);

            JToken token = JObject.Parse(test);
            string currency = (string)token.SelectToken("currency");
            string amount = (string)token.SelectToken("amount");

            list.Add(currency);
            list.Add("Available " + amount);
            return list;
        }

        //requests a buy order
        public string PostBuyOrders(string symbol, double price)
        {
            var param = new Dictionary<string, string>();
            param["symbol"] = symbol;
            param["side"] = "Buy";
            param["orderQty"] = price.ToString();
            param["ordType"] = "Market";

            return Query("POST", "/order", param, true);
        }

        //requests a sell order
        public string PostSellOrders(string symbol, double price)
        {
            var param = new Dictionary<string, string>();
            param["symbol"] = symbol;
            param["side"] = "Sell";
            param["orderQty"] = price.ToString();
            param["ordType"] = "Market";

            return Query("POST", "/order", param, true);
        }

        //deletes order by ID
        public string DeleteOrders(string orderID)
        {
            var param = new Dictionary<string, string>();
            param["orderID"] = orderID;
            param["text"] = "cancel order by ID";
            return Query("DELETE", "/order", param, true, true);
        }

        public void GetCandleData(List<string> symbol)
        {                
            for (int i = 0; i < symbol.Count; i++)
            {
                var param = new Dictionary<string, string>();
                param["symbol"] = symbol[i];
                param["binSize"] = Settings.candleBinSize;
                param["count"] = Settings.candleCount.ToString();
                param["reverse"] = "true";
                param["partial"] = "true";

                string json;
                dynamic results;

                json = Query("GET", "/trade/bucketed", param, true);
                results = JsonConvert.DeserializeObject<dynamic>(json);
 
                List<Candle> list = new List<Candle>();
                for(int j = 0; j < results.Count; j++)
                {
            
                    string val = Convert.ToString(results[j]);
                    JToken token = JObject.Parse(val);

                    Candle candle = new Candle();
                    candle.Timestamp = (string)token.SelectToken("timestamp");
                    candle.Symbol = (string)token.SelectToken("symbol");

                    try
                    {
                        candle.Open = (float)token.SelectToken("open");
                        candle.High = (float)token.SelectToken("high");
                        candle.Low = (float)token.SelectToken("low");
                        candle.Close = (float)token.SelectToken("close");
                        candle.Volume = (float)token.SelectToken("volume");
                        list.Add(candle);
                    }
                    catch(Exception e)
                    {
                        
                    }
                }
                list.Reverse();
                Settings.CandlesList.Add(list);
            }
        }

        public void GetCandleHistory(string symbol)
        {
            var param = new Dictionary<string, string>();
            param["symbol"] = symbol;
            param["binSize"] = Settings.candleBinSize;
            param["reverse"] = "true";
            param["startTime"] = "2021";
            param["count"] = "1000";

            string json = Query("GET", "/trade/bucketed", param, true);
            dynamic results = JsonConvert.DeserializeObject<dynamic>(json);

            for (int j = 0; j < results.Count; j++)
            {
                try
                {
                    string val = Convert.ToString(results[j]);
                    JToken token = JObject.Parse(val);

                    Candle candle = new Candle();
                    candle.Timestamp = (string)token.SelectToken("timestamp");
                    candle.Symbol = (string)token.SelectToken("symbol");
                    candle.Open = (float)token.SelectToken("open");
                    candle.High = (float)token.SelectToken("high");
                    candle.Low = (float)token.SelectToken("low");
                    candle.Close = (float)token.SelectToken("close");
                    candle.Volume = (float)token.SelectToken("volume");
                    Settings.TestingList.Add(candle);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //GetCandleHistory(symbol);
                }
            }
            Settings.TestingList.Reverse();    
        }

        //all api functions for Bitmex API
        #region APIfunctions

        private string BuildQueryData(Dictionary<string, string> param)
        {
            if (param == null)
                return "";

            StringBuilder b = new StringBuilder();
            foreach (var item in param)
                b.Append(string.Format("&{0}={1}", item.Key, WebUtility.UrlEncode(item.Value)));

            try { return b.ToString().Substring(1); }
            catch (Exception e) 
            {
                Console.WriteLine(e);
                return ""; 
            }
        }

        private string BuildJSON(Dictionary<string, string> param)
        {
            if (param == null)
                return "";

            var entries = new List<string>();
            foreach (var item in param)
                entries.Add(string.Format("\"{0}\":\"{1}\"", item.Key, item.Value));

            return "{" + string.Join(",", entries) + "}";
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private long GetExpires()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 3600; // set expires one hour in the future
        }

        private string Query(string method, string function, Dictionary<string, string> param = null, bool auth = false, bool json = false)
        {
            string paramData = json ? BuildJSON(param) : BuildQueryData(param);
            string url = "/api/v1" + function + ((method == "GET" && paramData != "") ? "?" + paramData : "");
            string postData = (method != "GET") ? paramData : "";

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(domain + url);
            webRequest.Method = method;

            if (auth)
            {
                string expires = GetExpires().ToString();
                string message = method + url + expires + postData;
                byte[] signatureBytes = hmacsha256(Encoding.UTF8.GetBytes(apiSecret), Encoding.UTF8.GetBytes(message));
                string signatureString = ByteArrayToString(signatureBytes);

                webRequest.Headers.Add("api-expires", expires);
                webRequest.Headers.Add("api-key", apiKey);
                webRequest.Headers.Add("api-signature", signatureString);
            }

            try
            {
                if (postData != "")
                {
                    webRequest.ContentType = json ? "application/json" : "application/x-www-form-urlencoded";
                    var data = Encoding.UTF8.GetBytes(postData);
                    using (var stream = webRequest.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }

                bool flag = false;
                while (flag == false)
                {
                    try
                    {
                        using (var client = new WebClient())
                        using (client.OpenRead("http://google.com/generate_204"))
                            flag = true;
                    }
                    catch
                    {
                        
                        flag = false;
                    }
                }

                if(flag == true)
                {
                    using (WebResponse webResponse = webRequest.GetResponse())
                    using (Stream str = webResponse.GetResponseStream())
                    using (StreamReader sr = new StreamReader(str))
                    {
                        //Console.WriteLine(sr.ReadToEnd());
                        return sr.ReadToEnd().ToString();
                    }
                }
                else
                {
                    Settings.window.bitmex = new BitMexApi();
                    Settings.window.control.Initialize_Form();
                    return null;
                }
                
            }
            catch (WebException wex)
            {
                bool flag = false;
                while (flag == false)
                {
                    try
                    {
                        using (var client = new WebClient())
                        using (client.OpenRead("http://google.com/generate_204"))
                            flag = true;
                    }
                    catch
                    {
                        //Settings.window.bitmex = new BitMexApi();
                        //Settings.window.control.Initialize_Form();
                        flag = false;
                    }
                }

                using (HttpWebResponse response = (HttpWebResponse)wex.Response)
                {
                    if (response == null)
                        Thread.Sleep(10000);
                        throw;

                    using (Stream str = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(str))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }


        private byte[] hmacsha256(byte[] keyByte, byte[] messageBytes)
        {
            using (var hash = new HMACSHA256(keyByte))
            {
                return hash.ComputeHash(messageBytes);
            }
        }


        private long lastTicks = 0;
        private object thisLock = new object();

        private void RateLimit()
        {
            lock (thisLock)
            {
                long elapsedTicks = DateTime.Now.Ticks - lastTicks;
                var timespan = new TimeSpan(elapsedTicks);
                if (timespan.TotalMilliseconds < rateLimit)
                    Thread.Sleep(rateLimit - (int)timespan.TotalMilliseconds);
                lastTicks = DateTime.Now.Ticks;
            }
        }

        #endregion APIfunctions

        //requests active orders from bitmex and loads them in to list of order objects
        public List<Order> GetActivePositions()
        {
            var param = new Dictionary<string, string>();
            string json = Query("GET", "/position", param, true);
            dynamic results = JsonConvert.DeserializeObject<dynamic>(json);
            List<Order> list = new List<Order>();

            for (int i = 0; i < 10; i++)
            {
                if (i < results.Count)
                {
                    string val = Convert.ToString(results[i]);
                    JToken token = JObject.Parse(val);
                    Order item = new Order();

                   double test = double.Parse((string)token.SelectToken("currentQty"));
                    if (test != 0)
                    {
                        //item.OrderID = (int)token.SelectToken("orderID");
                        item.Symbol = (string)token.SelectToken("symbol");
                        item.Quantity = (int)token.SelectToken("currentQty");
                        item.BaughtAt = (float)token.SelectToken("breakEvenPrice");
                        item.LastPrice = (float)token.SelectToken("lastPrice");

                        float tempRoe = (float)token.SelectToken("unrealisedRoePcnt");
                        item.RoePercent = tempRoe * 100;
                    }

                    double temp = double.Parse((string)token.SelectToken("currentQty"));
                    if (temp < 0) { item.Type = "Sell"; }
                    else { item.Type = "Buy"; }

                    if (item.Quantity != 0) { list.Add(item); }
                }
            }
            return list;
        }

        public List<Order> GetCurrentStats(List<string> symbols)
        {
            List<Order> list = new List<Order>();

            for (int i = 0; i < symbols.Count; i++)
            {
                try
                {
                    //set query parameters
                    var param = new Dictionary<string, string>();
                    param["symbol"] = symbols[i];
                    //request and convert to json object
                    string json = Query("GET", "/instrument", param, true);
                    dynamic results = JsonConvert.DeserializeObject<dynamic>(json);
                    string val = Convert.ToString(results[0]);
                    JToken token = JObject.Parse(val);
                    //create object from request
                    Order item = new Order();
                    item.Symbol = (string)token.SelectToken("symbol");
                    item.LastPrice = (float)token.SelectToken("lastPrice");

                    //so many more things can be called

                    list.Add(item);

                }
                catch(Exception e)
                {
                    Thread.Sleep(30000);
                    Console.WriteLine("Sleeping in GetCurrentStats");
                    Settings.window.control.Initialize_Form();
                }
                           
            }

            return list;
        }
    }
}
