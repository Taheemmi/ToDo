using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ToDoList
{
    public class Task
    {
        public string Description { get; set; }
        public string Category { get; set; }

        public Task(string description, string category)
        {
            Description = description;
            Category = category;

        }

        public override string ToString()
        {
            return $"{Description} {{Category}}";
        }
    }

    public class userAuth //  login credentials
    {
        private const string Username = "user"; // this is the username to login
        private const string Password = "root"; // this is the password

        public static bool Authenticate(string inputUsername, string inputPassword) // checks if the username and password are correct
        {
            return inputUsername == Username.Trim() && inputPassword == Password.Trim(); //white
        }
    }

    class Program
    {
        static List<Task> tasks = new List<Task>();

        static void Main(string[] args)
        {
            Console.WriteLine("Login"); // user login

            Console.Write("Enter Username: \n");
            string username = Console.ReadLine().Trim(); // trim whitespace from input

            Console.Write("Enter Password: \n");
            string password = Console.ReadLine().Trim();// trim whitespace from input

            if (userAuth.Authenticate(username, password))
            {
                Console.Clear(); // clears previous messages
                Console.WriteLine($"Authentication successful! welcome, {username} ");
            }
            else
            {
                Console.WriteLine("Incorrect username or password please try again"); // if credentials do not match
                return; //exit 
            }

            tasks = LoadTasksFromFile("tasks.txt"); // loads any data which was stored 

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
                        AddTask();
                        break;

                    case "2": // to view tasks
                        ViewTasks();
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

        static void AddTask()
        {
            Console.Write("Enter the task: ");
            string description = Console.ReadLine() ?? ""; // Use null-coalescing operator

            Console.WriteLine("Select the category: ");
            Console.WriteLine("1. Work");
            Console.WriteLine("2. Personal");
            string categoryChoice = Console.ReadLine() ?? "";

            string category = categoryChoice switch
            {
                "1" => "Work",
                "2" => "Personal",
                _ => "SideQuests"
            };

            Task newTask = new Task(description, category);
            tasks.Add(newTask);
            Console.WriteLine("Task connected sucessfully");
        }

        static void ViewTasks()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks found\n");
                return;
            }

            Console.WriteLine("Tasks:");
            for (int i = 0; i < tasks.Count; i++) // tasks present 
            {
                Console.WriteLine($"{i + 1}. {tasks[i]}");
            }
            Console.WriteLine();

            Console.WriteLine("Select an option: ");
            Console.WriteLine("1. Delete a Task");
            Console.WriteLine("2. Update a Task");
            Console.WriteLine("3. Go back");
            String option = Console.ReadLine() ?? "";

            switch (option)
            {
                case "1":
                    DeleteTask();
                    break;
                case "2":
                    UpdateTask();
                    break;
                case "3":
                    break;
                default:
                    Console.WriteLine("Cant do that mate");
                    break;
                }
            }

        // delete a task based on a number
        static void DeleteTask()
        {
            Console.WriteLine("Enter the task number you would like to delete: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumberToDelete) && taskNumberToDelete > 0 && taskNumberToDelete <= tasks.Count)  // this string reads the users input and convert the string to an interger. If successful the method is set to 1 for true and if not it is set to 0. Then checks if larger than 0.
            { 
                tasks.RemoveAt(taskNumberToDelete - 1);
                Console.WriteLine("Task deleted successfully!\n");
            }
            else
            {
                Console.WriteLine("Invalid task number. Please try again. \n");
            }
        }

        // update description and category of task
        static void UpdateTask()
        {
            Console.Write("Enter the number of the task you want to update: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumberToUpdate) && taskNumberToUpdate > 0 && taskNumberToUpdate <= tasks.Count)
            {
                Task taskToUpdate = tasks[taskNumberToUpdate - 1];
                Console.Write("Enter the new description: ");

                taskToUpdate.Description = Console.ReadLine() ?? "";
                Console.WriteLine("Select the new category: ");
                Console.WriteLine("1. Work");
                Console.WriteLine("2. Personal");
                string categoryChoice = Console.ReadLine() ?? "";
                taskToUpdate.Category = categoryChoice switch

                {
                    "1" => "Work",
                    "2" => "Personal",
                    _ => taskToUpdate.Category
                };
                Console.WriteLine("Task Updated Successfully!\n");
            }
            else
            {
                Console.WriteLine("Dunno mate \n");
            }

        }

        // load tasks from file including categories
        static List<Task> LoadTasksFromFile(string filename)
        {
            List<Task> tasks = new List<Task>();
            try
            {
                if (File.Exists(filename))

                {
                    string[] lines = File.ReadAllLines(filename);
                    foreach (var line in lines)
                    {
                        string[] parts = line.Split('|');
                        if (parts.Length == 2)
                        {
                            tasks.Add(new Task(parts[0], parts[1]));
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error loading tasks: {ex.Message}");
            }
            return tasks;
        }

        // save tasks to file, including categories
        static void SaveTasksToFile(string filename, List<Task> tasks)
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (var task in tasks)

                {
                    lines.Add($"{task.Description}|{task.Category}");
                }
                File.WriteAllLines(filename, lines);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error saving tasks: {ex.Message}");
            }
        }
    }
}
