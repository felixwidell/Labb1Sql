using Labb1Sql___.Data;
using Labb1SQL___.Models;
using Microsoft.Identity.Client;
using System.Linq;

namespace Labb1Sql___
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Skriv in siffran på valet du vill göra.\n 1. Hämta alla elever \n 2. Hämta alla elever i en viss klass \n 3. Lägga till ny personal \n 4. Hämta personal \n 5. Hämta alla betyg som satts den senaste månaden \n 6. Snittbetyg per kurs \n 7. Lägga till nya elever");
            var choice = Console.ReadLine();
            

            using (SkolContext context = new SkolContext())
            {
                var AllStudents = context.Elever.ToList();
                var AllStaff = context.Personal.ToList();
                var AllGrades = context.Betyg.ToList();
                var AllClasses = context.Klasser.ToList();
                switch (choice)
                {
                    case "1":
                        GetAllStudents(AllStudents);
                        break;
                    case "2":
                        GetAllStudentsFromClass(AllStudents, AllClasses);
                        break;
                    case "3":
                        var NewStaff = AddStaff();
                        context.Personal.Add(NewStaff);
                        context.SaveChanges();
                        Console.WriteLine("Personalen har blivit tillagd");
                        break;
                    case "4":
                        GetAllStaff(AllStaff);
                        break;
                    case "5":
                        GetLatestMonthGrades(AllStudents, AllGrades);
                        break;
                    case "6":
                        GetAverageGrade(AllStudents, AllClasses);
                        break;
                    case "7":
                        var NewStudent = AddStudent(AllClasses);
                        context.Elever.Add(NewStudent);
                        context.SaveChanges();
                        Console.WriteLine("Eleven har blivit tillagd");
                        break;
                }


                


            }

            static void GetAllStudents(List<Elever> Students)
            {
                foreach(Elever elever in Students)
                {
                    Console.WriteLine($"{elever.Firtsname} {elever.Lastname}");
                }
            }

            static void GetAllStudentsFromClass(List<Elever> Students, List<Klasser> Classes)
            {
                ConsoleKeyInfo keyinfo;
                int classIndex = 0;

                do
                {
                    int listItem2 = 0;
                    keyinfo = Console.ReadKey();

                    if (keyinfo.Key == ConsoleKey.DownArrow && classIndex < Classes.Count - 1)
                    {
                        Console.Clear();
                        classIndex++;
                        foreach (var Class in Classes)
                        {
                            if (classIndex == listItem2)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(Class.Class);
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine(Class.Class);
                            }

                            listItem2++;
                        }
                        Console.WriteLine("Class: " + classIndex);
                    }
                    else if (keyinfo.Key == ConsoleKey.UpArrow && classIndex > 0)
                    {
                        Console.Clear();
                        classIndex--;
                        foreach (var Class in Classes)
                        {
                            if (classIndex == listItem2)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(Class.Class);
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine(Class.Class);
                            }

                            listItem2++;
                        }
                        Console.WriteLine("Class: " + classIndex);
                    }
                } while (Console.ReadKey().Key != ConsoleKey.Enter);
                var StudentsInClass = Students.Where(x => x.ClassID == classIndex + 1).Select(x => x).ToList();
                var SelectedClass = Classes.Where(x => x.ID == classIndex + 1).Select(x => x.Class);
                foreach( var student in StudentsInClass) 
                {
                    Console.WriteLine($"{student.Firtsname} {student.Lastname} {SelectedClass.First()}");
                }
                

            }

            static Personal AddStaff()
            {
                Console.Write("Förnamn: ");
                var Firstname = Console.ReadLine();
                Console.WriteLine("Efternamn: ");
                var Lastname = Console.ReadLine();
                Console.WriteLine("Kategori: ");
                var Category = Console.ReadLine();

                var NewStaff = new Personal();
                NewStaff.Firstname = Firstname;
                NewStaff.Lastname = Lastname;
                NewStaff.Category = Category;

                return NewStaff;
            }

            static void GetAllStaff(List<Personal> AllStaff) 
            {
                foreach( var personal in AllStaff)
                {
                    Console.WriteLine(personal.Firstname + " " + personal.Lastname + " " + personal.Category);
                }
            }

            static void GetLatestMonthGrades(List<Elever> AllStudents, List<Betyg> AllGrades)
            {
                var Today = DateTime.Today;
                var LatestGrade = AllStudents.Where(x => x.GradeDate.Month == Today.Month).ToList();
                foreach( var student in LatestGrade)
                {
                    Console.WriteLine($"{student.Firtsname} {student.Lastname} {AllGrades.Where(x => x.ID == student.GradeID).First().Grade} {student.GradeDate}");
                }
            }

            static void GetAverageGrade(List<Elever> AllStudents, List<Klasser> AllClasses)
            {
                foreach(var Class in AllClasses)
                {
                    int GradeAverage = 0;
                    string StringGradeAverage;
                    var StudentsInClass = AllStudents.Where(x => x.ClassID == Class.ID).ToList();
                    foreach ( var student in StudentsInClass)
                    {
                        int Grade = student.GradeID;
                        GradeAverage = GradeAverage + Grade;
                    }
                    if (Math.Round(GradeAverage / Convert.ToDouble(StudentsInClass.Count), 1) == 3) 
                    {
                        StringGradeAverage = "VG";
                    }
                    else if (Math.Round(GradeAverage / Convert.ToDouble(StudentsInClass.Count), 1) == 2)
                    {
                        StringGradeAverage = "G";
                    }
                    else
                    {
                        StringGradeAverage = "IG";
                    }
                    Console.WriteLine($"{Class.Class} {StringGradeAverage}");
                }
            }

            static Elever AddStudent(List<Klasser> Classes)
            {
                Console.Write("Förnamn: ");
                var Firstname = Console.ReadLine();
                Console.WriteLine("Efternamn: ");
                var Lastname = Console.ReadLine();

                ConsoleKeyInfo keyinfo;
                int classIndex = 0;
                do
                {
                    int listItem2 = 0;
                    keyinfo = Console.ReadKey();

                    if (keyinfo.Key == ConsoleKey.DownArrow && classIndex <= Classes.Count)
                    {
                        Console.Clear();
                        classIndex++;
                        foreach (var Class in Classes)
                        {
                            if (classIndex == listItem2)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(Class.Class);
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine(Class.Class);
                            }

                            listItem2++;
                        }
                        Console.WriteLine("Class: " + classIndex);
                    }
                    else if (keyinfo.Key == ConsoleKey.UpArrow && classIndex > 0)
                    {
                        Console.Clear();
                        classIndex--;
                        foreach (var Class in Classes)
                        {
                            if (classIndex == listItem2)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine(Class.Class);
                                Console.ResetColor();
                            }
                            else
                            {
                                Console.WriteLine(Class.Class);
                            }

                            listItem2++;
                        }
                        Console.WriteLine("Class: " + classIndex);
                    }
                } while (Console.ReadKey().Key != ConsoleKey.Enter);

                var NewStudent = new Elever();
                NewStudent.Firtsname = Firstname;
                NewStudent.Lastname = Lastname;
                NewStudent.ClassID = Classes[classIndex].ID;

                return NewStudent;
            }
        }
    }
}