using MaterialDesignThemes.Wpf;
using MaterialMenu;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using static LoginForm.DatabaseConnection;
using static MaterialDesignThemes.Wpf.Theme.ToolBar;


using System.Windows.Media;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Diagnostics.Metrics;
//using static System.Net.Mime.MediaTypeNames;
using System.Windows.Controls.Primitives;

using System.Globalization;
using System.Threading;
using System.Security.Policy;

namespace LoginForm
{
    public partial class DashboardWindow : Window
    {
        //private bool isPanelOpen = true; // Praćenje statusa tamno sivog panela

        private string userRole;
        //private Zaposleni noviZaposleni;

        private string loggedInUserEmail; // Čuvamo email prijavljenog korisnika
        public enum Tema
        {
            Tamna,
            Svijetla,
            Zelena
        }
        private Tema trenutnaTema = Tema.Tamna;

        // Podrazumevani konstruktor za pokretanje aplikacije
        public DashboardWindow()
        {
            InitializeComponent();
            userRole = "Guest"; // Postavljamo podrazumevanu ulogu kao 'Guest'
            UpdateMenuForUserRole();
            SetInitialView(); // Postavlja prvo dugme kao aktivno pri pokretanju

            // Postavljanje minimalnih dimenzija
            this.MinWidth = 750;
            this.MinHeight = 200;
        }
        public DashboardWindow(string role, string email)
        {
            InitializeComponent();
            userRole = role;
            UpdateMenuForUserRole(); // Pozivamo funkciju da ažurira meni
           

            loggedInUserEmail = email; // Postavljamo email ulogovanog korisnika
            DatabaseConnection db = new DatabaseConnection();
            //var zaposleni = DatabaseConnection.GetZaposleniData(email);
            string sacuvanaTema = db.GetUserTheme(email); // Učitavanje sačuvane teme
            //MessageBox.Show("Sačuvana tema: " + sacuvanaTema, "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);


            // Postavljanje enum vrednosti na osnovu stringa
            trenutnaTema = sacuvanaTema switch
            {
                //"Svijetla" => Tema.Svijetla,
                //"Zelena" => Tema.Zelena,
                //_ => Tema.Tamna // Ako nije prepoznato, koristi podrazumevanu "Tamna"

                "Svijetla" => Tema.Tamna,
                "Zelena" => Tema.Svijetla,
                "Tamna" => Tema.Zelena,
                _ => Tema.Tamna // Ako nije prepoznato, koristi podrazumevanu "Tamna"
            };

            ToggleTheme(null, null); // Primeni temu

            string sacuvaniJezik = db.GetUserLanguage(email);
            jezikSrpski = sacuvaniJezik != "Srpski";
                ToggleLanguage(null, null);
            
            SetInitialView(); // Postavlja prvo dugme kao aktivno pri pokretanju

            // Postavljanje minimalnih dimenzija
            this.MinWidth = 750;
            this.MinHeight = 200;
        }

        
        private void UpdateMenuForUserRole()
        {
            if (userRole == "Admin")
            {
                LoginButton.Visibility = Visibility.Collapsed;
                ProfileButton.Visibility = Visibility.Visible;
                MenuItem1.Visibility = Visibility.Collapsed;
                MenuItem2.Visibility = Visibility.Collapsed;
                MenuItem3.Visibility = Visibility.Collapsed;
                MenuItem4.Visibility = Visibility.Collapsed;
                MenuItem5.Visibility = Visibility.Collapsed;
                MenuItem6.Visibility = Visibility.Visible;
                MenuItem7.Visibility = Visibility.Visible;


                // Inicijalizacija ObservableCollection-a
                //ZaposleniList = new ObservableCollection<Zaposleni>();
                //LoadZaposleniData();
                //ListaZaposlenih_Naslov.Visibility = Visibility.Visible;
                //ZaposleniDataGrid.Visibility = Visibility.Visible; // Prikazuje tabelu za admina
                //AddZaposleni.Visibility = Visibility.Visible;
            }
            else if (userRole == "Zaposleni")
            {
                LoginButton.Visibility = Visibility.Collapsed;
                ProfileButton.Visibility = Visibility.Visible;
                MenuItem6.Visibility = Visibility.Collapsed;

                MenuItem1.Visibility = Visibility.Visible;
               
                MenuItem2.Visibility = Visibility.Visible;
                MenuItem3.Visibility = Visibility.Visible;
                MenuItem4.Visibility = Visibility.Visible;
                MenuItem5.Visibility = Visibility.Visible;
                MenuItem7.Visibility = Visibility.Visible;

            }
            else // Ako je korisnik gost (nije prijavljen)
            {
                LoginButton.Visibility = Visibility.Visible;
                ProfileButton.Visibility = Visibility.Collapsed;
                MenuItem6.Visibility = Visibility.Collapsed;
            }
        }


        private void OpenLoginWindow()
        {
            LoginWindow loginWindow = new LoginWindow(this, jezikSrpski, trenutnaTema.ToString()); // Prosleđujemo trenutni prozor
            this.Effect = new System.Windows.Media.Effects.BlurEffect { Radius = 10 };
            BackgroundMask.Visibility = Visibility.Visible;
            loginWindow.ShowDialog();
            this.Effect = null;
            BackgroundMask.Visibility = Visibility.Collapsed;
            
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            OpenLoginWindow();
        }


        // Otvori Popup kada klikneš na ProfileButton
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ProfilePopup.IsOpen = true; // Otvori Popup
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Logika za izlogovanje korisnika (npr. čišćenje sesije ili korisničkih podataka)
            // Zatvori Popup
            ProfilePopup.IsOpen = false;

            // 🔹 Otvaramo Dashboard i prosleđujemo ulogu korisnika
            DashboardWindow dashboard = new DashboardWindow();
            dashboard.Show();

            this.Close(); // Zatvaramo Login prozor
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }




        public void ToggleMenu(object sender, RoutedEventArgs e)
        {
            if (MenuPanel.Visibility == Visibility.Collapsed)
            {
                // Otvori meni
                MenuPanel.Visibility = Visibility.Visible;
                Debug.WriteLine("Menu opened");

                // Pokreni animaciju otvaranja menija (ako postoji)
                var openMenuStoryboard = FindResource("OpenMenuAnimation") as Storyboard;
                if (openMenuStoryboard != null)
                {
                    openMenuStoryboard.Begin();
                }
                else
                {
                    MessageBox.Show("OpenMenuAnimation not found.");
                }

                // Animiraj marginu glavnog sadržaja (povećaj levi margin)
                var marginOpenStoryboard = FindResource("MarginToMenuOpen") as Storyboard;
                if (marginOpenStoryboard != null)
                {
                    marginOpenStoryboard.Begin();
                }
                else
                {
                    MessageBox.Show("MarginToMenuOpen storyboard not found.");
                }
            }
            else
            {
                // Animiraj marginu glavnog sadržaja (vrati levi margin na 0)
                var marginClosedStoryboard = FindResource("MarginToMenuClosed") as Storyboard;
                if (marginClosedStoryboard != null)
                {
                    marginClosedStoryboard.Begin();
                }
                else
                {
                    MessageBox.Show("MarginToMenuClosed storyboard not found.");
                }

                // Pokreni animaciju zatvaranja menija
                var closeMenuStoryboard = FindResource("CloseMenuAnimation") as Storyboard;
                if (closeMenuStoryboard != null)
                {
                    closeMenuStoryboard.Completed += (s, ev) =>
                    {
                        MenuPanel.Visibility = Visibility.Collapsed;
                        Debug.WriteLine("Menu closed");
                    };
                    closeMenuStoryboard.Begin();
                }
                else
                {
                    MessageBox.Show("CloseMenuAnimation not found.");
                }
            }
        }
        private Button activeButton; // Prati koje dugme je trenutno aktivno
        private void SetActiveMenuItem(Button button)
        {
            if (activeButton != null)
            {
                activeButton.Tag = "False"; // Resetuj prethodno dugme
            }

            button.Tag = "True"; // Obilježi kliknuto dugme kao aktivno
            activeButton = button;
        }

        private async void MenuButton1_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(MenuItem1);
            if (userRole == "Zaposleni")
            {
                MainContentControl.Content = new AktuelnostiView(loggedInUserEmail, jezikSrpski);
            }
            else { MainContentControl.Content = new AktuelnostiView("empty", jezikSrpski); }
            if (trenutnaTema == Tema.Tamna || trenutnaTema == Tema.Zelena)
            {
                Application.Current.Resources["PrimaryTextBrush"] = new SolidColorBrush(Colors.White); // Tamna i zelena
            }
            else
            {
                Application.Current.Resources["PrimaryTextBrush"] = new SolidColorBrush(Colors.Black); // Svetla tema
            }
            // Ažuriraj boje popupa ako je već otvoren
            if (MainContentControl.Content is AktuelnostiView aktuelnostiView)
            {
                var border = aktuelnostiView.FindName("OglasPopupBorder") as Border;
                if (border != null)
                {
                    SolidColorBrush backgroundBrush;
                    SolidColorBrush borderBrush;
                    Color textColor;
                    Color grayTextColor;

                    // Postavljanje boja na osnovu aktivne teme
                    switch (trenutnaTema)
                    {
                        case Tema.Tamna:
                            backgroundBrush = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                            borderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62));
                            textColor = Colors.White;
                            grayTextColor = Colors.LightGray;
                            break;

                        case Tema.Svijetla:
                            backgroundBrush = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                            borderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                            textColor = Colors.Black;
                            grayTextColor = Colors.DarkGray;
                            break;

                        case Tema.Zelena:
                            backgroundBrush = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                            borderBrush = new SolidColorBrush(Color.FromRgb(150, 200, 150));
                            textColor = Colors.White;
                            grayTextColor = Colors.Green;
                            break;

                        default:
                            backgroundBrush = Brushes.Gray;
                            borderBrush = Brushes.DarkGray;
                            textColor = Colors.Black;
                            grayTextColor = Colors.Gray;
                            break;
                    }

                    border.Background = backgroundBrush;
                    border.BorderBrush = borderBrush;

                    // Postavi boje teksta
                    if (aktuelnostiView.FindName("AktuelnostiTitle") is TextBlock titleText)
                        titleText.Foreground = new SolidColorBrush(textColor);

                    if (aktuelnostiView.FindName("AktuelnostiLabel") is TextBlock labelText)
                        labelText.Foreground = new SolidColorBrush(textColor);

                    if (aktuelnostiView.FindName("AktuelnostiPopupCloseButton") is Button closeButton)
                    {
                        closeButton.Foreground = new SolidColorBrush(textColor);
                        closeButton.Background = Brushes.Transparent;
                    }

                    // ➕ Promena jezika
                    if (aktuelnostiView.FindName("BtnDodajOglas") is Button dugme)
                        dugme.Content = jezikSrpski ? "➕ Dodaj novi oglas" : "➕ Add new post";

                    if (aktuelnostiView.FindName("AktuelnostiNaslov") is TextBlock naslov)
                        naslov.Text = jezikSrpski ? "Aktuelnosti" : "News";
                    // Pristupamo ItemsControl-u koji sadrži oglase
                    if (aktuelnostiView.FindName("OglasiWrapPanel") is ItemsControl itemsControl)
                    {
                        foreach (var item in itemsControl.Items)
                        {
                            await Task.Delay(10);
                            // Uzmi generisani container za svaku stavku
                            var container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                            if (container != null)
                            {
                                // Pretražujemo vizuelno stablo da pronađemo dugme
                                var buttonOtvori = FindChild<Button>(container, "OtvoriPDFButton");
                                if (buttonOtvori != null)
                                {
                                    buttonOtvori.Content = jezikSrpski ? "Otvori PDF" : "Open PDF";
                                }

                                var buttonObrisi = FindChild<Button>(container, "Obrisi_Button");
                                if (buttonObrisi != null)
                                {
                                    buttonObrisi.Content = jezikSrpski ? "Obriši" : "Delete";
                                }
                            }
                        }
                    }
                    // Pristup elementima unutar popup-a
                    if (aktuelnostiView.FindName("AktuelnostiTitle") is TextBlock title)
                        title.Text = jezikSrpski? "Aktuelnosti" : "News";
                    if (aktuelnostiView.FindName("AktuelnostiLabel") is TextBlock label)
                        label.Text = jezikSrpski ? "Tekst oglasa:" : "Text for post:";
                    if (aktuelnostiView.FindName("OdaberiPDFButton") is Button pdfButton)
                        pdfButton.Content = jezikSrpski ? "🡅 Odaberi PDF" : "🡅 Select PDF";
                    if (aktuelnostiView.FindName("PotvrdiButton") is Button confirmButton)
                        confirmButton.Content = jezikSrpski ? "Potvrdi" : "Confirm";
                    if (aktuelnostiView.FindName("PDFFileNameText") is TextBlock pdfText)
                        pdfText.Text = jezikSrpski ? "Nijedan fajl nije odabran" : "No file selected";
                }
            }
        }
        private async void MenuButton2_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(MenuItem2);

            if (userRole == "Zaposleni")
            {
                MainContentControl.Content = new KonkursiView(loggedInUserEmail, jezikSrpski);
            }
            else { MainContentControl.Content = new KonkursiView("empty", jezikSrpski); }

            // Ažuriraj boje popupa ako je već otvoren
            if (MainContentControl.Content is KonkursiView konkursiView)
            {
                var border = konkursiView.FindName("KonkursiPopupBorder") as Border;
                if (border != null)
                {
                    SolidColorBrush backgroundBrush;
                    SolidColorBrush borderBrush;
                    Color textColor;
                    Color grayTextColor;

                    // Postavljanje boja na osnovu aktivne teme
                    switch (trenutnaTema)
                    {
                        case Tema.Tamna:
                            backgroundBrush = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                            borderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62));
                            textColor = Colors.White;
                            grayTextColor = Colors.LightGray;
                            break;

                        case Tema.Svijetla:
                            backgroundBrush = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                            borderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                            textColor = Colors.Black;
                            grayTextColor = Colors.DarkGray;
                            break;

                        case Tema.Zelena:
                            backgroundBrush = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                            borderBrush = new SolidColorBrush(Color.FromRgb(150, 200, 150));
                            textColor = Colors.White;
                            grayTextColor = Colors.Green;
                            break;

                        default:
                            backgroundBrush = Brushes.Gray;
                            borderBrush = Brushes.DarkGray;
                            textColor = Colors.Black;
                            grayTextColor = Colors.Gray;
                            break;
                    }

                    border.Background = backgroundBrush;
                    border.BorderBrush = borderBrush;

                    // Postavi boje teksta
                    if (konkursiView.FindName("KonkursiTitle") is TextBlock titleText)
                        titleText.Foreground = new SolidColorBrush(textColor);

                    if (konkursiView.FindName("KonkursiLabel1") is TextBlock labelText1)
                        labelText1.Foreground = new SolidColorBrush(textColor);

                    if (konkursiView.FindName("KonkursiLabel2") is TextBlock labelText2)
                        labelText2.Foreground = new SolidColorBrush(textColor);

                    if (konkursiView.FindName("KonkursiPopupCloseButton") is Button closeButton)
                    {
                        closeButton.Foreground = new SolidColorBrush(textColor);
                        closeButton.Background = Brushes.Transparent;
                    }

                    // ➕ Promena jezika - naslov i dugme
                    if (konkursiView.FindName("BtnDodajOglas") is Button dugme)
                        dugme.Content = jezikSrpski ? "➕ Dodaj novi oglas" : "➕ Add new post";

                    if (konkursiView.FindName("Konkursi_Title") is TextBlock naslov)
                        naslov.Text = jezikSrpski ? "Konkursi" : "Accommodation applications";

                    // 🎯 Pristup ItemsControl-u koji sadrži konkurse
                    if (konkursiView.FindName("KonkursiWrapPanel") is ItemsControl itemsControl)
                    {
                        await Task.Delay(10); // Daj vremena za renderovanje

                        itemsControl.Dispatcher.InvokeAsync(() =>
                        {
                            foreach (var item in itemsControl.Items)
                            {
                                var container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                                if (container != null)
                                {
                                    var buttonOtvori = FindChild<Button>(container, "OtvoriPDFButton_Konkurs");
                                    if (buttonOtvori != null)
                                    {
                                        buttonOtvori.Content = jezikSrpski ? "Otvori PDF" : "Open PDF";
                                    }

                                    var buttonObrisi = FindChild<Button>(container, "Obrisi_Button_Konkurs");
                                    if (buttonObrisi != null)
                                    {
                                        buttonObrisi.Content = jezikSrpski ? "Obriši" : "Delete";
                                    }
                                }
                            }
                        });
                    }

                    // 🧾 Prevod elemenata u popup-u za dodavanje konkursa
                    if (konkursiView.FindName("KonkursiTitle") is TextBlock title)
                        title.Text = jezikSrpski ? "Konkursi" : "Accommodation applications";

                    if (konkursiView.FindName("KonkursiLabel1") is TextBlock label1)
                        label1.Text = jezikSrpski ? "Naslov konkursa:" : "Accommodation applications title:";

                    if (konkursiView.FindName("KonkursiLabel2") is TextBlock label2)
                        label2.Text = jezikSrpski ? "Tekst konkursa:" : "Accommodation applications text:";

                    if (konkursiView.FindName("OdaberiPDFButton") is Button pdfButton)
                        pdfButton.Content = jezikSrpski ? "🡅 Odaberi PDF" : "🡅 Select PDF";

                    if (konkursiView.FindName("PotvrdiButton") is Button confirmButton)
                        confirmButton.Content = jezikSrpski ? "Potvrdi" : "Confirm";

                    if (konkursiView.FindName("PDFFileNameText") is TextBlock pdfText)
                        pdfText.Text = jezikSrpski ? "Nijedan fajl nije odabran" : "No file selected";
                }
            }
        }
        private async void MenuButton3_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(MenuItem3);

            if (userRole == "Zaposleni")
            {
                MainContentControl.Content = new StudentskiSmjestajView(loggedInUserEmail, jezikSrpski);
            }
            else { MainContentControl.Content = new StudentskiSmjestajView("empty", jezikSrpski); }

            // Ažuriraj boje popupa ako je već otvoren
            if (MainContentControl.Content is StudentskiSmjestajView studentskismjestajView)
            {
                var border = studentskismjestajView.FindName("StudentskiSmjestajPopupBorder") as Border;
                if (border != null)
                {
                    SolidColorBrush backgroundBrush;
                    SolidColorBrush borderBrush;
                    Color textColor;
                    Color grayTextColor;

                    // Postavljanje boja na osnovu aktivne teme
                    switch (trenutnaTema)
                    {
                        case Tema.Tamna:
                            backgroundBrush = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                            borderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62));
                            textColor = Colors.White;
                            grayTextColor = Colors.LightGray;
                            break;

                        case Tema.Svijetla:
                            backgroundBrush = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                            borderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                            textColor = Colors.Black;
                            grayTextColor = Colors.DarkGray;
                            break;

                        case Tema.Zelena:
                            backgroundBrush = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                            borderBrush = new SolidColorBrush(Color.FromRgb(150, 200, 150));
                            textColor = Colors.White;
                            grayTextColor = Colors.Green;
                            break;

                        default:
                            backgroundBrush = Brushes.Gray;
                            borderBrush = Brushes.DarkGray;
                            textColor = Colors.Black;
                            grayTextColor = Colors.Gray;
                            break;
                    }

                    border.Background = backgroundBrush;
                    border.BorderBrush = borderBrush;

                    // Postavi boje teksta
                    if (studentskismjestajView.FindName("StudentskiSmjestajTitle") is TextBlock titleText)
                        titleText.Foreground = new SolidColorBrush(textColor);

                    if (studentskismjestajView.FindName("StudentskiSmjestajLabel1") is TextBlock labelText1)
                        labelText1.Foreground = new SolidColorBrush(textColor);

                    if (studentskismjestajView.FindName("StudentskiSmjestajLabel2") is TextBlock labelText2)
                        labelText2.Foreground = new SolidColorBrush(textColor);

                    if (studentskismjestajView.FindName("StudentskiSmjestajPopupCloseButton") is Button closeButton)
                    {
                        closeButton.Foreground = new SolidColorBrush(textColor);
                        closeButton.Background = Brushes.Transparent;
                    }

                    // ➕ Promena jezika - naslov i dugme
                    if (studentskismjestajView.FindName("BtnDodajOglas") is Button dugme)
                        dugme.Content = jezikSrpski ? "➕ Dodaj novi oglas" : "➕ Add new post";

                    if (studentskismjestajView.FindName("Smjestaj_Title") is TextBlock naslov)
                        naslov.Text = jezikSrpski ? "Studentski smještaj" : "Student accommodation";

                    // 🎯 Pristup ItemsControl-u koji sadrži prijave
                    if (studentskismjestajView.FindName("StudentskiSmjestajWrapPanel") is ItemsControl itemsControl)
                    {
                        await Task.Delay(10); // Daj vremena za renderovanje

                        itemsControl.Dispatcher.InvokeAsync(() =>
                        {
                            foreach (var item in itemsControl.Items)
                            {
                                var container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                                if (container != null)
                                {
                                    var buttonObrisi = FindChild<Button>(container, "Obrisi_Button_Smjestaj");
                                    if (buttonObrisi != null)
                                        buttonObrisi.Content = jezikSrpski ? "Obriši" : "Delete";
                                }
                            }
                        });
                    }

                    // 🧾 Prevod elemenata u popup-u za dodavanje prijave
                    if (studentskismjestajView.FindName("StudentskiSmjestajTitle") is TextBlock title)
                        title.Text = jezikSrpski ? "Studentski smještaj" : "Student accommodation";

                    if (studentskismjestajView.FindName("StudentskiSmjestajLabel1") is TextBlock label1)
                        label1.Text = jezikSrpski ? "Naslov oglasa:" : "Post title:";

                    if (studentskismjestajView.FindName("StudentskiSmjestajLabel2") is TextBlock label2)
                        label2.Text = jezikSrpski ? "Tekst oglasa:" : "Post text:";

                    if (studentskismjestajView.FindName("PotvrdiButton") is Button confirmButton)
                        confirmButton.Content = jezikSrpski ? "Potvrdi" : "Confirm";
                }
            }
        }
        private async void MenuButton4_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(MenuItem4);

            if (userRole == "Zaposleni")
            {
                MainContentControl.Content = new MenzaView(loggedInUserEmail, jezikSrpski);
            }
            else { MainContentControl.Content = new MenzaView("empty", jezikSrpski); }

            // Ažuriraj boje popupa ako je već otvoren
            if (MainContentControl.Content is MenzaView menzaView)
            {
                var border = menzaView.FindName("MenzaPopupBorder") as Border;
                if (border != null)
                {
                    SolidColorBrush backgroundBrush;
                    SolidColorBrush borderBrush;
                    Color textColor;
                    Color grayTextColor;

                    // Postavljanje boja na osnovu aktivne teme
                    switch (trenutnaTema)
                    {
                        case Tema.Tamna:
                            backgroundBrush = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                            borderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62));
                            textColor = Colors.White;
                            grayTextColor = Colors.LightGray;
                            break;

                        case Tema.Svijetla:
                            backgroundBrush = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                            borderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                            textColor = Colors.Black;
                            grayTextColor = Colors.DarkGray;
                            break;

                        case Tema.Zelena:
                            backgroundBrush = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                            borderBrush = new SolidColorBrush(Color.FromRgb(150, 200, 150));
                            textColor = Colors.White;
                            grayTextColor = Colors.Green;
                            break;

                        default:
                            backgroundBrush = Brushes.Gray;
                            borderBrush = Brushes.DarkGray;
                            textColor = Colors.Black;
                            grayTextColor = Colors.Gray;
                            break;
                    }

                    border.Background = backgroundBrush;
                    border.BorderBrush = borderBrush;

                    // Postavi boje teksta
                    if (menzaView.FindName("MenzaTitle") is TextBlock titleText)
                        titleText.Foreground = new SolidColorBrush(textColor);

                    if (menzaView.FindName("MenzaLabel1") is TextBlock labelText1)
                        labelText1.Foreground = new SolidColorBrush(textColor);

                    if (menzaView.FindName("MenzaLabel2") is TextBlock labelText2)
                        labelText2.Foreground = new SolidColorBrush(textColor);

                    if (menzaView.FindName("MenzaLabel3") is TextBlock labelText3)
                        labelText3.Foreground = new SolidColorBrush(textColor);

                    if (menzaView.FindName("MenzaLabel4") is TextBlock labelText4)
                        labelText4.Foreground = new SolidColorBrush(textColor);

                    if (menzaView.FindName("MenzaLabel5") is TextBlock labelText5)
                        labelText5.Foreground = new SolidColorBrush(textColor);

                    if (menzaView.FindName("MenzaLabel6") is TextBlock labelText6)
                        labelText6.Foreground = new SolidColorBrush(textColor);

                    if (menzaView.FindName("MenzaLabel7") is TextBlock labelText7)
                        labelText7.Foreground = new SolidColorBrush(textColor);

                    if (menzaView.FindName("MenzaPopupCloseButton") is Button closeButton)
                    {
                        closeButton.Foreground = new SolidColorBrush(textColor);
                        closeButton.Background = Brushes.Transparent;
                    }

                    if (menzaView.FindName("BtnDodajOglas") is Button dugme)
                        dugme.Content = jezikSrpski ? "➕ Dodaj novi oglas" : "➕ Add new post";

                    if (menzaView.FindName("MenzaNaslov") is TextBlock naslov)
                        naslov.Text = jezikSrpski ? "'Menza'" : "Students restaurant";

                    // 🎯 Pristup ItemsControl-u koji sadrži menije
                    if (menzaView.FindName("MenzaWrapPanel") is ItemsControl itemsControl)
                    {
                        await Task.Delay(20); // Daj vremena za renderovanje

                        itemsControl.Dispatcher.InvokeAsync(() =>
                        {
                            foreach (var item in itemsControl.Items)
                            {
                                var container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                                if (container != null)
                                {
                                    var buttonObrisi = FindChild<Button>(container, "Obrisi_Button_Menza");
                                    if (buttonObrisi != null)
                                        buttonObrisi.Content = jezikSrpski ? "Obriši" : "Delete";
                                }
                                // Labela: Cijena doručka
                                var label1 = FindChild<TextBlock>(container, "MenzaWrapPanel_Label1");
                                if (label1 != null)
                                    label1.Text = jezikSrpski ? "Cijena doručka: " : "Breakfast price: ";
                                
                                // Labela: Cijena ručka
                                var label2 = FindChild<TextBlock>(container, "MenzaWrapPanel_Label2");
                                if (label2 != null)
                                    label2.Text = jezikSrpski ? "Cijena ručka: " : "Lunch price: ";

                                // Labela: Cijena večere
                                var label3 = FindChild<TextBlock>(container, "MenzaWrapPanel_Label3");
                                if (label3 != null)
                                    label3.Text = jezikSrpski ? "Cijena večere: " : "Dinner price: ";

                                // Labela: Termin doručka
                                var label4 = FindChild<TextBlock>(container, "MenzaWrapPanel_Label4");
                                if (label4 != null)
                                    label4.Text = jezikSrpski ? "Termin doručka: " : "Breakfast time: ";

                                // Labela: Termin ručka
                                var label5 = FindChild<TextBlock>(container, "MenzaWrapPanel_Label5");
                                if (label5 != null)
                                    label5.Text = jezikSrpski ? "Termin ručka: " : "Lunch time: ";

                                // Labela: Termin večere
                                var label6 = FindChild<TextBlock>(container, "MenzaWrapPanel_Label6");
                                if (label6 != null)
                                    label6.Text = jezikSrpski ? "Termin večere: " : "Dinner time: ";
                            }
                        });
                    }

                    // 🧾 Prevod elemenata u popup-u za dodavanje menija
                    if (menzaView.FindName("MenzaTitle") is TextBlock popupTitle)
                        popupTitle.Text = jezikSrpski ? "'Menza'" : "Students restaurant";

                    if (menzaView.FindName("MenzaLabel1") is TextBlock label1)
                        label1.Text = jezikSrpski ? "Tekst oglasa:" : "Post text:";

                    if (menzaView.FindName("MenzaLabel2") is TextBlock label2)
                        label2.Text = jezikSrpski ? "Cijena doručka:" : "Breakfast price:";

                    if (menzaView.FindName("MenzaLabel3") is TextBlock label3)
                        label3.Text = jezikSrpski ? "Cijena ručka:" : "Lunch price:";

                    if (menzaView.FindName("MenzaLabel4") is TextBlock label4)
                        label4.Text = jezikSrpski ? "Cijena večere:" : "Dinner price:";

                    if (menzaView.FindName("MenzaLabel5") is TextBlock label5)
                        label5.Text = jezikSrpski ? "Termin doručka:" : "Breakfast time:";

                    if (menzaView.FindName("MenzaLabel6") is TextBlock label6)
                        label6.Text = jezikSrpski ? "Termin ručka:" : "Lunch time:";

                    if (menzaView.FindName("MenzaLabel7") is TextBlock label7)
                        label7.Text = jezikSrpski ? "Termin večere:" : "Dinner time:";

                    if (menzaView.FindName("PotvrdiButton") is Button confirmButton)
                        confirmButton.Content = jezikSrpski ? "Potvrdi" : "Confirm";
                }
            }
        }
        private async void MenuButton5_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(MenuItem5);
            if (userRole == "Zaposleni")
            {
                MainContentControl.Content = new KontaktView(loggedInUserEmail, jezikSrpski);
            }
            else { MainContentControl.Content = new KontaktView("empty", jezikSrpski); }

            // Ažuriraj boje popupa ako je već otvoren
            if (MainContentControl.Content is KontaktView kontaktView)
            {
                var border = kontaktView.FindName("KontaktPopupBorder") as Border;
                if (border != null)
                {
                    SolidColorBrush backgroundBrush;
                    SolidColorBrush borderBrush;
                    Color textColor;
                    Color grayTextColor;

                    // Postavljanje boja na osnovu aktivne teme
                    switch (trenutnaTema)
                    {
                        case Tema.Tamna:
                            backgroundBrush = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                            borderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62));
                            textColor = Colors.White;
                            grayTextColor = Colors.LightGray;
                            break;

                        case Tema.Svijetla:
                            backgroundBrush = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                            borderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                            textColor = Colors.Black;
                            grayTextColor = Colors.DarkGray;
                            break;

                        case Tema.Zelena:
                            backgroundBrush = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                            borderBrush = new SolidColorBrush(Color.FromRgb(150, 200, 150));
                            textColor = Colors.White;
                            grayTextColor = Colors.Green;
                            break;

                        default:
                            backgroundBrush = Brushes.Gray;
                            borderBrush = Brushes.DarkGray;
                            textColor = Colors.Black;
                            grayTextColor = Colors.Gray;
                            break;
                    }

                    border.Background = backgroundBrush;
                    border.BorderBrush = borderBrush;

                    // Postavi boje teksta
                    if (kontaktView.FindName("KontaktTitle") is TextBlock titleText)
                        titleText.Foreground = new SolidColorBrush(textColor);

                    if (kontaktView.FindName("KontaktLabel1") is TextBlock labelText1)
                        labelText1.Foreground = new SolidColorBrush(textColor);

                    if (kontaktView.FindName("KontaktLabel2") is TextBlock labelText2)
                        labelText2.Foreground = new SolidColorBrush(textColor);

                    if (kontaktView.FindName("KontaktLabel3") is TextBlock labelText3)
                        labelText3.Foreground = new SolidColorBrush(textColor);

                    if (kontaktView.FindName("KontaktLabel4") is TextBlock labelText4)
                        labelText4.Foreground = new SolidColorBrush(textColor);

                    if (kontaktView.FindName("KontaktLabel5") is TextBlock labelText5)
                        labelText5.Foreground = new SolidColorBrush(textColor);

                    if (kontaktView.FindName("KontaktLabel6") is TextBlock labelText6)
                        labelText6.Foreground = new SolidColorBrush(textColor);

                    if (kontaktView.FindName("KontaktLabel7") is TextBlock labelText7)
                        labelText7.Foreground = new SolidColorBrush(textColor);

                    if (kontaktView.FindName("KontaktLabel8") is TextBlock labelText8)
                        labelText8.Foreground = new SolidColorBrush(textColor);

                    if (kontaktView.FindName("KontaktPopupCloseButton") is Button closeButton)
                    {
                        closeButton.Foreground = new SolidColorBrush(textColor);
                        closeButton.Background = Brushes.Transparent;
                    }
                    if (kontaktView.FindName("BtnDodajKontakt") is Button dugme)
                        dugme.Content = jezikSrpski ? "➕ Dodaj kontakte" : "➕ Add new post";

                    if (kontaktView.FindName("KontaktiNaslov") is TextBlock naslov)
                        naslov.Text = jezikSrpski ? "Kontakti" : "Contacts";

                    // 🎯 Pristup ItemsControl-u koji sadrži kontakte
                    if (kontaktView.FindName("KontaktWrapPanel") is ItemsControl itemsControl)
                    {
                        await Task.Delay(10); // Daj vremena za renderovanje

                        itemsControl.Dispatcher.InvokeAsync(() =>
                        {
                            foreach (var item in itemsControl.Items)
                            {
                                var container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                                if (container != null)
                                {
                                    var buttonObrisi = FindChild<Button>(container, "KontaktWrapPanel_Delete");
                                    if (buttonObrisi != null)
                                        buttonObrisi.Content = jezikSrpski ? "Obriši" : "Delete";

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
                        });
                    }

                    // 🧾 Prevod elemenata u popup-u za dodavanje kontakta
                    if (kontaktView.FindName("KontaktTitle") is TextBlock popupTitle)
                        popupTitle.Text = jezikSrpski ? "Kontakti" : "Contacts";

                    if (kontaktView.FindName("KontaktLabel1") is TextBlock label1)
                        label1.Text = jezikSrpski ? "Tekst oglasa:" : "Post text:";

                    if (kontaktView.FindName("KontaktLabel2") is TextBlock label2)
                        label2.Text = jezikSrpski ? "Adresa 1:" : "Address 1:";

                    if (kontaktView.FindName("KontaktLabel3") is TextBlock label3)
                        label3.Text = jezikSrpski ? "Adresa 2:" : "Address 2:";

                    if (kontaktView.FindName("KontaktLabel4") is TextBlock label4)
                        label4.Text = jezikSrpski ? "Broj telefona 1:" : "Phone number 1:";

                    if (kontaktView.FindName("KontaktLabel5") is TextBlock label5)
                        label5.Text = jezikSrpski ? "Broj telefona 2:" : "Phone number 2:";

                    if (kontaktView.FindName("PotvrdiButton") is Button confirmButton)
                        confirmButton.Content = jezikSrpski ? "Potvrdi" : "Confirm";

                }
            }
        }

        private void MenuButton6_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(MenuItem6);

            if (userRole == "Admin")
            {
                MainContentControl.Content = new UpravljajZaposlenimaView(jezikSrpski);
            }

            if (MainContentControl.Content is UpravljajZaposlenimaView upravljajzaposlenimaView)
            {
                var border = upravljajzaposlenimaView.FindName("DodajZaposlenogPopupBorder") as Border;
                if (border != null)
                {
                    SolidColorBrush backgroundBrush;
                    SolidColorBrush borderBrush;
                    Color textColor;
                    Color grayTextColor;

                    // Postavljanje boja na osnovu aktivne teme
                    switch (trenutnaTema)
                    {
                        case Tema.Tamna:
                            backgroundBrush = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                            borderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62));
                            textColor = Colors.White;
                            grayTextColor = Colors.LightGray;
                            break;

                        case Tema.Svijetla:
                            backgroundBrush = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                            borderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                            textColor = Colors.Black;
                            grayTextColor = Colors.DarkGray;
                            break;

                        case Tema.Zelena:
                            backgroundBrush = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                            borderBrush = new SolidColorBrush(Color.FromRgb(150, 200, 150));
                            textColor = Colors.White;
                            grayTextColor = Colors.Green;
                            break;

                        default:
                            backgroundBrush = Brushes.Gray;
                            borderBrush = Brushes.DarkGray;
                            textColor = Colors.Black;
                            grayTextColor = Colors.Gray;
                            break;
                    }

                    border.Background = backgroundBrush;
                    border.BorderBrush = borderBrush;

                    // Postavi boje teksta
                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogTitle") is TextBlock titleText)
                        titleText.Foreground = new SolidColorBrush(textColor);

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel1") is TextBlock labelText1)
                        labelText1.Foreground = new SolidColorBrush(textColor);

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel2") is TextBlock labelText2)
                        labelText2.Foreground = new SolidColorBrush(textColor);

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel3") is TextBlock labelText3)
                        labelText3.Foreground = new SolidColorBrush(textColor);

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel4") is TextBlock labelText4)
                        labelText4.Foreground = new SolidColorBrush(textColor);

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel5") is TextBlock labelText5)
                        labelText5.Foreground = new SolidColorBrush(textColor);

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel6") is TextBlock labelText6)
                        labelText6.Foreground = new SolidColorBrush(textColor);

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel7") is TextBlock labelText7)
                        labelText7.Foreground = new SolidColorBrush(textColor);

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel8") is TextBlock labelText8)
                        labelText8.Foreground = new SolidColorBrush(textColor);

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel9") is TextBlock labelText9)
                        labelText9.Foreground = new SolidColorBrush(textColor);

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel10") is TextBlock labelText10)
                        labelText10.Foreground = new SolidColorBrush(textColor);

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel11") is TextBlock labelText11)
                        labelText11.Foreground = new SolidColorBrush(textColor);

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel12") is TextBlock labelText12)
                        labelText12.Foreground = new SolidColorBrush(textColor);

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel13") is TextBlock labelText13)
                        labelText13.Foreground = new SolidColorBrush(textColor);

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel14") is TextBlock labelText14)
                        labelText14.Foreground = new SolidColorBrush(textColor);

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogPopupCloseButton") is Button closeButton)
                    {
                        closeButton.Foreground = new SolidColorBrush(textColor);
                        closeButton.Background = Brushes.Transparent;
                    }

                    if (upravljajzaposlenimaView.FindName("BtnDodajZaposlenog") is Button dugme)
                        dugme.Content = jezikSrpski ? "➕ Dodaj zaposlenog" : "➕ Add employee";

                    if (upravljajzaposlenimaView.FindName("UpravljajZaposlenimaNaslov") is TextBlock naslov)
                        naslov.Text = jezikSrpski ? "Upravljaj zaposlenima" : "Manage employees";

                    // 🧾 Pristup elementima u popup-u
                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogTitle") is TextBlock popupTitle)
                        popupTitle.Text = jezikSrpski ? "Unesite podatke:" : "Enter details:";

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel1") is TextBlock label1)
                        label1.Text = jezikSrpski ? "Ime:" : "First Name:";

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel2") is TextBlock label2)
                        label2.Text = jezikSrpski ? "Prezime:" : "Last Name:";

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel3") is TextBlock label3)
                        label3.Text = jezikSrpski ? "Email:" : "Email:";

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel4") is TextBlock label4)
                        label4.Text = jezikSrpski ? "Korisničko Ime:" : "Username:";

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel5") is TextBlock label5)
                        label5.Text = jezikSrpski ? "Lozinka:" : "Password:";

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel6") is TextBlock label6)
                        label6.Text = jezikSrpski ? "Broj Telefona:" : "Phone Number:";

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel7") is TextBlock label7)
                        label7.Text = jezikSrpski ? "JMBG:" : "ID Number:";

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel8") is TextBlock label8)
                        label8.Text = jezikSrpski ? "Zvanje:" : "Title:";

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel9") is TextBlock label9)
                        label9.Text = jezikSrpski ? "Tema:" : "Theme:";

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel10") is TextBlock label10)
                        label10.Text = jezikSrpski ? "Jezik:" : "Language:";

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel11") is TextBlock label11)
                        label11.Text = jezikSrpski ? "Datum Rođenja:" : "Birth Date:";

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel12") is TextBlock label12)
                        label12.Text = jezikSrpski ? "Datum Zaposlenja:" : "Hire Date:";

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel13") is TextBlock label13)
                        label13.Text = jezikSrpski ? "Paviljon:" : "Pavilion:";

                    if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel14") is TextBlock label14)
                        label14.Text = jezikSrpski ? "Adresa:" : "Address:";

                    if (upravljajzaposlenimaView.FindName("PotvrdiButton") is Button confirmButton)
                        confirmButton.Content = jezikSrpski ? "Potvrdi" : "Confirm";

                    // 🎨 ComboBox vrednosti
                    if (upravljajzaposlenimaView.FindName("TemaComboBox") is ComboBox temaCombo)
                    {
                        ((ComboBoxItem)temaCombo.Items[0]).Content = jezikSrpski ? "Tamna" : "Dark";
                        ((ComboBoxItem)temaCombo.Items[1]).Content = jezikSrpski ? "Svijetla" : "Light";
                        ((ComboBoxItem)temaCombo.Items[2]).Content = jezikSrpski ? "Zelena" : "Green";
                    }

                    if (upravljajzaposlenimaView.FindName("JezikComboBox") is ComboBox jezikCombo)
                    {
                        ((ComboBoxItem)jezikCombo.Items[0]).Content = jezikSrpski ? "Srpski" : "Serbian";
                        ((ComboBoxItem)jezikCombo.Items[1]).Content = jezikSrpski ? "Engleski" : "English";
                    }

                    if (upravljajzaposlenimaView.FindName("PaviljonComboBox") is ComboBox paviljonCombo)
                    {
                        ((ComboBoxItem)paviljonCombo.Items[0]).Content = jezikSrpski ? "Paviljon 1" : "Pavilion 1";
                        ((ComboBoxItem)paviljonCombo.Items[1]).Content = jezikSrpski ? "Paviljon 2" : "Pavilion 2";
                        ((ComboBoxItem)paviljonCombo.Items[2]).Content = jezikSrpski ? "Paviljon 3" : "Pavilion 3";
                        ((ComboBoxItem)paviljonCombo.Items[3]).Content = jezikSrpski ? "Paviljon 4" : "Pavilion 4";
                    }

                    // 📋 DataGrid kolone
                    if (upravljajzaposlenimaView.FindName("ZaposleniDataGrid") is DataGrid dataGrid)
                    {
                        foreach (var column in dataGrid.Columns)
                        {
                            if (column.Header.ToString() == "OsobaID") column.Header = jezikSrpski ? "OsobaID" : "PersonID";
                            if (column.Header.ToString() == "Ime") column.Header = jezikSrpski ? "Ime" : "First name";
                            if (column.Header.ToString() == "Prezime") column.Header = jezikSrpski ? "Prezime" : "Last name";
                            if (column.Header.ToString() == "Korisničko ime") column.Header = jezikSrpski ? "Korisničko ime" : "Username";
                            if (column.Header.ToString() == "Email") column.Header = jezikSrpski ? "Email" : "Email";
                            if (column.Header.ToString() == "Šifra") column.Header = jezikSrpski ? "Šifra" : "Password";
                            if (column.Header.ToString() == "Datum zaposlenja") column.Header = jezikSrpski ? "Datum zaposlenja" : "Hire date";
                            if (column.Header.ToString() == "Paviljon") column.Header = jezikSrpski ? "Paviljon" : "Pavilion";
                            if (column.Header.ToString() == "Telefon") column.Header = jezikSrpski ? "Telefon" : "Phone";
                            if (column.Header.ToString() == "Datum rođenja") column.Header = jezikSrpski ? "Datum rođenja" : "Birth date";
                            if (column.Header.ToString() == "JMBG") column.Header = jezikSrpski ? "JMBG" : "ID Number";
                            if (column.Header.ToString() == "Adresa") column.Header = jezikSrpski ? "Adresa" : "Address";
                            if (column.Header.ToString() == "Zvanje") column.Header = jezikSrpski ? "Zvanje" : "Title";
                            if (column.Header.ToString() == "Tema") column.Header = jezikSrpski ? "Tema" : "Theme";
                            if (column.Header.ToString() == "Jezik") column.Header = jezikSrpski ? "Jezik" : "Language";
                            if (column.Header.ToString() == "Uredi") column.Header = jezikSrpski ? "Uredi" : "Edit";
                            if (column.Header.ToString() == "Obriši") column.Header = jezikSrpski ? "Obriši" : "Delete";
                        }
                    }

                    // 🔐 Polje za lozinku
                    if (upravljajzaposlenimaView.FindName("textPassword") is TextBlock passwordHint)
                        passwordHint.Text = jezikSrpski ? "Nova lozinka" : "New password";

                    // 💾 Dugme za čuvanje
                    if (upravljajzaposlenimaView.FindName("SaveButton") is Button saveButton)
                        saveButton.Content = jezikSrpski ? "Sačuvaj izmjene" : "Save Changes";
                }
            }
        }

        private async void MenuButton7_Click(object sender, RoutedEventArgs e)
        {
            SetActiveMenuItem(MenuItem7);
            if (userRole == "Zaposleni" || userRole == "Admin")
            {
                MainContentControl.Content = new UputstvoView(userRole, jezikSrpski);
            }
           
            else { MainContentControl.Content = new UputstvoView("empty", jezikSrpski); }

            
        }

        private async void SetInitialView()
        {
            // Postavi prvo dugme kao aktivno
            if (userRole == "Admin")
            {
                SetActiveMenuItem(MenuItem6);
                MainContentControl.Content = new UpravljajZaposlenimaView(jezikSrpski);

                // Ažuriraj boje popupa ako je već otvoren
                if (MainContentControl.Content is UpravljajZaposlenimaView upravljajzaposlenimaView)
                {
                    var border = upravljajzaposlenimaView.FindName("DodajZaposlenogPopupBorder") as Border;
                    if (border != null)
                    {
                        SolidColorBrush backgroundBrush;
                        SolidColorBrush borderBrush;
                        Color textColor;
                        Color grayTextColor;

                        // Postavljanje boja na osnovu aktivne teme
                        switch (trenutnaTema)
                        {
                            case Tema.Tamna:
                                backgroundBrush = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                                borderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62));
                                textColor = Colors.White;
                                grayTextColor = Colors.LightGray;
                                break;

                            case Tema.Svijetla:
                                backgroundBrush = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                                borderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                                textColor = Colors.Black;
                                grayTextColor = Colors.DarkGray;
                                break;

                            case Tema.Zelena:
                                backgroundBrush = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                                borderBrush = new SolidColorBrush(Color.FromRgb(150, 200, 150));
                                textColor = Colors.White;
                                grayTextColor = Colors.Green;
                                break;

                            default:
                                backgroundBrush = Brushes.Gray;
                                borderBrush = Brushes.DarkGray;
                                textColor = Colors.Black;
                                grayTextColor = Colors.Gray;
                                break;
                        }

                        border.Background = backgroundBrush;
                        border.BorderBrush = borderBrush;

                        if (upravljajzaposlenimaView.FindName("CheckBox") is CheckBox checkBox)
                            checkBox.Foreground = new SolidColorBrush(textColor);

                        // Postavi boje teksta
                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(textColor);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel1") is TextBlock labelText1)
                            labelText1.Foreground = new SolidColorBrush(textColor);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel2") is TextBlock labelText2)
                            labelText2.Foreground = new SolidColorBrush(textColor);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel3") is TextBlock labelText3)
                            labelText3.Foreground = new SolidColorBrush(textColor);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel4") is TextBlock labelText4)
                            labelText4.Foreground = new SolidColorBrush(textColor);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel5") is TextBlock labelText5)
                            labelText5.Foreground = new SolidColorBrush(textColor);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel6") is TextBlock labelText6)
                            labelText6.Foreground = new SolidColorBrush(textColor);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel7") is TextBlock labelText7)
                            labelText7.Foreground = new SolidColorBrush(textColor);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel8") is TextBlock labelText8)
                            labelText8.Foreground = new SolidColorBrush(textColor);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel9") is TextBlock labelText9)
                            labelText9.Foreground = new SolidColorBrush(textColor);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel10") is TextBlock labelText10)
                            labelText10.Foreground = new SolidColorBrush(textColor);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel11") is TextBlock labelText11)
                            labelText11.Foreground = new SolidColorBrush(textColor);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel12") is TextBlock labelText12)
                            labelText12.Foreground = new SolidColorBrush(textColor);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel13") is TextBlock labelText13)
                            labelText13.Foreground = new SolidColorBrush(textColor);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel14") is TextBlock labelText14)
                            labelText14.Foreground = new SolidColorBrush(textColor);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = new SolidColorBrush(textColor);
                            closeButton.Background = Brushes.Transparent;
                        }

                        if (upravljajzaposlenimaView.FindName("BtnDodajZaposlenog") is Button dugme)
                            dugme.Content = jezikSrpski ? "➕ Dodaj zaposlenog" : "➕ Add employee";

                        if (upravljajzaposlenimaView.FindName("UpravljajZaposlenimaNaslov") is TextBlock naslov)
                            naslov.Text = jezikSrpski ? "Upravljaj zaposlenima" : "Manage employees";

                        // 🧾 Pristup elementima u popup-u
                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogTitle") is TextBlock popupTitle)
                            popupTitle.Text = jezikSrpski ? "Unesite podatke:" : "Enter details:";

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel1") is TextBlock label1)
                            label1.Text = jezikSrpski ? "Ime:" : "First Name:";

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel2") is TextBlock label2)
                            label2.Text = jezikSrpski ? "Prezime:" : "Last Name:";

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel3") is TextBlock label3)
                            label3.Text = jezikSrpski ? "Email:" : "Email:";

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel4") is TextBlock label4)
                            label4.Text = jezikSrpski ? "Korisničko Ime:" : "Username:";

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel5") is TextBlock label5)
                            label5.Text = jezikSrpski ? "Lozinka:" : "Password:";

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel6") is TextBlock label6)
                            label6.Text = jezikSrpski ? "Broj Telefona:" : "Phone Number:";

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel7") is TextBlock label7)
                            label7.Text = jezikSrpski ? "JMBG:" : "ID Number:";

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel8") is TextBlock label8)
                            label8.Text = jezikSrpski ? "Zvanje:" : "Title:";

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel9") is TextBlock label9)
                            label9.Text = jezikSrpski ? "Tema:" : "Theme:";

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel10") is TextBlock label10)
                            label10.Text = jezikSrpski ? "Jezik:" : "Language:";

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel11") is TextBlock label11)
                            label11.Text = jezikSrpski ? "Datum Rođenja:" : "Birth Date:";

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel12") is TextBlock label12)
                            label12.Text = jezikSrpski ? "Datum Zaposlenja:" : "Hire Date:";

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel13") is TextBlock label13)
                            label13.Text = jezikSrpski ? "Paviljon:" : "Pavilion:";

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel14") is TextBlock label14)
                            label14.Text = jezikSrpski ? "Adresa:" : "Address:";

                        if (upravljajzaposlenimaView.FindName("PotvrdiButton") is Button confirmButton)
                            confirmButton.Content = jezikSrpski ? "Potvrdi" : "Confirm";

                        // 🎨 ComboBox vrednosti
                        if (upravljajzaposlenimaView.FindName("TemaComboBox") is ComboBox temaCombo)
                        {
                            ((ComboBoxItem)temaCombo.Items[0]).Content = jezikSrpski ? "Tamna" : "Dark";
                            ((ComboBoxItem)temaCombo.Items[1]).Content = jezikSrpski ? "Svijetla" : "Light";
                            ((ComboBoxItem)temaCombo.Items[2]).Content = jezikSrpski ? "Zelena" : "Green";
                        }

                        if (upravljajzaposlenimaView.FindName("JezikComboBox") is ComboBox jezikCombo)
                        {
                            ((ComboBoxItem)jezikCombo.Items[0]).Content = jezikSrpski ? "Srpski" : "Serbian";
                            ((ComboBoxItem)jezikCombo.Items[1]).Content = jezikSrpski ? "Engleski" : "English";
                        }

                        if (upravljajzaposlenimaView.FindName("PaviljonComboBox") is ComboBox paviljonCombo)
                        {
                            ((ComboBoxItem)paviljonCombo.Items[0]).Content = jezikSrpski ? "Paviljon 1" : "Pavilion 1";
                            ((ComboBoxItem)paviljonCombo.Items[1]).Content = jezikSrpski ? "Paviljon 2" : "Pavilion 2";
                            ((ComboBoxItem)paviljonCombo.Items[2]).Content = jezikSrpski ? "Paviljon 3" : "Pavilion 3";
                            ((ComboBoxItem)paviljonCombo.Items[3]).Content = jezikSrpski ? "Paviljon 4" : "Pavilion 4";
                        }

                        // 📋 DataGrid kolone
                        if (upravljajzaposlenimaView.FindName("ZaposleniDataGrid") is DataGrid dataGrid)
                        {
                            foreach (var column in dataGrid.Columns)
                            {
                                if (column.Header.ToString() == "OsobaID") column.Header = jezikSrpski ? "OsobaID" : "PersonID";
                                if (column.Header.ToString() == "Ime") column.Header = jezikSrpski ? "Ime" : "First name";
                                if (column.Header.ToString() == "Prezime") column.Header = jezikSrpski ? "Prezime" : "Last name";
                                if (column.Header.ToString() == "Korisničko ime") column.Header = jezikSrpski ? "Korisničko ime" : "Username";
                                if (column.Header.ToString() == "Email") column.Header = jezikSrpski ? "Email" : "Email";
                                if (column.Header.ToString() == "Šifra") column.Header = jezikSrpski ? "Šifra" : "Password";
                                if (column.Header.ToString() == "Datum zaposlenja") column.Header = jezikSrpski ? "Datum zaposlenja" : "Hire date";
                                if (column.Header.ToString() == "Paviljon") column.Header = jezikSrpski ? "Paviljon" : "Pavilion";
                                if (column.Header.ToString() == "Telefon") column.Header = jezikSrpski ? "Telefon" : "Phone";
                                if (column.Header.ToString() == "Datum rođenja") column.Header = jezikSrpski ? "Datum rođenja" : "Birth date";
                                if (column.Header.ToString() == "JMBG") column.Header = jezikSrpski ? "JMBG" : "ID Number";
                                if (column.Header.ToString() == "Adresa") column.Header = jezikSrpski ? "Adresa" : "Address";
                                if (column.Header.ToString() == "Zvanje") column.Header = jezikSrpski ? "Zvanje" : "Title";
                                if (column.Header.ToString() == "Tema") column.Header = jezikSrpski ? "Tema" : "Theme";
                                if (column.Header.ToString() == "Jezik") column.Header = jezikSrpski ? "Jezik" : "Language";
                                if (column.Header.ToString() == "Uredi") column.Header = jezikSrpski ? "Uredi" : "Edit";
                                if (column.Header.ToString() == "Obriši") column.Header = jezikSrpski ? "Obriši" : "Delete";
                            }
                        }

                        // 🔐 Polje za lozinku
                        if (upravljajzaposlenimaView.FindName("textPassword") is TextBlock passwordHint)
                            passwordHint.Text = jezikSrpski ? "Nova lozinka" : "New password";

                        // 💾 Dugme za čuvanje
                        if (upravljajzaposlenimaView.FindName("SaveButton") is Button saveButton)
                            saveButton.Content = jezikSrpski ? "Sačuvaj izmjene" : "Save Changes";

                    }
                }
            }
            else if (userRole == "Zaposleni")
            {
                SetActiveMenuItem(MenuItem1);
                MainContentControl.Content = new AktuelnostiView(loggedInUserEmail, jezikSrpski);
                // Ažuriraj boje popupa ako je već otvoren
                if (MainContentControl.Content is AktuelnostiView aktuelnostiView)
                {
                    var border = aktuelnostiView.FindName("OglasPopupBorder") as Border;
                    if (border != null)
                    {
                        SolidColorBrush backgroundBrush;
                        SolidColorBrush borderBrush;
                        Color textColor;
                        Color grayTextColor;

                        // Postavljanje boja na osnovu aktivne teme
                        switch (trenutnaTema)
                        {
                            case Tema.Tamna:
                                backgroundBrush = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                                borderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62));
                                textColor = Colors.White;
                                grayTextColor = Colors.LightGray;
                                break;

                            case Tema.Svijetla:
                                backgroundBrush = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                                borderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                                textColor = Colors.Black;
                                grayTextColor = Colors.DarkGray;
                                break;

                            case Tema.Zelena:
                                backgroundBrush = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                                borderBrush = new SolidColorBrush(Color.FromRgb(150, 200, 150));
                                textColor = Colors.White;
                                grayTextColor = Colors.Green;
                                break;

                            default:
                                backgroundBrush = Brushes.Gray;
                                borderBrush = Brushes.DarkGray;
                                textColor = Colors.Black;
                                grayTextColor = Colors.Gray;
                                break;
                        }

                        border.Background = backgroundBrush;
                        border.BorderBrush = borderBrush;

                        // Postavi boje teksta
                        if (aktuelnostiView.FindName("AktuelnostiTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(textColor);

                        if (aktuelnostiView.FindName("AktuelnostiLabel") is TextBlock labelText)
                            labelText.Foreground = new SolidColorBrush(textColor);

                        if (aktuelnostiView.FindName("AktuelnostiPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = new SolidColorBrush(textColor);
                            closeButton.Background = Brushes.Transparent;
                        }

                        // ➕ Promena jezika
                        if (aktuelnostiView.FindName("BtnDodajOglas") is Button dugme)
                            dugme.Content = jezikSrpski ? "➕ Dodaj novi oglas" : "➕ Add new post";

                        if (aktuelnostiView.FindName("AktuelnostiNaslov") is TextBlock naslov)
                            naslov.Text = jezikSrpski ? "Aktuelnosti" : "News";
                        // Pristupamo ItemsControl-u koji sadrži oglase
                        if (aktuelnostiView.FindName("OglasiWrapPanel") is ItemsControl itemsControl)
                        {
                            foreach (var item in itemsControl.Items)
                            {
                                await Task.Delay(10);
                                // Uzmi generisani container za svaku stavku
                                var container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                                if (container != null)
                                {
                                    // Pretražujemo vizuelno stablo da pronađemo dugme
                                    var buttonOtvori = FindChild<Button>(container, "OtvoriPDFButton");
                                    if (buttonOtvori != null)
                                    {
                                        buttonOtvori.Content = jezikSrpski ? "Otvori PDF" : "Open PDF";
                                    }

                                    var buttonObrisi = FindChild<Button>(container, "Obrisi_Button");
                                    if (buttonObrisi != null)
                                    {
                                        buttonObrisi.Content = jezikSrpski ? "Obriši" : "Delete";
                                    }
                                }
                            }
                        }
                        // Pristup elementima unutar popup-a
                        if (aktuelnostiView.FindName("AktuelnostiTitle") is TextBlock title)
                            title.Text = jezikSrpski ? "Aktuelnosti" : "News";
                        if (aktuelnostiView.FindName("AktuelnostiLabel") is TextBlock label)
                            label.Text = jezikSrpski ? "Tekst oglasa:" : "Text for post:";
                        if (aktuelnostiView.FindName("OdaberiPDFButton") is Button pdfButton)
                            pdfButton.Content = jezikSrpski ? "🡅 Odaberi PDF" : "🡅 Select PDF";
                        if (aktuelnostiView.FindName("PotvrdiButton") is Button confirmButton)
                            confirmButton.Content = jezikSrpski ? "Potvrdi" : "Confirm";
                        if (aktuelnostiView.FindName("PDFFileNameText") is TextBlock pdfText)
                            pdfText.Text = jezikSrpski ? "Nijedan fajl nije odabran" : "No file selected";
                    }
                }
            }
            else
            {
                SetActiveMenuItem(MenuItem1); MainContentControl.Content = new AktuelnostiView("empty");
                Application.Current.Resources["PrimaryTextBrush"] = new SolidColorBrush(Colors.White); // Tamna tema

            }

            //MainContentControl.Content = new AktuelnostiView(); // Početna strana
        }
    
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            // Prikazivanje poruke sa imenom dugmeta koje je kliknuto
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                MessageBox.Show($"Kliknuto dugme: {clickedButton.Content}", "Informacija", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

     

        private void ToggleTheme(object sender, RoutedEventArgs e)
        {
            // Rotacija teme
            trenutnaTema = trenutnaTema switch
            {
                Tema.Tamna => Tema.Svijetla,
                Tema.Svijetla => Tema.Zelena,
                Tema.Zelena => Tema.Tamna,
                _ => Tema.Tamna
            };

            switch (trenutnaTema)
            {
                case Tema.Tamna:
                    // postavi boje za tamnu temu
                    Application.Current.Resources["PrimaryTextBrush"] = new SolidColorBrush(Colors.White); // Tamna tema

                    // Tamna tema
                    //this.Resources["BackgroundBrush"] = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    //this.Resources["ForegroundBrush"] = new SolidColorBrush(Colors.White);
                    MainBorder.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80)); // Tamna siva
                    MainBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62)); // Tamna siva
                    MenuPanel.Background = new SolidColorBrush(Color.FromRgb(52, 73, 94)); // Tamna siva
                    ThemeToggleButton.Content = "🌙"; // Menja ikonu na dugmetu
                    ThemeToggleButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    ThemeToggleButton.Foreground = new SolidColorBrush(Colors.White);

                    MenuItem1.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80)); // Tamna siva
                    MenuItem1.Foreground = new SolidColorBrush(Colors.White);
                    MenuItem2.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80)); // Tamna siva
                    MenuItem2.Foreground = new SolidColorBrush(Colors.White);
                    MenuItem3.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80)); // Tamna siva
                    MenuItem3.Foreground = new SolidColorBrush(Colors.White);
                    MenuItem4.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80)); // Tamna siva
                    MenuItem4.Foreground = new SolidColorBrush(Colors.White);
                    MenuItem5.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80)); // Tamna siva
                    MenuItem5.Foreground = new SolidColorBrush(Colors.White);
                    MenuItem6.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80)); // Tamna siva
                    MenuItem6.Foreground = new SolidColorBrush(Colors.White);
                    MenuItem7.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80)); // Tamna siva
                    MenuItem7.Foreground = new SolidColorBrush(Colors.White);

                    MainContentContainer.Background = new SolidColorBrush(Color.FromRgb(52, 73, 94));
                    Border.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    //Border.Background = new SolidColorBrush(Color.FromRgb(123, 155, 166)); 

                    MenuButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    MenuButton.Foreground = new SolidColorBrush(Colors.White);
                    LoginButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    LoginButton.Foreground = new SolidColorBrush(Colors.White);
                    ProfileButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    ProfileButton.Foreground = new SolidColorBrush(Colors.White);
                    MinimizeButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    MinimizeButton.Foreground = new SolidColorBrush(Colors.White);
                    MaximizeButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    MaximizeButton.Foreground = new SolidColorBrush(Colors.White);
                    CloseButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    CloseButton.Foreground = new SolidColorBrush(Colors.White);
                    LanguageToggleButton.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    LanguageToggleButton.Foreground = new SolidColorBrush(Colors.White);

                    ProfilePopup_Border.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    ProfilePopup_Border.BorderBrush = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    ProfilePopup_Icon1_Border.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    ProfilePopup_Icon1.Foreground = new SolidColorBrush(Colors.White);
                    ProfilePopup_Icon2_Border.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    ProfilePopup_Icon2.Foreground = new SolidColorBrush(Colors.White);
                    ProfilePopup_Customize.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    ProfilePopup_Customize.Foreground = new SolidColorBrush(Colors.White);
                    ProfilePopup_Logout.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    ProfilePopup_Logout.Foreground = new SolidColorBrush(Colors.White);

                    CustomizeProfile_Popup profilePopup = (CustomizeProfile_Popup)CustomizeProfilePopup.Child;
                    profilePopup.Border.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    profilePopup.Border.BorderBrush = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                    profilePopup.Naslov.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.Close_Button.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.emailBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.lozinkaBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.CheckBox.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.osobaIDBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.imeBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.prezimeBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.usernameBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.brojtelefonaBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.datumrodjenjaBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.adresastanovanjaBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.jmbgBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.zvanjeBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.temaBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.jezikBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.datumzaposlenjaBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.paviljonBlock.Foreground = new SolidColorBrush(Colors.White);
                    //MainWindow.Foreground = new SolidColorBrush(Colors.White);

                    if (MainContentControl.Content is AktuelnostiView aktuelnostiView)
                    {
                        var border = aktuelnostiView.FindName("OglasPopupBorder") as Border;
                        if (border != null)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80)); // Tamna siva
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62)); // Tamna siva
                        }
                        // Postavi boje teksta
                        if (aktuelnostiView.FindName("AktuelnostiTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.White);

                        if (aktuelnostiView.FindName("AktuelnostiLabel") is TextBlock labelText)
                            labelText.Foreground = new SolidColorBrush(Colors.White);

                        if (aktuelnostiView.FindName("AktuelnostiPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = Brushes.White;
                            closeButton.Background = Brushes.Transparent;
                        }
                    }

                    // Promijeni boju KonkursiPopupBorder-a za svijetlu temu
                    else if (MainContentControl.Content is KonkursiView konkursiView)
                    {
                        var border = konkursiView.FindName("KonkursiPopupBorder") as Border;
                        if (border != null)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62));
                        }
                        // Postavi boje teksta
                        if (konkursiView.FindName("KonkursiiTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.White);

                        if (konkursiView.FindName("KonkursiLabel1") is TextBlock labelText1)
                            labelText1.Foreground = new SolidColorBrush(Colors.White);

                        if (konkursiView.FindName("KonkursiLabel2") is TextBlock labelText2)
                            labelText2.Foreground = new SolidColorBrush(Colors.White);

                        if (konkursiView.FindName("KonkursiPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = Brushes.White;
                            closeButton.Background = Brushes.Transparent;
                        }
                    }

                    // Promijeni boju StudentskiSmjestajPopupBorder-a za tamnu temu
                    else if (MainContentControl.Content is StudentskiSmjestajView studentskismjestajView)
                    {
                        var border = studentskismjestajView.FindName("StudentskiSmjestajPopupBorder") as Border;
                        if (border != null)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                        }
                        // Postavi boje teksta
                        if (studentskismjestajView.FindName("StudentskiSmjestajTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.White);

                        if (studentskismjestajView.FindName("StudentskiSmjestajLabel1") is TextBlock labelText1)
                            labelText1.Foreground = new SolidColorBrush(Colors.White);

                        if (studentskismjestajView.FindName("StudentskiSmjestajLabel2") is TextBlock labelText2)
                            labelText2.Foreground = new SolidColorBrush(Colors.White);

                        if (studentskismjestajView.FindName("StudentskiSmjestajPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = Brushes.White;
                            closeButton.Background = Brushes.Transparent;
                        }
                    }

                    // Promijeni boju MenzaPopupBorder-a za tamnu temu
                    else if (MainContentControl.Content is MenzaView menzaView)
                    {
                        var border = menzaView.FindName("MenzaPopupBorder") as Border;
                        if (border != null)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62));
                        }
                        // Postavi boje teksta
                        if (menzaView.FindName("MenzaTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.White);

                        if (menzaView.FindName("MenzaLabel1") is TextBlock labelText1)
                            labelText1.Foreground = new SolidColorBrush(Colors.White);

                        if (menzaView.FindName("MenzaLabel2") is TextBlock labelText2)
                            labelText2.Foreground = new SolidColorBrush(Colors.White);

                        if (menzaView.FindName("MenzaLabel3") is TextBlock labelText3)
                            labelText3.Foreground = new SolidColorBrush(Colors.White);

                        if (menzaView.FindName("MenzaLabel4") is TextBlock labelText4)
                            labelText4.Foreground = new SolidColorBrush(Colors.White);

                        if (menzaView.FindName("MenzaLabel5") is TextBlock labelText5)
                            labelText5.Foreground = new SolidColorBrush(Colors.White);

                        if (menzaView.FindName("MenzaLabel6") is TextBlock labelText6)
                            labelText6.Foreground = new SolidColorBrush(Colors.White);

                        if (menzaView.FindName("MenzaLabel7") is TextBlock labelText7)
                            labelText7.Foreground = new SolidColorBrush(Colors.White);

                        if (menzaView.FindName("MenzaPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = Brushes.White;
                            closeButton.Background = Brushes.Transparent;
                        }
                    }

                    // Promijeni boju KontaktPopupBorder-a za tamnu temu
                    else if (MainContentControl.Content is KontaktView kontaktView)
                    {
                        var border = kontaktView.FindName("KontaktPopupBorder") as Border;
                        if (border != null)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62));
                        }
                        // Postavi boje teksta
                        if (kontaktView.FindName("KontaktTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.White);

                        if (kontaktView.FindName("KontaktLabel1") is TextBlock labelText1)
                            labelText1.Foreground = new SolidColorBrush(Colors.White);

                        if (kontaktView.FindName("KontaktLabel2") is TextBlock labelText2)
                            labelText2.Foreground = new SolidColorBrush(Colors.White);

                        if (kontaktView.FindName("KontaktLabel3") is TextBlock labelText3)
                            labelText3.Foreground = new SolidColorBrush(Colors.White);

                        if (kontaktView.FindName("KontaktLabel4") is TextBlock labelText4)
                            labelText4.Foreground = new SolidColorBrush(Colors.White);

                        if (kontaktView.FindName("KontaktLabel5") is TextBlock labelText5)
                            labelText5.Foreground = new SolidColorBrush(Colors.White);

                        if (kontaktView.FindName("KontaktLabel6") is TextBlock labelText6)
                            labelText6.Foreground = new SolidColorBrush(Colors.White);

                        if (kontaktView.FindName("KontaktLabel7") is TextBlock labelText7)
                            labelText7.Foreground = new SolidColorBrush(Colors.White);

                        if (kontaktView.FindName("KontaktLabel8") is TextBlock labelText8)
                            labelText8.Foreground = new SolidColorBrush(Colors.White);

                        if (kontaktView.FindName("KontaktPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = Brushes.White;
                            closeButton.Background = Brushes.Transparent;
                        }
                    }

                    // Promijeni boju DodajZaposlenogPopupBorder-a za tamnuu temu
                    else if (MainContentControl.Content is UpravljajZaposlenimaView upravljajzaposlenimaView)
                    {
                        var border = upravljajzaposlenimaView.FindName("DodajZaposlenogPopupBorder") as Border;
                        if (border != null)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62));
                        }

                        if (upravljajzaposlenimaView.FindName("CheckBox") is CheckBox checkBox)
                        {
                            checkBox.Foreground = Brushes.White;
                        }
                        // Postavi boje teksta
                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel1") is TextBlock labelText1)
                            labelText1.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel2") is TextBlock labelText2)
                            labelText2.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel3") is TextBlock labelText3)
                            labelText3.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel4") is TextBlock labelText4)
                            labelText4.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel5") is TextBlock labelText5)
                            labelText5.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel6") is TextBlock labelText6)
                            labelText6.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel7") is TextBlock labelText7)
                            labelText7.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel8") is TextBlock labelText8)
                            labelText8.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel9") is TextBlock labelText9)
                            labelText9.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel10") is TextBlock labelText10)
                            labelText10.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel11") is TextBlock labelText11)
                            labelText11.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel12") is TextBlock labelText12)
                            labelText12.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel13") is TextBlock labelText13)
                            labelText13.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel14") is TextBlock labelText14)
                            labelText14.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = Brushes.White;
                            closeButton.Background = Brushes.Transparent;
                        }
                    }

                    // Promjeni boju UputstvoView za svijetlu temu
                    else if (MainContentControl.Content is UputstvoView uputstvoView)
                    {
                        if (uputstvoView.FindName("UputstvoTekst") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.White);
                    }
                    // Postavi tamni logo
                    LogoImage.Source = new BitmapImage(new Uri("\\Resources\\Icons\\SCNTLogo4.png", UriKind.Relative));

                    //((Button)sender).Content = "🌙"; // Menja ikondu na dugmetu
                    break;

                case Tema.Svijetla:
                    // postavi boje za svijetlu temu
                    Application.Current.Resources["PrimaryTextBrush"] = new SolidColorBrush(Colors.Black); // Svetla tema
                                                                                                           // Svetla tema
                    //this.Resources["BackgroundBrush"] = new SolidColorBrush(Colors.White);
                    //this.Resources["ForegroundBrush"] = new SolidColorBrush(Colors.Black);
                    MainBorder.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245)); // Svetlo siva
                    MainBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(220, 220, 220)); // Svetlo siva
                    MenuPanel.Background = new SolidColorBrush(Color.FromRgb(230, 230, 230)); // Svetlo siva
                    ThemeToggleButton.Content = "☀"; // Menja ikonu na dugmetu
                    ThemeToggleButton.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                    ThemeToggleButton.Foreground = new SolidColorBrush(Colors.Black);

                    MenuItem1.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)); // Svetlo siva
                    MenuItem1.Foreground = new SolidColorBrush(Colors.Black);
                    MenuItem2.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)); // Svetlo siva
                    MenuItem2.Foreground = new SolidColorBrush(Colors.Black);
                    MenuItem3.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)); // Svetlo siva
                    MenuItem3.Foreground = new SolidColorBrush(Colors.Black);
                    MenuItem4.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)); // Svetlo siva
                    MenuItem4.Foreground = new SolidColorBrush(Colors.Black);
                    MenuItem5.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)); // Svetlo siva
                    MenuItem5.Foreground = new SolidColorBrush(Colors.Black);
                    MenuItem6.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)); // Svetlo siva
                    MenuItem6.Foreground = new SolidColorBrush(Colors.Black);
                    MenuItem7.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)); // Svetlo siva
                    MenuItem7.Foreground = new SolidColorBrush(Colors.Black);

                    //Border.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245));
                    Border.Background = new SolidColorBrush(Color.FromRgb(220, 220, 220));
                    MainContentContainer.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245));

                    MenuButton.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                    MenuButton.Foreground = new SolidColorBrush(Colors.Black);
                    LoginButton.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                    LoginButton.Foreground = new SolidColorBrush(Colors.Black);
                    ProfileButton.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                    ProfileButton.Foreground = new SolidColorBrush(Colors.Black);
                    MinimizeButton.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                    MinimizeButton.Foreground = new SolidColorBrush(Colors.Black);
                    MaximizeButton.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                    MaximizeButton.Foreground = new SolidColorBrush(Colors.Black);
                    CloseButton.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                    CloseButton.Foreground = new SolidColorBrush(Colors.Black);
                    LanguageToggleButton.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                    LanguageToggleButton.Foreground = new SolidColorBrush(Colors.Black);
                    //MainWindow.Foreground = new SolidColorBrush(Colors.Black);

                    ProfilePopup_Border.Background = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                    ProfilePopup_Border.BorderBrush = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                    ProfilePopup_Icon1_Border.Background = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                    ProfilePopup_Icon1.Foreground = new SolidColorBrush(Colors.Black);
                    ProfilePopup_Icon2_Border.Background = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                    ProfilePopup_Icon2.Foreground = new SolidColorBrush(Colors.Black);
                    ProfilePopup_Customize.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                    ProfilePopup_Customize.Foreground = new SolidColorBrush(Colors.Black);
                    ProfilePopup_Logout.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                    ProfilePopup_Logout.Foreground = new SolidColorBrush(Colors.Black);

                    profilePopup = (CustomizeProfile_Popup)CustomizeProfilePopup.Child;
                    profilePopup.Border.Background = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                    profilePopup.Border.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200));
                    profilePopup.Naslov.Foreground = new SolidColorBrush(Colors.Black);
                    profilePopup.Close_Button.Foreground = new SolidColorBrush(Colors.Black);
                    profilePopup.emailBlock.Foreground = new SolidColorBrush(Colors.Black);
                    profilePopup.lozinkaBlock.Foreground = new SolidColorBrush(Colors.Black);
                    profilePopup.CheckBox.Foreground = new SolidColorBrush(Colors.Black);
                    profilePopup.osobaIDBlock.Foreground = new SolidColorBrush(Colors.Black);
                    profilePopup.imeBlock.Foreground = new SolidColorBrush(Colors.Black);
                    profilePopup.prezimeBlock.Foreground = new SolidColorBrush(Colors.Black);
                    profilePopup.usernameBlock.Foreground = new SolidColorBrush(Colors.Black);
                    profilePopup.brojtelefonaBlock.Foreground = new SolidColorBrush(Colors.Black);
                    profilePopup.datumrodjenjaBlock.Foreground = new SolidColorBrush(Colors.Black);
                    profilePopup.adresastanovanjaBlock.Foreground = new SolidColorBrush(Colors.Black);
                    profilePopup.jmbgBlock.Foreground = new SolidColorBrush(Colors.Black);
                    profilePopup.zvanjeBlock.Foreground = new SolidColorBrush(Colors.Black);
                    profilePopup.temaBlock.Foreground = new SolidColorBrush(Colors.Black);
                    profilePopup.jezikBlock.Foreground = new SolidColorBrush(Colors.Black);
                    profilePopup.datumzaposlenjaBlock.Foreground = new SolidColorBrush(Colors.Black);
                    profilePopup.paviljonBlock.Foreground = new SolidColorBrush(Colors.Black);


                    // Promijeni boju OglasPopupBorder-a za svijetlu temu
                    if (MainContentControl.Content is AktuelnostiView aktuelnostiView1)
                    {
                        var border = aktuelnostiView1.FindName("OglasPopupBorder") as Border;
                        if (border != null)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(230, 230, 230)); // Svetlo siva
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200)); // Svetlo siva
                        }
                        // Postavi boje teksta
                        if (aktuelnostiView1.FindName("AktuelnostiTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.Black);

                        if (aktuelnostiView1.FindName("AktuelnostiLabel") is TextBlock labelText)
                            labelText.Foreground = new SolidColorBrush(Colors.Black);

                        if (aktuelnostiView1.FindName("AktuelnostiPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = Brushes.Black;
                            closeButton.Background = Brushes.Transparent;
                        }
                    }

                    // Promijeni boju KonkursiPopupBorder-a za svijetlu temu
                    else if (MainContentControl.Content is KonkursiView konkursiView)
                    {
                        var border = konkursiView.FindName("KonkursiPopupBorder") as Border;
                        if (border != null)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(230, 230, 230)); // Svetlo siva
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200)); // Svetlo siva
                        }
                        // Postavi boje teksta
                        if (konkursiView.FindName("KonkursiTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.Black);

                        if (konkursiView.FindName("KonkursiLabel1") is TextBlock labelText1)
                            labelText1.Foreground = new SolidColorBrush(Colors.Black);

                        if (konkursiView.FindName("KonkursiLabel2") is TextBlock labelText2)
                            labelText2.Foreground = new SolidColorBrush(Colors.Black);

                        if (konkursiView.FindName("KonkursiPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = Brushes.Black;
                            closeButton.Background = Brushes.Transparent;
                        }
                    }

                    // Promijeni boju StudentskiSmjestajPopupBorder-a za svijetlu temu
                    else if (MainContentControl.Content is StudentskiSmjestajView studentskismjestajView)
                    {
                        var border = studentskismjestajView.FindName("StudentskiSmjestajPopupBorder") as Border;
                        if (border != null)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(230, 230, 230)); // Svetlo siva
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200)); // Svetlo siva
                        }
                        // Postavi boje teksta
                        if (studentskismjestajView.FindName("StudentskiSmjestajTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.Black);

                        if (studentskismjestajView.FindName("StudentskiSmjestajLabel1") is TextBlock labelText1)
                            labelText1.Foreground = new SolidColorBrush(Colors.Black);

                        if (studentskismjestajView.FindName("StudentskiSmjestajLabel2") is TextBlock labelText2)
                            labelText2.Foreground = new SolidColorBrush(Colors.Black);

                        if (studentskismjestajView.FindName("StudentskiSmjestajPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = Brushes.Black;
                            closeButton.Background = Brushes.Transparent;
                        }
                    }

                    // Promijeni boju MenzaPopupBorder-a za svijetlu temu
                    else if (MainContentControl.Content is MenzaView menzaView)
                    {
                        var border = menzaView.FindName("MenzaPopupBorder") as Border;
                        if (border != null)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(230, 230, 230)); // Svetlo siva
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200)); // Svetlo siva
                        }
                        // Postavi boje teksta
                        if (menzaView.FindName("MenzaTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.Black);

                        if (menzaView.FindName("MenzaLabel1") is TextBlock labelText1)
                            labelText1.Foreground = new SolidColorBrush(Colors.Black);

                        if (menzaView.FindName("MenzaLabel2") is TextBlock labelText2)
                            labelText2.Foreground = new SolidColorBrush(Colors.Black);

                        if (menzaView.FindName("MenzaLabel3") is TextBlock labelText3)
                            labelText3.Foreground = new SolidColorBrush(Colors.Black);

                        if (menzaView.FindName("MenzaLabel4") is TextBlock labelText4)
                            labelText4.Foreground = new SolidColorBrush(Colors.Black);

                        if (menzaView.FindName("MenzaLabel5") is TextBlock labelText5)
                            labelText5.Foreground = new SolidColorBrush(Colors.Black);

                        if (menzaView.FindName("MenzaLabel6") is TextBlock labelText6)
                            labelText6.Foreground = new SolidColorBrush(Colors.Black);

                        if (menzaView.FindName("MenzaLabel7") is TextBlock labelText7)
                            labelText7.Foreground = new SolidColorBrush(Colors.Black);

                        if (menzaView.FindName("MenzaPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = Brushes.Black;
                            closeButton.Background = Brushes.Transparent;
                        }
                    }

                    // Promijeni boju KontaktPopupBorder-a za svijetlu temu
                    else if (MainContentControl.Content is KontaktView kontaktView)
                    {
                        var border = kontaktView.FindName("KontaktPopupBorder") as Border;
                        if (border != null)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(230, 230, 230)); // Svetlo siva
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200)); // Svetlo siva
                        }
                        // Postavi boje teksta
                        if (kontaktView.FindName("KontaktTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.Black);

                        if (kontaktView.FindName("KontaktLabel1") is TextBlock labelText1)
                            labelText1.Foreground = new SolidColorBrush(Colors.Black);

                        if (kontaktView.FindName("KontaktLabel2") is TextBlock labelText2)
                            labelText2.Foreground = new SolidColorBrush(Colors.Black);

                        if (kontaktView.FindName("KontaktLabel3") is TextBlock labelText3)
                            labelText3.Foreground = new SolidColorBrush(Colors.Black);

                        if (kontaktView.FindName("KontaktLabel4") is TextBlock labelText4)
                            labelText4.Foreground = new SolidColorBrush(Colors.Black);

                        if (kontaktView.FindName("KontaktLabel5") is TextBlock labelText5)
                            labelText5.Foreground = new SolidColorBrush(Colors.Black);

                        if (kontaktView.FindName("KontaktLabel6") is TextBlock labelText6)
                            labelText6.Foreground = new SolidColorBrush(Colors.Black);

                        if (kontaktView.FindName("KontaktLabel7") is TextBlock labelText7)
                            labelText7.Foreground = new SolidColorBrush(Colors.Black);

                        if (kontaktView.FindName("KontaktLabel8") is TextBlock labelText8)
                            labelText8.Foreground = new SolidColorBrush(Colors.Black);

                        if (kontaktView.FindName("KontaktPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = Brushes.Black;
                            closeButton.Background = Brushes.Transparent;
                        }
                    }

                    // Promijeni boju DodajZaposlenogPopupBorder-a za svijetlu temu
                    else if (MainContentControl.Content is UpravljajZaposlenimaView upravljajzaposlenimaView)
                    {
                        var border = upravljajzaposlenimaView.FindName("DodajZaposlenogPopupBorder") as Border;
                        if (border != null)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(230, 230, 230)); // Svetlo siva
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(200, 200, 200)); // Svetlo siva
                        }

                        if (upravljajzaposlenimaView.FindName("CheckBox") is CheckBox checkBox)
                        {
                            checkBox.Foreground = Brushes.Black;
                        }
                        // Postavi boje teksta
                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.Black);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel1") is TextBlock labelText1)
                            labelText1.Foreground = new SolidColorBrush(Colors.Black);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel2") is TextBlock labelText2)
                            labelText2.Foreground = new SolidColorBrush(Colors.Black);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel3") is TextBlock labelText3)
                            labelText3.Foreground = new SolidColorBrush(Colors.Black);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel4") is TextBlock labelText4)
                            labelText4.Foreground = new SolidColorBrush(Colors.Black);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel5") is TextBlock labelText5)
                            labelText5.Foreground = new SolidColorBrush(Colors.Black);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel6") is TextBlock labelText6)
                            labelText6.Foreground = new SolidColorBrush(Colors.Black);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel7") is TextBlock labelText7)
                            labelText7.Foreground = new SolidColorBrush(Colors.Black);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel8") is TextBlock labelText8)
                            labelText8.Foreground = new SolidColorBrush(Colors.Black);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel9") is TextBlock labelText9)
                            labelText9.Foreground = new SolidColorBrush(Colors.Black);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel10") is TextBlock labelText10)
                            labelText10.Foreground = new SolidColorBrush(Colors.Black);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel11") is TextBlock labelText11)
                            labelText11.Foreground = new SolidColorBrush(Colors.Black);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel12") is TextBlock labelText12)
                            labelText12.Foreground = new SolidColorBrush(Colors.Black);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel13") is TextBlock labelText13)
                            labelText13.Foreground = new SolidColorBrush(Colors.Black);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel14") is TextBlock labelText14)
                            labelText14.Foreground = new SolidColorBrush(Colors.Black);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = Brushes.Black;
                            closeButton.Background = Brushes.Transparent;
                        }
                    }

                    // Promjeni boju UputstvoView za svijetlu temu
                    else if (MainContentControl.Content is UputstvoView uputstvoView)
                    {
                        if (uputstvoView.FindName("UputstvoTekst") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.Black);
                    }
                        // Postavi tamni logo
                        LogoImage.Source = new BitmapImage(new Uri("\\Resources\\Icons\\SCNTLogo5.png", UriKind.Relative));
                    break;

                case Tema.Zelena:
                    // postavi boje za zelenu temu
                    Application.Current.Resources["PrimaryTextBrush"] = new SolidColorBrush(Colors.White);

                    MainBorder.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41)); //  146, 161, 67
                    MainBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62));
                    MenuPanel.Background = new SolidColorBrush(Color.FromRgb(146, 161, 67));
                    ThemeToggleButton.Content = "🌿"; // Menja ikonu na dugmetu
                    ThemeToggleButton.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    ThemeToggleButton.Foreground = new SolidColorBrush(Colors.White);

                    MenuItem1.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    MenuItem1.Foreground = new SolidColorBrush(Colors.White);
                    MenuItem2.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    MenuItem2.Foreground = new SolidColorBrush(Colors.White);
                    MenuItem3.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    MenuItem3.Foreground = new SolidColorBrush(Colors.White);
                    MenuItem4.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    MenuItem4.Foreground = new SolidColorBrush(Colors.White);
                    MenuItem5.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    MenuItem5.Foreground = new SolidColorBrush(Colors.White);
                    MenuItem6.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    MenuItem6.Foreground = new SolidColorBrush(Colors.White);
                    MenuItem7.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    MenuItem7.Foreground = new SolidColorBrush(Colors.White);

                    MainContentContainer.Background = new SolidColorBrush(Color.FromRgb(146, 161, 67));
                    Border.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));

                    MenuButton.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    MenuButton.Foreground = new SolidColorBrush(Colors.White);
                    LoginButton.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    LoginButton.Foreground = new SolidColorBrush(Colors.White);
                    ProfileButton.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    ProfileButton.Foreground = new SolidColorBrush(Colors.White);
                    MinimizeButton.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    MinimizeButton.Foreground = new SolidColorBrush(Colors.White);
                    MaximizeButton.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    MaximizeButton.Foreground = new SolidColorBrush(Colors.White);
                    CloseButton.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    CloseButton.Foreground = new SolidColorBrush(Colors.White);
                    LanguageToggleButton.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    LanguageToggleButton.Foreground = new SolidColorBrush(Colors.White);

                    ProfilePopup_Border.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    ProfilePopup_Border.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    ProfilePopup_Icon1_Border.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    ProfilePopup_Icon1.Foreground = new SolidColorBrush(Colors.White);
                    ProfilePopup_Icon2_Border.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    ProfilePopup_Icon2.Foreground = new SolidColorBrush(Colors.White);
                    ProfilePopup_Customize.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    ProfilePopup_Customize.Foreground = new SolidColorBrush(Colors.White);
                    ProfilePopup_Logout.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    ProfilePopup_Logout.Foreground = new SolidColorBrush(Colors.White);

                    profilePopup = (CustomizeProfile_Popup)CustomizeProfilePopup.Child;
                    profilePopup.Border.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    profilePopup.Border.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                    profilePopup.Naslov.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.Close_Button.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.emailBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.lozinkaBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.CheckBox.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.osobaIDBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.imeBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.prezimeBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.usernameBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.brojtelefonaBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.datumrodjenjaBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.adresastanovanjaBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.jmbgBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.zvanjeBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.temaBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.jezikBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.datumzaposlenjaBlock.Foreground = new SolidColorBrush(Colors.White);
                    profilePopup.paviljonBlock.Foreground = new SolidColorBrush(Colors.White);
                    //MainWindow.Foreground = new SolidColorBrush(Colors.White);

                    if (MainContentControl.Content is AktuelnostiView aktuelnostiView2)
                    {
                        var border = aktuelnostiView2.FindName("OglasPopupBorder") as Border;
                        if (border != null)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41)); // Tamna siva
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62)); // Tamna siva
                        }
                        // Postavi boje teksta
                        if (aktuelnostiView2.FindName("AktuelnostiTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.White);

                        if (aktuelnostiView2.FindName("AktuelnostiLabel") is TextBlock labelText)
                            labelText.Foreground = new SolidColorBrush(Colors.White);

                        if (aktuelnostiView2.FindName("AktuelnostiPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = Brushes.White;
                            closeButton.Background = Brushes.Transparent;
                        }
                    }

                    // Promijeni boju KonkursiPopupBorder-a za svijetlu temu
                    else if (MainContentControl.Content is KonkursiView konkursiView)
                    {
                        var border = konkursiView.FindName("KonkursiPopupBorder") as Border;
                        if (border != null)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62));
                        }
                        // Postavi boje teksta
                        if (konkursiView.FindName("KonkursiiTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.White);

                        if (konkursiView.FindName("KonkursiLabel1") is TextBlock labelText1)
                            labelText1.Foreground = new SolidColorBrush(Colors.White);

                        if (konkursiView.FindName("KonkursiLabel2") is TextBlock labelText2)
                            labelText2.Foreground = new SolidColorBrush(Colors.White);

                        if (konkursiView.FindName("KonkursiPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = Brushes.White;
                            closeButton.Background = Brushes.Transparent;
                        }
                    }

                    // Promijeni boju StudentskiSmjestajPopupBorder-a za tamnu temu
                    else if (MainContentControl.Content is StudentskiSmjestajView studentskismjestajView)
                    {
                        var border = studentskismjestajView.FindName("StudentskiSmjestajPopupBorder") as Border;
                        if (border != null)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                        }
                        // Postavi boje teksta
                        if (studentskismjestajView.FindName("StudentskiSmjestajTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.White);

                        if (studentskismjestajView.FindName("StudentskiSmjestajLabel1") is TextBlock labelText1)
                            labelText1.Foreground = new SolidColorBrush(Colors.White);

                        if (studentskismjestajView.FindName("StudentskiSmjestajLabel2") is TextBlock labelText2)
                            labelText2.Foreground = new SolidColorBrush(Colors.White);

                        if (studentskismjestajView.FindName("StudentskiSmjestajPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = Brushes.White;
                            closeButton.Background = Brushes.Transparent;
                        }
                    }

                    // Promijeni boju MenzaPopupBorder-a za tamnu temu
                    else if (MainContentControl.Content is MenzaView menzaView)
                    {
                        var border = menzaView.FindName("MenzaPopupBorder") as Border;
                        if (border != null)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62));
                        }
                        // Postavi boje teksta
                        if (menzaView.FindName("MenzaTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.White);

                        if (menzaView.FindName("MenzaLabel1") is TextBlock labelText1)
                            labelText1.Foreground = new SolidColorBrush(Colors.White);

                        if (menzaView.FindName("MenzaLabel2") is TextBlock labelText2)
                            labelText2.Foreground = new SolidColorBrush(Colors.White);

                        if (menzaView.FindName("MenzaLabel3") is TextBlock labelText3)
                            labelText3.Foreground = new SolidColorBrush(Colors.White);

                        if (menzaView.FindName("MenzaLabel4") is TextBlock labelText4)
                            labelText4.Foreground = new SolidColorBrush(Colors.White);

                        if (menzaView.FindName("MenzaLabel5") is TextBlock labelText5)
                            labelText5.Foreground = new SolidColorBrush(Colors.White);

                        if (menzaView.FindName("MenzaLabel6") is TextBlock labelText6)
                            labelText6.Foreground = new SolidColorBrush(Colors.White);

                        if (menzaView.FindName("MenzaLabel7") is TextBlock labelText7)
                            labelText7.Foreground = new SolidColorBrush(Colors.White);

                        if (menzaView.FindName("MenzaPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = Brushes.White;
                            closeButton.Background = Brushes.Transparent;
                        }
                    }

                    // Promijeni boju KontaktPopupBorder-a za tamnu temu
                    else if (MainContentControl.Content is KontaktView kontaktView)
                    {
                        var border = kontaktView.FindName("KontaktPopupBorder") as Border;
                        if (border != null)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62));
                        }
                        // Postavi boje teksta
                        if (kontaktView.FindName("KontaktTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.White);

                        if (kontaktView.FindName("KontaktLabel1") is TextBlock labelText1)
                            labelText1.Foreground = new SolidColorBrush(Colors.White);

                        if (kontaktView.FindName("KontaktLabel2") is TextBlock labelText2)
                            labelText2.Foreground = new SolidColorBrush(Colors.White);

                        if (kontaktView.FindName("KontaktLabel3") is TextBlock labelText3)
                            labelText3.Foreground = new SolidColorBrush(Colors.White);

                        if (kontaktView.FindName("KontaktLabel4") is TextBlock labelText4)
                            labelText4.Foreground = new SolidColorBrush(Colors.White);

                        if (kontaktView.FindName("KontaktLabel5") is TextBlock labelText5)
                            labelText5.Foreground = new SolidColorBrush(Colors.White);

                        if (kontaktView.FindName("KontaktLabel6") is TextBlock labelText6)
                            labelText6.Foreground = new SolidColorBrush(Colors.White);

                        if (kontaktView.FindName("KontaktLabel7") is TextBlock labelText7)
                            labelText7.Foreground = new SolidColorBrush(Colors.White);

                        if (kontaktView.FindName("KontaktLabel8") is TextBlock labelText8)
                            labelText8.Foreground = new SolidColorBrush(Colors.White);

                        if (kontaktView.FindName("KontaktPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = Brushes.White;
                            closeButton.Background = Brushes.Transparent;
                        }
                    }

                    // Promijeni boju DodajZaposlenogPopupBorder-a za tamnuu temu
                    else if (MainContentControl.Content is UpravljajZaposlenimaView upravljajzaposlenimaView)
                    {
                        var border = upravljajzaposlenimaView.FindName("DodajZaposlenogPopupBorder") as Border;
                        if (border != null)
                        {
                            border.Background = new SolidColorBrush(Color.FromRgb(0, 62, 41));
                            border.BorderBrush = new SolidColorBrush(Color.FromRgb(31, 47, 62));
                        }

                        if (upravljajzaposlenimaView.FindName("CheckBox") is CheckBox checkBox)
                        {
                            checkBox.Foreground = Brushes.White;
                        }
                        // Postavi boje teksta
                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogTitle") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel1") is TextBlock labelText1)
                            labelText1.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel2") is TextBlock labelText2)
                            labelText2.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel3") is TextBlock labelText3)
                            labelText3.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel4") is TextBlock labelText4)
                            labelText4.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel5") is TextBlock labelText5)
                            labelText5.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel6") is TextBlock labelText6)
                            labelText6.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel7") is TextBlock labelText7)
                            labelText7.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel8") is TextBlock labelText8)
                            labelText8.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel9") is TextBlock labelText9)
                            labelText9.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel10") is TextBlock labelText10)
                            labelText10.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel11") is TextBlock labelText11)
                            labelText11.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel12") is TextBlock labelText12)
                            labelText12.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel13") is TextBlock labelText13)
                            labelText13.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogLabel14") is TextBlock labelText14)
                            labelText14.Foreground = new SolidColorBrush(Colors.White);

                        if (upravljajzaposlenimaView.FindName("DodajZaposlenogPopupCloseButton") is Button closeButton)
                        {
                            closeButton.Foreground = Brushes.White;
                            closeButton.Background = Brushes.Transparent;
                        }
                    }

                    // Promjeni boju UputstvoView za svijetlu temu
                    else if (MainContentControl.Content is UputstvoView uputstvoView)
                    {
                        if (uputstvoView.FindName("UputstvoTekst") is TextBlock titleText)
                            titleText.Foreground = new SolidColorBrush(Colors.White);
                    }
                    // Postavi tamni logo
                    LogoImage.Source = new BitmapImage(new Uri("\\Resources\\Icons\\SCNTLogo4.png", UriKind.Relative));
                    break;
            }

            string novaTema = trenutnaTema.ToString(); // "Tamna", "Svijetla", "Zelena"
            // **Sačuvaj temu u bazi**
            if (!string.IsNullOrEmpty(loggedInUserEmail))
            {
                DatabaseConnection db = new DatabaseConnection();
                db.UpdateUserTheme(loggedInUserEmail, novaTema);
            }

            // Postavite nove četkice na prozor
            Background = (SolidColorBrush)this.Resources["BackgroundBrush"];
        }


        public bool jezikSrpski = true; // Podrazumevano srpski
        private void ToggleAktuelnostiLanguage()
        {
            if (MainContentControl.Content is AktuelnostiView aktuelnostiView)
            {
                // Pristupamo ItemsControl-u koji sadrži oglase
                if (aktuelnostiView.FindName("OglasiWrapPanel") is ItemsControl itemsControl)
                {
                    foreach (var item in itemsControl.Items)
                    {
                        // Uzmi generisani container za svaku stavku
                        var container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                        if (container != null)
                        {
                            // Pretražujemo vizuelno stablo da pronađemo dugme
                            var buttonOtvori = FindChild<Button>(container, "OtvoriPDFButton");
                            if (buttonOtvori != null)
                            {
                                // Proveri trenutno stanje i promeni tekst
                                if (buttonOtvori.Content?.ToString() == "Otvori PDF")
                                    buttonOtvori.Content = "Open PDF";
                                else
                                    buttonOtvori.Content = "Otvori PDF";
                            }

                            var buttonObrisi = FindChild<Button>(container, "Obrisi_Button");
                            if (buttonObrisi != null)
                            {
                                if(buttonObrisi.Content?.ToString() == "Obriši")
                                    buttonObrisi.Content = "Delete";
                                else
                                    buttonObrisi.Content = "Obriši";
                            }
                        }

                        
                    }
                }
            }
        }
        private void ToggleKonkursiLanguage()
        {
            if (MainContentControl.Content is KonkursiView konkursiView)
            {
                // Pristupamo ItemsControl-u koji sadrži konkurse
                if (konkursiView.FindName("KonkursiWrapPanel") is ItemsControl itemsControl)
                {
                    foreach (var item in itemsControl.Items)
                    {
                        // Uzmi generisani container za svaku stavku
                        var container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                        if (container != null)
                        {
                            // Dugme za otvaranje PDF-a
                            var buttonOtvori = FindChild<Button>(container, "OtvoriPDFButton_Konkurs");
                            if (buttonOtvori != null)
                            {
                                if (buttonOtvori.Content?.ToString() == "Otvori PDF")
                                    buttonOtvori.Content = "Open PDF";
                                else
                                    buttonOtvori.Content = "Otvori PDF";
                            }

                            // Dugme za brisanje konkursa
                            var buttonObrisi = FindChild<Button>(container, "Obrisi_Button_Konkurs");
                            if (buttonObrisi != null)
                            {
                                if (buttonObrisi.Content?.ToString() == "Obriši")
                                    buttonObrisi.Content = "Delete";
                                else
                                    buttonObrisi.Content = "Obriši";
                            }
                        }
                    }
                }
            }
        }
        private void ToggleStudentskiSmjestajLanguage()
        {
            if (MainContentControl.Content is StudentskiSmjestajView smjestajView)
            {
                if (smjestajView.FindName("StudentskiSmjestajWrapPanel") is ItemsControl itemsControl)
                {
                    foreach (var item in itemsControl.Items)
                    {
                        var container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                        if (container != null)
                        {
                            var buttonObrisi = FindChild<Button>(container, "Obrisi_Button_Smjestaj");
                            if (buttonObrisi != null)
                            {
                                if (buttonObrisi.Content?.ToString() == "Obriši")
                                    buttonObrisi.Content = "Delete";
                                else
                                    buttonObrisi.Content = "Obriši";
                            }
                        }
                    }
                }
            }
        }
        private void ToggleMenzaLanguage()
        {
            if (MainContentControl.Content is MenzaView menzaView)
            {
                if (menzaView.FindName("MenzaWrapPanel") is ItemsControl itemsControl)
                {
                    foreach (var item in itemsControl.Items)
                    {
                        var container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                        if (container != null)
                        {
                            var buttonObrisi = FindChild<Button>(container, "Obrisi_Button_Menza");
                            if (buttonObrisi != null)
                            {
                                if (buttonObrisi.Content?.ToString() == "Obriši")
                                    buttonObrisi.Content = "Delete";
                                else
                                    buttonObrisi.Content = "Obriši";
                            }
                            // Labela: Cijena doručka
                            var label1 = FindChild<TextBlock>(container, "MenzaWrapPanel_Label1");
                            if (label1 != null)
                                label1.Text = label1.Text.Contains("Cijena doručka")
                                    ? "Breakfast price: "
                                    : "Cijena doručka: ";

                            // Labela: Cijena ručka
                            var label2 = FindChild<TextBlock>(container, "MenzaWrapPanel_Label2");
                            if (label2 != null)
                                label2.Text = label2.Text.Contains("Cijena ručka")
                                    ? "Lunch price: "
                                    : "Cijena ručka: ";

                            // Labela: Cijena večere
                            var label3 = FindChild<TextBlock>(container, "MenzaWrapPanel_Label3");
                            if (label3 != null)
                                label3.Text = label3.Text.Contains("Cijena večere")
                                    ? "Dinner price: "
                                    : "Cijena večere: ";

                            // Labela: Termin doručka
                            var label4 = FindChild<TextBlock>(container, "MenzaWrapPanel_Label4");
                            if (label4 != null)
                                label4.Text = label4.Text.Contains("Termin doručka")
                                    ? "Breakfast time: "
                                    : "Termin doručka: ";

                            // Labela: Termin ručka
                            var label5 = FindChild<TextBlock>(container, "MenzaWrapPanel_Label5");
                            if (label5 != null)
                                label5.Text = label5.Text.Contains("Termin ručka")
                                    ? "Lunch time: "
                                    : "Termin ručka: ";

                            // Labela: Termin večere
                            var label6 = FindChild<TextBlock>(container, "MenzaWrapPanel_Label6");
                            if (label6 != null)
                                label6.Text = label6.Text.Contains("Termin večere")
                                    ? "Dinner time: "
                                    : "Termin večere: ";
                        }
                    }
                }
            }
        }
        private void ToggleKontaktLanguage()
        {
            if (MainContentControl.Content is KontaktView kontaktView)
            {
                if (kontaktView.FindName("KontaktWrapPanel") is ItemsControl itemsControl)
                {
                    foreach (var item in itemsControl.Items)
                    {
                        var container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                        if (container != null)
                        {
                            var buttonObrisi = FindChild<Button>(container, "KontaktWrapPanel_Delete");
                            if (buttonObrisi != null)
                            {
                                if (buttonObrisi.Content?.ToString() == "Obriši")
                                    buttonObrisi.Content = "Delete";
                                else
                                    buttonObrisi.Content = "Obriši";
                            }
                            // Labela: Adresa 1
                            var label1 = FindChild<TextBlock>(container, "KontaktWrapPanel_Label1");
                            if (label1 != null)
                                label1.Text = label1.Text.Contains("Adresa")
                                    ? "Address 1: "
                                    : "Adresa 1: ";

                            // Labela: Adresa 2
                            var label2 = FindChild<TextBlock>(container, "KontaktWrapPanel_Label2");
                            if (label2 != null)
                                label2.Text = label2.Text.Contains("Adresa")
                                    ? "Address 2: "
                                    : "Adresa 2: ";

                            // Labela: Broj telefona 1
                            var label3 = FindChild<TextBlock>(container, "KontaktWrapPanel_Label3");
                            if (label3 != null)
                                label3.Text = label3.Text.Contains("telefona")
                                    ? "Phone number 1: "
                                    : "Broj telefona 1: ";

                            // Labela: Broj telefona 2
                            var label4 = FindChild<TextBlock>(container, "KontaktWrapPanel_Label4");
                            if (label4 != null)
                                label4.Text = label4.Text.Contains("telefona")
                                    ? "Phone number 2: "
                                    : "Broj telefona 2: ";
                        }
                    }
                }
            }
        }
        
        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T typedChild && (child as FrameworkElement)?.Name == childName)
                    return typedChild;

                var result = FindChild<T>(child, childName);
                if (result != null) return result;
            }

            return null;
        }

        private void ToggleLanguage(object sender, RoutedEventArgs e)
        {
            LanguageSymbol.Text = jezikSrpski ? "E" : "S";
            if (jezikSrpski)
            {
                // Postavi engleski tekst za sve dugmiće
                MenuButton.ToolTip = new ToolTip { Content = "Menu" };
                LogoImage.ToolTip = new ToolTip { Content = "Student center 'Nikola Tesla'" };
                LoginButton.Content = jezikSrpski ? "🔑 Login" : "🔑 Prijava";
                LoginButton.ToolTip = new ToolTip { Content = "Login" };
                ProfileButton.ToolTip = new ToolTip { Content = "Profile" };
                LanguageToggleButton.ToolTip = jezikSrpski ? "Change to Serbian" : "Promjeni u Engleski";
                ThemeToggleButton.ToolTip = new ToolTip { Content = "Change theme" };
                MinimizeButton.ToolTip = new ToolTip { Content = "Minimize" };
                MaximizeButton.ToolTip = new ToolTip { Content = "Maximize" };
                CloseButton.ToolTip = new ToolTip { Content = "Close" };

                MenuItem1.Content = "News";
                MenuItem2.Content = "Accommodation Applications";
                MenuItem3.Content = "Student Accommodation";
                MenuItem4.Content = "Students Restaurant";
                MenuItem5.Content = "Contacts";
                MenuItem6.Content = "Manage Employees";
                MenuItem7.Content = "Instructions";

                ProfilePopup_Customize.Content = "⚙ Customize profile";
                ProfilePopup_Logout.Content = "🔓 Logout";

                // Promjena za CustomizeProfile_Popup
                if (CustomizeProfilePopup.Child is CustomizeProfile_Popup profilePopup)
                {
                    if (profilePopup.FindName("Naslov") is TextBlock naslov)
                        naslov.Text = "Edit Profile";
                    if (profilePopup.FindName("emailBlock") is TextBlock emailBlock)
                        emailBlock.Text = "Email:";
                    if (profilePopup.FindName("lozinkaBlock") is TextBlock lozinkaBlock)
                        lozinkaBlock.Text = "Password:";
                    if (profilePopup.FindName("textPassword") is TextBlock textPassword)
                        textPassword.Text = "New password";
                    if (profilePopup.FindName("osobaIDBlock") is TextBlock osobaIDBlock)
                        osobaIDBlock.Text = "Person ID:";
                    if (profilePopup.FindName("imeBlock") is TextBlock imeBlock)
                        imeBlock.Text = "First Name:";
                    if (profilePopup.FindName("prezimeBlock") is TextBlock prezimeBlock)
                        prezimeBlock.Text = "Last Name:";
                    if (profilePopup.FindName("usernameBlock") is TextBlock usernameBlock)
                        usernameBlock.Text = "Username:";
                    if (profilePopup.FindName("brojtelefonaBlock") is TextBlock brojtelefonaBlock)
                        brojtelefonaBlock.Text = "Phone Number:";
                    if (profilePopup.FindName("datumrodjenjaBlock") is TextBlock datumrodjenjaBlock)
                        datumrodjenjaBlock.Text = "Date of Birth:";
                    if (profilePopup.FindName("adresastanovanjaBlock") is TextBlock adresastanovanjaBlock)
                        adresastanovanjaBlock.Text = "Address:";
                    if (profilePopup.FindName("jmbgBlock") is TextBlock jmbgBlock)
                        jmbgBlock.Text = "JMBG:";
                    if (profilePopup.FindName("zvanjeBlock") is TextBlock zvanjeBlock)
                        zvanjeBlock.Text = "Title:";
                    if (profilePopup.FindName("temaBlock") is TextBlock temaBlock)
                        temaBlock.Text = "App Theme:";
                    if (profilePopup.FindName("jezikBlock") is TextBlock jezikBlock)
                        jezikBlock.Text = "App Language:";
                    if (profilePopup.FindName("datumzaposlenjaBlock") is TextBlock datumzaposlenjaBlock)
                        datumzaposlenjaBlock.Text = "Employment Date:";
                    if (profilePopup.FindName("paviljonBlock") is TextBlock paviljonBlock)
                        paviljonBlock.Text = "Pavilion:";
                    if (profilePopup.FindName("SaveButton") is Button saveButton)
                        saveButton.Content = "Save Changes";
                }

                if (MainContentControl.Content is AktuelnostiView aktuelnostiView)
                {
                    if (aktuelnostiView.FindName("AktuelnostiNaslov") is TextBlock textTitle)
                        textTitle.Text = "News";
                    if (aktuelnostiView.FindName("BtnDodajOglas") is Button btnNewPost)
                        btnNewPost.Content = "➕ Add new post";

                    // Pristup elementima unutar popup-a
                    if (aktuelnostiView.FindName("AktuelnostiTitle") is TextBlock title)
                        title.Text = "News";
                    if (aktuelnostiView.FindName("AktuelnostiLabel") is TextBlock label)
                        label.Text = "Text for post:";
                    if (aktuelnostiView.FindName("OdaberiPDFButton") is Button pdfButton)
                        pdfButton.Content = "🡅 Select PDF";
                    if (aktuelnostiView.FindName("PotvrdiButton") is Button confirmButton)
                        confirmButton.Content = "Confirm";
                    if (aktuelnostiView.FindName("PDFFileNameText") is TextBlock pdfText)
                        pdfText.Text = "No file selected";
                      
                    ToggleAktuelnostiLanguage();
                }

                if (MainContentControl.Content is KonkursiView konkursiView)
                {
                    if (konkursiView.FindName("Konkursi_Title") is TextBlock textTitle)
                        textTitle.Text = "Accommodation applications";
                    if (konkursiView.FindName("BtnDodajOglas") is Button btnAdd)
                        btnAdd.Content = "➕ Add new post";

                    // Naslov u popupu
                    if (konkursiView.FindName("KonkursiTitle") is TextBlock popupTitle)
                        popupTitle.Text = "Accommodation Applications";

                    // Labela za naziv konkursa
                    if (konkursiView.FindName("KonkursiLabel1") is TextBlock konkursiLabel1)
                        konkursiLabel1.Text = "Accommodation applications title:";

                    // Labela za tekst konkursa
                    if (konkursiView.FindName("KonkursiLabel2") is TextBlock konkursiLabel2)
                        konkursiLabel2.Text = "Accommodation applications text:";

                    // Dugme za odabir PDF-a
                    if (konkursiView.FindName("OdaberiPDFButton") is Button pdfButton)
                        pdfButton.Content = "🡅 Select PDF";

                    // Dugme za potvrdu
                    if (konkursiView.FindName("PotvrdiButton") is Button confirmButton)
                        confirmButton.Content = "Confirm";

                    // Tekst ako fajl nije odabran
                    if (konkursiView.FindName("PDFFileNameText") is TextBlock pdfText)
                        pdfText.Text = "No file selected";

                    ToggleKonkursiLanguage();
                }

                if (MainContentControl.Content is StudentskiSmjestajView smjestajView)
                {
                    if (smjestajView.FindName("Smjestaj_Title") is TextBlock textTitle)
                        textTitle.Text = "Student accommodation";
                    if (smjestajView.FindName("BtnDodajOglas") is Button btnAdd)
                        btnAdd.Content = "➕ Add new post";

                    // Pristup elementima unutar popup-a
                    if (smjestajView.FindName("StudentskiSmjestajTitle") is TextBlock popupTitle)
                        popupTitle.Text = "Student accommodation";
                    if (smjestajView.FindName("StudentskiSmjestajLabel1") is TextBlock label1)
                        label1.Text = "Post title:";
                    if (smjestajView.FindName("StudentskiSmjestajLabel2") is TextBlock label2)
                        label2.Text = "Post text:";
                    if (smjestajView.FindName("PotvrdiButton") is Button confirmButton)
                        confirmButton.Content = "Confirm";

                    ToggleStudentskiSmjestajLanguage();
                }

                if (MainContentControl.Content is MenzaView menzaView)
                {
                    if (menzaView.FindName("MenzaNaslov") is TextBlock textTitle)
                        textTitle.Text = "Students restaurant";
                    if (menzaView.FindName("BtnDodajOglas") is Button btnAdd)
                        btnAdd.Content = "➕ Add new post";

                    // Pristup elementima unutar popup-a
                    if (menzaView.FindName("MenzaTitle") is TextBlock popupTitle)
                        popupTitle.Text = "Students restaurant";

                    if (menzaView.FindName("MenzaLabel1") is TextBlock label1)
                        label1.Text = "Post text:";

                    if (menzaView.FindName("MenzaLabel2") is TextBlock label2)
                        label2.Text = "Breakfast price:";

                    if (menzaView.FindName("MenzaLabel3") is TextBlock label3)
                        label3.Text = "Lunch price:";

                    if (menzaView.FindName("MenzaLabel4") is TextBlock label4)
                        label4.Text = "Dinner price:";

                    if (menzaView.FindName("MenzaLabel5") is TextBlock label5)
                        label5.Text = "Breakfast time:";

                    if (menzaView.FindName("MenzaLabel6") is TextBlock label6)
                        label6.Text = "Lunch time:";

                    if (menzaView.FindName("MenzaLabel7") is TextBlock label7)
                        label7.Text = "Dinner time:";

                    if (menzaView.FindName("PotvrdiButton") is Button confirmButton)
                        confirmButton.Content = "Confirm";

                    ToggleMenzaLanguage();
                }

                if (MainContentControl.Content is KontaktView kontaktView)
                {
                    if (kontaktView.FindName("KontaktiNaslov") is TextBlock textTitle)
                        textTitle.Text = "Contacts";

                    if (kontaktView.FindName("BtnDodajKontakt") is Button btnAdd)
                        btnAdd.Content = "➕ Add new post";

                    // Pristup elementima unutar popup-a
                    if (kontaktView.FindName("KontaktTitle") is TextBlock popupTitle)
                        popupTitle.Text = "Contacts:";

                    if (kontaktView.FindName("KontaktLabel1") is TextBlock label1)
                        label1.Text = "Post text:";

                    if (kontaktView.FindName("KontaktLabel2") is TextBlock label2)
                        label2.Text = "Address 1:";

                    if (kontaktView.FindName("KontaktLabel3") is TextBlock label3)
                        label3.Text = "Address 2:";

                    if (kontaktView.FindName("KontaktLabel4") is TextBlock label4)
                        label4.Text = "Phone number 1:";

                    if (kontaktView.FindName("KontaktLabel5") is TextBlock label5)
                        label5.Text = "Phone number 2:";

                    if (kontaktView.FindName("PotvrdiButton") is Button confirmButton)
                        confirmButton.Content = "Confirm";

                    ToggleKontaktLanguage();
                }

                if (MainContentControl.Content is UpravljajZaposlenimaView zaposleniView)
                {
                    if (zaposleniView.FindName("UpravljajZaposlenimaNaslov") is TextBlock textTitle)
                        textTitle.Text = "Manage employees";
                    if (zaposleniView.FindName("BtnDodajZaposlenog") is Button btnAdd)
                        btnAdd.Content = "➕ Add Employee";

                    // Access popup elements
                    if (zaposleniView.FindName("DodajZaposlenogTitle") is TextBlock popupTitle)
                        popupTitle.Text = "Enter details:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel1") is TextBlock label1)
                        label1.Text = "First Name:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel2") is TextBlock label2)
                        label2.Text = "Last Name:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel3") is TextBlock label3)
                        label3.Text = "Email:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel4") is TextBlock label4)
                        label4.Text = "Username:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel5") is TextBlock label5)
                        label5.Text = "Password:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel6") is TextBlock label6)
                        label6.Text = "Phone Number:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel7") is TextBlock label7)
                        label7.Text = "ID Number:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel8") is TextBlock label8)
                        label8.Text = "Title:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel9") is TextBlock label9)
                        label9.Text = "Theme:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel10") is TextBlock label10)
                        label10.Text = "Language:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel11") is TextBlock label11)
                        label11.Text = "Birth Date:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel12") is TextBlock label12)
                        label12.Text = "Hire Date:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel13") is TextBlock label13)
                        label13.Text = "Pavilion:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel14") is TextBlock label14)
                        label14.Text = "Address:";

                    if (zaposleniView.FindName("PotvrdiButton") is Button confirmButton)
                        confirmButton.Content = "Confirm";

                    // Update combo box items
                    if (zaposleniView.FindName("TemaComboBox") is ComboBox temaCombo)
                    {
                        ((ComboBoxItem)temaCombo.Items[0]).Content = "Dark";
                        ((ComboBoxItem)temaCombo.Items[1]).Content = "Light";
                    }

                    if (zaposleniView.FindName("JezikComboBox") is ComboBox jezikCombo)
                    {
                        ((ComboBoxItem)jezikCombo.Items[0]).Content = "Serbian";
                        ((ComboBoxItem)jezikCombo.Items[1]).Content = "English";
                    }

                    if (zaposleniView.FindName("PaviljonComboBox") is ComboBox paviljonCombo)
                    {
                        ((ComboBoxItem)paviljonCombo.Items[0]).Content = "Pavilion 1";
                        ((ComboBoxItem)paviljonCombo.Items[1]).Content = "Pavilion 2";
                        ((ComboBoxItem)paviljonCombo.Items[2]).Content = "Pavilion 3";
                        ((ComboBoxItem)paviljonCombo.Items[3]).Content = "Pavilion 4";
                    }

                    // DataGrid columns
                    if (zaposleniView.FindName("ZaposleniDataGrid") is DataGrid dataGrid)
                    {
                        foreach (var column in dataGrid.Columns)
                        {
                            if (column.Header.ToString() == "OsobaID") column.Header = "PersonID";
                            if (column.Header.ToString() == "Ime") column.Header = "First name";
                            if (column.Header.ToString() == "Prezime") column.Header = "Last name";
                            if (column.Header.ToString() == "Korisničko ime") column.Header = "Username";
                            if (column.Header.ToString() == "Email") column.Header = "Email";
                            if (column.Header.ToString() == "Šifra") column.Header = "Password";
                            if (column.Header.ToString() == "Datum zaposlenja") column.Header = "Hire date";
                            if (column.Header.ToString() == "Paviljon") column.Header = "Pavilion";
                            if (column.Header.ToString() == "Telefon") column.Header = "Phone";
                            if (column.Header.ToString() == "Datum rođenja") column.Header = "Birth date";
                            if (column.Header.ToString() == "JMBG") column.Header = "ID Number";
                            if (column.Header.ToString() == "Adresa") column.Header = "Address";
                            if (column.Header.ToString() == "Zvanje") column.Header = "Title";
                            if (column.Header.ToString() == "Tema") column.Header = "Theme";
                            if (column.Header.ToString() == "Jezik") column.Header = "Language";
                            if (column.Header.ToString() == "Uredi") column.Header = "Edit";
                            if (column.Header.ToString() == "Obriši") column.Header = "Delete";
                        }
                    }

                    if (zaposleniView.FindName("SaveButton") is Button saveButton)
                        saveButton.Content = "Save Changes";

                    if (zaposleniView.FindName("textPassword") is TextBlock passwordHint)
                        passwordHint.Text = "New password";

                    //ToggleZaposleniLanguage();
                }

                jezikSrpski = false;
                if (userRole == "Admin" && activeButton == MenuItem6)
                {
                    MainContentControl.Content = new UpravljajZaposlenimaView(jezikSrpski);
                    MenuButton6_Click(sender, e);
                }
                else if (userRole == "Admin" && activeButton == MenuItem7)
                {
                    MainContentControl.Content = new UputstvoView(userRole, jezikSrpski);
                    MenuButton7_Click(sender, e);
                }
                else if (userRole == "Zaposleni" && activeButton == MenuItem1)
                {
                    MainContentControl.Content = new AktuelnostiView(loggedInUserEmail, jezikSrpski);
                    MenuButton1_Click(sender, e);
                }
                else if (userRole == "Zaposleni" && activeButton == MenuItem2)
                {
                    MainContentControl.Content = new KonkursiView(loggedInUserEmail, jezikSrpski);
                    MenuButton2_Click(sender, e);
                }
                else if (userRole == "Zaposleni" && activeButton == MenuItem3)
                {
                    MainContentControl.Content = new StudentskiSmjestajView(loggedInUserEmail, jezikSrpski);
                    MenuButton3_Click(sender, e);
                }
                else if (userRole == "Zaposleni" && activeButton == MenuItem4)
                {
                    MainContentControl.Content = new MenzaView(loggedInUserEmail, jezikSrpski);
                    MenuButton4_Click(sender, e);
                }
                else if (userRole == "Zaposleni" && activeButton == MenuItem5)
                {
                    MainContentControl.Content = new KontaktView(loggedInUserEmail, jezikSrpski);
                    MenuButton5_Click(sender, e);
                }
                else if (userRole == "Zaposleni" && activeButton == MenuItem7)
                {
                    MainContentControl.Content = new UputstvoView(userRole, jezikSrpski);
                    MenuButton7_Click(sender, e);
                }
                else if (userRole != "Zaposleni" && userRole != "Admin" && activeButton == MenuItem7)
                {
                    MainContentControl.Content = new UputstvoView("Guest", jezikSrpski);
                    MenuButton7_Click(sender, e);
                }

            }
            else
            {
                // Vrati na srpski tekst
                MenuButton.ToolTip = new ToolTip { Content = "Meni" };
                LogoImage.ToolTip = new ToolTip { Content = "Studentski centar 'Nikola Tesla'" };
                LoginButton.Content = jezikSrpski ? "🔑 Login" : "🔑 Prijava";
                LoginButton.ToolTip = new ToolTip { Content = "Prijava" };
                ProfileButton.ToolTip = new ToolTip { Content = "Profil" };
                LanguageToggleButton.ToolTip = jezikSrpski ? "Change to Serbian" : "Promjeni u Engleski";
                ThemeToggleButton.ToolTip = new ToolTip { Content = "Promjeni temu" };
                MinimizeButton.ToolTip = new ToolTip { Content = "Umanji" };
                MaximizeButton.ToolTip = new ToolTip { Content = "Uvećaj" };
                CloseButton.ToolTip = new ToolTip { Content = "Zatvori" };

                MenuItem1.Content = "Aktuelnosti";
                MenuItem2.Content = "Konkursi";
                MenuItem3.Content = "Studentski smještaj";
                MenuItem4.Content = "Menza";
                MenuItem5.Content = "Kontakt";
                MenuItem6.Content = "Upravljaj zaposlenima";
                MenuItem7.Content = "Uputstvo";

                ProfilePopup_Customize.Content = "⚙ Uredi profil";
                ProfilePopup_Logout.Content = "🔓 Odjavi se";

                // Promjena za CustomizeProfile_Popup
                if (CustomizeProfilePopup.Child is CustomizeProfile_Popup profilePopup)
                {
                    if (profilePopup.FindName("Naslov") is TextBlock naslov)
                        naslov.Text = "Uredi profil";
                    if (profilePopup.FindName("emailBlock") is TextBlock emailBlock)
                        emailBlock.Text = "Email:";
                    if (profilePopup.FindName("lozinkaBlock") is TextBlock lozinkaBlock)
                        lozinkaBlock.Text = "Lozinka:";
                    if (profilePopup.FindName("textPassword") is TextBlock textPassword)
                        textPassword.Text = "Nova lozinka";
                    if (profilePopup.FindName("osobaIDBlock") is TextBlock osobaIDBlock)
                        osobaIDBlock.Text = "Osoba ID:";
                    if (profilePopup.FindName("imeBlock") is TextBlock imeBlock)
                        imeBlock.Text = "Ime:";
                    if (profilePopup.FindName("prezimeBlock") is TextBlock prezimeBlock)
                        prezimeBlock.Text = "Prezime:";
                    if (profilePopup.FindName("usernameBlock") is TextBlock usernameBlock)
                        usernameBlock.Text = "Korisničko ime:";
                    if (profilePopup.FindName("brojtelefonaBlock") is TextBlock brojtelefonaBlock)
                        brojtelefonaBlock.Text = "Broj telefona:";
                    if (profilePopup.FindName("datumrodjenjaBlock") is TextBlock datumrodjenjaBlock)
                        datumrodjenjaBlock.Text = "Datum rođenja:";
                    if (profilePopup.FindName("adresastanovanjaBlock") is TextBlock adresastanovanjaBlock)
                        adresastanovanjaBlock.Text = "Adresa stanovanja:";
                    if (profilePopup.FindName("jmbgBlock") is TextBlock jmbgBlock)
                        jmbgBlock.Text = "JMBG:";
                    if (profilePopup.FindName("zvanjeBlock") is TextBlock zvanjeBlock)
                        zvanjeBlock.Text = "Zvanje:";
                    if (profilePopup.FindName("temaBlock") is TextBlock temaBlock)
                        temaBlock.Text = "Tema aplikacije:";
                    if (profilePopup.FindName("jezikBlock") is TextBlock jezikBlock)
                        jezikBlock.Text = "Jezik aplikacije:";
                    if (profilePopup.FindName("datumzaposlenjaBlock") is TextBlock datumzaposlenjaBlock)
                        datumzaposlenjaBlock.Text = "Datum zaposlenja:";
                    if (profilePopup.FindName("paviljonBlock") is TextBlock paviljonBlock)
                        paviljonBlock.Text = "Paviljon:";
                    if (profilePopup.FindName("SaveButton") is Button saveButton)
                        saveButton.Content = "Sačuvaj promjene";
                }

                // Promjena za Aktuelnosti
                if (MainContentControl.Content is AktuelnostiView aktuelnostiView)
                {
                    if (aktuelnostiView.FindName("AktuelnostiNaslov") is TextBlock textTitle)
                        textTitle.Text = "Aktuelnosti";
                    if (aktuelnostiView.FindName("BtnDodajOglas") is Button btnNewPost)
                        btnNewPost.Content = "➕ Dodaj novi oglas";

                    // Pristup elementima unutar popup-a
                    if (aktuelnostiView.FindName("AktuelnostiTitle") is TextBlock title)
                        title.Text = "Aktuelnosti";
                    if (aktuelnostiView.FindName("AktuelnostiLabel") is TextBlock label)
                        label.Text = "Tekst oglasa:";
                    if (aktuelnostiView.FindName("OdaberiPDFButton") is Button pdfButton)
                        pdfButton.Content = "🡅 Odaberi PDF";
                    if (aktuelnostiView.FindName("PotvrdiButton") is Button confirmButton)
                        confirmButton.Content = "Potvrdi";
                    if (aktuelnostiView.FindName("PDFFileNameText") is TextBlock pdfText)
                        pdfText.Text = "Nijedan fajl nije odabran";
                    ToggleAktuelnostiLanguage();
                }

                if (MainContentControl.Content is KonkursiView konkursiView)
                {
                    // Naslov stranice
                    if (konkursiView.FindName("Konkursi_Title") is TextBlock textTitle)
                        textTitle.Text = "Konkursi";

                    // Dugme za dodavanje konkursa
                    if (konkursiView.FindName("BtnDodajOglas") is Button btnAdd)
                        btnAdd.Content = "➕ Dodaj novi oglas";

                    // Naslov u popupu
                    if (konkursiView.FindName("KonkursiTitle") is TextBlock popupTitle)
                        popupTitle.Text = "Konkursi";

                    // Labela za naziv konkursa
                    if (konkursiView.FindName("KonkursiLabel1") is TextBlock konkursiLabel1)
                        konkursiLabel1.Text = "Naslov konkursa:";

                    // Labela za tekst konkursa
                    if (konkursiView.FindName("KonkursiLabel2") is TextBlock konkursiLabel2)
                        konkursiLabel2.Text = "Tekst konkursa:";

                    // Dugme za odabir PDF-a
                    if (konkursiView.FindName("OdaberiPDFButton") is Button pdfButton)
                        pdfButton.Content = "🡅 Odaberi PDF";

                    // Dugme za potvrdu
                    if (konkursiView.FindName("PotvrdiButton") is Button confirmButton)
                        confirmButton.Content = "Potvrdi";

                    // Tekst ako fajl nije odabran
                    if (konkursiView.FindName("PDFFileNameText") is TextBlock pdfText)
                        pdfText.Text = "Nijedan fajl nije odabran";

                    ToggleKonkursiLanguage();
                }

                if (MainContentControl.Content is StudentskiSmjestajView smjestajView)
                {
                    if (smjestajView.FindName("Smjestaj_Title") is TextBlock textTitle)
                        textTitle.Text = "Studentski smještaj";
                    if (smjestajView.FindName("BtnDodajOglas") is Button btnAdd)
                        btnAdd.Content = "➕ Dodaj novi oglas";

                    // Pristup elementima unutar popup-a
                    if (smjestajView.FindName("StudentskiSmjestajTitle") is TextBlock popupTitle)
                        popupTitle.Text = "Studentski smještaj";
                    if (smjestajView.FindName("StudentskiSmjestajLabel1") is TextBlock label1)
                        label1.Text = "Naslov oglasa:";
                    if (smjestajView.FindName("StudentskiSmjestajLabel2") is TextBlock label2)
                        label2.Text = "Tekst oglasa:";
                    if (smjestajView.FindName("PotvrdiButton") is Button confirmButton)
                        confirmButton.Content = "Potvrdi";

                    ToggleStudentskiSmjestajLanguage();
                }

                if (MainContentControl.Content is MenzaView menzaView)
                {
                    if (menzaView.FindName("MenzaNaslov") is TextBlock textTitle)
                        textTitle.Text = "'Menza'";
                    if (menzaView.FindName("BtnDodajOglas") is Button btnAdd)
                        btnAdd.Content = "➕ Dodaj novi oglas";

                    // Pristup elementima unutar popup-a
                    if (menzaView.FindName("MenzaTitle") is TextBlock popupTitle)
                        popupTitle.Text = "'Menza'";

                    if (menzaView.FindName("MenzaLabel1") is TextBlock label1)
                        label1.Text = "Tekst oglasa:";

                    if (menzaView.FindName("MenzaLabel2") is TextBlock label2)
                        label2.Text = "Cijena doručka:";

                    if (menzaView.FindName("MenzaLabel3") is TextBlock label3)
                        label3.Text = "Cijena ručka:";

                    if (menzaView.FindName("MenzaLabel4") is TextBlock label4)
                        label4.Text = "Cijena večere:";

                    if (menzaView.FindName("MenzaLabel5") is TextBlock label5)
                        label5.Text = "Termin doručka:";

                    if (menzaView.FindName("MenzaLabel6") is TextBlock label6)
                        label6.Text = "Termin ručka:";

                    if (menzaView.FindName("MenzaLabel7") is TextBlock label7)
                        label7.Text = "Termin večere:";

                    if (menzaView.FindName("PotvrdiButton") is Button confirmButton)
                        confirmButton.Content = "Potvrdi";

                    ToggleMenzaLanguage();
                }

                if (MainContentControl.Content is KontaktView kontaktView)
                {
                    if (kontaktView.FindName("KontaktiNaslov") is TextBlock textTitle)
                        textTitle.Text = "Kontakti";
                    if (kontaktView.FindName("BtnDodajKontakt") is Button btnAdd)
                        btnAdd.Content = "➕ Dodaj kontakte";

                    // Pristup elementima unutar popup-a
                    if (kontaktView.FindName("KontaktTitle") is TextBlock popupTitle)
                        popupTitle.Text = "Kontakti:";

                    if (kontaktView.FindName("KontaktLabel1") is TextBlock label1)
                        label1.Text = "Tekst oglasa:";

                    if (kontaktView.FindName("KontaktLabel2") is TextBlock label2)
                        label2.Text = "Adresa 1:";

                    if (kontaktView.FindName("KontaktLabel3") is TextBlock label3)
                        label3.Text = "Adresa 2:";

                    if (kontaktView.FindName("KontaktLabel4") is TextBlock label4)
                        label4.Text = "Broj telefona 1:";

                    if (kontaktView.FindName("KontaktLabel5") is TextBlock label5)
                        label5.Text = "Broj telefona 2:";

                    if (kontaktView.FindName("PotvrdiButton") is Button confirmButton)
                        confirmButton.Content = "Potvrdi";

                    ToggleKontaktLanguage();
                }

                if (MainContentControl.Content is UpravljajZaposlenimaView zaposleniView)
                {
                    if (zaposleniView.FindName("UpravljajZaposlenimaNaslov") is TextBlock textTitle)
                        textTitle.Text = "Upravljaj zaposlenima";
                    if (zaposleniView.FindName("BtnDodajZaposlenog") is Button btnAdd)
                        btnAdd.Content = "➕ Dodaj zaposlenog";

                    // Pristup elementima unutar popup-a
                    if (zaposleniView.FindName("DodajZaposlenogTitle") is TextBlock popupTitle)
                        popupTitle.Text = "Unesite podatke:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel1") is TextBlock label1)
                        label1.Text = "Ime:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel2") is TextBlock label2)
                        label2.Text = "Prezime:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel3") is TextBlock label3)
                        label3.Text = "Email:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel4") is TextBlock label4)
                        label4.Text = "Korisničko Ime:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel5") is TextBlock label5)
                        label5.Text = "Lozinka:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel6") is TextBlock label6)
                        label6.Text = "Broj Telefona:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel7") is TextBlock label7)
                        label7.Text = "JMBG:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel8") is TextBlock label8)
                        label8.Text = "Zvanje:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel9") is TextBlock label9)
                        label9.Text = "Tema:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel10") is TextBlock label10)
                        label10.Text = "Jezik:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel11") is TextBlock label11)
                        label11.Text = "Datum Rođenja:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel12") is TextBlock label12)
                        label12.Text = "Datum Zaposlenja:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel13") is TextBlock label13)
                        label13.Text = "Paviljon:";

                    if (zaposleniView.FindName("DodajZaposlenogLabel14") is TextBlock label14)
                        label14.Text = "Adresa Stanovanja:";

                    if (zaposleniView.FindName("PotvrdiButton") is Button confirmButton)
                        confirmButton.Content = "Potvrdi";

                    // Update combo box items
                    if (zaposleniView.FindName("TemaComboBox") is ComboBox temaCombo)
                    {
                        ((ComboBoxItem)temaCombo.Items[0]).Content = "Tamna";
                        ((ComboBoxItem)temaCombo.Items[1]).Content = "Svijetla";
                    }

                    if (zaposleniView.FindName("JezikComboBox") is ComboBox jezikCombo)
                    {
                        ((ComboBoxItem)jezikCombo.Items[0]).Content = "Srpski";
                        ((ComboBoxItem)jezikCombo.Items[1]).Content = "Englski";
                    }

                    if (zaposleniView.FindName("PaviljonComboBox") is ComboBox paviljonCombo)
                    {
                        ((ComboBoxItem)paviljonCombo.Items[0]).Content = "Paviljon 1";
                        ((ComboBoxItem)paviljonCombo.Items[1]).Content = "Paviljon 2";
                        ((ComboBoxItem)paviljonCombo.Items[2]).Content = "Paviljon 3";
                        ((ComboBoxItem)paviljonCombo.Items[3]).Content = "Paviljon 4";
                    }

                    // DataGrid columns
                    if (zaposleniView.FindName("ZaposleniDataGrid") is DataGrid dataGrid)
                    {
                        foreach (var column in dataGrid.Columns)
                        {
                            if (column.Header.ToString() == "PersonID") column.Header = "OsobaID";
                            if (column.Header.ToString() == "First name") column.Header = "Ime";
                            if (column.Header.ToString() == "Last name") column.Header = "Prezime";
                            if (column.Header.ToString() == "Username") column.Header = "Korisničko ime";
                            if (column.Header.ToString() == "Email") column.Header = "Email";
                            if (column.Header.ToString() == "Password") column.Header = "Šifra";
                            if (column.Header.ToString() == "Hire date") column.Header = "Datum zaposlenja";
                            if (column.Header.ToString() == "Pavilion") column.Header = "Paviljon";
                            if (column.Header.ToString() == "Phone") column.Header = "Telefon";
                            if (column.Header.ToString() == "Birth date") column.Header = "Datum rođenja";
                            if (column.Header.ToString() == "ID Number") column.Header = "JMBG";
                            if (column.Header.ToString() == "Address") column.Header = "Adresa";
                            if (column.Header.ToString() == "Title") column.Header = "Zvanje";
                            if (column.Header.ToString() == "Theme") column.Header = "Tema";
                            if (column.Header.ToString() == "Language") column.Header = "Jezik";
                            if (column.Header.ToString() == "Edit") column.Header = "Uredi";
                            if (column.Header.ToString() == "Delete") column.Header = "Obriši";
                        }
                    }

                    if (zaposleniView.FindName("SaveButton") is Button saveButton)
                        saveButton.Content = "Sačuvaj izmjene";

                    //ToggleZaposleniLanguage();
                }

                jezikSrpski = true;
                if (userRole == "Admin" && activeButton == MenuItem6)
                {
                    MainContentControl.Content = new UpravljajZaposlenimaView(jezikSrpski);
                    MenuButton6_Click(sender, e);
                }
                else if (userRole == "Admin" && activeButton == MenuItem7)
                {
                    MainContentControl.Content = new UputstvoView(userRole, jezikSrpski);
                    MenuButton7_Click(sender, e);
                }
                else if (userRole == "Zaposleni" && activeButton == MenuItem1)
                {
                    MainContentControl.Content = new AktuelnostiView(loggedInUserEmail, jezikSrpski);
                    MenuButton1_Click(sender, e);
                }
                else if (userRole == "Zaposleni" && activeButton == MenuItem2)
                {
                    MainContentControl.Content = new KonkursiView(loggedInUserEmail, jezikSrpski);
                    MenuButton2_Click(sender, e);
                }
                else if (userRole == "Zaposleni" && activeButton == MenuItem3)
                {
                    MainContentControl.Content = new StudentskiSmjestajView(loggedInUserEmail, jezikSrpski);
                    MenuButton3_Click(sender, e);
                }
                else if (userRole == "Zaposleni" && activeButton == MenuItem4)
                {
                    MainContentControl.Content = new MenzaView(loggedInUserEmail, jezikSrpski);
                    MenuButton4_Click(sender, e);
                }
                else if (userRole == "Zaposleni" && activeButton == MenuItem5)
                {
                    MainContentControl.Content = new KontaktView(loggedInUserEmail, jezikSrpski);
                    MenuButton5_Click(sender, e);
                }
                else if (userRole == "Zaposleni" && activeButton == MenuItem7)
                {
                    MainContentControl.Content = new UputstvoView(userRole, jezikSrpski);
                    MenuButton7_Click(sender, e);
                }
                else if (userRole != "Zaposleni" && userRole != "Admin" && activeButton == MenuItem7)
                {
                    MainContentControl.Content = new UputstvoView("Guest", jezikSrpski);
                    MenuButton7_Click(sender, e);
                }
            }
            //isDarkTheme = !isDarkTheme;
            //string novaTema = isDarkTheme ? "Tamna" : "Svijetla";

            // Sačuvaj jezik u bazu
            if (!string.IsNullOrEmpty(loggedInUserEmail))
            {
                DatabaseConnection db = new DatabaseConnection();
                db.UpdateUserLanguage(loggedInUserEmail, jezikSrpski ? "Srpski" : "Engleski");
            }
        }


        private void ProfileSettings_Click(object sender, RoutedEventArgs e)
        {
            // Dodajte logiku za otvaranje prozora za podešavanja profila
            MessageBox.Show("Profile Settings clicked!");
        }

        private void LogoutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Ovde možete dodati logiku za logout
            MessageBox.Show("Logout clicked!");
            // Na primer, možete obrisati sesiju, korisničke podatke, ili uputiti korisnika na login ekran
        }


        // Otvori Popup kada je kliknuto na Customize Profile dugme
        private void CustomizeProfile_Click(object sender, RoutedEventArgs e)
        {
            ProfilePopup.IsOpen = false;
            
            // Prikazivanje zamagljivanja pozadine
            BackgroundMask.Visibility = Visibility.Visible;

            // Dodavanje Blur efekta na glavni prozor
            this.Effect = new System.Windows.Media.Effects.BlurEffect { Radius = 10 };


            // Otvori Popup
            CustomizeProfile_Popup profilePopup = (CustomizeProfile_Popup)CustomizeProfilePopup.Child;

            // Centriranje Popup-a
            double offsetX = ((this.Width) / (-2));
            double offsetY = 0;

            // Postavljanje ofseta
            CustomizeProfilePopup.HorizontalOffset = offsetX;
            CustomizeProfilePopup.VerticalOffset = offsetY;

            // Dobijanje podataka o adminu iz baze
            //Admin adminData = DatabaseConnection.GetAdminData();
            profilePopup.SetUserRole(userRole);
            if (userRole == "Admin")
            {
                // Ako je admin, uzmi podatke iz Admin tabele
                Admin adminData = DatabaseConnection.GetAdminData();
                if (adminData != null)
                {
                    profilePopup.SetAdminData(adminData);
                    CustomizeProfilePopup.IsOpen = true;
                }
                else
                {
                    MessageBox.Show("Podaci o adminu nisu pronađeni!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (userRole == "Zaposleni")
            {
                // Ako je zaposleni, uzmi podatke iz Zaposleni tabele
                Zaposleni zaposleniData = DatabaseConnection.GetZaposleniData(loggedInUserEmail); // Prosleđujemo email ulogovanog korisnika
                if (zaposleniData != null)
                {
                    profilePopup.SetZaposleniData(zaposleniData);
                    CustomizeProfilePopup.IsOpen = true;
                }
                else
                {
                    MessageBox.Show("Podaci o zaposlenom nisu pronađeni!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Nepoznata korisnička uloga!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        public void ClosePopup()
        {
            // Ukloni Blur efekat sa glavnog prozora
            this.Effect = null;

            // Sakrij Popup

            CustomizeProfilePopup.IsOpen = false;

            // Sakrij masku (zamagljivanje)
            BackgroundMask.Visibility = Visibility.Collapsed;
        }

        private void ProfilePopup_Closed(object sender, EventArgs e)
        {
            this.Effect = null;
            BackgroundMask.Visibility = Visibility.Collapsed;  // Uklanja zamagljenje kada se popup zatvori
        }




    }
}