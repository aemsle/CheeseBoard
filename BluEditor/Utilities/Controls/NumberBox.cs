using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Numerics;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BluEditor.Utilities.Controls
{
    [TemplatePart(Name = "PART_textBlock", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_textBox", Type = typeof(TextBox))]
    public class NumberBox : Control
    {
        private double m_originalValue;
        private double m_mouseXStart;
        private double m_multiplier;
        private bool m_captured;

        private bool m_valueChanged;

        public double Multiplier
        {
            get { return (double)GetValue(MultiplierProperty); }
            set { SetValue(MultiplierProperty, value); }
        }

        public static readonly DependencyProperty MultiplierProperty =
            DependencyProperty.Register(nameof(Multiplier), typeof(double), typeof(NumberBox),
                                        new PropertyMetadata(1.0d));

        public string Value

        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(string), typeof(NumberBox),
                                        new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_textBlock") is TextBlock textblock)
            {
                textblock.MouseLeftButtonDown += OnTextBlock_Mouse_LBD;
                textblock.MouseLeftButtonUp += OnTextBlock_Mouse_LBU;
                textblock.MouseMove += OnTextBlock_Mouse_Move;
            }
        }

        private void OnTextBlock_Mouse_LBD(object in_sender, MouseButtonEventArgs in_args)
        {
            double.TryParse(Value, out m_originalValue);

            Mouse.Capture((UIElement)in_sender);
            m_captured = true;
            m_valueChanged = false;
            in_args.Handled = true;

            m_mouseXStart = in_args.GetPosition(this).X;
            Focus();
        }

        private void OnTextBlock_Mouse_LBU(object in_sender, MouseButtonEventArgs in_args)
        {
            if (m_captured)
            {
                Mouse.Capture(null);
                m_captured = false;
                in_args.Handled = false;
                if (!m_valueChanged && GetTemplateChild("PART_textBox") is TextBox textBox)
                {
                    textBox.Visibility = Visibility.Visible;
                    textBox.Focus();
                    textBox.SelectAll();
                }
            }
        }

        private void OnTextBlock_Mouse_Move(object in_sender, MouseEventArgs in_args)
        {
            if (m_captured)
            {
                double currentMouseX = in_args.GetPosition(this).X;
                double mouseDelta = currentMouseX - m_mouseXStart;
                if (Math.Abs(mouseDelta) > SystemParameters.MinimumHorizontalDragDistance)
                {
                    if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control)) m_multiplier = 0.001d;
                    else if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)) m_multiplier = 0.1d;
                    else m_multiplier = 0.01d;
                    double newValue = m_originalValue + (mouseDelta * m_multiplier * Multiplier);
                    Value = newValue.ToString("0.#####");
                    m_valueChanged = true;
                }
            }
        }

        static NumberBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberBox),
                new FrameworkPropertyMetadata(typeof(NumberBox)));
        }
    }
}