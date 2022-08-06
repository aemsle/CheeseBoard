using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel; // CancelEventArgs
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BluEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnMainWindowLoaded;
            Closing += OnMainWindowClosing;
        }

        private void OnMainWindowLoaded(object in_sender, RoutedEventArgs in_args)
        {
            Loaded -= OnMainWindowLoaded;
            OpenProjectBrowser();
        }

        private void OpenProjectBrowser()
        {
            GameProject.ProjectBrowserDialog projectBrowser = new GameProject.ProjectBrowserDialog();
            if (projectBrowser.ShowDialog() == false || projectBrowser.DataContext == null) // if user closes window
            {
                Application.Current.Shutdown();
            }
            else
            {
                GameProject.Project.Current?.Unload();
                DataContext = projectBrowser.DataContext;
                GameProject.Project project = (GameProject.Project)DataContext;
                Title = $"Blu Editor: {project.ProjectName}";
            }
        }

        private void OnMainWindowClosing(object sender, CancelEventArgs e)
        {
            DLLWrapper.EngineAPI.Shutdown();
        }
    }
}