
namespace DotInsideNode
{
    public class KeyNameListControllers<T> : IListController<T> where T : dnObject, new()
    {
        KeyNameList<T> m_EnumKeys;

        public KeyNameListControllers(KeyNameList<T> model)
        {
            m_EnumKeys = model;
        }

        public override int ListCount => m_EnumKeys.ObjList.Count;
        public override void AddItem() => m_EnumKeys.AddObject(new T());
        public override bool Exchange(int indexL, int indexR) => m_EnumKeys.Exchange(indexL, indexR);
        public override T GetItem(int index) => m_EnumKeys.ObjList[index];
        public override bool RemoveItem(T obj) => m_EnumKeys.RemoveObject(obj);
        public override bool Rename(int index, string newName) => m_EnumKeys.Rename(index, newName);
    }
}
