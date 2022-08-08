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
using BluEditor.Components;
using BluEditor.GameProject;
using BluEditor.Utilities;

namespace BluEditor.Editors
{
    /// <summary>
    /// Interaction logic for GameObjectInspectorView.xaml
    /// </summary>
    public partial class GameObjectInspectorView : UserControl
    {
        private Action m_undoAction;

        private string m_propertyName;

        public static GameObjectInspectorView Instance { get; private set; } // maybe change this in future to allow for multiple inspectors

        public GameObjectInspectorView()
        {
            InitializeComponent();
            DataContext = null;
            Instance = this;
            DataContextChanged += (_, __) =>
            {
                if (DataContext != null)
                {
                    ((MSObject)DataContext).PropertyChanged += (s, e) => m_propertyName = e.PropertyName;
                }
            };
        }

        private Action? GetRenameAction()
        {
            MSObject viewModel = (MSObject)DataContext;
            List<(GameObject gameObject, string Name)>? selection = viewModel.SelectedObjects.Select(gameObject => (gameObject, gameObject.Name)).ToList();
            return new Action(() =>
            {
                selection.ForEach(item => item.gameObject.Name = item.Name);
                ((MSObject)DataContext).Refresh();
            });
        }

        private Action? GetEnableAction()
        {
            MSObject viewModel = (MSObject)DataContext;
            List<(GameObject gameObject, bool Enabled)>? selection = viewModel.SelectedObjects.Select(gameObject => (gameObject, gameObject.Enabled)).ToList();
            return new Action(() =>
            {
                selection.ForEach(item => item.gameObject.Enabled = item.Enabled);
                ((MSObject)DataContext).Refresh();
            });
        }

        private void OnName_TextBox_GotKeyboardFocus(object in_sender, KeyboardFocusChangedEventArgs in_args)
        {
            m_propertyName = string.Empty;
            m_undoAction = GetRenameAction();
        }

        private void OnName_TextBox_LostKeyboardFocus(object in_sender, KeyboardFocusChangedEventArgs in_args)
        {
            if (m_propertyName == nameof(MSObject.Name) && m_undoAction != null)
            {
                Action? redoAction = GetRenameAction();

                Project.UndoRedo.Add(new UndoRedoAction(
                "Rename Game Object",
                m_undoAction,
                redoAction
                ));
                m_propertyName = null;
            }
            m_undoAction = null;
        }

        private void OnEnable_CheckBox_Click(object in_sender, RoutedEventArgs in_args)
        {
            Action? undoAction = GetEnableAction();
            MSObject viewModel = (MSObject)DataContext;
            viewModel.Enabled = ((CheckBox)in_sender).IsChecked == true;
            Action? redoAction = GetEnableAction();

            Project.UndoRedo.Add(new UndoRedoAction(
            viewModel.Enabled == true ? "Enable GameObject" : "Disable GameObject",
            undoAction,
            redoAction
            ));
            m_propertyName = null;
        }
    }
}