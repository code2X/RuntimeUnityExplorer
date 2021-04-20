using ImGuiNET;
using System;
using System.Collections.Generic;

namespace DotInsideNode
{
    class Function:IFunction
    {
        IFunctionGraph m_FunctionGraph = null;
        IFunctionEditor m_BaseEditor = null;

        //Input and output params
        ParamManager m_InputParams = new ParamManager();
        ParamManager m_OutputParams = new ParamManager();

        //Instance Function Entry Node 
        List<ComNodeBase> m_EntryNodes = new List<ComNodeBase>();
        List<ComNodeBase> m_ReturnNodes = new List<ComNodeBase>();
        List<ComNodeBase> m_CallNodes = new List<ComNodeBase>();

        //Param and node interface
        public override ParamManager InputParams => m_InputParams;
        public override ParamManager OutputParams => m_OutputParams;
        public override List<ComNodeBase> EntryNodes => m_EntryNodes;
        public override List<ComNodeBase> ReturnNodes => m_ReturnNodes;
        public override List<ComNodeBase> CallNodes => m_CallNodes;

        public Function()
        {
            m_FunctionGraph = new FunctionGraph(this);
            m_BaseEditor = new FunctionDefaultEditor(this);
        }

        //Function Node Interface
        public override ComNodeBase GetNewFunctionEntry()
        {
            FunctionEntryNode entryNode = new FunctionEntryNode(this);
            m_EntryNodes.Add(entryNode);
            return entryNode;
        }

        public override ComNodeBase GetNewFunctionReturn()
        {
            FunctionReturnNode returnNode = new FunctionReturnNode(this);
            m_ReturnNodes.Add(returnNode);
            return returnNode;
        }

        public override ComNodeBase GetNewFunctionCall()
        {
            FunctionCallNode callNode = new FunctionCallNode(this);
            m_CallNodes.Add(callNode);
            return callNode;
        }

        public virtual void NotifyAddInputParam(IParam param)
        {
            foreach(FunctionCallNode callNode in CallNodes)
            {
                callNode.OnAddInputParam(param);
            }
            foreach (FunctionEntryNode entryNode in EntryNodes)
            {
                entryNode.OnAddOutputParam(param);
            }
        }

        public virtual void NotifyAddOutputParam(IParam param)
        {
            foreach (FunctionCallNode callNode in CallNodes)
            {
                callNode.OnAddOutputParam(param);
            }
        }

        public override void OpenGraph()
        {
            m_FunctionGraph?.OpenGraph();
        }

        public override void CloseGraph()
        {
            m_FunctionGraph?.CloseGraph();
        }

        public override void DrawEditor()
        {
            m_BaseEditor.DrawEditor();
            //Inputs
            if (ImGui.Button("+##Inputs Create"))
            {
                Param newParam = new Param();
                InputParams.AddParam(newParam);
                NotifyAddInputParam(newParam);
                Console.WriteLine("Inputs Create");
            }
            ImGui.SameLine();
            if (ImGui.CollapsingHeader("Inputs", ImGuiTreeNodeFlags.DefaultOpen))
            {
                InputParams.DrawParamList();
            }

            //Outputs
            if (ImGui.Button("+##Outputs Create"))
            {
                Param newParam = new Param();
                OutputParams.AddParam(newParam);
                NotifyAddOutputParam(newParam);
                Console.WriteLine("Outputs Create");
            }
            ImGui.SameLine();
            if (ImGui.CollapsingHeader("Outputs", ImGuiTreeNodeFlags.DefaultOpen))
            {
                OutputParams.DrawParamList();
            }
        }

    }
}
