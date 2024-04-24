using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4
{
    class FileManipulator
    {
        public static void MarkFileAsImportant(string filePath)
        {
            try
            {
                File.SetAttributes(filePath, File.GetAttributes(filePath) | FileAttributes.Hidden);
                Console.WriteLine($"{filePath} marked as important.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error marking {filePath} as important: {ex.Message}");
            }
        }

        public static void UnmarkFileAsImportant(string filePath)
        {
            try
            {
                File.SetAttributes(filePath, File.GetAttributes(filePath) & ~FileAttributes.Hidden);
                Console.WriteLine($"{filePath} unmarked as important.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error unmarking {filePath} as important: {ex.Message}");
            }
        }
    }
}
