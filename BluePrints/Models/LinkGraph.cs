using imnodesNET;
using System;
using System.Collections.Generic;

namespace DotInsideNode
{
    [Serializable]
    public struct LinkPair
    {
        public int start, end;

        public LinkPair(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
    }

    [Serializable]
    class LinkGraph
    {
        Dictionary<int, LinkPair> s_Links = new Dictionary<int, LinkPair>();
        Dictionary<int, List<int>> s_Start2Link = new Dictionary<int, List<int>>();
        Dictionary<int, List<int>> s_End2Link = new Dictionary<int, List<int>>();

        static Random s_Rand = new Random();

        public int LinkCount => s_Links.Count;

        public virtual int AddLink(LinkPair linkPair)
        {
            int id;
            while (s_Links.ContainsKey(id = s_Rand.Next())) ;
            s_Links.Add(id, linkPair);

            //add to Start2Link
            if (s_Start2Link.ContainsKey(linkPair.start) == false)
            {               
                s_Start2Link.Add(linkPair.start,new List<int>());
            }
            s_Start2Link[linkPair.start].Add(id);

            //add to End2Link
            if (s_End2Link.ContainsKey(linkPair.end) == false)
            {
                s_End2Link.Add(linkPair.end, new List<int>());
            }
            s_End2Link[linkPair.end].Add(id);

            return id;
        }

        public virtual void Draw()
        {
            foreach (var id2link in s_Links)
            {
                imnodes.Link(id2link.Key, id2link.Value.start, id2link.Value.end);
            }
        }

        public virtual bool RemoveLink(int link_id)
        {
            LinkPair pair;
            if (s_Links.TryGetValue(link_id, out pair))
            {
                return s_Links.Remove(link_id) &&
                        s_Start2Link.Remove(pair.start) &&
                        s_End2Link.Remove(pair.end);
            }
            return false;
        }

        public virtual bool RemoveLink(int link_id,out LinkPair pair)
        {
            if (s_Links.TryGetValue(link_id, out pair))
            {
                return s_Links.Remove(link_id) &&
                        s_Start2Link.Remove(pair.start) &&
                        s_End2Link.Remove(pair.end);
            }
            return false;
        }

        protected virtual bool RemoveLinks(List<int> links)
        {
            foreach (int link_id in links)
            {
                RemoveLink(link_id);
            }
            return false;
        }

        public virtual bool RemoveLinkByStart(int start)
        {
            List<int> links;
            if (s_Start2Link.TryGetValue(start, out links))
            {
                return RemoveLinks(links);
            }
            return false;
        }

        public virtual bool RemoveLinkByEnd(int end)
        {
            List<int> links;
            if (s_End2Link.TryGetValue(end, out links))
            {
                return RemoveLinks(links);
            }
            return false;
        }

        public virtual bool TryGetLinkIDByBegin(int start, out List<int> id)
        {
            return s_Start2Link.TryGetValue(start, out id);
        }
        
        public virtual bool TryGetLinkIDByEnd(int end, out List<int> id)
        {
            return s_End2Link.TryGetValue(end, out id);
        }

        public virtual bool TryGetLink(int id,out LinkPair link)
        {
            return s_Links.TryGetValue(id, out link);
        }

        public virtual bool IsConnect(int start,int end)
        {
            return IsConnect(new LinkPair(start, end));
        }

        public virtual bool IsConnect(LinkPair queryLink)
        {
            if(TryGetLinkIDByBegin(queryLink.start,out List<int> linkIDs))
            {
                foreach(int id in linkIDs)
                {
                    if(TryGetLink(id,out LinkPair linkPair) && linkPair.end == queryLink.end)
                    {
                        return true;
                    }
                }              
            }

            return false;
        }

    }

}
