using System.Collections.Generic;

namespace System.Windows.Controls.Samples.DataVisualisation
{
    public class SampleDataVM
    {
        public Dictionary<string,int>  Data { get; private set; }

        public SampleDataVM()
        {
            Data = new Dictionary<string, int>();

            Data.Add("aaaa", 10);
            Data.Add("bbbb", 60);
            Data.Add("cccc", 30);
            Data.Add("dddd", 40);
            Data.Add("eeee", 50);
            Data.Add("ffff", 80);
            Data.Add("gggg", 20);
        }

        public ObjectCollection Microsoft
        {
            get
            {
                ObjectCollection financialData = new ObjectCollection();
                financialData.Add(new FinancialData { Date = new DateTime(2009, 2, 13), Open = 19.27, High = 19.47, Low = 19.04, Close = 19.09 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 2, 12), Open = 18.97, High = 19.32, Low = 18.54, Close = 19.26 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 2, 11), Open = 18.94, High = 19.49, Low = 18.92, Close = 19.21 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 2, 10), Open = 19.25, High = 19.8, Low = 18.7, Close = 18.8 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 2, 9), Open = 19.64, High = 19.77, Low = 19.26, Close = 19.44 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 2, 6), Open = 19.16, High = 19.93, Low = 19.06, Close = 19.66 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 2, 5), Open = 18.51, High = 19.14, Low = 18.25, Close = 19.04 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 2, 4), Open = 18.54, High = 19, Low = 18.5, Close = 18.63 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 2, 3), Open = 17.85, High = 18.61, Low = 17.6, Close = 18.5 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 2, 2), Open = 17.03, High = 18.13, Low = 17, Close = 17.83 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 1, 30), Open = 17.74, High = 17.79, Low = 17.1, Close = 17.1 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 1, 29), Open = 17.78, High = 17.96, Low = 17.56, Close = 17.59 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 1, 28), Open = 17.8, High = 18.31, Low = 17.76, Close = 18.04 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 1, 27), Open = 17.78, High = 17.97, Low = 17.43, Close = 17.66 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 1, 26), Open = 17.29, High = 17.81, Low = 17.23, Close = 17.63 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 1, 23), Open = 16.97, High = 17.49, Low = 16.75, Close = 17.2 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 1, 22), Open = 18.05, High = 18.18, Low = 17.07, Close = 17.11 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 1, 21), Open = 18.87, High = 19.45, Low = 18.46, Close = 19.38 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 1, 20), Open = 19.46, High = 19.62, Low = 18.37, Close = 18.48 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 1, 16), Open = 19.63, High = 19.91, Low = 19.15, Close = 19.71 });
                financialData.Add(new FinancialData { Date = new DateTime(2009, 1, 15), Open = 19.07, High = 19.3, Low = 18.52, Close = 19.24 });
                return financialData;
            }
        }
        public class FinancialData
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Volume { get; set; }
        
        public FinancialData()
        { }      
    }

    }
}
