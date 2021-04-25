using ImGuiNET;
using System;
using System.Collections.Generic;
using NodeCom.Attr;

namespace DotInsideNode
{
    [Serializable]
    public class VarManager:IVarManager
    {
        dnObjectDictionary<IVar> m_DiObjectManager = new dnObjectDictionary<IVar>();

        public int TravelItemCount => m_DiObjectManager.ObjectCount;

        public VarManager()
        {
            m_DiObjectManager.NewObjectBaseName = "NewVar_";

            InitManagerEvent();
            diType.InitClassList();
            diContainer.InitClassList();
            ConnectValueType.InitClassList();
        }

        void InitManagerEvent()
        {
            m_DiObjectManager.OnObjectEvent += new dnObjectDictionary<IVar>.ObjectAction(ManagerEventProc);
        }

        //Var Event
        void ManagerEventProc(dnObjectDictionary<IVar>.EObjectEvent eObjectEvent, IVar @var)
        {
            switch (eObjectEvent)
            {
                case dnObjectDictionary<IVar>.EObjectEvent.Delete:
                    @var.OnVarDelete();     //Notify var delete action
                    break;
            }
        }

        bool OnSelectVar(int id)
        {
            return false;
        }

        #region Interface Impl
        public virtual void AddVar() { AddVar(new Variable()); }
        public virtual void AddVar(IVar variable) => m_DiObjectManager.AddObject(variable);
        public virtual bool ContainVar(int id) => m_DiObjectManager.ContainObject(id);
        public virtual bool ContainVar(string name) => m_DiObjectManager.ContainObject(name);
        public virtual bool SelectVar(int id) => OnSelectVar(id);
        public virtual IVar GetVarByID(int id) => m_DiObjectManager.GetObjectByID(id);
        public virtual IVar GetVarByName(string name) => m_DiObjectManager.GetObjectByName(name);
        public virtual bool DeleteVar(string var_name) => m_DiObjectManager.RemoveObject(var_name);
        public virtual bool DeleteVar(int var_id) => m_DiObjectManager.RemoveObject(var_id);
        public virtual bool ReplaceVar(IVar old_var, IVar new_var) => m_DiObjectManager.ReplaceObject(old_var, new_var);
        public virtual bool RenameVar(int obj_id, string new_name) => m_DiObjectManager.RenameObject(obj_id,new_name);
        public virtual void Visit(Vistors.EventVistor<IVar> vistor) => m_DiObjectManager.Visit(vistor);
        public virtual void Visit(Vistors.Vistor<IVar> vistor) => m_DiObjectManager.Visit(vistor);
        #endregion
    }

}
