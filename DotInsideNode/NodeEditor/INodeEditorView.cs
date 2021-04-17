using imnodesNET;
using System;
using System.Collections.Generic;
using DotInsideLib;

namespace DotInsideNode
{
    abstract class INodeEditorView : IWindowView
    {
        public override void DrawWindowContent()
        {
            DoNodeEditorStart();
            imnodes.BeginNodeEditor();
            DrawNodeEditorContent();
            imnodes.EndNodeEditor();
            DoNodeEditorEnd();
        }

        protected virtual void DoNodeEditorStart() { }
        protected abstract void DrawNodeEditorContent();
        protected virtual void DoNodeEditorEnd() { }
    }

    class NodeEditorBase: INodeEditorView
    {
        LinkManager m_LinkManager = LinkManager.Instance;
        NodeManager m_NodeManager = NodeManager.Instance;

        public void AddNode(INode nodeView,bool mosuePos = true)
        {
            m_NodeManager.AddNode(nodeView, mosuePos);
        }

        protected sealed override void DrawNodeEditorContent()
        {
            m_NodeManager.Draw();
            m_LinkManager.Draw();
            DrawContent();
        }

        protected override void DoNodeEditorStart()
        {
            DoStart();
        }

        protected override void DoNodeEditorEnd() 
        {
            m_NodeManager.Update();
            m_LinkManager.Update();
            DoEnd();
        }

        protected virtual void DoStart() { }
        protected virtual void DrawContent() { }
        protected virtual void DoEnd() { }
    }

}
