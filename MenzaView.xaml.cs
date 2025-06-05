using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    /// Interaction logic for MenzaView.xaml
    /// </summary>
    public partial class MenzaView : UserControl, INotifyPropertyChanged
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
        
        public MenzaView(string email = "empty", bool jezikSrpski = true)
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

                this.DataContext = null;  // Ručno osvežavanje UI-a
                this.DataContext = this;

                BtnDodajOglas.Visibility = Visibility.Collapsed;
            }
            UcitajIOsveziPrikazMenza();
        }

        private void UcitajIOsveziPrikazMenza()
        {
            MenzaWrapPanel.ItemsSource = DatabaseConnection.UcitajMenza();
            MenzaViewLanguage();
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

        private async void MenzaViewLanguage()
        {
            if (MenzaWrapPanel == null) return;

            await Task.Delay(50); // osiguraj da su itemi renderovani

            foreach (var item in MenzaWrapPanel.Items)
            {
                var container = MenzaWrapPanel.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                if (container != null)
                {
                   var dugmeObrisi = FindChild<Button>(container, "Obrisi_Button_Menza");
                    if (dugmeObrisi != null)
                        dugmeObrisi.Content = jezikSrpski ? "Obriši" : "Delete";
                    // Labela: Cijena doručka
                    var label1 = FindChild<TextBlock>(container, "MenzaWrapPanel_Label1");
                    if (label1 != null)
                        label1.Text = label1.Text = jezikSrpski ? "Cijena doručka: " : "Breakfast price: ";

                    // Labela: Cijena ručka
                    var label2 = FindChild<TextBlock>(container, "MenzaWrapPanel_Label2");
                    if (label2 != null)
                        label2.Text = label2.Text = jezikSrpski ? "Cijena ručka: " : "Lunch price: ";

                    // Labela: Cijena večere
                    var label3 = FindChild<TextBlock>(container, "MenzaWrapPanel_Label3");
                    if (label3 != null)
                        label3.Text = label3.Text = jezikSrpski ? "Cijena večere: " : "Dinner price: ";

                    // Labela: Termin doručka
                    var label4 = FindChild<TextBlock>(container, "MenzaWrapPanel_Label4");
                    if (label4 != null)
                        label4.Text = label4.Text = jezikSrpski ? "Termin doručka: " : "Breakfast time: ";

                    // Labela: Termin ručka
                    var label5 = FindChild<TextBlock>(container, "MenzaWrapPanel_Label5");
                    if (label5 != null)
                        label5.Text = label5.Text = jezikSrpski ? "Termin ručka: " : "Lunch time: ";

                    // Labela: Termin večere
                    var label6 = FindChild<TextBlock>(container, "MenzaWrapPanel_Label6");
                    if (label6 != null)
                        label6.Text = label6.Text = jezikSrpski ? "Termin večere: " : "Dinner time: ";
                }
            }
        }

        private void BtnDodajOglas_Click(object sender, RoutedEventArgs e)
        {
            // Otvori popup za unos podataka
            MenzaPopup.IsOpen = true;
            this.Effect = new BlurEffect { Radius = 10 };
        }

        private void PotvrdiButton_Click(object sender, RoutedEventArgs e)
        {
            MenzaPopup.IsOpen = false;
            // Provera da li su sva polja popunjena
            if (string.IsNullOrWhiteSpace(TekstOglasaTextBox.Text) ||
                string.IsNullOrWhiteSpace(CenaDoruckaTextBox.Text) ||
                string.IsNullOrWhiteSpace(CenaRuckaTextBox.Text) ||
                string.IsNullOrWhiteSpace(CenaVecereTextBox.Text) ||
                string.IsNullOrWhiteSpace(TerminDoruckaTextBox.Text) ||
                string.IsNullOrWhiteSpace(TerminRuckaTextBox.Text) ||
                string.IsNullOrWhiteSpace(TerminVecereTextBox.Text))
            {
                MenzaPopup.IsOpen = false;
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
                // Konverzija cena u decimalne vrednosti
                decimal cenaDorucka = Convert.ToDecimal(CenaDoruckaTextBox.Text);
                decimal cenaRucka = Convert.ToDecimal(CenaRuckaTextBox.Text);
                decimal cenaVecere = Convert.ToDecimal(CenaVecereTextBox.Text);


                int osobaID = id;

                using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=HCI_domDB;Uid=root;Pwd=radenkomsql;"))
                {
                    connection.Open();
                    string query = @"INSERT INTO Menza (Tekst, CijenaDorucka, CijenaRucka, CijenaVecere, 
                                                TerminDorucka, TerminRucka, TerminVecere, OsobaID) 
                             VALUES (@Tekst, @CijenaDorucka, @CijenaRucka, @CijenaVecere, 
                                     @TerminDorucka, @TerminRucka, @TerminVecere, @OsobaID)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Tekst", TekstOglasaTextBox.Text);
                        cmd.Parameters.AddWithValue("@CijenaDorucka", cenaDorucka);
                        cmd.Parameters.AddWithValue("@CijenaRucka", cenaRucka);
                        cmd.Parameters.AddWithValue("@CijenaVecere", cenaVecere);
                        cmd.Parameters.AddWithValue("@TerminDorucka", TerminDoruckaTextBox.Text);
                        cmd.Parameters.AddWithValue("@TerminRucka", TerminRuckaTextBox.Text);
                        cmd.Parameters.AddWithValue("@TerminVecere", TerminVecereTextBox.Text);
                        cmd.Parameters.AddWithValue("@OsobaID", osobaID);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    jezikSrpski ? "Podaci su uspješno sačuvani!" : "Data saved successfully!",
                    jezikSrpski ? "Uspjeh" : "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );


                // Zatvaranje popupa nakon unosa
                MenzaPopup.IsOpen = false;
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
            UcitajIOsveziPrikazMenza();
            this.Effect = null;
            TekstOglasaTextBox.Text = string.Empty;
            CenaDoruckaTextBox.Text = string.Empty;
            CenaRuckaTextBox.Text = string.Empty;
            CenaVecereTextBox.Text = string.Empty;
            TerminDoruckaTextBox.Text = string.Empty;
            TerminRuckaTextBox.Text = string.Empty;
            TerminVecereTextBox.Text = string.Empty;
            UcitajIOsveziPrikazMenza();
        }

        private void ObrisiMenza_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int menzaID)
            {
                MessageBoxResult result = MessageBox.Show(
                    jezikSrpski ? "Da li ste sigurni da želite obrisati ovaj oglas?" : "Are you sure you want to delete this post?",
                    jezikSrpski ? "Potvrda brisanja" : "Delete Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );


                if (result == MessageBoxResult.Yes)
                {
                    if (ObrisiMenza(menzaID))
                    {
                        MessageBox.Show(
                            jezikSrpski ? "Oglas uspješno obrisan." : "Post deleted successfully.",
                            jezikSrpski ? "Uspeh" : "Success",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

                        UcitajIOsveziPrikazMenza(); // Osvežavanje liste nakon brisanja
                    }
                    else
                    {
                        MessageBox.Show(
                            jezikSrpski ? "Greška pri brisanju oglasa." : "Error while deleting the post.",
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
            MenzaPopup.IsOpen = false;
            this.Effect = null;
        }
    }
}
