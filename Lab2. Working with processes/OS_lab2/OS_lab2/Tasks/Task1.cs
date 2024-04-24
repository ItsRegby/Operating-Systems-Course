using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_lab2.Tasks
{
    class Task1 : TaskBase
    {
        public Task1(string[] args) : base(args) { }

        public override void Execute()
        {
            bool ignoreCase = Array.Exists(Args, element => element == "--ignore-case");

            HashSet<string> uniqueWords = new HashSet<string>(ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

            string input;
            while ((input = Console.ReadLine()) != null)
            {
                
                string[] words = input.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                
                int i = 0;

                foreach (string word in words)
                {
                    i++;
                    if (uniqueWords.Add(word))
                    {
                        if (i % 2 == 0)
                        {
                            Console.Error.WriteLine(word);
                        }
                        else
                        {
                            Console.WriteLine(word);
                        }
                        
                    }
                }
            }
        }
    }
}
