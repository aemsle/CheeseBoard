using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BluEditor.Components
{
    [DataContract]
    public class Transform : Component
    {
        private Vector3 m_position = Vector3.Zero;

        [DataMember]
        public Vector3 Position
        {
            get { return m_position; }
            set
            {
                if (m_position != value)
                {
                    m_position = value;
                    OnPropertyChanged(nameof(Position));
                }
            }
        }

        private Vector3 m_rotation = Vector3.Zero;

        [DataMember]
        public Vector3 Rotation
        {
            get { return m_rotation; }
            set
            {
                if (m_rotation != value)
                {
                    m_rotation = value;
                    OnPropertyChanged(nameof(Rotation));
                }
            }
        }

        private Vector3 m_scale = new Vector3(1f, 1f, 1f);

        [DataMember]
        public Vector3 Scale
        {
            get { return m_scale; }
            set
            {
                if (m_scale != value)
                {
                    m_scale = value;
                    OnPropertyChanged(nameof(Scale));
                }
            }
        }

        public Transform(GameObject in_gameObject) : base(in_gameObject)
        {
        }
    }
}