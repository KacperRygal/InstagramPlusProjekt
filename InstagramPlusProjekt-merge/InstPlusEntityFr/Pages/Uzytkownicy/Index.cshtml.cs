using InstPlusEntityFr;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;

namespace InstagramPlusProjekt.Pages.Logowanie
{
    public class IndexModel : PageModel
    {
        DbInstagramPlus bazaInstagram = new DbInstagramPlus();
        public List<Uzytkownik> listaUzytkownikow = new List<Uzytkownik>();
        public void OnGet()
        {
            foreach (Uzytkownik u in bazaInstagram.Uzytkownicy)
            {
                listaUzytkownikow.Add(u);
            }
        }
    }
}
