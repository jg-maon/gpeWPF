using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace WpfApplication1
{
    class MainWindowBehavior : Behavior<Window>
    {

        #region DownKey Property

        public Key DownKey
        {
            get { return (Key)GetValue(DownKeyProperty); }
            set { SetValue(DownKeyProperty, value); }
        }

        public static readonly DependencyProperty DownKeyProperty =
            DependencyProperty.Register("DownKey",
            typeof(Key),
            typeof(MainWindowBehavior),
            new UIPropertyMetadata(null));

        #endregion



        public KeyStates KeyStates
        {
            get { return (KeyStates)GetValue(KeyStatesProperty); }
            set { SetValue(KeyStatesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for KeyStates.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty KeyStatesProperty =
            DependencyProperty.Register("KeyStates", typeof(KeyStates), typeof(MainWindowBehavior), new PropertyMetadata(KeyStates.None));



        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;

        }

        void AssociatedObject_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            this.DownKey = e.Key;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
            }
        }

    }
}
