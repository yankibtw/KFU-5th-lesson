using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KFU_5th_lesson
{
    struct Disease
    {
        public string name { get; set; }
    }
    struct Student
    {
        public string surname;
        public string name;
        public DateTime birth;
        public string exam;
        public uint mark;

        public Student(string surname, string name, DateTime birth, string exam, uint mark)
        {
            this.surname = surname;
            this.name = name;   
            this.birth = birth;
            this.exam = exam;
            this.mark = mark;
        }
    }
    struct GrandMa
    {
        public string name { get; set; }
        public int age { get; set; }
        public List<Disease> diseases { get; set; }
    }
    struct Clinic
    {
        public string name { get; set; }
        public List<Disease> diseases { get; set; }
        public int capacity { get; set; }
        public Queue<GrandMa> Grannies { get; set; }
    }
    class MyMethods
    {
        private bool IsLetter(string str)
        {
            foreach (char el in str)
            {
                if (char.IsLetter(el))
                {
                    return true;
                }
            }
            return false;
        }
        public void PrintList(List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"Элемент {i + 1}: {list[i]}");
            }
        }
        public List<string> LoadImagesFromFolder(string folderPath)
        {
            List<string> imageList = new List<string>();
            try
            {
                string[] imageFiles = Directory.GetFiles(folderPath, "*.jpg").Concat(Directory.GetFiles(folderPath, "*.jpeg")).ToArray();

                foreach (string imagePath in imageFiles)
                {
                    string imageName = Path.GetFileName(imagePath);
                    imageList.Add(imageName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки картинки: {ex.Message}");
            }

            return imageList;
        }
        public List<Student> ReadStudentsFromFile(string filePath)
        {
            List<Student> students = new List<Student>();
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    string[] data = line.Split(' ');
                    if (data.Length == 6)
                    {
                        Student student = new Student
                        {
                            name = data[1],
                            surname = data[0],
                            birth = DateTime.Parse(data[2]),
                            exam = data[4],
                            mark = uint.Parse(data[5])
                        };
                        students.Add(student);
                    }
                }
            }
            return students;
        }
        public void PrintStudentsInfo(Dictionary<string, Student> matrix)
        {
            foreach (var element in matrix)
            {
                Console.Write($"\n{element.Key}: {element.Value.mark}");
            }
        }
        public void AddNewStudentToDictionary(Dictionary<string, Student> students)
        {
            Console.WriteLine("\nВведите фамилию студента: ");
            string lastName = Console.ReadLine();
            if (lastName == null || IsLetter(lastName))
            {
                Console.WriteLine("Введите имя студента: ");
                string firstName = Console.ReadLine();
                if (firstName == null || IsLetter(firstName))
                {
                    string key = $"{lastName.ToLower()} {firstName.ToLower()}";

                    if (students.ContainsKey(key))
                    {
                        Console.WriteLine("Студент с такой фамилией и именем уже существует.");
                        return;
                    }
                    else
                    {
                        Student student = new Student
                        {
                            surname = lastName.ToLower(),
                            name = firstName.ToLower()
                        };

                        Console.WriteLine("Введите год рождения студента: ");
                        if (DateTime.TryParse(Console.ReadLine(), out student.birth))
                        {
                            Console.WriteLine("Введите экзамен, на который поступил студент: ");
                            string inputExam = Console.ReadLine();
                            if (inputExam.ToLower() == "информатика" || inputExam.ToLower() == "физика" || inputExam.ToLower() == "английский язык")
                            {
                                student.exam = inputExam;
                                Console.Write("Введите баллы студента: ");
                                if (uint.TryParse(Console.ReadLine(), out student.mark) && (student.mark > 206) && (student.mark < 281))
                                {
                                    students[key] = student;
                                    Console.WriteLine("Студент добавлен.");
                                }
                                else
                                {
                                    Console.WriteLine("Введите корректные баллы ");
                                    return;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Введите корректный экзамен(информатика, физика или английский язык)");
                                return;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Введите корректную дату рождения формата dd.mm.year");
                            return;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Введите корректное имя!");
                }
            }
            else
            {
                Console.WriteLine("Введите корректную фамилию!");
            }
        }
        public void SaveStudentsToFile(Dictionary<string, Student> students, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var pair in students)
                {
                    writer.WriteLine($"{pair.Value.surname} {pair.Value.name} {pair.Value.birth} {pair.Value.exam} {pair.Value.mark}");
                }
            }
        }
        public void RemoveStudent(Dictionary<string, Student> studentDictionary)
        {
            Console.WriteLine("\nВведите фамилию студента, которого хотите удалить: ");
            string lastName = Console.ReadLine();
            Console.WriteLine("Введите имя студента, которого хотите удалить:  ");
            string firstName = Console.ReadLine();
            string key = $"{lastName.ToLower()} {firstName.ToLower()}";

            if (studentDictionary.ContainsKey(key))
            {
                studentDictionary.Remove(key);
                Console.WriteLine("Студент удален.");
            }
            else
            {
                Console.WriteLine("Студент с такой фамилией и именем не найден.");
            }
        }
        public void SortStudentsByMark(Dictionary<string, Student> studentDictionary)
        {
            var sortedStudents = studentDictionary.Values;
            List<Student> sortedList = new List<Student>(sortedStudents);
            sortedList.Sort((x, y) => x.mark.CompareTo(y.mark));

            Console.WriteLine("\nСтуденты, отсортированные по баллам (по возрастанию):");
            foreach (var student in sortedList)
            {
                Console.WriteLine($"{student.name} {student.surname}: {student.mark} баллов");
            }
        }
        public string SendDisease(Clinic hospital)
        {
            string diseasesNames = "";
            foreach (var disease in hospital.diseases)
            {
                diseasesNames += disease.name + ", ";
            } 
            if (!string.IsNullOrEmpty(diseasesNames))
            {
                diseasesNames = diseasesNames.Remove(diseasesNames.Length - 2);
            }
            return diseasesNames;
        }
    }
    internal class Program
    {
        static void Lesson1()
        {
            Console.WriteLine("Задание 1:");
            MyMethods method = new MyMethods();
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
            List<string> images = method.LoadImagesFromFolder(folderPath);

            foreach (string imagePath in images)
            {
                Console.WriteLine(imagePath);
            }

            Random random = new Random();
            int n = images.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                string value = images[k];
                images[k] = images[n];
                images[n] = value;
            }

            Console.WriteLine("Исходный List:");
            method.PrintList(images);
        }
        static void Lesson2()
        {
            Console.WriteLine("Задание 2:");
            MyMethods method = new MyMethods();

            List<Student> studentsList = method.ReadStudentsFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "students.txt"));
            Dictionary<string, Student> studentDictionary = new Dictionary<string, Student>();

            foreach (var student in studentsList)
            {
                string key = $"{student.surname} {student.name}";
                studentDictionary[key] = student;
            }

            bool flag = true;
            while (flag)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("a. Новый студент");
                Console.WriteLine("b. Удалить");
                Console.WriteLine("c. Сортировать");
                Console.WriteLine("d. Список студентов");
                Console.WriteLine("q. Выйти");

                Console.WriteLine("Выберите опцию: ");
                char choice = Console.ReadKey().KeyChar;

                switch (choice)
                {
                    case 'a':
                        method.AddNewStudentToDictionary(studentDictionary);
                        method.SaveStudentsToFile(studentDictionary, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "students.txt"));
                        break;
                    case 'b':
                        method.RemoveStudent(studentDictionary);
                        method.SaveStudentsToFile(studentDictionary, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "students.txt"));
                        break;
                    case 'c':
                        method.SortStudentsByMark(studentDictionary);
                        break;
                    case 'd':
                        method.PrintStudentsInfo(studentDictionary);
                        break;
                    case 'q':
                        method.SaveStudentsToFile(studentDictionary, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "students.txt"));
                        return;
                }
            }
        }
        static void Lesson3()
        {
            Console.WriteLine("\nЗадание 3:");
            var diseases = new List<Disease>
        {
            new Disease { name = "Артрит" },
            new Disease { name = "Гастрит" },
            new Disease { name = "Грипп" },
            new Disease { name = "Артроз" }
        };

            Queue<GrandMa> grandmom = new Queue<GrandMa>();
            Console.WriteLine("Введите количество бабушек: ");
            int qty = int.Parse(Console.ReadLine());

            for (int i = 0; i < qty; i++)
            {
                Console.Write($"Введите имя {i + 1}-й бабушки: ");
                string name = Console.ReadLine();
                Console.Write($"Введите возраст {i + 1}-й бабушки: ");
                int age = int.Parse(Console.ReadLine());

                Console.Write($"Введите количество болезней у {i + 1}-й бабушки: ");
                int diseasesQty = int.Parse(Console.ReadLine());
                var grannyDiseases = new List<Disease>();
                for (int j = 0; j < diseasesQty; j++)
                {
                    Console.Write($"Введите название {j + 1}-й болезни: ");
                    string diseaseName = Console.ReadLine();
                    grannyDiseases.Add(new Disease { name = diseaseName });
                }

                grandmom.Enqueue(new GrandMa { name = name, age = age, diseases = grannyDiseases });
            }

            var clinics = new Stack<Clinic>();
            clinics.Push(new Clinic { name = "Первая больница", diseases = new List<Disease> { diseases[0], diseases[3] }, capacity = 3, Grannies = new Queue<GrandMa>() });
            clinics.Push(new Clinic { name = "Вторая больница", diseases = new List<Disease> { diseases[1], diseases[2] }, capacity = 2, Grannies = new Queue<GrandMa>() });
            clinics.Push(new Clinic { name = "Третья больница", diseases = new List<Disease> { diseases[0], diseases[2] }, capacity = 1, Grannies = new Queue<GrandMa>() });

            foreach (var granny in grandmom)
            {
                bool added = false;
                foreach (var hospital in clinics)
                {
                    var treatableDiseasesCount = 0;
                    foreach (var dis in granny.diseases)
                    {
                        if (hospital.diseases.Contains(dis))
                        {
                            treatableDiseasesCount++;
                        }
                    }
                    if (treatableDiseasesCount >= granny.diseases.Count / 2)
                    {
                        hospital.Grannies.Enqueue(granny);
                        added = true;
                        break;
                    }
                }

                if (!added)
                {
                    Console.WriteLine($"Бабушка {granny.name} не попадает в больницу.");
                }
            }
            MyMethods methods = new MyMethods();
            foreach (var hospital in clinics)
            {
                var occupancyPercentage = (double)hospital.Grannies.Count / hospital.capacity * 100;
                Console.WriteLine($"Больница: {hospital.name}");
                Console.WriteLine($"Лечимые болезни: {methods.SendDisease(hospital)}");
                Console.WriteLine($"Процент заполненности: {occupancyPercentage}%");
                Console.WriteLine($"Бабуль в больнице:");
                foreach (var granny in hospital.Grannies)
                {
                    Console.WriteLine($"{granny.name}, {granny.age} лет");
                }
                Console.WriteLine();
            }
        }
        
        static void Main(string[] args)
        {
            Lesson1();
            Lesson2();
            Lesson3();
        }
    }
}
