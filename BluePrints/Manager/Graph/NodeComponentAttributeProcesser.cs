using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NodeCom.Attr;

namespace DotInsideNode
{
    class NodeComponentAttributeProcesser
    {
        //LinkManager m_LinkManager;
        INodeGraph m_NodeGraph;

        public NodeComponentAttributeProcesser(INodeGraph nodeGraph)
        {
            m_NodeGraph = nodeGraph;
            //m_LinkManager = linkManager;
        }

        void ProcessInComConnect(INodeInput com)
        {
            object[] attrs = com.GetType().GetCustomAttributes(true);
            for (int i = 0; i < attrs.Length; i++)
            {
                if (attrs[i].GetType() == typeof(SingleConnect))
                {
                    m_NodeGraph.ngLinkManager.RemoveLinkByEnd(com.ID);
                }
            }
        }

        void ProcessSingleConncect(INodeInput inCom)
        {
            SingleConnect singleConnect = (SingleConnect)Attribute.GetCustomAttribute(inCom.GetType(), typeof(SingleConnect));
            if (singleConnect != null)
            {
                singleConnect.Process(m_NodeGraph.ngLinkManager, inCom);
            }
        }
        void ProcessSingleConncect(INodeOutput outCom)
        {
            SingleConnect singleConnect = (SingleConnect)Attribute.GetCustomAttribute(outCom.GetType(), typeof(SingleConnect));
            if (singleConnect != null)
            {
                singleConnect.Process(m_NodeGraph.ngLinkManager, outCom);
            }
        }

        bool ProcessConncectTypes(INodeInput inCom, INodeOutput outCom)
        {
            ConnectTypes connectTypes;
            Type inComType = inCom.GetType();
            Type outComType = outCom.GetType();

            connectTypes = (ConnectTypes)Attribute.GetCustomAttribute(inComType, typeof(ConnectTypes));
            if (connectTypes != null)
            {
                if (connectTypes.Contains(outComType) == false)
                {
                    Logger.Info(inCom.Name + " Can't Connect Type: " + outCom.Name);
                    return false;
                }

            }

            connectTypes = (ConnectTypes)Attribute.GetCustomAttribute(outComType, typeof(ConnectTypes));
            if (connectTypes != null)
            {
                if (connectTypes.Contains(inComType) == false)
                {
                    Logger.Info(outComType.Name + " Can't Connect Type: " + inComType.Name);
                    return false;
                }
            }

            return true;
        }

        public bool ConncectValueTypeProc(INodeInput inCom, INodeOutput outCom)
        {
            Type inComType = inCom.GetType();

            ConnectValueType connectValueType = (ConnectValueType)Attribute.GetCustomAttribute(inComType, typeof(ConnectValueType));
            if (connectValueType != null)
            {
                return connectValueType.Process(m_NodeGraph, inCom, outCom);
            }

            return true;
        }

        public bool ComCanConnect(INodeInput inCom, INodeOutput outCom)
        {
            if (ProcessConncectTypes(inCom, outCom) == false)
                return false;

            if (ConncectValueTypeProc(inCom, outCom) == false)
                return false;

            ProcessSingleConncect(inCom);
            ProcessSingleConncect(outCom);

            return true;
        }
    }
}
