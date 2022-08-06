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
    /// Interaction logic for NewProjectView.xaml
    /// </summary>
    public partial class NewProjectView : UserControl
    {
        public NewProjectView()
        {
            InitializeComponent();

            //https://www.youtube.com/watch?v=EW4Ud9b3VWo&list=PLU2nPsAdxKWQYxkmQ3TdbLsyc1l2j25XM&index=2
        }

        public void OnCreate_Button_Click(object in_sender, RoutedEventArgs in_args)
        {
            NewProjectVM viewModel = (NewProjectVM)DataContext;
            string projectPath = viewModel.CreateProject((ProjectTemplate)templateListBox.SelectedItem);
            bool dialogResult = false;
            Window window = Window.GetWindow(this);

            if (!string.IsNullOrEmpty(projectPath))
            {
                dialogResult = true;
                Project project = OpenProjectVM.Open(new ProjectData() { ProjectName = viewModel.ProjectName, ProjectPath = projectPath });
                window.DataContext = project;
            }

            window.DialogResult = dialogResult;
            window.Close();
        }
    }
}