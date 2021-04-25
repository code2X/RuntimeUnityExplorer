using ImGuiNET;
using System;
using DotInsideNode;                 

namespace BP.FunctionLibrary
{
    using FuncsCollapse = View.Function.DefaultCollapsingList;
    using VarsCollapse = View.Variable.CollapsingList;

    class DefaultController: IView
    {
        BP_FunctionLibrary m_FunctionLibrary = null;
        #region Model View
        FunctionManager m_Model;

        Column3View m_MainView = new Column3View();
        PrintRightView m_RightView = null;
        #endregion

        class LeftView : IView
        {
            public FuncsCollapse FunctionList = null;
            public VarsCollapse VarList = null;

            public void Draw()
            {
                FunctionList.Draw();
                VarList?.Draw();
            }
        }
        LeftView m_LeftView = new LeftView();

        class MiddleView : IView
        {   
            public TabBarView TabBar = new TabBarView();
            public DefaultBpTopView BPTop = null;
            public NodeEditorView NodeEditor = new NodeEditorView();

            public void Draw()
            {
                TabBar.Draw();
                BPTop.Draw();
                NodeEditor.Draw();
            }
        }
        MiddleView m_MiddleView = new MiddleView();

        public DefaultController(BP_FunctionLibrary functionLibrary, FunctionManager model)
        {

            m_FunctionLibrary = functionLibrary;
            m_Model = model;

            m_LeftView.FunctionList = new FuncsCollapse(m_Model);
            m_LeftView.FunctionList.List.OnListItemEvent += FuncListProc;
            m_LeftView.FunctionList.ListMenu.OnMenuEvent += FuncListEventProc;

            m_MiddleView.TabBar.OnTabEvent += TabEventProc;
            m_MiddleView.BPTop = new DefaultBpTopView(m_FunctionLibrary);
            m_MiddleView.NodeEditor.OnDropEvent += NodeEditorDropEventProc;

            m_RightView = new PrintRightView();

            ResetViews();
        }

        PopupVarGetSetController popupVarGetSetController = null;
        private void NodeEditorDropEventProc(INodeGraphEditor editor,INodeGraph graph)
        {
            m_LeftView.FunctionList.OnNodeEditorDrop(editor,graph);
            popupVarGetSetController?.OnNodeEditorDrop(editor, graph);

            //m_LeftView.VarList?.OnNodeEditorDrop(editor, graph);
        }

        protected virtual void FuncListEventProc(
            FuncsCollapse.ListMenuView.EMenuEvent eMenuEvent,
            FuncsCollapse.ListMenuView.EItemEvent eItemEvent, 
            IFunction function)
        {
            switch (eItemEvent)
            {
                case FuncsCollapse.ListMenuView.EItemEvent.Open_Graph:
                    m_MiddleView.TabBar?.OpenTab(function);
                    break;
                case FuncsCollapse.ListMenuView.EItemEvent.Open_in_New_Tab:
                    m_MiddleView.TabBar?.OpenNewTab(function);
                    break;
                case FuncsCollapse.ListMenuView.EItemEvent.Call:
                    function.Execute(0, null, out object[] outParams);
                    break;
                case FuncsCollapse.ListMenuView.EItemEvent.Delete:
                    //m_VarListView = new NullView();
                    m_Model.DeleteFunction(function.ID);
                    break;
            }
        }

        protected virtual void FuncListProc(
            TListView<IFunction>.EListItemEvent eListItemEvent, 
            IFunction function)
        {
            switch (eListItemEvent)
            {
                case TListView<IFunction>.EListItemEvent.SelectItem:
                    m_RightView.Submit(function.Draw);
                    break;
                case TListView<IFunction>.EListItemEvent.DoubleSelectItem:
                    Logger.Info(function.Name);
                    //ResetVarListView((VarManager)function.LocalVar);
                    m_MiddleView.TabBar?.OpenTab(function);
                    break;
            }
        }

        protected virtual void TabEventProc(TabBarView.ETabEvent eTabEvent, dnObject obj)
        {
            IFunction function = (IFunction)obj;
            switch (eTabEvent)
            {
                case TabBarView.ETabEvent.Open:
                    function.OpenGraph(m_MiddleView.NodeEditor);
                    m_MiddleView.NodeEditor?.Open();
                    ResetVarListView((VarManager)function.LocalVar);
                    break;
                case TabBarView.ETabEvent.Close:
                    function.CloseGraph();
                    m_MiddleView.NodeEditor?.Close();
                    break;
            }
        }

        protected virtual void ResetVarListView(VarManager varManager)
        {
            popupVarGetSetController = new PopupVarGetSetController(varManager);

            VarsCollapse varsCollapse = new VarsCollapse(varManager);
            varsCollapse.List.OnListItemEvent += VarListItemEventProc;
            varsCollapse.ListMenu.OnMenuEvent += varsCollapse.DefListMenuEventProc;
            m_LeftView.VarList = varsCollapse;
        }

        private void VarListItemEventProc(
            TListView<IVar>.EListItemEvent eListItemEvent, 
            IVar variable)
        {
            switch (eListItemEvent)
            {
                case TListView<IVar>.EListItemEvent.SelectItem:
                    m_RightView.Submit(variable.Draw);
                    break;
            }
        }

        protected virtual void ResetViews()
        {
            m_MainView.Clear();

            m_MainView.LeftView = m_LeftView;
            m_MainView.MidView = m_MiddleView;
            m_MainView.RightView = m_RightView;
        }

        public void Draw()
        {
            m_MainView.Draw();
        }

    }

}
