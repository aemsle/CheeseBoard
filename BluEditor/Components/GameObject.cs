using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.ObjectModel;
using BluEditor.GameProject;
using System.Runtime.Serialization;
using System.Windows.Input;
using BluEditor.Utilities;
using BluEditor.DLLWrapper;

namespace BluEditor.Components
{
    [DataContract]
    [KnownType(typeof(Transform))]
    public class GameObject : ViewModelBase
    {
        private int m_objectID = ID.INVALID_ID;

        public int ObjectID
        {
            get { return m_objectID; }
            set
            {
                if (m_objectID != value)
                {
                    m_objectID = value;
                    OnPropertyChanged(nameof(ObjectID));
                }
            }
        }

        private bool m_active;

        public bool Active
        {
            get { return m_active; }
            set
            {
                if (m_active != value)
                {
                    m_active = value;
                    if (m_active)
                    {
                        ObjectID = EngineAPI.CreateGameObject(this);
                        Debug.Assert(ID.IsValid(m_objectID));
                    }
                    else
                    {
                        EngineAPI.RemoveGameObject(this);
                    }

                    OnPropertyChanged(nameof(Active));
                }
            }
        }

        private bool m_enbaled = true;

        [DataMember]
        public bool Enabled
        {
            get { return m_enbaled; }
            set
            {
                if (m_enbaled != value)
                {
                    m_enbaled = value;
                    OnPropertyChanged(nameof(Enabled));
                }
            }
        }

        private string m_name;

        [DataMember]
        public string Name
        {
            get { return m_name; }
            set
            {
                if (m_name != value)
                {
                    m_name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        [DataMember]
        public Scene ParentScene { get; set; }

        [DataMember(Name = nameof(Components))]
        private readonly ObservableCollection<Component> m_components = new ObservableCollection<Component>();

        public ReadOnlyObservableCollection<Component> Components { get; private set; }

        public Component GetComponent(Type in_type) => Components.FirstOrDefault(c => c.GetType() == in_type);

        public T GetComponent<T>() where T : Component => GetComponent(typeof(T)) as T;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext in_context)
        {
            if (m_components != null)
            {
                Components = new ReadOnlyObservableCollection<Component>(m_components);
                OnPropertyChanged(nameof(Components));
            }
        }

        public GameObject(Scene in_scene)
        {
            Debug.Assert(in_scene != null);
            ParentScene = in_scene;
            m_components.Add(new Transform(this));
            OnDeserialized(new StreamingContext());
        }
    }

    public abstract class MSObject : ViewModelBase
    {
        // Enables updates on selected game objects, used to compare
        // mixed values without falling into a recursion loop
        private bool m_enableUpdates = true;

        // use nullable types to handle mixed values
        private bool? m_enbaled = true;

        public bool? Enabled
        {
            get { return m_enbaled; }
            set
            {
                if (m_enbaled != value)
                {
                    m_enbaled = value;
                    OnPropertyChanged(nameof(Enabled));
                }
            }
        }

        private string m_name;

        public string Name
        {
            get { return m_name; }
            set
            {
                if (m_name != value)
                {
                    m_name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private readonly ObservableCollection<IMSComponent> m_components = new ObservableCollection<IMSComponent>();
        public ReadOnlyObservableCollection<IMSComponent> Components { get; private set; }

        public List<GameObject> SelectedObjects { get; }

        public static float? GetMixedValue(List<GameObject> in_gameObjects, Func<GameObject, float> in_getProperty)
        {
            float? value = in_getProperty(in_gameObjects.First());

            foreach (GameObject? gameObject in in_gameObjects.Skip(1))
            {
                if (!value.Approx(in_getProperty(gameObject))) return null;
            }

            return value;
        }

        public static bool? GetMixedValue(List<GameObject> in_gameObjects, Func<GameObject, bool> in_getProperty)
        {
            bool? value = in_getProperty(in_gameObjects.First());

            foreach (GameObject? gameObject in in_gameObjects.Skip(1))
            {
                if (value != (in_getProperty(gameObject))) return null;
            }

            return value;
        }

        public static string GetMixedValue(List<GameObject> in_gameObjects, Func<GameObject, string> in_getProperty)
        {
            string? value = in_getProperty(in_gameObjects.First());

            foreach (GameObject? gameObject in in_gameObjects.Skip(1))
            {
                if (value != (in_getProperty(gameObject))) return null;
            }

            return value;
        }

        protected virtual bool UpdateGameObjects(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Enabled): SelectedObjects.ForEach(x => x.Enabled = Enabled.Value); return true;
                case nameof(Name): SelectedObjects.ForEach(x => x.Name = Name); return true;
            }
            return false;
        }

        protected virtual bool UpdateMSGameObjects()
        {
            Enabled = GetMixedValue(SelectedObjects, new Func<GameObject, bool>(x => x.Enabled));
            Name = GetMixedValue(SelectedObjects, new Func<GameObject, string>(x => x.Name));

            return true;
        }

        public void Refresh()
        {
            m_enableUpdates = false;
            UpdateMSGameObjects();
            m_enableUpdates = true;
        }

        public MSObject(List<GameObject> in_gameObjects)
        {
            Debug.Assert(in_gameObjects?.Any() == true);
            Components = new ReadOnlyObservableCollection<IMSComponent>(m_components);
            SelectedObjects = in_gameObjects;
            PropertyChanged += (s, e) => { if (m_enableUpdates) UpdateGameObjects(e.PropertyName); };
        }
    }

    public class MSGameObject : MSObject
    {
        public MSGameObject(List<GameObject> in_gameObjects) : base(in_gameObjects)
        {
            Refresh();
        }
    }
}