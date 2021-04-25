using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using BP.FunctionLibrary;

namespace DotInsideNode
{
    [Serializable]
    [BlueprintClass("Blueprint Function Library")]
    class BP_FunctionLibrary : IBP_FunctionLibrary
    {
        public override string NewBaseName => "NewFunctionLibray";

        #region MVC
        FunctionManager m_FunctionManager = new FunctionManager();

        [NonSerialized]
        DefaultController m_Controller;
        #endregion

        #region Deserialized
        [OnDeserializedAttribute]
        protected void OnDeserialized(StreamingContext sc)
        {
            ResetViewController();
        }
        #endregion

        static BP_FunctionLibrary __instance = new BP_FunctionLibrary();
        public static BP_FunctionLibrary Instance
        {
            get => __instance;
            set => __instance = value;
        }

        public BP_FunctionLibrary()
        {
            ResetViewController();
        }

        public virtual void ResetViewController()
        {
            m_Controller = new DefaultController(this, m_FunctionManager);
        }

        public override void Draw()
        {
            m_Controller.Draw();
        }
    }
}
