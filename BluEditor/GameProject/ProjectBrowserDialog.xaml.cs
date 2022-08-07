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
using System.Windows.Media.Animation;
using BluEditor.Utilities;

namespace BluEditor.GameProject
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ProjectBrowserDialog : Window
    {
        private readonly CustomEasingFunction m_easing = new CustomEasingFunction() { EasingMode = EasingMode.EaseInOut };

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

        private void AnimateToOpenProject()
        {
            ThicknessAnimation anim = new ThicknessAnimation(new Thickness(-1600, 10, 0, 0), new Thickness(0, 10, 0, 0), new Duration(TimeSpan.FromSeconds(1)));
            anim.EasingFunction = m_easing;
            browserContent.BeginAnimation(MarginProperty, anim);
        }

        private void AnimateToCreateProject()
        {
            ThicknessAnimation anim = new ThicknessAnimation(new Thickness(0, 10, 0, 0), new Thickness(-1600, 10, 0, 0), new Duration(TimeSpan.FromSeconds(1)));
            anim.EasingFunction = m_easing;
            browserContent.BeginAnimation(MarginProperty, anim);
        }

        public void OnToggleButton_Click(object in_sender, RoutedEventArgs in_args)
        {
            if (in_sender == openProjectButton)
            {
                if (createProjectButton.IsChecked == true)
                {
                    createProjectButton.IsChecked = false;

                    AnimateToOpenProject();
                    openProjectView.IsEnabled = true;
                    newProjectView.IsEnabled = false;
                }
                openProjectButton.IsChecked = true;
            }

            if (in_sender == createProjectButton)
            {
                if (openProjectButton.IsChecked == true)
                {
                    openProjectButton.IsChecked = false;

                    AnimateToCreateProject();
                    openProjectView.IsEnabled = false;
                    newProjectView.IsEnabled = true;
                }
                createProjectButton.IsChecked = true;
            }
        }
    }
}