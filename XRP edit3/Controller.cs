using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TradingPlatform
{
    public class Controller
    {
        Global Settings = Global.Instance;
        
        public bool initializing = true;

        public void Initialize_Form()
        {

            //initialize bitmex sign in and populate candle data
            Settings.window.uiTools.Load_Coin_List();
            Settings.window.bitmex.setBitmex(5000, Settings.window.user.ApiKey, Settings.window.user.ApiSecret);
            Settings.window.bitmex.GetCandleData(Settings.SymbolList);
            Settings.window.uiTools.UpdateBitmexData();

            //load all other elements before displaying graph
            Settings.window.uiTools.Load_Elements();
            Settings.window.uiTools.Load_Coin_Settings();
            Settings.window.uiTools.Show_Coin_Settings();
            Settings.window.uiTools.Get_Last_Candles();
            Settings.window.uiTools.Get_Current_Pricing();
           

            //load graphs
            Settings.window.uiTools.Populate_Candle_Data();
            Settings.window.uiTools.Load_MAIN_Chart();
            Settings.window.uiTools.Load_MACD_Chart();
            Settings.window.uiTools.Load_RSI_Chart();

            //update UI last
            Settings.window.uiTools.Update_Stats_UI();

            initializing = false;
        }
      
        public void Load_Data()
        {                        
            Settings.window.uiTools.Populate_Candle_Data();
            Settings.window.uiTools.Load_MAIN_Chart();
            Settings.window.uiTools.Load_MACD_Chart();
            Settings.window.uiTools.Load_RSI_Chart();

            Settings.window.uiTools.Update_Stats_UI();
            Settings.window.uiTools.Show_Coin_Settings();
        }

        public void Refresh_Data()
        {
            Settings.CandlesList.Clear();
            Settings.LastCandleList.Clear();

            Settings.window.bitmex.GetCandleData(Settings.SymbolList);
            Settings.window.uiTools.Get_Last_Candles();

            Load_Data();
        }

        public void Update_Pricing()
        {
            Settings.window.uiTools.Get_Current_Pricing();
            Settings.window.uiTools.Update_Stats_UI();
        }

        public void Refresh_Graphs()
        {           
            Settings.window.uiTools.Populate_Candle_Data_Single();
            Settings.window.uiTools.Load_MAIN_Chart();
            Settings.window.uiTools.Load_MACD_Chart();
            Settings.window.uiTools.Load_RSI_Chart();
            Settings.LastCandleList.Clear();
            Settings.window.uiTools.Get_Last_Candles();
            Settings.window.uiTools.Update_Stats_UI();
        }

        public void Update_Orders()
        {
            Settings.window.uiTools.UpdateBitmexData();
        }
    }
}
