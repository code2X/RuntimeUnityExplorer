using ImGuiNET;
using imnodesNET;
using System;
using System.Collections.Generic;
using DotInsideLib;
using System.Reflection;

namespace DotInsideNode
{

    class TextTB : INodeTitleBar
    {
        string m_Text;

        public TextTB(string text = "")
        {
            SetText(text);
        }

        public void SetText(string text) => m_Text = text; 

        protected override void DrawContent()
        {
            ImGui.TextUnformatted(m_Text);
        }
    }

    class TextOC : INodeOutput
    {
        string m_Text;

        public TextOC(string text = "")
        {
            m_Text = text;
        }
        public void SetText(string text) => m_Text = text;

        protected override void DrawContent()
        {
            ImGui.TextUnformatted(m_Text);
        }
    }

    class TextIC : INodeInput
    {
        string m_Text;

        public TextIC(string text = "")
        {
            m_Text = text;
        }
        public void SetText(string text) => m_Text = text;

        protected override void DrawContent()
        {
            ImGui.TextUnformatted(m_Text);
        }
    }

    class TextSC : INodeStatic
    {
        string m_Text;

        public TextSC(string text = "")
        {
            m_Text = text;
        }
        public void SetText(string text) => m_Text = text;

        protected override void DrawContent()
        {
            ImGui.TextUnformatted(m_Text);
        }
    }

}
