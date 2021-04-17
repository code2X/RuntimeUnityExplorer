using System;
using System.Threading;

namespace DotInsideLib
{
    public class Caller
    {
        public delegate void VoidFunc();

        public static bool Try(VoidFunc func)
        {
            try
            {
                func();
                return true;
            }
            catch (Exception exp)
            {
                Logger.Error(exp);
                return false;
            }
        }

        public static void EnterMutex(Mutex mutex, VoidFunc voidFunc)
        {
            if (mutex.WaitOne())
            {
                voidFunc();
                mutex.ReleaseMutex();
            }
        }

        public static void TryEnterMutex(Mutex mutex, VoidFunc voidFunc)
        {
            try
            {
                EnterMutex(mutex, voidFunc);
            }
            catch (Exception exp)
            {
                mutex.ReleaseMutex();
                Logger.Error(exp);
            }
        }
    }
}
