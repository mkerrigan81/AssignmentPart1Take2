using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Data.Linq;

namespace AssignmentPart1Take2
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();

            using (var db = new StudyAndPerformanceModelContainer())
            {
                //Creating an interface that allows the user to choose which question/information they want to go to
                Console.WriteLine("COURSE RELATED INFORMATION SYSTEM");
                Console.WriteLine("=================================");
                while (true)
                {
                    Console.WriteLine();
                    Console.WriteLine("1) List all Lecturer's: Names, Emails and Contact Numbers");
                    Console.WriteLine("2) Given a Lecturer's Staff Number, display the lecturers names and modules they teach");
                    Console.WriteLine("3) Given a Lecturer's Course Code, list the Module Code and Module Name linked with the course");
                    Console.WriteLine("4) Given a Module Code, display the Course Code plus name of all the courses linked with that module");
                    Console.WriteLine("5) Given a Student Number, display the student name and list the Module code, module name and result for all modules taken by that Student");
                    Console.WriteLine("6) Display the change in the system when a lecturer is replaced by a new recruit with a different staff number but same modules");

                    int userSelection = 0;
                    Console.WriteLine("Enter your selection: ");
                    while (!(int.TryParse(Console.ReadLine(), out userSelection) && (userSelection >= 1 && userSelection <= 6)))
                    {
                        Console.WriteLine("You need to enter a value between 1..6");
                        Console.Write("ENTER Your Selection: ");

                    }
                    //Depending on what number the user choose, it will execute one of the methods in the code below
                    Console.WriteLine();
                    switch (userSelection)
                    {
                        case 1:
                            program.LecturerList(db);
                            break;
                        case 2:
                            program.LecturerNameModuleName(db);
                            break;
                        case 3:
                            program.CourseCodeModuleCode(db);
                            break;
                        case 4:
                            program.ModuleCodeCourseCode(db);
                            break;
                        case 5:
                            program.ListStudentListModule(db);
                            break;
                        case 6:
                            program.ReplaceLecturer(db);
                            break;
                    }
                }
            }

        }



        
        //QUESTION B) 1
        public void LecturerList(StudyAndPerformanceModelContainer dbL)
        {
            //Linq query to get the lecturer's information from the Lecturers Table and sort it by surname
            var query = from l in dbL.Lecturers
                        orderby l.Surname
                        select l;
            Console.WriteLine("Lecturer's Listings");
            Console.WriteLine("----------------------------------------------------");
            foreach (var item in query)
            {
                //Ouputting the Lecturer's information to the console
                Console.WriteLine(item.Surname + " | " + item.FirstName + " | " + item.EmailAddress + " | " + item.ContactNumber);
            }
            Console.WriteLine("----------------------------------------------------");
        }//end of Question 1 

        //QUESTION B) 2
        public void LecturerNameModuleName(StudyAndPerformanceModelContainer dBL)
        {
            string staffNumber = getStaffNumber();
            //Doing a check to make sure that Staff Number exists in my records 
            bool lecturerExists = dBL.Lecturers.Any(l => l.StaffNumber.Equals(staffNumber));

            if (lecturerExists)
            {
                //If the Lecturer with that Staff Number exists we come in here and execute the following Linq query
                //which joins the Lecturer table with the Module table based on the Lecturer Id
                var query = from m in dBL.Modules
                            join l in dBL.Lecturers on m.LecturerId equals l.Id
                            where (l.StaffNumber.Equals(staffNumber))
                            select new
                            {
                                module = m,
                                lecturer = l
                            };
                Console.WriteLine("Lecturer's Listings");
                Console.WriteLine("----------------------------------------------------");

                if (query.Any())
                {
                    //From the query, we are outputting information firstly from the Lecturer table with their First Name and Surname
                    Console.WriteLine("Lecturer's Name: " + query.FirstOrDefault().lecturer.FirstName + " " + query.FirstOrDefault().lecturer.Surname);
                    Console.WriteLine("Modules Lecturer teaches: ");
                    Console.WriteLine("--------------------------");
                    foreach (var item in query)
                    {
                        //We then execute a foreach which will list all the names of the Modules which are taught by that Lecturer 
                        Console.WriteLine(item.module.ModuleName);
                    }
                }
                else
                {
                    Console.WriteLine("Error somewhere in LINQ Query");
                }
            }
            else
            {
                Console.WriteLine("No Lecturer's exist with that Staff Number");
                Console.WriteLine("------------------------------------------");
            }
        }//end of Question 2 

        //QUESTION B) 3
        public void CourseCodeModuleCode(StudyAndPerformanceModelContainer dBL)
        {
            string courseCode = getCourseCode();
            //Doing a check to make sure that the course exists in my records
            bool courseExists = dBL.Courses.Any(c => c.CourseCode.Equals(courseCode));

            if (courseExists)
            {
                var query = from m in dBL.Modules
                            join c in dBL.Courses on m.CourseId equals c.Id
                            where (c.CourseCode.Equals(courseCode))
                            select new 
                            { 
                                module = m,
                                course = c
                            };
                Console.WriteLine("Course and Module Listings");
                Console.WriteLine("----------------------------------------------------");

                if (query.Any())
                {
                    Console.WriteLine("Course Name: " + query.FirstOrDefault().course.CourseName);
                    Console.WriteLine("Modules linked with that Course: ");
                    Console.WriteLine("--------------------------");
                    foreach (var item in query)
                    {
                        Console.WriteLine(item.module.ModuleCode + " | " + item.module.ModuleName);
                    }
                }
                else
                {
                    Console.WriteLine("Error somewhere in LINQ Query");
                }
            }
            else
            {
                Console.WriteLine("No Course exists with that Course Number");
                Console.WriteLine("------------------------------------------");
            }
        }//end of Question 3

        //QUESTION B) 4
        public void ModuleCodeCourseCode(StudyAndPerformanceModelContainer dBL)
        {
            string moduleCode = getModuleCode();
            //Doing a check to make sure that the module exists in my records 
            bool moduleExists = dBL.Modules.Any(m => m.ModuleCode.Equals(moduleCode));

            if (moduleExists)
            {
                var query = from m in dBL.Modules
                            join c in dBL.Courses on m.CourseId equals c.Id
                            where (m.ModuleCode.Equals(moduleCode))
                            select new
                            {
                                module = m,
                                course = c
                            };
                Console.WriteLine("Course and Module Listings");
                Console.WriteLine("----------------------------------------------------");

                if (query.Any())
                {
                    Console.WriteLine("Module Name: " + query.FirstOrDefault().module.ModuleName);
                    Console.WriteLine("Courses linked with that Module: ");
                    Console.WriteLine("--------------------------");
                    foreach (var item in query)
                    {
                        Console.WriteLine(item.course.CourseCode + " | " + item.course.CourseName);
                    }
                }
                else
                {
                    Console.WriteLine("Error somewhere in LINQ Query");
                }
            }
            else
            {
                Console.WriteLine("No Module exists with that Module Number");
                Console.WriteLine("------------------------------------------");
            }
        }//end of Question 4

        //Question B) 5
        public void ListStudentListModule(StudyAndPerformanceModelContainer dBL)
        {
            string studentNumber = getStudentNumber();
            //Doing a check to see if the student exists in my records 
            bool studentExists = dBL.Students.Any(s => s.StudentNumber.Equals(studentNumber));

            if (studentExists)
            {
                //In this linq query we are executing 2 different joins since we will be pulling data from more than 2 tables
                //We are joining the Student table with Results firstly based on StudentId
                //Then we are doing the same for the Module table only with ModuleId
                var query = from r in dBL.Results
                            join s in dBL.Students on r.StudentId equals s.Id
                            join m in dBL.Modules on r.ModuleId equals m.Id
                            where (s.StudentNumber.Equals(studentNumber))
                            select new
                            {
                                result = r,
                                student = s,
                                module = m
                            };
                Console.WriteLine("Student Listings and Results");
                Console.WriteLine("----------------------------");

                if (query.Any())
                {
                    Console.WriteLine("Student Name: " + query.FirstOrDefault().student.FirstName + " " + query.FirstOrDefault().student.Surname);
                    Console.WriteLine("Modules taken and Results");
                    foreach (var item in query)
                    {
                        Console.WriteLine(item.module.ModuleCode + " | " + item.module.ModuleName + " | " + item.result.FinalResult);
                    }
                }
                else
                {
                    Console.WriteLine("Error somewhere in LINQ Query");
                }
            }

            else
            {
                Console.WriteLine("No Student exists with that Student Number");
                Console.WriteLine("------------------------------------------");
            }
        }//end of Question 5 

        //Question B) 6

        public void ReplaceLecturer(StudyAndPerformanceModelContainer dBL)
        {
            //This LINQ Query takes the details of a specific lecturer 
            var lecturerDetails =
                from l in dBL.Lecturers
                where l.FirstName == "Jeremy" && l.Surname == "Vedder" && l.EmailAddress == "jveds@test.com" && l.ContactNumber == "077 1241 2628"
                select l;

            foreach (var item in lecturerDetails)
            {
                //Here I am replacing all the old lecturer's details with new details 
                item.Id = 4;
                item.StaffNumber = "S80173";
                item.FirstName = "Stone";
                item.Surname = "McCready";
                item.EmailAddress = "mccreadystone@test.com";
                item.ContactNumber = "079 5192 0012";
              
            }
            try
                {
                //saving changes to the database 
                dBL.SaveChanges();
            }
            catch (Exception e)
            {
                //If there is a problem with saving the changes to the database I provide an exception method 
                Console.WriteLine("Big Exception Here Boys");
                Console.WriteLine("-----------------------");
                Console.WriteLine(e);
            }
        }

        //--------------Additional Methods----------------
        string getInput(string msg, string regExpression)
        {
            Console.Write(msg);
            String input = Console.ReadLine();
            //General input method using Regular Expression
            //If the expression is met the user can proceed and if not they will get an error message
            while (!Regex.IsMatch(input, regExpression))
            {
                Console.WriteLine("Invalid Entry");
                Console.WriteLine(msg);
                input = Console.ReadLine();
            }
            return input;
        }


        //Different methods each using the getInput method from above
        //These are for the different numbers and codes
        string getStaffNumber()
        {
            return getInput("Enter Staff Number:", @"^[S]{1}[0-9]+$");
        }

        string getCourseCode()
        {
            return getInput("Enter Course Number:", @"^[C]{1}[0-9]+$");
        }

        string getModuleCode()
        {
            return getInput("Enter Module Number:", @"^[M]{1}[0-9]+$");
        }

        string getStudentNumber()
        {
            return getInput("Enter Student Number:", @"^[S]{1}[0-9]+$");
        }
    }
}
