using InstPlusEntityFr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;


namespace Strona.Pages.Login
{
    public class IndexModel : PageModel
    {
        Uzytkownik nowyUzytkownik = new Uzytkownik();
        DbInstagramPlus bazaInstagram = new DbInstagramPlus();
        public String errorMessage = "";
        public void OnGet()
        {
            //
        }

        public void OnPostRejestrowanie()
        {
            nowyUzytkownik.Nazwa = Request.Form["login"];
            nowyUzytkownik.Haslo = Request.Form["haslo"];

            if (nowyUzytkownik.Haslo.Length == 0 || nowyUzytkownik.Nazwa.Length == 0)
            {
                errorMessage = "Pola nie mog¹ byæ puste!";
                return;
            }

            if (bazaInstagram.Uzytkownicy.Where(u => u.Nazwa == nowyUzytkownik.Nazwa).IsNullOrEmpty())
            {
                bool czy_silne_haslo = nowyUzytkownik.testHasla();
                bool czy_dobry_login = nowyUzytkownik.testLoginu();

                if (czy_silne_haslo && czy_dobry_login)
                {
                    bazaInstagram.Uzytkownicy.Add(nowyUzytkownik);
                    bazaInstagram.SaveChanges();
                    errorMessage = "Udalo sie dodac uzytkownika! Mozesz sie zalogowaæ na swoje konto.";
                }
                else if (!czy_dobry_login) errorMessage = "Has³o musi zawieraæ 8 znaków, wielka i ma³¹ litere oraz liczbê";
                else errorMessage = "Has³o musi zawieraæ 8 znaków, wielka i ma³¹ litere oraz liczbê";
            }
            else
            {
                errorMessage = "Podany login zajety!";
            }

        }

        public void OnPostLogowanie()
        {
            if(HttpContext.Session.GetInt32("UzytkownikId") == null) { 
            nowyUzytkownik.Nazwa = Request.Form["login"];
            nowyUzytkownik.Haslo = Request.Form["haslo"];

            if (nowyUzytkownik.Haslo.Length == 0 || nowyUzytkownik.Nazwa.Length == 0)
            {
                errorMessage = "Pola nie mog¹ byæ puste!";
			}

            Uzytkownik uz = bazaInstagram.Uzytkownicy.Where(u => u.Nazwa == nowyUzytkownik.Nazwa && u.Haslo == nowyUzytkownik.Haslo).First();

            if (uz == null)
            {
                errorMessage = "Niepoprawny login lub has³o!";
            }
            else
            {
                errorMessage = "Zalogowano";
                HttpContext.Session.SetInt32("UzytkownikId", uz.UzytkownikId);
                Console.WriteLine(HttpContext.Session.GetInt32("UzytkownikId"));
                Response.Redirect("/MainPage/Index");
			}
            }
            else
            {
                errorMessage = "Jesteœ ju¿ zalogowany.";
            }
        }
        public void OnPostWyloguj()
        {
            Console.Write("Wylogowywuje");
            HttpContext.Session.Clear();
            Response.Redirect("/MainPage/Index");
        }
    }
}
