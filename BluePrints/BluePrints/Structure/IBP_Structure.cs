
using System;

namespace DotInsideNode
{
    [Serializable]
    public abstract class IBP_Structure: IBluePrint
    {
        public abstract string Tooltip
        {
            get;
            set;
        }

        public abstract int MemberCount
        {
            get;
        }

        public override string NewBaseName => "NewUserDefinedStruct";
    }
}
