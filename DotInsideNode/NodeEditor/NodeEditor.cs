using imnodesNET;
using ImGuiNET;
using System.Collections.Generic;

namespace DotInsideNode
{
    class NodeEditor: NodeEditorBase
    {
        public override string GetWindowName() => "NodeEditorTest";
        MethodEntryNode entryNode = null;
        ReturnNode returnNode = null;

        public NodeEditor() 
        {
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
            AddNode(entryNode,false);
            AddNode(returnNode, false);
            LinkManager.GetInstance().TryCreateLink(entryNode.GetExecOutCom(), returnNode.GetExecInCom());
        }

        public string Compile()
        {
            if(entryNode != null)
                return entryNode.Compile();
            return "";
        }

        protected override void DoEnd()
        {
            List<string> list = new List<string>();
            list.Add("DotPrint - Type of");
            list.Add("DotPrint - Return");
            list.Add("DotPrint - Field");
            list.Add("DotPrint - Set Field");
            list.Add("DotPrint - Bool Var");
            ImGui.OpenPopupOnItemClick(PopupSelectList.GetInstance().GetPopupID());

            if (ImGui.IsMouseDown(ImGuiMouseButton.Right))
            {
                PopupSelectList.GetInstance().Reset(list, OnListSelected);
            }

            PopupSelectList.GetInstance().Draw();
        }

        void OnListSelected(string selected,int index)
        {
            if(index == 0)
            {
                AddNode(new TypeofNode());
            }
            else if (index == 1)
            {
                AddNode(new ReturnNode());
            }
            else if (index == 2)
            {
                AddNode(new FieldNode());
            }
            else if (index == 3)
            {
                AddNode(new FieldSetterNode());
            }
            else if (index == 4)
            {
                AddNode(new BoolVar.Getter());
            }
        }

    }
}
