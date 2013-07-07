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
    }
}
