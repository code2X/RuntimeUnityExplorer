
namespace DotInsideNode
{
    public class EnumerationController : KeyNameListControllers<IEnumItem>
    {
        public EnumerationController(KeyNameList<IEnumItem> model) : base(model)
        {
            model.NewObjectBaseName = "NewEnumertor";
        }
    }

}
