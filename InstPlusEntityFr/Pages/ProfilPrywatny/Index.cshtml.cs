using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InstPlusEntityFr.Pages.ProfilPrywatny
{
    public class IndexModel : PageModel
    {
        DbInstagramPlus db = new DbInstagramPlus();
        public String FilePath { get; set; }
        public void OnGet()
        {
			Console.WriteLine(FilePath);
			//pole na obraz, przycisk zmien profilowe, wyswietlanie loginu, przycisk zmien haslo, wyswietlanie postow uzytkownika
			var uz = db.Uzytkownicy.Where(s => s.UzytkownikId == HttpContext.Session.GetInt32("UserId"));
            foreach (Uzytkownik u in uz)
            {
                FilePath = u.Zdjecie;
            }
			Console.WriteLine("CHUJ");
			Console.WriteLine(FilePath);
            Page();
        }
    }
}
