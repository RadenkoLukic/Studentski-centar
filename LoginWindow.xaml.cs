using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Data;
using static MaterialDesignThemes.Wpf.Theme;

namespace LoginForm
{
    public partial class LoginWindow : Window
    {
        private DashboardWindow _dashboardWindow;
        private DatabaseConnection dbConnection;
        private bool jezikSrpski = true;
        

        public LoginWindow(DashboardWindow dashboardWindow, bool jezikSrpski, string tema)
        {
            InitializeComponent();
            dbConnection = new DatabaseConnection();
            
            _dashboardWindow = dashboardWindow;

            this.jezikSrpski = jezikSrpski;

            LoginWindowLanguage();
            ApplyTheme(tema);
        }

        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Sakrij placeholder tekst kada korisnik klikne
            TextBox.Visibility = Visibility.Collapsed;
            PasswordBox.Visibility = Visibility.Visible;
            PasswordBox.Focus();

            // Očisti placeholder tekst ako je još uvek prikazan
            if (TextBox.Text == "Lozinka")
            {
                TextBox.Text = "";
            }
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Kada TextBox dobije fokus (ako je korisnik tab-ovao u njega)
            TextBox.Text = "";
            TextBox.Visibility = Visibility.Collapsed;
            PasswordBox.Visibility = Visibility.Visible;
            PasswordBox.Focus();
        }

        private void passwordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Kada PasswordBox dobije fokus
            if (PasswordBox.Password == "")
            {
                TextBox.Visibility = Visibility.Collapsed;
            }
        }

        private void passwordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Kada PasswordBox izgubi fokus
            if (string.IsNullOrEmpty(PasswordBox.Password))
            {
                PasswordBox.Visibility = Visibility.Collapsed;
                TextBox.Text = "Lozinka"; // Vrati placeholder tekst
                LoginWindowLanguage();
                TextBox.Visibility = Visibility.Visible;
            }
        }

        private void CheckBox_Show(object sender, RoutedEventArgs e)
        {
            if (CheckBox.IsChecked == true)
            {
                // Prikaži lozinku
                TextBox.Text = PasswordBox.Password;
                TextBox.Visibility = Visibility.Visible;
                PasswordBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Sakrij lozinku
                if (string.IsNullOrEmpty(PasswordBox.Password))
                {
                    // Ako je prazno, vrati placeholder
                    TextBox.Text = "Lozinka";
                    LoginWindowLanguage();
                }
                TextBox.Visibility = Visibility.Collapsed;
                PasswordBox.Visibility = Visibility.Visible;
            }
        }

        private void CheckBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Kada CheckBox izgubi fokus, proveri da li treba sakriti lozinku
            if (CheckBox.IsChecked == true && !PasswordBox.IsVisible)
            {
                CheckBox.IsChecked = false;
                //LoginWindowLanguage();
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Ažuriraj tekst u TextBox-u ako je prikazana lozinka
            if (CheckBox.IsChecked == true)
            {
                TextBox.Text = PasswordBox.Password;
            }
        }


        private void LoginWindowLanguage()
        {
            Login_TBlock.Text = jezikSrpski ? "Prijava" : "Login";
            TextBox.Text = jezikSrpski ? "Lozinka" : "Password";
            LoginButton.Content = jezikSrpski ? "Prijavi se" : "Log in";
        }

        private void ApplyTheme(string themeName)
        {
            if (themeName == "Zelena")
            {
                Shape1.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                LoginUserInput.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                TextBox.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                PasswordBox.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                LoginButton.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                Login_TBlock.Foreground = new SolidColorBrush(Color.FromRgb(0, 62, 41));
            }
        }

        // Zatvaranje prozora
        private void Button_Close(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            //this.Effect = null;
            //BackgroundMask.Visibility = Visibility.Collapsed;
        }

      
        // Handle the LoginButton Click
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string userInput = LoginUserInput.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(userInput) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    jezikSrpski ? "Unesite email i lozinku!" : "Please enter email and password!",
                    jezikSrpski ? "Greška" : "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                    );

                return;
            }
            
            DatabaseConnection dbConnection = new DatabaseConnection();
            string hashedPassword = dbConnection.HashPassword(password);
            

            string userRole = dbConnection.CheckUserCredentials(userInput, hashedPassword);
            


            if (userRole == "Admin" || userRole == "Zaposleni")
            {
                
                    //MessageBox.Show("Uspešno ste se prijavili!", "Prijava", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Pronalazimo i zatvaramo početni prozor
                    foreach (Window window in Application.Current.Windows)
                    {
                        if (window is DashboardWindow) 
                        {
                            window.Close();
                            break;
                        }
                    }

                    // 🔹 Otvaramo Dashboard i prosleđujemo ulogu korisnika
                    DashboardWindow dashboard = new DashboardWindow(userRole, userInput);
                    dashboard.Show();

                    this.Close(); // Zatvaramo Login prozor
                
            }
            else
            {
                MessageBox.Show(
                    jezikSrpski ? "Pogrešan email ili lozinka!" : "Incorrect email or password!",
                    jezikSrpski ? "Prijava neuspješna" : "Login failed",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );

            }
        }


        private void RegisterAdminButton_Click(object sender, RoutedEventArgs e)
        {
            string email = LoginUserInput.Text;
            string password = PasswordBox.Password;     // Uzmite lozinku sa korisničkog unosa

            DatabaseConnection dbConnection = new DatabaseConnection();

            // Pozovi metodu za unos admina
            dbConnection.InsertAdmin(email, password);

            MessageBox.Show(
                jezikSrpski ? "Admin je uspješno registrovan!" : "Admin registered successfully!",
                jezikSrpski ? "Uspjeh" : "Success",
                MessageBoxButton.OK,
                MessageBoxImage.Information
                );

        }

        
        private bool ValidateLogin(string email, string password)
        {
            // Example validation
            return !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password) && email.Contains("@");
        }

        private void LoginUserInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
