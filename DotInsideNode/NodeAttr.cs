using System;

namespace DotInsideNode
{
    [AttributeUsage(
    AttributeTargets.Field |
    AttributeTargets.Method |
    AttributeTargets.Property,
    AllowMultiple = true)]

    class NodeCompAttr: System.Attribute
    {
        ComponentType compType;
        string name;
        Type type;

        public NodeCompAttr(ComponentType compType, string name, Type type)
        {
            this.compType = compType;
            this.name = name;
            this.type = type;
        }

        public ComponentType CompType
        {
            get
            {
                return compType;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public Type Type
        {
            get
            {
                return type;
            }
        }
    }
}
