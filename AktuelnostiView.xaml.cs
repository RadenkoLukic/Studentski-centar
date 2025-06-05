using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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
    /// Interaction logic for AktuelnostiView.xaml
    /// </summary>
    public partial class AktuelnostiView : UserControl, INotifyPropertyChanged
    {
        private string selectedPdfPath = ""; // Čuva putanju do odabranog PDF fajla
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

        public AktuelnostiView(string email = "empty", bool jezikSrpski = true)
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
                    MessageBox.Show("Zaposleni not found for the given email.");
                    return;
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
            UcitajIOsveziPrikazOglasa();
            //Application.Current.Resources["PrimaryTextBrush"] = new SolidColorBrush(Colors.White); // Tamna tema

        }

        private void UcitajIOsveziPrikazOglasa()
        {
            //OglasiListView.ItemsSource = DatabaseConnection.UcitajOglase();
            OglasiWrapPanel.ItemsSource = DatabaseConnection.UcitajOglase();
            AktuelnostiViewLanguage();
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
        private async void AktuelnostiViewLanguage()
        {
            if (OglasiWrapPanel == null) return;

            await Task.Delay(50); // osiguraj da su itemi renderovani

            foreach (var item in OglasiWrapPanel.Items)
            {
                var container = OglasiWrapPanel.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                if (container != null)
                {
                    var dugmeOtvori = FindChild<Button>(container, "OtvoriPDFButton");
                    if (dugmeOtvori != null)
                        dugmeOtvori.Content = jezikSrpski ? "Otvori PDF" : "Open PDF";

                    var dugmeObrisi = FindChild<Button>(container, "Obrisi_Button");
                    if (dugmeObrisi != null)
                        dugmeObrisi.Content = jezikSrpski ? "Obriši" : "Delete";
                }
            }
        }

        private void BtnDodajOglas_Click(object sender, RoutedEventArgs e)
        {
            // Otvori popup za unos podataka
            OglasPopup.IsOpen = true;
            this.Effect = new BlurEffect { Radius = 10 };
        }

        private void OdaberiPDFButton_Click(object sender, RoutedEventArgs e)
        {
            OglasPopup.IsOpen = false;
            // Otvori dijalog za odabir fajla
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF Files (*.pdf)|*.pdf"; // Filtriranje samo PDF fajlova

            if (openFileDialog.ShowDialog() == true)
            {
                selectedPdfPath = openFileDialog.FileName; // Sačuvaj putanju do fajla
                PDFFileNameText.Text = System.IO.Path.GetFileName(selectedPdfPath); // Prikaz imena fajla
            }
            OglasPopup.IsOpen = true;
        }

        private void PotvrdiButton_Click(object sender, RoutedEventArgs e)
        {
            OglasPopup.IsOpen = false;
            // Provera da li su unijeti svi podaci
            if (string.IsNullOrWhiteSpace(NaslovTextBox.Text) || string.IsNullOrEmpty(selectedPdfPath))
            {
                OglasPopup.IsOpen = false;
                //MessageBox.Show("Molimo unesite naslov i odaberite PDF fajl.", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                MessageBox.Show(
                    jezikSrpski ? "Molimo unesite naslov i odaberite PDF fajl." : "Please enter title and choose PDF file.",
                    jezikSrpski ? "Greška" : "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                this.Effect = null;
                return;
            }

            string naslov = NaslovTextBox.Text;
            string originalPath = selectedPdfPath;
            string fileName = System.IO.Path.GetFileName(originalPath);
            // 📂 Folder u koji se čuva PDF
            string saveDirectory = @"C:\Users\Toba\Desktop\WPF-UI__AnimatedSlidingLoginAndSignUpForm-1-main\Resources\PDFs\Aktuelnosti\";
            string newFilePath = System.IO.Path.Combine(saveDirectory, fileName);

            try
            {
                // Kreiraj folder ako ne postoji
                if (!Directory.Exists(saveDirectory))
                    Directory.CreateDirectory(saveDirectory);

                // Kopiraj fajl na zadatu lokaciju
                File.Copy(originalPath, newFilePath, true);

                using (MySqlConnection conn = new MySqlConnection("Server=localhost;Database=HCI_domDB;Uid=root;Pwd=radenkomsql;"))
                {
                    conn.Open();
                    MySqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        // 1️. Ubacivanje u tabelu Oglas
                        string insertOglas = "INSERT INTO Oglas (Naslov, DatumObjave, OsobaID) VALUES (@Naslov, @DatumObjave, @OsobaID); SELECT LAST_INSERT_ID();";
                        MySqlCommand cmdOglas = new MySqlCommand(insertOglas, conn, transaction);
                        cmdOglas.Parameters.AddWithValue("@Naslov", naslov);
                        cmdOglas.Parameters.AddWithValue("@DatumObjave", DateTime.Now); // Trenutni datum i vrijeme
                        cmdOglas.Parameters.AddWithValue("@OsobaID", id); // ID prijavljenog korisnika

                        int oglasId = Convert.ToInt32(cmdOglas.ExecuteScalar());

                        // 2️. Ubacivanje u tabelu Aktuelnosti
                        string insertAktuelnosti = "INSERT INTO Aktuelnosti (OglasID, Dokument) VALUES (@OglasID, @Dokument);";
                        MySqlCommand cmdAktuelnosti = new MySqlCommand(insertAktuelnosti, conn, transaction);
                        cmdAktuelnosti.Parameters.AddWithValue("@OglasID", oglasId);
                        cmdAktuelnosti.Parameters.AddWithValue("@Dokument", newFilePath); // Čuva putanju PDF-a

                        cmdAktuelnosti.ExecuteNonQuery();

                        transaction.Commit();
                        
                        MessageBox.Show(
                            jezikSrpski ? "Oglas je uspešno dodat!" : "News successfully added",
                            jezikSrpski ? "Uspjeh" : "Success",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                        );

                        // Resetovanje polja nakon unosa
                        NaslovTextBox.Text = string.Empty;
                        //PDFFileNameText.Text = "Nijedan fajl nije odabran";
                        PDFFileNameText.Text = jezikSrpski ? "Nijedan fajl nije odabran" : "No file selected";
                        selectedPdfPath = null;
                        OglasPopup.IsOpen = false;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        //MessageBox.Show("Greška pri upisu u bazu: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                        MessageBox.Show(
                            jezikSrpski ? "Greška pri upisu u bazu: " + ex.Message : "Database write error: " + ex.Message,
                            jezikSrpski ? "Greška" : "Error",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Greška pri povezivanju sa bazom: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show(
                    jezikSrpski ? "Greška pri povezivanju sa bazom: " + ex.Message : "Database connection error: " + ex.Message,
                    jezikSrpski ? "Greška" : "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );

            }
            UcitajIOsveziPrikazOglasa();
            this.Effect = null;
        }

        private void OtvoriPDF_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null && btn.Tag != null)
            {
                string filePath = btn.Tag.ToString();
                if (File.Exists(filePath))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    //MessageBox.Show("Fajl ne postoji!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    MessageBox.Show(
                        jezikSrpski ? "Fajl ne postoji!" : "The file does not exist!",
                        jezikSrpski ? "Greška" : "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        private void ObrisiOglas_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int oglasID)
            {
                //MessageBoxResult result = MessageBox.Show("Da li ste sigurni da želite da obrišete ovaj oglas?",
                //"Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                MessageBoxResult result = MessageBox.Show(
                    jezikSrpski ? "Da li ste sigurni da želite da obrišete ovaj oglas?" : "Are you sure you want to delete this post?",
                    jezikSrpski ? "Potvrda" : "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // Logika za brisanje oglasa iz baze
                    ObrisiOglasIzBaze(oglasID);

                    // Ponovo učitaj listu oglasa
                    UcitajIOsveziPrikazOglasa();
                }
            }
        }

        private void ZatvoriPopup_Click(object sender, RoutedEventArgs e)
        {
            OglasPopup.IsOpen = false;
            this.Effect = null;
        }
    }
}
