using System.Collections.Generic;
using System.Linq;

namespace System.Windows.Controls.Samples.TreeMap
{
    public class BlogPost
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int Length { get; set; }
        public double Popularity { get; set; }
        public IEnumerable<string> Tags { get; set; }

        public string ShortTitle
        {
            get { return Title.Split('[').First(); }
        }
        public string FormattedDate
        {
            get { return Date.ToShortDateString(); }
        }
        public double FormattedPopularity
        {
            get { return Math.Round(Popularity, 2); }
        }
        public string FormattedTags
        {
            get { return "{" + string.Join(", ", Tags.ToArray()) + "}"; }
        }
    }

    public class BlogTag
    {
        public string Tag { get; set; }
        public IEnumerable<BlogPost> Posts { get; set; }
    }
}
