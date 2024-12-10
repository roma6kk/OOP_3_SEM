using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Vector { }

namespace OOP10
{
    internal class OOP10
    {
        static void Main(string[] args)
        {
            string[] months =
        {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        };

            int n = 4;
            var monthsWithLengthN = months.Where(month => month.Length == n);
            Console.WriteLine($"Месяцы состоящие из {n} смиволов:");
            Console.WriteLine(string.Join(" ", monthsWithLengthN));


            string[] summerMonths = { "June", "July", "August" };
            string[] winterMonths = { "December", "January", "February" };

            var summerAndWinterMonths = months.Where(month => summerMonths.Contains(month) || winterMonths.Contains(month));
            Console.WriteLine("\nЛетние и зимние месяца:");
            Console.WriteLine(string.Join(" ", summerAndWinterMonths));

            var sortedMonths = months.OrderBy(month => month);
            Console.WriteLine("\nМесяцы отсортированные в алфавитном порядке:");
            Console.WriteLine(string.Join(" ", sortedMonths));

            var monthsWithUAndLength = months.Where(month => month.Contains('u') && month.Length >= 4);
            Console.WriteLine("\nМесяцы содержащие 'u' и длиной >= 4:");
            Console.WriteLine(string.Join(" ", monthsWithUAndLength));

            var joinLINQ = summerAndWinterMonths.Join(monthsWithUAndLength,monthsSAW => monthsSAW,uMonth => uMonth,(monthsSAW, uMonth) => monthsSAW);
            Console.WriteLine("\njoinLINQ:");
            Console.WriteLine(string.Join(" ", joinLINQ));

            List<Vector> vectors = new List<Vector>();
            vectors.Add(new Vector(new int[] { 1, 1 }));
            vectors.Add(new Vector(new int[] { -1, -2, 3 }));
            vectors.Add(new Vector(new int[] { 1, -2, 5, 5, 5 }));
            vectors.Add(new Vector(new int[] { 99, 101, 7, 7, 7, 7, 7 }));
            vectors.Add(new Vector(new int[] { 10, 0 }));
            vectors.Add(new Vector(new int[] { 20, 0 }));
            vectors.Add(new Vector(new int[] { 1, 1 }));
            vectors.Add(new Vector(new int[] { 5, 5 }));
            vectors.Add(new Vector(new int[] { 6, 6 }));
            vectors.Add(new Vector(new int[] { 7, 7 }));
            vectors.Add(new Vector(new int[] { 7, 8}));

            int VectorsContainsZeroCounter = vectors.Count(v => v.Elements.Contains(0));
            Console.WriteLine($"\nКоличество векторов, содержащих 0: {VectorsContainsZeroCounter}\n");
            
            double MinMagnitude = vectors.Min(v => v.GetMagnitude());
            List<Vector> MinMagnitudeVectors = new List<Vector>();
            MinMagnitudeVectors = vectors.Where(v => v.GetMagnitude() == MinMagnitude).ToList();
            Console.Write("\nСписок векторов с наименьшим модулем: ");
            Console.WriteLine(string.Join(" ", MinMagnitudeVectors) + '\n');

            Vector[] VectorArray = vectors.Where(v => v.Length == 3 || v.Length == 5 || v.Length == 7).ToArray();
            Console.Write("\nМассив векторов длины 3, 5, 7: ");
            Console.WriteLine(string.Join(" ", VectorArray.ToList()));

            double MaxMagnitude = vectors.Max(v => v.GetMagnitude());
            Vector MaxMagnitudeVector = vectors.First(v => v.GetMagnitude() == MaxMagnitude);
            Console.WriteLine($"\n\nМаксимальный вектор: {MaxMagnitudeVector}");

            Vector FirstNegativeVector = vectors.First(v => v.Elements.Any(e => e < 0));
            Console.WriteLine($"\nПервый вектор с отрицательным значением: {FirstNegativeVector}");

            List<Vector> OrderedByLengthVectors = vectors.OrderBy(v => v.Length).ToList();
            Console.Write("\nУпорядоченный список векторов по размеру: ");
            Console.WriteLine(string.Join(" ", OrderedByLengthVectors));

            var result = vectors
           .Where(v => v.Elements.All(e => e >= 0)) 
           .Select(v => new { Vector = v, Magnitude = v.GetMagnitude() }) 
           .OrderByDescending(v => v.Magnitude) 
           .GroupBy(v => v.Vector.Length) 
           .Select(group => new 
           {
               Length = group.Key,
               Count = group.Count(),
               MaxMagnitude = group.Max(v => v.Magnitude),
               Vectors = group.Select(g => g.Vector) 
           });
            foreach (var group in result)
            {
                Console.WriteLine("\nВекторы:");
                foreach (var vector in group.Vectors)
                {
                    Console.WriteLine(vector);
                }
                Console.WriteLine($"Длина: {group.Length}, Количество: {group.Count}, Максимальный модуль: {group.MaxMagnitude}");
                Console.WriteLine();
            }
        }

    }
}
