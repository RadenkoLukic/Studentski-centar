using MaterialDesignThemes.Wpf;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static LoginForm.DatabaseConnection;
using static MaterialDesignThemes.Wpf.Theme;


namespace LoginForm
{
    /// <summary>
    /// Interaction logic for CustomizeProfile_Popup.xaml
    /// </summary>
    public partial class CustomizeProfile_Popup : UserControl
    {

        public CustomizeProfile_Popup()
        {
            InitializeComponent();
            //this.DataContext = new Admin();  // Prazan model da spreči greške pre učitavanja podataka
        }

        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            if (parentWindow is DashboardWindow mainWindow)
            {
                mainWindow.ClosePopup();
            }
        }

        // Postavi podatke za admina u popup
        public void SetAdminData(Admin admin)
        {
            //MessageBox.Show($"Ime: {admin.Ime}, Prezime: {admin.Prezime}, Email: {admin.Email}"); // Provera podataka

            // Podesi DataContext da prikaže podatke o adminu
            this.DataContext = admin;

            //CustomizeProfilePopup.IsOpen = true;
        }
        public void SetZaposleniData(Zaposleni zaposleni)
        {
            //MessageBox.Show($"Ime: {zaposleni.Ime}, Prezime: {zaposleni.Prezime}, Email: {zaposleni.Email}"); // Provera podataka
            this.DataContext = zaposleni;
            Border.Height = 690;
            SaveButton.Margin = new Thickness(88, 595, 88, 0);

            DatumZaposlenja.Visibility = Visibility.Visible;
            Paviljon.Visibility = Visibility.Visible;


        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(passwordBox.Password) && passwordBox.Password.Length > 0)
            {
                //textPassword.Visibility = Visibility.Collapsed;
                textBorder.Visibility = Visibility.Collapsed;
                textPassword.Text = passwordBox.Password;
                passwordBox.Visibility = Visibility.Visible;
            }
            else
            {
                passwordBox.Password = null;
                textPassword.Text = "Nova Lozinka";
                textBorder.Visibility = Visibility.Visible;
                textPassword.Visibility = Visibility.Visible;
                passwordBox.Visibility = Visibility.Collapsed;

                //passwordBox.Visibility = Visibility.Collapsed;
            }


        }
        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //textBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(52, 73, 94)); // Plava ivica
            //textBorder.BorderThickness = new Thickness(2);
            //passwordBox.Focus();
            if (!string.IsNullOrEmpty(passwordBox.Password) && passwordBox.Password.Length > 0)
            {
                //textPassword.Text = passwordBox.Password;
                textPassword.Visibility = Visibility.Collapsed;
                textBorder.Visibility = Visibility.Collapsed;
                passwordBox.Visibility = Visibility.Visible;
                passwordBox.Focus();
            }
            else
            {
                
                textBorder.Visibility = Visibility.Collapsed;
                textPassword.Text = "Nova Lozinka";
                passwordBox.Password = null;
                textPassword.Visibility = Visibility.Collapsed;
                passwordBox.Visibility = Visibility.Visible;
                passwordBox.Focus();
            }
                

            //passwordBox.Visibility = Visibility.Visible;
            //MessageBox.Show($"Trenutna lozinka: {passwordBox.Password}", "Lozinka", MessageBoxButton.OK, MessageBoxImage.Information);

        }


        private void CheckBox_Show(object sender, RoutedEventArgs e)
        {
            if (CheckBox.IsChecked == true)
            {
                // Prikaži lozinku u TextBox-u
                textPassword.Text = passwordBox.Password;
                textBorder.Visibility = Visibility.Visible;
                textPassword.Visibility = Visibility.Visible;
                passwordBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Vrati na PasswordBox
                CheckBox.IsChecked = false;
                
                
                if (!string.IsNullOrEmpty(passwordBox.Password) && passwordBox.Password.Length > 0)
                {
                    passwordBox.Password = textPassword.Text;
                    passwordBox.Visibility = Visibility.Visible;
                    textPassword.Visibility = Visibility.Collapsed;
                    textBorder.Visibility = Visibility.Collapsed;
                }
                else
                {
                    textPassword.Text = "Nova Lozinka";
                    passwordBox.Visibility = Visibility.Collapsed;
                    textPassword.Visibility = Visibility.Visible;
                    textBorder.Visibility = Visibility.Visible;

                }
            }
        }
        private void CheckBox_LostFocus(object sender, RoutedEventArgs e)
        {
            CheckBox.IsChecked = false; // Kada izgubi fokus, poništi izbor
        }
        private void passwordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            passwordBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(52, 152, 219)); // Plava ivica
            passwordBorder.BorderThickness = new Thickness(3);
            if (!string.IsNullOrEmpty(passwordBox.Password) && passwordBox.Password.Length > 0)
            {
                // Vrati na PasswordBox
                CheckBox.IsChecked = false;
                passwordBox.Focus();
                //passwordBox.Password = textPassword.Text;

                passwordBox.Visibility = Visibility.Visible;
                textPassword.Visibility = Visibility.Collapsed;
                textBorder.Visibility = Visibility.Collapsed;
            }
            else
            {
                CheckBox.IsChecked = false;
                passwordBox.Focus();
                textPassword.Text = "Nova lozinka";
                //passwordBox.Password = null;
                passwordBox.Visibility = Visibility.Visible;
                textPassword.Visibility = Visibility.Collapsed;
                textBorder.Visibility = Visibility.Collapsed;
            }
            // Pomeri kursor na kraj lozinke
            //passwordBox.Select(passwordBox.Password.Length, 0);
        
            //textPassword.Text = "Nova Lozinka";
            //textPassword.Visibility = Visibility.Visible;
        }

        private void passwordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            passwordBorder.BorderBrush = new SolidColorBrush(Colors.Gray); // Vraća sivu ivicu
            passwordBorder.BorderThickness = new Thickness(1);
            // Ako korisnik nije uneo lozinku, prikaži "Nova Lozinka"
            if (string.IsNullOrEmpty(passwordBox.Password) && passwordBox.Password.Length == 0)
            {
                //passwordBox.Password = textPassword.Text;

                textPassword.Text = "Nova Lozinka";
                textPassword.Visibility = Visibility.Visible;
                textBorder.Visibility = Visibility.Visible;
                passwordBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                textBorder.Visibility = Visibility.Collapsed;
                textPassword.Visibility = Visibility.Collapsed;
                passwordBox.Visibility = Visibility.Visible;
            }
            //textPassword.Text = "Nova Lozinka";
            //textPassword.Visibility = Visibility.Visible;
        }
        private string userRole; // Dodajemo promenljivu za ulogu korisnika
        public void SetUserRole(string role)
        {
            userRole = role; // Postavljamo vrednost uloge
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Proveri koja je korisnička uloga
            if (userRole == "Admin")
            {
                SaveAdminChanges();
            }
            else if (userRole == "Zaposleni")
            {
                SaveEmployeeChanges();
            }
        }
        private void SaveAdminChanges()
        {
            // Prikaz poruke za potvrdu
            MessageBoxResult result = MessageBox.Show(
                "Da li ste sigurni da želite sačuvati izmene?",
                "Potvrda izmene",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );
            // Ako korisnik odabere "No", prekidamo izvršavanje metode
            if (result == MessageBoxResult.No)
            {
                return;
            }
            // Kreiranje instance Admin objekta i popunjavanje podacima iz TextBox-ova
            Admin admin = new Admin()
            {
                OsobaID = int.TryParse(osobaIDBox.Text, out int osobaId) ? osobaId : 0,
                Email = emailBox.Text,

                Ime = imeBox.Text,
                Prezime = prezimeBox.Text,
                Username = usernameBox.Text,
                BrojTelefona = brojtelefonaBox.Text,
                DatumRodjenja = DateTime.TryParse(datumrodjenjaBox.Text, out DateTime datumRodjenja) ? datumRodjenja : DateTime.MinValue,
                AdresaStanovanja = adresastanovanjaBox.Text,
                JMBG = jmbgBox.Text,
                Zvanje = zvanjeBox.Text,
                Tema = temaBox.Text,
                Jezik = jezikBox.Text
            };

            // Ažuriraj lozinku samo ako je korisnik uneo novu
            if (!string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                admin.Sifra = passwordBox.Password;
            }
            DatabaseConnection db = new DatabaseConnection();
            db.UpdateAdmin(admin);
            MessageBox.Show("Izmene su uspešno sačuvane!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);

        }
        private void SaveEmployeeChanges()
        {
            // Prikaz poruke za potvrdu
            MessageBoxResult result = MessageBox.Show(
                "Da li ste sigurni da želite sačuvati izmene?",
                "Potvrda izmene",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            // Ako korisnik odabere "No", prekidamo izvršavanje metode
            if (result == MessageBoxResult.No)
            {
                return;
            }
            Zaposleni zaposleni = new Zaposleni
            {
                OsobaID = int.TryParse(osobaIDBox.Text, out int osobaId) ? osobaId : 0,
                Email = emailBox.Text,

                Ime = imeBox.Text,
                Prezime = prezimeBox.Text,
                Username = usernameBox.Text,
                BrojTelefona = brojtelefonaBox.Text,
                DatumRodjenja = DateTime.TryParse(datumrodjenjaBox.Text, out DateTime datumRodjenja) ? datumRodjenja : DateTime.MinValue,
                AdresaStanovanja = adresastanovanjaBox.Text,
                JMBG = jmbgBox.Text,
                Zvanje = zvanjeBox.Text,
                Tema = temaBox.Text,
                Jezik = jezikBox.Text,
                Paviljon = paviljonBox.Text,
                DatumZaposlenja = DateTime.TryParse(datumzaposlenjaBox.Text, out DateTime datumZaposlenja) ? datumZaposlenja : DateTime.MinValue,


            };
            // Ažuriraj lozinku samo ako je korisnik uneo novu
            if (!string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                zaposleni.Sifra = passwordBox.Password;
            }
            DatabaseConnection db = new DatabaseConnection();
            db.UpdateZaposleni(zaposleni);
            MessageBox.Show("Izmene su uspešno sačuvane!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
            //this.Visibility = Visibility.Visible;
        }

    }
}
