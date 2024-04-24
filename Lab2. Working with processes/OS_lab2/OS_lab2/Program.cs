using OS_lab2.Tasks;
using OS_lab2;
using System;
using System.Collections.Generic;
using System.Linq;
class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: Please specify a task number (1-3).");
            return;
        }

        int taskNumber;
        if (!int.TryParse(args[0], out taskNumber))
        {
            Console.WriteLine("Invalid task number. Please use a number between 1 and 3.");
            return;
        }

        TaskBase task;
        switch (taskNumber)
        {
            case 1:
                task = new Task1(args);
                break;
            case 2:
                task = new Task2(args);
                break;
            case 3:
                task = new Task3(args);
                break;
            default:
                Console.WriteLine("Invalid task number. Please use a number between 1 and 3.");
                return;
        }

        task.Execute();
    }
}
