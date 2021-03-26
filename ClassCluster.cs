using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExplorerSpace
{
    class DspClassCluster
    {
        public static SortedDictionary<string, SortedDictionary<string, bool>> GetCluster(SortedDictionary<string, Type> classDict)
        {
            var classCluster = new SortedDictionary<string, SortedDictionary<string, bool>>();
            classCluster.Add("UI", new SortedDictionary<string, bool>());
            classCluster.Add("Enum Class", new SortedDictionary<string, bool>());
            classCluster.Add("All Class", new SortedDictionary<string, bool>());

            var OtherDict = new SortedDictionary<string, bool>();

            //PreProcess UI And Enum
            foreach (var i in classDict)
            {
                if (i.Key.IndexOf("UI") != -1)
                {
                    classCluster["UI"].Add(i.Key, false);
                }
                else if(i.Value.IsEnum)
                {
                    classCluster["Enum Class"].Add(i.Key, false);
                }
                else
                {
                    OtherDict.Add(i.Key, false);
                }
                classCluster["All Class"].Add(i.Key, false);
            }

            //Get Name Count
            Dictionary<string, int> startNameCount = countStartName(OtherDict);
            Dictionary<string, int> endNameCount = countEndName(OtherDict);

            //Start with name cluster
            foreach (var i in startNameCount)
            {
                if (i.Value > 4 &&
                    i.Key.Length > 1 &&
                    classCluster.ContainsKey(i.Key) == false)
                {
                    classCluster.Add(i.Key, new SortedDictionary<string, bool>());
                    //Console.WriteLine(i.Key + " = " + i.Value);
                    foreach (var str2type in classDict)
                    {
                        if (str2type.Key.StartsWith(i.Key))
                        {
                            classCluster[i.Key].Add(str2type.Key, false);
                        }
                    }
                }
            }

            //Console.WriteLine("----------");

            //End with name cluster
            foreach (var i in endNameCount)
            {
                if (i.Value > 4 &&
                    i.Key.Length > 1 &&
                    classCluster.ContainsKey(i.Key) == false)
                {
                    classCluster.Add(i.Key, new SortedDictionary<string, bool>());
                    //Console.WriteLine(i.Key + " = " + i.Value);
                    foreach (var str2type in classDict)
                    {
                        if (str2type.Key.EndsWith(i.Key))
                        {
                            classCluster[i.Key].Add(str2type.Key, false);
                        }
                    }
                }
            }
            //Console.WriteLine("----------");
            //foreach (var i in classCluster["UI"])
            //{
            //    Console.WriteLine(i.Key);
            //}

            return classCluster;
        }

        private static Dictionary<string, int> countStartName<T>(SortedDictionary<string, T> dict)
        {
            Dictionary<string, int> startNameCount = new Dictionary<string, int>();

            foreach (var i in dict)
            {
                string word = fristWord(i.Key);
                if (startNameCount.ContainsKey(word))
                {
                    ++startNameCount[word];
                }
                else
                {
                    startNameCount[word] = 1;
                }
            }

            return startNameCount;
        }

        private static Dictionary<string, int> countEndName<T>(SortedDictionary<string, T> dict)
        {
            Dictionary<string, int> endNameCount = new Dictionary<string, int>();

            foreach (var i in dict)
            {
                string word = endWord(i.Key);
                if (endNameCount.ContainsKey(word))
                {
                    ++endNameCount[word];
                }
                else
                {
                    endNameCount[word] = 1;
                }
            }

            return endNameCount;
        }

        public static string fristWord(string word)
        {
            if (word.Length > 2)
            {
                for (int i = 1; i < word.Length; ++i)
                {
                    if ('A' <= word[i] && 'Z' >= word[i])
                    {
                        return word.Substring(0, i);
                    }
                }
            }
            return word;
        }

        public static string endWord(string word)
        {
            for (int i = word.Length - 1; i >= 0; --i)
            {
                if ('A' <= word[i] && 'Z' >= word[i])
                {
                    return word.Substring(i, word.Length - i);
                }
            }
            return word;
        }
    }
}
