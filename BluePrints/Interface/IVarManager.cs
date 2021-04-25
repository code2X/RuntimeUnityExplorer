using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotInsideNode
{
    public interface IVarManager : IVisitable<IVar>
    {
        void AddVar();
        void AddVar(IVar variable);
        bool ContainVar(int id);
        bool ContainVar(string name);
        bool SelectVar(int id);
        IVar GetVarByID(int id);
        IVar GetVarByName(string name);
        bool DeleteVar(string var_name);
        bool DeleteVar(int var_id);
        bool ReplaceVar(IVar old_var, IVar new_var);
        bool RenameVar(int obj_id, string new_name);
    }
}
