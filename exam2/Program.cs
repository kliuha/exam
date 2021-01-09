using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Queues
{
    class Program
    {
        public static Object theLocker = new object();

        static void Main(string[] args)
        {
            int theNumber = 0;

            var queue = new LIFOQueue(100);
         

            ThreadPool.QueueUserWorkItem((o) => PutThenPick(queue));
            ThreadPool.QueueUserWorkItem((o) => PutThenPick(queue));
            ThreadPool.QueueUserWorkItem((o) => PutThenPick(queue));


            Thread.Sleep(1000);

            Console.ReadLine();
        }

        static void PutThenPick(LIFOQueue queue)
        {
            for (int i = 0; i < 100; i++)
            {
                queue.Put(i);
                queue.Pick();
            }
            Console.WriteLine("ready");
        }
    }

    interface IQueue
    {
        // берем из очереди
        int Pick();
        // ставим в очередь
        void Put(int putIt);
    }

    // first in first out Queue
    class LIFOQueue : IQueue
    {
        // реализация хранилища на массиве
        private int[] array;
        // длинна массива
        private int length;

        public LIFOQueue(int capacity)
        {
            array = new int[capacity];
        }

        public int Count()
        {
            return this.length;
        }

        public int Pick()
        {
            lock (array)
            {
                var retIt = array[length];
                length--;
                Console.WriteLine("\tpick "+ retIt+" length "+length+" treadID: "+ Thread.CurrentThread.ManagedThreadId);
                return retIt;
            }
        }

        public void Put(int putIt)
        {
            lock (array)
            {
                if (length == array.Length)
                {
                    return;
                }
                array[length + 1] = putIt;
                length++;
                Console.WriteLine("\tpuck "+ putIt+ " length " + length + " treadID: " + Thread.CurrentThread.ManagedThreadId);
            }
        }
    }
}



