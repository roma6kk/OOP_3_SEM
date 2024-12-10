using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Runtime.InteropServices;

namespace OOP11
{

    public class TypeInfo
    {
        public string ClassName { get; set; }
        public string AssemblyName { get; set; }
        public bool HasPublicConstructors { get; set; }
        public IEnumerable<string> PublicMethods { get; set; }
        public IEnumerable<string> FieldsAndProperties { get; set; }
        public IEnumerable<string> Interfaces { get; set; }
        public IEnumerable<string> MethodsWithParameterType { get; set; }
        public IEnumerable<object> MethodWithParameterValue { get; set; }



    }


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

        public void someMethod(string value, int a = 21)
        {
            Console.WriteLine($"\nGeneratedValue: {value}");
            return;
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

    public static class Reflector
    {

        public static string GetAssemblyName(object obj)
        {
            Type type = obj.GetType();
            return type.Assembly.FullName;
        }

        public static bool HasPublicConstructors(object obj)
        {
            Type type = obj.GetType();
            return type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Any();
        }

        public static IEnumerable<string> GetPublicMethods(object obj)
        {
            Type type = obj.GetType();

            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Select(method => method.Name);
        }

        public static IEnumerable<string> GetFieldsAndProperties(object obj)
        {
            Type type = obj.GetType();

            IEnumerable<string> fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Select(field => $"Field: {field.Name}");

            IEnumerable<string> properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Select(property => $"Property: {property.Name}");

            return fields.Concat(properties);
        }

        public static IEnumerable<string> GetInterfaces(object obj)
        {
            Type type = obj.GetType();
            return type.GetInterfaces().Select(i => i.FullName);
        }

        public static IEnumerable<string> GetMethodsWithParameterType(string className, Type parameterType)
        {
            Type type = Type.GetType(className);

            if (type == null)
            {
                throw new ArgumentException($"Класс с именем {className} не найден.");
            }

            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Where(method => method.GetParameters().Any(parameter => parameter.ParameterType == parameterType)).Select(method => method.Name);
        }


        public static IEnumerable<object> GetMethodsWithParameterValue(object obj)
        {
            Type type = obj.GetType();

            return type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Where(method => method.GetParameters()

            .Any(parameter => parameter.HasDefaultValue)).Select(method =>
            {
                var parametersWithDefaults = method.GetParameters().Where(parameter => parameter.HasDefaultValue)
                .ToDictionary(
                    parameter => parameter.Name,
                    parameter => parameter.DefaultValue
                 );

                return new
                {
                    Method = method.Name,
                    Parameters = parametersWithDefaults

                };


            });
        }

        //----------------------------------g)-----------------------------

        public static object GenerateValue(Type type)
        {
            object result = null;
            Random randomGenerator = new Random();

            if (type == typeof(int))
            {
                result = randomGenerator.Next(1, 100);
            }
            else if (type == typeof(string))
            {
                result = "Generated string";
            }
            else if (type == typeof(bool))
            {
                result = true;
            }

            return result;
        }



        public static void Invoked(object obj, string methodName, string jsonFilePath)
        {

            string jsonContent = File.ReadAllText(jsonFilePath);

            var typeInfo = JsonSerializer.Deserialize<TypeInfo>(jsonContent);

            if (typeInfo == null)
            {
                Console.WriteLine("Ошибка чтения JSON-файла.");
                return;
            }

            if (typeInfo.ClassName != obj.GetType().FullName)
            {
                Console.WriteLine($"Класс объекта {obj.GetType().FullName} не совпадает с данными в JSON: {typeInfo.ClassName}");
            }

            Type type = obj.GetType();

            MethodInfo method = type.GetMethod(methodName);

            if (method == null)
            {
                Console.WriteLine($"Метод {methodName} не найден в классе {type.Name}.");
                return;
            }

            ParameterInfo[] parameters = method.GetParameters();

            object[] methodParameters = new object[parameters.Length];

            var methodInfoFromJson = typeInfo.MethodWithParameterValue
                .OfType<JsonElement>()
                .Select(e => JsonSerializer.Deserialize<Dictionary<string, object>>(e.GetRawText()))
                .FirstOrDefault(m => m != null && m.ContainsKey("Method") && m["Method"].ToString() == methodName);

            if (methodInfoFromJson != null && methodInfoFromJson.ContainsKey("Parameters"))
            {
                var parameterValues = JsonSerializer.Deserialize<Dictionary<string, object>>(methodInfoFromJson["Parameters"].ToString());
                Console.WriteLine($"Параметры из JSON для метода {methodName}:");

                foreach (var param in parameterValues)
                {
                    Console.WriteLine($"Имя: {param.Key};\nЗначение: {param.Value}");
                }

                for (int i = 0; i < parameters.Length; i++)
                {

                    if (parameterValues != null && parameterValues.TryGetValue(parameters[i].Name, out var value))
                    {

                        try
                        {
                            if (value is JsonElement jsonElement)
                            {
                                value = jsonElement.Deserialize(parameters[i].ParameterType);
                            }
                            methodParameters[i] = Convert.ChangeType(value, parameters[i].ParameterType);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"\nОшибка преобразования параметра {parameters[i].Name}: {ex.Message}");
                            methodParameters[i] = GenerateValue(parameters[i].ParameterType);
                        }


                    }
                    else
                    {
                        methodParameters[i] = GenerateValue(parameters[i].ParameterType);
                    }
                }


            }
            else
            {
                Console.WriteLine($"\nДанные о параметрах для метода {methodName} отсутствуют в JSON.");

                for (int i = 0; i < parameters.Length; i++)
                {
                    methodParameters[i] = GenerateValue(parameters[i].ParameterType);
                }

            }



            object result = method.Invoke(obj, methodParameters);


            Console.WriteLine($"\nМетод {methodName} вызван успешно.");


        }


        public static void SaveToJson(object obj, string filePath, string className, Type parameterType)
        {
            TypeInfo result = new TypeInfo
            {
                ClassName = obj.GetType().FullName,
                AssemblyName = GetAssemblyName(obj),
                HasPublicConstructors = HasPublicConstructors(obj),
                PublicMethods = GetPublicMethods(obj),
                FieldsAndProperties = GetFieldsAndProperties(obj),
                Interfaces = GetInterfaces(obj),
                MethodsWithParameterType = GetMethodsWithParameterType(className, parameterType),
                MethodWithParameterValue = GetMethodsWithParameterValue(obj)
            };

            string json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
            Console.WriteLine("\n\n--------------------");
            Console.WriteLine($"Результаты сохранены в файл {filePath}");
            Console.WriteLine("--------------------\n\n");
        }



        public static T Create<T>(params object[] parameters) where T : class
        {
            Type type = typeof(T);

            ConstructorInfo constructor = type.GetConstructor(parameters.Select(p => p.GetType()).ToArray());

            return constructor == null ? throw new ArgumentException("Подходящий конструктор не найден.") : (T)constructor.Invoke(parameters);
        }



    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Vector vector = new Vector(new[] { 1, 2, 3, 4, 5 });

            //-------------------a)-------------------

            string getAssemblyName = Reflector.GetAssemblyName(vector);
            Console.WriteLine($"Полное имя сборки: {getAssemblyName}");

            //-------------------b)-------------------

            bool hasPublicConstructors = Reflector.HasPublicConstructors(vector);
            Console.WriteLine($"\nПроверка наличия публичных конструкторов у объекта: {hasPublicConstructors}");

            //-------------------c)-------------------

            IEnumerable<string> getPublicMethods = Reflector.GetPublicMethods(vector);

            Console.WriteLine("\nИзвлечение публичных методов класса:");
            foreach (var item in getPublicMethods)
            {
                Console.WriteLine(item);
            }

            //-------------------d)-------------------

            IEnumerable<string> getFieldsAndProperties = Reflector.GetFieldsAndProperties(vector);

            Console.WriteLine("\nИнформация о полях и свойствах класса:");
            foreach (var item in getFieldsAndProperties)
            {
                Console.WriteLine(item);
            }

            //-------------------e)-------------------

            IEnumerable<string> getInterfaces = Reflector.GetInterfaces(vector);

            Console.WriteLine("\nИзвлечение всех реализованных классом интерфейсов:");
            foreach (var item in getInterfaces)
            {
                Console.WriteLine(item);
            }

            //-------------------f)-------------------

            Console.WriteLine("\nВведите имя класса для поиска метода с определенным типом параметра (например, OOP11.Vector):");
            string className = Console.ReadLine();

            Console.WriteLine("\nВведите имя типа параметра (например, System.Object):");
            string parameterTypeName = Console.ReadLine();

            Type parameterType = Type.GetType(parameterTypeName);

            if (parameterType == null)
            {
                Console.WriteLine("Указанный тип параметра не найден.");
            }
            else
            {
                IEnumerable<string> getMethodsWithParameterType = Reflector.GetMethodsWithParameterType(className, parameterType);
                Console.WriteLine($"\nМетоды, содержащие параметр типа {parameterType}:");
                foreach (var item in getMethodsWithParameterType)
                {
                    Console.WriteLine(item);
                }
            }

            string filePath = "vectorJson.json";
            Reflector.SaveToJson(vector, filePath, className, parameterType);

            //-------------------g)-------------------

            Reflector.Invoked(vector, "someMethod", filePath);

            //---------------------Задание 2-------------------

            Console.WriteLine("\n\n---------------------Задание 2-------------------");

            Console.WriteLine("\nСоздание объекта Vector с параметрами через метод Reflector.Create:");
            var createdObject = Reflector.Create<Vector>(new int[] { 10, 20, 30 });
            createdObject.Print();

            Console.WriteLine("\nСоздание объекта Vector с использованием конструктора без параметров:");
            var defaultObject = Reflector.Create<Vector>();
            defaultObject.Print();

            Console.ReadKey();
        }
    }
}