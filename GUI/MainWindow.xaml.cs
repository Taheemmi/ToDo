using System.Windows.Controls;
using System.Windows;
using ToDoListApp;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace ToDoListApp
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<TodoItem> _todoList = new ObservableCollection<TodoItem>();

        public MainWindow()
        {
            InitializeComponent();
            taskList.ItemsSource = _todoList;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            string newTodoText = newTodoTextBox.Text.Trim();
            if (newTodoText.Length > 0)
            {
                TodoItem newTodoItem = new TodoItem { Text = newTodoText };
                _todoList.Add(newTodoItem);
                newTodoTextBox.Clear();
            }
        }
    }

    public class TodoItem
    {
        public string Text { get; set; }
    }
}

 
