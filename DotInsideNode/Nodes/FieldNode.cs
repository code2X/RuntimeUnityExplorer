using imnodesNET;
using ImGuiNET;
using System.Reflection;
using System.Collections.Generic;

namespace DotInsideNode
{
    class FieldNode : NodeBase
    {
        public SortedList<string, FieldInfo> classList = new SortedList<string, FieldInfo>();
        FieldInfo m_FieldInfo;

        TextTB m_TextTitleBar = new TextTB("Field");
        TextIC m_TextInput = new TextIC("input");
        TextOC m_TextOutput = new TextOC("output");
        TextSC m_TextStatic = new TextSC("static");

        public FieldNode()
        {
            AddComponet(m_TextTitleBar);
            AddComponet(m_TextInput);
            AddComponet(m_TextOutput);
            AddComponet(m_TextStatic);
        }

        protected override void DrawContent()
        {
        }
    }
}
