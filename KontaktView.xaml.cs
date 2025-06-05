using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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
    /// Interaction logic for KontaktView.xaml
    /// </summary>
    public partial class KontaktView : UserControl, INotifyPropertyChanged
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
        public KontaktView(string email = "empty", bool jezikSrpski = true)
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
                BtnDodajKontakt.Visibility = Visibility.Visible;
            }
            else
            {
                IsLoggedIn = false; // Neprijavljen korisnik
                //MessageBox.Show("IsAdmin vrednost: " + IsAdmin); // Provera vrednosti

                this.DataContext = null;  // Ručno osvežavanje UI-a
                this.DataContext = this;

                BtnDodajKontakt.Visibility = Visibility.Collapsed;
            }
            UcitajIOsveziPrikazKontakt();
        }

        private void UcitajIOsveziPrikazKontakt()
        {
            KontaktWrapPanel.ItemsSource = DatabaseConnection.UcitajKontakt();
            KontaktiViewLanguage();
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

        private async void KontaktiViewLanguage()
        {
            if (KontaktWrapPanel == null) return;

            await Task.Delay(50); // osiguraj da su itemi renderovani

            foreach (var item in KontaktWrapPanel.Items)
            {
                var container = KontaktWrapPanel.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                if (container != null)
                {
                 var dugmeObrisi = FindChild<Button>(container, "KontaktWrapPanel_Delete");
                    if (dugmeObrisi != null)
                        dugmeObrisi.Content = jezikSrpski ? "Obriši" : "Delete";
                    var label1 = FindChild<TextBlock>(container, "KontaktWrapPanel_Label1");
                    if (label1 != null)
                        label1.Text = jezikSrpski ? "Adresa 1: " : "Address 1: ";

                    var label2 = FindChild<TextBlock>(container, "KontaktWrapPanel_Label2");
                    if (label2 != null)
                        label2.Text = jezikSrpski ? "Adresa 2: " : "Address 2: ";

                    var label3 = FindChild<TextBlock>(container, "KontaktWrapPanel_Label3");
                    if (label3 != null)
                        label3.Text = jezikSrpski ? "Broj telefona 1: " : "Phone number 1: ";

                    var label4 = FindChild<TextBlock>(container, "KontaktWrapPanel_Label4");
                    if (label4 != null)
                        label4.Text = jezikSrpski ? "Broj telefona 2: " : "Phone number 2: ";
                }
            }
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true; // Sprečava dalje propagiranje događaja
        }

        private void BtnDodajKontakt_Click(object sender, RoutedEventArgs e)
        {
            // Otvori popup za unos podataka
            KontaktPopup.IsOpen = true;
            this.Effect = new BlurEffect { Radius = 10 };
        }

        private void PotvrdiButton_Click(object sender, RoutedEventArgs e)
        {
            // Provera obaveznih polja
            if (string.IsNullOrWhiteSpace(Adresa1TextBox.Text) ||
                string.IsNullOrWhiteSpace(BrTelefona1TextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text))
            {
                KontaktPopup.IsOpen = false;
                MessageBox.Show(
                    jezikSrpski ? "Sva obavezna polja (Adresa 1, Broj telefona 1, Email) moraju biti popunjena!"
                    : "All required fields (Address 1, Phone number 1, Email) must be filled!",
                    jezikSrpski ? "Greška" : "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                this.Effect = null;
                return;
            }

            KontaktModel noviKontakt = new KontaktModel
            {
                Adresa1 = Adresa1TextBox.Text,
                Adresa2 = string.IsNullOrWhiteSpace(Adresa2TextBox.Text) ? null : Adresa2TextBox.Text,
                BrTelefona1 = BrTelefona1TextBox.Text,
                BrTelefona2 = string.IsNullOrWhiteSpace(BrTelefona2TextBox.Text) ? null : BrTelefona2TextBox.Text,
                Email = EmailTextBox.Text,
                InstagramLink = string.IsNullOrWhiteSpace(InstagramLinkTextBox.Text) ? null : InstagramLinkTextBox.Text,
                FacebookLink = string.IsNullOrWhiteSpace(FacebookLinkTextBox.Text) ? null : FacebookLinkTextBox.Text,
                Tekst = string.IsNullOrWhiteSpace(TekstTextBox.Text) ? null : TekstTextBox.Text
            };

            if (DatabaseConnection.DodajKontaktUBazu(noviKontakt))
            {
                KontaktPopup.IsOpen = false;
                MessageBox.Show(
                    jezikSrpski ? "Kontakt uspješno dodat!" : "Contact added successfully!",
                    jezikSrpski ? "Uspjeh" : "Success",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );


                TekstTextBox.Text = string.Empty;
                Adresa1TextBox.Text = string.Empty;
                Adresa2TextBox.Text = string.Empty;
                BrTelefona1TextBox.Text = string.Empty;
                BrTelefona2TextBox.Text = string.Empty;
                EmailTextBox.Text = string.Empty;
                InstagramLinkTextBox.Text = string.Empty;
                FacebookLinkTextBox.Text = string.Empty;
                UcitajIOsveziPrikazKontakt();  // Ova funkcija učitava podatke iz baze i osvežava WrapPanel
                this.Effect = null;
            }
            else
            {
                MessageBox.Show(
                    jezikSrpski ? "Greška pri dodavanju kontakta!" : "Error while adding the contact!",
                    jezikSrpski ? "Greška" : "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

            }
        }

        private void ObrisiKontakt_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int kontaktID)
            {
                MessageBoxResult result = MessageBox.Show(
                    jezikSrpski ? "Da li ste sigurni da želite da obrišete ovaj kontakt?" : "Are you sure you want to delete this contact?",
                    jezikSrpski ? "Potvrda brisanja" : "Delete Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);


                if (result == MessageBoxResult.Yes)
                {
                    bool uspesnoObrisano = DatabaseConnection.ObrisiKontakt(kontaktID);

                    if (uspesnoObrisano)
                    {
                        UcitajIOsveziPrikazKontakt(); // Osvježavanje prikaza nakon brisanja
                    }
                    else
                    {
                        MessageBox.Show(
                            jezikSrpski ? "Došlo je do greške prilikom brisanja kontakta." : "An error occurred while deleting the contact.",
                            jezikSrpski? "Greška" : "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ZatvoriPopup_Click(object sender, RoutedEventArgs e)
        {
            KontaktPopup.IsOpen = false;
            this.Effect = null;
        }
    }
}
