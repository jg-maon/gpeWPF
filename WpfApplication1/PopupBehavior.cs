using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace WpfApplication1
{
    class PopupBehavior : Behavior<Popup>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            //AssociatedObject.PlacementTarget.MouseEnter += _MouseEntered;
            //AssociatedObject.PlacementTarget.MouseLeave += _MouseLeaved;
            AssociatedObject.PlacementTarget.MouseMove += _MouseMoved;
            //Mouse.AddPreviewMouseMoveHandler(AssociatedObject.PlacementTarget, _MouseMoved);

        }

        private void _MouseLeaved(object sender, MouseEventArgs e)
        {
            AssociatedObject.IsOpen = false;
        }

        private void _MouseEntered(object sender, MouseEventArgs e)
        {
            AssociatedObject.IsOpen = true;
        }

        private void _MouseMoved(object sender, MouseEventArgs e)
        {
            var current = e.GetPosition(AssociatedObject.PlacementTarget);
            AssociatedObject.HorizontalOffset = current.X;
            AssociatedObject.VerticalOffset = current.Y;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (null != AssociatedObject)
            {
                AssociatedObject.PlacementTarget.MouseMove -= _MouseMoved;
                AssociatedObject.PlacementTarget.MouseEnter -= _MouseEntered;
                AssociatedObject.PlacementTarget.MouseLeave -= _MouseLeaved;
            }
            //Mouse.RemovePreviewMouseMoveHandler(AssociatedObject.PlacementTarget, _MouseMoved);
        }
    }
}
