using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

public class Car
{
    public string VIN {  get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; } 
    public decimal Price { get; set; }

    public Car(string vin, string brand, string model, int year, decimal price)
    {
        VIN = vin;
        Brand = brand;
        Model = model;
        Year = year;
        Price = price;
    }


    public override string ToString()
    {
        return $"{Brand} {Model} ({Year}) - ${Price}";
    }
}

public class CarCollection : IList<Car>
{
    private Dictionary<string, Car> carDictionary;
    public CarCollection()
    {
        carDictionary = new Dictionary<string, Car>();
    }

    public int Count => carDictionary.Count;

    public bool IsReadOnly => false;

    public Car this[int index]
    {
        get => carDictionary.Values.ElementAt(index);
        set
        {
            if (value == null)
            {
                Console.WriteLine("Value can not be null");
            }

            var keyAtIndex = carDictionary.Keys.ElementAt(index);
            carDictionary.Remove(keyAtIndex);
            carDictionary[value.Model] = value;
        }
    }

    public void Add(Car car)
    {
        if (car == null)
        {
            Console.WriteLine("Value can not be null");
        }

        if (carDictionary.ContainsKey(car.VIN))
        {
            Console.WriteLine($"Car with model '{car.Model}' already exists.");
        }

        carDictionary.Add(car.VIN, car);
    }

    public void Clear()
    {
        carDictionary.Clear();
    }

    public bool Contains(Car car)
    {
        return car != null && carDictionary.ContainsKey(car.VIN);
    }

    public void CopyTo(Car[] array, int arrayIndex)
    {
        carDictionary.Values.CopyTo(array, arrayIndex);
    }


    public int IndexOf(Car car)
    {
        if (car == null) return -1;

        return carDictionary.Values.ToList().IndexOf(car);
    }

    public void Insert(int index, Car car)
    {
        if (car == null)
        {
            Console.WriteLine("Value can not be null");
        }
        if (carDictionary.ContainsKey(car.VIN))
        {
            Console.WriteLine($"Car with VIN '{car.VIN}' already exists.");
        }

        var tempList = carDictionary.Values.ToList();
        tempList.Insert(index, car);
        RebuildDictionary(tempList);
    }

    public bool Remove(Car car)
    {
        return car != null && carDictionary.Remove(car.VIN);
    }

    public void RemoveAt(int index)
    {
        var keyAtIndex = carDictionary.Keys.ElementAt(index);
        carDictionary.Remove(keyAtIndex);
    }

    public void DisplayCars()
    {
        foreach (var item in carDictionary)
        {
            Console.WriteLine(item.ToString());
        }
    }

    public Car FindCarByVIN(string vin)
    {
        foreach (var item in carDictionary)
        {
            if (item.Key == vin)
            {
                return item.Value;
            }
        }
            return null;
    }
    public IEnumerator<Car> GetEnumerator()
    {
        return carDictionary.Values.GetEnumerator();
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void RebuildDictionary(IEnumerable<Car> cars)
    {
        carDictionary.Clear();
        foreach (var car in cars)
        {
            carDictionary.Add(car.VIN, car);
        }
    }
}


class OOP9
{
     static void CollectionChange(object sender, NotifyCollectionChangedEventArgs e)
    {
         switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                Console.WriteLine("Item(s) added:");
                foreach (var newItem in e.NewItems)
                {
                    Console.WriteLine($"  {newItem}");
                }
                break;

            case NotifyCollectionChangedAction.Remove:
                Console.WriteLine("Item(s) removed:");
                foreach (var oldItem in e.OldItems)
                {
                    Console.WriteLine($"  {oldItem}");
                }
                break;

            case NotifyCollectionChangedAction.Replace:
                Console.WriteLine("Item(s) replaced:");
                Console.WriteLine("  Old items:");
                foreach (var oldItem in e.OldItems)
                {
                    Console.WriteLine($"    {oldItem}");
                }
                Console.WriteLine("  New items:");
                foreach (var newItem in e.NewItems)
                {
                    Console.WriteLine($"    {newItem}");
                }
                break;

            case NotifyCollectionChangedAction.Reset:
                Console.WriteLine("Collection reset.");
                break;
        }
    }
    static void Main(string[] args)
    {
        CarCollection carCollection = new CarCollection();

        carCollection.Add(new Car("VIN1","Toyota", "Avensis", 1996, 5000));
        carCollection.Add(new Car("VIN2","Honda", "Accord", 1996, 3500));
        carCollection.Add(new Car("VIN3","Renault", "Scenic", 2002, 4500));

        Console.WriteLine("Список машин:");
        carCollection.DisplayCars();

        Console.WriteLine("\nМашина с VIN: VIN1");
        var CarFoundedByVin = carCollection.FindCarByVIN("VIN1");        
        Console.WriteLine(CarFoundedByVin.ToString());
        

        Console.WriteLine("\nУдаление Honda Accord...");
        var carToRemove = new Car("VIN2", "Honda", "Accord", 1996, 3500);
        carCollection.Remove(carToRemove);

        Console.WriteLine("\nОбновленный список машин:");
        carCollection.DisplayCars();


        Dictionary<int, char> firstCollection = new Dictionary<int, char>
        {
            { 1, 'A' },
            { 2, 'B' },
            { 3, 'C' },
            { 4, 'D' },
            { 5, 'E' }
        };

        Console.WriteLine("Коллекция Dictionary:");
        foreach (var p in firstCollection)
        {
            Console.WriteLine($"Ключ: {p.Key}, Значение: {p.Value}");
        }

         int n = 2;
        Console.WriteLine($"\nУдаление {n} последовательных элементов...");
        var keysToRemove = firstCollection.Keys.Take(n).ToList();
        foreach (var key in keysToRemove)
        {
            firstCollection.Remove(key);
        }

         Console.WriteLine("Dictionary после удаления:");
        foreach (var p in firstCollection)
        {
            Console.WriteLine($"Ключ: {p.Key}, Значение: {p.Value}");
        }

         Console.WriteLine("\nДобавление новых элементов...");
        firstCollection.Add(6, 'F');
        firstCollection[7] = 'G'; 

         Console.WriteLine("Dictionary после добавлений:");
        foreach (var p in firstCollection)
        {
            Console.WriteLine($"Ключ: {p.Key}, Значение: {p.Value}");
        }

         Console.WriteLine("\nСоздание второй коллекции...");
        List<char> secondCollection = firstCollection.Values.ToList();

         Console.WriteLine("Вторая коллекция:");
        foreach (var v in secondCollection)
        {
            Console.WriteLine(v);
        }

         char valueToFind = 'F';
        Console.WriteLine($"\nПоиск значения '{valueToFind}' во второй коллекции...");
        if (secondCollection.Contains(valueToFind))
        {
            Console.WriteLine($"Значение '{valueToFind}' найдено.");
        }
        else
        {
            Console.WriteLine($"Значение '{valueToFind}' не найдено.");
        }
        ObservableCollection<Car> carCollection2 = new ObservableCollection<Car>();

        carCollection2.CollectionChanged += CollectionChange;

        Console.WriteLine("Добавление машин...");
        carCollection2.Add(new Car("VIN4", "Audi", "A6", 1996, 4700));
        carCollection2.Add(new Car("VIN5", "Mesedes-Benz", "D-Class", 1996, 900));
        carCollection2.Add(new Car("VIN6", "BMW", "5-Series", 1997, 4700));

        Console.WriteLine("\nУдаление машины...");
        carCollection2.RemoveAt(1); 

        Console.WriteLine("\nЗамена машины...");
        carCollection2[0] = new Car("VIN7", "Audi", "RS Q8", 2022, 120000);
        Console.WriteLine("\nМашины в наблюдемой коллекции:");
        foreach (var car in carCollection2)
        {
            Console.WriteLine(car.ToString());
        }

        Console.WriteLine("\nОчистка коллекции...");
        carCollection2.Clear();
    }
}


