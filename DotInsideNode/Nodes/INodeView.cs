using imnodesNET;
using ImGuiNET;
using System.Collections.Generic;

namespace DotInsideNode
{
    abstract class INodeView
    {
        int m_ID;

        public virtual void DrawNode()
        {
            imnodes.BeginNode(GetID());
            DrawNodeContent();
            imnodes.EndNode();
        }

        public virtual void SetID(int id) => m_ID = id;
        public virtual int GetID() => m_ID;
        protected abstract void DrawNodeContent();
        public virtual void DoNodeEnd() { }

        public virtual string Compile() { return string.Empty; }
    }

    class NodeBase: INodeView
    {
        Dictionary<int,INodeComponent> m_Components = new Dictionary<int, INodeComponent>();
        NodeComponentManager m_NodeComManager = NodeComponentManager.GetInstance();

        public int AddComponet(INodeComponent component)
        {
            int id = m_NodeComManager.AddComponet(component);
            AddMyComponent(id, component);
            return id;
        }

        public int AddComponet(INodeInput component)
        {
            int id = m_NodeComManager.AddComponet(component);
            AddMyComponent(id, component);
            return id;
        }

        public int AddComponet(INodeOutput component)
        {
            int id = m_NodeComManager.AddComponet(component);
            AddMyComponent(id, component);
            return id;
        }

        void AddMyComponent(int id, INodeComponent component)
        {
            component.SetID(id);
            component.SetParent(this);
            m_Components.Add(id, component);
        }

        protected sealed override void DrawNodeContent() 
        {
            foreach (var component in m_Components)
            {                
                component.Value.DrawComponent();
                if (component.Value.GetComponentType() == ComponentType.Input)
                    ImGui.SameLine();
            }
            DrawContent();
        }

        public sealed override void DoNodeEnd()
        {
            foreach (var component in m_Components)
            {
                component.Value.DoComponentEnd();
            }
            DoEnd();
        }

        protected virtual void DrawContent() { }
        protected virtual void DoEnd() { }
    }

    class ComNodeBase : NodeBase
    {
        public virtual INodeTitleBar GetTitleBarCom() => null;
        public virtual ExecIC GetExecInCom() => null;
        public virtual ExecOC GetExecOutCom() => null;
    }

}
