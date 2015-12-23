using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfApplication1
{
    class RadToDegreeConverter : IValueConverter
    {
        #region IValueConverter メンバー

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var fValue = value as float?;
            if (null != fValue)
            {
                return fValue.Value * (float)(180.0 / Math.PI);
            }
            var dValue = value as double?;
            if (null != dValue)
            {
                return dValue.Value * 180.0 / Math.PI;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var fValue = value as float?;
            if (null != fValue)
            {
                return fValue.Value * (float)(Math.PI / 180.0);
            }
            var dValue = value as double?;
            if (null != dValue)
            {
                return dValue.Value * Math.PI / 180.0;
            }
            return Binding.DoNothing;
        }

        #endregion
    }
}
