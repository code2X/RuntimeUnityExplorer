using ImGuiNET;
using System;
using System.Collections.Generic;

namespace DotInsideNode
{
    [Serializable]
    public class NodeComponentManager
    {
        public Dictionary<int, INodeComponent> m_Components = new Dictionary<int, INodeComponent>();
        public Dictionary<int, INodeComponent> m_LeftComponents = new Dictionary<int, INodeComponent>();
        public Dictionary<int, INodeComponent> m_RightComponents = new Dictionary<int, INodeComponent>();

        [NonSerialized]
        INodeGraph m_pNodeGraph = null;
        [NonSerialized]
        INode m_pNode = null;

        public NodeComponentManager(INodeGraph nodeGraph, INode node)
        {
            m_pNodeGraph = nodeGraph;
            m_pNode = node;
        }

        public int AddComponet(INodeComponent component)
        {
            int id = m_pNodeGraph.ngNodeComponentManager.AddComponet(component);
            FillComponent(id, component);
            m_Components.Add(id, component);
            return id;
        }

        public int AddComponet(INodeInput component)
        {
            int id = m_pNodeGraph.ngNodeComponentManager.AddComponet(component);
            FillComponent(id, component);
            m_LeftComponents.Add(id, component);
            return id;
        }

        public int AddComponet(INodeOutput component)
        {
            int id = m_pNodeGraph.ngNodeComponentManager.AddComponet(component);
            FillComponent(id, component);
            m_RightComponents.Add(id, component);
            return id;
        }

        void FillComponent(int id, INodeComponent component)
        {
            Assert.IsNotNull(m_pNode);

            component.ID = id;
            component.ParentNode = m_pNode;
            component.NodeGraph = m_pNodeGraph;
        }

        public void DrawComponent()
        {
            foreach (var component in m_Components)
            {
                component.Value.DrawComponent();
            }

            int sameLineCount = m_LeftComponents.Count < m_RightComponents.Count ? m_LeftComponents.Count : m_RightComponents.Count;
            var leftEnumerator = m_LeftComponents.GetEnumerator();
            var rightEnumerator = m_RightComponents.GetEnumerator();

            for (int i = 0; i < sameLineCount; ++i)
            {
                leftEnumerator.MoveNext();
                rightEnumerator.MoveNext();

                leftEnumerator.Current.Value.DrawComponent();
                ImGui.SameLine();
                rightEnumerator.Current.Value.DrawComponent();
            }

            for (int i = 0; i < m_LeftComponents.Count - sameLineCount; ++i)
            {
                leftEnumerator.MoveNext();
                leftEnumerator.Current.Value.DrawComponent();
            }

            for (int i = 0; i < m_RightComponents.Count - sameLineCount; ++i)
            {
                rightEnumerator.MoveNext();
                rightEnumerator.Current.Value.DrawComponent();
            }
        }

        public void PostfixProc()
        {
            foreach (var component in m_Components)
            {
                component.Value.DoComponentEnd();
            }
        }

        public void Clear()
        {
            foreach (var com in m_Components)
            {
                m_pNodeGraph.ngNodeComponentManager.RemoveComponent(com.Value);
            }
            foreach (var com in m_LeftComponents)
            {
                m_pNodeGraph.ngNodeComponentManager.RemoveComponent(com.Value);
            }
            foreach (var com in m_RightComponents)
            {
                m_pNodeGraph.ngNodeComponentManager.RemoveComponent(com.Value);
            }
            m_Components.Clear();
            m_LeftComponents.Clear();
            m_RightComponents.Clear();
        }
    }
}
