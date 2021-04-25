using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotInsideNode
{
    class StructViewFactory : IViewsFactory
    {
        [NonSerialized]
        BP_Structure m_Structure = null;
        IListController<IMember> m_Controller = null;

        public StructViewFactory(BP_Structure structure, IListController<IMember> controller)
        {
            m_Structure = structure;
            m_Controller = controller;
        }

        public override List<IView> GetViews()
        {
            List<IView> views = new List<IView>();
            views.Add(new DefaultBpTopView(m_Structure));
            views.Add(new V_StructureCollapseStructure(m_Structure, m_Controller));
            views.Add(new V_StructuresCollapseDefaultValues(m_Controller));

            return views;
        }
    }
}
