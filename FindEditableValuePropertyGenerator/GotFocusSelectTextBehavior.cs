using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FindEditableValuePropertyGenerator
{
    class GotFocusSelectTextBehavior
    {


        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(GotFocusSelectTextBehavior), new PropertyMetadata(false, _OnEnableChanged));

        private static void _OnEnableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if(null != element)
            {
                element.GotFocus -= _OnGotFocus;
                if((bool)e.NewValue)
                {       
                    element.GotFocus += _OnGotFocus;
                }
            }
        }

        static void _OnGotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (null != textBox)
            {
                textBox.SelectAll();
            }
        }


    }
}
