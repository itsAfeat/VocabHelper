using System;
using System.Collections.Generic;
using System.Linq;

namespace VocabHelper
{
    public class CSVFile
    {
        private Dictionary<string, string[]>? CSVDict;

        public CSVFile(string[]? fileContent)
        {
            if (fileContent != null)
            {
                Dictionary<string, string[]> dict = new();
                string[] localList = new string[fileContent.Length];
                string[] foreignList = new string[fileContent.Length];

                dict.Add(fileContent[0].Split(',')[0], localList);
                dict.Add(fileContent[0].Split(',')[1], foreignList);

                for (int i = 1; i < fileContent.Length; i++)
                {
                    string[] lineArr = fileContent[i].Split(',');
                    localList[i - 1] = lineArr[0];
                    foreignList[i - 1] = lineArr[1];
                }

                CSVDict = dict;
            }
            else
            { CSVDict = null; }
        }

        public CSVFile(string path) : this(IO.ReadFile(path))
        { }

        public CSVFile()
        { }

        public string[] GetLocalList()
        {
            if (CSVDict != null)
            { return CSVDict[CSVDict.Keys.ToArray()[0]]; }
            return Array.Empty<string>();
        }

        public string[] GetForeignList()
        {
            if (CSVDict != null)
            { return CSVDict[CSVDict.Keys.ToArray()[1]]; }
            return Array.Empty<string>();
        }

        public string GetLocalName()
        {
            if (CSVDict != null)
            { return CSVDict.Keys.ToArray()[0]; }
            return string.Empty;
        }

        public string GetForeignName()
        {
            if (CSVDict != null)
            { return CSVDict.Keys.ToArray()[1]; }
            return string.Empty;
        }
    }
}
