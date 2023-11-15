﻿using System;
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
        public static string[]? ReadFile(string path)
        {
            if (File.Exists(path))
            {
                List<string> lines = new();
                using StreamReader sr = File.OpenText(path);

                while (!sr.EndOfStream)
                {
                    string? line = sr.ReadLine();
                    if (line != null)
                    { lines.Add(line); }
                }

                return lines.ToArray();
            }
            return null;
        }
    }
}
