using System;

namespace ToDoList
{
    class Program
    {
        static void Main(string[] args)
        {
            // Entry point of the program
            TaskManager taskManager = new TaskManager();

            Console.WriteLine("Welcome to my To-Do List app");

            bool authenticated = false;
            string currentUser = string.Empty;

            while (!authenticated)
            {
                Console.WriteLine("Please choose an option:");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Sign up");
                string choice = Console.ReadLine()?.Trim() ?? "";

                switch (choice)
                {
                    // User authentication loop
                    case "1":
                        authenticated = Login(taskManager);
                        break;
                    case "2":
                        SignUp();
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }

            if (!authenticated)
            {
                // Exit if authentication fails
                Console.WriteLine("Authentication failed, goodbye!");
                return;
            }

            taskManager.LoadTasksFromFile("tasks.txt");

            Console.WriteLine("Welcome to the To-Do list app \n");

            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. View Tasks");
                Console.WriteLine("3. Exit");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    // Main menu loop
                    case "1":
                        taskManager.AddTask();
                        break;
                    case "2":
                        taskManager.ViewTasks();
                        break;
                    case "3":
                        TaskManager.SaveTasksToFile("tasks.txt", taskManager.Tasks);
                        Console.WriteLine("Exiting the To-Do List App. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.\n");
                        break;
                }
            }
        }

        static bool Login(TaskManager taskManager)
        {
            // Method to login a user
            Console.Write("Enter Username:");
            string username = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Enter Password:");
            string password = Console.ReadLine()?.Trim() ?? "";

            bool isAuthenticated = UserAuth.Authenticate(username, password);
            if (isAuthenticated)
            {
                taskManager.CurrentUser = username;
            }

            return isAuthenticated;
        }

        static void SignUp()
        {
            // Method to sign up a new user
            Console.Write("Enter new Username:");
            string username = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Enter new Password:");
            string password = Console.ReadLine()?.Trim() ?? "";

            if (UserAuth.SignUp(username, password))
            {
                Console.WriteLine("User registered successfully! You can now login.");
            }
            else
            {
                Console.WriteLine("Username already exists. Please try again with a different username.");
            }
        }
    }
}
