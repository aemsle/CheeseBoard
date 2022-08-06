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

namespace BluEditor.GameProject
{
    /// <summary>
    /// Interaction logic for OpenProjectView.xaml
    /// </summary>
    public partial class OpenProjectView : UserControl
    {
        public OpenProjectView()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                ListBoxItem item = (ListBoxItem)projectsListBox.ItemContainerGenerator
                .ContainerFromIndex(projectsListBox.SelectedIndex);
                item?.Focus();
            };
        }

        private void OnListBoxItem_Mouse_DoubleClick(object in_sender, MouseButtonEventArgs in_args)
        {
            OpenProject();
        }

        public void OnOpen_Button_Click(object in_sender, RoutedEventArgs in_args)
        {
            OpenProject();
        }

        private void OpenProject()
        {
            Project project = OpenProjectVM.Open((ProjectData)projectsListBox.SelectedItem);
            bool dialogResult = false;
            Window window = Window.GetWindow(this);

            if (project != null)
            {
                dialogResult = true;
                window.DataContext = project;
            }

            window.DialogResult = dialogResult;
            window.Close();
        }
    }
}