using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskComplete
{
    internal class NewClass
    {
        private ConsoleColor currentColor = ConsoleColor.White;
        private object colorLock = new object();
        public void SetColor(ConsoleColor color)
        {
            lock (colorLock)
            {
                this.currentColor = color;
                if (color == ConsoleColor.Red)
                {
                    Monitor.PulseAll(colorLock);
                }
            }
        }
        public void WaitRed()
        {
            lock (colorLock)
            {
                while (currentColor != ConsoleColor.Red)
                {
                    Monitor.Wait(colorLock);
                }
            }
        }
    }
}
