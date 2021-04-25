using ImGuiNET;
using DotInsideNode;

namespace Views
{
    public interface IPopupMenu:IView
    {
        bool IsOpen 
        {
            get;
        }
    }

    public class NullPopupMenu : IPopupMenu
    {
        public void Draw() { }
        bool IPopupMenu.IsOpen => true;
    }

    class PopupVarGetSet: IPopupMenu
    {
        string m_PopupID = "PopupVarGetSet";
        string m_Title = "";

        public string GetPopupID() => m_PopupID;

        #region Event
        public enum EMenuItem
        {
            Get,
            Set
        }

        public delegate void SelectAction(EMenuItem eMenuItem);
        public event SelectAction OnMenuEvent;
        #endregion

        public void Open(string title)
        {
            m_Title = title;
            ImGui.OpenPopup(m_PopupID);
        }

        public virtual void Draw()
        {
            
            if (ImGui.BeginPopup(m_PopupID, ImGuiWindowFlags.AlwaysAutoResize))
            {
                DrawMenu();

                ImGui.EndPopup();
            }
        }

        void DrawMenu()
        {
            ImGui.Text(m_Title);
            if(ImGui.Selectable("Get " + m_Title))
            {
                OnMenuEvent?.Invoke(EMenuItem.Get);
            }
            if (ImGui.Selectable("Set " + m_Title))
            {
                OnMenuEvent?.Invoke(EMenuItem.Set);
            }
        }

        public virtual bool IsOpen => ImGui.IsPopupOpen(m_PopupID);
    }

    class PopupVarGetSetController: NodeEditorDroppable
    {
        PopupVarGetSet m_Popup = new PopupVarGetSet();
        IVarManager m_VarManager = null;
        IVar m_Var = null;

        public PopupVarGetSetController(IVarManager manager)
        {
            m_VarManager = manager;
            Init();
        }

        void Init()
        {
            m_Popup.OnMenuEvent += OnMenuEvent;
        }

        private void OnMenuEvent(PopupVarGetSet.EMenuItem eMenuItem)
        {
            Logger.Info("Var Menu Select:" + eMenuItem);
            if (m_Var == null)
                return;

            switch (eMenuItem)
            {
                case PopupVarGetSet.EMenuItem.Get:
                    m_NodeGraph.ngNodeManager.AddNode(m_Var.GetNewGetter(m_NodeGraph));
                    break;
                case PopupVarGetSet.EMenuItem.Set:
                    m_NodeGraph.ngNodeManager.AddNode(m_Var.GetNewSetter(m_NodeGraph), true);
                    break;
            }
        }

        INodeGraph m_NodeGraph = null;
        public void OnNodeEditorDrop(INodeGraphEditor nodeGraphEditor, INodeGraph nodeGraph)
        {
            m_NodeGraph = nodeGraph;
            OnVarDragDrop(nodeGraphEditor,nodeGraph);
        }

        unsafe void OnVarDragDrop(INodeGraphEditor editor, INodeGraph nodeGraph)
        {
            ImGuiPayloadPtr pPayload = ImGui.AcceptDragDropPayload("VAR_DRAG");
            if (pPayload.NativePtr == null)
                return;

            int varID = *(int*)pPayload.Data;
            GetVar(varID);
            if(m_Var != null)
            {
                m_Popup.Open(m_Var.Name);
                editor.SubmitRightMenu(m_Popup);
                Logger.Info("Var Drop. ID:" + varID);
            }
        }

        void GetVar(int varID)
        {
            if(m_VarManager != null)
            {
                m_Var = m_VarManager.GetVarByID(varID);
            }
            else
            {
                m_Var = null;
            }            
        }

       
    }

}
