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
    }
}