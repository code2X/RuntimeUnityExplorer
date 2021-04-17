using ImGuiNET;
using System.Collections.Generic;

namespace DotInsideNode
{
    public abstract class ValueVar<T> : IVarBase
    {
        NodeManager m_NodeManager = NodeManager.Instance;
        VarManager m_VarManager = VarManager.Instance;

        protected T m_Value;
        protected List<VarSetter> m_Setters = new List<VarSetter>();
        protected List<VarGetter> m_Getters = new List<VarGetter>();

        public ValueVar()
        {
            VarType = typeof(T);
        }

        public override object VarValue
        {
            get => m_Value;
            set => m_Value = (T)value;
        }

        public override List<VarSetter> Setters
        {
            get => m_Setters;
            protected set => m_Setters = value;
        }

        public override List<VarGetter> Getters
        {
            get => m_Getters;
            protected set => m_Getters = value;
        }

        public override TContainer ContainerType
        {
            get => TContainer.Value;
        }

        protected IVarBase BaseDuplicate()
        {
            var constructor = this.GetType().GetConstructor(System.Type.EmptyTypes);
            IVarBase newVar = (IVarBase)constructor.Invoke(null);
            newVar.BaseCopy(this);
            newVar.VarValue = m_Value;

            return newVar;
        }

        public override IVarBase Duplicate()
        {
            return BaseDuplicate();
        }

        public override void OnChangeType(IVarBase template_var)
        {
            IVarBase newVar = template_var.BaseCopy(this);
            Assert.IsNotNull(newVar);
            newVar.CopyType(template_var);

            foreach (VarSetter setter in m_Setters)
            {
                setter.ChageVar(newVar);
            }
            foreach (VarGetter getter in m_Getters)
            {
                getter.ChageVar(newVar);
            }
            Assert.IsTrue(m_VarManager.TryReplaceVar(this, newVar));
            Assert.IsTrue(m_VarManager.SelectVar(newVar.VarID));
        }

        public override void OnVarDelete()
        {
            foreach (VarSetter setter in m_Setters)
            {
                m_NodeManager.TryDestroyNode(setter.ID);
            }
            foreach (VarGetter getter in m_Getters)
            {
                m_NodeManager.TryDestroyNode(getter.ID);
            }
        }

        public override ComNodeBase NewGetter()
        {
            VarGetter getter = new VarGetter(this);
            m_Getters.Add(getter);
            return getter;
        }

        public override ComNodeBase NewSetter()
        {
            VarSetter setter = new VarSetter(this);
            m_Setters.Add(setter);
            return setter;
        }

        public override void DrawEditor()
        {
            if (ImGui.CollapsingHeader("Variable", ImGuiTreeNodeFlags.DefaultOpen))
            {
                DrawBaseEditor();
            }
            if (ImGui.CollapsingHeader("Default Value", ImGuiTreeNodeFlags.DefaultOpen))
            {
                DrawValueEditor();
            }
        }

        protected abstract void DrawValueEditor();
    }

    [DotPrintVar]
    public class BoolVar : ValueVar<bool>
    {
        protected override void DrawValueEditor()
        {
            ImGui.Checkbox(VarName, ref m_Value);
        }
    }

    [DotPrintVar]
    public class IntVar : ValueVar<int>
    {
        int[] m_Range = new int[2];

        protected override void DrawValueEditor()
        {
            ImGui.InputInt(VarName, ref m_Value);
        }
    }

    [DotPrintVar]
    public class FloatVar : ValueVar<float>
    {
        protected override void DrawValueEditor()
        {
            ImGui.InputFloat(VarName, ref m_Value,0.1f);
        }
    }

    [DotPrintVar]
    public class DoubleVar : ValueVar<double>
    {
        protected override void DrawValueEditor()
        {
            ImGui.InputDouble(VarName, ref m_Value, 0.1);
        }
    }

    [DotPrintVar]
    public class StringVar : ValueVar<string>
    {
        public StringVar()
        {
            m_Value = "";
        }

        protected override void DrawValueEditor()
        {
            ImGui.InputText(VarName, ref m_Value,20);
        }
    }

    [DotPrintVar]
    public class VectorVar : ValueVar<Vector3>
    {
        protected override void DrawValueEditor()
        {
            ImGui.InputFloat3(VarName, ref m_Value);
        }
    }

}