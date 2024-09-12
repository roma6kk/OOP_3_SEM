using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //------------------------1--------------------------
            // Задание 1
            Console.WriteLine("Hellow, World!");
            sbyte a = 1;
            short b = 2;
            int c = 3;
            long d = 4;
            byte e = 5;
            ushort f = 6; 
            uint g = 7;
            ulong h = 8;
            char i = 'n';
            bool j = false;
            float k = 11.11f;
            double l = 12.12f;
            decimal m = 13;
            string n = "fourteen";
            object o = null;
            Console.WriteLine($"sbyte = {a}, short = {b}, int = {c}, long = {d}, byte = {e}, ushort = {f}," +
                $" uint = {g}, ulong = {h}, char = {i}, bool = {j}, float = {k}, double = {l}, decimal = {m}," +
                $" string = {n}, object = {o}" );
            // Задание 2
            // Неявное преобразование
            short aC = a;// Преобразование sbyte в short
            int aCC = aC;// Преобразование short в int
            long aCCC = aCC;// Преобразование int в long
            float gC = g;// Преобразование uint в float
            double gCC = gC;// Преобразование float в double
            // Явное преобразование
            float lC = (float)c;// Приведение double к float
            int lCC = (int)lC;// Приведение float к int
            short lCCC = (short)lCC;// Приведение int к short
            sbyte lCCCC = (sbyte)lCCC;// Приведение short к sbyte
            int mC = (int)m;// Приведение decimal к int
            // Задание 3
            object packing = c;// Упаковка переменной int
            int unpacking = (int)packing;// Распаковка переменной int
            // Задание 4
            var sum = a + 10;// Неявно типизированная переменная sum
            // Задание 5
            int? nullAbleInt = null;
            Console.WriteLine($"nullAbleInt: {nullAbleInt}");
            nullAbleInt = 10;
            Console.WriteLine($"nullAbleInt: {nullAbleInt}");
            // Задание 6
            var diffTypies = 10;
            // diffTypies = "Var";// Ошибка происходит из-за того, что тип переменной var определяется на этапе компиляции, и не может быть переопределен
            //------------------------2--------------------------
            // Задание 1
            string literal_1 = "literal_1";
            string literal_2 = "literal_2";
            bool areEqual = literal_1 == literal_2;// Сравнение строк
            Console.WriteLine($"literal_1 и literal_2 равны: {areEqual}");
            // Задание 2
            string String_1 = "String_1";
            string String_2 = "String_2";
            string String_3 = "String_3";
            string String_1_2_3 = string.Concat(String_1, " ", String_2, " ", String_3);// Сцепление (конкатенация) строк
            string copiedString = string.Copy(String_1);// Копирование строки String_1 в copied String
            Console.WriteLine($"copiedString: {copiedString}");
            string substring = String_3.Substring(0, 6);// Копируем часть строки начиная с позиции 0 длиной в 6 символов
            Console.WriteLine($"substring: {substring}");
            string[] stringNum = String_3.Split('_');// Разделяем String_3 по символу '_', слова помещаются в массив stringNum
            Console.Write("String_3 splitted:");
            foreach(string num in stringNum )
            {
                Console.Write(num + ' ');
            }
            Console.WriteLine();
            string insertedString = String_2.Insert(6, "__");// Вставляем в String_2 на 6 позицию значение "__"
            Console.WriteLine($"insertedString: {insertedString}");
            string removedString = String_3.Remove(6, 2);// Удаляем подстроку начиная с позиции 6 длиной 2 символа
            Console.WriteLine($"removedString: {removedString}");// В данной строке также продемонтсрировано интерполирование
            //------------------------3--------------------------
            // Задание 1
            int[,] matrix = new int[4,5];// Объявление матрицы 4x5 (RxC)
            Random ran = new Random();
            for(int counter = 0; counter < 4; counter++)// Инициализация матрицы
            {
                for(int counter2 = 0;counter2 < 4; counter2++)
                {
                    matrix[counter,counter2] = ran.Next(1,15);
                    Console.Write("{0}\t", matrix[counter, counter2]);// Вывод элемента матрицы
                }
                Console.WriteLine();
            }
            // Задание 2
            string[] stringArr = { "el1", "el2", "el3", "el4" };
            foreach(string str in stringArr )
            {
                Console.Write(str + ", ");
            }
            Console.Write($"Array length: {stringArr.Length}");
            Console.WriteLine();
            Console.Write("Введите индекс элемента, который хотите изменить (0 - 3): ");
            int position = int.Parse(Console.ReadLine());
            if (position < 0 || position >= stringArr.Length)// Проверяем, что индекс находится в пределах массива
            {
                Console.WriteLine("Ошибка: введён недопустимый индекс.");
                return;
            }
            Console.Write("Введите новое значение для элемента: ");
            string newValue = Console.ReadLine();
            stringArr[position] = newValue;// Меняем значение элемента на указанной позиции
            Console.WriteLine("Обновлённый массив:");// Выводим обновлённые элементы массива
            foreach (string str in stringArr)
            {
                Console.Write(str + ", ");
            }
            Console.WriteLine();
            // Задание 3 
            int[][] jaggedArr = new int[3][];
            jaggedArr[0] = new int[2];
            jaggedArr[1] = new int[3];
            jaggedArr[2] = new int[4];
            for (int counter = 0; counter < jaggedArr.Length; counter++)
            {
                for (int counter_2 = 0; counter_2 < jaggedArr[counter].Length; counter_2++)
                {
                    Console.Write($"Введите значение для jaggedArray[{counter}][{counter_2}]: ");
                    jaggedArr[counter][counter_2] = int.Parse(Console.ReadLine());  // Чтение значений с консоли
                }
            }
            Console.WriteLine("Значения зубчатого массива:");
            for (int counter = 0; counter < jaggedArr.Length; counter++)
            {
                for (int counter_2 = 0; counter_2 < jaggedArr[counter].Length; counter_2++)
                {
                    Console.Write($"{jaggedArr[counter][counter_2]} ");
                }
                Console.WriteLine();
            }
            // Задание 4
            var words = new[] { "word1", "word2", "word3" };
            Console.Write("Word array: ");
            foreach (var word in words)
            {
                Console.Write(word + ' ');
            }
            Console.WriteLine() ;
            //------------------------4--------------------------
            // Задание 1
            ValueTuple <int,string,char,string,ulong> tuple = (1, "two", '3', "four", 1);// Использовали расширение ValueTuple из NuGet
            // Задание 2
            Console.Write("Кортеж: ");
            Console.WriteLine(tuple);
            Console.Write("Элементы 1,3,4 кортежа: ");
            Console.WriteLine(tuple.Item1 + ", " + tuple.Item3 + ", " + tuple.Item4);
            // Задание 3
            (int tuple_1, string tuple_2, char tuple_3, string tuple_4, ulong tuple_5) = tuple;// Полная распаковка кортежа в переменные
            Console.WriteLine("Полная распаковка:");
            Console.WriteLine($"tuple_1: {tuple_1}, tuple_2: {tuple_2}, tuple_3: {tuple_3}, tuple_4: {tuple_4}, tuple_5: {tuple_5}");
            (int tuple_1_2, _, char tuple_3_2, string tuple_4_2, _) = tuple;// Распаковка с игнорированием некоторых элементов с помощью "_"
            Console.WriteLine("Частичная распаковка с игнорированием:");
            Console.WriteLine($"tuple_1_2: {tuple_1_2}, tuple_3_2: {tuple_3_2}, tuple_4_2: {tuple_4_2}");
            var (tuple_1_3, tuple_2_3, tuple_3_3, tuple_4_3, tuple_5_3) = tuple;// Распаковка в неявно типизированные переменные
            Console.WriteLine("Распаковка в неявно типизированные переменные (var):");
            Console.WriteLine($"tuple_1_3: {tuple_1_3}, tuple_2_3: {tuple_2_3}, tuple_3_3: {tuple_3_3}, tuple_4_3: {tuple_4_3}, tuple_5_3: {tuple_5_3}");
            // Задание 4
            ValueTuple<int, string, char, string, ulong> tupleForCompare = (1, "two", '3', "four", 1);// Использовали расширение ValueTuple из NuGet
            bool isEqual = tupleForCompare.Equals(tuple);
            Console.WriteLine($"tuple и tupleForCompare одинаковы? {isEqual}");
            //------------------------5--------------------------
            int[] numbers = { 5, 3, 8, 1, 4 };
            string text = "Пример";
            (int max, int min, int sum, char firstLetter) ProcessArrayAndString(int[] arr, string str)// Локальная функция для обработки массива и строки
            {
                int max = int.MinValue;
                int min = int.MaxValue;
                int valueSum = 0;
                foreach (int num in arr)// Поиск максимального, минимального и суммы элементов массива
                {
                    if (num > max) max = num;
                    if (num < min) min = num;
                    valueSum += num;
                }
                char firstLetter = str.Length > 0 ? str[0] : '\0'; // Если строка не пустая, берём первую букву
                                                                   // Возвращаем кортеж с результатами
                return (max, min, valueSum, firstLetter);
            }
            var result = ProcessArrayAndString(numbers, text);// Вызов локальной функции
            Console.WriteLine($"Максимальный элемент: {result.max}");
            Console.WriteLine($"Минимальный элемент: {result.min}");
            Console.WriteLine($"Сумма элементов: {result.sum}");
            Console.WriteLine($"Первая буква строки: {result.firstLetter}");
            //------------------------5--------------------------
            void CheckedFunction()
            {
                try
                {
                    checked
                    {
                        int maxIntValue = int.MaxValue;
                        Console.WriteLine("Значение перед переполнением (checked): " + maxIntValue);
                        int overflow = maxIntValue + 1;// Переполнение
                        Console.WriteLine("Значение после переполнения (checked): " + overflow);
                    }
                }
                catch (OverflowException)
                {
                    Console.WriteLine("Переполнение произошло в блоке checked!");
                }
            }
            void UncheckedFunction()
            {
                unchecked
                {
                    int maxIntValue = int.MaxValue;
                    Console.WriteLine("Значение перед переполнением (unchecked): " + maxIntValue);
                    int overflow = maxIntValue + 1;// Переполнение с исключением
                    Console.WriteLine("Значение после переполнения (unchecked): " + overflow);
                }
            }
            Console.WriteLine("СheckedFunction:");
            CheckedFunction();
            Console.WriteLine("UncheckedFunction:");
            UncheckedFunction();
        }
    }
    }

