using System.Windows;

namespace LoginSignup
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = LoginUsername.Text;
            string password = LoginPassword.Password;
            
            if (User.Authenticate(username, password))
            {
                MessageBox.Show("Login Succesfull!");
                // nav to MainMenu
            }
            else
            {
                MessageBox.Show("Invalid Username or Password");
            }
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            string username = SignupUsername.Text;
            string password = SignupPassword.Password;
            string confirmPassword = ConfirmPassword.Password;

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            // Add logic for handling signup
            MessageBox.Show($"Signup Successful!");
        }

        private void LoginUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }

    public static class User
    {
        private const string Username = "user";
        private const string Password = "root";

        public static bool Authenticate(string inputUsername, string inputPassword)
        {
            return inputUsername.Trim() == inputUsername && inputPassword.Trim() == Password;
        }
    }
}
