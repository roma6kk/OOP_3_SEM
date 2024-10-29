using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

interface IRunnable
{
    void Run();
}

enum SoftwareType
{
    TextProcessor,
    Game,
    Virus
}
class SoftwareException : Exception
{
    public SoftwareException() { }

    public SoftwareException(string message) : base(message) { }

    public SoftwareException(string message, Exception inner) : base(message, inner) { }
}

class InstallationException : SoftwareException
{
    public InstallationException() { }

    public InstallationException(string message) : base(message) { }

    public InstallationException(string message, Exception inner) : base(message, inner) { }
}

class UninstallationException : SoftwareException
{
    public UninstallationException() { }

    public UninstallationException(string message) : base(message) { }

    public UninstallationException(string message, Exception inner) : base(message, inner) { }
}

class InvalidSoftwareException : ArgumentException
{
    public InvalidSoftwareException() { }

    public InvalidSoftwareException(string message) : base(message) { }

    public InvalidSoftwareException(string message, Exception inner) : base(message, inner) { }
}
class InsufficientMemoryCustomException : Exception
{
    public InsufficientMemoryCustomException(string message) : base(message) { }
}
struct SoftwareInfo
{
    public string Name { get; set; }
    public SoftwareType Type { get; set; }

    public SoftwareInfo(string name, SoftwareType type)
    {
        Name = name;
        Type = type;
    }

    public override string ToString()
    {
        return $"Название: {Name}, Тип: {Type}";
    }
}
abstract partial class Software
{
    public abstract string Name { get; }
    public abstract SoftwareType Type { get; }
    public virtual void Run()
    {
        Console.WriteLine($"Запуск программы {Name}...");
    }
}

abstract class TextProcessor : Software
{
    public override string Name => "Text Processor";
}

class Word : TextProcessor, IRunnable
{
    public override string Name => "Microsoft Word";
    public override SoftwareType Type => SoftwareType.TextProcessor;

    public SoftwareInfo Info => new SoftwareInfo(Name, SoftwareType.TextProcessor);

    void IRunnable.Run()
    {
        Console.WriteLine("Реализован интерфейс IRunnable");
    }

    public override void Run()
    {
        Console.WriteLine("Запуск Microsoft Word...");
    }
}

class Virus : Software
{
    public override string Name => "Вирус";
    public override SoftwareType Type => SoftwareType.Virus;

    public SoftwareInfo Info => new SoftwareInfo(Name, SoftwareType.Virus);

    public override void Run()
    {
        Console.WriteLine("Вирус выполняет вредоносные действия...");
    }
}

class CConficker : Virus
{
    public override string Name => "Conficker Virus";

    public SoftwareInfo Info => new SoftwareInfo(Name, SoftwareType.Virus);

    public override void Run()
    {
        Console.WriteLine("Запуск вируса Conficker...");
    }
}

class Minesweeper : Game
{
    public override string Name => "Сапер";
    public override SoftwareType Type => SoftwareType.Game;

    public SoftwareInfo Info => new SoftwareInfo(Name, SoftwareType.Game);

    public override void Run()
    {
        Console.WriteLine("Запуск игры Сапер...");
    }
}

abstract class Game : Software
{
    public override string Name => "Game";

}

sealed class Developer
{
    public string Name { get; set; }

    public void Develop(Software software)
    {
        Console.WriteLine($"{Name} разрабатывает {software.Name}");
    }

    public override string ToString()
    {
        return $"Тип: {this.GetType().Name}, Имя разработчика: {this.Name}";
    }
}

class Printer
{
    public void IAmPrinting(Software someobj)
    {
        Console.WriteLine(someobj.ToString());
    }
}
class Computer
{
    private List<Software> softwareList = new List<Software>();

    public List<Software> GetAllSoftware()
    {
        return softwareList;
    }

    public void SetSoftwareList(List<Software> newSoftwareList)
    {
        softwareList = newSoftwareList;
    }
}
class Controller
{
    private Computer _computer;

    public Controller(Computer computer)
    {
        _computer = computer;
    }
    public void InstallSoftware(Software software)
    {
        Debug.Assert(software != null, "ПО не может быть null");

        var softwareList = _computer.GetAllSoftware();

        try
        {
            ValidateSoftware(software);
        }
        catch (InvalidSoftwareException ex)
        {
            Console.WriteLine($"Логирование на уровне InstallSoftware: {ex.Message}");
            throw;
        }

        if (software.Type == SoftwareType.Virus)
        {
            throw new InstallationException($"Программное обеспечение '{software.Name}' не может быть установлено, так как это вирус.");
        }

        if (softwareList.Count >= 4)
        {
            throw new InsufficientMemoryCustomException("Недостаточно памяти для установки нового программного обеспечения.");
        }

        softwareList.Add(software);
        Console.WriteLine($"Добавлено ПО: {software.Name}");
    }
    private void ValidateSoftware(Software software)
    {
        try
        {
            if (software == null)
            {
                throw new InvalidSoftwareException("Программное обеспечение не может быть null.");
            }
        }
        catch (InvalidSoftwareException ex)
        {
            Console.WriteLine($"Логирование на уровне ValidateSoftware: {ex.Message}");
            throw; 
        }
    }

    public void UninstallSoftwareByName(string name)
    {
        var softwareList = _computer.GetAllSoftware();
        var softwareToRemove = softwareList.Find(software => software.Name == name);
        if (softwareToRemove != null)
        {
            softwareList.Remove(softwareToRemove);
            Console.WriteLine($"Удалено ПО: {softwareToRemove.Name}");
        }
        else
        {
            throw new UninstallationException($"Программное обеспечение с именем '{name}' не найдено.");
        }
    }



    public void PrintSoftwareList()
    {
        var softwareList = _computer.GetAllSoftware();
        if (softwareList.Count == 0)
        {
            Console.WriteLine("Список программ пуст.");
        }
        else
        {
            Console.WriteLine("Список установленных программ:");
            foreach (var software in softwareList)
            {
                Console.WriteLine($"- {software.Name}");
            }
        }
    }

    public Software GetSoftware(int index)
    {
        var softwareList = _computer.GetAllSoftware();
        if (index >= 0 && index < softwareList.Count)
        {
            return softwareList[index];
        }
        else
        {
            throw new IndexOutOfRangeException("Индекс вне диапазона.");
        }
    }

    public void SetSoftware(int index, Software software)
    {
        var softwareList = _computer.GetAllSoftware();
        if (index >= 0 && index < softwareList.Count)
        {
            softwareList[index] = software;
            Console.WriteLine($"ПО на индексе {index} обновлено на {software.Name}");
        }
        else
        {
            Console.WriteLine("Индекс вне диапазона.");
        }
    }

    public void FindSoftwareByType(SoftwareType type)
    {
        var softwareList = _computer.GetAllSoftware().Where(s => s.Type == type).ToList();
        Console.WriteLine($"Найдено программ с типом {type}: {softwareList.Count()}");
        foreach (var software in softwareList)
        {
            Console.WriteLine(software.Name);
        }
    }

    public void ListSoftwareAlphabetically()
    {
        var softwareList = _computer.GetAllSoftware().OrderBy(s => s.Name).ToList();
        Console.WriteLine("Список программ в алфавитном порядке:");
        foreach (var software in softwareList)
        {
            Console.WriteLine(software.Name);
        }
    }
}

namespace OOP4
{
    internal class OOP4
    {
        static void Main(string[] args)
        {
            Word word = new Word();
            Minesweeper minesweeper = new Minesweeper();
            CConficker virus = new CConficker();

            Console.WriteLine(word.Info);
            Console.WriteLine(minesweeper.Info);
            Console.WriteLine(virus.Info);

            IRunnable runnableWord = word as IRunnable;
            runnableWord.Run();

            if (runnableWord is Word)
            {
                Console.WriteLine("Это Microsoft Word.");
            }

            Word wordApp = runnableWord as Word;
            if (wordApp != null)
            {
                wordApp.Run();
            }

            Developer developer = new Developer { Name = "Developer_name" };
            developer.Develop(word);
            developer.Develop(minesweeper);
            developer.Develop(virus);

            Printer printer = new Printer();
            Software[] softwareArray = { word, minesweeper, virus };
            foreach (var software in softwareArray)
            {
                printer.IAmPrinting(software);
            }

            Computer computer = new Computer();

            Controller controller = new Controller(computer);

            try
            {
                controller.InstallSoftware(null);
            }
            catch (InvalidSoftwareException ex)
            {
                Console.WriteLine($"Логирование на уровне Main: {ex.Message}");
            }

            try
            {
                controller.InstallSoftware(virus);
            }
            catch (InstallationException ex)
            {
                Console.WriteLine($"Исключение при установке: {ex.Message}");
                Console.WriteLine($"Место: {nameof(controller.InstallSoftware)}");
                Console.WriteLine($"Диагностика: {ex.StackTrace}");
            }
            finally
            {
                Console.WriteLine("Завершение попытки установки ПО.");
            }

            try
            {
                controller.UninstallSoftwareByName("NonExistentSoftware"); 
            }
            catch (UninstallationException ex)
            {
                Console.WriteLine($"UninstallationException: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Завершение попытки удаления ПО.");
            }

            try
            {
                controller.InstallSoftware(word);
                controller.InstallSoftware(minesweeper);
                controller.InstallSoftware(new Word());
                controller.InstallSoftware(new Word());
                controller.InstallSoftware(new Word());

            }
            catch (InsufficientMemoryCustomException ex)
            {
                Console.WriteLine($"InsufficientMemoryCustomException: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Завершение попытки установки ПО.");
            }


            controller.PrintSoftwareList();

            Software softwareAtIndex = controller.GetSoftware(0);
            softwareAtIndex?.Run();
            try
            {
                softwareAtIndex = controller.GetSoftware(10);
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"Исключение при доступе к индексу: {ex.Message}");
                Console.WriteLine($"Место: {nameof(controller.GetSoftware)}");
                Console.WriteLine($"Диагностика: {ex.StackTrace}");
            }
            finally
            {
                Console.WriteLine("Попытка получить приложение по индексу завершена.");
            }
            softwareAtIndex?.Run();

            controller.SetSoftware(0, new Virus());

            controller.PrintSoftwareList();

        }
    }
}
