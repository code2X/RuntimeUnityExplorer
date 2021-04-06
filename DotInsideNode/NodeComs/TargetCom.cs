using ImGuiNET;
using imnodesNET;
using System;
using System.Collections.Generic;
using DotInsideLib;
using System.Reflection;

namespace DotInsideNode
{
    class TargetIC : INodeInput
    {
        object m_Target = null;
        Type m_TargetType = null;
        INodeOutput m_ConnectBy = null;
        //Type m_Type;
        public delegate void TypeHandler(Type type);
        public event TypeHandler OnSetTargetType;

        public TargetIC()
        {
        }
        public object GetTarget() => m_Target;
        public void SetTarget(object target) => m_Target = target;
        public Type GetTargetType() => m_TargetType;
        public void SetTargetType(Type type)
        {
            if(OnSetTargetType != null)
                OnSetTargetType(type);
            m_TargetType = type;
        }

        protected override void DrawContent()
        {
            ImGui.TextUnformatted("Target");
        }

        public override void OnLinkStart()
        {
            LinkManager.GetInstance().TryRemoveLinkByEnd(GetID());
        }

        public override void DoComponentEnd()
        {

        }

        public override bool TryConnectBy(INodeOutput component)
        {
            m_ConnectBy = component;
            SetTargetType((Type)m_ConnectBy.Request(RequestType.InstanceType) );
            return true;
        }

        public string Compile()
        {
            return m_ConnectBy.GetParent().Compile();
        }

    }
}
