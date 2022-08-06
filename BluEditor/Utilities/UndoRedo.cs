using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BluEditor.Utilities
{
    public interface IUndoRedo
    {
        string m_actionName { get; }

        void Undo();

        void Redo();
    }

    public class UndoRedoAction : IUndoRedo
    {
        private Action m_undoAction;
        private Action m_redoAction;

        public string m_actionName { get; }

        public void Undo() => m_undoAction();

        public void Redo() => m_redoAction();

        public UndoRedoAction(string in_name)
        {
            m_actionName = in_name;
        }

        public UndoRedoAction(string in_name, Action in_undo, Action in_redo)
            : this(in_name)
        {
            Debug.Assert(in_undo != null && in_redo != null);
            m_undoAction = in_undo;
            m_redoAction = in_redo;
        }

        public UndoRedoAction(string in_name, string in_property, object in_instance, object in_oldValue, object in_newValue)
            : this
            (
                in_name,
                () => in_instance.GetType().GetProperty(in_property).SetValue(in_instance, in_oldValue),
                () => in_instance.GetType().GetProperty(in_property).SetValue(in_instance, in_newValue)
            )
        { }
    }

    public class UndoRedo
    {
        private bool m_enableAdd = true;
        private readonly ObservableCollection<IUndoRedo> m_undoList = new ObservableCollection<IUndoRedo>();

        private readonly ObservableCollection<IUndoRedo> m_redoList = new ObservableCollection<IUndoRedo>();

        public ReadOnlyObservableCollection<IUndoRedo> UndoList { get; }
        public ReadOnlyObservableCollection<IUndoRedo> RedoList { get; }

        public void Reset()
        {
            m_undoList.Clear();
            m_redoList.Clear();
        }

        public void Add(IUndoRedo cmd)
        {
            if (m_enableAdd)
            {
                m_undoList.Add(cmd);
                m_redoList.Clear();
            }
        }

        public void Undo()
        {
            if (m_undoList.Any())
            {
                var cmd = m_undoList.Last();
                m_undoList.RemoveAt(m_undoList.Count - 1);
                m_enableAdd = false;
                cmd.Undo();
                m_enableAdd = true;
                m_redoList.Insert(0, cmd);
            }
        }

        public void Redo()
        {
            if (m_redoList.Any())
            {
                var cmd = m_redoList.First();
                m_redoList.RemoveAt(0);
                m_enableAdd = false;
                cmd.Redo();
                m_enableAdd = true;
                m_undoList.Add(cmd);
            }
        }

        public UndoRedo()
        {
            UndoList = new ReadOnlyObservableCollection<IUndoRedo>(m_undoList);
            RedoList = new ReadOnlyObservableCollection<IUndoRedo>(m_redoList);
        }
    }
}