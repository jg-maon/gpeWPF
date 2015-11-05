using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication1
{
    class ParameterTabContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ClassTemplate { get; set; }
        public DataTemplate EditableValueTemplate { get; set; }
        public DataTemplate BooleanTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var node = item as ParameterNode;
            if (node != null)
            {
                if (node.Object is float)
                { return EditableValueTemplate; }
                if(node.Object is string)
                { return EditableValueTemplate; }
                if (node.Object is bool)
                { return BooleanTemplate; }
                else if (node.Object != null)
                { return ClassTemplate; }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
