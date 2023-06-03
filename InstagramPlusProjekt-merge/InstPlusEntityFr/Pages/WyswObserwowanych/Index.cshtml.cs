using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InstPlusEntityFr.Pages.WyswObserwowanych
{
    public class IndexModel : PageModel
    {
        public List<Uzytkownik> listaUzytkownikow { get; set; } = new List<Uzytkownik>();

        public void OnGet()
        {
            DbInstagramPlus db = new DbInstagramPlus();
            int zalogowanyId = (int)HttpContext.Session.GetInt32("UzytkownikId");


            var obserwowani = db.Obserwowani.Where(o => o.ObserwatorId == zalogowanyId);

            foreach (Obserwowany o in obserwowani)
            {
                listaUzytkownikow.Add(db.Uzytkownicy.Where(u=>u.UzytkownikId == o.ObserwowanyId).FirstOrDefault());
            }

            Page();
        }

        public IActionResult OnPostAnulujBtn()
        {
            return RedirectToPage("/ProfilPrywatny/Index");
        }
    }
}
