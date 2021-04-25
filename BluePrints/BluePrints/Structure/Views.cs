using ImGuiNET;

namespace DotInsideNode
{
    public class KeyNameListDrawer<T> where T:dnObject
    {
        IListController<T> m_Controller = null;
        Timer m_Timer = new Timer();

        public KeyNameListDrawer(IListController<T> controller)
        {
            m_Controller = controller;
        }

        public void DrawName(int index, T tObj, out bool onEvent)
        {
            onEvent = false;
            string objName = tObj.Name;
            ImGui.InputText("##StructureViewName" + tObj.ID, ref objName, 30);
            if (objName != tObj.Name)
            {
                m_Controller.Rename(index, objName);
            }
        }

        public void DrawRemoveButton(int index, T tObj, out bool onEvent)
        {
            onEvent = false;
            if (ImGui.Button("X##" + tObj.ID))
            {
                Assert.IsTrue(m_Controller.RemoveItem(tObj));
                onEvent = true;
            }
        }

        public void DragProc(int index, T tObj, out bool onEvent)
        {
            onEvent = false;
            if (ImGui.IsItemActive() && !ImGui.IsItemHovered())
            {
                int n_next = index + (ImGui.GetMouseDragDelta(0).y < 0.0f ? -1 : 1);
                if (m_Timer.Span.TotalMilliseconds < 200)
                    return;
                else
                    m_Timer.Reset();

                if (m_Controller.Exchange(index, n_next))
                {
                    Logger.Info(tObj.Name + ":" + n_next.ToString());
                    ImGui.ResetMouseDragDelta();
                }
            }
        }
    }

    public class StructureView : TListTableView<IMember>
    {
        KeyNameListDrawer<IMember> m_Drawer;

        public StructureView(IListController<IMember> controller) : base(controller)
        {
            diContainer.InitClassList();
            m_Drawer = new KeyNameListDrawer<IMember>(controller);
        }

        protected override void DrawItem(int index, IMember tObj, out bool onEvent)
        {
            onEvent = false;
            ImGui.TableNextColumn();
            m_Drawer.DrawName(index, tObj,out onEvent);

            ImGui.TableNextColumn();
            DrawType(index, tObj, out onEvent);

            ImGui.TableNextColumn();
            m_Drawer.DrawRemoveButton(index, tObj, out onEvent);
            ImGui.SameLine();
            ImGui.Selectable("##EnumeratorsViewArrowButton" + tObj.Name);
            m_Drawer.DragProc(index, tObj, out onEvent);
        }

        protected void DrawType(int index, IMember tObj, out bool onEvent)
        {
            onEvent = false;
            tObj.Editor.DrawMemberType();
            ImGui.SameLine();
            tObj.Editor.DrawContainerType();
        }

        private string[] m_Titles = new[] { "Member Name", "Type && Container", "Close && Drag" };
        protected override string[] Titles => m_Titles;
        protected override string TableLable => "StructureViewTable";
    }

    public class DefaultValuesView : TListTableView<IMember>
    {
        public DefaultValuesView(IListController<IMember> controller) : base(controller)
        {

        }

        protected override void DrawItem(int index, IMember obj, out bool onEvent)
        {
            onEvent = false;

            ImGui.TableNextColumn();
            ImGui.Text(obj.Name);

            ImGui.TableNextColumn();
            obj.Container.DrawContainerValue(obj.Name);
        }

        private string[] m_Titles = new[] { "Member Name", "Default Value" };
        protected override string[] Titles => m_Titles;
        protected override string TableLable => "DefaultValuesViewTable";
    }

    class V_StructureCollapseStructure : IView
    {
        IBP_Structure m_Structure = null;
        IListController<IMember> m_Controller = null;
        IView m_vStructure = null;

        string TooltipTitle => "Tooltip##V_StructureCollapseStructure";

        public V_StructureCollapseStructure(IBP_Structure structure, IListController<IMember> controller)
        {
            m_Structure = structure;
            m_Controller = controller;
            m_vStructure = new StructureView(controller);
        }

        public virtual void Draw()
        {
            //Structure Collapsing Header
            if (ImGui.Button("+##V_StructureCollapseStructure"))
            {
                m_Controller.AddItem();
            }
            ImGui.SameLine();
            if (ImGui.CollapsingHeader("Structure"))
            {
                DrawTooltip();
                m_vStructure.Draw();
            }
        }

        protected void DrawTooltip()
        {
            string tooltip = m_Structure.Tooltip;
            ImGui.InputText(TooltipTitle, ref tooltip, 50);
            m_Structure.Tooltip = tooltip;
        }
    }

    class V_StructuresCollapseDefaultValues : IView
    {
        IView m_vDefaultValues = null;

        public V_StructuresCollapseDefaultValues(IListController<IMember> controller)
        {
            m_vDefaultValues = new DefaultValuesView(controller);
        }

        public virtual void Draw()
        {
            //Default Values
            if (ImGui.CollapsingHeader("Default Values" + "##V_StructuresCollapseDefaultValues"))
            {
                m_vDefaultValues.Draw();
            }
        }
    }

}
