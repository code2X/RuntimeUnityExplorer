using imnodesNET;
using System;
using System.Collections.Generic;

namespace DotInsideNode
{
    public interface ILinkManager
    {
        void AddLink(int startCom, int endCom);
        bool IsConnect(int startCom, int endCom);
        void TryCreateLink(int startCom, int endCom);
        void TryCreateLink(INodeOutput startNodeCom, INodeInput endNodeCom);
        bool RemoveLink(int linkId);
        bool RemoveLinkByBegin(int startCom);
        bool RemoveLinkByEnd(int endCom);
    }

    public interface DynamicView : IView
    {
        void Update();
    }

    [Serializable]
    public class LinkManager: DynamicView
    {
        LinkGraph m_Model = new LinkGraph();

        public virtual void Draw()
        {
            m_Model.Draw();
        }

        public virtual void Update()
        {
            CheckLinkCreated();
            CheckLinkStarted();
            CheckLinkDropped();
            CheckLinkDestroyed();
            CheckLinkHovered();
        }

        #region EventDefine
        public enum ELinkEvent
        {
            Created,
            Started,
            Dropped,
            Destroyed,
            Hovered
        }
        public delegate void LinkAction(ELinkEvent eLinkEvent, int? beginId,int? endId,int? linkId);
        public event LinkAction OnLinkEvent;
        #endregion

        #region EventNotify
        protected virtual void NotifyLinkCreatedEvent(int startAttr, int endAttr) => OnLinkEvent?.Invoke(ELinkEvent.Created, startAttr, endAttr, null);
        protected virtual void NotifyLinkStartedEvent(int startAttr) => OnLinkEvent?.Invoke(ELinkEvent.Started, startAttr, null,null);
        protected virtual void NotifyLinkDroppedEvent(int startAttr) => OnLinkEvent?.Invoke(ELinkEvent.Dropped, startAttr, null, null);
        protected virtual void NotifyLinkDestroyedEvent(int startAttr, int endAttr, int linkId) => OnLinkEvent?.Invoke(ELinkEvent.Destroyed, startAttr, endAttr, linkId);
        protected virtual void NotifyLinkHoveredEvent(int startAttr, int endAttr,int linkId) => OnLinkEvent?.Invoke(ELinkEvent.Hovered, startAttr, endAttr, linkId);
        protected virtual void NotifyLinkHoveredEvent(int linkId)
        {
            bool res = false;
            if(res = m_Model.TryGetLink(linkId,out LinkPair linkPair))
            {
                NotifyLinkHoveredEvent(linkPair.start, linkPair.end, linkId);
            }
            else
            {
                Logger.Info("Hovered link not exist");
            }           
        }
        #endregion

        #region EventCheck
        protected virtual void CheckLinkCreated()
        {
            int startAttr = -1, endAttr = -1;
            if (imnodes.IsLinkCreated(ref startAttr, ref endAttr))
            {
                NotifyLinkCreatedEvent(startAttr, endAttr);
            }
        }

        protected virtual void CheckLinkStarted()
        {
            int startAttr = -1;
            if(imnodes.IsLinkStarted(ref startAttr))
            {
                NotifyLinkStartedEvent(startAttr);
            }
        }

        protected virtual void CheckLinkDropped()
        {
            int startAttr = -1;
            if (imnodes.IsLinkDropped(ref startAttr))
            {
                NotifyLinkDroppedEvent(startAttr);
            }
        }

        protected virtual void CheckLinkDestroyed()
        {
            int linkId = -1;
            if (imnodes.IsLinkDestroyed(ref linkId))
            {
                Assert.IsTrue(RemoveLink(linkId));
            }
        }

        protected virtual void CheckLinkHovered()
        {
            int linkId = -1;
            if (imnodes.IsLinkHovered(ref linkId))
            {
                NotifyLinkHoveredEvent(linkId);
            }
        }
        #endregion

        #region LinkManager
        public virtual void AddLink(LinkPair linkPair) => m_Model.AddLink(linkPair);
        public virtual bool IsConnect(LinkPair linkPair) => m_Model.IsConnect(linkPair);
        public virtual bool IsConnect(int startAttr, int endAttr) => IsConnect(new LinkPair(startAttr, endAttr));
        public virtual void TryCreateLink(LinkPair linkPair) => TryCreateLink(linkPair.start, linkPair.end);
        public virtual void TryCreateLink(int startAttr, int endAttr) => NotifyLinkCreatedEvent(startAttr, endAttr);
        public virtual void TryCreateLink(INodeOutput startNodeCom, INodeInput endNodeCom) => NotifyLinkCreatedEvent(startNodeCom.ID, endNodeCom.ID);

        public virtual bool RemoveLink(int linkId)
        {
            if (m_Model.RemoveLink(linkId, out LinkPair linkPair))
            {
                NotifyLinkDestroyedEvent(linkPair.start, linkPair.end,linkId);
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool RemoveLinkByBegin(int beginAttr)
        {
            if(m_Model.TryGetLinkIDByBegin(beginAttr, out List<int> linkIds))
            {
                foreach(int linkId in linkIds)
                {
                    Assert.IsTrue(RemoveLink(linkId));
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool RemoveLinkByEnd(int endAttr)
        {
            if (m_Model.TryGetLinkIDByEnd(endAttr, out List<int> links))
            {
                foreach (int linkId in links)
                {
                    Assert.IsTrue(RemoveLink(linkId));
                }
                return true;
            }
            else
            {
                return false;
            }           
        }
        #endregion
    }

}
