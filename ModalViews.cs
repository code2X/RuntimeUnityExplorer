using ImGuiNET;
using System.Reflection;

namespace ExplorerSpace
{

    public abstract class IModalInputView : IView
    {
    }

    public class ParamInputTable
    {
        static System.Numerics.Vector4 errorColor = new System.Numerics.Vector4(0.4f, 0.1f, 0.1f, 0.65f);

        public void DrawHeader()
        {
            ImGui.TableSetupScrollFreeze(0, 1); // Make top row always visible
            ImGuiUtils.TableSetupColumn("Type", "Name", "Value");
            ImGui.TableHeadersRow();
        }

        public void DrawParamRow(ParameterInfo param, ref string inputText, bool error = false)
        {
            if (error)
                ImGui.TableSetBgColor(ImGuiTableBgTarget.RowBg0 + 1, ImGui.GetColorU32(errorColor));
            ImGui.TableSetColumnIndex(0);
            ImGui.Text(param.ParameterType.Name);
            ImGui.TableSetColumnIndex(1);
            ImGui.Text(param.Name);
            ImGui.TableSetColumnIndex(2);
            ImGui.InputText("", ref inputText, 20);
        }

        public void DrawParamRow(FieldInfo param, ref string inputText, bool error = false)
        {
            if (error)
                ImGui.TableSetBgColor(ImGuiTableBgTarget.RowBg0 + 1, ImGui.GetColorU32(errorColor));
            ImGui.TableSetColumnIndex(0);
            ImGui.Text(param.FieldType.Name);
            ImGui.TableSetColumnIndex(1);
            ImGui.Text(param.Name);
            ImGui.TableSetColumnIndex(2);
            ImGui.InputText("", ref inputText, 20);
        }
    }

    public class MethodInvokeWindow : IModalInputView
    {
        static string windowName = "Method Invoke";
        static ParamInputTable paramTable = new ParamInputTable();

        bool showWindow = false;
        ParameterInfo[] parameters;
        object parentObj = null;
        bool errored = false;
        int errorRow = -1;
        string[] InputText;
        MethodInfo method;

        private MethodInvokeWindow() {}
        static MethodInvokeWindow instance = new MethodInvokeWindow();
        public static MethodInvokeWindow GetInstance() => instance;

        public void Reset()
        {
            this.method = null;
            this.errored = false;
            this.errorRow = -1;
            this.showWindow = false;
            this.parameters = null;
            this.parentObj = null;
            this.InputText = null;
        }

        public void Show(MethodInfo method, ParameterInfo[] parameters, object parentObj = null)
        {
            Reset();
            this.method = method;
            this.showWindow = true;
            this.parameters = parameters;
            this.parentObj = parentObj;

            InputText = new string[this.parameters.Length];
            for (int i = 0; i < InputText.Length; ++i)
            {
                InputText[i] = "";
            }
        }

        public override void OnGUI()
        {
            if (showWindow)
            {
                if (!ImGui.IsPopupOpen(windowName))
                    ImGui.OpenPopup(windowName);
                if (ImGui.BeginPopupModal(windowName, ref showWindow))
                {
                    DragWindow();
                    ImGui.EndPopup();
                }
            }
        }

        void DragWindow()
        {
            ImGui.BeginTable("MethodInvokeTable", 3);
            paramTable.DrawHeader();
            for (int i = 0; i < parameters.Length; ++i)
            {
                ImGui.TableNextRow();
                if(errorRow == i)
                    paramTable.DrawParamRow(parameters[i], ref InputText[i], true);
                else
                    paramTable.DrawParamRow(parameters[i], ref InputText[i]);
            }
            ImGui.EndTable();

            if (ImGui.Button("Call"))
            {
                MethodInvoke invoke = new MethodInvoke(method, parentObj);
                object outObj;
                int res = invoke.Invoke(out outObj, InputText);
                if(res == 0)    //success
                {
                    showWindow = false;
                }
                else if(res == -1)
                {
                    errored = true;
                }
                else
                {
                    errorRow = res - 1;
                }
            }

            if(errored)
            {
                ImGui.SameLine();
                ImGui.Text("Invoke Error");
            }
        }
    }

    class ValueInputWindow : IModalInputView
    {
        enum SetType
        {
            Null,
            Property,
            Field
        }

        public static string windowName = "Set Value";
        static ParamInputTable paramTable = new ParamInputTable();

        bool showWindow = false;
        FieldInfo varInfo;
        object parentObj = null;
        bool errored = false;
        string InputText = "";
        SetType setType = SetType.Null;

        public void Reset()
        {
            this.showWindow = false;
            this.varInfo = null;
            this.parentObj = null;
            this.errored = false;
            this.InputText = "";
            this.setType = SetType.Null;
        }

        public void Show(PropertyInfo propInfo, object parentObj = null)
        {
            try
            {
                this.InputText = propInfo.GetValue(parentObj).ToString();
            }
            catch (System.Exception exp)
            {
                this.InputText = "";
                Logger.Error(exp);
            }
            this.errored = false;
            this.showWindow = true;
            this.parentObj = parentObj;
            this.setType = SetType.Property;
        }

        public void Show(FieldInfo varInfo, object parentObj = null)
        {
            try
            {
                this.InputText = varInfo.GetValue(parentObj).ToString();
            }
            catch(System.Exception exp)
            {
                this.InputText = "";
                Logger.Error(exp);
            }
            this.errored = false;         
            this.showWindow = true;
            this.varInfo = varInfo;
            this.parentObj = parentObj;
            this.setType = SetType.Field;
        }

        public override void OnGUI()
        {
            if (showWindow)
            {
                if (!ImGui.IsPopupOpen(windowName))
                    ImGui.OpenPopup(windowName);
                if (ImGui.BeginPopupModal(windowName, ref showWindow))
                {
                    DragValueWindow();
                    ImGui.EndPopup();
                }
            }
        }

        void DragValueWindow()
        {
            ImGui.BeginTable("ValueInputTable", 3);
            paramTable.DrawHeader();
            ImGui.TableNextRow();
            paramTable.DrawParamRow(varInfo, ref InputText, errored);
            ImGui.EndTable();

            if (ImGui.Button("OK"))
            {
                errored = !ValueSetter.TrySetValue(varInfo,InputText, parentObj);
                if(errored == false)
                {
                    showWindow = false;
                }
            }
        }


    }

}
