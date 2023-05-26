using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InstPlusEntityFr.Pages.ProfilPrywatny
{
    public class IndexModel : PageModel
    {
        DbInstagramPlus db = new DbInstagramPlus();
        public String FilePath { get; set; }
        public String errorMessage = "";
        public void OnGet()
        {
            //////////wiktor
            if (HttpContext.Session.Keys.Contains("INFO"))
            {
                errorMessage = HttpContext.Session.GetString("INFO");
                HttpContext.Session.Remove("INFO");
            }

            //////////

			Console.WriteLine(FilePath);
			//pole na obraz, przycisk zmien profilowe, wyswietlanie loginu, przycisk zmien haslo, wyswietlanie postow uzytkownika
			var uz = db.Uzytkownicy.Where(s => s.UzytkownikId == HttpContext.Session.GetInt32("UserId"));
            foreach (Uzytkownik u in uz)
            {
                FilePath = u.Zdjecie;
            }
			//Console.WriteLine("CHUJ"); //co to za pozosta³oœci wtf
			//Console.WriteLine(FilePath);
            Page();
        }
    }
}
