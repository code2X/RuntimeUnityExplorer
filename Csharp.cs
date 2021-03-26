using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Linq.Expressions;
using System.Reflection;

using HarmonyLib;
using UnityEngine;

namespace ExplorerSpace
{
    class CsharpKeyword
    {
        public static HashSet<Type> GeneralTypes = new HashSet<Type>
        {
            typeof(int),
            typeof(float),
            typeof(byte),
            typeof(double),
            typeof(ulong),
            typeof(uint),
            typeof(bool),
            typeof(Boolean),
            typeof(short),
            typeof(long),
            typeof(string)
        };
    }

    public class CsharpClass
    {
        private MethodInfo[] methodInfos;
        private PropertyInfo[] propertyInfos;
        private FieldInfo[] fieldInfos;
        private EventInfo[] eventInfos;

        private SortedList<string, FieldInfo> sortField;
        private SortedList<string, PropertyInfo> sortProp;
        public SortedList<string, MethodInfo> sortMethod;
        public SortedList<string, MethodInfo> sortGetMethod;
        public SortedList<string, MethodInfo> sortSetMethod;

        private SortedList<string, FieldInfo> sortStaticField;
        private SortedList<string, MethodInfo> sortStaticProp;
        public SortedList<string, MethodInfo> sortStaticMethod;

        public Type type;

        public SortedList<string, FieldInfo> FieldList
        {
            get
            {
                return sortField;
            }
        }
        public SortedList<string, PropertyInfo> PropList
        {
            get
            {
                return sortProp;
            }
        }
        public SortedList<string, MethodInfo> MethodList
        {
            get
            {
                return sortMethod;
            }
        }
        public SortedList<string, FieldInfo> StaticFieldList
        {
            get
            {
                return sortStaticField;
            }
        }
        public SortedList<string, MethodInfo> StaticMethodList
        {
            get
            {
                return sortStaticMethod;
            }
        }
        public SortedList<string, MethodInfo> StaticPropList
        {
            get
            {
                return sortStaticProp;
            }
        }

        public CsharpClass(Type classType)
        {
            type = classType;

            try
            {
                sortField = new SortedList<string, FieldInfo>();
                sortProp = new SortedList<string, PropertyInfo>();
                sortMethod = new SortedList<string, MethodInfo>();
                sortGetMethod = new SortedList<string, MethodInfo>();
                sortSetMethod = new SortedList<string, MethodInfo>();

                sortStaticField = new SortedList<string, FieldInfo>();
                sortStaticMethod = new SortedList<string, MethodInfo>();
                sortStaticProp = new SortedList<string, MethodInfo>();

                methodInfos = classType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                propertyInfos = classType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                fieldInfos = classType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                eventInfos = classType.GetEvents();

                AddNameList();
            }
            catch (Exception info)
            {
                //Console.WriteLine(info.Message);
            }
        }

        //Get a name new in sortlist
        private string GetName<T>(SortedList<string, T> sortList, string name)
        {
            while (sortMethod.ContainsKey(name))
            {
                name += "#";
            }
            return name;
        }

        private void AddNameList()
        {
            //Method
            foreach (MethodInfo m in methodInfos)
            {             
                if(m.IsStatic)
                {
                    //Static Property Function
                    if (m.Name.StartsWith("get_") || m.Name.StartsWith("set_"))
                    {
                        string name = GetName(sortStaticProp, m.Name);
                        sortStaticProp.Add(name, m);
                    }
                    //Static Function
                    else
                    {
                        string name = GetName(sortStaticMethod, m.Name);
                        sortStaticMethod.Add(name, m);
                    }
                }
                else
                {
                    string name = GetName(sortMethod, m.Name);

                    //Unity Function
                    if (name.StartsWith("BroadcastMessage") ||
                      name.StartsWith("CancelInvoke") ||
                      name.StartsWith("GetComponent") ||
                      name.StartsWith("SendMessage") ||
                      name.StartsWith("StartCoroutine") ||
                      name.StartsWith("StopAllCoroutine") ||
                      name.StartsWith("StopCoroutine") ||
                      name.StartsWith("Invoke") ||
                      name.StartsWith("ToString") ||
                      name.StartsWith("GetType") ||
                      name.StartsWith("Get") ||
                      name.StartsWith("IsInvok") ||
                      name == "CompareTo" ||
                      name == "Equals")
                        continue;

                    //Property Function
                    //if (name.StartsWith("get_"))
                    //{
                    //    sortGetMethod.Add(name, m);
                    //}
                    //else if (name.StartsWith("set_"))
                    //{
                    //    sortSetMethod.Add(name, m);
                    //}
                    ////General Function
                    //else
                    //{
                        sortMethod.Add(name, m);
                    //}
                }
          
            }

            //Property
            foreach (PropertyInfo m in propertyInfos)
            {
                string name = GetName(sortProp, m.Name);
                sortProp.Add(m.Name, m);
            }

            //Field
            foreach (FieldInfo m in fieldInfos)
            {
                //Static Field
                if (m.IsStatic)
                {
                    string name = GetName(sortStaticField, m.Name);
                    sortStaticField.Add(name, m);
                }
                //General Field
                else
                {
                    string name = GetName(sortField, m.Name);
                    sortField.Add(m.Name, m);
                }
            }

        }
    }

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
            Debug.Log("Patch:" + patchInfo.Name);
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
            Debug.Log("UnPatch:" + patchInfo.Name);
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
