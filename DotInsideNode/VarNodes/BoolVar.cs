using System.Collections.Generic;
using ImGuiNET;

namespace DotInsideNode
{
    class Math
    {
        public static int Clamp(int value, int min, int max)
        {
            if(value < min)
            {
                return min;
            }
            else if(value > max)
            {
                return max;
            }
            return value;
        }
    }

    class VarManager
    {
        Dictionary<string, VarBase> m_Vars = new Dictionary<string, VarBase>();

        string m_NewVarBaseName = "NewVar_";

        public void AddVar(VarBase variable)
        {
            int index = 0;
            string varName = m_NewVarBaseName + index;
            while(m_Vars.ContainsKey(varName))
            {
                ++index;
                varName = m_NewVarBaseName + index;
            }

            variable.SetName(varName);
            m_Vars.Add(varName, variable);
        }

        public void DrawVar()
        {
            int lineNum = m_Vars.Count;
            lineNum = Math.Clamp(lineNum, 1, 10);
            ImGui.BeginListBox(
                "##VariablesListBox",
                new Vector2(ImGui.GetColumnWidth(), ImGui.GetTextLineHeightWithSpacing() * lineNum)
                );
            foreach (var varPair in m_Vars)
            {
                if(ImGui.Selectable(varPair.Key))
                {
                    System.Console.WriteLine(varPair.Key);
                }
                if (ImGui.BeginPopupContextItem())
                {
                    ImGui.Text(varPair.Key);
                    ImGui.EndPopup();
                }
            }
            ImGui.EndListBox();

        }
    }

    class BoolVar : VarBase
    {
        //ObjectOC m_ObjectOC = new ObjectOC();
        bool m_ReadOnly = false;
        string m_Tooltip = "";

        bool m_Value = false;

        public class Setter : ComNodeBase
        {

        }

        public class Getter : ComNodeBase
        {
            //TextTB m_TextTitleBar = new TextTB("Var");
            ObjectOC m_ObjectOC = new ObjectOC();

            public Getter()
            {
                //AddComponet(m_TextTitleBar);
                m_ObjectOC.SetName("Var1");
                m_ObjectOC.SetObjectType(typeof(int));
                AddComponet(m_ObjectOC);
            }
        }
    }

}
