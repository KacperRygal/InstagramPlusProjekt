using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InstPlusEntityFr.Pages.StatystykiAdmin
{
    public class IndexModel : PageModel
    {
        public List<int> ListaIlosciDodPostow { get; set; } = new List<int>();

        public void OnGet()
        {
            DbInstagramPlus db = new DbInstagramPlus();

            //dodane posty w ci¹gu ostatniego miesi¹ca
            ListaIlosciDodPostow.Add(db.Posty.Where(p=>p.DataPublikacji > DateTime.Now.AddDays(-1)).Count());
            ListaIlosciDodPostow.Add(db.Posty.Where(p => p.DataPublikacji > DateTime.Now.AddDays(-7)).Count());
            ListaIlosciDodPostow.Add(db.Posty.Where(p => p.DataPublikacji > DateTime.Now.AddMonths(-1)).Count());
            ListaIlosciDodPostow.Add(db.Posty.Where(p => p.DataPublikacji > DateTime.Now.AddMonths(-6)).Count());
        }
        
        public IActionResult OnPostAnulujBtn()
        {
            return RedirectToPage("/ProfilPrywatny/Index");
        }
    }
}
