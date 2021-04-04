using ImGuiNET;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Collections;

namespace DotInsideLib
{
    public abstract class IView
    {
        public abstract void OnGUI();
        public virtual void Update() { }
    }

    public abstract class IWindowView : IView
    {
        protected bool showWindow = false;

        public override void OnGUI()
        {
            if (!showWindow) return;

            ImGui.Begin(GetWindowName(), ref showWindow, GetWindowFlags());
            DrawWindowContent();
            ImGui.End();
        }

        public void ShowWindow() => showWindow = true;
        public void CloseWindow() => showWindow = false;

        public virtual ImGuiWindowFlags GetWindowFlags() => ImGuiWindowFlags.None;
        public virtual string GetWindowName() => "ModalWindow";
        public abstract void DrawWindowContent();
    }

    public class ISingletonWindowView<T> : IWindowView
    {
        static ISingletonWindowView<T> instance = new ISingletonWindowView<T>();
        public static ISingletonWindowView<T> GetInstance() => instance;

        public override void DrawWindowContent() => throw new NotImplementedException();
    }
}
