using ImGuiNET;
using imnodesNET;
using System;
using System.Reflection;

namespace DotInsideNode
{
    class ObjectIC : INodeInput
    {
        object m_Object = null;
        Type m_Type = null;

        public ObjectIC(Type type = null, object obj = null)
        {


            m_Object = obj;
            m_Type = type;
        }

        public object GetObject()
        {
            if (m_Object == null)
            {
                Logger.Warn("ObjectOC object is null");
            }

            return m_Object;
        }
        public void SetObject(object obj) => m_Object = obj;
        public Type GetObjectType()
        {
            if (m_Type == null)
            {
                Logger.Warn("ObjectOC type is null");
            }

            return m_Type;
        }
        public void SetObjectType(Type type) => m_Type = type;

        protected override void DrawContent()
        {
            if (m_Type != null)
                ImGui.TextUnformatted(m_Type.Name);
            else
                ImGui.TextUnformatted("");
        }

        public override bool TryConnectBy(INodeOutput component)
        {
            return true;
        }

        public override void OnLinkDropped()
        {
        }


        public override object Request(RequestType type)
        {
            switch (type)
            {
                case RequestType.InstanceType:
                    return m_Type;
            }
            return null;
        }
    }
}
