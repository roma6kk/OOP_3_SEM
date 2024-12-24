using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Formatting = Newtonsoft.Json.Formatting;
using System.Linq;
using JsonSerializer = System.Text.Json.JsonSerializer;

public interface ISerializer
{
    void Serialize<T>(T obj, string filePath);
    T Deserialize<T>(string filePath);
}

public class BinarySerializer : ISerializer
{
    public void Serialize<T>(T obj, string filePath)
    {
        IFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            formatter.Serialize(stream, obj);
        }
    }

    public T Deserialize<T>(string filePath)
    {
        IFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(filePath, FileMode.Open))
        {
            return (T)formatter.Deserialize(stream);
        }
    }
}

public class SoapSerializer : ISerializer
{
    public void Serialize<T>(T obj, string filePath)
    {
        SoapFormatter formatter = new SoapFormatter();
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            formatter.Serialize(stream, obj);
        }
    }

    public T Deserialize<T>(string filePath)
    {
        SoapFormatter formatter = new SoapFormatter();
        using (FileStream stream = new FileStream(filePath, FileMode.Open))
        {
            return (T)formatter.Deserialize(stream);
        }
    }
}

public class JsonSerializerWrapper : ISerializer
{
    public void Serialize<T>(T obj, string filePath)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(obj, options);
        File.WriteAllText(filePath, jsonString);
    }

    public T Deserialize<T>(string filePath)
    {
        string jsonString = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<T>(jsonString);
    }
}

public class XmlSerializerWrapper : ISerializer
{
    public void Serialize<T>(T obj, string filePath)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            xmlSerializer.Serialize(stream, obj);
        }
    }

    public T Deserialize<T>(string filePath)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        using (FileStream stream = new FileStream(filePath, FileMode.Open))
        {
            return (T)xmlSerializer.Deserialize(stream);
        }
    }

}

[Serializable]
public abstract class Software
{
    public abstract string Name { get; }
    protected Software() { }

}

[Serializable]
public class Minesweeper : Software
{
    public override string Name { get; } = "Сапер";
    public string login { get; set; }
    public string Description { get; set; } = "Описание сапера"; 
    [NonSerialized]
    private string password;

    public Minesweeper(string l, string p)
    {
        this.login = l;
        this.password = p;
    }

    public Minesweeper() { }

    public override string ToString()
    {
        return $"Name: {Name}, Login: {login}";
    }
}


public class Student
{
    public string Name { get; set; }
    public int Age { get; set; }
    public int Course { get; set; }
    public Student() 
    {
        Name = "NotInitializedName";
        Age = -1;
        Course = 0;
    }
    public Student(string name, int age, int course)
    {
        Name = name;
        Age = age;
        Course = course;
    }
}




class Program
{
    static void Main(string[] args)
    {
        Minesweeper[] games = new Minesweeper[]
        {
            new Minesweeper("player1", "pass1"),
            new Minesweeper("player2", "pass2"),
            new Minesweeper("player3", "pass3")
        };

        string basePath = Directory.GetCurrentDirectory();
        ISerializer[] serializers = new ISerializer[]
        {
            new BinarySerializer(),
            new SoapSerializer(),
            new JsonSerializerWrapper(),
            new XmlSerializerWrapper()
        };

        string[] formats = { "Binary", "SOAP", "JSON", "XML" };

        for (int i = 0; i < serializers.Length; i++)
        {
            string filePath = Path.Combine(basePath, $"games_{formats[i]}.dat");

            Console.WriteLine($"\n--- {formats[i]} ---");

            serializers[i].Serialize(games, filePath);
            Console.WriteLine($"Коллекция сохранена в файл: {filePath}");

            if (i == 3)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filePath);

                XPathNavigator navigator = doc.CreateNavigator();

                Console.WriteLine("\nВсе логины:");
                XPathNodeIterator loginNodes = navigator.Select("//Minesweeper/login");
                while (loginNodes.MoveNext())
                {
                    Console.WriteLine(loginNodes.Current.Value);
                }

                Console.WriteLine("\nMinesweeper с login='player2':");
                XPathNodeIterator minesweeperNodes = navigator.Select("//Minesweeper[login='player2']");
                while (minesweeperNodes.MoveNext())
                {
                    Console.WriteLine(minesweeperNodes.Current.OuterXml);
                }

            }

            Minesweeper[] deserializedGames = serializers[i].Deserialize<Minesweeper[]>(filePath);

            Console.WriteLine("\nДесериализованные объекты:");
            foreach (var game in deserializedGames)
            {
                Console.WriteLine(game);
            }
        }
        Student[] students = new Student[4]
        {
            new Student("Иван", 20,3),
            new Student("Мария", 22,4),
            new Student("Петр", 19,1),
            new Student("Анна", 21,2)
        };
        string jsonString = JsonConvert.SerializeObject(students, Formatting.Indented);

        JArray jsonArray = JArray.Parse(jsonString);
        Console.WriteLine("\nИсходный JSON:");
        Console.WriteLine(jsonString);
        File.WriteAllText("D:\\Poman\\prog\\C#\\OOP13\\OOP13\\parsed_array.json", jsonString);
        var olderThan20 = from student in jsonArray
                          where (int)student["Age"] > 20
                          select student;

        Console.WriteLine("\nСтуденты старше 20 лет:");
        foreach (var student in olderThan20)
        {
            Console.WriteLine($"Имя: {student["Name"]}, Возраст: {student["Age"]}");
        }

        var studentIvan = from student in jsonArray
                          where (string)student["Name"] == "Иван"
                          select student;

        Console.WriteLine("\nСтуденты с именем 'Иван':");
        foreach (var student in studentIvan)
        {
            Console.WriteLine($"Имя: {student["Name"]}, Возраст: {student["Age"]}");
        }

        var youngestStudent = jsonArray.Min(student => (int)student["Age"]);
        Console.WriteLine($"\nВозраст самого младшего студента: {youngestStudent}");

        var sortedByAge = jsonArray.OrderByDescending(student => (int)student["Age"]);

        Console.WriteLine("\nСтуденты, отсортированные по возрасту:");
        foreach (var student in sortedByAge)
        {
            Console.WriteLine($"Имя: {student["Name"]}, Возраст: {student["Age"]}");
        }
    }
}
