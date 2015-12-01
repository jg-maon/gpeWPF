using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApplication1
{
    public class LayoutItemBehavior
    {


        public static double GetFloatingWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(FloatingWidthProperty);
        }

        public static void SetFloatingWidth(DependencyObject obj, double value)
        {
            obj.SetValue(FloatingWidthProperty, value);
        }

        // Using a DependencyProperty as the backing store for FloatingWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FloatingWidthProperty =
            DependencyProperty.RegisterAttached("FloatingWidth", typeof(double), typeof(LayoutItemBehavior), new PropertyMetadata(100.0, _OnFloatingWidthChanged));

        private static void _OnFloatingWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var layoutItem = d as Xceed.Wpf.AvalonDock.Controls.LayoutItem;
            if(null == layoutItem)
            { return; }
            layoutItem.LayoutElement.FloatingWidth = (double)e.NewValue;
        }



        public static double GetFloatingHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(FloatingHeightProperty);
        }

        public static void SetFloatingHeight(DependencyObject obj, double value)
        {
            obj.SetValue(FloatingHeightProperty, value);
        }

        // Using a DependencyProperty as the backing store for FloatingHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FloatingHeightProperty =
            DependencyProperty.RegisterAttached("FloatingHeight", typeof(double), typeof(LayoutItemBehavior), new PropertyMetadata(100.0, _OnFloatingHeightChanged));

        private static void _OnFloatingHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var layoutItem = d as Xceed.Wpf.AvalonDock.Controls.LayoutItem;
            if (null == layoutItem)
            { return; }
            layoutItem.LayoutElement.FloatingHeight = (double)e.NewValue;

        }



    }
}
