using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace WpfApplication1
{
	class BindableSelectedItemsBehavior: Behavior<ListBox>
	{


		public IList SelectedItems
		{
			get { return (IList)GetValue(SelectedItemsProperty); }
			set { SetValue(SelectedItemsProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SelectedItemsProperty =
			DependencyProperty.Register("SelectedItems", typeof(IList), typeof(BindableSelectedItemsBehavior), new PropertyMetadata(null, _OnSelectedItemsChanged));

		private static void _OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var tree = (d as BindableSelectedItemsBehavior)?.AssociatedObject;
			var items = e.NewValue as IList;
			if (null != items && null != tree)
			{
				tree.SelectedItems.Clear();
				foreach(var item in items )
				{
					tree.SelectedItems.Add(item);
				}
			}
		}

		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.SelectionChanged += _AssociatedObject_SelectionChanged;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			if (null != AssociatedObject)
			{
				AssociatedObject.SelectionChanged -= _AssociatedObject_SelectionChanged;
			}
		}

		private void _AssociatedObject_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			SelectedItems.Clear();
			foreach (var item in AssociatedObject.SelectedItems)
				SelectedItems.Add(item);
		}
	}
}
