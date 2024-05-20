using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace ToDoList
{
    class Program
    {
        public class userAuth //login credentials
        {
            private const string Username = "user";
            private const string Password = "root";

            public static bool Authenticate(string inputUsername, string inputPassword)
            {
                return inputUsername == Username.Trim() && inputPassword == Password.Trim();
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Login"); // user login

            Console.Write("Enter Username: \n");
            string Username = Console.ReadLine().Trim(); // trim whitespace from input

            Console.Write("Enter Password: \n");
            string Password = Console.ReadLine().Trim();// trim whitespace from input

            if (userAuth.Authenticate(Username, Password))
            {
                Console.Clear(); // clears previous messages
                Console.WriteLine($"Authentication successful! welcome, {Username} ");

            }
            else
            {
                Console.WriteLine("Incorrect username or password please try again");
                return; //exit 
            }


            List<string> tasks = LoadTasksFromFile("tasks.txt"); // loads any data which was stored 

            Console.WriteLine("Welcome to the To-Do List App!\n");

            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. View Tasks");
                Console.WriteLine("3. Exit");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1": // to add tasks
                        Console.Write("Enter the task: ");
                        string task = Console.ReadLine() ?? ""; // Use null-coalescing operator
                        tasks.Add(task);
                        Console.WriteLine("Task added successfully!\n");
                        break;
                    case "2": // to view tasks
                        if (tasks.Count == 0) // no tasks
                        {
                            Console.WriteLine("No tasks found.\n");
                        }
                        else
                        {
                            Console.WriteLine("Tasks:");
                            for (int i = 0; i < tasks.Count; i++) // tasks present 
                            {
                                Console.WriteLine($"{i + 1}. {tasks[i]}");
                            }
                            Console.WriteLine();

                            Console.WriteLine("Please select an option");
                            Console.WriteLine("1. Delete task");
                            Console.WriteLine("2. Update task");
                            Console.WriteLine("3. Return to main menu");

                            string subChoice = Console.ReadLine() ?? "";
                            
                            switch (subChoice)
                            {
                                case "1":
                                    Console.WriteLine("Enter the task you want to delete");
                                    if (int.TryParse(Console.ReadLine(), out int taskNumberToDelete) && taskNumberToDelete > 0 && taskNumberToDelete <= tasks.Count)
                                    {
                                        tasks.RemoveAt(taskNumberToDelete - 1);
                                        Console.WriteLine("Task has been deleted successfuly");
                                    }
                                    else
                                    {
                                        Console.WriteLine("It dont exist m8 \n");
                                    }
                                    break;
                                case "2":
                                    Console.WriteLine("Which task will you like to update?");
                                    if (int.TryParse(Console.ReadLine(), out int taskNumberToUpdate) && taskNumberToUpdate > 0 && taskNumberToUpdate <= tasks.Count)
                                    {
                                        Console.WriteLine("Enter the new task descripion: ");
                                        string newTask = Console.ReadLine() ?? "";
                                        tasks[taskNumberToUpdate - 1] = newTask;
                                        Console.WriteLine("Task updated succesfully\n ");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid option, Try again");

                                    }
                                    break;

                                case "3":
                                    break;

                                default:
                                    Console.WriteLine("Invalid option. Please Try again!");
                                    break;
                            }
                        }
                        break;


                    case "3": // exit application and save tasks
                        SaveTasksToFile("tasks.txt", tasks);
                        Console.WriteLine("Exiting the To-Do List App. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.\n");
                        break;
                }
            }
        }

        static List<string> LoadTasksFromFile(string filename)
        {
            List<string> tasks = new List<string>();

            try
            {
                if (File.Exists(filename))
                {
                    tasks.AddRange(File.ReadAllLines(filename));
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error loading tasks: {ex.Message}");
            }
            
            return tasks;
        }

        static void SaveTasksToFile(string filename, List<string> tasks)
        { 
            try
            {
                File.WriteAllLines(filename, tasks);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error saving tasks: {ex.Message}");
            }
        }    
    }
}
