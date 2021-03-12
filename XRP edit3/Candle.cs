using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPlatform
{
    class Candle
    {
        #region Variables

        private string symbol;
        private int id;
        private string timestamp;
        private float open;
        private float close;
        private float high;
        private float low;
        private float volume;
        private float rsi;
        private float ema1;
        private float ema2;
        private float ema3;
        private float macd;
        private float average;
        private float macdSignal;
        private float emaDifference;
        private float macdDifference;
        private bool last3Macd; //if last 3 macd signals are in same range under 20 then dont buy
        private Candle oneb4last;
        private float obv = 0; //on balance volume - price prediction
        private double stopLoss = 0;
        private double profitTrigger = 0;

        #endregion Variables

        #region GetSets
        public string Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }
        public float Open
        {
            get { return open; }
            set { open = value; }
        }
        public float Close
        {
            get { return close; }
            set { close = value; }
        }
        public float High
        {
            get { return high; }
            set { high = value; }
        }
        public float Low
        {
            get { return low; }
            set { low = value; }
        }
        public float Volume
        {
            get { return volume; }
            set { volume = value; }
        }
        public float Rsi
        {
            get { return rsi; }
            set { rsi = value; }
        }
        public float Ema1
        {
            get { return ema1; }
            set { ema1 = value; }
        }
        public float Ema2
        {
            get { return ema2; }
            set { ema2 = value; }
        }
        public float Ema3
        {
            get { return ema3; }
            set { ema3 = value; }
        }
        public float Macd
        {
            get { return macd; }
            set { macd = value; }
        }
        public float Average
        {
            get { return average; }
            set { average = value; }
        }
        public float MacdSignal
        {
            get { return macdSignal; }
            set { macdSignal = value; }
        }
        public bool Last3Macd
        {
            get { return last3Macd; }
            set { last3Macd = value; }
        }
        public float EmaDifference
        {
            get { return emaDifference; }
            set { emaDifference = value; }
        }
        public float MacdDifference
        {
            get { return macdDifference; }
            set { macdDifference = value; }
        }

        public Candle Oneb4Last
        {
            get { return oneb4last; }
            set { oneb4last = value; }
        }

        public float Obv
        {
            get { return obv; }
            set { obv = value; }
        }

        public double Stoploss
        {
            get { return stopLoss; }
            set { stopLoss = value; }
        }

        public double ProfitTrigger
        {
            get { return profitTrigger; }
            set { profitTrigger = value; }
        }

        #endregion GetSets
    }
}
