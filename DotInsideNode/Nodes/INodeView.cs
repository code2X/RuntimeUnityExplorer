using imnodesNET;
using System;
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
        static Dictionary<int, INodeComponent> g_Components = new Dictionary<int, INodeComponent>();
        static Dictionary<int, INodeInput> g_InComponents = new Dictionary<int, INodeInput>();
        static Dictionary<int, INodeOutput> g_OutComponents = new Dictionary<int, INodeOutput>();
        static Random s_Random = new Random();

        Dictionary<int,INodeComponent> m_Components = new Dictionary<int, INodeComponent>();

        public static bool ConnectComponet(int start, int end)
        {
            INodeInput inCom;
            INodeOutput outCom;
            if (g_InComponents.TryGetValue(end, out inCom) && g_OutComponents.TryGetValue(start, out outCom))
            {
                return inCom.ConnectBy(outCom) && outCom.ConnectTo(inCom);
            }
            return false;
        }

        public int AddComponet(INodeComponent component)
        {
            int id;
            while (g_Components.ContainsKey(id = s_Random.Next())) ;

            component.SetID(id);
            component.SetParent(this);
            g_Components.Add(id,component);
            m_Components.Add(id, component);
            return id;
        }

        public void AddComponet(INodeInput component)
        {
            int id = AddComponet((INodeComponent)component);
            g_InComponents.Add(id, component);
        }

        public void AddComponet(INodeOutput component)
        {
            int id = AddComponet((INodeComponent)component);
            g_OutComponents.Add(id, component);
        }

        protected sealed override void DrawNodeContent() 
        {
            foreach (var component in m_Components)
            {
                component.Value.DrawComponent();
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
