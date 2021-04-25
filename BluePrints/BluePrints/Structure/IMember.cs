using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotInsideNode
{
    [Serializable]
    public class IMember : dnObject
    {
        diContainer m_Member = new ValueContainer();

        [NonSerialized]
        IMemberEditor m_Editor;

        public diContainer Container => m_Member;

        public Type MemberType
        {
            get => m_Member.ValueType.ValueType;
        }

        public diContainer.EContainer ContainerType
        {
            get => m_Member.ContainerType;
        }
        public IMemberEditor Editor => m_Editor;

        [OnDeserializedAttribute]
        private void OnDeserialized(StreamingContext sc)
        {
            ResetEditor();
        }

        void ResetEditor()
        {
            m_Editor = new DefaultMemberEditor(this);
            m_Editor.OnObjectTypeChange += OnVariableTypeChange;
            m_Editor.OnContainerTypeChange += OnContainerTypeChange;
        }

        public IMember()
        {
            ResetEditor();
        }

        public void OnContainerTypeChange(diContainer.EContainer SelectContainerType)
        {
            if (SelectContainerType != m_Member.ContainerType)
            {
                foreach (diContainer container in diContainer.ContainerClassList)
                {
                    if (container.ContainerType == SelectContainerType)
                    {
                        m_Member = container.DuplicateContainer();
                        Logger.Info(container.ContainerType.ToString());
                        break;
                    }
                }
                Logger.Info("ContainerTypeChange");
            }
        }

        public void OnVariableTypeChange(diType newType)
        {
            m_Member.ValueType = newType;
        }

        //Param Editor
        public abstract class IMemberEditor : IContainerEditor
        {
            public abstract void DrawMemberType();
            public abstract void DrawContainerType();
        }

        public class DefaultMemberEditor : IMemberEditor
        {
            IMember m_Param;

            public DefaultMemberEditor(IMember param)
            {
                m_Param = param;
            }

            public override void DrawMemberType()
            {
                DrawdiType(m_Param.MemberType, "##VariableType" + m_Param.Name);
            }

            public override void DrawContainerType()
            {
                DrawContainerType(m_Param.ContainerType, m_Param.ContainerType.ToString() + "##" + m_Param.Name);
            }
        }

    }
}
