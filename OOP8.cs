using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public delegate void UserEventHandler(int parameter);

public class User
{
    public string Name { get; set; }
    public int Position { get; set; }
    public double Size { get; set; }

    public event UserEventHandler Move;
    public event UserEventHandler Compress;

    public User(string name, int initialPosition = 0, double initialSize = 1.0)
    {
        Name = name;
        Position = initialPosition;
        Size = initialSize;
    }

    public void OnMove(int offset)
    {
        Move?.Invoke(offset);
    }

    public void OnCompress(int factor)
    {
        Compress?.Invoke(factor);
    }

    public void RegisterEvents()
    {
        Move += offset => Position += offset;
        Compress += factor => Size *= factor;
    }

    public override string ToString()
    {
        return $"User {Name}: Position = {Position}, Size = {Size:F2}";
    }
}


public class StringProcessor
{
    public static string RemovePunctuation(string input)
    {
        return new string(Array.FindAll(input.ToCharArray(), c => !char.IsPunctuation(c)));
    }

    public static string AddSymbols(string input, string prefix = "<", string suffix = ">")
    {
        return $"{prefix}{input}{suffix}";
    }

    public static string ToUpperCase(string input)
    {
        return input.ToUpper();
    }

    public static string RemoveExtraSpaces(string input)
    {
        return string.Join(" ", input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
    }

    public static bool IsStringLongerThan(string input, int length)
    {
        return input.Length > length;
    }
}

namespace OOP8
{
    internal class OOP8
    {
        static void Main(string[] args)
        {
            var user1 = new User("Пользователь 1");
            var user2 = new User("Пользователь 2");
            var user3 = new User("Пользователь 3");

            user1.RegisterEvents();
            user2.Move += offset => user2.Position += offset; 
            user3.Compress += factor => user3.Size *= factor;

            Console.WriteLine("Пользователи :");
            Console.WriteLine(user1);
            Console.WriteLine(user2);
            Console.WriteLine(user3);

            Console.WriteLine("\nВызов событий...");
            user1.OnMove(10);
            user1.OnCompress(2);
            user2.OnMove(5);
            user3.OnCompress(3);

            Console.WriteLine("\nПользователи:");
            Console.WriteLine(user1);
            Console.WriteLine(user2);
            Console.WriteLine(user3);

            string text = "  Hello, world! This is an example...  ";
            Console.WriteLine($"Текст: {text}");
            Func<string, string> processString = StringProcessor.RemovePunctuation;
            processString += input => StringProcessor.AddSymbols(input, "[", "]");
            processString += StringProcessor.ToUpperCase;
            processString += StringProcessor.RemoveExtraSpaces;

            foreach (Func<string, string> method in processString.GetInvocationList())
            {
                text = method(text);
                Console.WriteLine($"Текст: {text}");
            }

            Predicate<string> checkLength = str => StringProcessor.IsStringLongerThan(str, 10);
            Console.WriteLine($"\nОбработанная строка длиннее 10 символов? {checkLength(text)}");
        }
    }
}
