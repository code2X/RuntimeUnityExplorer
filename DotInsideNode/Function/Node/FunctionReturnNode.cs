
namespace DotInsideNode
{
    class FunctionReturnNode : ComNodeBase
    {
        diObjectTB m_TextTitleBar;
        ExecOC m_ExecOC = new ExecOC();

        IFunction m_Function;

        public FunctionReturnNode(IFunction function)
        {
            m_TextTitleBar = new diObjectTB(function);
            m_Function = function;

            AddComponet(m_TextTitleBar);
            AddComponet(m_ExecOC);
        }
    }
}
