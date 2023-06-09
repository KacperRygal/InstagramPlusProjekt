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
                errorMessage = "Pola nie mogą być puste!";
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
                    errorMessage = "Udalo sie dodac uzytkownika! Mozesz sie zalogować na swoje konto.";
                }
                else if (!czy_dobry_login) errorMessage = "Login musi być krótszy niż 40 znaków!";
                else errorMessage = "Hasło musi zawierać 8 znaków, wielka i małą litere oraz liczbę";
            }
            else
            {
                errorMessage = "Podany login zajety!";
            }

        }
        public void OnPostWyloguj()
        {
            Console.Write("Wylogowywuje");
            HttpContext.Session.Clear();
            Response.Redirect("/MainPage/Index");
        }
        public void OnPostLogowanie()
        {
            if(HttpContext.Session.GetInt32("UzytkownikId") == null) { 
            nowyUzytkownik.Nazwa = Request.Form["login"];
            nowyUzytkownik.Haslo = Request.Form["haslo"];

            if (nowyUzytkownik.Haslo.Length == 0 || nowyUzytkownik.Nazwa.Length == 0)
            {
                errorMessage = "Pola nie mogą być puste!";
			}

            Uzytkownik uz = bazaInstagram.Uzytkownicy.Where(u => u.Nazwa == nowyUzytkownik.Nazwa && u.Haslo == nowyUzytkownik.Haslo).FirstOrDefault();

            if (uz == null)
            {
                errorMessage = "Niepoprawny login lub hasło!";
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
                errorMessage = "Jesteś już zalogowany.";
            }
        }
   
    }
}
