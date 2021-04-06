using System;
using ImGuiNET;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.Threading;

namespace DotInsideLib
{
/// <summary>
/// Instance View
/// </summary>
    public class InstanceMethodView:MethodView
    {
        protected override void Draw()
        {
            if (ImGui.CollapsingHeader("Method"))
            {
                DrawMethodTable(GetClass().MethodList);
                DrawMethodTable(GetClass().GetMethodList);
                DrawMethodTable(GetClass().SetMethodList);
            }
            //methodInvokeWindow.OnGUI();
        }
    }

    class InstanceFieldView : FieldView
    {
        protected override void Draw()
        {
            if (ImGui.CollapsingHeader("Field"))
            {
                DrawInstanceFieldTable(GetClass().FieldList);
            }
            //valueInputWindow.OnGUI();
        }
    }

    class InstancePropView : PropertyView
    {
        protected override void Draw()
        {
            if (ImGui.CollapsingHeader("Property"))
            {
                DrawInstancePropertyTable(GetClass().PropList);
            }
            //valueInputWindow.OnGUI();
        }
    }

}
