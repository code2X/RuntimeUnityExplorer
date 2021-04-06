using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotInsideNode
{
    class VarBase
    {
        protected string m_Name = "";
        protected int m_Type;

        public string GetName() => m_Name;
        public void SetName(string name) => m_Name = name;
        public void SetType(int type) => m_Type = type;
    }
}
