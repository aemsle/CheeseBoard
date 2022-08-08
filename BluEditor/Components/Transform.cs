using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using BluEditor.Utilities;
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

        public override IMSComponent GetMultiSelectComponent(MSObject in_msObject) => new MSTransform(in_msObject);

        public Transform(GameObject in_gameObject) : base(in_gameObject)
        {
        }
    }

    internal sealed class MSTransform : MSComponent<Transform>
    {
        // Translation

        private float? m_posX;

        public float? PosX
        {
            get { return m_posX; }
            set
            {
                if (!m_posX.Approx(value))
                {
                    m_posX = value;
                    OnPropertyChanged(nameof(PosX));
                }
            }
        }

        private float? m_posY;

        public float? PosY
        {
            get { return m_posY; }
            set
            {
                if (!m_posY.Approx(value))
                {
                    m_posY = value;
                    OnPropertyChanged(nameof(PosY));
                }
            }
        }

        private float? m_posZ;

        public float? PosZ
        {
            get { return m_posZ; }
            set
            {
                if (!m_posZ.Approx(value))
                {
                    m_posZ = value;
                    OnPropertyChanged(nameof(PosZ));
                }
            }
        }

        // Rotation

        private float? m_rotX;

        public float? RotX
        {
            get { return m_rotX; }
            set
            {
                if (!m_rotX.Approx(value))
                {
                    m_rotX = value;
                    OnPropertyChanged(nameof(RotX));
                }
            }
        }

        private float? m_rotY;

        public float? RotY
        {
            get { return m_rotY; }
            set
            {
                if (!m_rotY.Approx(value))
                {
                    m_rotY = value;
                    OnPropertyChanged(nameof(RotY));
                }
            }
        }

        private float? m_rotZ;

        public float? RotZ
        {
            get { return m_rotZ; }
            set
            {
                if (!m_rotZ.Approx(value))
                {
                    m_rotZ = value;
                    OnPropertyChanged(nameof(RotZ));
                }
            }
        }

        // Scale

        private float? m_scaX;

        public float? ScaX
        {
            get { return m_scaX; }
            set
            {
                if (!m_scaX.Approx(value))
                {
                    m_scaX = value;
                    OnPropertyChanged(nameof(ScaX));
                }
            }
        }

        private float? m_scaY;

        public float? ScaY
        {
            get { return m_scaY; }
            set
            {
                if (!m_scaY.Approx(value))
                {
                    m_scaY = value;
                    OnPropertyChanged(nameof(ScaY));
                }
            }
        }

        private float? m_scaZ;

        public float? ScaZ
        {
            get { return m_scaZ; }
            set
            {
                if (!m_scaZ.Approx(value))
                {
                    m_scaZ = value;
                    OnPropertyChanged(nameof(ScaZ));
                }
            }
        }

        protected override bool UpdateComponents(string in_propertyName)
        {
            switch (in_propertyName)
            {
                case nameof(PosX):
                case nameof(PosY):
                case nameof(PosZ):
                    SelectedComponents.ForEach(c => c.Position = new Vector3(m_posX ?? c.Position.X, m_posY ?? c.Position.Y, m_posZ ?? c.Position.Z));
                    return true;

                case nameof(RotX):
                case nameof(RotY):
                case nameof(RotZ):
                    SelectedComponents.ForEach(c => c.Rotation = new Vector3(m_rotX ?? c.Rotation.X, m_rotY ?? c.Rotation.Y, m_rotZ ?? c.Rotation.Z));
                    return true;

                case nameof(ScaX):
                case nameof(ScaY):
                case nameof(ScaZ):
                    SelectedComponents.ForEach(c => c.Scale = new Vector3(m_scaX ?? c.Scale.X, m_scaY ?? c.Scale.Y, m_scaZ ?? c.Scale.Z));
                    return true;
            }
            return false;
        }

        protected override bool UpdateMSComponents()
        {
            PosX = MSObject.GetMixedValue(SelectedComponents, new Func<Transform, float>(x => x.Position.X));
            PosY = MSObject.GetMixedValue(SelectedComponents, new Func<Transform, float>(x => x.Position.Y));
            PosZ = MSObject.GetMixedValue(SelectedComponents, new Func<Transform, float>(x => x.Position.Z));

            RotX = MSObject.GetMixedValue(SelectedComponents, new Func<Transform, float>(x => x.Rotation.X));
            RotY = MSObject.GetMixedValue(SelectedComponents, new Func<Transform, float>(x => x.Rotation.Y));
            RotZ = MSObject.GetMixedValue(SelectedComponents, new Func<Transform, float>(x => x.Rotation.Z));

            ScaX = MSObject.GetMixedValue(SelectedComponents, new Func<Transform, float>(x => x.Scale.X));
            ScaY = MSObject.GetMixedValue(SelectedComponents, new Func<Transform, float>(x => x.Scale.Y));
            ScaZ = MSObject.GetMixedValue(SelectedComponents, new Func<Transform, float>(x => x.Scale.Z));

            return true;
        }

        public MSTransform(MSObject in_msObjetc) : base(in_msObjetc)
        {
            Refresh();
        }
    }
}