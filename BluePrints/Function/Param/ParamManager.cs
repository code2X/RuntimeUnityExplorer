using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotInsideNode
{
    [Serializable]
    public class ParamManager
    {
        dnObjectDictionary<IParam> m_DiObjectManager = new dnObjectDictionary<IParam>();

        public ParamManager()
        {
            m_DiObjectManager.NewObjectBaseName = "NewParam";
        }

        //Param Manager 
        public int ParamCount => m_DiObjectManager.ObjectCount;
        public void AddParam(IParam variable) => m_DiObjectManager.AddObject(variable);
        public bool ContainParam(int id) => m_DiObjectManager.ContainObject(id);
        public bool ContainParam(string name) => m_DiObjectManager.ContainObject(name);
        public bool SelectParam(int id) => false;
        public IParam GetParamByID(int id) => m_DiObjectManager.GetObjectByID(id);
        public IParam GetParamByName(string name) => m_DiObjectManager.GetObjectByName(name);
        public bool TryDeleteVar(string var_name) => m_DiObjectManager.RemoveObject(var_name);
        public bool TryDeleteVar(int var_id) => m_DiObjectManager.RemoveObject(var_id);

        public void DrawParamList()
        {
            m_DiObjectManager.Visit(DrawParam);
        }

        public void DrawParam(IParam param, out bool onEvent)
        {
            onEvent = false;
            param.Draw();
        }

        //public delegate void ParamAction(IParam param);
        public void ExecuteForEachParam(Vistors.EventVistor<IParam> paramAction)
        {
            Assert.IsNotNull(paramAction);
            m_DiObjectManager.Visit(paramAction);
        }
    }
}
