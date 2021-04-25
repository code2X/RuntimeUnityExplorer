using System;
using System.Collections.Generic;

namespace DotInsideNode
{
    [Serializable]
    public class IEnumItem : dnObject
    {
        string m_Description = "";   // Description of the object

        public int Value => ID;
        public bool CustomValue = false;
        public virtual string Description
        {
            get => m_Description;
            set => m_Description = value;
        }
    }

    [Serializable]
    abstract public class IBP_Enumeration : IBluePrint
    {

        public abstract string Description
        {
            get;
            set;
        }

        /// <summary>
        /// All enum item
        /// </summary>
        public abstract List<IEnumItem> Enumerators
        {
            get;
        }
        public abstract void AddEnumItem();

        public virtual int EnumCount => Enumerators.Count;
        public override string NewBaseName => "NewUserDefinedEnum";
    }
}
