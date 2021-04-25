using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotInsideNode
{
    public abstract class IListController<T> where T : dnObject
    {
        public abstract int ListCount
        {
            get;
        }
        public abstract T GetItem(int index);
        public abstract void AddItem();
        public abstract bool RemoveItem(T obj);
        public abstract bool Rename(int index, string newName);
        public abstract bool Exchange(int indexL, int indexR);
    }
}
