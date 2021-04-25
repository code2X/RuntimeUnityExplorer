using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotInsideNode
{
    [Serializable]
    class FlowControlNode: ComNodeBase
    {
        protected static uint m_TitleBarColor = StyleManager.GetU32Color(160, 160, 160);

        protected TextTB m_TextTitleBar = new TextTB("");
        protected ExecIC m_ExecIC = new ExecIC();

        public FlowControlNode(INodeGraph nodeGraph) : base(nodeGraph)
        {
            m_ExecIC.Text = "";
            AddComponet(m_TextTitleBar);
            AddComponet(m_ExecIC);

            Style.AddStyle(StyleManager.StyleType.TitleBar, m_TitleBarColor);
        }
    }
}
