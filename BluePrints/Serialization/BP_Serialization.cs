using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace DotInsideNode
{
    abstract class IAssetSerializer
    {
        public string AssetMagic => "DotInside";
        public string AssetFilePosfix = ".asset";
    }

    class BP_Serialization : IAssetSerializer
    {
        string GetNewName(IBluePrint bp, string dirPath)
        {
            if(File.Exists(dirPath + "/" + bp.NewBaseName + AssetFilePosfix) == false)
            {
                return bp.NewBaseName;
            }

            int index = 1;
            while (File.Exists(dirPath + "/" + bp.NewBaseName + index + AssetFilePosfix))
            {
                ++index;
            }
            return bp.NewBaseName + index;
        }

        public class ByteWriter
        {
            static byte[] ToByte(string str) => Encoding.Default.GetBytes(str);

            public static void Write(FileStream stream, byte[] bytes)
            {
                stream.Write(bytes, 0, bytes.Length);
                stream.Seek(bytes.Length, SeekOrigin.Current);
            }
            
            public static void Write(FileStream stream, string str)
            {
                byte[] bytes = ToByte(str);
                Write(stream, bytes);
            }
        }

        public class ByteReader
        {
            FileStream m_Stream;

            public ByteReader(FileStream stream)
            {
                m_Stream = stream;
            }

            public byte[] Read(int size)
            {
                byte[] res = new byte[size];
                m_Stream.Read(res, 0, size);
                m_Stream.Seek(size, SeekOrigin.Current);
                return res;
            }

            public string ReadStr(int size)
            {
                byte[] res = Read(size);
                return Encoding.Default.GetString(res);
            }

            public int ReadInt()
            {
                byte[] res = Read(4);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(res);
                return BitConverter.ToInt32(res, 0);
            }
        }

        public bool Create(IBluePrint bp,string dirPath)
        {

            Assert.IsNotNull(bp);
            Assert.IsFalse(dirPath == string.Empty);

            string fileName = GetNewName(bp, dirPath);
            string filePath = dirPath + "/" + fileName + AssetFilePosfix;

            if (File.Exists(filePath) == true)
                return false;

            string bpType = bp.GetType().Name;

            Save(bp, filePath);
            if (File.Exists(filePath) == false)
                return false;

            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryReader binaryReader = new BinaryReader(stream);
                string magic = binaryReader.ReadString();
                Assert.IsTrue(magic == AssetMagic);
                int typeSize = binaryReader.ReadInt32();
                Assert.IsTrue(typeSize == bpType.Length);
                string type = binaryReader.ReadString();
                Assert.IsTrue(type == bpType);
            }

            return true;
        }

        public bool Save(IBluePrint bp, string filePath)
        {
            string bpType = bp.GetType().Name;

            bool noError = Caller.Try(() =>
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    //Headers
                    BinaryWriter writer = new BinaryWriter(stream);
                    writer.Write(AssetMagic);
                    writer.Write(bpType.Length);
                    writer.Write(bpType);

                    //Serialize
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, bp);
                }
            });

            if(noError == false && File.Exists(filePath))
            {
                File.Delete(filePath);
                return false;
            }

            return true;
        }

        public bool Read(string filePath,out IBluePrint bp) 
        {
            bool res = false;
            bp = null;
            IBluePrint lamBP = null;

            Caller.Try(() =>
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    
                    res = Parse(stream, out IBluePrint tmp);
                    lamBP = tmp;
                }               
            });
            bp = lamBP;
            return res;
        }

        bool Parse(FileStream stream, out IBluePrint bp)
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            bp = null;

            string magic = binaryReader.ReadString();
            if(magic != AssetMagic)
                return false;

            int typeSize = binaryReader.ReadInt32();
            if (0 >= typeSize || typeSize > 50)
                return false;

            string bpType = binaryReader.ReadString();
            if (bpType.Length != typeSize)
                return false;

            bp = CreateIBluePrint(stream);

            return bp != null? true: false;
        }

        IBluePrintFactory m_bpFactory = new BluePrintFactory();

        IBluePrint CreateIBluePrint(FileStream stream)
        {
            BinaryFormatter reader = new BinaryFormatter();
            IBluePrint readdata = (IBluePrint)reader.Deserialize(stream);
            return readdata;
        }

    }

    abstract class IBluePrintFactory
    {
        public abstract IBluePrint GetBluePrint(String bpType);
    }

    class BluePrintFactory:IBluePrintFactory
    {
        public override IBluePrint GetBluePrint(String bpType)
        {
            if (bpType == string.Empty)
            {
                return null;
            }
            switch(bpType)
            {
                case "Enumeration":
                    return new BP_Enumeration();
                case "Structure":
                    return new BP_Structure();
            }

            return null;
        }
    }

}
