using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPlatform
{
    public class Algorithm
    {
        Global Settings = Global.Instance;
        Trading_Strategies Strategy = new Trading_Strategies();

        public bool hasBought = false;

        public bool hasGotBTC = false;
        public bool hasGotXRP = false;
        public bool hasGotBCH = false;
        public bool hasGotETH = false;
        public bool hasGotADAH = false;
        public bool hasGotBNBU = false;
        public bool hasGotDODGE = false;
        public bool hasGotDOT = false;
        public bool hasGotEOSH21 = false;
        public bool hasGotLINK = false;
        public bool hasGotLTC = false;
        public bool hasGotTRX = false;

        public int BTCtimer = 0;
        public int XRPtimer = 0;
        public int BCHtimer = 0;
        public int ETHtimer = 0;
        public int ADAHtimer = 0;
        public int BNBUtimer = 0;
        public int DODGEtimer = 0;
        public int DOTtimer = 0;
        public int EOSH21timer = 0;
        public int LINKtimer = 0;
        public int LTCtimer = 0;
        public int TRXtimer = 0;

        public int defaultWaitTimer = 5;
        public int counter = 0;

        public void Trade_Brain()
        {
            start();
        }

        private void start()
        {
            hasGotBTC = false;
            hasGotXRP = false;
            hasGotBCH = false;
            hasGotETH = false;
            hasGotADAH = false;
            hasGotBNBU = false;
            hasGotDODGE = false;
            hasGotEOSH21 = false;
            hasGotDOT = false;
            hasGotLINK = false;
            hasGotLTC = false;
            hasGotTRX = false;

            //if there are no bids then prepare to buy an order
            if (Settings.activeOrdersList.Count != 0)
            {
                for (int i = 0; i < Settings.activeOrdersList.Count; i++)
                {
                    for (int j = 0; j < Settings.CurrentStats.Count; j++)
                    {
                        if (Settings.activeOrdersList[i].Symbol == Settings.CurrentStats[j].Symbol)
                        {
                            switch (Settings.activeOrdersList[i].Symbol)
                            {
                                case "XRPUSD":
                                    hasGotBTC = true;
                                    hasBought = true;
                                    BTCtimer = defaultWaitTimer;
                                    Strategy.Check_Order(Settings.activeOrdersList[i], Settings.LastCandleList[j], Settings.coinSettingsList[1]);
                                    break;
                                //case "XRPUSD":
                                //    hasGotXRP = true;
                                //    hasBought = true;
                                //    XRPtimer = defaultWaitTimer;
                                //    Strategy.Check_Order(Settings.activeOrdersList[i], Settings.LastCandleList[j], Settings.coinSettingsList[1]);
                                //    break;
                                case "BCHUSD":
                                    hasGotBCH = true;
                                    hasBought = true;
                                    BCHtimer = defaultWaitTimer;
                                    Strategy.Check_Order(Settings.activeOrdersList[i], Settings.LastCandleList[j], Settings.coinSettingsList[2]);
                                    break;
                                case "ETHUSD":
                                    hasGotETH = true;
                                    hasBought = true;
                                    ETHtimer = defaultWaitTimer;
                                    Strategy.Check_Order(Settings.activeOrdersList[i], Settings.LastCandleList[j], Settings.coinSettingsList[3]);
                                    break;
                                case "ADAH21":
                                    hasGotADAH = true;
                                    hasBought = true;
                                    ADAHtimer = defaultWaitTimer;
                                    Strategy.Check_Order(Settings.activeOrdersList[i], Settings.LastCandleList[j], Settings.coinSettingsList[4]);
                                    break;
                                case "DOGEUSDT":
                                    hasGotDODGE = true;
                                    hasBought = true;
                                    DODGEtimer = defaultWaitTimer;
                                    Strategy.Check_Order(Settings.activeOrdersList[i], Settings.LastCandleList[j], Settings.coinSettingsList[5]);
                                    break;
                                case "EOSH21":
                                    hasGotEOSH21 = true;
                                    hasBought = true;
                                    EOSH21timer = defaultWaitTimer;
                                    Strategy.Check_Order(Settings.activeOrdersList[i], Settings.LastCandleList[j], Settings.coinSettingsList[6]);
                                    break;
                                case "LINKUSDT":
                                    hasGotLINK = true;
                                    hasBought = true;
                                    LINKtimer = defaultWaitTimer;
                                    Strategy.Check_Order(Settings.activeOrdersList[i], Settings.LastCandleList[j], Settings.coinSettingsList[7]);
                                    break;
                                case "LTCUSD":
                                    hasGotLTC = true;
                                    hasBought = true;
                                    LTCtimer = defaultWaitTimer;
                                    Strategy.Check_Order(Settings.activeOrdersList[i], Settings.LastCandleList[j], Settings.coinSettingsList[8]);
                                    break;
                                case "TRXH21":
                                    hasGotTRX = true;
                                    hasBought = true;
                                    TRXtimer = defaultWaitTimer;
                                    Strategy.Check_Order(Settings.activeOrdersList[i], Settings.LastCandleList[j], Settings.coinSettingsList[9]);
                                    break;
                            }
                        }
                    }
                }
            }

            //check each timer
            if (Settings.timeSetter == true)
            {
                counter++;
                if (counter == 3)
                {
                    Timers();
                    counter = 0;
                }
            }
            else
            {
                Timers();
            }


            if (hasBought == false)
            {
                Check_If_Buy();
            }

        }

        public void setTimer(Order order)
        {
            if (order.Symbol == "XRPUSD")
            {
                BTCtimer = defaultWaitTimer;
            }

            //if (order.Symbol == "XRPUSD")
            //{
            //    XRPtimer = defaultWaitTimer;
            //}

            if (order.Symbol == "BCHUSD")
            {
                BCHtimer = defaultWaitTimer;
            }

            if (order.Symbol == "ETHUSD")
            {
                ETHtimer = defaultWaitTimer;
            }

            if (order.Symbol == "ADAH21")
            {
                ADAHtimer = defaultWaitTimer;
            }

            if (order.Symbol == "BNBUSDTH21")
            {
                BNBUtimer = defaultWaitTimer;
            }

            if (order.Symbol == "DOGEUSDT")
            {
                DODGEtimer = defaultWaitTimer;
            }

            if (order.Symbol == "DOTUSDTH21")
            {
                DOTtimer = defaultWaitTimer;
            }

            if (order.Symbol == "EOSH21")
            {
                EOSH21timer = defaultWaitTimer;
            }

            if (order.Symbol == "LINKUSDT")
            {
                LINKtimer = defaultWaitTimer;
            }

            if (order.Symbol == "LTCUSD")
            {
                LTCtimer = defaultWaitTimer;
            }

            if (order.Symbol == "TRXH21")
            {
                TRXtimer = defaultWaitTimer;
            }
        }

        private void Timers()
        {
            if (BTCtimer != 0)
            {
                BTCtimer--;
            }

            if (XRPtimer != 0)
            {
                XRPtimer--;
            }

            if (BCHtimer != 0)
            {
                BCHtimer--;
            }

            if (ETHtimer != 0)
            {
                ETHtimer--;
            }

            if (ADAHtimer != 0)
            {
                ADAHtimer--;
            }

            if (BNBUtimer != 0)
            {
                BNBUtimer--;
            }

            if (DODGEtimer != 0)
            {
                DODGEtimer--;
            }

            if (DOTtimer != 0)
            {
                DOTtimer--;
            }

            if (EOSH21timer != 0)
            {
                EOSH21timer--;
            }

            if (LINKtimer != 0)
            {
                LINKtimer--;
            }

            if (LTCtimer != 0)
            {
                LTCtimer--;
            }

            if (TRXtimer != 0)
            {
                TRXtimer--;
            }
        }

        private void Check_If_Buy()
        {
            if (hasGotBTC == false && hasBought == false)
            {
                if (Settings.LastCandleList[0].Last3Macd == false && BTCtimer == 0)
                {
                    Strategy.Buy_Order(Settings.CurrentStats[0], Settings.LastCandleList[0], Settings.coinSettingsList[0]);
                    //Buy_Order(LastCandlesList[0]);
                }
            }

            //if (hasGotXRP == false)
            //{
            //    if (Settings.LastCandleList[1].Last3Macd == false && XRPtimer == 0)
            //    {
            //        Strategy.Buy_Order(Settings.CurrentStats[1], Settings.LastCandleList[1], Settings.coinSettingsList[1]);
            //        //Buy_Order(LastCandlesList[1]);
            //    }
            //}

            //if (hasGotBCH == false)
            //{
            //    if (Settings.LastCandleList[2].Last3Macd == false && BCHtimer == 0)
            //    {
            //        Strategy.Buy_Order(Settings.CurrentStats[2], Settings.LastCandleList[2], Settings.coinSettingsList[2]);
            //        //Buy_Order(LastCandlesList[2]);
            //    }
            //}

            //if (hasGotETH == false)
            //{
            //    if (Settings.LastCandleList[3].Last3Macd == false && ETHtimer == 0)
            //    {
            //        Strategy.Buy_Order(Settings.CurrentStats[3], Settings.LastCandleList[3], Settings.coinSettingsList[3]);
            //        //Buy_Order(LastCandlesList[3]);
            //    }
            //}

            //if (hasGotADAH == false)
            //{
            //    if (Settings.LastCandleList[4].Last3Macd == false && ADAHtimer == 0)
            //    {
            //        Strategy.Buy_Order(Settings.CurrentStats[4], Settings.LastCandleList[4], Settings.coinSettingsList[4]);
            //        //Buy_Order(LastCandlesList[4]);
            //    }
            //}

            //if (hasGotDODGE == false)
            //{
            //    if (Settings.LastCandleList[5].Last3Macd == false && DODGEtimer == 0)
            //    {
            //        Strategy.Buy_Order(Settings.CurrentStats[5], Settings.LastCandleList[5], Settings.coinSettingsList[5]);
            //        //Buy_Order(LastCandlesList[6]);
            //    }
            //}


            //if (hasGotEOSH21 == false)
            //{
            //    if (Settings.LastCandleList[6].Last3Macd == false && EOSH21timer == 0)
            //    {
            //        Strategy.Buy_Order(Settings.CurrentStats[6], Settings.LastCandleList[6], Settings.coinSettingsList[6]);
            //        //Buy_Order(LastCandlesList[8]);
            //    }
            //}

            //if (hasGotLINK == false)
            //{
            //    if (Settings.LastCandleList[7].Last3Macd == false && LINKtimer == 0)
            //    {
            //        Strategy.Buy_Order(Settings.CurrentStats[7], Settings.LastCandleList[7], Settings.coinSettingsList[7]);
            //        //Buy_Order(LastCandlesList[9]);
            //    }
            //}

            //if (hasGotLTC == false)
            //{
            //    if (Settings.LastCandleList[8].Last3Macd == false && LTCtimer == 0)
            //    {
            //        Strategy.Buy_Order(Settings.CurrentStats[8], Settings.LastCandleList[8], Settings.coinSettingsList[8]);
            //        //Buy_Order(LastCandlesList[10]);
            //    }
            //}

            //if (hasGotTRX == false)
            //{
            //    if (Settings.LastCandleList[9].Last3Macd == false && TRXtimer == 0)
            //    {
            //        Strategy.Buy_Order(Settings.CurrentStats[9], Settings.LastCandleList[9], Settings.coinSettingsList[9]);
            //        //Buy_Order(LastCandlesList[11]);
            //    }
            //}
        }
    }
}
