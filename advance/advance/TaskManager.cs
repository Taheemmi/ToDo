using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Task
{
    public string Description { get; set; }
    public string Category { get; set; }
    public string Priority { get; set; }
    public DateTime Deadline { get; set; }
    public string Username { get; set; }

    public Task(string description, string category, string priority, DateTime deadline, string username)
    {
        Description = description;
        Category = category;
        Priority = priority;
        Deadline = deadline;
        Username = username;
    }

    public int DaysRemaining()
    {
        return (int)(Deadline - DateTime.Now).TotalDays;
    }

    public override string ToString()
    {
        return $"{Description} ({Category}) - Priority: {Priority}, Days remaining: {DaysRemaining()}, Created by: {Username}";
    }
}

public class TaskManager
{
    public List<Task> Tasks = new List<Task>();
    public List<User> users = new List<User>();
    public string CurrentUser { get; set; }

    public void LoadTasksFromFile(string filePath)
    {
        Tasks = new List<Task>();

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length == 5)
                {
                    string description = parts[0];
                    string category = parts[1];
                    string priority = parts[2];
                    DateTime deadline = DateTime.ParseExact(parts[3], "yyyy-MM-dd", null);
                    string username = parts[4];
                    Task task = new Task(description, category, priority, deadline, username);
                    Tasks.Add(task);
                }
            }
        }
    }

    public static void SaveTasksToFile(string filename, List<Task> tasks)
    {
        List<string> lines = new List<string>();    
        foreach (var task in tasks)
        {
            lines.Add($"{task.Description}|{task.Category}|{task.Priority}|{task.Deadline:yyyy-MM-dd}|{task.Username}");
        }
        File.WriteAllLines(filename, lines);
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

        Console.Write("Enter the Deadline (yyyy-MM-dd): "); // function to add date at the end of the task and calculate the days remaingin
        DateTime deadline;
        while (!DateTime.TryParse(Console.ReadLine(), out deadline) || deadline < DateTime.Now)
        {
            Console.WriteLine("Invalid date or date has expired. Enter the deadline in this format please (yyyy-MM-dd): ");
        }

        Task newTask = new Task(description, category, priority, deadline, CurrentUser);
        Tasks.Add(newTask);
        Console.WriteLine("Task added successfully!");
    }

    public void ViewTasks()
    {
        var userTasks = Tasks.Where(t => t.Username == CurrentUser).ToList();

        if (userTasks.Count == 0)
        {
            Console.WriteLine("No tasks found\n");
            return;
        }

        Console.WriteLine("Tasks:");
        DisplayTasks(userTasks);

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
        string sortChoice = Console.ReadLine()?.Trim() ?? "";

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
        return Tasks.Where(task => task.Category.Equals(category, StringComparison.OrdinalIgnoreCase) && task.Username == CurrentUser).ToList();
    }

    public List<Task> FilterByPriority(string priority)
    {
        return Tasks.Where(task => task.Priority.Equals(priority, StringComparison.OrdinalIgnoreCase) && task.Username == CurrentUser).ToList();
    }

    public List<Task> SortByDeadline(bool ascending = true)
    {
        return ascending ? Tasks.Where(task => task.Username == CurrentUser).OrderBy(task => task.Deadline).ToList() : Tasks.Where(task => task.Username == CurrentUser).OrderByDescending(task => task.Deadline).ToList();
    }

    public List<Task> SortByPriority()
    {
        return Tasks.Where(task => task.Username == CurrentUser).OrderBy(task => task.Priority).ToList();
    }

    public void DeleteTask()
    {
        Console.Write("Enter the task number you would like to delete: ");
        var userTasks = Tasks.Where(t => t.Username == CurrentUser).ToList();

        if (int.TryParse(Console.ReadLine(), out int taskNumberToDelete) && taskNumberToDelete > 0 && taskNumberToDelete <= userTasks.Count)
        {
            Tasks.Remove(userTasks[taskNumberToDelete - 1]);
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
        var userTasks = Tasks.Where(t => t.Username == CurrentUser).ToList();
        if (int.TryParse(Console.ReadLine(), out int taskNumberToUpdate) && taskNumberToUpdate > 0 && taskNumberToUpdate <= userTasks.Count)
        {
            Task taskToUpdate = userTasks[taskNumberToUpdate - 1];
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

    public static List<User> LoadUsersFromFile(string filename)
    {
        List<User> users = new List<User>();
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
        return users; // return an empty list of the file does not exist
    }

    public static void SaveUsersToFile(string filename, List<User> users)
    {
        List<string> lines = new List<string>();
        foreach (var user in users)
        {
            lines.Add($"{user.Username}|{user.Password}");
        }
        File.WriteAllLines(filename, lines);
    }
}
