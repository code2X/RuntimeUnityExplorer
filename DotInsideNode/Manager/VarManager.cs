using System;
using System.Collections.Generic;

namespace DotInsideNode
{
    [Serializable]
    public class VarManager
    {
        public static List<IVarBase> VarClassList = new List<IVarBase>();

        Dictionary<string, IVarBase> m_Name2Vars = new Dictionary<string, IVarBase>();
        Dictionary<int, IVarBase> m_ID2Vars = new Dictionary<int, IVarBase>();

        IVarBase m_SelectedVar = null;
        string m_NewVarBaseName = "NewVar_";
        VarListDrawer m_ListDrawer = null;

        void InitBluePrintVarClasses(List<IVarBase> class_list)
        {
            if (class_list.Count != 0)
                return;
            var attrList = AttributeTools.GetNamespaceCustomAttributes(typeof(DotPrintVar));
            foreach (var pair in attrList)
            {
                System.Type type = pair.Key;

                IVarBase @var = (IVarBase)ClassTools.CallDefaultConstructor(type);
                class_list.Add(@var);
                Logger.Info("BP Variable: " + type);
            }
        }

        VarManager()
        {
            m_ListDrawer = new VarListDrawer(ref m_Name2Vars);
            m_ListDrawer.OnDelete += new VarListDrawer.VarAction(OnDelete);
            m_ListDrawer.OnSelect += new VarListDrawer.VarAction(OnSelect);
            m_ListDrawer.OnDuplicate += new VarListDrawer.VarAction(OnDuplicate);
            InitBluePrintVarClasses(VarClassList);
        }

        void OnSelect(IVarBase variable)
        {
            m_SelectedVar = variable;
        }

        void OnDelete(IVarBase variable)
        {
            TryDeleteVar(variable.VarName);
        }

        void OnDuplicate(IVarBase variable)
        {
            IVarBase dup = variable.Duplicate();
            AddVar(dup);
        }

        static VarManager __instance = new VarManager();
        public static VarManager Instance
        {
            get => __instance;
        }

        string GetNewVarName()
        {
            int index = 0;
            string varName = m_NewVarBaseName + index;
            while (m_Name2Vars.ContainsKey(varName))
            {
                ++index;
                varName = m_NewVarBaseName + index;
            }

            return varName;
        }

        int GetNewVarID()
        {
            int index = 0;
            while (m_ID2Vars.ContainsKey(index))
            {
                ++index;
            }

            return index;
        }

        public void AddVar(IVarBase variable)
        {
            string name = GetNewVarName();
            int id = GetNewVarID();
            variable.VarName = name;
            variable.VarID = id;

            m_Name2Vars.Add(name, variable);
            m_ID2Vars.Add(id, variable);
        }

        public bool ContainVar(int id)
        {
            return GetVarByID(id) != null ? true : false;
        }

        public bool ContainVar(string name) 
        {
            return GetVarByName(name) != null ? true : false;
        }

        public bool SelectVar(int id)
        {
            IVarBase variable;
            if ((variable = GetVarByID(id)) != null)
            {
                m_SelectedVar = variable;
                return true;
            }
            return false;
        }

        public IVarBase GetVarByID(int id)
        {
            IVarBase variable;
            return m_ID2Vars.TryGetValue(id, out variable) ? variable: null;  
        }

        public IVarBase GetVarByName(string name)
        {
            IVarBase variable;
            return m_Name2Vars.TryGetValue(name, out variable) ? variable : null;
        }
        
        public void DrawVarList()
        {
            m_ListDrawer.DrawList();
        }

        public void DrawVarInfo()
        {
            if( m_SelectedVar != null)
                m_SelectedVar.DrawEditor();
        }

        public bool TryDeleteVar(string var_name)
        {
            if (var_name == string.Empty)
                return false;

            IVarBase @var;
            return m_Name2Vars.TryGetValue(var_name, out @var) ? TryDeleteVar(@var) : false;
        }

        public bool TryDeleteVar(int var_id)
        {
            if (var_id == 0)
                return false;

            IVarBase @var;
            return m_ID2Vars.TryGetValue(var_id, out @var) ? TryDeleteVar(@var) : false;
        }

        bool TryDeleteVar(IVarBase @var)
        {
            @var.OnVarDelete();     //Notify var delete action
            if (m_ID2Vars.Remove(@var.VarID) == false)
            {
                Logger.Warn("Try delete var not in id dict");
                return false;
            }
            if (m_Name2Vars.Remove(@var.VarName) == false)
            {
                Logger.Warn("Try delete var not in name dict");
                return false;
            }
            return true;
        }

        public bool TryReplaceVar(IVarBase old_var, IVarBase new_var)
        {
            IVarBase var1,var2;
            if (m_Name2Vars.TryGetValue(old_var.VarName, out var1) == false || 
                m_ID2Vars.TryGetValue(old_var.VarID,out var2) == false)
                return false;

            if (var1.VarID != var2.VarID || var1.VarName != var2.VarName)
                return false;

            m_ID2Vars[old_var.VarID] = new_var;
            m_Name2Vars[old_var.VarName] = new_var;          
            return true;
        }
    }

}
