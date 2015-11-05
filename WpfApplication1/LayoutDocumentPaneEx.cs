using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.AvalonDock.Layout;

namespace WpfApplication1
{
    class LayoutDocumentPaneEx : LayoutDocumentPane
    {
        public string Name { get; set; }
        
        public LayoutDocumentPaneEx():base(){}
        public LayoutDocumentPaneEx(LayoutContent firstChild) : base(firstChild) { }
    }
}
