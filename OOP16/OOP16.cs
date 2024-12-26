using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP16
{
    public class Course
    {
        public string Name { get; set; }
        public Teacher AssignedTeacher { get; set; }

        public Course(string name, Teacher teacher)
        {
            Name = name;
            AssignedTeacher = teacher;
        }
    }

    public class Teacher
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public List<string> Notes { get; set; } = new List<string>();

        public Teacher(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public void NewCourse(string courseName, List<Course> courses)
        {
            courses.Add(new Course(courseName, this));
        }

        public void RateStudent(Student student, string feedback)
        {
            Console.WriteLine($"Student {student.Name} rated: {feedback}");
        }

        public void MakeNote(string note)
        {
            Notes.Add(note);
        }
    }

    public class Student
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public List<Course> SubscribedCourses { get; set; } = new List<Course>();

        public Student(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public void SubscribeToCourse(Course course)
        {
            SubscribedCourses.Add(course);
        }

        public void CancelCourse(Course course)
        {
            SubscribedCourses.Remove(course);
        }

        public void GetCourses()
        {
            if (SubscribedCourses.Count == 0)
            {
                Console.WriteLine($"У студента {Name} нет курсов.");
                return;
            }

            Console.WriteLine($"Courses for {Name}:");
            foreach (var course in SubscribedCourses)
            {
                Console.WriteLine(course.Name);
            }
        }
    }

    public static class SystemManager
    {
        public static void RegisterTeacher(string name, string password, List<Teacher> teachers)
        {
            teachers.Add(new Teacher(name, password));
        }

        public static void RegisterStudent(string name, string password, List<Student> students)
        {
            students.Add(new Student(name, password));
        }

        public static object AuthenticateUser(string login, string password, List<Teacher> teachers, List<Student> students)
        {
            foreach (var teacher in teachers)
            {
                if (teacher.Name == login && teacher.Password == password)
                {
                    return teacher;
                }
            }

            foreach (var student in students)
            {
                if (student.Name == login && student.Password == password)
                {
                    return student;
                }
            }

            return null;
        }

        public static void RemoveTeacher(string name, List<Teacher> teachers)
        {
            teachers.RemoveAll(t => t.Name == name);
        }

        public static void RemoveStudent(string name, List<Student> students)
        {
            students.RemoveAll(s => s.Name == name);
        }

        public static void RemoveCourse(string name, List<Course> courses)
        {
            courses.RemoveAll(c => c.Name == name);
        }
    }

    internal class OOP16
    {
        static void Main(string[] args)
        {
            var teachers = new List<Teacher>();
            var students = new List<Student>();
            var courses = new List<Course>();

            SystemManager.RegisterTeacher("Dr. Smith", "password123", teachers);
            SystemManager.RegisterTeacher("Prof. Johnson", "physics2023", teachers);

            while (true)
            {
                Console.WriteLine("Добро пожаловать! Выберите действие:");
                Console.WriteLine("1. Войти");
                Console.WriteLine("2. Зарегистрироваться (только для студентов)");
                Console.WriteLine("3. Выйти из программы");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.WriteLine("Введите логин:");
                    string login = Console.ReadLine();
                    Console.WriteLine("Введите пароль:");
                    string password = Console.ReadLine();

                    var currentUser = SystemManager.AuthenticateUser(login, password, teachers, students);

                    if (currentUser is Teacher teacher)
                    {
                        Console.WriteLine($"Добро пожаловать, преподаватель {teacher.Name}!");

                        Console.WriteLine("Выберите действие:");
                        Console.WriteLine("1. Создать курс");
                        Console.WriteLine("2. Оценить студента");
                        Console.WriteLine("3. Добавить заметку");
                        string teacherChoice = Console.ReadLine();

                        if (teacherChoice == "1")
                        {
                            Console.WriteLine("Введите название курса:");
                            string courseName = Console.ReadLine();
                            teacher.NewCourse(courseName, courses);
                            Console.WriteLine($"Курс {courseName} успешно создан.");
                        }
                        else if (teacherChoice == "2")
                        {
                            if (students.Count == 0)
                            {
                                Console.WriteLine("Нет зарегистрированных студентов для оценки.");
                                continue;
                            }

                            Console.WriteLine("Введите имя студента для оценки:");
                            string studentName = Console.ReadLine();
                            var studentToRate = students.Find(s => s.Name == studentName);

                            if (studentToRate != null)
                            {
                                Console.WriteLine("Введите отзыв для студента:");
                                string feedback = Console.ReadLine();
                                teacher.RateStudent(studentToRate, feedback);
                            }
                            else
                            {
                                Console.WriteLine("Студент не найден.");
                            }
                        }
                        else if (teacherChoice == "3")
                        {
                            Console.WriteLine("Введите заметку:");
                            string note = Console.ReadLine();
                            teacher.MakeNote(note);
                            Console.WriteLine("Заметка добавлена.");
                        }
                        else
                        {
                            Console.WriteLine("Неверный выбор.");
                        }
                    }
                    else if (currentUser is Student student)
                    {
                        Console.WriteLine($"Добро пожаловать, студент {student.Name}!");

                        Console.WriteLine("Выберите действие:");
                        Console.WriteLine("1. Подписаться на курс");
                        Console.WriteLine("2. Просмотреть свои курсы");
                        string studentChoice = Console.ReadLine();

                        if (studentChoice == "1")
                        {
                            if (courses.Count == 0)
                            {
                                Console.WriteLine("Нет доступных курсов для подписки.");
                                continue;
                            }

                            Console.WriteLine("Доступные курсы:");
                            for (int i = 0; i < courses.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {courses[i].Name} (Преподаватель: {courses[i].AssignedTeacher.Name})");
                            }

                            Console.WriteLine("Введите номер курса для подписки:");
                            if (int.TryParse(Console.ReadLine(), out int courseIndex) && courseIndex > 0 && courseIndex <= courses.Count)
                            {
                                var selectedCourse = courses[courseIndex - 1];
                                student.SubscribeToCourse(selectedCourse);
                                Console.WriteLine($"Вы успешно подписались на курс {selectedCourse.Name}.");
                            }
                            else
                            {
                                Console.WriteLine("Неверный выбор курса.");
                            }
                        }
                        else if (studentChoice == "2")
                        {
                            student.GetCourses();
                        }
                        else
                        {
                            Console.WriteLine("Неверный выбор.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неверные логин или пароль.");
                    }
                }
                else if (choice == "2")
                {
                    Console.WriteLine("Введите имя для регистрации:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Введите пароль для регистрации:");
                    string password = Console.ReadLine();

                    SystemManager.RegisterStudent(name, password, students);
                    Console.WriteLine($"Студент {name} успешно зарегистрирован!");
                }
                else if (choice == "3")
                {
                    Console.WriteLine("Выход из программы. До свидания!");
                    break;
                }
                else
                {
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                }
            }
        }
    }
}
