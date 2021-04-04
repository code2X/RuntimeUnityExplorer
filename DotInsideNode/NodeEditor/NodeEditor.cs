using imnodesNET;
using ImGuiNET;

namespace DotInsideNode
{
    class NodeEditor: NodeEditorBase
    {
        public override string GetWindowName() => "NodeEditorTest";
        MethodEntryNode entryNode = null;
        ReturnNode returnNode = null;

        public NodeEditor() 
        {
            //AddNode(new MethodEntryNode());
            //AddNode(new ReturnNode());
            ShowWindow();
        }
        protected override void DrawContent()
        {
        }

        public void CreateMethod()
        {
            if (entryNode != null) 
                return;

            entryNode = new MethodEntryNode();
            returnNode = new ReturnNode();
            AddNode(entryNode);
            AddNode(returnNode);
            AddLink(entryNode.GetExecOutCom().GetID(), returnNode.GetExecInCom().GetID());
        }

        public string Compile()
        {
            if(entryNode != null)
                return entryNode.Compile();
            return "";
        }

        string m_SearchText = "";
        protected override void DoEnd()
        {
            if (ImGui.BeginPopupContextItem("item context menu"))
            {
                ImGui.InputTextWithHint("选择节点", "search", ref m_SearchText, 20);
                if (ImGui.Selectable("Class"))
                {
                    AddNode(new ClassNode());
                }
                if (ImGui.Selectable("Field"))
                {

                }
                ImGui.EndPopup();
            }
        }

    }
}
