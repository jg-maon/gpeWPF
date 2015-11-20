using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.AvalonDock.Themes;

namespace WpfApplication1
{
    class DarkTheme : ExpressionDarkTheme
    {
        public override Uri GetResourceUri()
        {
            return new Uri(
                "/WpfApplication1;component/DarkTheme.xaml",UriKind.Relative);
        }
    }
}
