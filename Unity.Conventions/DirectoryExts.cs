using System;
using System.Collections.Generic;
using System.IO;

namespace Unity.Conventions
{
    public static class DirectoryExts
    {
        public static IEnumerable<string> GetFiles(string path, string patters, SearchOption searchOption)
        {
            var result = new List<string>();
            var searchPatterns = patters.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pattern in searchPatterns)
            {
                result.AddRange(Directory.GetFiles(path, pattern, searchOption));
            }

            return result;
        }
    }
}