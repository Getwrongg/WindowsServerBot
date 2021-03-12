using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPlatform
{

    class Global
    {
        //GLOBAL VARIABLES
        public MainWindow window;

        public List<List<Candle>> CandlesList = new List<List<Candle>>();
        public List<Order> activeOrdersList = new List<Order>();
        public List<Order> CurrentStats = new List<Order>();
        public List<Candle> LastCandleList = new List<Candle>();
        public List<Candle> TestingList = new List<Candle>();

        
        

        //BITMEX SETTINGS
        public int candleCount = 500;
        public string candleBinSize = "5m";
        public double currentPrice = 0;

        //UI SETTINGS
        public List<string> SymbolList = new List<string>();


        //controller variables
        public List<CoinSettings> coinSettingsList = new List<CoinSettings>();

        public bool timeSetter = false;
        public bool onlySellActive = false;


        //back test
        //back testings variabels
        public float profit = 0;
        public float loss = 0;
        public float buyProfit = 0;
        public float shortProfit = 0;
        public float buyPrice = 0;
        public bool backTestHasBuy = false;
        public Order backtestOrder;
        public bool buyOrder = false;
        public bool sellOrder = false;
        public bool testing = false;
        public bool BackTestActive = false;
        public double btcCounter = 0;        

        #region CLASSSETTING

        private static Global instance = null;
        private static readonly object padlock = new object();
        public static Global Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Global();
                    }
                    return instance;
                }
            }
        }

        #endregion CLASSSETTING


    }
}
