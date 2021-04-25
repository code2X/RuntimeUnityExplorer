using System;

namespace DotInsideNode
{
    public interface IFunctionManager: IVisitable<IFunction>
    {
        void AddFunction();
        void AddFunction(IFunction variable);
        bool ContainFunction(int id);
        bool ContainFunction(string name);
        bool SelectFunction(int id);
        IFunction GetFunctionByID(int id);
        IFunction GetFunctionByName(string name);
        bool DeleteFunction(string var_name);
        bool DeleteFunction(int var_id);
        bool ReplaceFunction(IFunction oldFunc, IFunction new_var);
        bool RenameFunction(int obj_id, string new_name);
    }

    [Serializable]
    class FunctionManager: IFunctionManager
    {
        dnObjectDictionary<IFunction> m_DiObjectManager = new dnObjectDictionary<IFunction>();

        public int TravelItemCount => m_DiObjectManager.ObjectCount;

        public FunctionManager()
        {
            m_DiObjectManager.NewObjectBaseName = "NewFunction_";
            m_DiObjectManager.OnObjectEvent += ManagerEventProc;
        }

        //Var Event
        void ManagerEventProc(dnObjectDictionary<IFunction>.EObjectEvent eObjectEvent, IFunction function)
        {
            switch(eObjectEvent)
            {
                case dnObjectDictionary<IFunction>.EObjectEvent.Delete:
                    break;
            }            
        }

        bool OnSelectFunction(int id)
        {
            return false;
        }

        //Function Manager 
        public virtual void AddFunction() => m_DiObjectManager.AddObject(new Function());
        public virtual void AddFunction(IFunction variable) => m_DiObjectManager.AddObject(variable);
        public virtual bool ContainFunction(int id) => m_DiObjectManager.ContainObject(id);
        public virtual bool ContainFunction(string name) => m_DiObjectManager.ContainObject(name);
        public virtual bool SelectFunction(int id) => OnSelectFunction(id);
        public virtual IFunction GetFunctionByID(int id) => m_DiObjectManager.GetObjectByID(id);
        public virtual IFunction GetFunctionByName(string name) => m_DiObjectManager.GetObjectByName(name);
        public virtual bool DeleteFunction(string var_name) => m_DiObjectManager.RemoveObject(var_name);
        public virtual bool DeleteFunction(int var_id) => m_DiObjectManager.RemoveObject(var_id);

        public virtual void Visit(Vistors.EventVistor<IFunction> vistor) => m_DiObjectManager.Visit(vistor);
        public void Visit(Vistors.Vistor<IFunction> vistor) => m_DiObjectManager.Visit(vistor);

        public virtual bool ReplaceFunction(IFunction oldFunc, IFunction newFunc) => m_DiObjectManager.ReplaceObject(oldFunc, newFunc);
        public virtual bool RenameFunction(int funcID, string newName) => m_DiObjectManager.RenameObject(funcID, newName);       
    }
}
