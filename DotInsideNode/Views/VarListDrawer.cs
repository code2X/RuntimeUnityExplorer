using System.Collections.Generic;
using System.Reflection;
using ImGuiNET;

namespace DotInsideNode
{
    class VarListDrawer
    {
        class RenameAction
        {
            public delegate void Action(IVarBase variable, string new_name);
            IVarBase m_RenameVar = null;
            public event Action OnAction;

            void DrawPopupMenuItem(IVarBase @var)
            {
                if (ImGui.Selectable("Rename"))
                {
                    m_RenameVar = @var;
                }
            }

            void DoNotifyAction()
            {
                if (m_RenameVar != null)
                {
                    //OnRename?.Invoke(m_RenameVar);
                }
            }
        }

        public delegate void VarAction(IVarBase variable);
        //public delegate void VarRenameAction(VarBase variable,string new_name);
        public event VarAction OnDelete;
        public event VarAction OnSelect;
        //public event VarRenameAction OnRename;
        public event VarAction OnDuplicate;

        Dictionary<string, IVarBase> m_Name2Vars = new Dictionary<string, IVarBase>();

        public VarListDrawer(ref Dictionary<string, IVarBase> name2vars)
        {
            m_Name2Vars = name2vars;
        }

        IVarBase m_DeleteVar = null;
        IVarBase m_SelectedVar = null;
        //VarBase m_RenameVar = null;
        IVarBase m_DuplicateVar = null;

        void Reset()
        {
            m_DeleteVar = null;
            m_SelectedVar = null;
            //m_RenameVar = null;
            m_DuplicateVar = null;
        }

        public void DrawList()
        {
            int lineNum = MATH.Utils.Clamp(m_Name2Vars.Count, 0, 10);
            if (lineNum == 0)
                return;

            ImGui.BeginListBox("##VariablesListBox",
                new Vector2(ImGui.GetColumnWidth(), ImGui.GetTextLineHeightWithSpacing() * lineNum)
                );
            foreach (var varPair in m_Name2Vars)
            {
                DrawSelectItem(varPair.Value);
                DoDragVar(varPair.Value);
                DrawPopupMenu(varPair.Value);
            }
            ImGui.EndListBox();

            DoNotifyAction();
        }

        void DoNotifyAction()
        {
            if (m_DeleteVar != null)
            {
                OnDelete?.Invoke(m_DeleteVar);
            }
            if (m_SelectedVar != null)
            {
                OnSelect?.Invoke(m_SelectedVar);
            }
            //if (m_RenameVar != null)
            //{
            //    //OnRename?.Invoke(m_RenameVar);
            //}
            if (m_DuplicateVar != null)
            {
                OnDuplicate?.Invoke(m_DuplicateVar);
            }
            Reset();
        }

        void DrawSelectItem(IVarBase @var)
        {
            if (ImGui.Selectable(@var.VarName))
            {
                m_SelectedVar = @var;
            }
            //DrawTooltip(@var);
        }

        void DrawTooltip(IVarBase @var)
        {
            if(var.Tooltip != string.Empty &&
                ImGui.IsItemHovered())
            {
                ImGui.SetTooltip(var.Tooltip);
            }
        }

        void DrawPopupMenu(IVarBase @var)
        {
            ImGuiUtils.PopupContextItemView(() =>
            {
                if (ImGui.Selectable("Delete"))
                {
                    m_DeleteVar = @var;
                }
                if (ImGui.Selectable("Duplicate"))
                {
                    m_DuplicateVar = @var;
                }
            });
        }

        int dragVarID;
        unsafe void DoDragVar(IVarBase @var)
        {
            dragVarID = @var.VarID;
            fixed (int* p = &dragVarID)
            {
                System.IntPtr intPtr = new System.IntPtr(p);
                if (ImGui.BeginDragDropSource(ImGuiDragDropFlags.None))
                {
                    // Set payload to carry the index of our item (could be anything)
                    ImGui.SetDragDropPayload("VAR_DRAG", intPtr, sizeof(int));

                    // Display preview (could be anything, e.g. when dragging an image we could decide to display
                    // the filename and a small preview of the image, etc.)
                    ImGui.Text(@var.VarName + ":" + dragVarID);
                    ImGui.EndDragDropSource();
                }
            }
        }

    }

}
