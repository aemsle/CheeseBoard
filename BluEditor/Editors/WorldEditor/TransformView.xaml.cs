using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using System.Linq;

using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Numerics;
using BluEditor.Components;
using BluEditor.GameProject;
using BluEditor.Utilities;

namespace BluEditor.Editors
{
    /// <summary>
    /// Interaction logic for TransformView.xaml
    /// </summary>
    public partial class TransformView : UserControl
    {
        private Action m_undoAction = null;
        public bool m_propertyChanged = false;

        public TransformView()
        {
            InitializeComponent();
            Loaded += OnTransformViewLoaded;
        }

        private void OnTransformViewLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnTransformViewLoaded;
            ((MSTransform)DataContext).PropertyChanged += (s, e) => m_propertyChanged = true;
        }

        private Action GetAction(Func<Transform, (Transform transform, Vector3)> in_selector,
                                 Action<(Transform transform, Vector3)> in_forEach)
        {
            if (!(DataContext is MSTransform viewModel))
            {
                m_undoAction = null;
                m_propertyChanged = false;
                return null;
            }
            var selection = viewModel.SelectedComponents.Select(x => in_selector(x)).ToList();

            return new Action(() =>
            {
                selection.ForEach(x => in_forEach(x));
                ((MSObject)GameObjectInspectorView.Instance.DataContext)?.GetMSComponent<MSTransform>().Refresh();
            });
        }

        private Action GetPositionAction() => GetAction(x => (x, x.Position), (x) => x.transform.Position = x.Item2);

        private Action GetRotationAction() => GetAction(x => (x, x.Rotation), (x) => x.transform.Rotation = x.Item2);

        private Action GetScaleAction() => GetAction(x => (x, x.Scale), (x) => x.transform.Scale = x.Item2);

        private void RecordActions(Action in_redoAction, string name)
        {
            if (m_propertyChanged)
            {
                Debug.Assert(m_undoAction != null);
                m_propertyChanged = false;

                Project.UndoRedo.Add(new UndoRedoAction(
                name,
                m_undoAction,
                in_redoAction
                ));
            }
        }

        private void OnPosition_VectorBox_PreviewMouse_LBD(object sender, MouseButtonEventArgs e)
        {
            m_propertyChanged = false;
            m_undoAction = GetPositionAction();
        }

        private void OnPosition_VectorBox_PreviewMouse_LBU(object sender, MouseButtonEventArgs e)
        {
            RecordActions(GetPositionAction(), "Position change");
        }

        private void OnRotation_VectorBox_PreviewMouse_LBD(object sender, MouseButtonEventArgs e)
        {
            m_propertyChanged = false;
            m_undoAction = GetRotationAction();
        }

        private void OnRotation_VectorBox_PreviewMouse_LBU(object sender, MouseButtonEventArgs e)
        {
            RecordActions(GetRotationAction(), "Rotation change");
        }

        private void OnScale_VectorBox_PreviewMouse_LBD(object sender, MouseButtonEventArgs e)
        {
            m_propertyChanged = false;
            m_undoAction = GetScaleAction();
        }

        private void OnScale_VectorBox_PreviewMouse_LBU(object sender, MouseButtonEventArgs e)
        {
            RecordActions(GetScaleAction(), "Scale change");
        }

        private void OnPosition_VectorBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (m_propertyChanged && m_undoAction != null)
            {
                OnPosition_VectorBox_PreviewMouse_LBU(sender, null);
            }
        }

        private void OnRotation_VectorBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (m_propertyChanged && m_undoAction != null)
            {
                OnRotation_VectorBox_PreviewMouse_LBU(sender, null);
            }
        }

        private void OnScale_VectorBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (m_propertyChanged && m_undoAction != null)
            {
                OnScale_VectorBox_PreviewMouse_LBU(sender, null);
            }
        }
    }
}