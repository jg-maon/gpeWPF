using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication1
{
    class DataGridItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ValueTemplate { get; set; }

        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            if(item is EditableValue)
            { return ValueTemplate; }
            return base.SelectTemplate(item, container);
        }
    }
}
