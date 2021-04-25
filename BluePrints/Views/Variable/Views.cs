using ImGuiNET;
using System;
using DotInsideNode;

namespace View.Variable
{
    class VarListView : TListView<IVar>
    {
        protected VarListView()
        { }

        public VarListView(IVisitable<IVar> traverler) : base(traverler)
        { }

        void DrawTooltip(IVar @var)
        {
            if (var.Tooltip != string.Empty &&
                ImGui.IsItemHovered())
            {
                ImGui.SetTooltip(var.Tooltip);
            }
        }

        protected override void DrawListItem(IVar tObj, out bool onEvent)
        {
            onEvent = false;
            base.DrawListItem(tObj, out onEvent);
            DrawTooltip(tObj);
        }

        protected override string GetPayloadType() => "VAR_DRAG";

        INodeGraph m_NodeGraph = null;

    }

    class CollapsingList : IView
    {
        public class ListMenuView : TEnumMenuView<IVar, ListMenuView.EItemEvent>
        {
            public enum EItemEvent
            {
                Delete,
                Duplicate,
            }
        }

        public dnObjectDictionary<IVar> m_DiObjectManager = new dnObjectDictionary<IVar>();
        VarListView m_ListView = null;
        ListMenuView m_ListMenuView = new ListMenuView();

        public VarListView List => m_ListView;
        public ListMenuView ListMenu => m_ListMenuView;

        IVarManager m_VarManager;

        public CollapsingList(IVarManager varManager)
        {
            m_VarManager = varManager;
            m_ListView = new VarListView(varManager);
            m_ListView.MenuDrawer = m_ListMenuView;
        }

        public virtual void Draw()
        {
            //Variables
            if (ImGui.Button("+##Variables Create"))
            {
                m_VarManager.AddVar();
                Console.WriteLine("Variable Create");
            }
            ImGui.SameLine();
            if (ImGui.CollapsingHeader("Local Variables"))
            {
                m_ListView.DrawList();
            }
        }

        public void DefListMenuEventProc(
            ListMenuView.EMenuEvent eMenuEvent,
            ListMenuView.EItemEvent eItemEvent,
            IVar variable)
        {
            switch (eItemEvent)
            {
                case ListMenuView.EItemEvent.Delete:
                    m_VarManager.DeleteVar(variable.Name);
                    break;
                case ListMenuView.EItemEvent.Duplicate:
                    IVar dup = (IVar)variable.Duplicate();
                    m_VarManager.AddVar(dup);
                    break;
            }
        }
    }


}