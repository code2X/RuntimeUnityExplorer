using imnodesNET;
using System;
using System.Collections.Generic;

namespace DotInsideNode
{
    class NodeComponentManager: ILinkEventObserver
    {
        Random s_Random = new Random();

        Dictionary<int, INodeComponent> g_Components = new Dictionary<int, INodeComponent>();
        Dictionary<int, INodeInput> g_InComponents = new Dictionary<int, INodeInput>();
        Dictionary<int, INodeOutput> g_OutComponents = new Dictionary<int, INodeOutput>();

        LinkManager m_LinkManager = LinkManager.GetInstance();

        NodeComponentManager() 
        {
            m_LinkManager.AttachEventObserver(this);
        }
        static NodeComponentManager instance = new NodeComponentManager();
        public static NodeComponentManager GetInstance() => instance;

        bool TryConnectComponet(int start, int end)
        {
            INodeInput inCom;
            INodeOutput outCom;
            if (g_InComponents.TryGetValue(end, out inCom) && g_OutComponents.TryGetValue(start, out outCom))
            {
                return inCom.TryConnectBy(outCom) && outCom.TryConnectTo(inCom);
            }
            return false;
        }

        public override void NotifyLinkCreated(int start,int end)
        {
            if (TryConnectComponet(start, end))
            {
                m_LinkManager.AddLink(new LinkPair(start, end));
            }
        }

        public override void NotifyLinkStarted(int start)
        {
            INodeInput inCom;
            INodeOutput outCom;

            if ( g_InComponents.TryGetValue(start, out inCom) )
            {
                inCom.OnLinkStart();
            }
            else if( g_OutComponents.TryGetValue(start, out outCom) )
            {
                outCom.OnLinkStart();
            }
        }

        public override void NotifyLinkDropped(int start)
        {
            INodeInput inCom;
            INodeOutput outCom;

            if (g_InComponents.TryGetValue(start, out inCom))
            {
                inCom.OnLinkDropped();
            }
            else if (g_OutComponents.TryGetValue(start, out outCom))
            {
                outCom.OnLinkDropped();
            }
        }

        public override void NotifyLinkDestroyed(int start)
        {
            INodeInput inCom;
            INodeOutput outCom;

            if (g_InComponents.TryGetValue(start, out inCom))
            {
                inCom.OnLinkDestroyed();
            }
            else if (g_OutComponents.TryGetValue(start, out outCom))
            {
                outCom.OnLinkDestroyed();
            }
        }

        public override void NotifyLinkHovered(int start)
        {
            INodeInput inCom;
            INodeOutput outCom;

            if (g_InComponents.TryGetValue(start, out inCom))
            {
                inCom.OnLinkHovered();
            }
            else if (g_OutComponents.TryGetValue(start, out outCom))
            {
                outCom.OnLinkHovered();
            }
        }

        void AddInComponet(int id, INodeInput inCom)
        {
            g_InComponents.Add(id, inCom);
        }

        void AddOutComponet(int id, INodeOutput outCom)
        {
            g_OutComponents.Add(id, outCom);
        }

        public int AddComponet(INodeComponent component)
        {
            int id;
            while (g_Components.ContainsKey(id = s_Random.Next())) ;
            g_Components.Add(id, component);

            return id;
        }

        public int AddComponet(INodeInput component)
        {
            int id = AddComponet((INodeComponent)component);
            AddInComponet(id, component);
            return id;
        }

        public int AddComponet(INodeOutput component)
        {
            int id = AddComponet((INodeComponent)component);
            AddOutComponet(id, component);
            return id;
        }
    }
}
