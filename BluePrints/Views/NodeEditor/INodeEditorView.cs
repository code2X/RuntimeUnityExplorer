using imnodesNET;
using System;
using System.Collections.Generic;
using DotInsideLib;
using ImGuiNET;

namespace DotInsideNode
{
    abstract class INodeEditorView
    {
        bool m_Open = true;

        public virtual void DrawWindowContent()
        {
            if (!m_Open)
                return;
            DoNodeEditorStart();
            imnodes.BeginNodeEditor();
            DrawNodeEditorContent();        
            imnodes.EndNodeEditor();
            DoNodeEditorEnd();
        }
        public bool IsOpen
        {
            get => m_Open;
        }
        public void Open() => m_Open = true;
        public void Close() => m_Open = false;

        protected virtual void DoNodeEditorStart() { }
        protected abstract void DrawNodeEditorContent();
        protected virtual void DoNodeEditorEnd() { }
    }

    class NodeEditorBase: INodeEditorView, INodeGraphEditor
    {
        static INodeGraph m_NodeGraph = null;
        public virtual void SubmitGraph(INodeGraph nodeGraph) => m_NodeGraph = nodeGraph;
        protected INodeGraph NodeGraph => m_NodeGraph;

        //public override string WindowName => "NodeEditorBase";

        //using DropAction = Action<INodeGraph>;
        //public delegate void DropAction(INodeGraph bp);
        public event Action<INodeGraphEditor, INodeGraph> OnDropEvent;

        public void AddNode(INode nodeView,bool atMousePos = true)
        {
            m_NodeGraph?.ngNodeManager.AddNode(nodeView, atMousePos);
        }

        protected sealed override void DrawNodeEditorContent()
        {
            m_NodeGraph?.Draw();
            DrawContent();
        }

        protected override void DoNodeEditorStart()
        {
            DoStart();
        }

        protected override void DoNodeEditorEnd() 
        {
            m_NodeGraph?.Update();
            DragDropProc();

            if (m_RightMenu.IsOpen == false)
                m_RightMenu = m_NullPopupMenu;
            else
                m_RightMenu.Draw();
            DoEnd();
        }

        protected virtual void DoStart() { }
        protected virtual void DrawContent() { }
        protected virtual void DoEnd() { }

        protected virtual void DragDropProc()
        {
            if (ImGui.BeginDragDropTarget())
            {
                if (m_NodeGraph != null)
                    OnDropEvent?.Invoke(this,m_NodeGraph);
            }
        }
        IPopupMenu m_NullPopupMenu = new NullPopupMenu();
        IPopupMenu m_RightMenu = new NullPopupMenu();

        void INodeGraphEditor.SubmitRightMenu(IPopupMenu view)
        {
            m_RightMenu = view;
        }
    }

}
