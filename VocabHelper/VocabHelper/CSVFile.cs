using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Markup;

namespace VocabHelper
{
    public class CSVFile
    {
        private Dictionary<string, string[]>? CSVDict;
        private Dictionary<char, (int, float)> letterAmountRatingDict = new();

        public CSVFile(string[]? fileContent)
        {
            if (fileContent != null)
            {
                Dictionary<string, string[]> dict = new();
                string[] localList = new string[fileContent.Length-1];
                string[] foreignList = new string[fileContent.Length-1];

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



            if (CSVDict != null)
            {
                Dictionary<char, int> letterAmount = new();
                string[] foreignList = CSVDict[CSVDict.Keys.ToArray()[1]];
                
                foreach (string word in foreignList)
                {
                    foreach (char letter in word)
                    {
                        if (letterAmountRatingDict.TryGetValue(letter, out (int, float) value))
                        { letterAmountRatingDict[letter] = (value.Item1+1, value.Item2); }
                        else
                        { letterAmountRatingDict.Add(letter, (1, 0.0f)); }
                    }
                }

                foreach (char key in letterAmountRatingDict.Keys)
                {
                    (int, float) value = letterAmountRatingDict[key];
                    value.Item2 = (float)Math.Round((float)value.Item1 / letterAmountRatingDict.Values.Count, 2);

                    letterAmountRatingDict[key] = value;
                }
            }
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
