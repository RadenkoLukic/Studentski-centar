using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.IO;
using System.Data;
using static LoginForm.DatabaseConnection;
using System.Windows.Controls;

namespace LoginForm
{
    public class DatabaseConnection
    {
        private static string connectionString = "Server=localhost;Database=HCI_domDB;Uid=root;Pwd=radenkomsql;";
        private MySqlConnection connection;

        public DatabaseConnection()
        {
            connection = new MySqlConnection(connectionString);
        }

        // Otvori vezu sa bazom
        public void OpenConnection()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    Console.WriteLine("Povezivanje sa bazom uspešno!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška prilikom povezivanja sa bazom: " + ex.Message);
            }
        }
        public void UpdateUserTheme(string email, string tema)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Direktno ažuriramo `Osoba.Tema` na osnovu `Email` iz `Admin` ili `Zaposleni`
                    string updateQuery = @"
                UPDATE Osoba 
                SET Tema = @Tema 
                WHERE OsobaID = (SELECT OsobaID FROM Admin WHERE Email = @Email 
                                 UNION 
                                 SELECT OsobaID FROM Zaposleni WHERE Email = @Email)";

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Tema", tema);
                        cmd.Parameters.AddWithValue("@Email", email);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("Korisnik nije pronađen!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri ažuriranju teme: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        public void UpdateUserLanguage(string email, string jezik)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Direktno ažuriramo `Osoba.Jezik` na osnovu `Email` iz `Admin` ili `Zaposleni`
                    string updateQuery = @"
                UPDATE Osoba 
                SET Jezik = @Jezik 
                WHERE OsobaID = (SELECT OsobaID FROM Admin WHERE Email = @Email 
                                 UNION 
                                 SELECT OsobaID FROM Zaposleni WHERE Email = @Email)";

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Jezik", jezik);
                        cmd.Parameters.AddWithValue("@Email", email);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            MessageBox.Show("Korisnik nije pronađen!", "Greška", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri ažuriranju jezika: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public string GetUserTheme(string email)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                SELECT Tema FROM Osoba 
                WHERE OsobaID = (SELECT OsobaID FROM Admin WHERE Email = @Email 
                                 UNION 
                                 SELECT OsobaID FROM Zaposleni WHERE Email = @Email)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            return result.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri učitavanju teme: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return "Tamna"; // Ako ne pronađe podatak, podrazumevano je Tamna
        }

        public string GetUserLanguage(string email)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                SELECT Jezik FROM Osoba 
                WHERE OsobaID = (SELECT OsobaID FROM Admin WHERE Email = @Email 
                                 UNION 
                                 SELECT OsobaID FROM Zaposleni WHERE Email = @Email)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            return result.ToString(); // Vraća "Srpski" ili "Engleski"
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri učitavanju jezika: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return "Srpski"; // Ako ne pronađe podatak, podrazumevano je Tamna
        }


        // Zatvori vezu sa bazom
        public void CloseConnection()
        {
            try
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                    Console.WriteLine("Veza sa bazom je zatvorena.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška prilikom zatvaranja veze sa bazom: " + ex.Message);
            }
        }

        public class Zaposleni
        {
            public int OsobaID { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Sifra { get; set; }
            public DateTime DatumZaposlenja { get; set; }
            public string Paviljon { get; set; }
            public string Ime { get; set; }
            public string Prezime { get; set; }
            public string BrojTelefona { get; set; }
            public DateTime? DatumRodjenja { get; set; }
            public string AdresaStanovanja { get; set; }
            public string JMBG { get; set; }
            public string Zvanje { get; set; }
            public string Tema { get; set; }
            public string Jezik { get; set; }
        }
        public void PrikaziSvePodatke(Zaposleni zaposleni)
        {
            string poruka = $"🆔 OsobaID: {zaposleni.OsobaID}\n" +
                            $"👤 Username: {zaposleni.Username}\n" +
                            $"📧 Email: {zaposleni.Email}\n" +
                            $"🔑 Šifra: {zaposleni.Sifra}\n" +
                            $"📅 Datum zaposlenja: {zaposleni.DatumZaposlenja:dd.MM.yyyy}\n" +
                            $"🏢 Paviljon: {zaposleni.Paviljon}\n" +
                            $"📛 Ime: {zaposleni.Ime}\n" +
                            $"👨‍👩‍👧 Prezime: {zaposleni.Prezime}\n" +
                            $"📱 Broj telefona: {zaposleni.BrojTelefona}\n" +
                            $"🎂 Datum rođenja: {(zaposleni.DatumRodjenja.HasValue ? zaposleni.DatumRodjenja.Value.ToString("dd.MM.yyyy") : "N/A")}\n" +
                            $"🏠 Adresa stanovanja: {zaposleni.AdresaStanovanja}\n" +
                            $"🆔 JMBG: {zaposleni.JMBG}\n" +
                            $"🎓 Zvanje: {zaposleni.Zvanje}\n" +
                            $"🎨 Tema: {zaposleni.Tema}\n" +
                            $"🗣️ Jezik: {zaposleni.Jezik}";

            MessageBox.Show(poruka, "Detalji zaposlenog", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        public class Admin
        {
            public int OsobaID { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Sifra { get; set; }

            public string Ime { get; set; }
            public string Prezime { get; set; }
            public string BrojTelefona { get; set; }
            public DateTime? DatumRodjenja { get; set; }
            public string AdresaStanovanja { get; set; }
            public string JMBG { get; set; }
            public string Zvanje { get; set; }
            public string Tema { get; set; }
            public string Jezik { get; set; }
        }
        public static Admin GetAdminData()
        {
            Admin admin = null;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                SELECT 
                o.OsobaID, o.Ime, o.Prezime, o.BrojTelefona, o.DatumRodjenja, 
                o.AdresaStanovanja, o.JMBG, o.Zvanje, o.Tema, o.Jezik,
                a.Username, a.Email, a.Sifra
                FROM Admin a
                INNER JOIN Osoba o ON a.OsobaID = o.OsobaID
                WHERE a.OsobaID = 1";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        admin = new Admin
                        {
                            OsobaID = reader.GetInt32(reader.GetOrdinal("OsobaID")),
                            Username = reader.GetString(reader.GetOrdinal("Username")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Sifra = reader.GetString(reader.GetOrdinal("Sifra")),
                            Ime = reader.GetString(reader.GetOrdinal("Ime")),
                            Prezime = reader.GetString(reader.GetOrdinal("Prezime")),
                            BrojTelefona = reader.IsDBNull(reader.GetOrdinal("BrojTelefona")) ? null : reader.GetString(reader.GetOrdinal("BrojTelefona")),
                            DatumRodjenja = reader.IsDBNull(reader.GetOrdinal("DatumRodjenja")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DatumRodjenja")),
                            AdresaStanovanja = reader.IsDBNull(reader.GetOrdinal("AdresaStanovanja")) ? null : reader.GetString(reader.GetOrdinal("AdresaStanovanja")),
                            Zvanje = reader.IsDBNull(reader.GetOrdinal("Zvanje")) ? null : reader.GetString(reader.GetOrdinal("Zvanje")),
                            JMBG = reader.GetString(reader.GetOrdinal("JMBG")),
                            Tema = reader.GetString(reader.GetOrdinal("Tema")),
                            Jezik = reader.GetString(reader.GetOrdinal("Jezik"))

                        };
                    }
                }

                conn.Close();
            }

            return admin;
        }

        public static Zaposleni GetZaposleniData(string email)
        {

            Zaposleni zaposleni = null;
            string query = @"
                SELECT 
                    o.OsobaID,
                    o.Ime,
                    o.Prezime,
                    o.BrojTelefona,
                    o.DatumRodjenja,
                    o.AdresaStanovanja,
                    o.JMBG,
                    o.Zvanje,
                    o.Tema,
                    o.Jezik,
                    z.Username,
                    z.Email,
                    z.Sifra,
                    z.DatumZaposlenja,
                    z.Paviljon
                FROM Osoba o
                JOIN Zaposleni z ON o.OsobaID = z.OsobaID
                WHERE z.Email = @email";

            try
            {

                MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@email", email);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {

                    zaposleni = new Zaposleni
                    {
                        OsobaID = reader.GetInt32(reader.GetOrdinal("OsobaID")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Sifra = reader.GetString(reader.GetOrdinal("Sifra")),
                        DatumZaposlenja = reader.GetDateTime(reader.GetOrdinal("DatumZaposlenja")),
                        Paviljon = reader.GetString(reader.GetOrdinal("Paviljon")),
                        Ime = reader.GetString(reader.GetOrdinal("Ime")),
                        Prezime = reader.GetString(reader.GetOrdinal("Prezime")),
                        BrojTelefona = reader.IsDBNull(reader.GetOrdinal("BrojTelefona")) ? null : reader.GetString(reader.GetOrdinal("BrojTelefona")),
                        DatumRodjenja = reader.IsDBNull(reader.GetOrdinal("DatumRodjenja")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DatumRodjenja")),
                        AdresaStanovanja = reader.IsDBNull(reader.GetOrdinal("AdresaStanovanja")) ? null : reader.GetString(reader.GetOrdinal("AdresaStanovanja")),
                        Zvanje = reader.IsDBNull(reader.GetOrdinal("Zvanje")) ? null : reader.GetString(reader.GetOrdinal("Zvanje")),
                        JMBG = reader.GetString(reader.GetOrdinal("JMBG")),
                        Tema = reader.GetString(reader.GetOrdinal("Tema")),
                        Jezik = reader.GetString(reader.GetOrdinal("Jezik"))
                    };

                }
                conn.Close();
                return zaposleni;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška: " + ex.Message);
                return null;

            }


        }
        public bool InsertZaposleni(Zaposleni zaposleni)
        {

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Osoba (Ime, Prezime ,BrojTelefona, DatumRodjenja, AdresaStanovanja, JMBG, Zvanje, Tema, Jezik) " +
                                   "VALUES (@Ime, @Prezime, @BrojTelefona, @DatumRodjenja, @AdresaStanovanja, @JMBG, @Zvanje, @Tema, @Jezik); " +
                                   "SET @OsobaID = LAST_INSERT_ID(); " +
                                   "INSERT INTO Zaposleni (OsobaID, Username, Email, Sifra, DatumZaposlenja, Paviljon) " +
                                   "VALUES (@OsobaID, @Username, @Email, @Sifra, @DatumZaposlenja, @Paviljon);";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Ime", zaposleni.Ime);
                    cmd.Parameters.AddWithValue("@Prezime", zaposleni.Prezime);
                    cmd.Parameters.AddWithValue("@BrojTelefona", zaposleni.BrojTelefona);
                    cmd.Parameters.AddWithValue("@DatumRodjenja", zaposleni.DatumRodjenja);
                    cmd.Parameters.AddWithValue("@AdresaStanovanja", zaposleni.AdresaStanovanja);
                    cmd.Parameters.AddWithValue("@JMBG", zaposleni.JMBG);
                    cmd.Parameters.AddWithValue("@Zvanje", zaposleni.Zvanje);
                    cmd.Parameters.AddWithValue("@Tema", zaposleni.Tema);
                    cmd.Parameters.AddWithValue("@Jezik", zaposleni.Jezik);

                    cmd.Parameters.AddWithValue("@Username", zaposleni.Username);
                    cmd.Parameters.AddWithValue("@Email", zaposleni.Email);
                    cmd.Parameters.AddWithValue("@Sifra", zaposleni.Sifra);
                    cmd.Parameters.AddWithValue("@DatumZaposlenja", zaposleni.DatumZaposlenja);
                    cmd.Parameters.AddWithValue("@Paviljon", zaposleni.Paviljon);


                    int result = cmd.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri dodavanju zaposlenog: " + ex.Message);
                    return false;
                }
            }
        } //ima
        public bool InsertZaposleni1(Zaposleni zaposleni)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlTransaction transaction = conn.BeginTransaction(); // Početak transakcije

                    // 1. Ubacujemo osobu u tabelu Osoba
                    string queryOsoba = @"
                INSERT INTO Osoba (Ime, Prezime, BrojTelefona, DatumRodjenja, AdresaStanovanja, JMBG, Zvanje, Tema, Jezik) 
                VALUES (@Ime, @Prezime, @BrojTelefona, @DatumRodjenja, @AdresaStanovanja, @JMBG, @Zvanje, @Tema, @Jezik);
                SELECT LAST_INSERT_ID();"; // Dohvatanje poslednje unetog OsobaID

                    int osobaID;
                    using (MySqlCommand cmdOsoba = new MySqlCommand(queryOsoba, conn, transaction))
                    {
                        cmdOsoba.Parameters.AddWithValue("@Ime", zaposleni.Ime);
                        cmdOsoba.Parameters.AddWithValue("@Prezime", zaposleni.Prezime);
                        cmdOsoba.Parameters.AddWithValue("@BrojTelefona", string.IsNullOrEmpty(zaposleni.BrojTelefona) ? (object)DBNull.Value : zaposleni.BrojTelefona);
                        cmdOsoba.Parameters.AddWithValue("@DatumRodjenja", zaposleni.DatumRodjenja.HasValue ? zaposleni.DatumRodjenja.Value : (object)DBNull.Value);
                        cmdOsoba.Parameters.AddWithValue("@AdresaStanovanja", string.IsNullOrEmpty(zaposleni.AdresaStanovanja) ? (object)DBNull.Value : zaposleni.AdresaStanovanja);
                        cmdOsoba.Parameters.AddWithValue("@JMBG", string.IsNullOrEmpty(zaposleni.JMBG) ? (object)DBNull.Value : zaposleni.JMBG);
                        cmdOsoba.Parameters.AddWithValue("@Zvanje", string.IsNullOrEmpty(zaposleni.Zvanje) ? (object)DBNull.Value : zaposleni.Zvanje);
                        cmdOsoba.Parameters.AddWithValue("@Tema", zaposleni.Tema);
                        cmdOsoba.Parameters.AddWithValue("@Jezik", zaposleni.Jezik);

                        osobaID = Convert.ToInt32(cmdOsoba.ExecuteScalar()); // Dobijanje ID-a nove osobe
                    }

                    // 2. Ubacujemo zaposlenog u tabelu Zaposleni
                    string queryZaposleni = @"
                INSERT INTO Zaposleni (OsobaID, Username, Email, Sifra, DatumZaposlenja, Paviljon) 
                VALUES (@OsobaID, @Username, @Email, @Sifra, @DatumZaposlenja, @Paviljon);";
                    string hashedPassword = HashPassword(zaposleni.Sifra);
                    using (MySqlCommand cmdZaposleni = new MySqlCommand(queryZaposleni, conn, transaction))
                    {
                        cmdZaposleni.Parameters.AddWithValue("@OsobaID", osobaID);
                        cmdZaposleni.Parameters.AddWithValue("@Username", zaposleni.Username);
                        cmdZaposleni.Parameters.AddWithValue("@Email", zaposleni.Email);
                        cmdZaposleni.Parameters.AddWithValue("@Sifra", hashedPassword);
                        cmdZaposleni.Parameters.AddWithValue("@DatumZaposlenja", zaposleni.DatumZaposlenja);
                        cmdZaposleni.Parameters.AddWithValue("@Paviljon", zaposleni.Paviljon);

                        cmdZaposleni.ExecuteNonQuery(); // Izvršavanje upita
                    }

                    transaction.Commit(); // Potvrda transakcije
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Greška prilikom unosa zaposlenog: " + ex.Message);
                    return false;
                }
            }
        }
        public bool DeleteZaposleni(int osobaID)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Osoba WHERE OsobaID = @OsobaID";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OsobaID", osobaID);
                    int affectedRows = command.ExecuteNonQuery();
                    return affectedRows > 0;
                }
            }
        } //ima
        public List<Zaposleni> GetZaposleniList()
        {
            List<Zaposleni> zaposleniList = new List<Zaposleni>();

            // Konekcija ka bazi podataka
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // SQL upit koji spaja tabele Zaposleni i Osoba
                string query = @"
                SELECT 
                    Z.OsobaID, 
                    Z.Username, 
                    Z.Email, 
                    Z.Sifra, 
                    Z.DatumZaposlenja, 
                    Z.Paviljon,
                    O.Ime, 
                    O.Prezime, 
                    O.BrojTelefona, 
                    O.DatumRodjenja, 
                    O.AdresaStanovanja, 
                    O.JMBG, 
                    O.Zvanje,
                    O.Tema,
                    O.Jezik
                FROM Zaposleni Z
                INNER JOIN Osoba O ON Z.OsobaID = O.OsobaID";

                // Komanda za izvršavanje upita
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();

                    // Čitanje podataka iz baze
                    while (reader.Read())
                    {
                        Zaposleni zaposleni = new Zaposleni
                        {
                            OsobaID = reader.GetInt32(reader.GetOrdinal("OsobaID")),
                            Username = reader.GetString(reader.GetOrdinal("Username")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Sifra = reader.GetString(reader.GetOrdinal("Sifra")),
                            DatumZaposlenja = reader.GetDateTime(reader.GetOrdinal("DatumZaposlenja")),
                            Paviljon = reader.GetString(reader.GetOrdinal("Paviljon")),
                            Ime = reader.GetString(reader.GetOrdinal("Ime")),
                            Prezime = reader.GetString(reader.GetOrdinal("Prezime")),
                            BrojTelefona = reader.IsDBNull(reader.GetOrdinal("BrojTelefona")) ? null : reader.GetString(reader.GetOrdinal("BrojTelefona")),
                            DatumRodjenja = reader.IsDBNull(reader.GetOrdinal("DatumRodjenja")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DatumRodjenja")),
                            AdresaStanovanja = reader.IsDBNull(reader.GetOrdinal("AdresaStanovanja")) ? null : reader.GetString(reader.GetOrdinal("AdresaStanovanja")),
                            Zvanje = reader.IsDBNull(reader.GetOrdinal("Zvanje")) ? null : reader.GetString(reader.GetOrdinal("Zvanje")),
                            JMBG = reader.GetString(reader.GetOrdinal("JMBG")),
                            Tema = reader.GetString(reader.GetOrdinal("Tema")),
                            Jezik = reader.GetString(reader.GetOrdinal("Jezik"))
                        };

                        zaposleniList.Add(zaposleni);
                    }
                }
            }
            CloseConnection();
            return zaposleniList;
        } //ima
        public void UpdateZaposleni(Zaposleni zaposleni)
        {
            OpenConnection();
            // Početno ažuriranje podataka u tabeli Zaposleni
            string queryZaposleni = "";
            string hashedPassword = "";

            if (string.IsNullOrWhiteSpace(zaposleni.Sifra))
            {
                queryZaposleni = "UPDATE Zaposleni SET " +
                            "Username = @Username, " +
                            "Email = @Email, " +
                            "DatumZaposlenja = @DatumZaposlenja, " +
                            "Paviljon = @Paviljon " +
                            "WHERE OsobaID = @OsobaID";
                //PrikaziSvePodatke(zaposleni);
                MySqlCommand cmdZaposleni = new MySqlCommand(queryZaposleni, connection);
                cmdZaposleni.Parameters.AddWithValue("@Username", zaposleni.Username);
                cmdZaposleni.Parameters.AddWithValue("@Email", zaposleni.Email);
                if (hashedPassword != "") { cmdZaposleni.Parameters.AddWithValue("@Sifra", hashedPassword); }
                cmdZaposleni.Parameters.AddWithValue("@DatumZaposlenja", zaposleni.DatumZaposlenja);
                cmdZaposleni.Parameters.AddWithValue("@Paviljon", zaposleni.Paviljon);
                cmdZaposleni.Parameters.AddWithValue("@OsobaID", zaposleni.OsobaID);

                cmdZaposleni.ExecuteNonQuery();
            }
            else
            {
                queryZaposleni = "UPDATE Zaposleni SET " +
                              "Username = @Username, " +
                              "Email = @Email, " +
                              "Sifra = @Sifra, " +
                              "DatumZaposlenja = @DatumZaposlenja, " +
                              "Paviljon = @Paviljon " +
                              "WHERE OsobaID = @OsobaID";
                
                hashedPassword = HashPassword(zaposleni.Sifra);
                MySqlCommand cmdZaposleni = new MySqlCommand(queryZaposleni, connection);
                cmdZaposleni.Parameters.AddWithValue("@Username", zaposleni.Username);
                cmdZaposleni.Parameters.AddWithValue("@Email", zaposleni.Email);
                if (hashedPassword != "") { cmdZaposleni.Parameters.AddWithValue("@Sifra", hashedPassword); }
                cmdZaposleni.Parameters.AddWithValue("@DatumZaposlenja", zaposleni.DatumZaposlenja);
                cmdZaposleni.Parameters.AddWithValue("@Paviljon", zaposleni.Paviljon);
                cmdZaposleni.Parameters.AddWithValue("@OsobaID", zaposleni.OsobaID);

                cmdZaposleni.ExecuteNonQuery();
            }
            PrikaziSvePodatke(zaposleni);
            CloseConnection();
            OpenConnection();

            // Ažuriranje podataka u tabeli Osoba
            string queryOsoba = "UPDATE Osoba SET " +
                                "Ime = @Ime, " +
                                "Prezime = @Prezime, " +
                                "BrojTelefona = @BrojTelefona, " +
                                "DatumRodjenja = @DatumRodjenja, " +
                                "AdresaStanovanja = @AdresaStanovanja, " +
                                "JMBG = @JMBG, " +
                                "Zvanje = @Zvanje, " +
                                "Tema = @Tema, " +
                                "Jezik = @Jezik " +
                                "WHERE OsobaID = @OsobaID";

            MySqlCommand cmdOsoba = new MySqlCommand(queryOsoba, connection);
            cmdOsoba.Parameters.AddWithValue("@Ime", zaposleni.Ime);
            cmdOsoba.Parameters.AddWithValue("@Prezime", zaposleni.Prezime);
            cmdOsoba.Parameters.AddWithValue("@BrojTelefona", zaposleni.BrojTelefona);
            cmdOsoba.Parameters.AddWithValue("@DatumRodjenja", zaposleni.DatumRodjenja);
            cmdOsoba.Parameters.AddWithValue("@AdresaStanovanja", zaposleni.AdresaStanovanja);
            cmdOsoba.Parameters.AddWithValue("@JMBG", zaposleni.JMBG);
            cmdOsoba.Parameters.AddWithValue("@Zvanje", zaposleni.Zvanje);
            cmdOsoba.Parameters.AddWithValue("@Tema", zaposleni.Tema);
            cmdOsoba.Parameters.AddWithValue("@Jezik", zaposleni.Jezik);
            cmdOsoba.Parameters.AddWithValue("@OsobaID", zaposleni.OsobaID);

            cmdOsoba.ExecuteNonQuery();
            CloseConnection();
        } //ima

        public void UpdateAdmin(Admin admin)
        {
            OpenConnection();
            string queryAdmin = "";
            string hashedPassword = "";
            // Početno ažuriranje podataka u tabeli Zaposleni
            if (string.IsNullOrWhiteSpace(admin.Sifra))
            {
                queryAdmin = "UPDATE Admin SET " +
                          "Username = @Username, " +
                          "Email = @Email " +
                          "WHERE OsobaID = @OsobaID";
            }
            else
            {
                queryAdmin = "UPDATE Admin SET " +
                              "Username = @Username, " +
                              "Email = @Email, " +
                              "Sifra = @Sifra " +
                              "WHERE OsobaID = @OsobaID";
                hashedPassword = HashPassword(admin.Sifra);
            }


            MySqlCommand cmdAdmin = new MySqlCommand(queryAdmin, connection);
            cmdAdmin.Parameters.AddWithValue("@Username", admin.Username);
            cmdAdmin.Parameters.AddWithValue("@Email", admin.Email);
            if (hashedPassword != "") { cmdAdmin.Parameters.AddWithValue("@Sifra", hashedPassword); }
            cmdAdmin.Parameters.AddWithValue("@OsobaID", admin.OsobaID);

            cmdAdmin.ExecuteNonQuery();
            CloseConnection();
            OpenConnection();

            // Ažuriranje podataka u tabeli Osoba
            string queryOsoba = "UPDATE Osoba SET " +
                                "Ime = @Ime, " +
                                "Prezime = @Prezime, " +
                                "BrojTelefona = @BrojTelefona, " +
                                "DatumRodjenja = @DatumRodjenja, " +
                                "AdresaStanovanja = @AdresaStanovanja, " +
                                "JMBG = @JMBG, " +
                                "Zvanje = @Zvanje, " +
                                "Tema = @Tema, " +
                                "Jezik = @Jezik " +
                                "WHERE OsobaID = @OsobaID";

            MySqlCommand cmdOsoba = new MySqlCommand(queryOsoba, connection);
            cmdOsoba.Parameters.AddWithValue("@Ime", admin.Ime);
            cmdOsoba.Parameters.AddWithValue("@Prezime", admin.Prezime);
            cmdOsoba.Parameters.AddWithValue("@BrojTelefona", admin.BrojTelefona);
            cmdOsoba.Parameters.AddWithValue("@DatumRodjenja", admin.DatumRodjenja);
            cmdOsoba.Parameters.AddWithValue("@AdresaStanovanja", admin.AdresaStanovanja);
            cmdOsoba.Parameters.AddWithValue("@JMBG", admin.JMBG);
            cmdOsoba.Parameters.AddWithValue("@Zvanje", admin.Zvanje);
            cmdOsoba.Parameters.AddWithValue("@Tema", admin.Tema);
            cmdOsoba.Parameters.AddWithValue("@Jezik", admin.Jezik);
            cmdOsoba.Parameters.AddWithValue("@OsobaID", admin.OsobaID);

            cmdOsoba.ExecuteNonQuery();
            CloseConnection();
        }


        // Izvrši SQL upit koji ne vraća rezultate (INSERT, UPDATE, DELETE)
        public void ExecuteQuery(string query)
        {
            try
            {
                OpenConnection();

                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();

                Console.WriteLine("Upit je uspešno izvršen.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška prilikom izvršavanja upita: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }


        // Izvrši SQL upit koji vraća podatke (SELECT)
        public DataTable ExecuteSelectQuery(string query)
        {
            DataTable dataTable = new DataTable();

            try
            {
                OpenConnection();

                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(query, connection);
                dataAdapter.Fill(dataTable);

                Console.WriteLine("Podaci su uspešno preuzeti.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška prilikom preuzimanja podataka: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return dataTable;
        }


        // Metoda za proveru korisničkih kredencijala
        public string CheckUserCredentials(string userInput, string hashedPassword)
        {
            string queryAdmin = "SELECT 'Admin' AS Role FROM Admin WHERE Email = @userInput AND Sifra = @hashedPassword";
            string queryEmployee = "SELECT 'Zaposleni' AS Role FROM Zaposleni WHERE Email = @userInput AND Sifra = @hashedPassword";

            try
            {
                OpenConnection();

                MySqlCommand cmdAdmin = new MySqlCommand(queryAdmin, connection);
                cmdAdmin.Parameters.AddWithValue("@userInput", userInput);
                cmdAdmin.Parameters.AddWithValue("@hashedPassword", hashedPassword);

                object resultAdmin = cmdAdmin.ExecuteScalar(); // Proveravamo admina

                if (resultAdmin != null)
                {
                    return "Admin"; // Ako je korisnik admin, vraćamo "Admin"
                }

                MySqlCommand cmdEmployee = new MySqlCommand(queryEmployee, connection);
                cmdEmployee.Parameters.AddWithValue("@userInput", userInput);
                cmdEmployee.Parameters.AddWithValue("@hashedPassword", hashedPassword);

                object resultEmployee = cmdEmployee.ExecuteScalar(); // Proveravamo zaposlenog

                if (resultEmployee != null)
                {
                    return "Zaposleni"; // Ako je korisnik zaposlen, vraćamo "Zaposleni"
                }

                return null; // Ako korisnik nije pronađen ni u jednoj tabeli
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška prilikom provere kredencijala: " + ex.Message);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }



        // Metoda za heširanje lozinke
        public string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convert the password string to a byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the byte array to a string
                StringBuilder builder = new StringBuilder();
                foreach (byte byteValue in bytes)
                {
                    builder.Append(byteValue.ToString("x2"));
                }

                return builder.ToString();
            }
        }



        // Metoda za unos admina sa email-om i heširanom lozinkom
        public void InsertAdmin(string email, string password)
        {
            try
            {
                // Heširaj lozinku pre nego što je uneseš
                string password1 = "admin123";
                string email1 = "admin@gmail.com";
                string hashedPassword = HashPassword(password1);
                string id = "1";
                string username = "admin";


                // SQL upit za unos novog admina
                string query = "INSERT INTO Admin (OsobaID, Username, Email, Sifra) VALUES (@OsobaID, @Username, @Email, @Sifra)";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@OsobaID", id);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Email", email1);
                command.Parameters.AddWithValue("@Sifra", hashedPassword);

                // Otvori vezu sa bazom i izvrši upit
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri unosu admina: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public string GetUserRole(string email, string password)
        {
            string role = string.Empty;

            try
            {
                // Poveži se sa bazom podataka
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Upit koji proverava da li je korisnik admin
                    string query = @"
                SELECT 'Admin' 
                FROM Admin 
                WHERE Email = @Email AND Sifra = @Password
                UNION
                SELECT 'Zaposleni' 
                FROM Zaposleni 
                WHERE Email = @Email AND Sifra = @Password;
            ";

                    // Kreiraj MySqlCommand objekat
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", password);

                        // Izvrši upit i pročitaj rezultat
                        var result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            role = result.ToString(); // Vraća 'Admin' ili 'Zaposleni'
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri povezivanju sa bazom: " + ex.Message);
            }

            return role;
        }

        public class OglasModel
        {
            public int OglasID { get; set; }
            public string Naslov { get; set; }
            public string Dokument { get; set; }
            public DateTime DatumObjave { get; set; }
            public string ImePrezime { get; set; } // Spojeno ime i prezime
        }
        public static List<OglasModel> UcitajOglase()
        {
            List<OglasModel> oglasi = new List<OglasModel>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT O.OglasID, O.Naslov, A.Dokument, O.DatumObjave, Os.Ime, Os.Prezime 
                         FROM Oglas O
                         INNER JOIN Aktuelnosti A ON O.OglasID = A.OglasID
                         INNER JOIN Zaposleni Z ON O.OsobaID = Z.OsobaID
                         INNER JOIN Osoba Os ON Z.OsobaID = Os.OsobaID
                         ORDER BY O.DatumObjave DESC";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    oglasi.Add(new OglasModel
                    {
                        OglasID = reader.GetInt32(0),
                        Naslov = reader.GetString(1),
                        Dokument = reader.GetString(2),
                        DatumObjave = reader.GetDateTime(3),
                        ImePrezime = $"{reader.GetString(4)} {reader.GetString(5)}"
                    });
                }
            }

            return oglasi;
        }
        public static void ObrisiOglasIzBaze(int oglasID)
        {
            string queryGetFilePath = "SELECT Dokument FROM Aktuelnosti WHERE OglasID = @OglasID";
            string queryAktuelnosti = "DELETE FROM Aktuelnosti WHERE OglasID = @OglasID";
            string queryOglas = "DELETE FROM Oglas WHERE OglasID = @OglasID";
            string? pdfPath = null;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1️. Dobavljanje putanje do PDF fajla
                        using (MySqlCommand cmdGetFilePath = new MySqlCommand(queryGetFilePath, conn, transaction))
                        {
                            cmdGetFilePath.Parameters.AddWithValue("@OglasID", oglasID);
                            var result = cmdGetFilePath.ExecuteScalar();
                            if (result != null)
                            {
                                pdfPath = result.ToString();
                            }
                        }

                        // 2️. Brisanje zapisa iz Aktuelnosti
                        using (MySqlCommand cmdAktuelnosti = new MySqlCommand(queryAktuelnosti, conn, transaction))
                        {
                            cmdAktuelnosti.Parameters.AddWithValue("@OglasID", oglasID);
                            cmdAktuelnosti.ExecuteNonQuery();
                        }

                        // 3️. Brisanje zapisa iz Oglas
                        using (MySqlCommand cmdOglas = new MySqlCommand(queryOglas, conn, transaction))
                        {
                            cmdOglas.Parameters.AddWithValue("@OglasID", oglasID);
                            cmdOglas.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        // 4️. Ako PDF fajl postoji, brišemo ga sa diska
                        if (!string.IsNullOrEmpty(pdfPath) && File.Exists(pdfPath))
                        {
                            File.Delete(pdfPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Greška prilikom brisanja oglasa: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        public class KonkursModel
        {
            public int OglasID { get; set; }
            public string Naslov { get; set; }
            public string Dokument { get; set; }
            public string SadrzajOglasa { get; set; }
            public DateTime DatumObjave { get; set; }
            public string ImePrezime { get; set; } // Spojeno ime i prezime
        }
        public static List<KonkursModel> UcitajKonkurse()
        {
            List<KonkursModel> konkursi = new List<KonkursModel>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT O.OglasID, O.Naslov, K.SadrzajOglasa, K.Dokument, O.DatumObjave, Os.Ime, Os.Prezime 
                         FROM Oglas O
                         INNER JOIN Konkursi K ON O.OglasID = K.OglasID
                         INNER JOIN Zaposleni Z ON O.OsobaID = Z.OsobaID
                         INNER JOIN Osoba Os ON Z.OsobaID = Os.OsobaID
                         ORDER BY O.DatumObjave DESC";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    konkursi.Add(new KonkursModel
                    {
                        OglasID = reader.GetInt32(reader.GetOrdinal("OglasID")),
                        Naslov = reader.GetString(reader.GetOrdinal("Naslov")),
                        DatumObjave = reader.GetDateTime(reader.GetOrdinal("DatumObjave")),
                        SadrzajOglasa = reader.IsDBNull(reader.GetOrdinal("SadrzajOglasa")) ? null : reader.GetString(reader.GetOrdinal("SadrzajOglasa")),
                        Dokument = reader.IsDBNull(reader.GetOrdinal("Dokument")) ? null : reader.GetString(reader.GetOrdinal("Dokument")),
                        ImePrezime = $"{reader.GetString(reader.GetOrdinal("Ime"))} {reader.GetString(reader.GetOrdinal("Prezime"))}"
                    });
                }
            }

            return konkursi;
        }
        public static void ObrisiKonkursIzBaze(int oglasID)
        {
            string queryGetFilePath = "SELECT Dokument FROM Konkursi WHERE OglasID = @OglasID";
            string queryKonkursi = "DELETE FROM Konkursi WHERE OglasID = @OglasID";
            string queryOglas = "DELETE FROM Oglas WHERE OglasID = @OglasID";
            string? pdfPath = null;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1️. Dobavljanje putanje do PDF fajla
                        using (MySqlCommand cmdGetFilePath = new MySqlCommand(queryGetFilePath, conn, transaction))
                        {
                            cmdGetFilePath.Parameters.AddWithValue("@OglasID", oglasID);
                            var result = cmdGetFilePath.ExecuteScalar();
                            if (result != null)
                            {
                                pdfPath = result.ToString();
                            }
                        }

                        // 2️. Brisanje zapisa iz Konkursi
                        using (MySqlCommand cmdKonkursi = new MySqlCommand(queryKonkursi, conn, transaction))
                        {
                            cmdKonkursi.Parameters.AddWithValue("@OglasID", oglasID);
                            cmdKonkursi.ExecuteNonQuery();
                        }

                        // 3️. Brisanje zapisa iz Oglas
                        using (MySqlCommand cmdOglas = new MySqlCommand(queryOglas, conn, transaction))
                        {
                            cmdOglas.Parameters.AddWithValue("@OglasID", oglasID);
                            cmdOglas.ExecuteNonQuery();
                        }

                        transaction.Commit();

                        // 4️. Ako PDF fajl postoji, brišemo ga sa diska
                        if (!string.IsNullOrEmpty(pdfPath) && File.Exists(pdfPath))
                        {
                            File.Delete(pdfPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show($"Greška prilikom brisanja oglasa: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }



        public class MenzaModel
        {
            public int MenzaID { get; set; }
            public string Tekst { get; set; }
            public decimal CijenaDorucka { get; set; }
            public decimal CijenaRucka { get; set; }
            public decimal CijenaVecere { get; set; }
            public string TerminDorucka { get; set; }
            public string TerminRucka { get; set; }
            public string TerminVecere { get; set; }
            public DateTime DatumObjave { get; set; }
            public string ImePrezime { get; set; }
        }
        public static List<MenzaModel> UcitajMenza()
        {
            List<MenzaModel> menzaLista = new List<MenzaModel>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT m.MenzaID, m.Tekst, m.CijenaDorucka, m.CijenaRucka, m.CijenaVecere, 
                            m.TerminDorucka, m.TerminRucka, m.TerminVecere, m.DatumObjave, 
                            o.Ime, o.Prezime
                            FROM Menza m
                            INNER JOIN Zaposleni z ON m.OsobaID = z.OsobaID
                            INNER JOIN Osoba o ON z.OsobaID = o.OsobaID;";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                menzaLista.Add(new MenzaModel
                                {
                                    MenzaID = reader.GetInt32(0),
                                    Tekst = reader.GetString(1),
                                    CijenaDorucka = reader.GetDecimal(2),
                                    CijenaRucka = reader.GetDecimal(3),
                                    CijenaVecere = reader.GetDecimal(4),
                                    TerminDorucka = reader.GetString(5),
                                    TerminRucka = reader.GetString(6),
                                    TerminVecere = reader.GetString(7),
                                    DatumObjave = reader.GetDateTime(8),
                                    ImePrezime = $"{reader.GetString(9)} {reader.GetString(10)}"
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška pri učitavanju podataka: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return menzaLista;
        }
        public static bool ObrisiMenza(int menzaID)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Menza WHERE MenzaID = @MenzaID";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@MenzaID", menzaID);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0; // Vraća true ako je nešto obrisano
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška pri brisanju: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }


        public class StudentskiSmjestajModel
        {
            public int OglasID { get; set; }
            public string Naslov { get; set; }
            public string TekstOglasa { get; set; }
            public DateTime DatumObjave { get; set; }
            public string ImePrezime { get; set; } // Ime i prezime zaposlenog koji je objavio
        }
        public static List<StudentskiSmjestajModel> UcitajStudentskiSmjestaj()
        {
            List<StudentskiSmjestajModel> oglasi = new List<StudentskiSmjestajModel>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"SELECT O.OglasID, O.Naslov, S.TekstOglasa, O.DatumObjave, Os.Ime, Os.Prezime 
                         FROM Oglas O
                         INNER JOIN StudentskiSmjestaj S ON O.OglasID = S.OglasID
                         INNER JOIN Zaposleni Z ON O.OsobaID = Z.OsobaID
                         INNER JOIN Osoba Os ON Z.OsobaID = Os.OsobaID
                         ORDER BY O.DatumObjave DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            oglasi.Add(new StudentskiSmjestajModel
                            {
                                OglasID = reader.GetInt32("OglasID"),
                                Naslov = reader.GetString("Naslov"),
                                TekstOglasa = reader.GetString("TekstOglasa"),
                                DatumObjave = reader.GetDateTime("DatumObjave"),
                                ImePrezime = $"{reader.GetString("Ime")} {reader.GetString("Prezime")}"
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška pri učitavanju podataka: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return oglasi;
        }
        public static bool ObrisiStudentskiSmjestaj(int oglasID)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Početak transakcije da bi oba brisanja bila u istoj transakciji
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Prvo brišemo iz tabele StudentskiSmjestaj
                            string query1 = @"DELETE FROM StudentskiSmjestaj WHERE OglasID = @OglasID";
                            using (MySqlCommand cmd1 = new MySqlCommand(query1, connection, transaction))
                            {
                                cmd1.Parameters.AddWithValue("@OglasID", oglasID);
                                cmd1.ExecuteNonQuery();
                            }

                            // Zatim brišemo iz tabele Oglas
                            string query2 = @"DELETE FROM Oglas WHERE OglasID = @OglasID";
                            using (MySqlCommand cmd2 = new MySqlCommand(query2, connection, transaction))
                            {
                                cmd2.Parameters.AddWithValue("@OglasID", oglasID);
                                cmd2.ExecuteNonQuery();
                            }

                            // Potvrda transakcije
                            transaction.Commit();
                            return true;  // Vraćamo true ako je brisanje bilo uspešno
                        }
                        catch (Exception ex)
                        {
                            // U slučaju greške, poništavamo transakciju
                            transaction.Rollback();
                            MessageBox.Show($"Greška pri brisanju podataka: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                            return false;  // Vraćamo false ako dođe do greške
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška pri povezivanju sa bazom: {ex.Message}", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;  // Vraćamo false ako nije moguće povezivanje sa bazom
                }
            }
        }


        public class KontaktModel
        {
            public int KontaktID { get; set; }
            public string Adresa1 { get; set; }
            public string? Adresa2 { get; set; }
            public string BrTelefona1 { get; set; }
            public string? BrTelefona2 { get; set; }
            public string Email { get; set; }
            public string? InstagramLink { get; set; }
            public string? FacebookLink { get; set; }
            public string? Tekst { get; set; }
        }
        public static bool DodajKontaktUBazu(KontaktModel kontakt)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                    INSERT INTO Kontakt (Adresa1, Adresa2, BrTelefona1, BrTelefona2, Email, InstagramLink, FacebookLink, Tekst)
                    VALUES (@Adresa1, @Adresa2, @BrTelefona1, @BrTelefona2, @Email, @InstagramLink, @FacebookLink, @Tekst)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Adresa1", kontakt.Adresa1);
                        cmd.Parameters.AddWithValue("@Adresa2", kontakt.Adresa2 ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@BrTelefona1", kontakt.BrTelefona1);
                        cmd.Parameters.AddWithValue("@BrTelefona2", kontakt.BrTelefona2 ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Email", kontakt.Email);
                        cmd.Parameters.AddWithValue("@InstagramLink", kontakt.InstagramLink ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@FacebookLink", kontakt.FacebookLink ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Tekst", kontakt.Tekst ?? (object)DBNull.Value);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri unosu podataka: " + ex.Message);
                    return false;
                }
            }
        }
        public static List<KontaktModel> UcitajKontakt()
        {
            List<KontaktModel> kontakti = new List<KontaktModel>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM Kontakt"; // Učitava sve podatke iz tabele Kontakt

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            kontakti.Add(new KontaktModel
                            {
                                KontaktID = reader.GetInt32("KontaktID"),
                                Adresa1 = reader.GetString("Adresa1"),
                                Adresa2 = reader.IsDBNull(reader.GetOrdinal("Adresa2")) ? null : reader.GetString("Adresa2"),
                                BrTelefona1 = reader.GetString("BrTelefona1"),
                                BrTelefona2 = reader.IsDBNull(reader.GetOrdinal("BrTelefona2")) ? null : reader.GetString("BrTelefona2"),
                                Email = reader.GetString("Email"),
                                InstagramLink = reader.IsDBNull(reader.GetOrdinal("InstagramLink")) ? null : reader.GetString("InstagramLink"),
                                FacebookLink = reader.IsDBNull(reader.GetOrdinal("FacebookLink")) ? null : reader.GetString("FacebookLink"),
                                Tekst = reader.IsDBNull(reader.GetOrdinal("Tekst")) ? null : reader.GetString("Tekst")
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri učitavanju kontakata: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return kontakti;
        }
        public static bool ObrisiKontakt(int kontaktID)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM Kontakt WHERE KontaktID = @KontaktID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@KontaktID", kontaktID);

                        int affectedRows = cmd.ExecuteNonQuery();
                        return affectedRows > 0; // Ako je bar jedan red obrisan, vrati true
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greška pri brisanju kontakta: " + ex.Message, "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }


    }
}

