using ImGuiNET;
using System;
using DotInsideNode;

namespace View.Function
{
    class FuncList : TListView<IFunction>, NodeEditorDroppable
    {
        IFunctionManager m_Manager;

        public FuncList(IFunctionManager manager) : base(manager)
        {
            m_Manager = manager;
            //NodeEditorBase.OnDropEvent += new NodeEditorBase.DropAction(OnFunctionDragDrop);
        }

        void DrawTooltip(IFunction function)
        {
            if (function.Description != string.Empty &&
                ImGui.IsItemHovered())
            {
                ImGui.SetTooltip(function.Description);
            }
        }

        protected override void DrawListItem(IFunction tObj, out bool onEvent)
        {
            base.DrawListItem(tObj, out onEvent);
            DrawTooltip(tObj);
        }

        public void OnNodeEditorDrop(INodeGraphEditor nodeGraphEditor, INodeGraph nodeGraph)
        {
            OnFunctionDragDrop(nodeGraph);
        }

        unsafe void OnFunctionDragDrop(INodeGraph bp)
        {
            ImGuiPayloadPtr pPayload = ImGui.AcceptDragDropPayload("FUNCTION_DRAG");
            if (pPayload.NativePtr != null)
            {
                int fucntionID = *(int*)pPayload.Data;
                CreateFunctionCallNode(bp, fucntionID);
            }
        }

        void CreateFunctionCallNode(INodeGraph bp, int functionID)
        {
            IFunction function = m_Manager.GetFunctionByID(functionID);
            Assert.IsNotNull(function);
            bp.ngNodeManager.AddNode(function.GetNewFunctionCall(bp));
        }

        protected override string GetPayloadType() => "FUNCTION_DRAG";
    }

    class DefaultCollapsingList : IView, NodeEditorDroppable
    {
        public class ListMenuView : TEnumMenuView<IFunction, ListMenuView.EItemEvent>
        {
            public enum EItemEvent
            {
                Open_Graph,
                Open_in_New_Tab,
                Call,
                Delete,
                Duplicate,
            }
        }

        [NonSerialized]
        FuncList m_ListView = null;
        [NonSerialized]
        ListMenuView m_ListMenuView = new ListMenuView();

        public FuncList List => m_ListView;
        public ListMenuView ListMenu => m_ListMenuView;

        IFunctionManager m_FunctionManager;

        public DefaultCollapsingList(IFunctionManager functionManager)
        {
            m_FunctionManager = functionManager;
            m_ListView = new FuncList(functionManager);
            m_ListView.MenuDrawer = m_ListMenuView;
        }

        public virtual void Draw()
        {
            //Function
            if (ImGui.Button("+##Function Create"))
            {
                m_FunctionManager.AddFunction();
                Console.WriteLine("IFunction Create");
            }
            ImGui.SameLine();
            if (ImGui.CollapsingHeader("Functions"))
            {
                m_ListView.DrawList();
            }
        }

        public void OnNodeEditorDrop(INodeGraphEditor nodeGraphEditor, INodeGraph nodeGraph)
        {
            ((NodeEditorDroppable)m_ListView).OnNodeEditorDrop(nodeGraphEditor, nodeGraph);
        }
    }

}
