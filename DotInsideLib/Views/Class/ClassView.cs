using System;
using ImGuiNET;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.Threading;

namespace DotInsideLib
{
    public enum ClassInfo
    {
        Null,
        Field,
        Property,
        Method,
    }

    public abstract class IClassView
    {
        public static ImGuiTableFlags TableFlags = ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.Borders | ImGuiTableFlags.Resizable | ImGuiTableFlags.Reorderable | ImGuiTableFlags.Hideable;
        public abstract ClassInfo GetInfo();
        protected abstract void Draw();

        public virtual void DrawView()
        {
            if (show)
            {
                Draw();
            }
        }

        public virtual void Show(Type type, object instance = null)
        {
            show = false; //prevent error
#if (ModalWindow)
            //netframework 4.0
            if(type != curClassType)
            {
#else
            {
#endif
                curInstance = instance;
                string classTypeName = type.Name;

                if (DotInside.classListDetails.ContainsKey(classTypeName) == false)
                {
                    DotInside.classListDetails.Add(type.Name, new CsharpClass(type));
                }
                curClass = DotInside.classListDetails[classTypeName];
            }
            curClassType = type;
            show = true;
        }

        public virtual CsharpClass GetClass() => curClass;
        public virtual object GetClassInstance() => curInstance;

        protected bool show = false;
        CsharpClass curClass;
        Type curClassType;
        object curInstance;
    }

    public class ClassView : IClassView
    {
        List<IClassView> subViews;
        Dictionary<Type, bool> topTabs;

        static ClassView instance = new ClassView();
        public static ClassView GetInstance() => instance;
        public override ClassInfo GetInfo() => throw new NotImplementedException();

        ClassView()
        {
            subViews = new List<IClassView>();
            topTabs = new Dictionary<Type, bool>();
        }

        public void AddSubView(IClassView classView)
        {
            subViews.Add(classView);
        }

        public void Show(Type type)
        {
            TryAddTopTab(type);
            base.Show(type);
            ShowSubViews(type);
        }

        protected override void Draw()
        {
            ImGui.Begin("Class Information", ref show);
            DrawTopTabs();
            DrawSubViews();
            ImGui.End();
        }

        void DrawTopTabs()
        {
            //store tab state
            var typesState = new Dictionary<Type, bool>();
            if (ImGui.BeginTabBar("ClassInfoViewTopTab"))
            {
                foreach (var type in topTabs)
                {
                    bool opened = topTabs[type.Key];
                    if (opened && ImGui.BeginTabItem(type.Key.Name, ref opened, ImGuiTabItemFlags.None))
                    {
                        if (show) Show(type.Key);
                        ImGui.EndTabItem();
                    }
                    typesState[type.Key] = opened;
                }

                //chage orgianl tab state
                foreach (var type in typesState)
                {
                    if (typesState[type.Key] == false)
                        topTabs.Remove(type.Key);
                }

                ImGui.EndTabBar();
            }

        }

        void TryAddTopTab(Type type)
        {
            if (topTabs.ContainsKey(type) == false)
            {
                topTabs.Add(type, true);
            }
        }

        void ShowSubViews(Type type)
        {
            foreach (IClassView view in subViews)
            {
                view.Show(type);
            }
        }

        void DrawSubViews()
        {
            foreach (IClassView view in subViews)
            {
                view.DrawView();
            }
        }
    }
}
