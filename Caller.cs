using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerSpace
{
    class Caller
    {
        public delegate void VoidFunc();

        public static void Try(VoidFunc voidFunc)
        {
            try
            {
                voidFunc();
            }
            catch (Exception exp)
            {
                Logger.Error(exp);
            }
        }
    }
}
