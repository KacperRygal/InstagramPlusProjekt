using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StackExchange.Redis;
using System;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace Strona.Pages.Login
{
    public class IndexModel : PageModel
    {
        public Uzytkownik uzytkownik = new Uzytkownik();
        public String errorMessage = "";
        public void OnGet()
        {

        }

        public void OnPostRejestrowanie()
        {
            uzytkownik.Nazwa = Request.Form["login"];
            uzytkownik.Haslo = Request.Form["haslo"];

            if (uzytkownik.Haslo.Length == 0 || uzytkownik.Nazwa.Length==0)
            {
                errorMessage = "Pola nie mog¹ byæ puste!";
                return;
            }

            try
            {
                
                String connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Instagram+;Integrated Security=False;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT nazwa FROM Uzytkownicy WHERE nazwa='" + uzytkownik.Nazwa + "'";
                    SqlCommand cmd = new SqlCommand(sql, connection);

                    bool czy_istnieje = false;
                    bool czy_silne_haslo = uzytkownik.testHasla();
                    bool czy_dobry_login = uzytkownik.testLoginu();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.HasRows)  czy_istnieje = true;
                    }
                    reader.Close();

                    if (!czy_istnieje && czy_silne_haslo && czy_dobry_login)
                    {
                        SqlCommand insert = new SqlCommand("INSERT INTO Uzytkownicy (nazwa, haslo) VALUES (@nazwa, @haslo)", connection);
                        insert.Parameters.AddWithValue("@nazwa", uzytkownik.Nazwa);
                        insert.Parameters.AddWithValue("@haslo", uzytkownik.Haslo);
                        insert.ExecuteNonQuery();
                    }

                    connection.Close();

                    if (czy_istnieje) errorMessage="Podany login zajety!";
                    else if (!czy_dobry_login) errorMessage = "Has³o musi zawieraæ 8 znaków, wielka i ma³¹ litere oraz liczbê";
                    else if(!czy_silne_haslo) errorMessage = "Has³o musi zawieraæ 8 znaków, wielka i ma³¹ litere oraz liczbê";
                    else errorMessage = "Udalo sie dodac uzytkownika!";
                }
            }
            catch (Exception ex)
            {
				errorMessage = ex.Message + "--- b³¹d po³¹czenia";
            }

        }

        public void OnPostLogowanie()
        {
            uzytkownik.Nazwa = Request.Form["login"];
            uzytkownik.Haslo = Request.Form["haslo"];

			if (uzytkownik.Haslo.Length == 0 || uzytkownik.Nazwa.Length == 0)
			{
				errorMessage = "Pola nie mog¹ byæ puste!";
				return;
			}

			try
            {
                String connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=InstagramPlus;Integrated Security=False;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT nazwa,haslo FROM Uzytkownicy WHERE haslo='" + uzytkownik.Haslo + "' AND nazwa='" + uzytkownik.Nazwa + "'";
                    SqlCommand cmd = new SqlCommand(sql, connection);

                    bool czy_istnieje = false;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int licznik_kont = 0;
                        if (reader.HasRows)
                        {
                            czy_istnieje = true;
                            licznik_kont++;
                        }
                        if (licznik_kont != 1) czy_istnieje = false;
                    }
                    reader.Close();
                    connection.Close();

                    if (!czy_istnieje) errorMessage = "Zle haslo lub login";
                    else errorMessage = "Zajebiscie";
                }
            }
            catch (Exception ex)
            {
				errorMessage = ex.Message + "--- b³¹d po³¹czenia";
            }
        }
        
    

        public class Uzytkownik
        {
            public String Nazwa { get; set; }
            public String Haslo { get; set; }

            public bool testLoginu()
            {
                if(Nazwa.Length<=30) return true; else return false;
            }
            public bool testHasla()
            {
                if (Haslo.Length<=30 && Haslo.Length >= 8 && MalaLitera(Haslo)&& DuzaLitera(Haslo) && Liczba(Haslo)) return true;
                else return false;
            }
            private bool MalaLitera(String t)
            {
                foreach (char c in t)
                if (char.IsUpper(c)) return true;
                return false;
            }
            private bool DuzaLitera(String t)
            {
                foreach (char c in t)
                    if (char.IsLower(c)) return true;
                return false;
            }
            private bool Liczba(String t)
            {
                foreach (char c in t)
                    if (char.IsDigit(c)) return true;
                return false;
            }

            Uzytkownik(int id, String nazwa, String haslo, Blob zdjecie, DateTime vipdo, bool moderator)
            {
                Nazwa = nazwa;
                Haslo = haslo;
            }
            public Uzytkownik() { }
        }
    }
}
