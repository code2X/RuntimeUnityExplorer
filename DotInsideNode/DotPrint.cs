using imnodesNET;
using ImGuiNET;

namespace DotInsideNode
{
    class DotPrint : DotInsideLib.IWindowView
    {
        public override string GetWindowName() => "DotPrint";
        public static ImGuiTableFlags TableFlags = ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Hideable;

        NodeEditor nodeEditor = new NodeEditor();

        DotPrint()
        {
            nodeEditor.CreateMethod();
            ShowWindow();
        }
        static DotPrint instance = new DotPrint();
        public static DotPrint GetInstance() => instance;

        public override void DrawWindowContent()
        {


            if (ImGui.BeginTable("EditorTable", 3, TableFlags))
            {
                DotInsideLib.ImGuiUtils.TableSetupHeaders();
                ImGui.TableSetColumnIndex(0);
                DrawLeft();
                ImGui.TableSetColumnIndex(1);
                DrawEditorTop();
                nodeEditor.DrawWindowContent();
                ImGui.TableSetColumnIndex(2);
                DrawRight();

                ImGui.EndTable();
            }
        }

        void DrawEditorTop()
        {
            if(ImGui.Button("Compile"))
            {
                compileText = nodeEditor.Compile();
            }
            ImGui.SameLine();
            ImGui.Button("Save");
            ImGui.SameLine();
            ImGui.Button("Browse");
        }

        void DrawLeft()
        {
            if( ImGui.Button("Create Function"))
            {
                nodeEditor.CreateMethod();
            }
            ImGui.CollapsingHeader("Functions");

            ImGui.CollapsingHeader("Variables");
        }

        string compileText = "";
        void DrawRight()
        {
            ImGui.InputTextMultiline("", ref compileText, 10000, new Vector2(ImGui.GetColumnWidth(), ImGui.GetTextLineHeight() * 16));
        }

    }
}
