using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskComplete
{
    public class Warehouse
    {
        public enum ItemStatus { AVAILABLE, PRE_ORDER, UNAVAILABLE }

        public ItemStatus FetchBookStatus(string bookTitle)
        {
            // Імітація затримки в 2 секунди
            Task.Delay(2000).Wait();

            // Генерація випадкового статусу
            var values = Enum.GetValues(typeof(ItemStatus));
            var random = new Random();
            return (ItemStatus)values.GetValue(random.Next(values.Length));
        }
    }
    public class Bookstore
    {

        List<Warehouse.ItemStatus> st = new List<Warehouse.ItemStatus>();
        public void StartBooking()
        {
            Warehouse warehouse = new Warehouse();

            List<string> titles = new List<string>
            {
            "Harry Potter and the Philosopher's Stone",
            "Harry Potter and the Chamber of Secrets",
            "Harry Potter and the Prisoner of Azkaban",
            "Harry Potter and the Goblet of Fire",
            "Harry Potter and the Half-Blood Prince",
            "Harry Potter and the Deathly Hallows",
            };

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            /*// Використання Parallel.ForEach для одночасної обробки запитів
            Parallel.ForEach(titles*//*, new ParallelOptions { MaxDegreeOfParallelism = 2}*//*, title =>
            {
                threadCounter.TryAdd(Task.CurrentId ?? 0, null);

                var status = warehouse.FetchBookStatus(title);
                st.Add(status);

                Console.WriteLine($"{title} - {status}");
            });
            */
            foreach (var title in titles)
            {
                Console.WriteLine(title);
            }
            Console.WriteLine();
            foreach (var a in GetStatuses(titles, warehouse))
            {
                Console.WriteLine(a);
            }


            stopwatch.Stop();

            Console.WriteLine($"Time elapsed {stopwatch.ElapsedMilliseconds} ms");
        }
        public Dictionary<string, Warehouse.ItemStatus> GetStatuses(List<string> titles, Warehouse warehouse)
        {
            Dictionary<string, Warehouse.ItemStatus> itemStatuses = new Dictionary<string, Warehouse.ItemStatus>(titles.Count);

            int arraySize = titles.Count;
            CountdownEvent countDownLatch = new CountdownEvent(2);

            /*Dictionary<string, Warehouse.ItemStatus> firstThreadItemStatuses = new Dictionary<string, Warehouse.ItemStatus>();
            Dictionary<string, Warehouse.ItemStatus> secondThreadItemStatuses = new Dictionary<string, Warehouse.ItemStatus>();*/

            for (int i = 0; i < arraySize; i += arraySize / 2)
            {
                List<string> modifiedList = titles.GetRange(i, arraySize / 2);
                /*int priority = (i == 0) ? 2 : 1;*/

                Task.Run(() =>
                {
                    try
                    {
                        //Thread.CurrentThread.Priority = (ThreadPriority)priority;

                        foreach (var s in modifiedList)
                        {
                            var status = warehouse.FetchBookStatus(s);
                            Console.WriteLine($"{s} - {status}");

                            lock (itemStatuses)
                            {
                                itemStatuses.Add(s, status);
                            }
                            /*if (priority == 2)
                                firstThreadItemStatuses.Add(s, status);
                            else
                                secondThreadItemStatuses.Add(s, status);*/
                        }
                    }
                    finally
                    {
                        countDownLatch.Signal();
                    }
                });
            }

            countDownLatch.Wait();

            /*foreach (var kvp in firstThreadItemStatuses)
            {
                itemStatuses.Add(kvp.Key, kvp.Value);
            }

            foreach (var kvp in secondThreadItemStatuses)
            {
                itemStatuses.Add(kvp.Key, kvp.Value);
            }*/

            return itemStatuses;
        }



    }
}
