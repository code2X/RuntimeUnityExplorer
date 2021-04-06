using DotInsideLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using imnodesNET;

namespace DotInsideNode
{
    class TypeofNode : NodeBase
    {
        public static SortedList<string, CsharpClass> classList = DotInside.classListDetails;
        CsharpClass m_Class = null;

        TextTB m_TextTitleBar = new TextTB("Type of");
        ComboSC m_NodeCombo;
        TypeOC m_NodeTypeOutput;

        //public void Init()
        //{
        //    Type info = typeof(CsharpClass);
        //    var propertys = info.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        //
        //    foreach (var prop in propertys)
        //    {
        //        object[] attributes = prop.GetCustomAttributes(true);
        //        for (int i = 0; i < attributes.Length; i++)
        //        {
        //            NodeCompAttr comp = (NodeCompAttr)attributes[i];
        //            if (comp.CompType == ComponentType.Output)
        //            {
        //                nodeComponents.Add(new NodeTextOutput(comp.Name));
        //            }
        //        }
        //    }
        //}

        public TypeofNode()
        {
            var enumerator = classList.GetEnumerator();
            enumerator.MoveNext();
            m_Class = enumerator.Current.Value;

            m_NodeTypeOutput = new TypeOC(m_Class.type);
            m_NodeCombo = new ComboSC(classList.Keys);
            m_NodeCombo.OnSelect += new ComboSC.SelectHandler(OnClassSelect);

            AddComponet(m_TextTitleBar);
            AddComponet(m_NodeCombo);
            AddComponet(m_NodeTypeOutput);   
        }

        void OnClassSelect(string item)
        {
            if(classList.TryGetValue(item,out m_Class))
            {
                m_NodeTypeOutput.SetType(m_Class.type);
                System.Console.WriteLine(item);
            }           
        }

        public override string Compile()
        {
            string res = "typeof(" + m_Class.typeName + ")";
            return res;
        }

        protected override void DrawContent()
        {
            //imnodesNET.imnodes.Lin
            //System.Console.WriteLine(classListDetails.Count);
        }
    }
}
