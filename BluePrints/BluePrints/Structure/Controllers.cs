
namespace DotInsideNode
{
    public class StructControllers : KeyNameListControllers<IMember>
    {
        public StructControllers(KeyNameList<IMember> model):base(model)
        {
            model.NewObjectBaseName = "MenmberVar_";
        }
    }
}
