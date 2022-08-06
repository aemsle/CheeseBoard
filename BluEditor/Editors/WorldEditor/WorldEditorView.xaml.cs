using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
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

namespace BluEditor.Editors
{
    /// <summary>
    /// Interaction logic for WorldEditorView.xaml
    /// </summary>
    public partial class WorldEditorView : UserControl
    {
        public WorldEditorView()
        {
            InitializeComponent();
            Loaded += OnWorldEditorViewLoaded;
        }

        public void OnWorldEditorViewLoaded(object in_sender, RoutedEventArgs in_args)
        {
            Loaded -= OnWorldEditorViewLoaded;
            Focus();

            Utilities.Logger.Log("Info Message");
            Utilities.Logger.Log("Warning Message", Utilities.MessageType.WARNING);
            Utilities.Logger.Log("Info Message", Utilities.MessageType.ERROR);

            ((INotifyCollectionChanged)Project.UndoRedo.UndoList).CollectionChanged += (s, e) => Focus();
        }
    }
}