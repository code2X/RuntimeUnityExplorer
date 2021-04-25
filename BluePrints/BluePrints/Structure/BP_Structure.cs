
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotInsideNode
{
    [Serializable]
    [BlueprintClass("Structure")]
    public class BP_Structure: IBP_Structure
    {
        #region MVC
        KeyNameList<IMember> m_Members = new KeyNameList<IMember>();

        [NonSerialized]
        List<IView> m_Views = new List<IView>();
        [NonSerialized]
        IViewsFactory m_ViewsFactory = null;

        [NonSerialized]
        IListController<IMember> m_Controller = null;
        #endregion

        string m_Tooltip = "";

        public override string Tooltip 
        { 
            get => m_Tooltip; 
            set => m_Tooltip = value; 
        }
        public override int MemberCount => 0;

        public BP_Structure()
        {
            ResetViewController();
        }

        #region Deserialized
        [OnDeserializedAttribute]
        protected void OnDeserialized(StreamingContext sc)
        {
            ResetViewController();
        }
        #endregion

        public virtual void ResetViewController()
        {
            m_Controller = new StructControllers(m_Members);
            m_ViewsFactory = new StructViewFactory(this, m_Controller);
            m_Views = m_ViewsFactory.GetViews();
        }

        public override void Draw()
        {
            foreach (IView view in m_Views)
            {
                view.Draw();
            }
        }

        //public void AddItem() => m_StructItemManager.AddMemberItem(new IMember());
        //protected StructItemManager ItemManager => m_StructItemManager;

        /*
        class StructureView : IWindowView
        {
            private Structure m_Controller = null;

            public override string WindowName => "Structure View";
            public override ImGuiWindowFlags WindowFlags => ImGuiWindowFlags.NoCollapse;
            bool m_Open = true;

            public StructureView(Structure enumertion)
            {
                m_Controller = enumertion;
                ShowWindow();
            }

            public override void DrawWindowContent()
            {
                if (!m_Open)
                    return;
                DrawEditor();
                UpdateEditor();
            }
            public bool IsOpen => m_Open;
            public void Open() => m_Open = true;
            public void Close() => m_Open = false;

            protected virtual void DrawEditor()
            {
                if (ImGui.Button("Save"))
                {
                    m_Controller.Save(m_Controller.AssetPath);
                }
                //Structure Collapsing Header
                if (ImGui.Button("+##Structure"))
                {
                    m_Controller.AddItem();
                }
                ImGui.SameLine();
                if (ImGui.CollapsingHeader("Structure"))
                {
                    m_Controller.ItemManager.DrawStructure();
                }

                //Default Values
                if (ImGui.CollapsingHeader("Default Values"))
                {
                    m_Controller.ItemManager.DrawDefaultValue();
                }

            }

            protected virtual void UpdateEditor() { }
        }
        */

    }
}