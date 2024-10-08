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

abstract class Software
{
    public abstract string Name { get; }
    public virtual void Run()
    {
        Console.WriteLine($"Запуск программы {Name}...");
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType() != this.GetType()) return false;

        var other = (Software)obj;
        return (this.Name == other.Name);
    }

    public override int GetHashCode()
    {
        if (this.Name.Length == 0) return 0;
        else
        {
            int HashCode = 1;
            for (int i = 0; i < this.Name.Length; i++)
            {
                if (i <= 3)
                    HashCode = HashCode * (int)this.Name[i];
                else
                    HashCode = HashCode * (int)this.Name[i] + i;
            }
            return HashCode;
        }
    }

    public override string ToString()
    {
        return $"Тип: {this.GetType().Name}, Название: {this.Name}";
    }
}

abstract class TextProcessor : Software
{
    public override string Name => "Text Processor";
}

class Word : TextProcessor, IRunnable
{
    public override string Name => "Microsoft Word";
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

    public override void Run()
    {
        Console.WriteLine("Вирус выполняет вредоносные действия...");
    }

}
class CConficker : Virus
{
    public override string Name => "Conficker Virus";

    public override void Run()
    {
        Console.WriteLine("Запуск вируса Conficker...");
    }

}

abstract class Game : Software
{
    public override string Name => "Game";

}

class Minesweeper : Game
{
    public override string Name => "Сапер";

    public override void Run()
    {
        Console.WriteLine("Запуск игры Сапер...");
    }

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


namespace OOP4
{
    internal class OOP4
    {
        static void Main(string[] args)
        {
            Software word = new Word();
            Software minesweeper = new Minesweeper();
            Software virus = new CConficker();

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
        }
    }
}
