using ImGuiNET;
using System.Reflection;
using System;

namespace DotInsideLib
{
    public class ArrayElementInputWindow : ValueInputWindowBase
    {
        public override string GetPopupName() => "Array Element Value";

        //method
        Array arrayObj;
        object elementObj;
        int elementIndex;

        private ArrayElementInputWindow() { Reset(); }
        static ArrayElementInputWindow instance = new ArrayElementInputWindow();
        public static ArrayElementInputWindow GetInstance() => instance;

        public new void Reset()
        {
            base.Reset();
            this.elementObj = null;
            this.elementIndex = 0;
        }

        public void Show(Array array, object elementObj, int elementIndex)
        {
            Reset();
            this.arrayObj = array;
            this.elementObj = elementObj;
            this.elementIndex = elementIndex;
            Caller.Try(() =>
            {
                this.inputText = elementObj.ToString();
            });
            ShowWindow();
        }

        public override void DrawPopupContent()
        {
            DrawTable("ArrayElementInputTable", elementObj.GetType(), elementIndex.ToString(), errored);

            if (ImGui.Button("OK"))
            {
                bool res = ArrayElementSetter.TrySetValue(arrayObj, elementObj.GetType(), elementIndex, inputText);
                if (res)
                {
                    errored = false;
                    CloseWindow();
                }
                else
                {
                    errored = true;
                }
            }
        }
    }
}
