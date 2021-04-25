using ImGuiNET;

namespace DotInsideNode
{
    public class EnumeratorsView : TListTableView<IEnumItem>
    {
        KeyNameListDrawer<IEnumItem> m_Drawer;

        public EnumeratorsView(IListController<IEnumItem> controller) : base(controller)
        {
            m_Drawer = new KeyNameListDrawer<IEnumItem>(controller);
            //m_Timer.Reset();
        }

        protected override void DrawItem(int index, IEnumItem tObj, out bool onEvent)
        {
            onEvent = false;

            //Name
            ImGui.TableNextColumn();
            m_Drawer.DrawName(index, tObj, out onEvent);
            //Description
            ImGui.TableNextColumn();
            DrawDescription(index, tObj, out onEvent);
            //Remove Button
            ImGui.TableNextColumn();
            m_Drawer.DrawRemoveButton(index, tObj, out onEvent);
            ImGui.SameLine();
            //Drag Button
            ImGui.Selectable("##EnumeratorsViewDrag" + tObj.Name);
            m_Drawer.DragProc(index, tObj, out onEvent);
        }

        protected void DrawDescription(int index, IEnumItem tObj, out bool onEvent)
        {
            onEvent = false;
            string objDescription = tObj.Description;
            ImGui.InputText("##IEnumItemDescription" + tObj.ID, ref objDescription, 30);
            tObj.Description = objDescription;
        }

        private string[] m_Titles = new[] { "Display Name", "Description", "Close && Drag" };
        protected override string[] Titles => m_Titles;
        protected override string TableLable => "EnumeratorsViewTable";
    }

    class DefaultEnumerationEnumeratorsView : IView
    {
        IListController<IEnumItem> m_Controller = null;
        IView m_EnumeratorsView = null;

        public DefaultEnumerationEnumeratorsView(IListController<IEnumItem> controller)
        {
            m_Controller = controller;
            m_EnumeratorsView = new EnumeratorsView(controller);
        }

        public virtual void Draw()
        {
            //Enumerators Collapsing Header
            if (ImGui.Button("+##EnumeratorsButton"))
            {
                m_Controller.AddItem();
            }
            ImGui.SameLine();
            if (ImGui.CollapsingHeader("Enumerators"))
            {
                m_EnumeratorsView.Draw();
            }
        }
    }

    class DefaultBpTopView : IView
    {
        private IBluePrint m_BluePrint = null;

        public DefaultBpTopView(IBluePrint bp)
        {
            m_BluePrint = bp;
            //m_EnumeratorsView = new EnumeratorsView(m_Controller.Model);
        }

        public virtual void Draw()
        {
            if (ImGui.Button("Save"))
            {
                m_BluePrint.Save(m_BluePrint.AssetPath);
            }
            ImGui.SameLine();
            if (ImGui.Button("Compile"))
            {
                m_BluePrint.Compile();
            }
        }
    }

    class DefaultEnumerationDescriptionView : IView
    {
        string TableTitle => "EnumeratorsDescriptionTable" + m_Controller.ID;
        string DescriptionTitle => "##EnumeratorsDescriptionTable";
        private BP_Enumeration m_Controller = null;

        public DefaultEnumerationDescriptionView(BP_Enumeration enumertion)
        {
            m_Controller = enumertion;
        }

        public virtual void Draw()
        {
            DrawDescription();
        }

        protected virtual void DrawDescription()
        {
            if (ImGui.CollapsingHeader("Description"))
            {
                ImGuiEx.TableView(TableTitle, () =>
                {
                    ImGui.TableNextColumn();
                    ImGui.Text("Enum Description");

                    ImGui.TableNextColumn();
                    string description = m_Controller.Description;
                    ImGui.InputText(DescriptionTitle, ref description, 50);
                    m_Controller.Description = description;

                }, 2);
            }
        }

    }

}
