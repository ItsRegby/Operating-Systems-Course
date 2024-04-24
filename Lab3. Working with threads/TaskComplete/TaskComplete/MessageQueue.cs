using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskComplete
{
    public class MessageQueue<T>
    {
        private readonly Queue<T> queue;
        private readonly int maxSize;

        public MessageQueue(int maxSize)
        {
            if (maxSize <= 0)
            {
                throw new ArgumentException("Max size should be greater than 0");
            }

            this.maxSize = maxSize;
            this.queue = new Queue<T>();
        }

        public void Add(T item)
        {
            lock (queue)
            {
                while (queue.Count >= maxSize)
                {
                    Console.WriteLine($"Thread {item} is waiting to add an item to the queue.");
                    Monitor.Wait(queue);
                    Console.WriteLine($"Thread {item} add an item to the queue.");
                }

                queue.Enqueue(item);

                Monitor.PulseAll(queue);
            }
        }

        public T Poll()
        {
            lock (queue)
            {
                while (queue.Count == 0)
                {
                    Monitor.Wait(queue);
                }

                T item = queue.Dequeue();
                Monitor.PulseAll(queue);

                return item;
            }
        }
    }
}
