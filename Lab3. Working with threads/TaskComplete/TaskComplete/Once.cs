using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskComplete
{
    public interface IOnce
    {
        void Exec(Action action);
    }

    public class Once : IOnce
    {
        private volatile int executed = 0;

        public void Exec(Action action)
        {

            if (Interlocked.CompareExchange(ref executed, 1, 0) == 0)
            {
                action();
            }
        }
    }
    public class AtomicBool
    {
        int value = 0;
        public void Set(bool newValue)
        {
            Interlocked.Exchange(ref value, newValue ? 1 : 0);
        }
        public bool Get()
        {
            return Interlocked.CompareExchange(ref value, 0, 0) == 1;
        }

        public bool CompareAndSet(bool expectedValue, bool newValue)
        {
            int expectedIntValue = expectedValue ? 1 : 0;
            int newIntValue = newValue ? 1 : 0;

            return Interlocked.CompareExchange(ref value, newIntValue, expectedIntValue) == expectedIntValue;
        }

    }
    public abstract class Service
    {
        private int stopFlag = 0;
        public void Stop()
        {
            if (Interlocked.Exchange(ref stopFlag, 1) == 0)
            {
                StopInternal();
            }
        }
        public abstract void StopInternal();
    }
    public class ServiceRealization : Service
    {
        public override void StopInternal()
        {
            Console.WriteLine("Excenge");
        }
    }
}
