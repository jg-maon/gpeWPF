using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication1.Controls
{
    /// <summary>
    /// このカスタム コントロールを XAML ファイルで使用するには、手順 1a または 1b の後、手順 2 に従います。
    ///
    /// 手順 1a) 現在のプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfApplication1.Controls"
    ///
    ///
    /// 手順 1b) 異なるプロジェクトに存在する XAML ファイルでこのカスタム コントロールを使用する場合
    /// この XmlNamespace 属性を使用場所であるマークアップ ファイルのルート要素に
    /// 追加します:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfApplication1.Controls;assembly=WpfApplication1.Controls"
    ///
    /// また、XAML ファイルのあるプロジェクトからこのプロジェクトへのプロジェクト参照を追加し、
    /// リビルドして、コンパイル エラーを防ぐ必要があります:
    ///
    ///     ソリューション エクスプローラーで対象のプロジェクトを右クリックし、
    ///     [参照の追加] の [プロジェクト] を選択してから、このプロジェクトを参照し、選択します。
    ///
    ///
    /// 手順 2)
    /// コントロールを XAML ファイルで使用します。
    ///
    ///     <MyNamespace:MouseIncrementingTextBox/>
    ///
    /// </summary>
    [TemplatePart(Name=PART_TextBox, Type=typeof(TextBox)), TemplatePart(Name=PART_ValueBorder, Type=typeof(Border))]
    public class MouseIncrementingTextBox : Control
    {
        #region TemplateParts
        private const string PART_TextBox = "PART_TextBox";
        private const string PART_ValueBorder = "PART_ValueBorder";
        #endregion
        #region Parts
        private TextBox m_textBox;
        private Border m_border;
        #endregion

        static MouseIncrementingTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MouseIncrementingTextBox), new FrameworkPropertyMetadata(typeof(MouseIncrementingTextBox)));
        }
        public static double Clamp(double v, double min, double max)
        {
            return Math.Max(min, Math.Min(v, max));
        }
        #region Properties

        public double XIncrementValue
        {
            get { return (double)GetValue(XIncrementValueProperty); }
            set { SetValue(XIncrementValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for XIncrementValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty XIncrementValueProperty =
            DependencyProperty.Register("XIncrementValue", typeof(double), typeof(MouseIncrementingTextBox), new PropertyMetadata(0.0));




        public double YIncrementValue
        {
            get { return (double)GetValue(YIncrementValueProperty); }
            set { SetValue(YIncrementValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for YIncrementValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YIncrementValueProperty =
            DependencyProperty.Register("YIncrementValue", typeof(double), typeof(MouseIncrementingTextBox), new PropertyMetadata(0.0));





        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(MouseIncrementingTextBox), new PropertyMetadata(0.0, _OnValueChanged));

        

        private static void _OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as MouseIncrementingTextBox;
            if (null != textBox)
            {
                var oldValue = (double)e.OldValue;
                var newValue = (double)e.NewValue;

                textBox.OnValueChanged(oldValue, newValue);
            }
        }

        private void OnValueChanged(double oldValue, double newValue)
        {
            if(null != m_textBox)
            {
                m_textBox.Text = Value.ToString();
            }
            _ComputeBorderWidth();
        }




        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(MouseIncrementingTextBox), new PropertyMetadata(0.0, _OnMaximumChanged));

        private static void _OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as MouseIncrementingTextBox;
            if(null != textBox)
            {
                var oldValue = (double)e.OldValue;
                var newValue = (double)e.NewValue;

                textBox.OnMaximumChanged(oldValue, newValue);
            }
        }

        protected virtual void OnMaximumChanged(double oldValue, double newValue)
        {
            _ComputeBorderWidth();
        }




        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimum.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(MouseIncrementingTextBox), new PropertyMetadata(0.0, _OnMinimumChanged));

        private static void _OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as MouseIncrementingTextBox;
            if (null != textBox)
            {
                var oldValue = (double)e.OldValue;
                var newValue = (double)e.NewValue;

                textBox.OnMinimumChanged(oldValue, newValue);
            }
        }

        protected virtual void OnMinimumChanged(double oldValue, double newValue)
        {
            _ComputeBorderWidth();
        }




        #endregion


        /// <summary>
        /// 値からBorderの幅を算出して設定を行う
        /// </summary>
        private void _ComputeBorderWidth()
        {
            if (null == m_textBox || null == m_border)
            {
                return;
            }

            // 一応範囲内に計算しなおしておく
            var value = Clamp(Value, Minimum, Maximum);

            // テキストボックスの幅(ValueがMaxの時にこの値になる)
            var textBoxWidth = m_textBox.ActualWidth;
            
            // 現在の値の割合
            var r = (Minimum - value) / (Minimum - Maximum);
            
            m_border.Width = textBoxWidth * r;

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            m_textBox = GetTemplateChild(PART_TextBox) as TextBox;
            m_border = GetTemplateChild(PART_ValueBorder) as Border;

        }


        private class MouseIncrementor
        {

            private MouseDirections _enumMouseDirection = MouseDirections.None;

            private Point _objPoint;
            public enum MouseDirections
            {
                LeftRight,
                None,
                UpDown
            }

            public MouseDirections MouseDirection
            {
                get { return _enumMouseDirection; }
                set { _enumMouseDirection = value; }
            }

            public Point Point
            {
                get { return _objPoint; }
                set { _objPoint = value; }
            }

            public MouseIncrementor(Point objPoint, MouseDirections enumMouseDirection)
            {
                _objPoint = objPoint;
                _enumMouseDirection = enumMouseDirection;

            }

        }
        private MouseIncrementor _objMouseIncrementor;

        private string _strOriginalTextValue = null;


    }
}
