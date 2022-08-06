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

namespace BluEditor.Utilities
{
    /// <summary>
    /// Interaction logic for LoggerView.xaml
    /// </summary>
    public partial class LoggerView : UserControl
    {
        public LoggerView()
        {
            InitializeComponent();

            Loaded += LoggerView_Loaded;
        }

        private void LoggerView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= LoggerView_Loaded;
        }

        private void OnClear_Button_Clicked(object in_sender, RoutedEventArgs in_args)
        {
            Logger.Clear();
        }

        private void OnLogFilter_Button_Clicked(object in_sender, RoutedEventArgs in_args)
        {
            int messageType = 0b0000; // mask nothing
            if (toggleInfoLog.IsChecked == true) messageType |= (int)MessageType.INFO; // mask info
            if (toggleWarningLog.IsChecked == true) messageType |= (int)MessageType.WARNING; // mask warning
            if (toggleErrorLog.IsChecked == true) messageType |= (int)MessageType.ERROR; // mask error

            Logger.SetFilter(messageType);
        }
    }
}