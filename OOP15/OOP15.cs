using System;
using System.Threading;
using System.Threading.Tasks;

namespace OOP15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TPL.TaskPrime(1000000);
            Thread.Sleep(2000);

            TPL.TaskPrimeCancel(1000000);
            TPL.TaskCombination(3);
            TPL.TaskContinuationConseq(10);
            TPL.TaskContinuationAwaitResult(13);

            TPL.For();
            Thread.Sleep(2000);

            TPL.ParallelFor();
            Thread.Sleep(2000);

            TPL.ParallelForEach();

            TPL.BruteForce("kol");
            Thread.Sleep(2000);

            Good item = new Good("Бутылка рома", 40);
            Producer prod = new Producer("БК Завод", item, 3, 15);
            Buyer visiter = new Buyer("Неизвестный покупатель", item, 3);
            Store rednwhite = new Store("Красное и белое");

            rednwhite.RunStore(prod, visiter);

            TPL.Wait().GetAwaiter().GetResult();
            Thread.Sleep(10000);
        }
    }
}