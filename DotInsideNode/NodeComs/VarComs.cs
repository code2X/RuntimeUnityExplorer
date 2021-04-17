using ImGuiNET;

namespace DotInsideNode
{
    [SingleConnect]
    class VarIC : INodeInput
    {
        ComObject m_Object = new ComObject();
        IVarBase m_Var;
        LinkManager m_LinkManager = LinkManager.Instance;
        INodeOutput m_Connect = null;

        public VarIC(IVarBase variable)
        {
            m_Var = variable;
        }

        public ComObject GetObject() => m_Object;

        protected override void DrawContent()
        {
            ImGui.TextUnformatted(m_Var.VarName);
        }

        public override bool TryConnectBy(INodeOutput component)
        {
            m_Connect = component;            
            return true;
        }

        public override void OnLinkDropped()
        {
            
        }

        public void ChageVar(IVarBase variable)
        {
            m_Var = variable;
        }

        public override object Request(RequestType type)
        {
            switch (type)
            {
                case RequestType.InstanceType:
                    return m_Object.Type;
            }
            throw new RequestTypeError(type, m_Connect);
        }
    }

    class VarOC : INodeOutput
    {
        ComObject m_Object = new ComObject();
        IVarBase m_Var;
        bool m_ShowName;
        LinkManager m_LinkManager = LinkManager.Instance;
        INodeInput m_Connect = new NullIC();

        public VarOC(IVarBase variable,bool show_name = true)
        {
            m_ShowName = show_name;
            m_Var = variable;
        }

        public ComObject GetObject() => m_Object;

        protected override void DrawContent()
        {
            if(m_ShowName)
                ImGui.TextUnformatted(m_Var.VarName);
        }

        public override bool TryConnectTo(INodeInput component)
        {
            m_Connect = component;
            return true;
        }

        public void ChageVar(IVarBase variable)
        {
            m_Var = variable;
            m_Connect.SendMessage(MessageType.InstanceTypeChange);
        }

        public override object Request(RequestType type)
        {
            switch (type)
            {
                case RequestType.InstanceType:
                    return m_Var.VarType;
                case RequestType.InstanceObject:
                    return m_Var.VarValue;
            }
            throw new RequestTypeError(type, m_Connect);
        }
    }

}
