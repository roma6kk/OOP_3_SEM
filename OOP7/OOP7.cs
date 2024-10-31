using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
public interface IRepository<T>
{
    void Add(T item);
    void Remove(T item);
    T Get(int id);
}

public class CollectionType<T> : IRepository<T> where T : IComparable<T>
{
    private readonly List<T> _items = new List<T>();

    public void Add(T item)
    {
        try
        {
            _items.Add(item);
            Console.WriteLine($"Item added: {item}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding item: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Add operation completed.");
        }
    }

    public void Remove(T item)
    {
        try
        {
            if (_items.Contains(item))
            {
                _items.Remove(item);
                Console.WriteLine($"Item removed: {item}");
            }
            else
            {
                Console.WriteLine("Item not found in collection.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing item: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Remove operation completed.");
        }
    }

    public T Get(int id)
    {
        try
        {
            if (id >= 0 && id < _items.Count)
            { 
                return _items[id];
            }
            else
            {
                throw new IndexOutOfRangeException("Invalid index.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving item: {ex.Message}");
            return default;
        }
        finally
        {
            Console.WriteLine("Get operation completed.");
        }
    }

    public T FindByPredicate(Func<T, bool> predicate)
    {
        try
        {
            return _items.FirstOrDefault(predicate);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during search: {ex.Message}");
            return default;
        }
        finally
        {
            Console.WriteLine("Search operation completed.");
        }
    }
    public void SaveToFile(string filePath)
    {
        try
        {
            string json = JsonConvert.SerializeObject(_items, Formatting.Indented);
            File.WriteAllText(filePath, json);
            Console.WriteLine("Data saved successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data: {ex.Message}");
        }
    }

    public void LoadFromFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                _items.Clear();
                _items.AddRange(JsonConvert.DeserializeObject<List<T>>(json));
                Console.WriteLine("Data loaded successfully.");
            }
            else
            {
                Console.WriteLine("File not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading data: {ex.Message}");
        }
    }
}

public class  SoftwareCollection<T> where T : Software
{
    private List<T> items = new List<T>();

    public void Add(T item)
    {
        items.Add(item);
        Console.WriteLine($"{item.Name} added in collection.");
    }

    public void Remove(T item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Console.WriteLine($"{item.Name} removed from collection.");
        }
        else
        {
            Console.WriteLine("Element not found.");
        }
    }

    public T Get(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            Console.WriteLine("Index out of range.");
            return null;
        }
        return items[index];
    }

    public T FindByPredicate(Func<T, bool> predicate)
    {
        return items.Find(new Predicate<T>(predicate));
    }

}

namespace OOP7
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Тестирование generic int коллекции ===");
            var intCollection = new CollectionType<int>();
            intCollection.Add(10);
            intCollection.Add(20);
            intCollection.Add(30);
            intCollection.Remove(20);
            Console.WriteLine($"Полученный int с индексом 0: {intCollection.Get(0)}");
            Console.WriteLine($"Поиск по предикату: (x == 30): {intCollection.FindByPredicate(x => x == 30)}");
            intCollection.SaveToFile("JSON_COLLECTION.json");

            var newIntCollection = new CollectionType<int>();
            newIntCollection.LoadFromFile("JSON_COLLECTION.json");
            Console.WriteLine($"Полученный int с индексом 0 из загруженного файла: {newIntCollection.Get(0)}");
            Console.WriteLine($"Поиск по предикату из загруженного файла: (x == 30): {newIntCollection.FindByPredicate(x => x == 30)}");
            Console.WriteLine("\n=== Тестирование generic double коллекции ===");
            var doubleCollection = new CollectionType<double>();
            doubleCollection.Add(10.5);
            doubleCollection.Add(20.5);
            doubleCollection.Add(30.75);
            doubleCollection.Remove(20.5);
            Console.WriteLine($"Полученный double с индексом 0: {doubleCollection.Get(0)}");
            Console.WriteLine($"Поиск по предикату (x > 10): {doubleCollection.FindByPredicate(x => x > 10)}");

            Console.WriteLine("\n=== Тестирование generic string коллекции ===");
            var stringCollection = new CollectionType<string>();
            stringCollection.Add("Hello");
            stringCollection.Add("World");
            stringCollection.Add("Generic");
            stringCollection.Remove("World");
            Console.WriteLine($"Полученный string с индексом 0: {stringCollection.Get(0)}");
            Console.WriteLine($"Поиск по предикату: (x.StartsWith('G')): {stringCollection.FindByPredicate(x => x.StartsWith("G"))}");



            var softwareCollection = new SoftwareCollection<Software>();
            softwareCollection.Add(new Word());
            softwareCollection.Add(new Minesweeper());
            softwareCollection.Add(new CConficker());

            Console.WriteLine("\nПолучение элемента по индексу:");
            var software = softwareCollection.Get(1);
            Console.WriteLine($"Получен элемент: {software?.Name}");

            Console.WriteLine("\nПоиск элемента по предикату:");
            var foundSoftware = softwareCollection.FindByPredicate(s => s.Name.Contains("Virus"));
            Console.WriteLine($"Найден элемент: {foundSoftware?.Name}");

            Console.WriteLine("\nУдаление элемента из коллекции:");
            softwareCollection.Remove(new Word());

            Console.WriteLine("\nПечать всех элементов:");
            foreach (var item in new Software[] { new Word(), new Minesweeper(), new CConficker() })
            {
                item.Run();
            }
        }
    }
}


