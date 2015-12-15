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
            if (!GetSizeChangeEventAdded(d))
            {
                SetLayoutItem(layoutItem.LayoutElement, layoutItem);
                //layoutItem.SizeChanged += layoutItem_SizeChanged;
                layoutItem.LayoutElement.PropertyChanged += LayoutElement_PropertyChanged;
                SetSizeChangeEventAdded(layoutItem, true);
            }
        }



        public static Xceed.Wpf.AvalonDock.Controls.LayoutItem GetLayoutItem(DependencyObject obj)
        {
            return (Xceed.Wpf.AvalonDock.Controls.LayoutItem)obj.GetValue(LayoutItemProperty);
        }

        public static void SetLayoutItem(DependencyObject obj, Xceed.Wpf.AvalonDock.Controls.LayoutItem value)
        {
            obj.SetValue(LayoutItemProperty, value);
        }

        // Using a DependencyProperty as the backing store for LayoutItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LayoutItemProperty =
            DependencyProperty.RegisterAttached("LayoutItem", typeof(Xceed.Wpf.AvalonDock.Controls.LayoutItem), typeof(LayoutItemBehavior), new PropertyMetadata(null));





        private static bool GetSizeChangeEventAdded(DependencyObject obj)
        {
            return (bool)obj.GetValue(SizeChangeEventAddedProperty);
        }

        private static void SetSizeChangeEventAdded(DependencyObject obj, bool value)
        {
            obj.SetValue(SizeChangeEventAddedProperty, value);
        }

        // Using a DependencyProperty as the backing store for SizeChangeEventAdded.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty SizeChangeEventAddedProperty =
            DependencyProperty.RegisterAttached("SizeChangeEventAdded", typeof(bool), typeof(LayoutItemBehavior), new PropertyMetadata(false));



        static void LayoutElement_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var layout = sender as Xceed.Wpf.AvalonDock.Layout.LayoutContent;
            var layoutElement = sender as Xceed.Wpf.AvalonDock.Layout.LayoutElement;
            var layoutItem = GetLayoutItem(layoutElement);
            if(e.PropertyName == "FloatingHeight")
            {
                SetFloatingHeight(layoutItem, layout.FloatingHeight);
            }
            else if(e.PropertyName == "FloatingWidth")
            {
                SetFloatingWidth(layoutItem, layout.FloatingWidth);
            }
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
            if(!GetSizeChangeEventAdded(d))
            {
                //layoutItem.SizeChanged += layoutItem_SizeChanged;
                SetLayoutItem(layoutItem.LayoutElement, layoutItem);
                layoutItem.LayoutElement.PropertyChanged += LayoutElement_PropertyChanged;
                SetSizeChangeEventAdded(d, true);
            }

        }



    }
}
