using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TradingPlatform
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Global Settings = Global.Instance;//hold all global variables
        public UI_Tools uiTools = new UI_Tools();//UI tools hold all tools for the ui  
        public Controller control = new Controller();//controller holds all functions
        public Algorithm algo = new Algorithm();
        public Account user = new Account();
        public BitMexApi bitmex = new BitMexApi();
        
        //graph declares
        public SeriesCollection SeriesCollection { get; set; }
        public SeriesCollection MACDSeriesCollection { get; set; }
        public SeriesCollection RSISeriesCollection { get; set; }

        private System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        private System.Windows.Threading.DispatcherTimer timeOutCounter = new System.Windows.Threading.DispatcherTimer();

        private int counter = 1;

        public MainWindow()
        {
            InitializeComponent();
            Settings.activeOrdersList.Clear();
            Settings.CandlesList.Clear();
            Settings.coinSettingsList.Clear();
            Settings.CurrentStats.Clear();
            Settings.SymbolList.Clear();

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            Settings.window = this;
            control.Initialize_Form();
            DataContext = this;

            WebSocketOrganiser weby = new WebSocketOrganiser();

            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Start();
        }

        object temp;
        EventArgs temp1;

        public void loadTimer()
        {
            dispatcherTimer_Tick(temp, temp1);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            temp = sender;
            temp1 = e;

            if (Settings.timeSetter == true)
            {
                //if (SellActiveOrderCheckBox.IsChecked == true)
                //{
                //    settings.onlySellActive = true;
                //}
                //else
                //{
                //    settings.onlySellActive = false;
                //}

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

                dispatcherTimer.Interval = TimeSpan.FromSeconds(3);

                if (counter == 3)
                {
                    DataContext = null;
                    control.Refresh_Data();                 
                    DataContext = this;
                    counter = 0;
                }

                if(counter > 1)
                {
                    control.Update_Pricing();
                    Settings.window.uiTools.UpdateBitmexData();
                    algo.Trade_Brain();
                }
                

                counter++;
            }
        }

        #region Buttons

        private void SymbolList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
            uiTools.Load_Coin_Image(SymbolList.SelectedItem.ToString());
            settingsCoinLabel.Content = SymbolList.SelectedItem.ToString();


            if (Settings.window.control.initializing == false)
            {
                DataContext = null;
                control.Load_Data();
                DataContext = this;
            }    
        }

        private void SaveCoinSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Settings.window.uiTools.Save_Coin_Settings();
            this.DataContext = null;
            Settings.window.control.Refresh_Graphs();
            DataContext = this;
        }

        private void StopStartButton_Click(object sender, RoutedEventArgs e)
        {
            if (StopStartButton.Background == Brushes.Red)
            {
                StopStartButton.Background = Brushes.Green;
                StopStartButton.Content = "START";
                Settings.timeSetter = false;
            }         
            else if (StopStartButton.Background == Brushes.Green)
            {
                StopStartButton.Background = Brushes.Red;
                StopStartButton.Content = "STOP";
                dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
                Settings.timeSetter = true;
            }
        }


        #endregion

        private void BackTestButton_Click(object sender, RoutedEventArgs e)
        {
            string symbol = SymbolList.SelectedItem.ToString();

            if (Settings.TestingList.Count == 0 || Settings.TestingList[0].Symbol != symbol)
            {
                Settings.TestingList.Clear();
                bitmex.GetCandleHistory(symbol);
            }

            Settings.BackTestActive = true;
            BackTest test = new BackTest();

            CoinSettings temp = new CoinSettings();
            temp.Ema1 = Int32.Parse(Ema1Textbox.Text);
            temp.Ema2 = Int32.Parse(Ema2Textbox.Text);
            temp.Ema3 = Int32.Parse(Ema3Textbox.Text);
            temp.RsiHigh = double.Parse(RsiHigh.Text);
            temp.RsiLow = double.Parse(RsiLow.Text);
            temp.LossPercent = double.Parse(lossPercentBox.Text);
            temp.ProfitPercent = double.Parse(profitPercentBox.Text);
            temp.MACDthreshold = double.Parse(MacdThresholdBox.Text);
            temp.EMAThreshold = double.Parse(EmaThresholdBox.Text);



            test.Start_Backtest(Settings.TestingList, temp);
            Settings.BackTestActive = false;
        }
    }
}
