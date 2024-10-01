using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

public class Production
{
    public readonly int ID;
    public string OrganizationName;
    public Production(string name)
    {
        ID = GetHashCode();
        OrganizationName = name;
    }

}
public  class array
{
    public class Developer
    {
        public string FIO;
        public int ID;
        public string Department;
        public Developer() {
            FIO = "Ananyev Roman Vasilyevich";
            Department = "BSTU FIT 2-7";
            ID = GetHashCode();
        }
        public Developer(string fio, string department)
        {
            FIO = fio;
            ID = GetHashCode();
            Department = department;
        }
    }

    readonly Developer  me = new Developer();
    readonly Production OrgInfo = new Production("MyOrganization");

    public static int counter;
    public int[] elements;
    public array(int[] elements)
    {
        this.elements = elements;
        counter++;
    }
    public int this[int index]
    {
        get
        {
            if (index < 0 || index >= elements.Length)
            {
                Console.WriteLine("Индекс вне допустимого диапазона");
                return -1;
            }
            return elements[index];
        }
        set
        {
            if (index < 0 || index >= elements.Length)
            {
                Console.WriteLine("Индекс вне допустимого диапазона");
            }
            else
            {
                elements[index] = value;
            }
        }
    }


    public static array operator *(array a, array b)
    {
        if (a.elements.Length != b.elements.Length)
        {
            Console.WriteLine("Массивы должны быть одинаковой длины");
            return null; 
        }
        else
        {
            int[] result = new int[a.elements.Length];
            for (int i = 0; i < a.elements.Length; i++)
                result[i] = a.elements[i] * b.elements[i];
            return new array(result);
        }

    }
    public static bool operator true(array a)
    {
        if (a.elements.Length != 0)
        {

            foreach (var element in a.elements)
            {
                if (element < 0)
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            Console.WriteLine("Массив пустой или не существует");
            return false;
        }
    }
    public static bool operator false(array a) 
    {
        if(a.elements.Length != 0) 
        {
            foreach (var element in a.elements)
            {
                if (element > 0)
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            Console.WriteLine("Массив пустой или не существует");
            return false;
        }
    }

    public static bool operator ==(array a, array b)
    {
        return a.elements.SequenceEqual(b.elements);
    }
    public static bool operator !=(array a, array b)
    {
        return !a.elements.SequenceEqual(b.elements);
    }
    public static bool operator <(array a, array b)
    {
        return a.elements.Sum() < b.elements.Sum();
    }
    public static bool operator >(array a, array b)
    {
        return a.elements.Sum() > b.elements.Sum();
    }
    public static implicit operator int(array a)
    {
        return a.elements.Length;
    }
    public void Print()
    {
        foreach (var element in this.elements)
        {
            Console.Write($"{element} ");
        }
        Console.WriteLine();
    }
    public void PrintDeveloper()
    {
        
        Console.WriteLine(me.ID + ": " + me.Department + " " + me.FIO);
    }
    public void PrintProduction()
    {
        Console.WriteLine(OrgInfo.ID + ": " + OrgInfo.OrganizationName);
    }
}
public static class StatisticOperation
{
    public static int GetSum(array a)
    {
        int sum = 0;
        foreach (var i in a.elements)
        {
            sum += i;
        }
        return sum;
    }

    public static int MaxMinDiff( array a)
    {
        return a.elements.Max() - a.elements.Min();
    }
    
    public static bool IsLetter(this string input, char character)
    {
        if (input.Length == 0) return false;
        else
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (character == input[i])
                    return true;
            }
            return false;
        }
    }

    public static array RemoveNegativeElements(this array a)
    {
        List<int> positiveElements = new List<int>();
        for (int i = 0; i < a.elements.Length; i++)
        {
            if (a.elements[i] >= 0)
            {
                positiveElements.Add(a.elements[i]);
            }
        }
        return new array(positiveElements.ToArray());
    }
}

namespace OOP3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] elems = { 1,2,3,4,5,-6,7,8,9,10 };
            array arr1 = new array(elems);
            array arr2 = new array(elems);
            Console.WriteLine("arr1, arr2:");
            arr1.Print();
            Console.WriteLine("Перегрузка операции \"*\" ");
            array arr3 = arr1 * arr2;
            arr3.Print();
            if(arr1)
                Console.WriteLine("Если arr1 не содержит отрицательных элементов это сообщение выведется в консоль");
            Console.WriteLine($"arr1 > arr3? {arr1 > arr3}");
            Console.WriteLine($"arr1 == arr3? {arr1 == arr2}");
            int SizeArr1 = arr1;
            Console.WriteLine($"Выполено автоматическое приведение из array в int: {SizeArr1}");
            Console.WriteLine($"Строка STRING содержит R? {StatisticOperation.IsLetter("STRING", 'R')}");
            Console.Write($"arr3: ");
            arr3.Print();
            Console.WriteLine($"Разница между минимальным и максимальным элементом arr3: {StatisticOperation.MaxMinDiff(arr3)}");
            Console.WriteLine($"Удалим отрциательные элементы arr1: ");
            arr1 = StatisticOperation.RemoveNegativeElements(arr1);
            arr1.Print();

            Console.WriteLine("arr1:");
            arr1.PrintProduction();
            arr1.PrintDeveloper();
        }
    }
}
