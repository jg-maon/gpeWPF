using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication1
{
    class IdInfoTableTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LightSetTemplate { get; set; }
        public DataTemplate FogTemplate { get; set; }

        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var tabPage = item as IdInfoTableTabPageViewModel;
            if(null != tabPage)
            {
                var category = tabPage.CategoryName;
                switch(category)
                {
                    case "LightSet":
                        return LightSetTemplate;
                    case "Fog":
                        return FogTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
