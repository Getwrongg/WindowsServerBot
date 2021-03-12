using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPlatform
{
    public class CoinSettings
    {
        private string symbol;
        private int ema1;
        private int ema2;
        private int ema3;
        private double profitPercent;
        private double lossPercent;
        private double rsiHigh;
        private double rsiLow;
        private bool trailingProfit;
        private double ProfitThreshold;
        private double macdThreshold;
        private double emaThreshold;
        private double tradeWith;
        private double activeOrderPrice;
        private bool trailingProfitTrigger = false;


        //profit trail
        double tempStopLossPercent;
        double tempStopProfitPercent;
        double savedBuyPrice;
        

        public string Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }

        public int Ema1
        {
            get { return ema1; }
            set { ema1 = value; }
        }

        public int Ema2
        {
            get { return ema2; }
            set { ema2 = value; }
        } 

        public int Ema3
        {
            get { return ema3; }
            set { ema3 = value; }
        }

        public double ProfitPercent
        {
            get { return profitPercent; }
            set { profitPercent = value; }
        }

        public double LossPercent
        {
            get { return lossPercent; }
            set { lossPercent = value; }
        }
        public double RsiHigh
        {
            get { return rsiHigh; }
            set { rsiHigh = value; }
        }
        public double RsiLow
        {
            get { return rsiLow; }
            set { rsiLow = value; }
        }

        public bool TrailingCheck
        {
            get { return trailingProfit; }
            set { trailingProfit = value; }
        }

        public double TPThreshold
        {
            get { return ProfitThreshold; }
            set { ProfitThreshold = value; }
        }

        public double TradeWith
        {
            get { return tradeWith; }
            set { tradeWith = value; }
        }

        public double ActiveOrderPrice
        {
            get { return activeOrderPrice; }
            set { activeOrderPrice = value; }
        }

        public bool TrailingProfitTrigger
        {
            get { return trailingProfitTrigger; }
            set { trailingProfitTrigger = value; }
        }

        public double MACDthreshold
        {
            get { return macdThreshold; }
            set { macdThreshold = value; }
        }

        public double EMAThreshold
        {
            get { return emaThreshold; }
            set { emaThreshold = value; }
        }


        public double TempStopLossPercent
        {
            get { return tempStopLossPercent; }
            set { tempStopLossPercent = value; }
        }
        public double TempStopProfitPercent
        {
            get { return tempStopProfitPercent; }
            set { tempStopProfitPercent = value; }
        }
        public double SavedBuyPrice
        {
            get { return savedBuyPrice; }
            set { savedBuyPrice = value; }
        }
    }
}
