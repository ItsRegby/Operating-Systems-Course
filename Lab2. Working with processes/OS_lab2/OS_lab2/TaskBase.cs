using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_lab2
{
    abstract class TaskBase
    {
        protected string[] Args { get; }

        public TaskBase(string[] args)
        {
            Args = args;
        }

        public abstract void Execute();
    }
}
