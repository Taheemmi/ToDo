using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ToDoList
{
    public class Task
    {
        public string Description { get; set; } // get is to return property value and set is a new value
        public string Category { get; set; }
        public string Priority { get; set; }
        public DateTime Deadline { get; set; }

        public Task(string description, string category, string priority, DateTime deadline) // this lists all the factors which go into a todo note and assigns it to the respected string
        {
            Description = description;
            Category = category;
            Priority = priority;
            Deadline = deadline;
        }

        public int DaysRemaining()
        {
            return (int)(Deadline - DateTime.Now).TotalDays; // from the users input it calculates how many days are remaining to the system date
        }

        public override string ToString()
        {
            return $"{Description} ({Category}) - Priority: {Priority}, Days remaining: {DaysRemaining()}"; // How it will display
        }
    }

    public class TaskManager
    {
        public List<Task> Tasks = new List<Task>();

        public void LoadTasks(List<Task> tasks)
        {
            Tasks = tasks;
        }

        public void AddTask()
        {
            Console.Write("Enter the task: ");
            string description = Console.ReadLine()?.Trim() ?? "";

 /* for every user input, I use ' ?.Trim() ?? '
 * ?. checks if the value is not null before trying to call Trim().
 * Trim() removes spaces at the start and end of the string.
 * ?? returns the value on the left unless it is null
 * "" is returned if nothing on the left is inserted
 */


            Console.WriteLine("Select the category:");
            Console.WriteLine("1. Work");
            Console.WriteLine("2. Personal");
            string categoryChoice = Console.ReadLine()?.Trim() ?? "";

            string category = categoryChoice switch
            {
                "1" => "Work",
                "2" => "Personal",
                _ => "SideQuests"
            };

            Console.WriteLine("Select the priority:");
            Console.WriteLine("1. High");
            Console.WriteLine("2. Medium");
            Console.WriteLine("3. Low");
            string priorityChoice = Console.ReadLine()?.Trim() ?? "";

            string priority = priorityChoice switch
            {
                "1" => "High",
                "2" => "Medium",
                "3" => "Low",
                _ => "Unspecified"
            };

            Console.Write("Enter the Deadline (yyyy-MM-dd): \n");
            DateTime deadline;
            while (!DateTime.TryParse(Console.ReadLine(), out deadline) || deadline < DateTime.Now)
            {
                Console.WriteLine("Invalid date or date has expired. Enter the deadline in this format please (yyyy-MM-dd): ");
            }

            Task newTask = new Task(description, category, priority, deadline);
            Tasks.Add(newTask);
            Console.WriteLine("Task added successfully!");
        }

        public void ViewTasks()
        {
            if (Tasks.Count == 0)
            {
                Console.WriteLine("No tasks found\n");
                return;
            }

            Console.WriteLine("Tasks:");
            DisplayTasks(Tasks);

            while (true)
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Filter Tasks");
                Console.WriteLine("2. Sort Tasks");
                Console.WriteLine("3. Delete a Task");
                Console.WriteLine("4. Update a Task");
                Console.WriteLine("5. Go back");

                string option = Console.ReadLine()?.Trim() ?? "";

                switch (option)
                {
                    case "1":
                        FilterTasks();
                        break;
                    case "2":
                        SortTasks();
                        break;
                    case "3":
                        DeleteTask();
                        break;
                    case "4":
                        UpdateTask();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void DisplayTasks(List<Task> tasks)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i]}");
            }
            Console.WriteLine();
        }

        private void FilterTasks()
        {
            Console.WriteLine("Filter By:");
            Console.WriteLine("1. Category");
            Console.WriteLine("2. Priority");
            string filterChoice = Console.ReadLine()?.Trim() ?? "";

            List<Task> filteredTasks;

            switch (filterChoice)
            {
                case "1":
                    Console.Write("Enter Category (Work, Personal, SideQuests): ");
                    string category = Console.ReadLine()?.Trim() ?? "";
                    filteredTasks = FilterByCategory(category);
                    break;
                case "2":
                    Console.Write("Enter Priority (High, Medium, Low, Unspecified): ");
                    string priority = Console.ReadLine()?.Trim() ?? "";
                    filteredTasks = FilterByPriority(priority);
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    return;
            }
            Console.WriteLine("Filtered Tasks:");
            DisplayTasks(filteredTasks);
        }

        private void SortTasks()
        {
            Console.WriteLine("Sort by:");
            Console.WriteLine("1. Deadline (Ascending)");
            Console.WriteLine("2. Deadline (Descending)");
            Console.WriteLine("3. Priority");
            string sortChoice = Console.ReadLine()?.Trim();

            List<Task> sortedTasks;

            switch (sortChoice)
            {
                case "1":
                    sortedTasks = SortByDeadline(true);
                    break;
                case "2":
                    sortedTasks = SortByDeadline(false);
                    break;
                case "3":
                    sortedTasks = SortByPriority();
                    break;
                default:
                    Console.WriteLine("Invalid sort option. Please try again.");
                    return;
            }

            Console.WriteLine("Sorted Tasks:");
            DisplayTasks(sortedTasks);
        }

        public List<Task> FilterByCategory(string category)
        {
            return Tasks.Where(task => task.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Task> FilterByPriority(string priority)
        {
            return Tasks.Where(task => task.Priority.Equals(priority, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Task> SortByDeadline(bool ascending = true)
        {
            return ascending ? Tasks.OrderBy(task => task.Deadline).ToList() : Tasks.OrderByDescending(task => task.Deadline).ToList();
        }

        public List<Task> SortByPriority()
        {
            return Tasks.OrderBy(task => task.Priority).ToList();
        }

        public void DeleteTask()
        {
            Console.Write("Enter the task number you would like to delete: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumberToDelete) && taskNumberToDelete > 0 && taskNumberToDelete <= Tasks.Count)
            {
                Tasks.RemoveAt(taskNumberToDelete - 1);
                Console.WriteLine("Task deleted successfully!\n");
            }
            else
            {
                Console.WriteLine("Invalid task number. Please try again.\n");
            }
        }

        public void UpdateTask()
        {
            Console.Write("Enter the number of the task you want to update: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumberToUpdate) && taskNumberToUpdate > 0 && taskNumberToUpdate <= Tasks.Count)
            {
                Task taskToUpdate = Tasks[taskNumberToUpdate - 1];
                Console.Write("Enter the new description: ");
                taskToUpdate.Description = Console.ReadLine()?.Trim() ?? "";

                Console.WriteLine("Select the new category:");
                Console.WriteLine("1. Work");
                Console.WriteLine("2. Personal");

                string categoryChoice = Console.ReadLine()?.Trim() ?? "";
                taskToUpdate.Category = categoryChoice switch
                {
                    "1" => "Work",
                    "2" => "Personal",
                    _ => taskToUpdate.Category
                };

                Console.WriteLine("Select the priority:");
                Console.WriteLine("1. High");
                Console.WriteLine("2. Medium");
                Console.WriteLine("3. Low");

                string priorityChoice = Console.ReadLine()?.Trim() ?? "";
                taskToUpdate.Priority = priorityChoice switch
                {
                    "1" => "High",
                    "2" => "Medium",
                    "3" => "Low",
                    _ => taskToUpdate.Priority
                };

                Console.WriteLine("Please enter the new deadline (yyyy-MM-dd): ");
                DateTime deadline;
                while (!DateTime.TryParse(Console.ReadLine(), out deadline) || deadline < DateTime.Now)
                {
                    Console.WriteLine("Invalid or expired date used. Please enter a valid date (yyyy-MM-dd): ");
                }
                taskToUpdate.Deadline = deadline;

                Console.WriteLine("Task Updated Successfully!\n");
            }
            else
            {
                Console.WriteLine("Invalid task number. Please try again.\n");
            }
        }
    }

    public static class FileManager
    {
        public static List<Task> LoadTasksFromFile(string filename)
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
                        if (parts.Length == 4 && DateTime.TryParse(parts[3], out DateTime deadline))
                        {
                            tasks.Add(new Task(parts[0], parts[1], parts[2], deadline));
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

        public static void SaveTasksToFile(string filename, List<Task> tasks)
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (var task in tasks)
                {
                    lines.Add($"{task.Description}|{task.Category}|{task.Priority}|{task.Deadline:yyyy-MM-dd}");
                }
                File.WriteAllLines(filename, lines);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error saving tasks: {ex.Message}");
            }
        }

        public static List<User> LoadUsersFromFile(string filename)
        {
            List<User> users = new List<User>();
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
                            users.Add(new User(parts[0], parts[1]));
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error loading users: {ex.Message}");
            }
            return users;
        }

        public static void SaveUsersToFile(string filename, List<User> users)
        {
            try
            {
                List<string> lines = new List<string>();
                foreach (var user in users)
                {
                    lines.Add($"{user.Username}|{user.Password}");
                }
                File.WriteAllLines(filename, lines);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error saving users: {ex.Message}");
            }
        }
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }

    public static class UserAuth
    {
        private static List<User> Users = FileManager.LoadUsersFromFile("Users.txt");

        public static bool Authenticate(string inputUsername, string inputPassword)
        {
            return Users.Any(user => user.Username == inputUsername && user.Password == inputPassword);
        }

        public static bool SignUp(string newUsername, string newPassword)
        {
            if (Users.Any(user => user.Username == newUsername))
            {
                return false;
            }

            Users.Add(new User(newUsername, newPassword));
            FileManager.SaveUsersToFile("Users.txt", Users);
            return true;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Entry point of the program
            TaskManager taskManager = new TaskManager();

            Console.WriteLine("Welcome to my To-Do List app");

            bool authenticated = false;

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
                        authenticated = Login();
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

            taskManager.LoadTasks(FileManager.LoadTasksFromFile("tasks.txt"));

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
                        FileManager.SaveTasksToFile("tasks.txt", taskManager.Tasks);
                        Console.WriteLine("Exiting the To-Do List App. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.\n");
                        break;
                }
            }
        }

        static bool Login()
        {
            // Method to login a user
            Console.Write("Enter Username: ");
            string username = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Enter Password: ");
            string password = Console.ReadLine()?.Trim() ?? "";

            return UserAuth.Authenticate(username, password);
        }

        static void SignUp()
        {
            // Method to sign up a new user
            Console.Write("Enter new Username: ");
            string username = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Enter new Password: ");
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
