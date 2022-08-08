using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace BluEditor.Dictionaries
{
    public partial class ControlTemplates : ResourceDictionary
    {
        private void OnTextBox_KeyDown(object in_sender, KeyEventArgs in_args)
        {
            TextBox textBox = (TextBox)in_sender;

            BindingExpression expression = textBox.GetBindingExpression(TextBox.TextProperty);

            if (expression == null) return;
            if (in_args.Key == Key.Enter)
            {
                if (textBox.Tag is ICommand command && command.CanExecute(textBox.Text))
                {
                    command.Execute(textBox.Text);
                }
                else
                {
                    expression.UpdateSource();
                }
                Keyboard.ClearFocus();
                in_args.Handled = true;
            }
            else if (in_args.Key == Key.Escape)
            {
                expression.UpdateSource();
                Keyboard.ClearFocus();
            }
        }

        private void OnTextBoxRename_KeyDown(object in_sender, KeyEventArgs in_args)
        {
            TextBox textBox = (TextBox)in_sender;

            BindingExpression expression = textBox.GetBindingExpression(TextBox.TextProperty);

            if (expression == null) return;
            if (in_args.Key == Key.Enter)
            {
                if (textBox.Tag is ICommand command && command.CanExecute(textBox.Text))
                {
                    command.Execute(textBox.Text);
                }
                else
                {
                    expression.UpdateSource();
                }
                textBox.Visibility = Visibility.Collapsed;
                in_args.Handled = true;
            }
            else if (in_args.Key == Key.Escape)
            {
                expression.UpdateSource();
                textBox.Visibility = Visibility.Collapsed;
            }
        }

        private void OnTextBoxRename_LostFocus(object in_sender, RoutedEventArgs in_args)
        {
            TextBox textBox = (TextBox)in_sender;
            if (!textBox.IsVisible) return;
            BindingExpression expression = textBox.GetBindingExpression(TextBox.TextProperty);
            if (expression != null)
            {
                expression.UpdateTarget();
                textBox.Visibility = Visibility.Collapsed;
            }
        }

        private void OnClose_Button_Click(object in_sender, RoutedEventArgs in_args)
        {
            Window window = (Window)((FrameworkElement)in_sender).TemplatedParent;
            window.Close();
        }

        private void OnMaximizeRestore_Button_Click(object in_sender, RoutedEventArgs in_args)
        {
            Window window = (Window)((FrameworkElement)in_sender).TemplatedParent;
            window.WindowState = (window.WindowState == WindowState.Normal)
             ? window.WindowState = WindowState.Maximized : window.WindowState = WindowState.Normal;
        }

        private void OnMinimize_Button_Click(object in_sender, RoutedEventArgs in_args)
        {
            Window window = (Window)((FrameworkElement)in_sender).TemplatedParent;
            window.WindowState = WindowState.Minimized;
        }
    }
}