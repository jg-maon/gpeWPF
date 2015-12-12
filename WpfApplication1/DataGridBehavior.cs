using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace WpfApplication1
{
    class DataGridBehavior : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.AutoGeneratingColumn += AssociatedObject_AutoGeneratingColumn;
        }

        void AssociatedObject_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (null != this.AssociatedObject)
            {

                this.AssociatedObject.AutoGeneratingColumn -= AssociatedObject_AutoGeneratingColumn;
            }
        }
    }
}
