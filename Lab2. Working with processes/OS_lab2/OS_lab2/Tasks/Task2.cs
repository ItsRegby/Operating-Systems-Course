using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_lab2.Tasks
{
    class Task2 : TaskBase
    {
        public Task2(string[] args) : base(args) { }

        public override void Execute()
        {
            bool positiveOnly = Array.Exists(Args, element => element == "--positive-only");

            string input;
            while ((input = Console.ReadLine()) != null)
            {
                string[] words = input.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string word in words)
                {
                    if (int.TryParse(word, out int number))
                    {
                        if (!positiveOnly || (positiveOnly && number > 0))
                        {
                            Console.WriteLine(number);
                        }
                    }
                }
            }
        }
    }
}
