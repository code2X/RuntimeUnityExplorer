using ImGuiNET;
using System.Collections.Generic;

namespace DotInsideNode
{
    abstract class IFunctionGraph
    {
        public abstract void OpenGraph();
        public abstract void CloseGraph();
        public abstract void Execute();
    }

    class FunctionGraph: IFunctionGraph
    {
        LinkManager m_LinkManager = null;
        NodeManager m_NodeManager = null;
        VarManager m_LocalVarManager = null;
        NodeComponentManager m_NodeComponentManager = null;
        Dictionary<int, Vector2> m_NodePositions = null;
        bool isInit = false;
        ComNodeBase m_EntryPoint = null;

        IFunction m_Function = null;

        public FunctionGraph(IFunction function)
        {
            Assert.IsNotNull(function);
            m_Function = function;

            m_LinkManager = new LinkManager();
            m_NodeManager = new NodeManager();
            m_LocalVarManager = new VarManager();
            m_NodeComponentManager = new NodeComponentManager(m_LinkManager);           
        }

        public override void OpenGraph()
        {
            LinkManager.Instance = m_LinkManager;
            NodeComponentManager.Instance = m_NodeComponentManager;
            NodeManager.Instance = m_NodeManager;

            if (m_NodePositions != null)
                m_NodeManager.NodeEditorPostions = m_NodePositions;

            if(isInit == false)
            {
                m_EntryPoint = m_Function.GetNewFunctionEntry();
                NodeManager.Instance.AddNode(m_EntryPoint, false);
                isInit = true;
            }               
        }

        public override void CloseGraph()
        {
            m_NodePositions = m_NodeManager.NodeEditorPostions;
        }

        public void OnFunctionDelete()
        {

        }

        public override void Execute() 
        {
            m_EntryPoint?.Play(0);
        }
    }
}
