using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace processes
{
    class ProcessManager
    {
        private readonly Dictionary<string, List<int>> processGroups = new Dictionary<string, List<int>>();

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\ninfo - Show information about all processes");
                Console.WriteLine("search - Search for a process by name or part of it");
                Console.WriteLine("kill - Stop a process by ID");
                Console.WriteLine("add - Create/Add a process to a group");
                Console.WriteLine("info - Show information about groups and their processes");
                Console.WriteLine("kill - Stop a group of processes");
                Console.WriteLine("exit - Exit the program\n");

                Console.Write(" > ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "pinfo":
                        ShowAllProcesses();
                        break;
                    case "search":
                        SearchProcess();
                        break;
                    case "kill":
                        StopProcess();
                        break;
                    case "gadd":
                        AddToGroup();
                        break;
                    case "ginfo":
                        ShowProcessGroups();
                        break;
                    case "gkill":
                        StopProcessGroup();
                        break;
                    case "exit":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid command choice. Please try again.");
                        break;
                }
            }
        }
        

        private void ShowAllProcesses()
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "wmic",
                Arguments = "process get ProcessId,ParentProcessId,Name",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(psi))
            {
                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    Console.WriteLine(output);
                    process.WaitForExit();
                }
            }
        }
        private void SearchProcess()
        {
            Console.Write("Enter the process name or part of it to search: ");
            string searchTerm = Console.ReadLine();

            Process[] processes = Process.GetProcesses()
                .Where(process => process.ProcessName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            if (processes.Length > 0)
            {
                Console.WriteLine("\n{0,-10} {1,-20} {2,-30}", "ProcessId", "ParentProcessId", "Name");
                Console.WriteLine(new string('-', 60));

                foreach (var process in processes)
                {
                    Console.WriteLine("{0,-10} {1,-20} {2,-30}", process.Id, GetParentProcessId(process), process.ProcessName);
                }
            }
            else
            {
                Console.WriteLine($"No processes found with the name or part of the name: {searchTerm}");
            }
        }
        private int GetParentProcessId(Process process)
        {
            try
            {
                using (ManagementObject managementObject = new ManagementObject($"win32_process.handle='{process.Id}'"))
                {
                    managementObject.Get();
                    int parentId = Convert.ToInt32(managementObject["ParentProcessId"]);
                    return parentId;
                }
            }
            catch
            {
                return -1;
            }
        }
        private void StopProcess()
        {
            Console.Write("Enter the process ID to stop: ");
            if (int.TryParse(Console.ReadLine(), out int processId))
            {
                StopProcessById(processId);
            }
            else
            {
                Console.WriteLine("Invalid process ID format.");
            }
        }

        private void StopProcessById(int processId)
        {
            if (ProcessExists(processId))
            {
                Process.Start("taskkill", $"/F /PID {processId}").WaitForExit();
                Console.WriteLine($"The process with ID {processId} has been stopped.");
            }
            else
            {
                Console.WriteLine($"The process with ID {processId} does not exist or has already been stopped.");
            }
        }

        private bool ProcessExists(int processId)
        {
            try
            {
                Process.GetProcessById(processId);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        private void AddToGroup()
        {
            Console.Write("Enter the process ID to add to a group: ");
            if (int.TryParse(Console.ReadLine(), out int processId))
            {
                Console.Write("Enter the group name: ");
                string groupName = Console.ReadLine();

                AddProcessToGroup(processId, groupName);
            }
            else
            {
                Console.WriteLine("Invalid process ID format.");
            }
        }

        private void AddProcessToGroup(int processId, string groupName)
        {
            if (ProcessExists(processId))
            {
                if (!processGroups.ContainsKey(groupName))
                {
                    processGroups[groupName] = new List<int>();
                }

                processGroups[groupName].Add(processId);
                Console.WriteLine($"The process with ID {processId} has been added to the group {groupName}.");
            }
            else
            {
                Console.WriteLine($"The process with ID {processId} does not exist or has already been stopped.");
            }
        }

        private void ShowProcessGroups()
        {
            foreach (var group in processGroups)
            {
                Console.WriteLine($"Group: {group.Key}");
                Console.WriteLine("Processes:");

                foreach (var processId in group.Value)
                {
                    Console.WriteLine($"  - {processId}");
                }

                Console.WriteLine();
            }
        }

        private void StopProcessGroup()
        {
            Console.Write("Enter the group name to stop: ");
            string groupName = Console.ReadLine();

            if (processGroups.ContainsKey(groupName))
            {
                foreach (var processId in processGroups[groupName])
                {
                    StopProcessById(processId);
                }
                processGroups.Remove(groupName);
                Console.WriteLine($"The process group {groupName} has been stopped.");
            }
            else
            {
                Console.WriteLine($"The process group with name {groupName} does not exist.");
            }
        }
    }
}
