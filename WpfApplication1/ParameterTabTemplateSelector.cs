using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication1
{
    class ParameterTabTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ValueTemplate { get; set; }
        public DataTemplate BoolTemplate { get; set; }
        public DataTemplate GroupTemplate { get; set; }
        public DataTemplate Float2Template { get; set; }

        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var value = item as EditableValue;
            if (null != value)
            {
                if (value.Value is ObservableCollection<EditableValue>)
                {
                    return GroupTemplate;
                }
                else if (value.Value is float)
                {
                    return ValueTemplate;
                }
                else if (value.Value is bool)
                {
                    return BoolTemplate;
                }
                else if(value.Value is float[])
                {
                    return Float2Template;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
