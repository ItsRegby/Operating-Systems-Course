using lab4;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: lab4 <command> <file_path> [options]");
            return;
        }

        string command = args[0];
        string filePath = args[1];

        switch (command.ToLower())
        {
            case "mark":
                FileManipulator.MarkFileAsImportant(filePath);
                break;
            case "unmark":
                FileManipulator.UnmarkFileAsImportant(filePath);
                break;
            case "find":
                FindImportantFiles(args.Skip(2).ToArray());
                break;
            default:
                Console.WriteLine("Invalid command. Use 'mark', 'unmark', or 'find'.");
                break;
        }
    }

    static void FindImportantFiles(string[] options)
    {
        string searchDir = Directory.GetCurrentDirectory();
        string extension = null;
        string nameContains = null;

        for (int i = 0; i < options.Length; i += 2)
        {
            switch (options[i].ToLower())
            {
                case "--dir":
                    searchDir = options[i + 1];
                    break;
                case "--ext":
                    extension = options[i + 1];
                    break;
                case "--name-contains":
                    nameContains = options[i + 1];
                    break;
                default:
                    Console.WriteLine($"Invalid option: {options[i]}");
                    return;
            }
        }

        ImportantFileFinder.FindImportantFiles(searchDir, extension, nameContains);
    }
}
