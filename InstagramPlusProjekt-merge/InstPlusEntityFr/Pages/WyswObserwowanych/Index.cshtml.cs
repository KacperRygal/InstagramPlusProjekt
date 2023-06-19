using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InstPlusEntityFr.Pages.WyswObserwowanych
{
    public class IndexModel : PageModel
    {
        public List<Uzytkownik> listaUzytkownikow { get; set; } = new List<Uzytkownik>();
		public int czyKogosProfil = 1;
		public int? id;
		public void OnGet()
        {
            DbInstagramPlus db = new DbInstagramPlus();
            int? zalogowanyId = (int?)HttpContext.Session.GetInt32("UzytkownikId");
			int? kogosId = (int?)HttpContext.Session.GetInt32("SzukaneID");
			
            if (kogosId != null)
			{
				if (zalogowanyId != null) czyKogosProfil = 2;
				else czyKogosProfil = 0;
				id = kogosId;
				HttpContext.Session.Remove("SzukaneID");
			}
			IQueryable<Obserwowany> obserwowani;

			if (czyKogosProfil == 0 || czyKogosProfil == 2)
			{
				obserwowani = db.Obserwowani.Where(o => o.ObserwatorId == kogosId);
			}
			else
			{
				obserwowani = db.Obserwowani.Where(o => o.ObserwatorId == zalogowanyId);
			}

			//var obserwowani = db.Obserwowani.Where(o => o.ObserwatorId == zalogowanyId);

            foreach (Obserwowany o in obserwowani)
            {
                listaUzytkownikow.Add(db.Uzytkownicy.Where(u=>u.UzytkownikId == o.ObserwowanyId).FirstOrDefault());
            }

            Page();
        }

        public IActionResult OnPostAnulujBtn(int czy, int id)
        {
			if (czy == 1)
			{
				return RedirectToPage("/ProfilPrywatny/Index");
			}
			else
			{
				HttpContext.Session.SetInt32("PowrotneID", id);
				return RedirectToPage("/ProfilPubliczny/Index");
			}
		}
    }
}
