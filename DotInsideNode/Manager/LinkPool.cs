using imnodesNET;
using System;
using System.Collections.Generic;

namespace DotInsideNode
{
    public struct LinkPair
    {
        public int start, end;

        public LinkPair(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
    }

    class LinkPool
    {
        static Dictionary<int, LinkPair> s_Links = new Dictionary<int, LinkPair>();
        static Dictionary<int, int> s_Start2Link = new Dictionary<int, int>();
        static Dictionary<int, int> s_End2Link = new Dictionary<int, int>();

        static Random s_Rand = new Random();

        public void AddLink(LinkPair linkPair)
        {
            int id;
            while (s_Links.ContainsKey(id = s_Rand.Next())) ;
            s_Links.Add(id, linkPair);
            s_Start2Link.Add(linkPair.start, id);
            s_End2Link.Add(linkPair.end, id);
        }

        public void Draw()
        {
            foreach (var id2link in s_Links)
            {
                imnodes.Link(id2link.Key, id2link.Value.start, id2link.Value.end);
            }
        }

        public bool RemoveLink(int link_id)
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

        public bool RemoveLinkByStart(int start)
        {
            int id;
            if (s_Start2Link.TryGetValue(start, out id))
            {
                return RemoveLink(id);
            }
            return false;
        }

        public bool RemoveLinkByEnd(int end)
        {
            int id;
            if (s_End2Link.TryGetValue(end, out id))
            {
                return RemoveLink(id);
            }
            return false;
        }

        public bool TryGetLinkIDByStart(int start, out int id)
        {
            return s_Start2Link.TryGetValue(start, out id);
        }

        public bool TryGetLinkIDByEnd(int end, out int id)
        {
            return s_End2Link.TryGetValue(end, out id);
        }

        public bool TryGetLink(int id,out LinkPair link)
        {
            return s_Links.TryGetValue(id, out link);
        }
    }

}
