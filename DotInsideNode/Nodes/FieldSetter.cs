using imnodesNET;
using ImGuiNET;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace DotInsideNode
{
    class FieldSetterNode : ComNodeBase
    {
        public SortedList<string, FieldInfo> FieldDict = new SortedList<string, FieldInfo>();
        FieldInfo m_FieldInfo;

        ExecIC m_ExecIC = new ExecIC();
        ExecOC m_ExecOC = new ExecOC();
        TextTB m_TextTitleBar = new TextTB("Set Field");
        ComboSC m_FieldCombo = new ComboSC();
        TargetIC m_TargetIC = new TargetIC();
        ObjectIC m_ObjectIC = new ObjectIC();
        ObjectOC m_ObjectOC = new ObjectOC();

        public FieldSetterNode()
        {
            m_TargetIC.OnSetTargetType += new TargetIC.TypeHandler(OnTargetTypeSet);

            AddComponet(m_TextTitleBar);
            AddComponet(m_ExecIC);
            AddComponet(m_ExecOC);
            AddComponet(m_FieldCombo);
            AddComponet(m_TargetIC);
            AddComponet(m_ObjectOC);
            AddComponet(m_ObjectIC);
        }

        protected override void DrawContent()
        {
        }

        void OnTargetTypeSet(Type type)
        {
            FieldInfo[] allFileds = FieldTools.GetAllField(type);
            FieldDict.Clear();
            foreach (FieldInfo field in allFileds)
            {
                FieldDict.Add(field.Name, field);
            }

            m_FieldCombo.SetItemLsit(FieldDict.Keys);

            foreach (var pair in FieldDict)
            {
                m_ObjectOC.SetObjectType(pair.Value.FieldType);
                m_ObjectIC.SetObjectType(pair.Value.FieldType);
                break;
            }
            //Console.WriteLine(type.Name + "ds");
        }


    }
}
