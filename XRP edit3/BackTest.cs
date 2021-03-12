using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPlatform
{
    class BackTest
    {
        Global settings = Global.Instance;
        Trading_Strategies Strategy = new Trading_Strategies();

        public void Start_Backtest(List<Candle> list, CoinSettings set)
        {
            Tools tool = new Tools();
            Trading_Strategies strat = new Trading_Strategies();

            //settings.window.BackTestCandleListbox.Items.Clear();
            //settings.window.profitLabel.Content = "Profit: 0";
            //settings.window.lossLabel.Content = "Loss: 0";


            tool.EMA_1(list, set.Ema1);
            tool.EMA_2(list, set.Ema2);
            tool.EMA_3(list, set.Ema3);
            tool.MACD(list);
            tool.Signal_Line(list);
            tool.Relative_Strength_Index(list);
            tool.CalcLast3Macd(list);
            tool.EmaDifference(list);
            tool.MacdDifference(list);
            tool.CheckTwoBeforeLast(list);
            tool.Single_Candle_Average(list);
            tool.GetOBVReading(list);
            tool.setStopLossLineForBuy(list);
            //tool.setStopLossLineForShort(list);

            settings.backtestOrder = new Order();

            settings.buyProfit = 0;
            settings.shortProfit = 0;
            settings.sellOrder = false;
            settings.buyOrder = false;
            settings.backTestHasBuy = false;

            CoinSettings coin = settings.coinSettingsList[settings.window.SymbolList.SelectedIndex];

            float prof = 0;
            float loss = 0;

            for (int i = 100; i < list.Count; i++)
            {
                settings.backtestOrder.Symbol = list[i].Symbol;
                settings.backtestOrder.LastPrice = list[i].Open;

                if (settings.backTestHasBuy == false)
                {
                    settings.backtestOrder.BaughtAt = list[i].Average;
                    
                    Strategy.Buy_Order(settings.backtestOrder, list[i], coin);

                    if (settings.buyOrder == true)
                    {
                        settings.backtestOrder.Type = "Buy";
                    }

                    if (settings.sellOrder == true)
                    {
                        settings.backtestOrder.Type = "Sell";
                    }
                }
                else
                {
                    strat.Check_Order(settings.backtestOrder, list[i], coin);

                    if (settings.backTestHasBuy == false)
                    {
                        if (settings.backtestOrder.Type == "Buy")
                        {
                            float temp = settings.backtestOrder.LastPrice;
                            temp = temp - list[i].Average;
                            settings.buyProfit = settings.buyProfit + temp;

                            if (temp > 0)
                            {
                                prof = prof + temp;
                                settings.window.profitLabel.Content = "Profit: " + prof;
                            }
                            else
                            {
                                loss = loss + temp;
                                settings.window.lossLabel.Content = "Loss: " + loss;
                            }


                            string item = settings.backtestOrder.Type + ", " + settings.buyProfit + ", Baught at: " + list[i].Average + ", Sold at: " + settings.backtestOrder.LastPrice;
                            settings.window.BackTestListBox.Items.Add(item);

                        }

                        if (settings.backtestOrder.Type == "Sell")
                        {
                            float temp = settings.backtestOrder.LastPrice;
                            temp = list[i].Average - temp;

                            if (temp > 0)
                            {
                                prof = prof + temp;
                                settings.window.profitLabel.Content = "Profit: " + prof;
                            }
                            else
                            {
                                loss = loss + temp;
                                settings.window.lossLabel.Content = "Loss: " + loss;
                            }

                            settings.shortProfit = settings.shortProfit + temp;

                            string item = settings.backtestOrder.Type + ", " + settings.shortProfit + ", Baught at: " + list[i].Average + ", Sold at: " + settings.backtestOrder.LastPrice;
                            settings.window.BackTestListBox.Items.Add(item);

                        }

                        //float total = float.Parse(settings.window.profitLabel.Content.ToString());
                        //total = total - float.Parse(settings.window.lossLabel.Content.ToString());
                        //settings.window.profitLabel.Content = total;
                        settings.backtestOrder = new Order();
                    }
                }

            }
        }
    }
}
