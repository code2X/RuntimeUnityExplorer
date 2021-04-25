
namespace DotInsideNode
{
    public interface INodeGraph
    {
        void OpenGraph();
        void CloseGraph();

        LinkManager ngLinkManager
        {
            get;
        }

        NodeManager ngNodeManager
        {
            get;
        }
        EditorNodeComponentManager ngNodeComponentManager
        {
            get;
        }

        void Draw();
        void Update();
    }
}
