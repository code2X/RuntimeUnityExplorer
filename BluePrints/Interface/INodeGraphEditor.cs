using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Views;

namespace DotInsideNode
{
    using NodeGraphDropAction = Action<INodeGraphEditor,INodeGraph>;
    public interface INodeGraphEditor
    {
        event NodeGraphDropAction OnDropEvent;
        void SubmitGraph(INodeGraph nodeGraph);
        void SubmitRightMenu(IPopupMenu view);
    }

    public interface NodeEditorDroppable
    {
        void OnNodeEditorDrop(INodeGraphEditor nodeGraphEditor, INodeGraph nodeGraph);
    }
}
