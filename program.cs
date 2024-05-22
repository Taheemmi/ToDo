using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace ToDoList
{
    public class Task
    {
        public string Description { get; set; } // get is to return property value and set is a new value
        public string Category { get; set; }
        public string Priority { get; set; }
        public DateTime Deadline { get; set; }

        public Task(string description, string category, string priority, DateTime deadline)
        {
            Description = description;
            Category = category;
            Priority = priority;
            Deadline = deadline;
        }

        public int DaysRemaining()
        {
            return (Deadline - DateTime.Now).Days;
        }

        public override string ToString()
        {
            return $"{Description} ({Category}) - Priority: {Priority}, Days remaining: {DaysRemaining()}";
        }
    }

    public static class userAuth //  login credentials
    {
        private const string Username = "user"; // this is the username to login
        private const string Password = "root"; // this is the password

        public static bool Authenticate(string inputUsername, string inputPassword) // checks if the username and password are correct
        {
            return inputUsername.Trim() == Username && inputPassword.Trim() == Password; 
        }
    }

    public class TaskManager
    {
        public List<Task> Tasks { get; private set; } = new List<Task>();

        public void LoadTasks(List<Task> tasks) 
        { 
            Tasks = tasks;
        }

        public void AddTask()
        {
            Console.Write("Enter the task: ");
            string description = Console.ReadLine()?.Trim() ?? ""; // Use null-coalescing operator

            Console.WriteLine("Select the category: ");
            Console.WriteLine("1. Work");
            Console.WriteLine("2. Personal");
            string categoryChoice = Console.ReadLine()?.Trim() ?? "";

            string category = categoryChoice switch
            {
                "1" => "Work",
                "2" => "Personal",
                _ => "SideQuests"
            };

            Console.WriteLine("Select the priority: ");
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
            Console.WriteLine("Task connected sucessfully!");
        }

        public void ViewTasks()
        {
            if (Tasks.Count == 0) // if there are no tasks detected then this will display
            {
                Console.WriteLine("No tasks found\n");
                return;
            }

            Console.WriteLine("Tasks:");
            for (int i = 0; i < Tasks.Count; i++) // tasks present 
            {
                Console.WriteLine($"{i + 1}. {Tasks[i]}");
            }
            Console.WriteLine();

            Console.WriteLine("Select an option: ");
            Console.WriteLine("1. Delete a Task");
            Console.WriteLine("2. Update a Task");
            Console.WriteLine("3. Go back");

            string option = Console.ReadLine()?.Trim() ?? ""; // reads user input and assigns to the number at the starts

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
        public void DeleteTask()
        {
            Console.WriteLine("Enter the task number you would like to delete: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumberToDelete) && taskNumberToDelete > 0 && taskNumberToDelete <= Tasks.Count)  // this string reads the users input and convert the string to an interger. If successful the method is set to 1 for true and if not it is set to 0. Then checks if larger than 0.
            {
                Tasks.RemoveAt(taskNumberToDelete - 1);
                Console.WriteLine("Task deleted successfully!\n");
            }
            else
            {
                Console.WriteLine("Invalid task number. Please try again. \n");
            }
        }

        // update description and category of task
        public void UpdateTask()
        {
            Console.Write("Enter the number of the task you want to update: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumberToUpdate) && taskNumberToUpdate > 0 && taskNumberToUpdate <= Tasks.Count)
            {
                Task taskToUpdate = Tasks[taskNumberToUpdate - 1];
                Console.Write("Enter the new description: ");
                taskToUpdate.Description = Console.ReadLine()?.Trim() ?? "";


                Console.WriteLine("Select the new category: ");
                Console.WriteLine("1. Work");
                Console.WriteLine("2. Personal");

                string categoryChoice = Console.ReadLine()?.Trim() ?? "";

                taskToUpdate.Category = categoryChoice switch
                {
                    "1" => "Work",
                    "2" => "Personal",
                    _ => taskToUpdate.Category
                };

                Console.WriteLine("Select the priority: ");
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
                    Console.WriteLine("Invalid or expired date used. Please enter a valid date (yyyy-MM-DD): ");
                }
                taskToUpdate.Deadline = deadline;

                Console.WriteLine("Task Updated Successfully!\n");
            }
            else
            {
                Console.WriteLine("Computer says nooo\n"); // if something unexpected happpens
            }
        }
    }
        
    public static class FileManager
    {
        // load tasks from file including categories
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

        // save tasks to file, including categories
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
        }
        class Program
        {
            static void Main(string[] args)
        {
            TaskManager taskManager = new TaskManager();

            Console.WriteLine("Login"); // user login

            Console.Write("Enter Username: \n");
            string username = Console.ReadLine()?.Trim(); // trim whitespace from input

            Console.Write("Enter Password: \n");
            string password = Console.ReadLine()?.Trim(); // trim whitespace from input

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

            taskManager.LoadTasks(FileManager.LoadTasksFromFile("tasks.txt")); // load tasks from file

            Console.WriteLine("Welcome to the To-Do list app \n ");

            while (true)
            { 
                Console.WriteLine("Please select a option: ");
                Console.WriteLine("1. Add Task");
                Console.WriteLine("2. View Tasks");
                Console.WriteLine("3. Exit");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        taskManager.AddTask();
                        break;
                    case "2":
                        taskManager.ViewTasks();
                        break;
                    case "3":
                        FileManager.SaveTasksToFile("tasks.txt", taskManager.Tasks);
                        Console.WriteLine("Exiting the To-Do List App. G'bye");
                        return;
                        break;

                    default:
                        Console.WriteLine("Invalid option. please try again. \n");
                        break;
                    
                }
            }
        }
    }
}
