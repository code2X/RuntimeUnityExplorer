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

    class LinkPool
    {
        public struct LinkPair
        {
            public int start, end;

            public LinkPair(int start, int end)
            {
                this.start = start;
                this.end = end;
            }
        }

        static Dictionary<int,LinkPair> s_Links = new Dictionary<int, LinkPair>();
        static Random s_Rand = new Random();

        public void AddLink(LinkPair linkPair)
        {
            int id;
            while (s_Links.ContainsKey(id = s_Rand.Next())) ;
            s_Links.Add(id, linkPair);
        }

        public void Draw()
        {
            foreach (var id2link in s_Links)
            {
                imnodes.Link(id2link.Key, id2link.Value.start, id2link.Value.end);
            }
        }

        public bool RemoveLink(int link_id)
        {
            return s_Links.Remove(link_id);
        }
    }

    class NodeEditorBase: INodeEditorView
    {
        static HashSet<int> s_NodeIdSet = new HashSet<int>();
        static Random s_Rand = new Random();

        Dictionary<int, INodeView> m_NodeViews = new Dictionary<int, INodeView>();
        LinkPool linkPool = new LinkPool();

        public void AddNode(INodeView nodeView)
        {
            int id;
            while (s_NodeIdSet.Contains(id = s_Rand.Next())) ;

            nodeView.SetID(id);
            s_NodeIdSet.Add(id);   
            m_NodeViews.Add(id, nodeView);

            //imnodes.SetNodeScreenSpacePos(id, ImGuiNET.ImGui.GetMousePosOnOpeningCurrentPopup());
        }

        protected sealed override void DrawNodeEditorContent()
        {
            foreach (var nodeView in m_NodeViews)
            {
                nodeView.Value.DrawNode();
            }

            linkPool.Draw();
            DrawContent();
        }

        protected override void DoNodeEditorStart()
        {
            DoStart();
        }

        protected override void DoNodeEditorEnd() 
        {
            foreach (var nodeView in m_NodeViews)
            {
                nodeView.Value.DoNodeEnd();
            }

            CheckLinkCreate();
            RemoveLink();
            DoEnd();
        }

        void CheckLinkCreate()
        {
            int start_attr = -1, end_attr = -1;
            if (imnodes.IsLinkCreated(ref start_attr, ref end_attr))
            {
                AddLink(new LinkPool.LinkPair(start_attr, end_attr));
            }
        }

        public void AddLink(LinkPool.LinkPair linkPair)
        {
            NodeBase.ConnectComponet(linkPair.start, linkPair.end);
            linkPool.AddLink(linkPair);
        }

        public void AddLink(int start_attr, int end_attr)
        {
            AddLink(new LinkPool.LinkPair(start_attr, end_attr));
        }

        void RemoveLink()
        {
            int link = -1;
            imnodes.GetSelectedLinks(ref link);
            if (link != -1 && ImGuiNET.ImGui.IsKeyPressed((int)Keys.A))
            {
                linkPool.RemoveLink(link);
                imnodes.ClearLinkSelection();
            }
        }

        protected virtual void DoStart() { }
        protected virtual void DrawContent() { }
        protected virtual void DoEnd() { }
    }
}
