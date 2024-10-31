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

    public abstract class Software
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

