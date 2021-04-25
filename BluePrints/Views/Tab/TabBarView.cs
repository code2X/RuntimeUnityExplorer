using ImGuiNET;
using System.Collections.Generic;

namespace DotInsideNode
{
    public class TabBarView: IView
    {
        public class TabItem
        {
            public dnObject obj = null;
            public bool state = false;

            public TabItem(dnObject obj, bool state)
            {
                this.obj = obj;
                this.state = state;
            }
        }

        protected Dictionary<int, TabItem> m_ID2Item = new Dictionary<int, TabItem>();

        public int TabCount => m_ID2Item.Count;

        //MenuDrawer
        public ITMenuView<dnObject> MenuDrawer
        {
            get; set;
        }

        //Event
        public enum ETabEvent
        {
            Open,
            Close,
            Remove
        }

        public delegate void TabAction(ETabEvent eTabEvent, dnObject obj);
        public event TabAction OnTabEvent;

        protected virtual int GetNewVarID()
        {
            int index = 0;
            while (m_ID2Item.ContainsKey(index))
            {
                ++index;
            }

            return index;
        }

        public int? OpenTab(dnObject obj)
        {
            if(ContainObject(obj))
            {
                return null;
            }
            else
            {
                return OpenNewTab(obj);
            }               
        }

        public bool ContainObject(dnObject obj)
        {
            foreach (var pair in m_ID2Item)
            {
                if (pair.Value.obj.Name == obj.Name)
                    return true;
            }
            return false;
        }

        public int? OpenNewTab(dnObject obj)
        {
            int id = GetNewVarID();
            m_ID2Item.Add(id, new TabItem(obj, true));
            return id;
        }      

        public virtual void Draw()
        {
            if (m_ID2Item.Count == 0)
                return;
            bool onMenuEvent = false;

            if (ImGui.BeginTabBar("##TabBarViewTopTab",ImGuiTabBarFlags.AutoSelectNewTabs | ImGuiTabBarFlags.Reorderable))
            {
                foreach (var pair in m_ID2Item)
                {
                    if (pair.Value.state && ImGui.BeginTabItem(pair.Value.obj.Name + "##" + pair.Key,ref pair.Value.state))
                    {
                        SelecteTab(pair.Key,pair.Value.obj);
                        ImGui.EndTabItem();
                    }

                    //Call menu drawer when drawer is setted
                    MenuDrawer?.DrawMenuView(pair.Value.obj, out onMenuEvent);
                    if (onMenuEvent) break;
                }
                ImGui.EndTabBar();

                TabStateProc();
            }
        }

        protected virtual void TabStateProc()
        {
            //Process tab whether closed
            foreach (var pair in m_ID2Item)
            {
                if (pair.Value.state == false)
                {
                    if (RemoveTab(pair.Key))
                    {
                        break;
                    }
                }
            }
        }

        public virtual bool ContainTab(int tabID) => m_ID2Item.ContainsKey(tabID);
        int? m_PrevSelectID = null;
        protected virtual bool SelecteTab(int tabID,dnObject obj)
        {
            Assert.IsNotNull(obj);

            if(tabID != m_PrevSelectID)
            {
                if (ContainTab(tabID) == false)
                    return false;

                TabClosedProc();
                NotifyTabEvent(ETabEvent.Open, tabID, obj);

                m_PrevSelectID = tabID;
            }           
            return true;
        }

        public virtual bool RemoveTab(int tabID)
        {
            if (m_ID2Item.TryGetValue(tabID, out TabItem removeObject))
            {
                bool res;
                if (res = m_ID2Item.Remove(tabID))
                {
                    NotifyTabEvent(ETabEvent.Remove, tabID, removeObject.obj);
                }
                if (TabCount == 0)
                    m_PrevSelectID = null;
                return res;
            }
            return false;
        }

        protected virtual void TabClosedProc()
        {           
            if (m_PrevSelectID != null &&
                m_ID2Item.TryGetValue(m_PrevSelectID.Value, out TabItem CloseObject))
            {
                NotifyTabEvent(ETabEvent.Close, m_PrevSelectID.Value, CloseObject.obj);
            }
        }

        /// <summary>
        /// Tab Event
        /// </summary>
        protected virtual void NotifyTabEvent(ETabEvent eTabEvent, int tabID, dnObject obj)
        {
            Logger.Info(eTabEvent + " Tab Actoin:  " + obj.Name);
            OnTabEvent?.Invoke(eTabEvent, obj);
        }
    }

}
