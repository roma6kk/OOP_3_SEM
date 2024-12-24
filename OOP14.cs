using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class ProcsManager
{
    string outputFilePath;
    public ProcsManager(string outPath)
    {
        outputFilePath = outPath;
    }
    public void getProcs()
    {
        using (StreamWriter writer = new StreamWriter(this.outputFilePath))
        {
            writer.WriteLine("Список запущенных процессов:");
            writer.WriteLine(new string('-', 80));

            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    writer.WriteLine($"ID процесса: {process.Id}");
                    writer.WriteLine($"Имя процесса: {process.ProcessName}");
                    writer.WriteLine($"Приоритет: {process.BasePriority}");
                    writer.WriteLine($"Время запуска: {(process.StartTime.ToString() ?? "Недоступно")}");
                    writer.WriteLine($"Состояние: {(process.Responding ? "Работает" : "Не отвечает")}");
                    writer.WriteLine($"Время работы процессора: {process.TotalProcessorTime}");
                    writer.WriteLine($"Память, используемая процессом: {process.WorkingSet64 / 1024 / 1024} МБ");
                    writer.WriteLine(new string('-', 80));
                }
                catch (Exception ex)
                {
                    writer.WriteLine($"Ошибка при получении данных о процессе {process.ProcessName}: {ex.Message}");
                    writer.WriteLine(new string('-', 80));
                }
            }
        }

        Console.WriteLine($"Информация о процессах записана в файл: {this.outputFilePath}");
    }
}


public class DomainManager
{
    public void ExploreCurrentDomain()
    {
        AppDomain currentDomain = AppDomain.CurrentDomain;

        Console.WriteLine("Имя текущего домена: " + currentDomain.FriendlyName);
        Console.WriteLine("Детали конфигурации:");
        Console.WriteLine(currentDomain.SetupInformation);

        Console.WriteLine("\nЗагруженные сборки:");
        foreach (Assembly assembly in currentDomain.GetAssemblies())
        {
            Console.WriteLine($"Сборка: {assembly.GetName().Name}, Версия: {assembly.GetName().Version}");
        }
    }

    public AppDomain CreateNewDomain(string domainName)
    {
        AppDomain newDomain = AppDomain.CreateDomain(domainName);
        Console.WriteLine($"\nСоздан новый домен: {newDomain.FriendlyName}");
        return newDomain;
    }

    public void LoadAssemblyIntoDomain(AppDomain domain, string assemblyPath)
    {
        try
        {
            domain.Load(AssemblyName.GetAssemblyName(assemblyPath));
            Console.WriteLine($"Сборка успешно загружена в домен {domain.FriendlyName}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки сборки в домен {domain.FriendlyName}: {ex.Message}");
        }
    }

    public void UnloadDomain(AppDomain domain)
    {
        string domainName = domain.FriendlyName;
        try
        {
            AppDomain.Unload(domain);
            Console.WriteLine($"Домен {domainName} успешно выгружен.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка выгрузки домена {domainName}: {ex.Message}");
        }
    }
}

public class PrimeNumbersTask
{
    private Thread workerThread;
    private bool isPaused;
    private readonly object pauseLock = new object();
    private int n;
    private string outputFilePath;

    public PrimeNumbersTask(int n, string outputPath)
    {
        this.n = n;
        this.outputFilePath = outputPath;
        isPaused = false;
        workerThread = new Thread(GeneratePrimes)
        {
            Name = "PrimeNumbersThread",
            Priority = ThreadPriority.Normal
        };
    }

    public void Start()
    {
        Console.WriteLine("Запуск потока...");
        workerThread.Start();
    }

    public void Pause()
    {
        Console.WriteLine("Приостановка потока...");
        lock (pauseLock)
        {
            isPaused = true;
        }
    }

    public void Resume()
    {
        Console.WriteLine("Возобновление потока...");
        lock (pauseLock)
        {
            isPaused = false;
            Monitor.PulseAll(pauseLock);
        }
    }

    public void Abort()
    {
        Console.WriteLine("Прерывание потока...");
        workerThread.Abort();
    }

    public void GetThreadInfo()
    {
        Console.WriteLine($"\nИнформация о потоке:");
        Console.WriteLine($"Имя: {workerThread.Name}");
        Console.WriteLine($"Статус: {workerThread.ThreadState}");
        Console.WriteLine($"Приоритет: {workerThread.Priority}");
        Console.WriteLine($"ID: {workerThread.ManagedThreadId}");
    }

    private void GeneratePrimes()
    {
        using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            writer.WriteLine($"Простые числа от 1 до {n}:");
            Console.WriteLine($"Простые числа от 1 до {n}:");

            for (int i = 1; i <= n; i++)
            {
                lock (pauseLock)
                {
                    while (isPaused)
                    {
                        Monitor.Wait(pauseLock);
                    }
                }

                if (IsPrime(i))
                {
                    writer.WriteLine(i);
                    Console.WriteLine(i);
                }

                Thread.Sleep(100);
            }
        }

        Console.WriteLine("Задача завершена.");
    }

    private bool IsPrime(int number)
    {
        if (number < 2) return false;
        for (int i = 2; i <= Math.Sqrt(number); i++)
        {
            if (number % i == 0) return false;
        }
        return true;
    }
}

public class EvenOddTask
{
    private int n;
    private string outputFilePath;
    private object syncLock = new object();
    private bool isEvenTurn = true;

    public EvenOddTask(int n, string outputPath)
    {
        this.n = n;
        this.outputFilePath = outputPath;
    }

    public void Start()
    {
        Thread evenThread = new Thread(PrintEvenNumbers)
        {
            Name = "EvenThread",
            Priority = ThreadPriority.Highest 
        };

        Thread oddThread = new Thread(PrintOddNumbers)
        {
            Name = "OddThread",
            Priority = ThreadPriority.Lowest 
        };

        using (StreamWriter writer = new StreamWriter(outputFilePath))
        {
            writer.WriteLine("Вывод чётных и нечётных чисел:");
        }

        evenThread.Start();
        oddThread.Start();

        evenThread.Join();
        oddThread.Join();

        Console.WriteLine("Задача завершена.");
    }

    private void PrintEvenNumbers()
    {
        for (int i = 0; i <= n; i += 2)
        {
            lock (syncLock)
            {
                while (!isEvenTurn)
                {
                    Monitor.Wait(syncLock);
                }

                WriteToOutput(i, "Чётное");
                isEvenTurn = false;
                Monitor.Pulse(syncLock);
            }

            Thread.Sleep(100); 
        }
    }

    private void PrintOddNumbers()
    {
        for (int i = 1; i <= n; i += 2)
        {
            lock (syncLock)
            {
                while (isEvenTurn)
                {
                    Monitor.Wait(syncLock);
                }

                WriteToOutput(i, "Нечётное");
                isEvenTurn = true;
                Monitor.Pulse(syncLock);
            }

            Thread.Sleep(200);
        }
    }

    private void WriteToOutput(int number, string type)
    {
        using (StreamWriter writer = new StreamWriter(outputFilePath, true))
        {
            writer.WriteLine($"{type} число: {number}");
        }
        Console.WriteLine($"{type} число: {number}");
    }
}

public class TimerTask
{
    private Timer timer;
    private string outputFilePath;
    private int intervalMilliseconds;

    public TimerTask(string outputFilePath, int intervalMilliseconds)
    {
        this.outputFilePath = outputFilePath;
        this.intervalMilliseconds = intervalMilliseconds;
    }

    public void Start()
    {
        Console.WriteLine("Запуск повторяющейся задачи...");
        timer = new Timer(WriteCurrentTimeToFile, null, 0, intervalMilliseconds);
    }

    public void Stop()
    {
        Console.WriteLine("Остановка повторяющейся задачи...");
        timer?.Dispose();
    }

    private void WriteCurrentTimeToFile(object state)
    {
        string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        lock (this)
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath, true))
            {
                writer.WriteLine($"Текущее время: {currentTime}");
            }
        }
        Console.WriteLine($"Текущее время записано в файл: {currentTime}");
    }
}

namespace OOP14
{
    internal class OOP14
    {
        static void Main(string[] args)
        {
            ProcsManager p = new ProcsManager("D:\\Poman\\prog\\C#\\OOP14\\OOP14\\bin\\Debug\\ProcsInfo.txt");
            p.getProcs();

            //===============2=============

            DomainManager domainManager = new DomainManager();

            domainManager.ExploreCurrentDomain();

            AppDomain newDomain = domainManager.CreateNewDomain("NewDomain");

            string assemblyPath = Assembly.GetExecutingAssembly().Location;
            domainManager.LoadAssemblyIntoDomain(newDomain, assemblyPath);

            domainManager.UnloadDomain(newDomain);

            //============3==============

            Console.Write("Введите число n: ");
            int n = int.Parse(Console.ReadLine());
            string outputPath = "PrimesOutput.txt";

            PrimeNumbersTask task = new PrimeNumbersTask(n, outputPath);

            task.Start();
            Thread.Sleep(500); 
            task.GetThreadInfo();

            task.Pause();
            Thread.Sleep(1000); 
            task.GetThreadInfo();

            task.Resume();
            Thread.Sleep(500);
            task.GetThreadInfo();

            task.Abort(); 
            task.GetThreadInfo();

            Console.WriteLine("Программа завершена.");

            //==============4==============

            Console.Write("Введите число n: ");
            int k = int.Parse(Console.ReadLine());

            EvenOddTask task2 = new EvenOddTask(n, "EvenOddOutput.txt");
            task2.Start();

            //==================5===========

            Console.Write("Введите интервал в миллисекундах: ");
            int intervalMilliseconds = int.Parse(Console.ReadLine());
            string outputFilePath = "TimerTaskOutput.txt";

            TimerTask timerTask = new TimerTask(outputFilePath, intervalMilliseconds);

            timerTask.Start();

            Console.WriteLine("Нажмите любую клавишу для остановки...");
            Console.ReadKey();

            timerTask.Stop();
            Console.WriteLine("Программа завершена.");
        }
    }
}
