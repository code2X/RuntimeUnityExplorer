using imnodesNET;
using ImGuiNET;
using System.Reflection;
using System.Collections.Generic;

namespace DotInsideNode
{
    class MethodEntryNode : ComNodeBase
    {
        string m_Name = "Method";
        TextTB m_TextTitleBar = new TextTB("Method Entry");
        ExecOC m_ExecOC = new ExecOC();

        public override INodeTitleBar GetTitleBarCom() => m_TextTitleBar;
        public override ExecOC GetExecOutCom() => m_ExecOC;

        public MethodEntryNode()
        {
            AddComponet(m_TextTitleBar);
            AddComponet(m_ExecOC);
        }

        public override string Compile()
        {
            string res = "private void " + m_Name + "()";

            return res;
        }

        protected override void DrawContent()
        {
        }
    }
}
