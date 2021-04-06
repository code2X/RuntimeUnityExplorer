using ImGuiNET;
using imnodesNET;
using System;
using System.Collections.Generic;
using DotInsideLib;
using System.Reflection;

namespace DotInsideNode
{
    class ExecIC : INodeInput
    {
        INodeOutput m_ConnectBy = null;

        public INodeOutput GetConnect() => m_ConnectBy;
        public INodeView GetConnectNode()
        {
            if(m_ConnectBy != null)
            {
                return m_ConnectBy.GetParent();
            }
            return null;
        }

        protected override void DrawContent()
        {
            ImGui.TextUnformatted("Exec");
        }

        public override bool TryConnectBy(INodeOutput component) 
        {
            if (component.GetType().Name != typeof(ExecOC).Name)
                return false;

            m_ConnectBy = component;
            component.OnLinkStart();
            Console.WriteLine("ExecIC ConnectBy");
            return true; 
        }

        public override void OnLinkDestroyed()
        {
            Console.WriteLine("ExecIC Link Destroyed");
            m_ConnectBy = null;
        }

        public override void OnLinkStart()
        {
            Console.WriteLine("ExecIC Link Started");
            LinkManager.GetInstance().TryRemoveLinkByEnd(GetID());
        }

        protected override PinShape GetPinShape() => PinShape.Triangle;
    }

    class ExecOC : INodeOutput
    {
        INodeInput m_ConnectTo = null;

        public INodeInput GetConnect() => m_ConnectTo;
        public INodeView GetConnectNode()
        {
            if (m_ConnectTo != null)
            {
                return m_ConnectTo.GetParent();
            }
            return null;
        }

        protected override void DrawContent()
        {
            ImGui.TextUnformatted("Exec");
        }

        public override bool TryConnectTo(INodeInput component) 
        {
            if (component.GetType().Name != typeof(ExecIC).Name)
                return false;

            m_ConnectTo = component;
            component.OnLinkStart();
            Console.WriteLine("ExecOC ConnectTo");
            return true; 
        }

        public override void OnLinkDestroyed()
        {
            Console.WriteLine("ExecOC Link Destroyed");
            m_ConnectTo = null;
        }

        public override void OnLinkStart() 
        {
            Console.WriteLine("ExecOC Link Started");
            LinkManager.GetInstance().TryRemoveLinkByStart(GetID());
        }

        public override void OnLinkDropped()
        {
            Console.WriteLine("ExecOC Link Dropped");
        }

        protected override PinShape GetPinShape() => PinShape.Triangle;
    }

}
