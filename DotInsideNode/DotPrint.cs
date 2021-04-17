using imnodesNET;
using ImGuiNET;
using System;
using System.Collections.Generic;

namespace DotInsideNode
{
    class DotPrint : DotInsideLib.IWindowView
    {
        public override string GetWindowName() => "DotPrint";
        public static ImGuiTableFlags TableFlags = ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Hideable;

        NodeEditor m_NodeEditor = new NodeEditor();
        VarManager m_VarManager = VarManager.Instance;

        DotPrint()
        {
            m_NodeEditor.CreateMethod();
            ShowWindow();
        }
        static DotPrint instance = new DotPrint();
        public static DotPrint GetInstance() => instance;

        public override void DrawWindowContent()
        {


            if (ImGui.BeginTable("EditorTable", 3, TableFlags))
            {
                ImGuiUtils.TableSetupHeaders();
                ImGui.TableSetColumnIndex(0);
                DrawLeft();
                ImGui.TableSetColumnIndex(1);
                DrawEditorTop();
                m_NodeEditor.DrawWindowContent();
                ImGui.TableSetColumnIndex(2);
                DrawRight();

                ImGui.EndTable();
            }
        }

        void DrawEditorTop()
        {
            if(ImGui.Button("Compile"))
            {
                compileText = m_NodeEditor.Compile();
            }
            ImGui.SameLine();
            ImGui.Button("Save");
            ImGui.SameLine();
            ImGui.Button("Browse");
            ImGui.SameLine();
            if(ImGui.Button("Play"))
            {
                try
                {
                    m_NodeEditor.Play();
                }
                catch(Exception exp)
                {
                    compileText = exp.ToString();
                }
            }
        }

        List<IVarBase> varList = new List<IVarBase>();
        void DrawLeft()
        {
            //Function
            if( ImGui.Button("+##Function Create"))
            {
                m_NodeEditor.CreateMethod();
            }
            ImGui.SameLine();
            ImGui.CollapsingHeader("Functions");

            //Variables
            if (ImGui.Button("+##Variables Create"))
            {
                m_VarManager.AddVar(new BoolVar());
                Console.WriteLine("Variables Create");
            }
            ImGui.SameLine();
            if(ImGui.CollapsingHeader("Variables"))
            {
                m_VarManager.DrawVarList();
            }
        }

        string compileText = "";
        void DrawRight()
        {
            m_VarManager.DrawVarInfo();
            ImGui.InputTextMultiline("", ref compileText, 10000, new Vector2(ImGui.GetColumnWidth(), ImGui.GetTextLineHeight() * 16));
        }

    }
}
