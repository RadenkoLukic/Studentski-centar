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

namespace LoginForm
{
    /// <summary>
    /// Interaction logic for UputstvoView.xaml
    /// </summary>
    public partial class UputstvoView : UserControl
    {
        private string userRole;
        private bool jezikSrpski;
        public UputstvoView(string role, bool jezikSrpski)
        {
            InitializeComponent();
            this.userRole = role;
            this.jezikSrpski = jezikSrpski;
            PrikaziUputstvo();
        }

        private void PrikaziUputstvo()
        {
            if (jezikSrpski)
            {
                if (userRole == "Admin")
                {
                    UputstvoTekst.Text = "📄 Uputstvo za korištenje – Administrator\r\n" +
                        "Dobrodošli!\r\n\r\n" +
                        "Kao administrator imate mogućnost dodavanja i brisanja zaposlenih korisnika kao i izmjene podataka vezanik za iste.\r\n\r\n" +
                        "🔘 Navigacija:\r\n" +
                        "U gornjem lijevom uglu nalazi se dugme za meni (☰). Klikom na njega otvara se bočna traka sa sledećim stavkama:\r\n\r\n" +
                        "◦ Upravljaj zaposlenima - Pregled liste zaposlenih kao i rad sa njihovim nalozima kao i dodavanje novih naloga.\r\n\r\n" +
                        "   ▪ Tabelarni pregled informacija o zaposlenima: ID Osobe(iz baze podataka), Ime, Prezime, Korisničko ime, Email, Šifra, Datum zaposlenja, Paviljon, Telefon, Datum rođenja, JMBG, Adresa, Zvanje, Tema i Jezik.\r\n\r\n" +
                        "   ▪ Mogućnost izmjene određenih informacija o zaposlenim klikom na '✏️ Uredi' dugme (svijetlo plave boje) za odabranog zaposlenog (ID Osobe, Tema i Jezik se ne mogu mijenjati).\r\n\r\n" +
                        "   ▪ Mogućnost brisanja naloga klikom na '❌ Obrši' dugme (crvene boje) kojim se briše zaposleni u čijem redu se nalazi Obriši dugme.\r\n\r\n" +
                        "   ▪ Dodavanje novog naloga za zaposlenog klikom na '➕ Dodaj zaposlenog' dugme kojim se otvara prozor za unos potrebnih podataka za zaposlenog:\r\n\r\n" +
                        "       ▪ Ime\r\n\r\n" +
                        "       ▪ Prezime\r\n\r\n" +
                        "       ▪ Email\r\n\r\n" +
                        "       ▪ Korisničko ime\r\n\r\n" +
                        "       ▪ Lozinka, kao i mogućnost prikazivanja ili sakrivanja lozinke prilikom unosa klikom na 👁\r\n\r\n" +
                        "       ▪ Broj telefona\r\n\r\n" +
                        "       ▪ JMBG (13 cifara)\r\n\r\n" +
                        "       ▪ Zvanje\r\n\r\n" +
                        "       ▪ Tema (padajući meni sa mogućnosti izbora Tamne, Svijetle i Zelene teme)\r\n\r\n" +
                        "       ▪ Jezik (padajući meni sa mogućnosti izbora Srpskog i Engleskog jezika)\r\n\r\n" +
                        "       ▪ Datum rođenja (ručni unos datuma u formatu dd/mm/yyyy ili klikom na ikonicu kalendara)\r\n\r\n" +
                        "       ▪ Datum zaposlenja (ručni unos datuma u formatu dd/mm/yyyy ili klikom na ikonicu kalendara)\r\n\r\n" +
                        "       ▪ Paviljon (padajući meni sa mogućnosti izbora Paviljon 1, Paviljon 2, Paviljon 3, Paviljon 4)\r\n\r\n" +
                        "       ▪ Adresa stanovanja\r\n\r\n" +
                        "       ▪ Potvrdi - dugme, potvrda unosa i kreiranje novog naloga\r\n\r\n" +
                        "◦ Uputstvo – Trenutno gledano uputstvo za rad sa aplikacijom.\r\n\r\n" +
                        "🔘 Gornji desni ugao aplikacije:\r\n" +
                        "U gornjem desnom uglu nalaze se sledeća dugmad:\r\n\r\n" +
                        "◦ 👤 Profil - Ovaj prozor omogućava pregled i uređivanje osnovnih podataka o korisniku (⚙ Uredi profil) kao i odjavu sa naloga (🔓 Odjavi se). Informacije su organizovane u vertikalnom prikazu i predstavljene kroz polja za unos (tekstualna polja).\r\n\r\n" +
                        "   📌 Opis prozora '⚙ Uredi profil':\r\n" +
                        "       ▪ Email: Prikazuje email adresu korisnika.\r\n\r\n" +
                        "       ▪ Lozinka: Polje za unos nove lozinke, sa dodatnim kontrolama za prikaz i skrivanje unijetog teksta.\r\n\r\n" +
                        "       ▪ Osoba ID: Jedinstveni identifikator korisnika. Nije moguće mijenjati (polje je samo za prikaz).\r\n\r\n" +
                        "       ▪ Ime i Prezime: Polja za unos imena i prezimena korisnika.\r\n\r\n" +
                        "       ▪ Korisničko ime: Prikazuje korisničko ime. Može biti uređeno.\r\n\r\n" +
                        "       ▪ Broj telefona: Telefonski kontakt korisnika.\r\n\r\n" +
                        "       ▪ Datum rođenja: Prikazuje datum u formatu dd.MM.yyyy.\r\n\r\n" +
                        "       ▪ Adresa stanovanja: Unos adrese prebivališta korisnika.\r\n\r\n" +
                        "       ▪ JMBG: Prikazuje jedinstveni matični broj građana. Može biti izmjenjen (13 cifara obavezno).\r\n\r\n" +
                        "       ▪ Zvanje: Stručno zvanje ili radno mijesto korisnika.\r\n\r\n" +
                        "       ▪ Tema aplikacije: Prikazuje trenutno aktivnu temu aplikacije (npr. Tamna, Svijetla, Zelena). Ovo je informativno polje, nije moguće mijenjati direktno ovde.\r\n\r\n" +
                        "       ▪ Jezik aplikacije: Prikazuje koji jezik je trenutno aktivan (Srpski ili Engleski). Takođe nije moguće mijenjati direktno ovde.\r\n\r\n" +
                        "       ▪ 'Sačuvaj promjene' - dugme: Čuva podatke u bazu.\r\n\r\n" +
                        "   💡 Dodatne funkcionalnosti:\r\n" +
                        "   - Polje za unos lozinke sadrži ikonicu 👁 pomoću koje korisnik može prikazati ili sakriti lozinku prilikom unosa.\r\n\r\n" +
                        "   - Polja Osoba ID, Tema, i Jezik su read-only – korisnik ih vidi, ali ne može menjati.\r\n\r\n" +
                        "◦ 🎨 Tema – Klikom na ovo dugme možete promeniti izgled aplikacije. Na raspolaganju su tri teme:\r\n\r\n" +
                        "   ▪ 🌙 Tamna\r\n\r\n" +
                        "   ▪ ☀ Svijetla\r\n\r\n" +
                        "   ▪ 🌿 Zelena\r\n\r\n" +
                        "◦ 🌐 Jezik – Omogućava promjenu jezika aplikacije između srpskog '🌐S' i engleskog '🌐E' jezika.\r\n\r\n" +
                        "◦ ➖ Umanji – Smanjuje prozor aplikacije na traku zadataka.\r\n\r\n" +
                        "◦ 🗖 Uvećaj / Vrati – Maksimizuje ili vraća aplikaciju na prethodnu veličinu prozora.\r\n\r\n" +
                        "◦ ❌ Zatvori – Zatvara aplikaciju.";
                }
                else if (userRole == "Zaposleni")
                {
                     UputstvoTekst.Text = "📄 Uputstvo za korištenje – Zaposleni (prijavljeni korisnik)\r\n" +
                        "Dobrodošli!\r\n\r\n" +
                        "Kao zaposleni imate mogućnost da pregledate osnovne informacije u aplikaciji kao i mogućnost dodavanja i brisanja sadržaja.\r\n\r\n" +
                        "🔘 Navigacija:\r\n" +
                        "U gornjem lijevom uglu nalazi se dugme za meni (☰). Klikom na njega otvara se bočna traka sa sledećim stavkama:\r\n\r\n" +
                        "◦ Aktuelnosti – Pregled najnovijih informacija, oglasa i obaveštenja vezanih za studentski dom i studente uz mogućnost otvaranja PDF fajlova, dodavanja novih i brisanja postojećih oglasa.\r\n" +
                        "Dodavanje se vrši klikom na dugme '+ Dodaj novi oglas' i unosom sadržaja oglasa kao i dodavanjem zvaničnog PDF dokumeta, a brisanje klikom na dugme 'Obriši'.\r\n\r\n" +
                        "◦ Konkursi – Informacije o trenutno aktivnim konkursima za smještaj uz mogućnost otvaranja PDF fajlova, dodavanja novih i brisanja postojećih oglasa.\r\n" +
                        "Dodavanje se vrši klikom na dugme '+ Dodaj novi oglas' i unosom naslova oglasa, sadržaja oglasa kao i dodavanjem zvaničnog PDF dokumeta, a brisanje klikom na dugme 'Obriši'.\r\n\r\n" +
                        "◦ Studentski smještaj – Osnovne informacije o kapacitetima, pravilima i uslovima smještaja u domu  uz mogućnost dodavanja novih i brisanja postojećih oglasa..\r\n" +
                        "Dodavanje se vrši klikom na dugme '+ Dodaj novi oglas' i unosom naslova oglasa i sadržaja oglasa, a brisanje klikom na dugme 'Obriši'.\r\n\r\n" +
                        "◦ Menza – Radno vrijeme studentske menze, termini i cijene obroka  uz mogućnost dodavanja novih i brisanja postojećih oglasa.\r\n" +
                        "Dodavanje se vrši klikom na dugme '+ Dodaj novi oglas' i unosom sadržaja oglasa, terminom i cijenom svakog obroka pojedinačno a brisanje klikom na dugme 'Obriši'.\r\n\r\n" +
                        "◦ Kontakt – Podaci za kontakt sa upravom studentskog doma ili tehničkom podrškom  uz mogućnost dodavanja novih i brisanja postojećih oglasa..\r\n" +
                        "Dodavanje se vrši klikom na dugme '+ Dodaj kontakte' i unosom sadržaja oglasa, adrese, broja telefona, Email-a, Facebook i Instagram linka a brisanje klikom na dugme 'Obriši'.\r\n\r\n" +
                        "◦ Uputstvo – Trenutno gledano uputstvo za rad sa aplikacijom.\r\n\r\n" +
                        "🔘 Gornji desni ugao aplikacije:\r\n" +
                        "U gornjem desnom uglu nalaze se sledeća dugmad:\r\n\r\n" +
                        "◦ 👤 Profil - Ovaj prozor omogućava pregled i uređivanje osnovnih podataka o korisniku (⚙ Uredi profil) kao i odjavu sa naloga (🔓 Odjavi se). Informacije su organizovane u vertikalnom prikazu i predstavljene kroz polja za unos (tekstualna polja).\r\n\r\n" +
                        "   📌 Opis prozora '⚙ Uredi profil':\r\n" +
                        "       ▪ Email: Prikazuje email adresu korisnika.\r\n\r\n" +
                        "       ▪ Lozinka: Polje za unos nove lozinke, sa dodatnim kontrolama za prikaz i skrivanje unijetog teksta.\r\n\r\n" +
                        "       ▪ Osoba ID: Jedinstveni identifikator korisnika. Nije moguće mijenjati (polje je samo za prikaz).\r\n\r\n" +
                        "       ▪ Ime i Prezime: Polja za unos imena i prezimena korisnika.\r\n\r\n" +
                        "       ▪ Korisničko ime: Prikazuje korisničko ime. Može biti uređeno.\r\n\r\n" +
                        "       ▪ Broj telefona: Telefonski kontakt korisnika.\r\n\r\n" +
                        "       ▪ Datum rođenja: Prikazuje datum u formatu dd.MM.yyyy.\r\n\r\n" +
                        "       ▪ Adresa stanovanja: Unos adrese prebivališta korisnika.\r\n\r\n" +
                        "       ▪ JMBG: Prikazuje jedinstveni matični broj građana. Može biti izmjenjen (13 cifara obavezno).\r\n\r\n" +
                        "       ▪ Zvanje: Stručno zvanje ili radno mijesto korisnika.\r\n\r\n" +
                        "       ▪ Tema aplikacije: Prikazuje trenutno aktivnu temu aplikacije (npr. Tamna, Svijetla, Zelena). Ovo je informativno polje, nije moguće mijenjati direktno ovde.\r\n\r\n" +
                        "       ▪ Jezik aplikacije: Prikazuje koji jezik je trenutno aktivan (Srpski ili Engleski). Takođe nije moguće mijenjati direktno ovde.\r\n\r\n" +
                        "       ▪ Datum zaposlenja: (Skriveno dok se ne odnosi na zaposlenog). Prikazuje datum kada je zaposleni korisnik primljen u radni odnos.\r\n\r\n" +
                        "       ▪ Paviljon: Prikazuje broj paviljona u kom je zaposlen.\r\n\r\n" +
                        "       ▪ 'Sačuvaj promjene' - dugme: Čuva podatke u bazu.\r\n\r\n" +
                        "   💡 Dodatne funkcionalnosti:\r\n" +
                        "   - Polje za unos lozinke sadrži ikonicu 👁 pomoću koje korisnik može prikazati ili sakriti lozinku prilikom unosa.\r\n\r\n" +
                        "   - Polja Osoba ID, Tema, i Jezik su read-only – korisnik ih vidi, ali ne može menjati.\r\n\r\n" +
                        "◦ 🎨 Tema – Klikom na ovo dugme možete promeniti izgled aplikacije. Na raspolaganju su tri teme:\r\n\r\n" +
                        "   ▪ 🌙 Tamna\r\n\r\n" +
                        "   ▪ ☀ Svijetla\r\n\r\n" +
                        "   ▪ 🌿 Zelena\r\n\r\n" +
                        "◦ 🌐 Jezik – Omogućava promjenu jezika aplikacije između srpskog '🌐S' i engleskog '🌐E' jezika.\r\n\r\n" +
                        "◦ ➖ Umanji – Smanjuje prozor aplikacije na traku zadataka.\r\n\r\n" +
                        "◦ 🗖 Uvećaj / Vrati – Maksimizuje ili vraća aplikaciju na prethodnu veličinu prozora.\r\n\r\n" +
                        "◦ ❌ Zatvori – Zatvara aplikaciju.";
                }
                else
                {
                    UputstvoTekst.Text = "📄 Uputstvo za korištenje – Gost (neprijavljeni korisnik)\r\n" +
                        "Dobrodošli!\r\n\r\n" +
                        "Kao neprijavljeni korisnik imate mogućnost da pregledate osnovne informacije u aplikaciji. Za pristup dodatnim funkcijama potrebno je da se prijavite.\r\n\r\n" +
                        "🔘 Navigacija:\r\n" +
                        "U gornjem lijevom uglu nalazi se dugme za meni (☰). Klikom na njega otvara se bočna traka sa sledećim stavkama:\r\n\r\n" +
                        "◦ Aktuelnosti – Pregled najnovijih informacija, oglasa i obaveštenja vezanih za studentski dom i studente uz mogućnost otvaranja PDF fajlova.\r\n\r\n" +
                        "◦ Konkursi – Informacije o trenutno aktivnim konkursima za smještaj uz mogućnost otvaranja PDF fajlova.\r\n\r\n" +
                        "◦ Studentski smještaj – Osnovne informacije o kapacitetima, pravilima i uslovima smještaja u domu.\r\n\r\n" +
                        "◦ Menza – Radno vrijeme studentske menze, termini i cijene obroka.\r\n\r\n" +
                        "◦ Kontakt – Podaci za kontakt sa upravom studentskog doma ili tehničkom podrškom.\r\n\r\n" +
                        "◦ Uputstvo – Trenutno gledano uputstvo za rad sa aplikacijom.\r\n\r\n" +
                        "🔘 Gornji desni ugao aplikacije:\r\n" +
                        "U gornjem desnom uglu nalaze se sledeća dugmad:\r\n\r\n" +
                        "◦ 🔑 Prijava – Otvara prozor za prijavljivanje korisnika. Nakon uspješne prijave, dobijate pristup dodatnim funkcijama u zavisnosti od uloge (zaposleni ili administrator).\r\n\r\n" +
                        "◦ 🎨 Tema – Klikom na ovo dugme možete promeniti izgled aplikacije. Na raspolaganju su tri teme:\r\n\r\n" +
                        "   ▪ 🌙 Tamna\r\n\r\n" +
                        "   ▪ ☀ Svijetla\r\n\r\n" +
                        "   ▪ 🌿 Zelena\r\n\r\n" +
                        "◦ 🌐 Jezik – Omogućava promjenu jezika aplikacije između srpskog '🌐S' i engleskog '🌐E' jezika.\r\n\r\n" +
                        "◦ ➖ Umanji – Smanjuje prozor aplikacije na traku zadataka.\r\n\r\n" +
                        "◦ 🗖 Uvećaj / Vrati – Maksimizuje ili vraća aplikaciju na prethodnu veličinu prozora.\r\n\r\n" +
                        "◦ ❌ Zatvori – Zatvara aplikaciju.";
                }
            }
            else // English
            {
                if (userRole == "Admin")
                {
                    UputstvoTekst.Text = "📄 User Guide – Administrator\r\n" +
                        "Welcome!\r\n\r\n" +
                        "As an administrator, you have the ability to add, delete, and edit employee accounts and related information.\r\n\r\n" +
                        "🔘 Navigation:\r\n" +
                        "In the top left corner, there's a menu button (☰). Clicking it opens a side panel with the following options:\r\n\r\n" +
                        "◦ Manage Employees – View the list of employees and manage their accounts or add new ones.\r\n\r\n" +
                        "   ▪ Tabelar overview of employee information: Person ID (from database), First Name, Last Name, Username, Email, Password, Date of Employment, Pavilion, Phone, Date of Birth, JMBG, Address, Title, Theme, and Language.\r\n\r\n" +
                        "   ▪ Ability to edit certain employee information by clicking the '✏️ Edit' button (light blue) for the selected employee (Person ID, Theme, and Language cannot be changed).\r\n\r\n" +
                        "   ▪ Ability to delete an account by clicking the '❌ Delete' button (red), which deletes the employee from the corresponding row.\r\n\r\n" +
                        "   ▪ Add a new employee account by clicking the '➕ Add employee' button, which opens a form for entering required employee data:\r\n\r\n" +
                        "       ▪ First Name\r\n\r\n" +
                        "       ▪ Last Name\r\n\r\n" +
                        "       ▪ Email\r\n\r\n" +
                        "       ▪ Username\r\n\r\n" +
                        "       ▪ Password, with the option to show or hide it by clicking 👁\r\n\r\n" +
                        "       ▪ Phone Number\r\n\r\n" +
                        "       ▪ JMBG (13 digits)\r\n\r\n" +
                        "       ▪ Title\r\n\r\n" +
                        "       ▪ Theme (dropdown menu: Dark, Light, Green)\r\n\r\n" +
                        "       ▪ Language (dropdown menu: Serbian, English)\r\n\r\n" +
                        "       ▪ Date of Birth (manual input in dd/mm/yyyy format or using the calendar icon)\r\n\r\n" +
                        "       ▪ Date of Employment (manual input in dd/mm/yyyy format or using the calendar icon)\r\n\r\n" +
                        "       ▪ Pavilion (dropdown: Pavilion 1, 2, 3, 4)\r\n\r\n" +
                        "       ▪ Residential Address\r\n\r\n" +
                        "       ▪ Confirm – button to confirm and create a new account\r\n\r\n" +
                        "◦ Instructions – The currently viewed instructions for using the application.\r\n\r\n" +
                        "🔘 Top-right corner of the application:\r\n" +
                        "In the top-right corner, the following buttons are available:\r\n\r\n" +
                        "◦ 👤 Profile – Opens a window to view and edit basic user data ('⚙ Customize profile') or log out ('🔓 Log Out'). Information is shown in a vertical form with input fields.\r\n\r\n" +
                        "   📌 Description of the '⚙ Customize profile' window:\r\n" +
                        "       ▪ Email: Displays the user's email address.\r\n\r\n" +
                        "       ▪ Password: Field to enter a new password, with controls to show/hide the input.\r\n\r\n" +
                        "       ▪ Person ID: Unique user identifier. Cannot be edited (read-only field).\r\n\r\n" +
                        "       ▪ First and Last Name: Fields to enter user's name and surname.\r\n\r\n" +
                        "       ▪ Username: Shows the username. Editable.\r\n\r\n" +
                        "       ▪ Phone Number: User's contact number.\r\n\r\n" +
                        "       ▪ Date of Birth: Displays the date in dd.MM.yyyy format.\r\n\r\n" +
                        "       ▪ Residential Address: Enter user's home address.\r\n\r\n" +
                        "       ▪ JMBG: Displays the unique citizen ID number. Editable (13 digits required).\r\n\r\n" +
                        "       ▪ Title: User's professional title or position.\r\n\r\n" +
                        "       ▪ Application Theme: Shows the currently active theme (Dark, Light, Green). Read-only.\r\n\r\n" +
                        "       ▪ Application Language: Shows the active language (Serbian or English). Read-only.\r\n\r\n" +
                        "       ▪ 'Save Changes' – button to save updated data to the database.\r\n\r\n" +
                        "   💡 Additional Features:\r\n" +
                        "   - The password field has a 👁 icon to toggle visibility.\r\n\r\n" +
                        "   - The fields Person ID, Theme, and Language are read-only – users can view but not edit them.\r\n\r\n" +
                        "◦ 🎨 Theme – Click this to change the application's appearance. Available themes:\r\n\r\n" +
                        "   ▪ 🌙 Dark\r\n\r\n" +
                        "   ▪ ☀ Light\r\n\r\n" +
                        "   ▪ 🌿 Green\r\n\r\n" +
                        "◦ 🌐 Language – Switches the application language between Serbian '🌐S' and English '🌐E'.\r\n\r\n" +
                        "◦ ➖ Minimize – Minimizes the app to the taskbar.\r\n\r\n" +
                        "◦ 🗖 Maximize / Restore – Maximizes or restores the app window.\r\n\r\n" +
                        "◦ ❌ Close – Closes the application.";
                }
                else if (userRole == "Zaposleni")
                {
                    UputstvoTekst.Text = "📄 User Guide – Employee (Logged-in User)\r\n" +
                        "Welcome!\r\n\r\n" +
                        "As an employee, you have access to basic information in the application, as well as the ability to add and delete content.\r\n\r\n" +
                        "🔘 Navigation:\r\n" +
                        "In the top-left corner, there is a menu button (☰). Clicking it opens a sidebar with the following items:\r\n\r\n" +
                        "◦ News – Overview of the latest announcements, posts, and information related to the student dormitory and students, with the ability to open PDF files, add new posts, and delete existing ones.\r\n" +
                        "To add a post, click '+ Add new post' and enter the post content along with the official PDF document. To delete a post, click 'Delete'.\r\n\r\n" +
                        "◦ Accommodation Applications – Information about currently active accommodation applications, with the ability to open PDF files, add new posts, and delete existing ones.\r\n" +
                        "To add a post, click '+ Add new post' and enter the post title, content, and official PDF document. To delete a post, click 'Delete'.\r\n\r\n" +
                        "◦ Student Accommodation – Basic information about capacities, rules, and conditions of accommodation in the dormitory, with the ability to add and delete posts.\r\n" +
                        "To add a post, click '+ Add new post' and enter the title and content of the post. To delete, click 'Delete'.\r\n\r\n" +
                        "◦ Students Restaurant – Working hours of the student canteen, meal times, and prices, with the ability to add and delete posts.\r\n" +
                        "To add a post, click '+ Add new post' and enter the content, time, and price for each meal. To delete, click 'Delete'.\r\n\r\n" +
                        "◦ Contacts – Contact information for dormitory administration or technical support, with the ability to add and delete contact posts.\r\n" +
                        "To add, click '+ Add contacts' and enter the content, address, phone number, email, Facebook and Instagram links. To delete, click 'Delete'.\r\n\r\n" +
                        "◦ Instructions – The currently viewed user guide for using the application.\r\n\r\n" +
                        "🔘 Top-right corner of the application:\r\n" +
                        "In the top-right corner, the following buttons are available:\r\n\r\n" +
                        "◦ 👤 Profile – This window allows viewing and editing basic user information (⚙ Customize profile), as well as logging out (🔓 Log out). The information is organized vertically and displayed through input fields (text boxes).\r\n\r\n" +
                        "   📌 Description of the '⚙ Customize profile' window:\r\n" +
                        "       ▪ Email: Displays the user's email address.\r\n\r\n" +
                        "       ▪ Password: Field for entering a new password, with additional controls to show or hide the entered text.\r\n\r\n" +
                        "       ▪ User ID: Unique user identifier. Cannot be edited (display only).\r\n\r\n" +
                        "       ▪ First Name and Last Name: Fields for entering the user's first and last name.\r\n\r\n" +
                        "       ▪ Username: Displays the user's username. Can be edited.\r\n\r\n" +
                        "       ▪ Phone Number: User's contact number.\r\n\r\n" +
                        "       ▪ Date of Birth: Displays the date in dd.MM.yyyy format.\r\n\r\n" +
                        "       ▪ Home Address: Field for entering the user's residence address.\r\n\r\n" +
                        "       ▪ JMBG: Displays the unique citizen ID number. Can be modified (must be 13 digits).\r\n\r\n" +
                        "       ▪ Title: Professional title or job position of the user.\r\n\r\n" +
                        "       ▪ Application Theme: Shows the currently active theme (e.g., Dark, Light, Green). This is a read-only field and cannot be edited here.\r\n\r\n" +
                        "       ▪ Application Language: Displays the currently active language (Serbian or English). This is also read-only.\r\n\r\n" +
                        "       ▪ Employment Date: (Hidden unless applicable to the user). Shows the date the employee was hired.\r\n\r\n" +
                        "       ▪ Pavilion: Displays the pavilion number the user is assigned to.\r\n\r\n" +
                        "       ▪ 'Save Changes' - button: Saves the changes to the database.\r\n\r\n" +
                        "   💡 Additional features:\r\n" +
                        "   - The password field includes an 👁 icon that allows the user to show or hide the entered password.\r\n\r\n" +
                        "   - The User ID, Theme, and Language fields are read-only – the user can view them but cannot edit them.\r\n\r\n" +
                        "◦ 🎨 Theme – Click this button to change the application's appearance. Three themes are available:\r\n\r\n" +
                        "   ▪ 🌙 Dark\r\n\r\n" +
                        "   ▪ ☀ Light\r\n\r\n" +
                        "   ▪ 🌿 Green\r\n\r\n" +
                        "◦ 🌐 Language – Allows you to switch the application language between Serbian '🌐S' and English '🌐E'.\r\n\r\n" +
                        "◦ ➖ Minimize – Minimizes the application window to the taskbar.\r\n\r\n" +
                        "◦ 🗖 Maximize / Restore – Maximizes or restores the application to its previous window size.\r\n\r\n" +
                        "◦ ❌ Close – Closes the application.";
                }
                else
                {
                    UputstvoTekst.Text = "📄 User Guide – Guest (Unregistered User)\r\n" +
                        "Welcome!\r\n\r\n" +
                        "As an unregistered user, you can view basic information in the application. To access additional features, you need to log in.\r\n\r\n" +
                        "🔘 Navigation:\r\n" +
                        "In the top-left corner, there is a menu button (☰). Clicking it opens a sidebar with the following items:\r\n\r\n" +
                        "◦ News – View the latest announcements, posts, and information related to the student dormitory and students with the option to open PDF files.\r\n\r\n" +
                        "◦ Accommodation Applications – Information about currently active accommodation application calls with the option to open PDF files.\r\n\r\n" +
                        "◦ Student Accommodation – Basic information about dormitory capacities, rules, and accommodation conditions.\r\n\r\n" +
                        "◦ Students Restaurant – Working hours of the student canteen, meal times, and prices.\r\n\r\n" +
                        "◦ Contacts – Contact information for dormitory administration or technical support.\r\n\r\n" +
                        "◦ Instructions – The currently displayed user guide for using the application.\r\n\r\n" +
                        "🔘 Top-right corner of the application:\r\n" +
                        "The top-right corner contains the following buttons:\r\n\r\n" +
                        "◦ 🔑 Login – Opens the login window. After a successful login, you gain access to additional features based on your role (employee or administrator).\r\n\r\n" +
                        "◦ 🎨 Theme – Allows you to change the visual theme of the application. Three themes are available:\r\n\r\n" +
                        "   ▪ 🌙 Dark\r\n\r\n" +
                        "   ▪ ☀ Light\r\n\r\n" +
                        "   ▪ 🌿 Green\r\n\r\n" +
                        "◦ 🌐 Language – Allows switching the application language between Serbian '🌐S' and English '🌐E'.\r\n\r\n" +
                        "◦ ➖ Minimize – Minimizes the application window to the taskbar.\r\n\r\n" +
                        "◦ 🗖 Maximize / Restore – Maximizes or restores the application to its previous window size.\r\n\r\n" +
                        "◦ ❌ Close – Closes the application.";

                }
            }
        }

    }
}
