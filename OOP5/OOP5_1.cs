using System;
using System.Collections.Generic;
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

    public void AddSoftware(Software software)
    {
        softwareList.Add(software);
        Console.WriteLine($"Добавлено ПО: {software.Name}");
    }

    public void RemoveSoftware(string name)
    {
        var softwareToRemove = softwareList.Find(software => software.Name == name);
        if (softwareToRemove != null)
        {
            softwareList.Remove(softwareToRemove);
            Console.WriteLine($"Удалено ПО: {softwareToRemove.Name}");
        }
        else
        {
            Console.WriteLine($"ПО с именем {name} не найдено.");
        }
    }

    public void PrintSoftwareList()
    {
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
        if (index >= 0 && index < softwareList.Count)
        {
            return softwareList[index];
        }
        else
        {
            Console.WriteLine("Индекс вне диапазона.");
            return null;
        }
    }

    public void SetSoftware(int index, Software software)
    {
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

    public List<Software> GetAllSoftware()
    {
        return softwareList;
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
        _computer.AddSoftware(software);
    }

    public void UninstallSoftwareByName(string name)
    {
        _computer.RemoveSoftware(name);
    }

    public void FindSoftwareByType(SoftwareType type)
    {
        List<Software> softwareList = new List<Software>();

        foreach (var software in _computer.GetAllSoftware())
        {
            if (software.Type == type)
            {
                softwareList.Add(software);
            }
        }

        Console.WriteLine($"Найдено программ с типом {type}: {softwareList.Count}");
        foreach (var software in softwareList)
        {
            Console.WriteLine(software.Name);
        }
    }

    class SoftwareNameComparer : IComparer<Software>
    {
        public int Compare(Software x, Software y)
        {
            return string.Compare(x.Name, y.Name);
        }
    }

    public void ListSoftwareAlphabetically()
    {
        List<Software> softwareList = _computer.GetAllSoftware();

        softwareList.Sort(new SoftwareNameComparer());

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
            Software[] softwareArray = { word, minesweeper, virus};
            foreach (var software in softwareArray)
            {
                printer.IAmPrinting(software);
            }

            Computer computer = new Computer();

            computer.AddSoftware(word);
            computer.AddSoftware(minesweeper);
            computer.AddSoftware(virus);

            computer.PrintSoftwareList();

            computer.RemoveSoftware("Minesweeper");

            computer.PrintSoftwareList();

            Software softwareAtIndex = computer.GetSoftware(0);
            softwareAtIndex?.Run();

            computer.SetSoftware(0, new Virus());

            computer.PrintSoftwareList();
            Controller controller = new Controller(computer);

            Console.WriteLine("Тест InstallSoftware:");
            controller.InstallSoftware(new Word());
            computer.PrintSoftwareList();

            Console.WriteLine("Тест UninstallSoftwareByName:");
            controller.UninstallSoftwareByName("Conficker Virus");
            computer.PrintSoftwareList();

            Console.WriteLine("Тест FindSoftwareByType:");
            controller.FindSoftwareByType(SoftwareType.TextProcessor);

            Console.WriteLine("Тест ListSoftwareAlphabetically:");
            controller.ListSoftwareAlphabetically();
        }
    }
}
