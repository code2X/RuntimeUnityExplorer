
using System.Collections.Generic;
using ImGuiNET;

namespace DotInsideNode
{
    public abstract class TTableView<T> where T : dnObject
    {
        IVisitable<T> m_Traveler;

        public TTableView(IVisitable<T> traverler)
        {
            m_Traveler = traverler;
        }

        public void Draw()
        {
            ImGuiEx.TableView("##TableView" + typeof(T).Name, () =>
            {
                m_Traveler.Visit(Vistor);
            }, Titles);
        }

        protected virtual void Vistor(T tObj, out bool onEvent)
        {
            ImGui.TableNextRow();
            DrawItem(tObj, out onEvent);
        }

        protected abstract string[] Titles
        {
            get;
        }
        protected abstract void DrawItem(T obj, out bool onEvent);
    }

    public abstract class TListTableView<T>: IView  where T : dnObject
    {
        //protected List<T> m_KeyNameList = null;
        IListController<T> m_Controller = null;

        public TListTableView(IListController<T> controller)
        {
            m_Controller = controller;
        }

        public void Draw()
        {
            ImGuiEx.TableView(TableLable, () =>
            {
                for (int i = 0; i < m_Controller.ListCount; ++i)
                {
                    ImGui.TableNextRow();
                    DrawItem(i, m_Controller.GetItem(i), out bool onEvent);
                    if (onEvent)
                        break;
                }
            }, Titles);
        }

        /*
        public void Draw()
        {
            if (m_KeyNameList == null)
                return;

            ImGuiEx.TableView(TableLable, () =>
            {
                for (int i = 0; i< m_KeyNameList.Count; ++i)
                {
                    ImGui.TableNextRow();
                    DrawItem(i,m_KeyNameList[i], out bool onEvent);
                    if (onEvent)
                        break;
                }
            }, Titles);
        }*/

        protected abstract string TableLable
        {
            get;
        }
        protected abstract string[] Titles
        {
            get;
        }
        protected abstract void DrawItem(int index,T obj, out bool onEvent);
    }
}