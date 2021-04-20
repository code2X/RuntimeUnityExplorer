using imnodesNET;
using ImGuiNET;
using System.Collections.Generic;

namespace DotInsideNode
{
    public abstract class INode: diObject
    {
        StyleManager m_StyleManager = new StyleManager();

        public StyleManager Style
        {
            get => m_StyleManager;
        }
        public virtual bool Removable => true;

        public virtual void DrawNode()
        {
            m_StyleManager.PushColorStyle();
            imnodes.BeginNode(this.ID);
            DrawNodeContent();
            imnodes.EndNode();
            m_StyleManager.PopColorStyle();
        }

        //Logic Flow
        protected virtual void DrawNodeContent() { }
        public virtual void DoNodeEnd() { }

        //Event
        public enum EEvent
        {
            Clicked,
            Selected,
            Hovered,
            Detroyed
        }

        public virtual void EventProc(EEvent eEvent) { }

        //Runtime
        public virtual string Compile() { return string.Empty; }
        public virtual object Play(int callerID, params object[] objects)
        {
            NodeLogger.ExceInfo(this);
            return ExecNode(callerID, objects);
        }
        protected virtual object ExecNode(int callerID, params object[] objects) => null; 

        //Request
        protected class RequestTypeError : System.Exception
        {
            public RequestTypeError(ERequest type, INodeComponent connect = null)
                :
                base("Error Request Type: " + type + ", Connect:" + connect)
            { }
        }

        public virtual object Request(ERequest type) => throw new System.NotImplementedException();
    }

    public class NullNode : INode
    {}

    public class NodeBase: INode
    {
        Dictionary<int,INodeComponent> m_Components = new Dictionary<int, INodeComponent>();
        Dictionary<int, INodeComponent> m_LeftComponents = new Dictionary<int, INodeComponent>();
        Dictionary<int, INodeComponent> m_RightComponents = new Dictionary<int, INodeComponent>();

        NodeComponentManager m_NodeComManager => NodeComponentManager.Instance;

        public int AddComponet(INodeComponent component)
        {
            int id = m_NodeComManager.AddComponet(component);
            FillComponent(id, component);
            m_Components.Add(id, component);
            return id;
        }

        public int AddComponet(INodeInput component)
        {
            int id = m_NodeComManager.AddComponet(component);
            FillComponent(id, component);
            m_LeftComponents.Add(id, component);
            return id;
        }

        public int AddComponet(INodeOutput component)
        {
            int id = m_NodeComManager.AddComponet(component);
            FillComponent(id, component);
            m_RightComponents.Add(id, component);
            return id;
        }

        void FillComponent(int id, INodeComponent component)
        {
            component.ID = id;
            component.ParentNode = this;
        }

        //Node Flow
        protected sealed override void DrawNodeContent() 
        {
            foreach (var component in m_Components)
            {
                component.Value.DrawComponent();
            }

            int sameLineCount = m_LeftComponents.Count < m_RightComponents.Count ? m_LeftComponents.Count : m_RightComponents.Count;
            var leftEnumerator = m_LeftComponents.GetEnumerator();
            var rightEnumerator = m_RightComponents.GetEnumerator();

            for(int i = 0; i < sameLineCount;++i)
            {
                leftEnumerator.MoveNext();
                rightEnumerator.MoveNext();

                leftEnumerator.Current.Value.DrawComponent();
                ImGui.SameLine();
                rightEnumerator.Current.Value.DrawComponent();
            }

            for (int i = 0; i < m_LeftComponents.Count - sameLineCount; ++i)
            {
                leftEnumerator.MoveNext();
                leftEnumerator.Current.Value.DrawComponent();
            }

            for (int i = 0; i < m_RightComponents.Count - sameLineCount; ++i)
            {
                rightEnumerator.MoveNext();
                rightEnumerator.Current.Value.DrawComponent();
            }

            //ENodeComponent prevType = ENodeComponent.Null;
            //m_Components.GetEnumerator();
            //foreach (var component in m_Components)
            //{
            //    if (component.Value.ComponentType is ENodeComponent.Input && prevType == ENodeComponent.Input)
            //        ImGui.NewLine();
            //
            //    component.Value.DrawComponent();
            //
            //    if (component.Value.ComponentType == ENodeComponent.Input)
            //        ImGui.SameLine();
            //    prevType = component.Value.ComponentType;
            //}
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

        //Event
        public override void EventProc(EEvent eEvent) => DefEventProc(eEvent);
        protected virtual void DefEventProc(EEvent eEvent)
        {
            switch(eEvent)
            {
                case EEvent.Detroyed:
                    OnNodeDetroyed();
                    break;
            }
        }

        void OnNodeDetroyed()
        {
            foreach(var com in m_Components)
            {
                m_NodeComManager.RemoveComponent(com.Value);
            }
            foreach (var com in m_LeftComponents)
            {
                m_NodeComManager.RemoveComponent(com.Value);
            }
            foreach (var com in m_RightComponents)
            {
                m_NodeComManager.RemoveComponent(com.Value);
            }
            m_Components.Clear();
            m_LeftComponents.Clear();
            m_RightComponents.Clear();
        }

    }

    public class ComNodeBase : NodeBase
    {
        public virtual INodeTitleBar GetTitleBarCom() => throw new System.NotImplementedException();
        public virtual ExecIC GetExecInCom() => throw new System.NotImplementedException();
        public virtual ExecOC GetExecOutCom() => throw new System.NotImplementedException();
    }
}
