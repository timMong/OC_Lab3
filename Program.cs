using System;
using System.Threading;
using System.Collections.Generic;
namespace LR3
{
    class Program
    {
        public const int  NUM_OF_PRODUCERS = 3;
        public const int NUM_OF_CONSUMERS = 2;
        public const int SIZE_OF_QUEUE = 200;
        //List<int> QUEUE = new List<int>();
     static int[] QUEUE = new int[SIZE_OF_QUEUE];

    static int NUM_OF_NOT_ZERO_1 = 0, NUM_OF_NOT_ZERO_2 = 0, COUNT = 0;
        static bool SOSOTOYANIYE = false;

        static Object locker_get = new Object();
        static Object locker_set = new Object();

        static public int Random()
        {
            Random rand = new Random();
            return (rand.Next(1, 101));
        }
        static public void Set(object x)
        {
            while (true)
            {
                lock (locker_set)
                {
                    for (int i = 0; i < SIZE_OF_QUEUE; i++)
                    {
                        if (QUEUE[i] != 0)
                            NUM_OF_NOT_ZERO_1++;
                    }
                    if (NUM_OF_NOT_ZERO_1 < 80 || NUM_OF_NOT_ZERO_1 <= 100)
                    {

                        QUEUE[COUNT] = Random();
                        Console.WriteLine($"{Convert.ToInt32(x) + 1} поток положил в {COUNT}: " + QUEUE[COUNT]);
                        if (COUNT < 100)
                            COUNT++;
                    }
                    if (NUM_OF_NOT_ZERO_1 > 100)
                    {
                        Thread.Sleep(200);
                    }
                    if (SOSOTOYANIYE)
                        return;
                    NUM_OF_NOT_ZERO_1 = 0;
                }
            }
        }
        static public void Get(object x)
        {
            while (true)
            {
                lock (locker_get)
                {
                    for (int i = 0; i < SIZE_OF_QUEUE; i++)
                    {
                        if (QUEUE[i] != 0)
                            NUM_OF_NOT_ZERO_2++;
                    }
                    if (NUM_OF_NOT_ZERO_2 == 0)
                    {
                        Thread.Sleep(150);
                    }
                    else
                    {

                        int element = QUEUE[COUNT];
                        if (element != 0)
                        {
                            Console.WriteLine($"{Convert.ToInt32(x) + 1} поток взял: " + element);
                            QUEUE[COUNT] = 0;
                            if (COUNT != 0)
                                COUNT--;
                        }
                    }
                    NUM_OF_NOT_ZERO_2 = 0;
                }
            }
        }
        static void Main(string[] args)
        {
            int z = 0;
            ParameterizedThreadStart[] paramProducer = new ParameterizedThreadStart[NUM_OF_PRODUCERS];
            ParameterizedThreadStart[] paramConsumer = new ParameterizedThreadStart[NUM_OF_CONSUMERS];
            Thread[] producer = new Thread[NUM_OF_PRODUCERS];
            Thread[] consumer = new Thread[NUM_OF_CONSUMERS];
            for(int i = 0; i < SIZE_OF_QUEUE; i++)
                QUEUE[i] = 0;
            for (int i = 0; i < NUM_OF_PRODUCERS; i++)
            {
                paramProducer[i] = new ParameterizedThreadStart(Set);
                producer[i] = new Thread(paramProducer[i]);
                producer[i].Start(i);
            }   
            for (int i = 0; i < NUM_OF_CONSUMERS; i++)
            {
                paramConsumer[i] = new ParameterizedThreadStart(Get);
                consumer[i] = new Thread(paramConsumer[i]);
                consumer[i].Start(i);
            }

            if(Console.ReadKey(true).Key == ConsoleKey.Q)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("! ПРОИЗВОДИТЕЛИ ЗАКОНЧИЛИ !");
                Console.ResetColor();
                SOSOTOYANIYE = true;
            }
        }
    }
}
