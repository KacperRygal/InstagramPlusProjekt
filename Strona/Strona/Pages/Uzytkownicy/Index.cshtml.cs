using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StackExchange.Redis;
using System.Data;
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
                
            
                String connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Instagram+;Integrated Security=False;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    /*SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "insert into Uzytkownicy values(12,'XD','123',NULL,NULL,1)";
                    cmd.ExecuteNonQuery();*/


                    String sql = "SELECT * FROM uzytkownicy";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {      
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine("Dzieki dziala");
                                Uzytkownik uzytkownik = new Uzytkownik();
                                uzytkownik.Id = reader.GetInt32(0);
                                uzytkownik.Nazwa = reader.GetString(1);
                                uzytkownik.Haslo = reader.GetString(2);

                                listaUzytkownikow.Add(uzytkownik);
                            }
                            reader.Close();
                        }
                    
                    }
                    connection.Close();
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
