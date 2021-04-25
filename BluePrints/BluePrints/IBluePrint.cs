
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotInsideNode
{
    [Serializable]
    public abstract class IBluePrint: dnObject, IView
    {
        //All diType Class Instance
        public static Dictionary<Type,Attribute> TypeClassDict = new Dictionary<Type, Attribute>();

        public static void InitClassList()
        {
            if (TypeClassDict.Count != 0)
                return;

            TypeClassDict = AttributeTools.GetNamespaceCustomAttributes(typeof(BlueprintClass));
            foreach (var container in TypeClassDict)
                Logger.Info("BluePrint Class: " + container.Key);
        }

        public abstract void Draw();

        //For judge blueprint is comileable
        public virtual bool Compileable
        {
            get => false;
        }
        public virtual void Compile() { }
        public virtual void Save(string dirPath) 
        {
            Assert.IsFalse(dirPath == string.Empty);
            Assert.IsFalse(dirPath == null);

            BP_Serialization serialization = new BP_Serialization();
            serialization.Save(this, dirPath);
        }

        //When create a new instance, need default base name to create file
        public abstract string NewBaseName
        {
            get;
        }

        public virtual string AssetPath
        {
            get; set;
        }
    }
}