using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StackExchange.Redis;
using System.Data.SqlClient;
using System.Reflection.Metadata;

namespace InstagramPlusProjekt.Pages.Logowanie
{
    public class IndexModel : PageModel
    {
        public List<Uzytkownik> listaUzytkownikow = new List<Uzytkownik>();

        public void OnGet()
        {
            try
            {
                //Server=
                //Server=localhost\InstagramPlusConnection;Database=instagramplusprojekt;User Id=root;Password=;port=3306;
                //String connectionString = @"Server = localhost\InstagramPlusConnection; Database = instagramplusprojekt; User Id = root; Password =; port = 3306";
                String connectionString = "Dsn = InstagramPlusConnection; description ={ instagram + connector odbc}; server = localhost; uid = root; database = instagramplusprojekt; port = 3306";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM uzytkownicy";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Uzytkownik uzytkownik = new Uzytkownik();
                                uzytkownik.Id = reader.GetInt32(0);
                                uzytkownik.Nazwa = reader.GetString(1);
                                uzytkownik.Haslo = reader.GetString(2);
                                //uzytkownik.Zdejcie = reader. //??? jak to zrobiæ???
                                //uzytkownik.Vip_do = reader.GetDateTime(4);
                                //uzytkownik.Moderator = reader.GetBoolean(5);

                                listaUzytkownikow.Add(uzytkownik);
                            }
                        }
                    
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "--- b³¹d po³¹czenia");
            }
        }
    }

    public class Uzytkownik
    {
        public int Id { get; set; }
        public String Nazwa { get; set; }
        public String Haslo { get; set; }
        public Blob Zdejcie { get; set; } //czy powinien to byæ string czy typ Blob???
        public DateTime Vip_do { get; set; }
        public bool Moderator { get; set; }

        Uzytkownik(int id, String nazwa, String haslo, Blob zdjecie, DateTime vipdo, bool moderator)
        {
            Id = id;
            Nazwa = nazwa;
            Haslo = haslo;
            Zdejcie = zdjecie;
            Vip_do = vipdo;
            Moderator = moderator;
        }

        public Uzytkownik() { }
    }
}
