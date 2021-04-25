using ImGuiNET;
using System;

namespace DotInsideNode
{
    [Serializable]
    class ParamOC : ObjectOC
    {
        IParam m_Param;

        public ParamOC(IParam param)
        {
            m_Param = param;
        }

        protected override void DrawContent()
        {
            ImGui.TextUnformatted(m_Param.Name);
        }
    }
}
