using ImGuiNET;
using System.Reflection;
using System;

namespace DotInsideLib
{
    public class PropertyValueInputWindow : ValueInputWindowBase
    {
        PropertyInfo propertyInfo;

        private PropertyValueInputWindow() { Reset(); }
        static PropertyValueInputWindow instance = new PropertyValueInputWindow();
        public static PropertyValueInputWindow GetInstance() => instance;

        public new void Reset()
        {
            base.Reset();
            this.propertyInfo = null;
        }

        public void Show(PropertyInfo propertyInfo, object parentObj = null)
        {
            Reset();
            Caller.Try(() =>
            {
                this.inputText = propertyInfo.GetValue(parentObj, null).ToString();
            });
            ShowWindow();
            this.propertyInfo = propertyInfo;
            this.parentObj = parentObj;
        }

        public override void DrawPopupContent()
        {
            DrawTable("propertyValueInputTable", propertyInfo.PropertyType, propertyInfo.Name, errored);

            if (ImGui.Button("OK"))
            {
                errored = !PropertyValueSetter.TrySetValue(propertyInfo, inputText, parentObj);
                if (errored == false)
                {
                    doSuccess();
                }
            }
        }

    }
}
