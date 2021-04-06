using ImGuiNET;
using System.Reflection;
using System;

namespace DotInsideLib
{
    public class FieldValueInputWindow : ValueInputWindowBase
    {
        FieldInfo varInfo;

        private FieldValueInputWindow() { Reset(); }
        static FieldValueInputWindow instance = new FieldValueInputWindow();
        public static FieldValueInputWindow GetInstance() => instance;

        public new void Reset()
        {
            base.Reset();
            this.varInfo = null;
        }

        public void Show(FieldInfo varInfo, object parentObj = null)
        {
            Reset();
            Caller.Try(() =>
            {
                this.inputText = varInfo.GetValue(parentObj).ToString();
            });
            ShowWindow();
            this.varInfo = varInfo;
            this.parentObj = parentObj;
        }

        public override void DrawPopupContent()
        {
            DrawTable("FieldValueInputTable", varInfo.FieldType, varInfo.Name, errored);

            if (ImGui.Button("OK"))
            {
                errored = !FieldValueSetter.TrySetValue(varInfo, inputText, parentObj);
                if (errored == false)
                {
                    doSuccess();
                }
            }
        }
    }
}
