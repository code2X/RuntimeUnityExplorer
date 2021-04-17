using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace DotInsideLib
{
    class SinglePatch
    {
        public static Harmony harmony = new Harmony("Runtime Unity SinglePatch");
        public static int count = 0;
        public static bool block = false;
        public static MethodInfo prefix = typeof(SinglePatch).GetMethod("Process");
        private static bool patched = false;
        private static MethodInfo patchedMethodInfo;

        public static void Reset()
        {
            count = 0;
            //block = false;
        }

        public static void Patch(ref MethodInfo patchInfo)
        {
            Console.WriteLine("Patch:" + patchInfo.Name);
            Reset();

            if (patched == true)
            {
                UnPatch(ref patchedMethodInfo);
            }
            try
            {
                var original = patchInfo;
                //var prefix = typeof(SinglePatch).GetMethod("Process");
                //Debug.Log("Prefix:" + prefix.Name);
                harmony.Patch(original, new HarmonyMethod(prefix));
                patched = true;
                patchedMethodInfo = original;
            }
            catch (Exception info)
            {
                Console.WriteLine(info.Message);
            }
        }

        public static void UnPatch(ref MethodInfo patchInfo)
        {
            Console.WriteLine("UnPatch:" + patchInfo.Name);
            Reset();

            try
            {
                var original = patchInfo;
                //var prefix = typeof(SinglePatch).GetMethod("Process");
                harmony.Unpatch(original, prefix);
                patched = false;
            }
            catch (Exception info)
            {
                Console.WriteLine(info.Message);
            }
        }

        public static bool Process()
        {
            ++count;
            if (block)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
