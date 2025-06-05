using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for StudentskiSmjestajVew.xaml
    /// </summary>
    public partial class StudentskiSmjestajView : UserControl, INotifyPropertyChanged
    {
        private int loggedUserId; // ID ulogovanjog zaposlenog
        string userEmail;
        private bool jezikSrpski = true;

        Zaposleni zaposleni = new Zaposleni();
        int id;
        public bool _isloggedin;
        public bool IsLoggedIn
        {
            get { return _isloggedin; }
            set
            {
                _isloggedin = value;
                OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public StudentskiSmjestajView(string email = "empty", bool jezikSrpski = true)
        {
            this.jezikSrpski = jezikSrpski;
            this.DataContext = this;  // Postavi pre inicijalizacije
            InitializeComponent();
            if (email != "empty")
            {
                userEmail = email;
                zaposleni = GetZaposleniData(userEmail);
                if (zaposleni == null)
                {
                    // Handle the case when zaposleni is null
                    MessageBox.Show("Zaposleni not found for the given email.");
                    return; // Or handle it in another way, such as redirecting to a login page or showing an error.
                }
                id = zaposleni.OsobaID;
                IsLoggedIn = true; // Korisnik je prijavljen
                //MessageBox.Show("IsAdmin vrednost: " + IsAdmin); // Provera vrednosti

                this.DataContext = null;  // Ručno osvežavanje UI-a
                this.DataContext = this;
                BtnDodajOglas.Visibility = Visibility.Visible;
            }
            else
            {
                IsLoggedIn = false; // Neprijavljen korisnik
                //MessageBox.Show("IsAdmin vrednost: " + IsAdmin); // Provera vrednosti

                this.DataContext = null;  // Ručno osvežavanje UI-a
                this.DataContext = this;

                BtnDodajOglas.Visibility = Visibility.Collapsed;
            }
            UcitajIOsveziPrikazStudentskiSmjestaj();
        }

        private void UcitajIOsveziPrikazStudentskiSmjestaj()
        {
            StudentskiSmjestajWrapPanel.ItemsSource = DatabaseConnection.UcitajStudentskiSmjestaj();
            StudentskiSmjestajLanguage();
        }

        public static T FindChild<T>(DependencyObject parent, string childName) where T : FrameworkElement
        {
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T childType && childType.Name == childName)
                {
                    return childType;
                }

                foundChild = FindChild<T>(child, childName);
                if (foundChild != null) break;
            }

            return foundChild;
        }

        private async void StudentskiSmjestajLanguage()
        {
            if (StudentskiSmjestajWrapPanel == null) return;

            await Task.Delay(50); // osiguraj da su itemi renderovani

            foreach (var item in StudentskiSmjestajWrapPanel.Items)
            {
                var container = StudentskiSmjestajWrapPanel.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                if (container != null)
                {
                    var dugmeObrisi = FindChild<Button>(container, "Obrisi_Button_Smjestaj");
                    if (dugmeObrisi != null)
                        dugmeObrisi.Content = jezikSrpski ? "Obriši" : "Delete";
                }
            }
        }

        private void BtnDodajOglas_Click(object sender, RoutedEventArgs e)
        {
            // Otvori popup za unos podataka
            StudentskiSmjestajPopup.IsOpen = true;
            this.Effect = new BlurEffect { Radius = 10 };
        }

        private void PotvrdiButton_Click(object sender, RoutedEventArgs e)
        {
            StudentskiSmjestajPopup.IsOpen = false;

            // Provera da li su sva polja popunjena
            if (string.IsNullOrWhiteSpace(TekstObjaveTextBox.Text) ||
                string.IsNullOrWhiteSpace(NaslovTextBox.Text))
            {
                StudentskiSmjestajPopup.IsOpen = false;
                MessageBox.Show(
                    jezikSrpski ? "Molimo vas da popunite sva polja." : "Please fill in all fields.",
                    jezikSrpski ? "Greška" : "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                this.Effect = null;
                return;
            }

            try
            {
                int osobaID = id;
                int oglasID = 0;

                using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=HCI_domDB;Uid=root;Pwd=radenkomsql;"))
                {
                    connection.Open();

                    // Prvo unosimo podatke u tabelu Oglas
                    string queryOglas = @"INSERT INTO Oglas (Naslov, DatumObjave, OsobaID) 
                                  VALUES (@Naslov, NOW(), @OsobaID);
                                  SELECT LAST_INSERT_ID();";

                    using (MySqlCommand cmdOglas = new MySqlCommand(queryOglas, connection))
                    {
                        cmdOglas.Parameters.AddWithValue("@Naslov", NaslovTextBox.Text);
                        cmdOglas.Parameters.AddWithValue("@OsobaID", osobaID);

                        // Dobijamo ID novounesenog oglasa
                        oglasID = Convert.ToInt32(cmdOglas.ExecuteScalar());
                    }

                    // Sada unosimo podatke u tabelu StudentskiSmjestaj koristeći oglasID
                    string querySmjestaj = @"INSERT INTO StudentskiSmjestaj (OglasID, TekstOglasa) 
                                     VALUES (@OglasID, @TekstOglasa)";

                    using (MySqlCommand cmdSmjestaj = new MySqlCommand(querySmjestaj, connection))
                    {
                        cmdSmjestaj.Parameters.AddWithValue("@OglasID", oglasID);
                        cmdSmjestaj.Parameters.AddWithValue("@TekstOglasa", TekstObjaveTextBox.Text);

                        cmdSmjestaj.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    jezikSrpski ? "Podaci su uspešno sačuvani!" : "Data saved successfully!",
                    jezikSrpski ? "Uspjeh" : "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );


                // Zatvaranje popupa nakon unosa
                StudentskiSmjestajPopup.IsOpen = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    (jezikSrpski ? "Greška pri unosu podataka: " : "Error while entering data: ") + ex.Message,
                    jezikSrpski ? "Greška" : "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

            }

            UcitajIOsveziPrikazStudentskiSmjestaj();
            this.Effect = null;

            // Resetovanje polja
            TekstObjaveTextBox.Text = string.Empty;
            NaslovTextBox.Text = string.Empty;
        }


        private void ObrisiStudentskiSmjestaj_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int oglasID)
            {
                MessageBoxResult result = MessageBox.Show(
                    jezikSrpski ? "Da li ste sigurni da želite obrisati ovaj oglas?" : "Are you sure you want to delete this post?",
                    jezikSrpski ? "Potvrda brisanja" : "Delete Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );


                if (result == MessageBoxResult.Yes)
                {
                    if (ObrisiStudentskiSmjestaj(oglasID))
                    {
                        MessageBox.Show(
                            jezikSrpski ? "Oglas uspešno obrisan." : "Post deleted successfully.",
                            jezikSrpski ? "Uspjeh" : "Success",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                        );

                        UcitajIOsveziPrikazStudentskiSmjestaj(); // Osvežavanje liste nakon brisanja
                    }
                    else
                    {
                        MessageBox.Show(
                            jezikSrpski ? "Greška pri brisanju oglasa." : "Error deleting the post.",
                            jezikSrpski ? "Greška" : "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );

                    }
                }
            }
        }

        private void ZatvoriPopup_Click(object sender, RoutedEventArgs e)
        {
            StudentskiSmjestajPopup.IsOpen = false;
            this.Effect = null;
        }
    }
}
