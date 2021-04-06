using System;
using System.Collections.Generic;
using imnodesNET;

namespace DotInsideNode
{
    class NodeManager
    {

        NodeManager()
        {}
        static NodeManager instance = new NodeManager();
        public static NodeManager GetInstance() => instance;

        static Random s_Rand = new Random();
        Dictionary<int, INodeView> m_Nodes = new Dictionary<int, INodeView>();

        public void AddNode(INodeView nodeView, bool mousePos = true)
        {
            int id;
            while (m_Nodes.ContainsKey(id = s_Rand.Next())) ;

            nodeView.SetID(id);
            m_Nodes.Add(id, nodeView);

            if(mousePos)
                imnodes.SetNodeScreenSpacePos(id, ImGuiNET.ImGui.GetMousePos());
        }

        public void Draw()
        {
            foreach (var nodeView in m_Nodes)
            {
                nodeView.Value.DrawNode();
            }
        }

        public void DoEnd()
        {
            foreach (var nodeView in m_Nodes)
            {
                nodeView.Value.DoNodeEnd();
            }
        }
    }
}
