using ImGuiNET;
using System;
using System.Collections.Generic;

namespace DotInsideNode
{

    public abstract class IVar
    {
        //protected static ImGuiTableFlags TableFlags = ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable;
        protected int m_ID = -1;
        protected string m_Name = "";
        protected Type m_Type = typeof(bool);
        protected bool m_InstanceEditable = false;
        protected bool m_BluePrintReadOnly = false;
        protected string m_Tooltip = string.Empty;
        protected bool m_ExposeOnSpawn = false;
        protected bool m_Private = false;
        protected bool m_ExposeToCinematics = false;

        protected string m_EditName = string.Empty;

        public virtual bool BluePrintReadOnly
        {
            get => m_BluePrintReadOnly;
        }
        public virtual string Tooltip
        {
            get => m_Tooltip;
        }
        public virtual string VarName
        {
            get => m_Name;
            set
            {
                m_Name = value;
                m_EditName = m_Name;
            }
        }
        public virtual Type VarType
        {
            get => m_Type;
            protected set => m_Type = value;
        }
        public virtual Type VarBaseType
        {
            get => m_Type;
        }
        public virtual int VarID
        {
            get => m_ID;
            set => m_ID = value;
        }
        public abstract object VarValue
        {
            get;
            set;
        }
        public abstract List<VarSetter> Setters
        {
            get;
            protected set;
        }
        public abstract List<VarGetter> Getters
        {
            get;
            protected set;
        }

        public virtual IVarBase BaseCopy(IVarBase item)
        {
            var constructor = this.GetType().GetConstructor(System.Type.EmptyTypes);
            IVarBase copy = (IVarBase)constructor.Invoke(null);

            copy.m_ID = item.m_ID;
            copy.m_Name = item.m_Name;
            copy.m_EditName = item.m_EditName;
            copy.m_Type = item.m_Type;
            copy.m_InstanceEditable = item.m_InstanceEditable;
            copy.m_BluePrintReadOnly = item.m_BluePrintReadOnly;
            copy.m_Tooltip = item.m_Tooltip;
            copy.m_ExposeOnSpawn = item.m_ExposeOnSpawn;
            copy.m_Private = item.m_Private;
            copy.m_ExposeToCinematics = item.m_ExposeToCinematics;

            copy.Setters = item.Setters;
            copy.Getters = item.Getters;

            return copy;
        }
    }

    public abstract class IVarBase:IVar
    {
        public enum TContainer
        {
            Value,
            Array,
            Set,
            Dict
        }

        public abstract TContainer ContainerType
        {
            get;
        }

        public virtual void CopyType(IVarBase item)
        {
            this.VarType = item.VarType;
        }

        public abstract void OnVarDelete();
        public abstract void OnChangeType(IVarBase newvar);
        public abstract IVarBase Duplicate();

        protected virtual void DrawBaseEditor()
        {
            ImGui.InputText("Name",ref m_EditName, 30);
            DrawType();
            ImGui.SameLine();
            DrawContainerType();
            //ImGui.Checkbox("Instance Editable", ref m_InstanceEditable);
            ImGui.Checkbox("BluePrint Read Only", ref m_BluePrintReadOnly);
            ImGui.InputText("ToolTip", ref m_Tooltip, 30);
            //ImGui.Checkbox("Expose On Spawn", ref m_ExposeOnSpawn);
            //ImGui.Checkbox("Private", ref m_Private);
            //ImGui.Checkbox("Expose To Cinematics", ref m_ExposeToCinematics);
        }

        public virtual void DrawEditor() { }
        public virtual ComNodeBase NewGetter() => throw new System.NotImplementedException();
        public virtual ComNodeBase NewSetter() => throw new System.NotImplementedException();

        TContainer m_SelectContainer = TContainer.Value;

        public void DrawContainerType()
        {
            if (ImGui.BeginCombo(m_SelectContainer.ToString(), "", ImGuiComboFlags.NoPreview))
            {
                foreach(TContainer type in Enum.GetValues(typeof(TContainer)))
                {
                    if (ImGui.Selectable(type.ToString()))
                    {
                       m_SelectContainer = type;
                    }
                }
                
                ImGui.EndCombo();
            }
        }

        public virtual void DrawType()
        {
            if (ImGui.BeginCombo("Type", m_Type.Name))
            {
                foreach (IVarBase @var in VarManager.VarClassList)
                {
                    //Current container type
                    if(var.ContainerType == m_SelectContainer)
                    {
                        //Selected and type not equal
                        if (ImGui.Selectable(@var.VarBaseType.Name) &&
                            Equals(@var.VarType, VarType) == false)
                        {
                            this.OnChangeType(@var);
                        }
                    }
                }
                ImGui.EndCombo();
            }
        }
    }

}
