using System;
using System.Collections.Generic;
using ImGuiNET;
using System.Runtime.InteropServices;

namespace ExplorerSpace
{
    public enum ClassInfo
    {
        Null,
        Field,
        Property,
        Method,
    }

    class UnityGameExplorer
    {
        static InstanceView instanceView;
        static AssemblyExplorerView explorerView;
        static Assembler g_Assembly;
        public static SortedDictionary<string, Type> g_ClassName2Type;
        public static SortedList<string, CsharpClass> classListDetails;

        static unsafe void MyRender()
        {
            explorerView.OnGUI();
            instanceView.OnGUI();
            ImGui.ShowDemoWindow();
            MethodInvokeWindow.GetInstance().OnGUI();
        }

        static void Main(string[] args)
        {
            explorerView = AssemblyExplorerView.GetInstance();
            classListDetails = new SortedList<string, CsharpClass>();

            try
            {
                g_Assembly = new Assembler("Assembly-CSharp.dll");
                g_ClassName2Type = g_Assembly.getTypeDict();

                explorerView.AutoCluster(g_ClassName2Type);

                foreach (var cls in g_ClassName2Type)
                {
                    classListDetails.Add(cls.Key, new CsharpClass(cls.Value));
                }
            }
            catch (Exception exp)
            {
                Logger.Error(exp);
            }
            instanceView = new InstanceView();

            try
            {
                GIRerSurface.AddRenderCallback(MyRender);
                Console.ReadLine();
            }
            catch(Exception exp)
            {
                Logger.Error(exp);
            }

        }


    }
}
