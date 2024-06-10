using System;
using System.Windows;
using System.Windows.Controls;

namespace AddTask
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void AddTask(object sender, RoutedEventArgs e)
        {
            string description = taskNameTextBox.Text;
            string category = ((ComboBoxItem)categoryComboBox.SelectedItem)?.Content.ToString();
            string priority = ((ComboBoxItem)priorityComboBox.SelectedItem)?.Content.ToString();
            DateTime? deadline = taskCalendar.SelectedDate;

            // Validate input
            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please enter a task description");
                return;
            }
            if (category == null)
            {
                MessageBox.Show("Please select a category");
                return;
            }
            if (priority == null)
            {
                MessageBox.Show("Please select a priority");
                return;
            }
            if (deadline == null)
            {
                MessageBox.Show("Please select a deadline");
                return;
            }

            Task newTask = new Task(description, category, priority, deadline.Value);
            Tasks.Add(newTask);
            MessageBox.Show("Task added successfully");
        }
    }

    // Assuming you have a Task class and a Tasks collection somewhere in your project
    public class Task
    {
        public string Description { get; set; }
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
    }

    public static class Tasks
    {
        public static List<Task> TaskList { get; } = new List<Task>();

        public static void Add(Task task)
        {
            TaskList.Add(task);
        }
    }
}
