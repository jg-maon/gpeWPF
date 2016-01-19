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
    ///     <MyNamespace:YawPitchTweaker/>
    ///
    /// </summary>
    [TemplatePart(Name = PART_TweakerCanvas, Type = typeof(Canvas))]
    [TemplatePart(Name = PART_YawPitchSelector, Type = typeof(Canvas))]
    public class YawPitchTweaker : Control
    {
        #region TemplateParts
        private const string PART_TweakerCanvas = "PART_TweakerCanvas";
        private const string PART_YawPitchSelector = "PART_YawPitchSelector";
        private const string HorizontalMainLine = "HorizontalMainLine";
        private const string VerticalMainLine = "VerticalMainLine";
        private const string HorizontalSubTopLine = "HorizontalSubTopLine";
        private const string HorizontalSubBottomLine = "HorizontalSubBottomLine";
        private const string VerticalSubLeftLine = "VerticalSubLeftLine";
        private const string VerticalSubRightLine = "VerticalSubRightLine";
        #endregion
        #region Parts
        private Canvas m_tweakerCanvas;
        private Canvas m_yawPitchSelector;
        private Line m_horizontalMainLine = null;
        private Line m_verticalMainLine = null;
        private Line m_horizontalSubTopLine = null;
        private Line m_horizontalSubBottomLine = null;
        private Line m_verticalSubLeftLine = null;
        private Line m_verticalSubRightLine = null;
        #endregion
        static YawPitchTweaker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(YawPitchTweaker), new FrameworkPropertyMetadata(typeof(YawPitchTweaker)));
        }

        #region Properties


        public double Yaw
        {
            get { return (double)GetValue(YawProperty); }
            set { SetValue(YawProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Yaw.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty YawProperty =
            DependencyProperty.Register("Yaw", typeof(double), typeof(YawPitchTweaker), new FrameworkPropertyMetadata(0.0));

        public double Pitch
        {
            get { return (double)GetValue(PitchProperty); }
            set { SetValue(PitchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Pitch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PitchProperty =
            DependencyProperty.Register("Pitch", typeof(double), typeof(YawPitchTweaker), new FrameworkPropertyMetadata(0.0));




        public double MaxYaw
        {
            get { return (double)GetValue(MaxYawProperty); }
            set { SetValue(MaxYawProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxYaw.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxYawProperty =
            DependencyProperty.Register("MaxYaw", typeof(double), typeof(YawPitchTweaker), new FrameworkPropertyMetadata(180.0));



        public double MinYaw
        {
            get { return (double)GetValue(MinYawProperty); }
            set { SetValue(MinYawProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinYaw.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinYawProperty =
            DependencyProperty.Register("MinYaw", typeof(double), typeof(YawPitchTweaker), new FrameworkPropertyMetadata(-180.0));




        public double MaxPitch
        {
            get { return (double)GetValue(MaxPitchProperty); }
            set { SetValue(MaxPitchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxPitch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxPitchProperty =
            DependencyProperty.Register("MaxPitch", typeof(double), typeof(YawPitchTweaker), new FrameworkPropertyMetadata(90.0));



        public double MinPitch
        {
            get { return (double)GetValue(MinPitchProperty); }
            set { SetValue(MinPitchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinPitch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinPitchProperty =
            DependencyProperty.Register("MinPitch", typeof(double), typeof(YawPitchTweaker), new FrameworkPropertyMetadata(-90.0));




        public Brush MainLineBrush
        {
            get { return (Brush)GetValue(MainLineBrushProperty); }
            set { SetValue(MainLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MainLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainLineBrushProperty =
            DependencyProperty.Register("MainLineBrush", typeof(Brush), typeof(YawPitchTweaker), new FrameworkPropertyMetadata(Brushes.DarkSlateGray));




        public Brush SubLineBrush
        {
            get { return (Brush)GetValue(SubLineBrushProperty); }
            set { SetValue(SubLineBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SubLineBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SubLineBrushProperty =
            DependencyProperty.Register("SubLineBrush", typeof(Brush), typeof(YawPitchTweaker), new FrameworkPropertyMetadata(Brushes.DimGray));

        

        #endregion


        private TranslateTransform m_yawPitchSelectorTransform = new TranslateTransform();
        private Point? m_currentPosition;


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if(null != m_tweakerCanvas)
            {
                m_tweakerCanvas.MouseLeftButtonDown -= _TweakerCanvas_MouseLeftButtonDown;
                m_tweakerCanvas.MouseLeftButtonUp -= _TweakerCanvas_MouseLeftButtonUp;
                m_tweakerCanvas.MouseMove -= _TweakerCanvas_MouseMove;
                m_tweakerCanvas.SizeChanged -= _TweakerCanvas_SizeChanged;

            }
            m_tweakerCanvas = GetTemplateChild(PART_TweakerCanvas) as Canvas;

            if(null != m_tweakerCanvas)
            {
                m_tweakerCanvas.MouseLeftButtonDown += _TweakerCanvas_MouseLeftButtonDown;
                m_tweakerCanvas.MouseLeftButtonUp += _TweakerCanvas_MouseLeftButtonUp;
                m_tweakerCanvas.MouseMove += _TweakerCanvas_MouseMove;
                m_tweakerCanvas.SizeChanged += _TweakerCanvas_SizeChanged;
            }

            m_yawPitchSelector = GetTemplateChild(PART_YawPitchSelector) as Canvas;

            if (null != m_yawPitchSelector)
            {
                m_yawPitchSelector.RenderTransform = m_yawPitchSelectorTransform;
            }


            m_horizontalMainLine = GetTemplateChild(HorizontalMainLine) as Line;
            m_verticalMainLine = GetTemplateChild(VerticalMainLine) as Line;
            m_horizontalSubTopLine = GetTemplateChild(HorizontalSubTopLine) as Line;
            m_horizontalSubBottomLine = GetTemplateChild(HorizontalSubBottomLine) as Line;
            m_verticalSubLeftLine = GetTemplateChild(VerticalSubLeftLine) as Line;
            m_verticalSubRightLine = GetTemplateChild(VerticalSubRightLine) as Line;


            _UpdateYawPitchSelectorPosition(Yaw, Pitch);
        }


        private void _TweakerCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            if (m_currentPosition != null)
            {
                Point newPoint = new Point
                {
                    X = m_currentPosition.Value.X * e.NewSize.Width,
                    Y = m_currentPosition.Value.Y * e.NewSize.Height
                };

                var width = e.NewSize.Width;
                var height = e.NewSize.Height;
                if (null != m_horizontalMainLine)
                {
                    m_horizontalMainLine.X1 = 0.0;
                    m_horizontalMainLine.X2 = width;
                    m_horizontalMainLine.Y1 = height / 2.0;
                    m_horizontalMainLine.Y2 = height / 2.0;
                    m_horizontalMainLine.Stroke = MainLineBrush;
                }
                if (null != m_verticalMainLine)
                {
                    m_verticalMainLine.X1 = width / 2.0;
                    m_verticalMainLine.X2 = width / 2.0;
                    m_verticalMainLine.Y1 = 0.0;
                    m_verticalMainLine.Y2 = height;
                }
                if (null != m_horizontalSubTopLine)
                {
                    m_horizontalSubTopLine.X1 = 0.0;
                    m_horizontalSubTopLine.X2 = width;
                    m_horizontalSubTopLine.Y1 = height / 4.0;
                    m_horizontalSubTopLine.Y2 = height / 4.0;
                }
                if (null != m_horizontalSubBottomLine)
                {
                    m_horizontalSubBottomLine.X1 = 0.0;
                    m_horizontalSubBottomLine.X2 = width;
                    m_horizontalSubBottomLine.Y1 = height * 3.0 / 4.0;
                    m_horizontalSubBottomLine.Y2 = height * 3.0 / 4.0;
                }
                if (null != m_verticalSubLeftLine)
                {
                    m_verticalSubLeftLine.X1 = width / 4.0;
                    m_verticalSubLeftLine.X2 = width / 4.0;
                    m_verticalSubLeftLine.Y1 = 0.0;
                    m_verticalSubLeftLine.Y2 = height;
                }
                if (null != m_verticalSubRightLine)
                {
                    m_verticalSubRightLine.X1 = width * 3.0 / 4.0;
                    m_verticalSubRightLine.X2 = width * 3.0 / 4.0;
                    m_verticalSubRightLine.Y1 = 0.0;
                    m_verticalSubRightLine.Y2 = height;
                }

                _UpdateYawPitchSelectorPositionAndCalculate(newPoint, false);
            }
        }

        private void _TweakerCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var canvas = sender as Canvas;
                Point p = e.GetPosition(canvas);
                _UpdateYawPitchSelectorPositionAndCalculate(p, true);
                Mouse.Synchronize();
            }
        }

        private void _TweakerCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((Canvas)sender).ReleaseMouseCapture();
        }

        private void _TweakerCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var canvas = sender as Canvas;
            var p = e.GetPosition(canvas);
            _UpdateYawPitchSelectorPositionAndCalculate(p, true);
            canvas.CaptureMouse();
            e.Handled = true;
        }




        private void _UpdateYawPitchSelectorPosition(double Yaw, double Pitch)
        {
            if (null == m_tweakerCanvas || null == m_yawPitchSelector)
            {
                return;
            }

            m_currentPosition = null;

            // 0~1 (min-v)/(min-max)
            Point p = new Point(
                (MinPitch - Pitch) / (MinPitch - MaxPitch),
                (MinPitch - Yaw) / (MinPitch - MaxPitch));

            m_currentPosition = p;

            m_yawPitchSelectorTransform.X = (p.X * m_tweakerCanvas.Width) - (m_yawPitchSelector.Width / 2);
            m_yawPitchSelectorTransform.Y = (p.Y * m_tweakerCanvas.Height) - (m_yawPitchSelector.Height / 2);
        }



        private void _UpdateYawPitchSelectorPositionAndCalculate(Point p, bool calculateYawPitch)
        {
            if (p.Y < 0)
            {
                p.Y = 0;
            }
            else if (p.Y > m_tweakerCanvas.ActualHeight)
            {
                p.Y = m_tweakerCanvas.ActualHeight;
            }


            if (p.X < 0)
            {
                p.X = 0;
            }
            else if (p.X > m_tweakerCanvas.ActualWidth)
            {
                p.X = m_tweakerCanvas.ActualWidth;
            }


            m_yawPitchSelectorTransform.X = p.X - (m_yawPitchSelector.Width / 2);
            m_yawPitchSelectorTransform.Y = p.Y - (m_yawPitchSelector.Height / 2);

            p.X = p.X / m_tweakerCanvas.ActualWidth;
            p.Y = p.Y / m_tweakerCanvas.ActualHeight;

            m_currentPosition = p;

            if (calculateYawPitch)
            {
                _CalculateYawPitch(p);
            }

        }

        /// <summary>
        /// X,Yの値からYawPitchの値を計算
        /// </summary>
        /// <param name="p">X,Yそれぞれ0.0～1.0 X:Pitch, Y:Yaw</param>
        private void _CalculateYawPitch(Point p)
        {
            //Min*(1-a)+Max*a
            Pitch = MaxPitch * p.X + MinPitch * (1.0 - p.X);
            Yaw = MaxYaw * p.Y + MinYaw * (1.0 - p.Y);
        }
    }
}
