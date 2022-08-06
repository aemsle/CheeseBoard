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
using System.Windows.Shapes;

namespace BluEditor.GameProject
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ProjectBrowserDialog : Window
    {
        public ProjectBrowserDialog()
        {
            InitializeComponent();
            Topmost = true;
            Loaded += OnProjectBrowserDialogLoaded;
        }

        public void OnProjectBrowserDialogLoaded(object in_sender, RoutedEventArgs in_args)
        {
            Loaded -= OnProjectBrowserDialogLoaded;
            if (!OpenProjectVM.Projects.Any())
            {
                openProjectButton.IsEnabled = false;
                openProjectView.Visibility = Visibility.Hidden;
                OnToggleButton_Click(createProjectButton, new RoutedEventArgs());
            }
        }

        public void OnToggleButton_Click(object in_sender, RoutedEventArgs in_args)
        {
            if (in_sender == openProjectButton)
            {
                if (createProjectButton.IsChecked == true)
                {
                    createProjectButton.IsChecked = false;

                    browserContent.Margin = new Thickness(0, 25, 0, 0);
                }
                openProjectButton.IsChecked = true;
            }

            if (in_sender == createProjectButton)
            {
                if (openProjectButton.IsChecked == true)
                {
                    openProjectButton.IsChecked = false;

                    browserContent.Margin = new Thickness(-800, 25, 0, 0);
                }
                createProjectButton.IsChecked = true;
            }
        }
    }
}