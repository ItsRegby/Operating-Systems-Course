using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_lab2.Tasks
{
    class Task3 : TaskBase
    {
        public Task3(string[] args) : base(args) { }

        public override void Execute()
        {
            bool keepLength = Array.Exists(Args, element => element == "--keep-len");

            string input;
            while ((input = Console.ReadLine()) != null)
            {
                string[] words = input.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i].Contains("@"))
                    {
                        words[i] = keepLength ? new string('x', words[i].Length) : "xxxxx";
                    }
                }

                Console.WriteLine(string.Join(" ", words));
            }
        }
    }
}
