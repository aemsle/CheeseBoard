using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace BluEditor.Components
{
    public interface IMSComponent
    {
    }

    [DataContract]
    public abstract class Component : ViewModelBase
    {
        public abstract IMSComponent GetMultiSelectComponent(MSObject in_msObject);

        [DataMember]
        public GameObject Owner { get; private set; }

        public Component(GameObject in_gameObject)
        {
            Debug.Assert(in_gameObject != null);
            Owner = in_gameObject;
        }
    }

    public abstract class MSComponent<T> : ViewModelBase, IMSComponent where T : Component
    {
        private bool m_enableUpdates = true;

        public List<T> SelectedComponents { get; }

        protected abstract bool UpdateComponents(string in_propertyName);

        protected abstract bool UpdateMSComponents();

        public void Refresh()
        {
            m_enableUpdates = false;
            UpdateMSComponents();
            m_enableUpdates = true;
        }

        public MSComponent(MSObject in_msObject)
        {
            Debug.Assert(in_msObject?.SelectedObjects?.Any() == true);
            SelectedComponents = in_msObject.SelectedObjects.Select(obj => obj.GetComponent<T>()).ToList();
            PropertyChanged += (s, e) => { if (m_enableUpdates) UpdateComponents(e.PropertyName); };
        }
    }
}