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

    class ComboCompo : INodeStatic
    {
        public delegate void SelectHandler(string item);
        public event SelectHandler OnSelect;

        int m_CurItem;
        IList<string> m_Items;
        string m_SearchText = "";

        public ComboCompo(IList<string> items, int current_item = 0)
        {            
            m_Items = items;
            m_CurItem = current_item;
        }

        public string GetCurItem() => m_Items[m_CurItem];

        protected override void DrawContent()
        {
            if (ImGui.BeginCombo(m_Items[m_CurItem], "", ImGuiComboFlags.NoPreview))
            {
                ImGui.InputTextWithHint("select class", "search", ref m_SearchText, 20);
                for (int n = 0; n < m_Items.Count; n++)
                {
                    //seach text
                    if (m_Items[n].IndexOf(m_SearchText) == -1)
                        continue;

                    //select
                    bool is_selected = (m_CurItem == n);
                    if (ImGui.Selectable(m_Items[n], is_selected))
                        DoSelect(n);
                       
                    // Set the initial focus when opening the combo (scrolling + keyboard navigation focus)
                    if (is_selected)
                        ImGui.SetItemDefaultFocus();
                }
                ImGui.EndCombo();
            }
        }

        void DoSelect(int index)
        {
            m_CurItem = index;
            OnSelect(GetCurItem());
        }
    }

    class TypeOC : INodeOutput
    {
        Type m_Type;

        public TypeOC(Type type)
        {
            m_Type = type;
        }
        public void SetType(Type type) => m_Type = type;

        protected override void DrawContent()
        {
            ImGui.TextUnformatted(m_Type.Name);
        }

        public override void DoComponentEnd() 
        {
            int id = GetID();
            if (imnodes.IsLinkDropped(ref id))
            {
                Console.WriteLine("start");
                ImGui.OpenPopup(GetID().ToString());
            }

            var methodInfos = typeof(Type).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            var propertyInfos = typeof(Type).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            var fieldInfos = typeof(Type).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            if (ImGui.BeginPopup(GetID().ToString()))
            {
                ImGui.Text("Methods");
                foreach (var i in methodInfos)
                {
                    ImGui.Button(i.Name);
                }

                ImGui.Text("Propertys");
                foreach (var i in propertyInfos)
                {
                    ImGui.Button(i.Name);
                }

                ImGui.Text("Fields");
                foreach (var i in fieldInfos)
                {
                    ImGui.Button(i.Name);
                }

                ImGui.EndPopup();
            }
        }

    }

}
