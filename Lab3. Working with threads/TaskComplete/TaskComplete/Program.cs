using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TaskComplete;
using System.Text;

public class Program
{
    static int uniqueCounter = 0;
    static object counterLock = new object();
    private static ConsoleColor[] allowedColors = { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue };
    private static ConsoleColor GetConsoleColor()
    {
        return allowedColors[new Random().Next(allowedColors.Length)];
    }
    public static void Main()
    {
        int numberOf = 1;
        while (numberOf != 0)
        {
            Console.Write("Введіть номер завдання (0-вихід): ");
            numberOf = int.Parse(Console.ReadLine());
            switch (numberOf)
            {
                case 6:
                    NewClass colorTest = new NewClass();
                    for (int i = 0; i < 10; i++)
                    {
                        new Thread(() =>
                        {
                            try
                            {
                                colorTest.WaitRed();
                                Console.WriteLine("Thread woke up. Red color detected!");
                            }
                            catch (ThreadInterruptedException e)
                            {
                                throw new InvalidOperationException(e.Message, e);
                            }
                        }).Start();
                    }
                    while (true)
                    {
                        new Thread(() =>
                        {
                            try
                            {
                                ConsoleColor tempcolor = GetConsoleColor();
                                Console.ForegroundColor = tempcolor;
                                Console.WriteLine($"Color set to: {tempcolor}");
                                colorTest.SetColor(tempcolor);
                            }
                            catch (ThreadInterruptedException e)
                            {
                                throw new InvalidOperationException(e.Message, e);
                            }
                        }).Start();

                        Thread.Sleep(1000);
                    }
                    break;
                case 5:
                    Service service = new ServiceRealization();

                    var tasks = new Task[5];
                    for (int i = 0; i < tasks.Length; i++)
                    {
                        tasks[i] = Task.Run(() =>
                        {
                            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} attempting to stop the service.");
                            service.Stop();
                        });
                    }
                    break;
                case 4:
                    AtomicBool atomic = new AtomicBool();
                    if (!atomic.Get())
                    {
                        Console.WriteLine("Default value works");
                    }

                    atomic.Set(true);

                    if (atomic.Get())
                    {
                        Console.WriteLine("Modified value works");
                    }

                    atomic.Set(false);
                    if (!atomic.Get())
                    {
                        Console.WriteLine("Modified2 value works");
                    }

                    atomic.Set(false);
                    if (!atomic.Get())
                    {
                        Console.WriteLine("Modified3 value works");
                    }
                    
                    if(!atomic.CompareAndSet(true, false))
                    {
                        Console.WriteLine("Compare and Set");
                    }    
                    break;
                case 1:
                    Console.WriteLine("===================Виконання==================");
                    Bookstore bookstore = new Bookstore();
                    bookstore.StartBooking();
                    Console.WriteLine(); 
                    break;
                case 2:
                    IOnce once = new Once();

                    // Виклик методу Exec тільки один раз
                    once.Exec(() => Console.WriteLine("Task executed"));
                    once.Exec(() => Console.WriteLine("This won't be executed"));
                    once.Exec(() => Console.WriteLine("This won't be executed too"));
                    break;
                case 3:
                    MessageQueue<int> queue = new MessageQueue<int>(2);
                    HashSet<int> uniqueElements = new HashSet<int>();




                    /*// Потік, який додає елементи в чергу
                    Thread producerThread = new Thread(() =>
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            Console.WriteLine($"Producing item: {i}");
                            queue.Add(i);
                            Thread.Sleep(100);
                        }
                    });

                    // Потік, який витягує елементи з черги
                    Thread consumerThread = new Thread(() =>
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            int item = queue.Poll();
                            Console.WriteLine($"Consuming item: {item}");
                            Thread.Sleep(200);
                        }
                    });

                    producerThread.Start();
                    consumerThread.Start();

                    producerThread.Join();
                    consumerThread.Join();*/

                    Thread producer1Thread = new Thread(() => ProduceItems(queue, uniqueElements, "Producer 1"));
                    Thread producer2Thread = new Thread(() => ProduceItems(queue, uniqueElements, "Producer 2"));

                    Thread consumer1Thread = new Thread(() => ConsumeItems(queue, "Consumer 1"));
                    Thread consumer2Thread = new Thread(() => ConsumeItems(queue, "Consumer 2"));
                    Thread consumer3Thread = new Thread(() => ConsumeItems(queue, "Consumer 3"));

                    producer1Thread.Start();
                    producer2Thread.Start();
                    consumer1Thread.Start();
                    consumer2Thread.Start();
                    consumer3Thread.Start();

                    producer1Thread.Join();
                    producer2Thread.Join();
                    consumer1Thread.Join();
                    consumer2Thread.Join();
                    consumer3Thread.Join();
                    break;
            }
        }
    }
    static void ProduceItems(MessageQueue<int> queue, HashSet<int> uniqueElements, string producerName)
    {
        for (int i = 0; i < 10; i++)
        {
            int newItem;
            lock (uniqueElements)
            {
                do
                {
                    newItem = GenerateUniqueItem();
                } while (!uniqueElements.Add(newItem));
            }

            Console.WriteLine($"{producerName} producing item: {newItem}");
            queue.Add(newItem);
            Thread.Sleep(100);
        }
    }

    static int GenerateUniqueItem()
    {
        lock (counterLock)
        {
            return uniqueCounter++;
        }
    }

    static void ConsumeItems(MessageQueue<int> queue, string consumerName)
    {
        for (int i = 0; i < 10; i++)
        {
            int item = queue.Poll();
            Console.WriteLine($"{consumerName} consuming item: {item}");
            Thread.Sleep(200);
        }
    }
}