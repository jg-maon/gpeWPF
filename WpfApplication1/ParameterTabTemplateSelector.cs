using System;
using System.Collections;
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
            var value = item as IEditableValue;
            if (null != value)
            {
                if (value.Value is ObservableCollection<IEditableValue>)
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
                else if(value.Value is float[] || value.Value is IEnumerable || value.Value is TArrayValue<object>)
                {
                    return Float2Template;
                }
                else if(value.Value is TArrayValue<float>)
                {
                    return Float2Template;
                }
                else if(value.Value is TUnitValue<float>)
                {
                    return Float2Template;
                }
                else if (value.Value is TUnitValue<TArrayValue<float>>)
                {
                    return Float2Template;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
