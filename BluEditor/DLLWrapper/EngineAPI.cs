using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BluEditor.Components;

namespace BluEditor.EngineAPIStructs
{
    [StructLayout(LayoutKind.Sequential)]
    public class TransformDescriptor
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class GameObjectDescriptor
    {
        public TransformDescriptor transformDesc = new TransformDescriptor();
    }
}

namespace BluEditor.DLLWrapper
{
    public static class EngineAPI
    {
        private const string m_dllName = "EngineDLL.dll";

        [DllImport(m_dllName)]
        private static extern int CreateGameObject(EngineAPIStructs.GameObjectDescriptor desc);

        public static int CreateGameObject(GameObject in_gameObject)
        {
            EngineAPIStructs.GameObjectDescriptor desc = new EngineAPIStructs.GameObjectDescriptor();

            { // transform component
                Transform transformComponent = in_gameObject.GetComponent<Transform>();
                desc.transformDesc.Position = transformComponent.Position;
                desc.transformDesc.Rotation = transformComponent.Rotation;
                desc.transformDesc.Scale = transformComponent.Scale;
            }

            return CreateGameObject(desc);
        }

        [DllImport(m_dllName)]
        private static extern void RemoveGameObject(int in_ID);

        public static void RemoveGameObject(GameObject in_gameObject)
        {
            RemoveGameObject(in_gameObject.ObjectID);
        }

        [DllImport(m_dllName)]
        public static extern void Shutdown();
    }
}