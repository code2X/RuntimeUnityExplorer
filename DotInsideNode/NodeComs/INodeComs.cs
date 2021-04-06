using imnodesNET;

namespace DotInsideNode
{
    abstract class INodeComponent
    {
        int m_ID;
        INodeView m_Parent = null;

        public virtual void SetID(int id) => m_ID = id;
        public virtual int GetID() => m_ID;
        public virtual void SetParent(INodeView node) => m_Parent = node;
        public virtual INodeView GetParent() => m_Parent;
        public abstract ComponentType GetComponentType();

        public abstract void DrawComponent();
        public virtual void DoComponentEnd() { }
    }

    enum ComponentType
    {
        TitleBar,
        Input,
        Output,
        Static,
    }

    abstract class INodeTitleBar : INodeComponent
    {
        public override ComponentType GetComponentType() => ComponentType.TitleBar;
        public override void DrawComponent()
        {
            imnodes.BeginNodeTitleBar();
            DrawContent();
            imnodes.EndNodeTitleBar();
        }
        
        protected abstract void DrawContent();
    }

    abstract class INodeStatic : INodeComponent
    {
        public override ComponentType GetComponentType() => ComponentType.Static;
        public override void DrawComponent()
        {
            imnodes.BeginStaticAttribute(GetID());
            DrawContent();
            imnodes.EndStaticAttribute();
        }

        protected abstract void DrawContent();
    }

    interface ILinkEvent
    {
        void OnLinkDropped();
        void OnLinkDestroyed();
        void OnLinkHovered();
        void OnLinkStart();
    }

    enum RequestType
    {
        Instance,
        InstanceType
    }

    enum MessageType
    {
        InstanceChange,
        InstanceTypeChange
    }

    abstract class INodeEventComponent : INodeComponent, ILinkEvent
    {
        public virtual object Request(RequestType type) { return null; }
        public virtual object SendMessage(params string[] msgs) { return null; }
        public virtual object SendMessage(MessageType type) { return null; }
        public virtual void OnLinkDropped() { }
        public virtual void OnLinkDestroyed() { }
        public virtual void OnLinkHovered() { }
        public virtual void OnLinkStart() { }
    }

    abstract class INodeOutput : INodeEventComponent
    {
        public override ComponentType GetComponentType() => ComponentType.Output;
        public override void DrawComponent()
        {
            imnodes.BeginOutputAttribute(GetID(), GetPinShape());
            DrawContent();
            imnodes.EndOutputAttribute();
        }

        protected abstract void DrawContent();
        protected virtual PinShape GetPinShape() => PinShape.Circle;


        public virtual bool TryConnectTo(INodeInput component) { return false; }
    }

    abstract class INodeInput : INodeEventComponent
    {
        public override ComponentType GetComponentType() => ComponentType.Input;
        public override void DrawComponent()
        {
            imnodes.BeginInputAttribute(GetID(), GetPinShape());
            DrawContent();
            imnodes.EndInputAttribute();
        }

        protected abstract void DrawContent();
        protected virtual PinShape GetPinShape() => PinShape.Circle;

        public virtual bool TryConnectBy(INodeOutput component) { return false; }
    }
}
