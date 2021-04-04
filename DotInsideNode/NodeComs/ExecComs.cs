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

        protected override void DrawContent()
        {
            ImGui.TextUnformatted("Exec");
        }

        public override bool ConnectBy(INodeOutput component) 
        {
            m_ConnectBy = component;
            Console.WriteLine(component);
            return true; 
        }

        protected override PinShape GetPinShape() => PinShape.Quad;
    }

    class ExecOC : INodeOutput
    {
        INodeInput m_ConnectTo = null;

        public INodeInput GetConnect() => m_ConnectTo;

        protected override void DrawContent()
        {
            ImGui.TextUnformatted("Exec");
        }

        public override bool ConnectTo(INodeInput component) 
        {
            m_ConnectTo = component;
            Console.WriteLine("ConnectTo");
            return true; 
        }

        protected override PinShape GetPinShape() => PinShape.Quad;
    }
}
