using System;
using System.Collections.Generic;

namespace DotInsideNode
{
    abstract class IViewsFactory
    {
        public abstract List<IView> GetViews();
    }

    class EnumViewFactory : IViewsFactory
    {
        [NonSerialized]
        BP_Enumeration m_Enumeration = null;
        [NonSerialized]
        IListController<IEnumItem> m_Controller = null;

        public EnumViewFactory(BP_Enumeration enumeration, IListController<IEnumItem> controller)
        {
            m_Enumeration = enumeration;
            m_Controller = controller;
        }

        public override List<IView> GetViews()
        {
            List<IView> views = new List<IView>();
            views.Add(new DefaultBpTopView(m_Enumeration));
            views.Add(new DefaultEnumerationEnumeratorsView(m_Controller));
            views.Add(new DefaultEnumerationDescriptionView(m_Enumeration));

            return views;
        }
    }


}
