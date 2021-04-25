using System.Collections.Generic;
using ImGuiNET;

namespace DotInsideNode
{
    public abstract class TListView<T> where T : dnObject
    {
        IVisitable<T> m_Traveler;

        protected TListView()
        {}

        public TListView(IVisitable<T> traverler)
        {
            m_Traveler = traverler;
        }

        //MenuDrawer
        public ITMenuView<T> MenuDrawer
        {
            get; set;
        }

        //List Item Event
        public enum EListItemEvent
        {
            SelectItem,
            DoubleSelectItem,
        }
        public delegate void TListAction(EListItemEvent eListItemEvent, T obj);
        public event TListAction OnListItemEvent;

        protected virtual void NotifyEvent(EListItemEvent eListItemEvent,T tObj)
        {
            OnListItemEvent?.Invoke(eListItemEvent, tObj);
        }

        public virtual int MaxItemCount => 10;

        /// <summary>
        /// Interface
        /// </summary>
        public virtual void DrawList()
        {
            if (m_Traveler.TravelItemCount == 0)
                return;

            int lineNum = MATH.Utils.Clamp(m_Traveler.TravelItemCount, 0, MaxItemCount);
            if (lineNum == 0)
                return;

            ImGui.BeginListBox("##ListBox" + typeof(T),
                new Vector2(ImGui.GetColumnWidth(), ImGui.GetTextLineHeightWithSpacing() * lineNum)
                );
            m_Traveler.Visit(Vistor);
            ImGui.EndListBox();
        }

        protected virtual void Vistor(T tObj, out bool onEvent)
        {
            DrawListItem(tObj, out onEvent);

            if (Dragable)
            {
                DragVarProc(tObj, out onEvent);
            }

            //Call menu drawer when drawer is setted
            MenuDrawer?.DrawMenuView(tObj, out onEvent);
        }

        protected virtual void DrawListItem(T tObj,out bool onEvent)
        {
            onEvent = false;
            if (ImGui.Selectable(tObj.Name))
            {
                NotifyEvent(EListItemEvent.SelectItem, tObj);
            }

            // Do stuff on Selectable() double click.                    
            if (ImGui.IsItemHovered() && ImGui.IsMouseDoubleClicked(0))
            {                                                                                                                                                                                                                       
                NotifyEvent(EListItemEvent.DoubleSelectItem, tObj);
            }
        }

        /// <summary>
        /// process drag item action
        /// </summary>
        int dragVarID;
        protected virtual unsafe void DragVarProc(T tObj, out bool onEvent)
        {
            onEvent = false;
            dragVarID = tObj.ID;
            fixed (int* p = &dragVarID)
            {
                System.IntPtr intPtr = new System.IntPtr(p);
                if (ImGui.BeginDragDropSource(ImGuiDragDropFlags.None))
                {
                    // Set payload to carry the index of our item (could be anything)
                    ImGui.SetDragDropPayload(GetPayloadType(), intPtr, sizeof(int));

                    // Display preview (could be anything, e.g. when dragging an image we could decide to display
                    // the filename and a small preview of the image, etc.)
                    ImGui.Text(tObj.Name + ":" + dragVarID);
                    ImGui.EndDragDropSource();
                }
            }
        }

        protected virtual bool Dragable => true;
        protected abstract string GetPayloadType();
    }

}
