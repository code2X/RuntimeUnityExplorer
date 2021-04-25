using System;

namespace DotInsideNode
{
    [AttributeUsage(AttributeTargets.Class)]
    class BlueprintClass : System.Attribute
    {
        string m_Name;

        public BlueprintClass(string name)
        {
            m_Name = name;
        }

        public string Name => m_Name;
    }
}
