using System;
using System.IO;

namespace SchemaTests
{
    internal class Helpers
    {
        public static string LoadTestFile(string folderName, string fileName)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("SchemaTests." + folderName + "." + fileName))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
