using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace TumakovHomeTask
{
    class MyMethods
    {
        private Random random = new Random();
        public void SearchForLettersType(char[] chars)
        {
            List<char> vowelsOfTheLetter = new List<char> { 'A', 'E', 'I', 'O', 'Y', 'А', 'У', 'О', 'Ы', 'И', 'Э', 'Я', 'Ю', 'Е', 'Ё' };
            int qtyVowels = 0, qtyConsonant = 0;

            foreach (char el in chars)
            {
                if (char.IsLetter(el))
                {
                    _ = vowelsOfTheLetter.Contains(char.ToUpper(el)) ? qtyVowels++ : qtyConsonant++;
                }
            }
            Console.WriteLine($"Количество гласных букв: {qtyVowels}, количество согласных букв: {qtyConsonant}");
        }
        public void PrintMatrix(LinkedList<LinkedList<int>> matrix)
        {
            foreach (var row in matrix)
            {
                foreach (var col in row)
                {
                    Console.Write(col + "   ");
                }
                Console.WriteLine();
            }
        }
        public void PrintMatrix(Dictionary<string, int[]> matrix)
        {
            foreach (var element in matrix)
            {
                Console.Write($"{element.Key}: ");
                foreach (var row in element.Value)
                {
                    Console.Write($"{row}  ");
                }
                Console.WriteLine();
            }
        }
        public void PrintArray(List<KeyValuePair<string, double>> array)
        {
            foreach (var element in array)
            {
                Console.Write($"{element.Key}: {element.Value:F2}");
                Console.WriteLine();
            }
        }
        public LinkedList<LinkedList<int>> CreateMatrix(int rows, int cols)
        {
            LinkedList<LinkedList<int>> matrix = new LinkedList<LinkedList<int>>();

            for (int i = 0; i < rows; i++)
            {
                LinkedList<int> row = new LinkedList<int>();
                for (int j = 0; j < cols; j++)
                {
                    row.AddLast(random.Next(-20, 40));
                }
                matrix.AddLast(row);
            }

            return matrix;
        }
        public LinkedList<LinkedList<int>> MultiplyMatrix(LinkedList<LinkedList<int>> firstMatrix, LinkedList<LinkedList<int>> secondMatrix)
        {
            LinkedList<LinkedList<int>> resultMatrix = new LinkedList<LinkedList<int>>();
            if (firstMatrix.First.Value.Count != secondMatrix.Count)
            {
                Console.WriteLine("Невозможно умножить эти матрицы. Количество столбцов первой матрицы должно быть равно количеству строк второй матрицы.");
                return null;
            }
            for (int i = 0; i < firstMatrix.Count; i++)
            {
                LinkedList<int> row = new LinkedList<int>();
                for (int j = 0; j < secondMatrix.Count; j++)
                {
                    int sum = 0;
                    for (int k = 0; k < firstMatrix.First.Value.Count; k++)
                    {
                        sum += firstMatrix.ElementAt(i).ElementAt(k) * secondMatrix.ElementAt(k).ElementAt(j);
                    }
                    row.AddLast(sum);
                }
                resultMatrix.AddLast(row);
            }
            return resultMatrix;
        }
        public double CalculateAverage(int[] numbers)
        {
            double sum = 0;
            foreach (int number in numbers)
            {
                sum += number;
            }
            return sum / numbers.Length;
        }
        public Dictionary<string, int[]> GenerateTemperatureData()
        {
            Dictionary<string, int[]> temperatureData = new Dictionary<string, int[]>();

            for (int month = 1; month <= 12; month++)
            {
                int[] monthlyTemperatures = new int[30];
                for (int day = 0; day < 30; day++)
                {
                    monthlyTemperatures[day] = random.Next(-20, 40);
                }
                string monthName = GetMonthName(month);
                temperatureData.Add(monthName, monthlyTemperatures);
            }
            return temperatureData;
        }
        public Dictionary<string, double> SearchAverageTemperatureOfYear(Dictionary<string, int[]> temperaturesDate)
        {
            Dictionary<string, double> averageTemperatureOfYear = new Dictionary<string, double>();
            foreach (var el in temperaturesDate)
            {
                int[] temperatures = el.Value;
                double averageTemperature = CalculateAverage(temperatures);
                averageTemperatureOfYear.Add(el.Key, averageTemperature);
            }

            return averageTemperatureOfYear;
        }
        private string GetMonthName(int month)
        {
            DateTimeFormatInfo date = new DateTimeFormatInfo();
            return date.GetMonthName(month);
        }
    }
    internal class Program
    {
        static void Lesson2()
        {
            Console.WriteLine("\nЗадание 2:");
            MyMethods method = new MyMethods();
            LinkedList<LinkedList<int>> firstMatrix = method.CreateMatrix(2, 2);
            LinkedList<LinkedList<int>> secondMatrix = method.CreateMatrix(2, 2);

            Console.WriteLine("Первая матрица: ");
            method.PrintMatrix(firstMatrix);
            Console.WriteLine("Вторая матрица: ");
            method.PrintMatrix(secondMatrix);

            try
            {
                LinkedList<LinkedList<int>> resultMatrix = method.MultiplyMatrix(firstMatrix, secondMatrix);
                Console.WriteLine("Результат перемножения матриц: ");
                method.PrintMatrix(resultMatrix);
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        static void Lesson3()
        {
            Console.WriteLine("\nЗадание 3:");
            MyMethods method = new MyMethods();

            Dictionary<string, int[]> temperature = method.GenerateTemperatureData();
            method.PrintMatrix(temperature);

            Console.WriteLine("\nПолученные средние значения температуры: ");
            Dictionary<string, double> result = method.SearchAverageTemperatureOfYear(temperature);

            List<KeyValuePair<string, double>> sortedTemperatures = new List<KeyValuePair<string, double>>(result);
            sortedTemperatures.Sort((x, y) => x.Value.CompareTo(y.Value));
            method.PrintArray(sortedTemperatures);
        }
        static void Main(string[] args)
        {
            MyMethods method = new MyMethods();
            Console.WriteLine("Задание 1: ");
            if (args.Length != 1)
            {
                Console.WriteLine("Пожалуйста, укажите имя файла в качестве аргумента.");
                return;
            }
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, args[0]);
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Указанный файл не существует.");
                return;
            }
            try
            {
                string content = File.ReadAllText(filePath);
                char[] charArray = content.ToCharArray();
                method.SearchForLettersType(charArray);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл не найден.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Произошла ошибка: " + e.Message);
            }

            Lesson2();
            Lesson3();
        }
    }
}
