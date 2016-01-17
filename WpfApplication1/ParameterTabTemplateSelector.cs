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
        public DataTemplate BoolTemplate { get; set; }
        public DataTemplate GroupTemplate { get; set; }

        public DataTemplate ValueTemplate { get; set; }
        public DataTemplate Float2Template { get; set; }
        public DataTemplate Float3Template { get; set; }
        public DataTemplate Float4Template { get; set; }
        public DataTemplate ColorTemplate { get; set; }
        public DataTemplate StringTemplate { get; set; }

        public DataTemplate ComboBoxTemplate { get; set; }
        /*
        public DataTemplate SliderValueTemplate { get; set; }
        public DataTemplate SliderFloat2Template { get; set; }
        public DataTemplate SliderFloat3Template { get; set; }
        public DataTemplate SliderFloat4Template { get; set; }
        public DataTemplate SliderColorTemplate { get; set; }
        //*/
        public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            var value = item as IEditableValue;
            if (null != value)
            {
                if(value is ComboBoxValue)
                {
                    return ComboBoxTemplate;
                }
                //bool isSliderValue = value is ISliderValue;
                switch(value.EditorType)
                {
                    case (int)GX_META_INFO_TYPE.INT8:
                    case (int)GX_META_INFO_TYPE.INT16:
                    case (int)GX_META_INFO_TYPE.INT32:
                    case (int)GX_META_INFO_TYPE.INT64:
                    case (int)GX_META_INFO_TYPE.UINT8:
                    case (int)GX_META_INFO_TYPE.UINT16:
                    case (int)GX_META_INFO_TYPE.UINT32:
                    case (int)GX_META_INFO_TYPE.UINT64:
                    case (int)GX_META_INFO_TYPE.FLOAT32:
                    case (int)GX_META_INFO_TYPE.FLOAT64:
                        return /*(isSliderValue) ? SliderValueTemplate :*/ ValueTemplate;
                    case (int)GX_META_INFO_TYPE.BOOL:
                        return BoolTemplate;
                    case (int)GX_META_INFO_TYPE.VECTOR2AL:
                        return Float2Template;
                    case (int)GX_META_INFO_TYPE.VECTOR3AL:
                        return Float3Template;
                    case (int)GX_META_INFO_TYPE.VECTOR4AL:
                        return Float4Template;
                    case (int)GX_META_INFO_TYPE.COLOR32:
                        return ColorTemplate;
                    case (int)GX_META_INFO_TYPE.STRINGW:
                        return StringTemplate;
                
                }

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
                else if(value.Value is TUnitEditableValue<float>)
                {
                    return Float2Template;
                }
                else if (value.Value is TUnitEditableValue<TArrayValue<float>>)
                {
                    return Float2Template;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
