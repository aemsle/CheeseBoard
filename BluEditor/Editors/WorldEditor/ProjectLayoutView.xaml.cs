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
using BluEditor.GameProject;
using BluEditor.Components;
using BluEditor.Editors;
using BluEditor.Utilities;

namespace BluEditor.Editors
{
    /// <summary>
    /// Interaction logic for ProjectLayoutView.xaml
    /// </summary>
    public partial class ProjectLayoutView : UserControl
    {
        public ProjectLayoutView()
        {
            InitializeComponent();
        }

        private void OnAddGameObject_Button_Click(object in_sender, RoutedEventArgs in_args)
        {
            Button button = (Button)in_sender;
            Scene viewModel = (Scene)button.DataContext;
            viewModel.AddGameObjectCommand.Execute(new GameObject(viewModel) { Name = "Empty GameObject" });
        }

        private void OnGameObject_ListBox_SelectionChanged(object in_sender, SelectionChangedEventArgs in_args)
        {
            ListBox listbox = (ListBox)in_sender;
            List<GameObject> newSelection = listbox.SelectedItems.Cast<GameObject>().ToList();
            List<GameObject> previousSelection = newSelection.Except(
                in_args.AddedItems.Cast<GameObject>()
                ).Concat(
                in_args.RemovedItems.Cast<GameObject>()
                ).ToList(
            );

            Project.UndoRedo.Add(new UndoRedoAction(
                "Selection Changed",
                () => //undo
                {
                    listbox.UnselectAll();
                    previousSelection.ForEach(x => ((ListBoxItem)listbox.ItemContainerGenerator.ContainerFromItem(x)).IsSelected = true);
                },
                () => //redo
                {
                    listbox.UnselectAll();
                    newSelection.ForEach(x => ((ListBoxItem)listbox.ItemContainerGenerator.ContainerFromItem(x)).IsSelected = true);
                }
                ));

            MSGameObject msGameObject = null;
            if (newSelection.Any())
            {
                msGameObject = new MSGameObject(newSelection);
            }

            GameObjectInspectorView.Instance.DataContext = msGameObject;
        }
    }
}