using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static LoginForm.DatabaseConnection;

namespace LoginForm
{
    /// <summary>
    /// Interaction logic for UpravljajZaposlenimaView.xaml
    /// </summary>
    public partial class UpravljajZaposlenimaView : UserControl
    {
        private bool isEditingEnabled = false;
        private int editableRowIndex = -1; // Podrazumevano nijedan red nije za uređivanje

        private bool _jezikSrpski = true;
        public ObservableCollection<Zaposleni> ZaposleniList { get; set; }
        public UpravljajZaposlenimaView(bool jezikSrpski = true)
        {
            InitializeComponent();
            BtnDodajZaposlenog.Visibility = Visibility.Visible;
            RefreshZaposleniList();
            ZaposleniDataGrid.Visibility = Visibility.Visible;

            _jezikSrpski = jezikSrpski;
            LoadZaposleniData();
        }

        private void DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is DatePicker dp)
            {
                var textBox = dp.Template.FindName("PART_TextBox", dp) as DatePickerTextBox;
                if (textBox != null)
                {
                    var watermark = textBox.Template.FindName("PART_Watermark", textBox) as ContentControl;
                    if (watermark != null)
                    {
                        watermark.Content = " dd/mm/yyyy";
                    }
                }
            }
        }
        private void PART_Button_Click(object sender, RoutedEventArgs e)
        {
            // Dohvatiti ComboBox koji je u šablonu
            var comboBox = (ComboBox)((Button)sender).TemplatedParent;

            // Prebacivanje IsDropDownOpen vrednosti
            comboBox.IsDropDownOpen = !comboBox.IsDropDownOpen;
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
                textPassword.Text = _jezikSrpski ? "Nova lozinka" : "New password";
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
                textPassword.Text = _jezikSrpski ? "Nova lozinka" : "New password";
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
                    textPassword.Text = _jezikSrpski ? "Nova lozinka" : "New password";
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
                textPassword.Text = _jezikSrpski ? "Nova lozinka" : "New password";
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

                textPassword.Text = _jezikSrpski ? "Nova lozinka" : "New password";
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

        private void BtnDodajZaposlenog_Click(object sender, RoutedEventArgs e)
        {
            // Otvori popup za unos podataka
            DodajZaposlenogPopup.IsOpen = true;
            this.Effect = new BlurEffect { Radius = 10 };
        }
        private void PotvrdiButton_Click(object sender, RoutedEventArgs e)
        {
            // Dobijanje unetog zaposlenog iz poslednjeg reda tabele
            if (ZaposleniDataGrid.Items.Count > 0 && ZaposleniDataGrid.Items[ZaposleniDataGrid.Items.Count - 1] is Zaposleni noviZaposleni)
            {
                if (string.IsNullOrWhiteSpace(noviZaposleni.Ime) ||
                    string.IsNullOrWhiteSpace(noviZaposleni.Prezime) ||
                    string.IsNullOrWhiteSpace(noviZaposleni.Email))
                {
                    MessageBox.Show("Molimo unesite sve podatke!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DatabaseConnection dbConnection = new DatabaseConnection();
                bool success = dbConnection.InsertZaposleni(noviZaposleni);

                if (success)
                {
                    MessageBox.Show("Novi zaposleni je uspešno dodat!", "Uspeh", MessageBoxButton.OK, MessageBoxImage.Information);
                    ZaposleniDataGrid.ItemsSource = dbConnection.GetZaposleniList(); // Osvježavanje liste
                    //AddZaposleniButton.Visibility = Visibility.Collapsed; // Sakrij dugme
                    BtnDodajZaposlenog.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("Greška pri dodavanju zaposlenog.", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        } //InsertZaposleni(noviZaposleni)
        private void PotvrdiButton1_Click(object sender, RoutedEventArgs e)
        {
            
            // string.IsNullOrWhiteSpace(SifraBox.Password) ||
            // Proveri da li su sva polja popunjena (može se dodati dodatna validacija)
            if (string.IsNullOrWhiteSpace(ImeTextBox.Text) ||
                string.IsNullOrWhiteSpace(PrezimeTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(UsernameTextBox.Text) ||

                string.IsNullOrWhiteSpace(passwordBox.Password) ||

                string.IsNullOrWhiteSpace(BrojTelefonaTextBox.Text) ||
                string.IsNullOrWhiteSpace(JMBGTextBox.Text) ||
                string.IsNullOrWhiteSpace(ZvanjeTextBox.Text) ||
                PaviljonComboBox.SelectedItem == null ||
                TemaComboBox.SelectedItem == null ||
                JezikComboBox.SelectedItem == null ||
                DatumRodjenjaDatePicker.SelectedDate == null ||
                DatumZaposlenjaDatePicker.SelectedDate == null ||
                string.IsNullOrWhiteSpace(AdresaTextBox.Text))
            {
                DodajZaposlenogPopup.IsOpen = false;
                MessageBox.Show(
                    _jezikSrpski ? "Molimo popunite sva polja!" : "Please fill in all fields!",
                    _jezikSrpski ? "Greška" : "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                this.Effect = null;
                return;
            }


            string pav = PaviljonComboBox.SelectedItem is ComboBoxItem paviljonItem ? paviljonItem.Content.ToString() : null;
            if (pav.StartsWith("Pavilion 1")) { pav = "Paviljon 1"; }
            if (pav.StartsWith("Pavilion 2")) { pav = "Paviljon 2"; }
            if (pav.StartsWith("Pavilion 3")) { pav = "Paviljon 3"; }
            if (pav.StartsWith("Pavilion 4")) { pav = "Paviljon 4"; }


            string tema = TemaComboBox.SelectedItem is ComboBoxItem temaItem ? temaItem.Content.ToString() : null;
            if (tema.StartsWith("Dark")) { tema = "Tamna"; }
            if (tema.StartsWith("Light")) { tema = "Svijetla"; }

            string jezik = JezikComboBox.SelectedItem is ComboBoxItem jezikItem ? jezikItem.Content.ToString() : null;
            if (jezik.StartsWith("English")) { jezik = "Engleski"; }
            if (jezik.StartsWith("Serbian")) { jezik = "Srpski"; }

            Zaposleni noviZaposleni = new Zaposleni
            {
                Ime = ImeTextBox.Text,
                Prezime = PrezimeTextBox.Text,
                Email = EmailTextBox.Text,
                Username = UsernameTextBox.Text,

                Sifra = passwordBox.Password,

                BrojTelefona = BrojTelefonaTextBox.Text,
                JMBG = JMBGTextBox.Text,
                Zvanje = ZvanjeTextBox.Text,
                Paviljon = pav,
                Tema = tema,
                Jezik = jezik,
                DatumRodjenja = DatumRodjenjaDatePicker.SelectedDate ?? DateTime.MinValue,  // Ako nije selektovan, postavi podrazumevanu vrednost
                DatumZaposlenja = DatumZaposlenjaDatePicker.SelectedDate ?? DateTime.MinValue,
                AdresaStanovanja = AdresaTextBox.Text
            };

                DatabaseConnection dbConnection = new DatabaseConnection();
                bool success = dbConnection.InsertZaposleni1(noviZaposleni);
               

            if (success)
            {
                DodajZaposlenogPopup.IsOpen = false;
                MessageBox.Show(
                    _jezikSrpski ? "Novi zaposleni je uspješno dodat!" : "New employee has been added successfully!",
                    _jezikSrpski ? "Uspjeh" : "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                this.Effect = null;
                ZaposleniDataGrid.ItemsSource = dbConnection.GetZaposleniList(); // Osvježavanje liste
                //AddZaposleniButton.Visibility = Visibility.Collapsed; // Sakrij dugme
                   
                // Sakrij popup
                DodajZaposlenogPopup.IsOpen = false;

                // Očisti polja nakon zatvaranja
                //  SifraBox.Clear();
                ImeTextBox.Clear();
                PrezimeTextBox.Clear();
                EmailTextBox.Clear();
                UsernameTextBox.Clear();

                passwordBox.Clear();

                BrojTelefonaTextBox.Clear();
                JMBGTextBox.Clear();
                ZvanjeTextBox.Clear();
                AdresaTextBox.Clear();
                PaviljonComboBox.SelectedIndex = 1;
                TemaComboBox.SelectedIndex = 1;
                JezikComboBox.SelectedIndex = 1;
                DatumRodjenjaDatePicker.SelectedDate = null;
                DatumZaposlenjaDatePicker.SelectedDate = null;
            }
            else
            {
                DodajZaposlenogPopup.IsOpen = false;
                MessageBox.Show(
                    _jezikSrpski ? "Greška pri dodavanju novog zaposlenog." : "Error while adding new employee.",
                    _jezikSrpski ? "Greška" : "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                this.Effect = null;
            }
        }
        
        private void DeleteZaposleni_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.Tag is int osobaID)
            {
                MessageBoxResult result = MessageBox.Show(
                    _jezikSrpski ? "Da li ste sigurni da želite da obrišete ovog zaposlenog?" : "Are you sure you want to delete this employee?",
                    _jezikSrpski ? "Potvrda brisanja" : "Delete Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);


                if (result == MessageBoxResult.Yes)
                {
                    DatabaseConnection dbConnection = new DatabaseConnection();
                    bool isDeleted = dbConnection.DeleteZaposleni(osobaID);

                    if (isDeleted)
                    {
                        MessageBox.Show(
                            _jezikSrpski ? "Zaposleni uspješno obrisan!" : "Employee deleted successfully!",
                            _jezikSrpski ? "Obavještenje" : "Notification",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

                        // Osvježavanje tabele
                        ZaposleniDataGrid.ItemsSource = dbConnection.GetZaposleniList();
                    }
                    else
                    {
                        MessageBox.Show(
                            _jezikSrpski ? "Greška prilikom brisanja zaposlenog!" : "Error while deleting the employee!",
                            _jezikSrpski ? "Greška" : "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        } //DeleteZaposleni(osobaID)
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var row = button.DataContext as Zaposleni;
            if (row == null) return;

            // Ako je datum u nekom nepravilnom formatu, automatski ga formatiraj
            if (DateTime.TryParse(row.DatumZaposlenja.ToString(), out DateTime datum))
            {
                // Ako konverzija uspe, postavi datum u željeni format (npr. "dd/MM/yyyy")
                row.DatumZaposlenja = datum;
            }



            // Dobijamo indeks reda u kojem je dugme kliknuto
            editableRowIndex = ZaposleniDataGrid.Items.IndexOf(row);

            //MessageBox.Show($"Uređivanje omogućeno samo za red {editableRowIndex}.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            MessageBox.Show(
                _jezikSrpski ? $"Uređivanje omogućeno samo za red {editableRowIndex}." : $"Editing is only allowed for row {editableRowIndex}.",
                _jezikSrpski ? "Info" : "Info",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
            // Prikazivanje dugmeta za čuvanje
            SaveButton.Visibility = Visibility.Visible;
        }
        private void ZaposleniDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            int rowIndex = ZaposleniDataGrid.Items.IndexOf(e.Row.Item);

            // Ako nije kliknuto na dugme, zabrani unos
            if (editableRowIndex == -1)
            {
                e.Cancel = true;
                MessageBox.Show(
                    _jezikSrpski ? "Kliknite na dugme za izmjenu pre nego što mijenjate podatke!" : "Click the button to edit before changing the data!",
                    _jezikSrpski ? "Upozorenje" : "Warning",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            // Ako korisnik pokušava da menja red koji nije onaj koji je izabrao, zabrani unos
            if (rowIndex != editableRowIndex)
            {
                e.Cancel = true;
                MessageBox.Show(
                    _jezikSrpski ? $"Možete mijenjati samo red {editableRowIndex}!" : $"You can only edit row {editableRowIndex}!",
                    _jezikSrpski ? "Zabranjeno uređivanje" : "Editing Forbidden",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }
        private void SaveChanges_Click1(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                _jezikSrpski ? "Da li ste sigurni da želite da sačuvate izmjene?" : "Are you sure you want to save the changes?",
                _jezikSrpski ? "Potvrda izmjene" : "Confirm Changes",
                 MessageBoxButton.YesNo,
                 MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                DatabaseConnection dbConnection = new DatabaseConnection();
                bool isUpdated = false;

                foreach (var item in ZaposleniDataGrid.Items)
                {
                    if (item is Zaposleni izmenjeni)
                    {
                        var original = originalZaposleniList.FirstOrDefault(z => z.OsobaID == izmenjeni.OsobaID);
                        if (original == null) continue;

                        bool podaciSuIzmenjeni =
                            izmenjeni.Username != original.Username ||
                            izmenjeni.Email != original.Email ||
                            izmenjeni.Sifra != original.Sifra ||
                            izmenjeni.DatumZaposlenja != original.DatumZaposlenja ||
                            izmenjeni.Paviljon != original.Paviljon ||
                            izmenjeni.Ime != original.Ime ||
                            izmenjeni.Prezime != original.Prezime ||
                            izmenjeni.BrojTelefona != original.BrojTelefona ||
                            izmenjeni.DatumRodjenja != original.DatumRodjenja ||
                            izmenjeni.AdresaStanovanja != original.AdresaStanovanja ||
                            izmenjeni.JMBG != original.JMBG ||
                            izmenjeni.Zvanje != original.Zvanje ||
                            izmenjeni.Tema != original.Tema ||
                            izmenjeni.Jezik != original.Jezik;

                        if (izmenjeni.Sifra == original.Sifra)
                        { izmenjeni.Sifra = ""; }

                        if (podaciSuIzmenjeni)
                        {
                            dbConnection.UpdateZaposleni(izmenjeni);
                            isUpdated = true;
                            LoadZaposleniData();
                        }
                    }
                }


                if (isUpdated)
                {
                    SaveButton.Visibility = Visibility.Collapsed;
                    ZaposleniDataGrid.ItemsSource = dbConnection.GetZaposleniList();

                    isEditingEnabled = false; // Ponovo blokiramo uređivanje
                    editableRowIndex = -1; // Resetujemo mogućnost uređivanja

                    MessageBox.Show(
                        _jezikSrpski ? "Podaci su uspješno ažurirani." : "The data has been successfully updated.",
                        _jezikSrpski ? "Obavještenje" : "Notification",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Nema izmena koje treba sačuvati.", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                    MessageBox.Show(
                        _jezikSrpski ? "Nema izmjena koje treba sačuvati." : "There are no changes to save.",
                        _jezikSrpski ? "Obavještenje" : "Notification",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
        } //UpdateZaposeni(zaposleni)
        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                _jezikSrpski ? "Da li ste sigurni da želite da sačuvate izmjene?" : "Are you sure you want to save the changes?",
                _jezikSrpski ? "Potvrda izmjene" : "Confirm Changes",
                 MessageBoxButton.YesNo,
                 MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                List<string> dozvoljeniPaviljoni = new List<string>
                {
                    "Paviljon 1",
                    "Paviljon 2",
                    "Paviljon 3",
                    "Paviljon 4"
                };

                DatabaseConnection dbConnection = new DatabaseConnection();
                bool isUpdated = false;

                foreach (var item in ZaposleniDataGrid.Items)
                {
                    if (item is Zaposleni izmenjeni)
                    {
                        var original = originalZaposleniList.FirstOrDefault(z => z.OsobaID == izmenjeni.OsobaID);
                        if (original == null) continue;

                        bool podaciSuIzmenjeni =
                            izmenjeni.Username != original.Username ||
                            izmenjeni.Email != original.Email ||
                            izmenjeni.Sifra != original.Sifra ||
                            izmenjeni.DatumZaposlenja != original.DatumZaposlenja ||
                            izmenjeni.Paviljon != original.Paviljon ||
                            izmenjeni.Ime != original.Ime ||
                            izmenjeni.Prezime != original.Prezime ||
                            izmenjeni.BrojTelefona != original.BrojTelefona ||
                            izmenjeni.DatumRodjenja != original.DatumRodjenja ||
                            izmenjeni.AdresaStanovanja != original.AdresaStanovanja ||
                            izmenjeni.JMBG != original.JMBG ||
                            izmenjeni.Zvanje != original.Zvanje ||
                            izmenjeni.Tema != original.Tema ||
                            izmenjeni.Jezik != original.Jezik;

                        if (izmenjeni.Sifra == original.Sifra)
                        {
                            izmenjeni.Sifra = "";
                        }

                        if (podaciSuIzmenjeni)
                        {
                            if (!dozvoljeniPaviljoni.Contains(izmenjeni.Paviljon))
                            {
                                MessageBox.Show(
                                    _jezikSrpski
                                        ? $"Neispravan unos za paviljon: {izmenjeni.Paviljon}"
                                        : $"Invalid entry for pavilion: {izmenjeni.Paviljon}",
                                    _jezikSrpski ? "Greška" : "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                continue; // preskoči ovog zaposlenog
                            }
                           
                            if (string.IsNullOrWhiteSpace(izmenjeni.JMBG) || izmenjeni.JMBG.Length != 13 || !izmenjeni.JMBG.All(char.IsDigit))
                            {
                                MessageBox.Show(
                                    _jezikSrpski
                                        ? $"JMBG mora sadržavati tačno 13 cifara za korisnika: {izmenjeni.Ime} {izmenjeni.Prezime}"
                                        : $"JMBG must contain exactly 13 digits for user: {izmenjeni.Ime} {izmenjeni.Prezime}",
                                    _jezikSrpski ? "Greška" : "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                                continue; // preskoči ovog zaposlenog
                            }


                            dbConnection.UpdateZaposleni(izmenjeni);
                            isUpdated = true;
                            LoadZaposleniData();
                        }
                    }
                }

                if (isUpdated)
                {
                    SaveButton.Visibility = Visibility.Collapsed;
                    ZaposleniDataGrid.ItemsSource = dbConnection.GetZaposleniList();

                    isEditingEnabled = false;
                    editableRowIndex = -1;

                    MessageBox.Show(
                        _jezikSrpski ? "Podaci su uspješno ažurirani." : "The data has been successfully updated.",
                        _jezikSrpski ? "Obavještenje" : "Notification",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(
                        _jezikSrpski ? "Nema izmjena koje treba sačuvati." : "There are no changes to save.",
                        _jezikSrpski ? "Obavještenje" : "Notification",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
        }

        private List<Zaposleni> originalZaposleniList;

        private void LoadZaposleniData()
        {
            DatabaseConnection dbConnection = new DatabaseConnection();
            var zaposleniList = dbConnection.GetZaposleniList();

            // Podesi ObservableCollection za prikaz
            ZaposleniList = new ObservableCollection<Zaposleni>(zaposleniList);
            ZaposleniDataGrid.ItemsSource = ZaposleniList;

            // Sačuvaj originalnu kopiju za poređenje
            originalZaposleniList = zaposleniList.Select(z => new Zaposleni
            {
                OsobaID = z.OsobaID,
                Username = z.Username,
                Email = z.Email,
                Sifra = z.Sifra,
                DatumZaposlenja = z.DatumZaposlenja,
                Paviljon = z.Paviljon,
                Ime = z.Ime,
                Prezime = z.Prezime,
                BrojTelefona = z.BrojTelefona,
                DatumRodjenja = z.DatumRodjenja,
                AdresaStanovanja = z.AdresaStanovanja,
                JMBG = z.JMBG,
                Zvanje = z.Zvanje,
                Tema = z.Tema,
                Jezik = z.Jezik
            }).ToList();
        }

        public void RefreshZaposleniList()
        {
            DatabaseConnection dbConnection = new DatabaseConnection();
            var zaposleniList = dbConnection.GetZaposleniList(); // `GetZaposleniList()` vraća listu zaposlenih
            ZaposleniDataGrid.ItemsSource = zaposleniList;
            ZaposleniDataGrid.Items.Refresh();  // Osvežavanje prikaza
        } //GetZaposleniList()
        private void ZatvoriPopup_Click(object sender, RoutedEventArgs e)
        {
            DodajZaposlenogPopup.IsOpen = false;
            this.Effect = null;
        }
    }
}
