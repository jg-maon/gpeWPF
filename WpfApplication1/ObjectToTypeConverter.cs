using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfApplication1
{
    class ObjectToTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            float? f = value as float?;
            if(f != null)
            {
                return f.Value;
            }
            int? i = value as int?;
            if(i != null)
            {
                return i.Value;
            }
            Type type = value.GetType();
            
            return System.Convert.ChangeType(value, type);
            
            //return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
