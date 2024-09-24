using System;
using System.Data;
using System.Linq;

public partial class Vector
{

    public readonly int ID;
    public const int const_field = 10;
    private int Get_only { get; }
    public static int InstanceCounter { get; set; }
    private int Stat { get; set; }
    private int[] Elements { get; set; }
    private int Length { get; set; }

public int this[int index]
    {
        get
        {
            if (index >= 0 && index < Length)
            {
                return Elements[index];
            }
            else
            {
                Stat = 2; 
                return 0;
            }
        }
        set
        {
            if (index >= 0 && index < Length)
            {
                Elements[index] = value;
                Stat = 0;
            }
            else
            {
                Stat = 2; 
            }
        }
    }

    private Vector(bool isPrivate)
    {
        Elements = new int[] { 0 };
        Length = 1;
        Stat = 0;
        Console.WriteLine("Был использован закрытый конструктор");
    }
    static Vector()
    {
        InstanceCounter = 0;
        Console.WriteLine($"Статический конструктор инициализировал статическую переменную InstanceCounter = {InstanceCounter}");
    }
    public Vector() 
    {
        Random rand = new Random();
        Length = 5;
        Elements = new int[Length];
        for (int i = 0; i < Length; i++)
        {
            Elements[i] = rand.Next(1, 100); 
        }
        Stat = 0;
        ID = GetHashCode();
    }
    public Vector(int[] elements)
    {
        if (elements != null && elements.Length > 0)
        {
            this.Elements = elements;
            this.Length = elements.Length;
            this.Stat = 0;
            ID = GetHashCode();
        }
        else
        {
            this.Stat = 1;
        }
    }

    public Vector(int size = 3, int status = 0, int[] elems = null)
    {
        if (elems == null || elems.Length != size)
        {
            Elements = new int[size];
            Random rand = new Random();
            for (int i = 0; i < size; i++)
            {
                Elements[i] = rand.Next(1, 100);
            }
        }
        else
        {
            Elements = elems;
        }

        Length = size;
        ID = GetHashCode();
        Stat = status;
    }


    public void Add(ref int value)
    {
        for (int i = 0; i < Length; i++)
        {
            Elements[i] += value;
        }
        value += 10;
        Stat = 0;
    }

    public void Multiply(out int value)
    {

        value = 2;
        for (int i = 0; i < Length; i++)
        {
            Elements[i] *= value;
        }
        Stat = 0; 
    }

    public void Print()
    {
        if (Stat == 0)
        {
            Console.WriteLine("Элементы вектора: ");
            for (int i = 0; i < Length; i++)
            {
                Console.Write(Elements[i] + " ");
            }
            Console.WriteLine($"Количество экземпляров: {InstanceCounter}");
        }
        else
        
        {
            Console.WriteLine($"Ошибка: код {Stat}");
        }

    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Vector other = (Vector)obj;
        if (Length != other.Length)
        {
            return false;
        }

        for (int i = 0; i < Length; i++)
        {
            if (Elements[i] != other.Elements[i])
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        foreach (var element in Elements)
        {
            hash = hash * 31 + element;
        }
        InstanceCounter++;
        return hash;
    }

    public override string ToString()
    {
        return $"Vector: [Length: {Length}, Elements: {string.Join(", ", Elements)}]";
    }
    //---------------- Задание 3
    public bool ContainsZero()
    {
        return Elements.Contains(0);
    }

    public double GetMagnitude()
    {
        return Math.Sqrt(Elements.Sum(e => e * e));
    }

}

namespace OOP2
{
    class OOP2
    {
        static void Main(string[] args)
        {
            //----------- Задание 1
            int[] data = { 3, 4, 5, 6, 7 };
            Console.WriteLine("Был использован конструктор с параметрами");
            Vector vector = new Vector(data);
            vector.Print();

            Console.WriteLine("\nБыл использован конструктор без параметров");
            Vector vectorB = new Vector();
            Console.WriteLine($"ID: {vector.ID}");
            vectorB.Print();
            vectorB.Partial();

            Console.WriteLine("\nБыл использован конструктор с параметрами по умолчанию");
            Vector vectorC = new Vector(4);
            vectorC.Print();
            vectorC.Partial();

            int value;
            Console.WriteLine("\nВызван метод Multiply(), (выходной) параметр был передан по ссылке и инициализирован в методе");
            vector.Multiply(out value);     
            vector.Print();         

            Console.WriteLine("\nВызван метод Add(), (входной) параметр передан по ссылке");
            vector.Add(ref value);         
            vector.Print();
            //-------- Задание 2
            Console.WriteLine($"\nvectorB равен vectorC? {vectorB.Equals(vectorC)}");
            Console.WriteLine($"Тип vectorC: {vectorC.GetType()}");
            //-------- Задание 3
            Vector[] vectorArray = new Vector[]
            {
            new Vector(new int[] { 1, 2, 3 }),
            new Vector(new int[] { 0, 4, 5 }),
            new Vector(new int[] { 6, 7, 0 }),
            new Vector(new int[] { 8, 9, 10 })
            };

            Console.WriteLine("\nВекторы, содержащие 0:");
            foreach (var v in vectorArray.Where(v => v.ContainsZero()))
            {
                v.Print();
            }

            double minMagnitude = vectorArray.Min(v => v.GetMagnitude());
            Console.WriteLine("\nВекторы с наименьшим модулем:");
            foreach (var v in vectorArray.Where(v => v.GetMagnitude() == minMagnitude))
            {
                v.Print();
            }
            //-------- Задание 4
            var vectorAnonymous = new
            {
                elements = new int[] { 1, 2, 3, 4, 5 },
                length = 5,                  
                stat = 0                     
            };

            Console.Write($"Значения анонимного типа: размер: {vectorAnonymous.length}," +
                $" переменная состояния: {vectorAnonymous.stat}, элементы: ");
            foreach (var element in vectorAnonymous.elements)
            {
                Console.Write(element + " ");
            }
            Console.WriteLine();
        }
    }

}
