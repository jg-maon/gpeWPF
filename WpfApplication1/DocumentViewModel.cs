using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    abstract class DocumentViewModel : PaneViewModel
    {
        public DocumentViewModel(string title):
            base(title)
        { }

    }
}
