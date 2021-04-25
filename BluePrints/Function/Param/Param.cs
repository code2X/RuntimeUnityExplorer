using System;
using System.Runtime.Serialization;

namespace DotInsideNode
{
    [Serializable]
    class Param : IParam
    {
        diContainer m_Container;
        [NonSerialized]
        IParamEditor m_BaseEditor;

        public Param()
        {
            m_Container = new ValueContainer();
            m_BaseEditor = new ParamDefaultDrawer(this);
            m_BaseEditor.OnContainerTypeChange += OnContainerTypeChange;
            m_BaseEditor.OnObjectTypeChange += OnVariableTypeChange;
        }

        public override Type ParamType
        {
            get => m_Container.ValueType.ValueType;
        }
        public override diContainer.EContainer ContainerType
        {
            get => m_Container.ContainerType;
        }

        #region Deserialized
        [OnDeserializedAttribute]
        protected void OnDeserialized(StreamingContext sc)
        {
            m_BaseEditor = new ParamDefaultDrawer(this);
            m_BaseEditor.OnContainerTypeChange += OnContainerTypeChange;
            m_BaseEditor.OnObjectTypeChange += OnVariableTypeChange;
        }
        #endregion

        public void OnContainerTypeChange(diContainer.EContainer SelectContainerType)
        {
            if (SelectContainerType != m_Container.ContainerType)
            {
                foreach (diContainer container in diContainer.ContainerClassList)
                {
                    if (container.ContainerType == SelectContainerType)
                    {
                        m_Container = container.DuplicateContainer();
                        Logger.Info(container.ContainerType.ToString());
                        break;
                    }
                }
                Logger.Info("ContainerTypeChange");
            }
        }

        public void OnVariableTypeChange(diType newType)
        {
            m_Container.ValueType = newType;
        }

        public override void Draw()
        {
            m_BaseEditor.Draw();
        }
    }
}

