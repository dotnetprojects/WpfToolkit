using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace System.Windows.Controls.Samples.TreeMap
{
    public partial class TreeMapIntroduction : UserControl
    {
        public TreeMapIntroduction()
        {
            InitializeComponent();

            IEnumerable<BlogPost> blogPosts;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(
                "System.Windows.Controls.Samples.BlogPostData.xml"
                ))
            {
                using (var reader = XmlReader.Create(stream))
                {
                    var doc = XDocument.Load(reader);
                    blogPosts = doc
                        .Element("Posts")
                        .Elements("Post")
                        .Select(p => new BlogPost
                        {
                            Title = p.Element("Title").Value,
                            Date = DateTime.Parse(p.Element("Date").Value, CultureInfo.InvariantCulture),
                            Tags = p.Element("Tags").Elements("Tag").Select(t => t.Value),
                            Length = int.Parse(p.Element("Length").Value, CultureInfo.InvariantCulture),
                            Popularity = double.Parse(p.Element("Popularity").Value, CultureInfo.InvariantCulture),
                        });
                }
            }

            var blogPostsByTag = blogPosts
                .SelectMany(p => p.Tags.Select(t => new { Tag = t, Post = p }))
                .GroupBy(p => p.Tag)
                .Select(g => new BlogTag { Tag = g.Key, Posts = g.Select(p => p.Post).ToArray() });

            AllPosts.DataContext = blogPosts;
            RecentPosts.DataContext = blogPosts.Reverse().Take(10).Reverse();
            ByTag.DataContext = blogPostsByTag;
            ByTagDetailed.DataContext = blogPostsByTag;
            RecentPosts2.DataContext = blogPosts.Reverse().Take(10).Reverse();
        }
    }

    public class DockPanel : System.Windows.Controls.DockPanel
    {
    }

#if SILVERLIGHT
    // Silverlight's Viewbox is sealed; simulate it with a ContentControl wrapper
    public class Viewbox : ContentControl
    {
        public Viewbox()
        {
            Template = (ControlTemplate)XamlReader.Load(@"
                <ControlTemplate
                    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" " +
#if !NO_CORE_VIEWBOX
                    @">
                    <Viewbox>
                        <ContentPresenter/>
                    </Viewbox>" +
#else
                    @"xmlns:controls=""clr-namespace:System.Windows.Controls;assembly=System.Windows.Controls.Toolkit"">
                    <controls:Viewbox>
                        <ContentPresenter/>
                    </controls:Viewbox>" +
#endif
                @"</ControlTemplate>");
        }
    }
#else
    public class Viewbox : System.Windows.Controls.Viewbox
    {
    }
#endif
}
