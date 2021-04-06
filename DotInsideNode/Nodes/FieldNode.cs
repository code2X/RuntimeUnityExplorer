using imnodesNET;
using ImGuiNET;
using System.Reflection;
using System.Collections.Generic;
using System;

namespace DotInsideNode
{
    class FieldNode : ComNodeBase
    {
        public SortedList<string, FieldInfo> FieldDict = new SortedList<string, FieldInfo>();
        FieldInfo m_FieldInfo;

        TextTB m_TextTitleBar = new TextTB("Field");
        ComboSC m_NodeCombo = new ComboSC();
        TargetIC m_TargetIC = new TargetIC();
        ObjectOC m_ObjectOC = new ObjectOC();

        public FieldNode()
        {
            m_TargetIC.OnSetTargetType += new TargetIC.TypeHandler(OnTargetTypeSet);

            AddComponet(m_TextTitleBar);
            AddComponet(m_NodeCombo);
            AddComponet(m_TargetIC);
            AddComponet(m_ObjectOC);
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

            m_NodeCombo.SetItemLsit(FieldDict.Keys);

            foreach (var pair in FieldDict)
            {
                m_ObjectOC.SetObjectType(pair.Value.FieldType);
                break;
            }
            //Console.WriteLine(type.Name + "ds");
        }


    }
}
