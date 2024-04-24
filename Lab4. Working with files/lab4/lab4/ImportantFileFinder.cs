using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4
{
    class ImportantFileFinder
    {
        public static void FindImportantFiles(string searchDir, string extension, string nameContains)
        {
            try
            {
                var files = Directory.GetFiles(searchDir, $"*.{extension ?? "*"}", SearchOption.AllDirectories)
                    .Where(file => nameContains == null || Path.GetFileName(file).Contains(nameContains))
                    .Where(file => (File.GetAttributes(file) & FileAttributes.Hidden) == FileAttributes.Hidden);

                foreach (var file in files)
                {
                    Console.WriteLine(file);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching for files: {ex.Message}");
            }
        }
    }
}
