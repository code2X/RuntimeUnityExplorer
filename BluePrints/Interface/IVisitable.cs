using System;

namespace DotInsideNode
{
    public abstract class Vistors
    {
        public delegate void Vistor<T>(T tObj);
        
        /// <param name="onEvent">If have event return true,then visit process should stop</param>
        public delegate void EventVistor<T>(T tObj, out bool onEvent);
    }

    /// <summary>
    /// Define a way to visit inside data struct
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IVisitable<T>
    {
        int TravelItemCount
        {
            get;
        }
        void Visit(Vistors.Vistor<T> vistor);
        void Visit(Vistors.EventVistor<T> vistor);
    }
}
