using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InstPlusEntityFr.Pages.StatystykiAdmin
{
    public class IndexModel : PageModel
    {
        //elementy: [0] -> 24h [1] -> tydzieñ [2] -> miesi¹c [3] -> 6 miesiêcy

        //lista do zrobienia:
        public List<int> IlosciDodPostow { get; set; } = new List<int>(); //jest - sprawdziæ poprawnoœæ
        public List<int> IlosciDodPolubien { get; set; } = new List<int>(); //jest - sprawdziæ poprawnoœæ
        public List<int> IlosciDodKomentarzy { get; set; } = new List<int>(); //jest - sprawdziæ poprawnoœæ
        public List<int> AutorzyPostowZNajwKoment { get; set; } = new List<int>(); //brak
        public List<int> AutorzyPostowZNajwPolubien { get; set; } = new List<int>(); //brak
        public List<int> UzytkownicyNajwKoment { get; set; } = new List<int>(); //brak
        public List<int> UzytkownicyNajwPostow { get; set; } = new List<int>(); //brak
        public List<int> NajczestszeTagi { get; set; } = new List<int>(); //brak
        public List<int> PostyUzytkownikowVip { get; set; } = new List<int>(); //brak
        public List<int> UzytkownicyNajwObserwowanych { get; set; } = new List<int>(); //brak

        public void OnGet()
        {
            DbInstagramPlus db = new DbInstagramPlus();

            //dodane posty w okresie czasu
            var posty24h = db.Posty.Where(p => p.DataPublikacji > DateTime.Now.AddDays(-1));
            var posty7dni = db.Posty.Where(p => p.DataPublikacji > DateTime.Now.AddDays(-7));
            var posty1mies = db.Posty.Where(p => p.DataPublikacji > DateTime.Now.AddMonths(-1));
            var posty6mies = db.Posty.Where(p => p.DataPublikacji > DateTime.Now.AddMonths(-6));

            IlosciDodPostow.Add(posty24h.Count());
            IlosciDodPostow.Add(posty7dni.Count());
            IlosciDodPostow.Add(posty1mies.Count());
            IlosciDodPostow.Add(posty6mies.Count());

            //dodane komentarze w okresie czasu

            //miej optymalnie i bardziej optymalnie:
            //IlosciDodKomentarzy.Add(db.Komentarze.Join(db.Posty, k => k.PostId, p => p.PostId,(k, p) => new { komentarz = k, post = p }).Where(kp => kp.post.DataPublikacji > DateTime.Now.AddDays(-1)).Count());
            IlosciDodKomentarzy.Add(db.Komentarze.Count(k => posty24h.Any(p => p.PostId == k.PostId)));
            IlosciDodKomentarzy.Add(db.Komentarze.Count(k => posty7dni.Any(p => p.PostId == k.PostId)));
            IlosciDodKomentarzy.Add(db.Komentarze.Count(k => posty1mies.Any(p => p.PostId == k.PostId)));
            IlosciDodKomentarzy.Add(db.Komentarze.Count(k => posty6mies.Any(p => p.PostId == k.PostId)));

            //dodane polubienia w okresie czasu
            IlosciDodPolubien.Add(db.PolubieniaPostow.Count(l => posty24h.Any(p => p.PostId == l.PostId)));
            IlosciDodPolubien.Add(db.PolubieniaPostow.Count(l => posty7dni.Any(p => p.PostId == l.PostId)));
            IlosciDodPolubien.Add(db.PolubieniaPostow.Count(l => posty1mies.Any(p => p.PostId == l.PostId)));
            IlosciDodPolubien.Add(db.PolubieniaPostow.Count(l => posty6mies.Any(p => p.PostId == l.PostId)));
        }

        public IActionResult OnPostAnulujBtn()
        {
            return RedirectToPage("/ProfilPrywatny/Index");
        }
    }
}

/* UWAGA
 * na tej stronie brakuje sporej iloœci statystyk
 * TASK - jak ktoœ ma czas to prosze niech siê tym zajmie
 *                                      ~Wiktor
 */
