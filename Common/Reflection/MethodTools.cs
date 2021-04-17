using System;
using System.Collections.Generic;
using System.Reflection;

class MethodTools
{
    public static MethodInfo[] GetAllMethod(Type type)
    {
        return type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
    }
 
    public static string GetParamStr(MethodInfo method)
    {
        string str = "";
        str += "(";
        var parameters = method.GetParameters();
        for (int i = 0; i < parameters.Length; ++i)
        {
            if (i != parameters.Length - 1)
            {
                str += parameters[i].ParameterType.Name + " " + parameters[i].Name + ",";
            }
            else
            {
                str += parameters[i].ParameterType.Name;
            }
        }
        str += ")";

        return str;
    }

    public static List<string> GetMethodList(Type type)
    {
        List<string> methodList = new List<string>();
        var methodInfos = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
        foreach (var method in methodInfos)
        {
            string str = "";
            str += method.Name;
            str += GetParamStr(method);
            str += ": " + method.ReturnType.Name;

            methodList.Add(str);
        }

        return methodList;
    }
}
