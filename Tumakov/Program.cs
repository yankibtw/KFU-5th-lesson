using System;
using System.IO;
using System.Linq;

namespace Tumakov
{
    class MyMethods
    {
        private Random random = new Random();
        public void SearchForLettersType(char[] chars)
        {
            string vowelsOfTheLetter = "AEIOUYАУОЫИЭЯЮЕЁ";
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
        public void PrintMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + "   ");
                }
                Console.WriteLine();
            }
        }
        public void PrintArray(double[] array)
        {
            foreach (double el in array)
            {
                Console.Write($"{el} ");
            }
        }
        public int[,] CreateMatrix(int rows, int cols)
        {
            int[,] matrix = new int[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = random.Next(-20, 40);
                }
            }
            return matrix;
        }
        public int[,] MultiplyMatrix(int[,] firstMatrix, int[,] secondMatrix)
        {
            int[,] resultMatrix = new int[firstMatrix.GetLength(0), secondMatrix.GetLength(1)];
            if (firstMatrix.GetLength(1) != secondMatrix.GetLength(0))
            {
                Console.WriteLine("Невозможно умножить эти матрицы. Количество столбцов первой матрицы должно быть равно количеству строк второй матрицы.");
                return null;
            }
            for (int i = 0; i < firstMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < secondMatrix.GetLength(1); j++)
                {
                    for (int k = 0; k < firstMatrix.GetLength(1); k++)
                    {
                        resultMatrix[i, j] += firstMatrix[i, k] * secondMatrix[k, j];
                    }
                }
            }
            return resultMatrix;
        }
        public double[] SearchAverageTemperatureOfYear(int[,] matrix)
        {
            double[] averageTemperatureOfYear = new double[matrix.GetLength(0)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                double avgSum = 0;
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    avgSum += matrix[i, j];
                }
                averageTemperatureOfYear[i] = Math.Round(avgSum / matrix.GetLength(1), 2);
            }
            return SortAverageTemperatures(averageTemperatureOfYear);
        }
        public double[] SortAverageTemperatures(double[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        double temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
            return array;
        }
    }
    internal class Program
    {
        static void Lesson2()
        {
            Console.WriteLine("\nЗадание 2:");
            MyMethods method = new MyMethods();
            int[,] firstMatrix = method.CreateMatrix(2, 2);
            int[,] secondMatrix = method.CreateMatrix(2, 2);

            Console.WriteLine("Первая матрица: ");
            method.PrintMatrix(firstMatrix);
            Console.WriteLine("Вторая матрица: ");
            method.PrintMatrix(secondMatrix);

            try
            {
                int[,] resultMatrix = method.MultiplyMatrix(firstMatrix, secondMatrix);
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
            int[,] temperature = method.CreateMatrix(12, 30);
            method.PrintMatrix(temperature);
            double[] result = method.SearchAverageTemperatureOfYear(temperature);
            Console.WriteLine("\nПолученные средние значения температуры отсортированные по возрастанию: ");
            method.PrintArray(result);
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
