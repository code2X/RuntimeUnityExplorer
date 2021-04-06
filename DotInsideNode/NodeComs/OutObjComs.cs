using ImGuiNET;
using imnodesNET;
using System;
using System.Reflection;

namespace DotInsideNode
{
    class ObjectOC : INodeOutput
    {
        object m_Object = null;
        Type m_Type = null;
        string m_Name = string.Empty; 

        public ObjectOC(Type type = null, object obj = null)
        {


            m_Object = obj;
            m_Type = type;
        }

        public void SetName(string str) => m_Name = str;
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
            if (m_Type == null)
            {
                ImGui.TextUnformatted("");
            }
            else if(m_Name == string.Empty)
            {
                ImGui.TextUnformatted(m_Type.Name);
            }       
            else
            {
                ImGui.TextUnformatted(m_Type.Name + "  " + m_Name);
            }
        }

        public override bool TryConnectTo(INodeInput component)
        {
            return true;
        }

        public override void OnLinkDropped()
        {
            if(m_Type != null)
                PopupSelectList.GetInstance().Show(MethodTools.GetMethodList(m_Type), OnListSelected);
        }

        void OnListSelected(string selected, int index)
        {
            Logger.Info(selected);
            MethodInfo methodInfo = MethodTools.GetAllMethod(m_Type)[index];
            if (methodInfo == null) return;

            MethodNode endNode = new MethodNode(methodInfo);
            NodeManager.GetInstance().AddNode(endNode);
            LinkManager.GetInstance().TryCreateLink(this, endNode.GetTarget());
        }

        public override object Request(RequestType type) 
        { 
            switch(type)
            {
                case RequestType.InstanceType:
                    return m_Type;
            }
            return null;
        }
    }

}
