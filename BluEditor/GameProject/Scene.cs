using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BluEditor.Components;
using System.Windows.Input;
using BluEditor.Utilities;

namespace BluEditor.GameProject
{
    [DataContract]
    public class Scene : ViewModelBase
    {
        private string m_sceneName;

        [DataMember]
        public string SceneName
        {
            get { return m_sceneName; }
            set
            {
                if (m_sceneName != value)
                {
                    m_sceneName = value;
                    OnPropertyChanged(nameof(SceneName));
                }
            }
        }

        [DataMember]
        public Project Project { get; private set; }

        private bool m_isActive;

        [DataMember]
        public bool IsActive
        {
            get { return m_isActive; }
            set
            {
                if (m_isActive != value)
                {
                    m_isActive = value;
                    OnPropertyChanged(nameof(IsActive));
                }
            }
        }

        [DataMember(Name = nameof(GameObjects))]
        private readonly ObservableCollection<GameObject> m_gameObjects = new ObservableCollection<GameObject>();

        public ReadOnlyObservableCollection<GameObject> GameObjects { get; private set; }

        public ICommand AddGameObjectCommand { get; private set; }
        public ICommand RemoveGameObjectCommand { get; private set; }

        private void AddGameObject(GameObject in_gameObject, int index = -1)
        {
            Debug.Assert(!m_gameObjects.Contains(in_gameObject));
            in_gameObject.Active = IsActive;
            if (index == -1)
            {
                m_gameObjects.Add(in_gameObject);
            }
            else
            {
                m_gameObjects.Insert(index, in_gameObject);
            }
        }

        private void RemoveGameObject(GameObject in_gameObject)
        {
            Debug.Assert(m_gameObjects.Contains(in_gameObject));
            in_gameObject.Active = false;
            m_gameObjects.Remove(in_gameObject);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext in_context) // once we juice the shit add the funky actions
        {
            if (m_gameObjects != null)
            {
                GameObjects = new ReadOnlyObservableCollection<GameObject>(m_gameObjects);
                OnPropertyChanged(nameof(GameObjects));
            }

            foreach (GameObject obj in GameObjects)
            {
                obj.Active = true;
            }

            AddGameObjectCommand = new RelayCommand<GameObject>(x =>
            {
                AddGameObject(x);
                int gameObjectIndex = m_gameObjects.Count - 1;
                Project.UndoRedo.Add(new UndoRedoAction(
                    $"Add {x.Name}",
                    () => RemoveGameObject(x),
                    () => AddGameObject(x, gameObjectIndex)
                    ));
            });

            RemoveGameObjectCommand = new RelayCommand<GameObject>(x =>
            {
                var gameObjectIndex = m_gameObjects.IndexOf(x);
                RemoveGameObject(x);
                Project.UndoRedo.Add(new UndoRedoAction(
                    $"Remove {x.Name}",
                    () => AddGameObject(x, gameObjectIndex),
                    () => RemoveGameObject(x)
                    ));
            }); // dont remove GameObjects that are active
        }

        public Scene(Project in_project, string in_sceneName)
        {
            Debug.Assert(in_project != null);
            Project = in_project;
            SceneName = in_sceneName;
            GameObjects = new ReadOnlyObservableCollection<GameObject>(m_gameObjects);
            OnDeserialized(new StreamingContext());
        }
    }
}