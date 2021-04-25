
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DotInsideNode
{
    class CT
    {
        List<Line> lineList = new List<Line>();

        public class Line
        {
            string m_Text = "";

            public Line(string text)
            {
                m_Text = text;
            }

            public void LSpace(int size) => m_Text = m_Text.PadLeft(m_Text.Length + size);

            public string Text => m_Text + "\n";
        }

        public class EnumClass
        {
            string m_Text = "";
            List<Line> itemList = new List<Line>();

            public EnumClass(string name)
            {
                m_Text += 
                    new Line("enum " + name).Text + 
                    new Line("{").Text;
            }

            public void AddItem(IEnumItem item)
            {
                itemList.Add(new Line(item.Name + " = " + item.ID + ","));
            }

            public string Text
            {
                get
                {           
                    foreach (Line item in itemList)
                    {
                        item.LSpace(5);
                        m_Text += item.Text;
                    }
                    m_Text += 
                        new Line("}").Text;
                    return m_Text;
                }
            }
        }

        public void EnterBracket()
        {

        }

        public void LeaveBracket()
        {

        }

        void AddLine(Line line)
        {
            lineList.Add(line);
        }

        public static CT operator +(CT left, Line b)
        {
            left.AddLine(b);
            return left;
        }

        public string Compile()
        {
            string result = "";

            foreach (Line item in lineList)
            {
                result += item.Text;
            }

            return result;
        }
    }

    [Serializable]
    [BlueprintClass("Enumeration")]
    public class BP_Enumeration: IBP_Enumeration
    {
        #region MVC
        KeyNameList<IEnumItem> m_EnumKeys = new KeyNameList<IEnumItem>();

        [NonSerialized]
        List<IView> m_Views;
        [NonSerialized]
        IViewsFactory m_ViewsFactory;

        [NonSerialized]
        IListController<IEnumItem> m_Controller;
        #endregion

        #region Deserialized
        [OnDeserializedAttribute]
        protected void OnDeserialized(StreamingContext sc)
        {
            ResetViewController();
        }
        #endregion

        string m_Description = "";

        public BP_Enumeration()
        {
            ResetViewController();
        }

        public virtual void ResetViewController()
        {
            m_Controller = new EnumerationController(m_EnumKeys);
            m_ViewsFactory = new EnumViewFactory(this, m_Controller);
            m_Views = m_ViewsFactory.GetViews();
        }

        public override string Description
        {
            get => m_Description;
            set => m_Description = value;
        }

        public override List<IEnumItem> Enumerators => m_EnumKeys.ObjList;

        public override void Compile() 
        {
            CT.EnumClass enumClass = new CT.EnumClass("Keycode");
            foreach (IEnumItem item in Enumerators)
            {
                enumClass.AddItem(item);
            }
            Console.WriteLine(enumClass.Text);
        }

        public override void AddEnumItem() => m_EnumKeys.AddObject(new IEnumItem());
        public override void Draw()
        {
            foreach(IView view in m_Views)
            {
                view.Draw();
            }
        }
    }

    public interface IView
    {
        void Draw();
    }

}