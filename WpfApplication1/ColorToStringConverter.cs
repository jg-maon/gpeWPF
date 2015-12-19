using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfApplication1
{
    class ColorToStringConverter : IValueConverter
    {
        #region IValueConverter メンバー

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var array = value as IEnumerable<byte>;
            if(null != array)
            {
                return "(" + string.Join(", ", array) + ")";
            }
            var fArray = value as IEnumerable<float>;
            if(null != fArray)
            {
                return "(" + string.Join(", ", fArray) + ")";
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var str = value as string;
            str = str.Replace(" ", "");
            if(str[0] != '(')
            {
                return Binding.DoNothing;
            }
            str = str.TrimStart('(');
            if(str[str.Length-1] != ')')
            {
                return Binding.DoNothing;
            }
            str = str.TrimEnd(')');

            var values = str.Split(',');

            try
            {
                if (typeof(TArrayValue<float>) == targetType)
                {
                    return Array.ConvertAll(values, (v) => float.Parse(v));
                }
                else if (typeof(TArrayValue<byte>) == targetType)
                {
                    return Array.ConvertAll(values, (v) => byte.Parse(v));
                }
            }
            catch (Exception) { return value; }
            return Binding.DoNothing;
        }

        #endregion
    }
}
