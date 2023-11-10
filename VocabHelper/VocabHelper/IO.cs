using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VocabHelper
{
    public class IO
    {
        public static void ReadFile(string path)
        {
            if (File.Exists(path))
            {
                using StreamReader sr = File.OpenText(path);
                
                while (!sr.EndOfStream)
                { Debug.WriteLine(sr.ReadLine()); }
            }
        }
    }
}
