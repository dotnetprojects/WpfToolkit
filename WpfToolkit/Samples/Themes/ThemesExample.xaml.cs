using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.Themes;

namespace System.Windows.Controls.Samples.Themes
{
    /// <summary>
    /// Interaction logic for ThemesExample.xaml
    /// </summary>
    public partial class ThemesExample : UserControl
    {
        public ThemesExample()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                string theme = e.AddedItems[0].ToString();

                // Application Level
                Application.Current.ApplyTheme(theme);
            }
        }
    }
}
