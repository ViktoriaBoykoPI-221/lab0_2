using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        string filePath = "Students.csv"; // Путь к файлу CSV
        if (!File.Exists(filePath))
        {
            // Если файл не существует, создаем его и добавляем заголовок
            File.WriteAllText(filePath, "Фамилия,Имя,Группа,Оценка\n");
        }

        List<Student> students = ReadCsvFile(filePath);

        Console.WriteLine("Программа управления данными о защите дипломов");
        while (true)
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Вывести студентов по группе и оценке");
            Console.WriteLine("2. Выход");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Введите группу (ПИ, МОА, ПМИ, КБ): ");
                    string group = Console.ReadLine();
                    Console.Write("Введите оценку(3-5): ");
                    if (int.TryParse(Console.ReadLine(), out int grade))
                    {
                        List<Student> filteredStudents = GetStudentsByGroupAndGrade(students, group, grade);
                        if (filteredStudents.Count > 0)
                        {
                            Console.WriteLine("Результаты:");
                            Console.WriteLine("┌──────────────┬─────────────┬──────────┬─────────┐");
                            Console.WriteLine("│    Фамилия   │     Имя     │  Группа  │ Оценка  │");
                            Console.WriteLine("├──────────────┼─────────────┼──────────┼─────────┤");
                            foreach (var student in filteredStudents)
                            {
                                Console.WriteLine($"│ {student.LastName,-12} │ {student.FirstName,-11} │ {student.Group,-8} │ {student.Grade,-7} │");
                            }
                            Console.WriteLine("└─────-────────┴─────────────┴──────────┴─────────┘");
                        }
                        else
                        {
                            Console.WriteLine("Нет студентов с заданной группой и оценкой.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Введите корректную оценку.");
                    }
                    break;
                case "2":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Пожалуйста, выберите 1 или 2.");
                    break;
            }
        }
    }

    static List<Student> ReadCsvFile(string filePath)
    {
        List<Student> students = new List<Student>();
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                // Пропускаем первую строку с заголовками
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] data = line.Split(',');
                    if (data.Length == 4)
                    {
                        string lastName = data[0];
                        string firstName = data[1];
                        string group = data[2];
                        int grade = int.Parse(data[3]);

                        students.Add(new Student(lastName, firstName, group, grade));
                    }
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Файл не найден.");
        }
        return students;
    }

    static List<Student> GetStudentsByGroupAndGrade(List<Student> students, string group, int grade)
    {
        return students.Where(student => student.Group == group && student.Grade == grade).ToList();
    }
}

class Student
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Group { get; set; }
    public int Grade { get; set; }

    public Student(string lastName, string firstName, string group, int grade)
    {
        LastName = lastName;
        FirstName = firstName;
        Group = group;
        Grade = grade;
    }
}
