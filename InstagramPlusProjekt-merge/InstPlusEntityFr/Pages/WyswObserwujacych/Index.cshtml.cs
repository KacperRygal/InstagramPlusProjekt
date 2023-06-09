using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InstPlusEntityFr.Pages.WyswObserwujacych
{
    public class IndexModel : PageModel
    {
        public List<Uzytkownik> listaUzytkownikow { get; set; } = new List<Uzytkownik>();

        public void OnGet()
        {
            DbInstagramPlus db = new DbInstagramPlus();
            int? zalogowanyId = (int?)HttpContext.Session.GetInt32("UzytkownikId");


            var obserwujacy = db.Obserwujacy.Where(o => o.ObserwowanyId == zalogowanyId);

            foreach (Obserwujacy o in obserwujacy)
            {
                listaUzytkownikow.Add(db.Uzytkownicy.Where(u => u.UzytkownikId == o.ObserwatorId).FirstOrDefault());
            }

            Page();
        }

        public IActionResult OnPostAnulujBtn()
        {
            return RedirectToPage("/ProfilPrywatny/Index");
        }
    }
}
