using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.IO;
using System.Web;
using iText.Html2pdf;
using System.Web.Mvc;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.IO;
using System.Text;
using iText.Html2pdf;
using iText.IO.Source;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;

namespace InstPlusEntityFr.Pages.StatystykiAdmin
{
    public class IndexModel : PageModel
    {
        //elementy: [0] -> 24h [1] -> tydzieñ [2] -> miesi¹c [3] -> 6 miesiêcy

        //lista do zrobienia:
        public List<int> IlosciDodPostow { get; set; } = new List<int>();
        public List<int> IlosciDodPolubien { get; set; } = new List<int>();
        public List<int> IlosciDodKomentarzy { get; set; } = new List<int>();
        public List<String> AutorzyPostowZNajwKoment { get; set; } = new List<String>();
        public List<String> AutorzyPostowZNajwPolubien { get; set; } = new List<String>();
        public List<String> AutorzyKomentZNajwPolubien { get; set; } = new List<String>();
        public List<String> UzytkownicyNajwKoment { get; set; } = new List<String>(); //brak
        public List<String> UzytkownicyNajwPostow { get; set; } = new List<String>(); //brak
        public List<String> NajczestszeTagi { get; set; } = new List<String>(); //brak
        public List<int> PostyUzytkownikowVip { get; set; } = new List<int>(); //brak
        public List<String> UzytkownicyNajwObserwujacych { get; set; } = new List<String>(); //brak

        IQueryable<Post>? posty24h;
        IQueryable<Post>? posty7dni;
        IQueryable<Post>? posty1mies;
        IQueryable<Post>? posty6mies;
        DbInstagramPlus db = new DbInstagramPlus();

        IQueryable<Komentarz>? kom24h;
        IQueryable<Komentarz>? kom7dni;
        IQueryable<Komentarz>? kom1mies;
        IQueryable<Komentarz>? kom6mies;

        public void OnGet()
        {
            // - nie chcia³o mi siê robiæ listy list z t¹d du¿o kodu ale jest wiêksza czytelnoœæ
            // - UWAGA - raporty nie bêd¹ dzia³aæ w pe³ni bez uzupe³nienia list zakresowych komentarzy i postów!!!
            // - najlepiej wywo³ywaæ je w poni¿szej kolejnoœci

            //dodane posty w okresie czasu
            PobiezIloscDodPostow();

            //dodane komentarze w okresie czasu
            PobierzIloscKom();

            //dodane polubienia w okresie czasu
            PobierzIloscPolubien();

            //autorzy postów z najwiêksz¹ iloœci¹ komentarzy
            PobierzAutPostowNajwKom();

            //autorzy postów z najwiêksz¹ iloœci¹ polubieñ
            PobierzAutPostowNajwPolub();

            //autorzy komentarzy z najwiêksz¹ iloœci¹ polubieñ
            PobierzAutKomNajwPolub();

            //u¿ytkownicy o najwiêkszej liczbie dodanych komentarzy
            //PobierzUzytkNajwKom(); // -- nie dzia³a!!! (crashuje)


        }

        //powrót do profilu prywatnego
        public IActionResult OnPostAnulujBtn()
        {
            return RedirectToPage("/ProfilPrywatny/Index");
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        ///METODY ZBIERAJ¥CE DANE Z BAZY///
        /////////////////////////////////////////////////////////////////////////////////////////////////////

        private void PobiezIloscDodPostow()
        {
            posty24h = db.Posty.Where(p => p.DataPublikacji > DateTime.Now.AddDays(-1));
            posty7dni = db.Posty.Where(p => p.DataPublikacji > DateTime.Now.AddDays(-7));
            posty1mies = db.Posty.Where(p => p.DataPublikacji > DateTime.Now.AddMonths(-1));
            posty6mies = db.Posty.Where(p => p.DataPublikacji > DateTime.Now.AddMonths(-6));

            IlosciDodPostow.Add(posty24h.Count());
            IlosciDodPostow.Add(posty7dni.Count());
            IlosciDodPostow.Add(posty1mies.Count());
            IlosciDodPostow.Add(posty6mies.Count());
        }

        private void PobierzIloscKom()
        {
            IlosciDodKomentarzy.Add(db.Komentarze.Count(k => posty24h.Any(p => p.PostId == k.PostId)));
            IlosciDodKomentarzy.Add(db.Komentarze.Count(k => posty7dni.Any(p => p.PostId == k.PostId)));
            IlosciDodKomentarzy.Add(db.Komentarze.Count(k => posty1mies.Any(p => p.PostId == k.PostId)));
            IlosciDodKomentarzy.Add(db.Komentarze.Count(k => posty6mies.Any(p => p.PostId == k.PostId)));
        }

        private void PobierzIloscPolubien()
        {
            IlosciDodPolubien.Add(db.PolubieniaPostow.Count(l => posty24h.Any(p => p.PostId == l.PostId)));
            IlosciDodPolubien.Add(db.PolubieniaPostow.Count(l => posty7dni.Any(p => p.PostId == l.PostId)));
            IlosciDodPolubien.Add(db.PolubieniaPostow.Count(l => posty1mies.Any(p => p.PostId == l.PostId)));
            IlosciDodPolubien.Add(db.PolubieniaPostow.Count(l => posty6mies.Any(p => p.PostId == l.PostId)));
        }

        private void PobierzAutPostowNajwKom()
        {
            Post postMaxKom24h = posty24h.OrderByDescending(p => db.Komentarze.Count(k => k.PostId == p.PostId)).FirstOrDefault();
            if (postMaxKom24h != null)
            {
                Uzytkownik uzytkownik = db.Uzytkownicy.Where(u => u.UzytkownikId == postMaxKom24h.UzytkownikId).FirstOrDefault();
                int iloscKom = db.Komentarze.Count(k => posty24h.Any(p => p.PostId == k.PostId));
                AutorzyPostowZNajwKoment.Add($"{uzytkownik.Nazwa}, ({iloscKom})");
            }
            else
                AutorzyPostowZNajwKoment.Add("---");

            Post postMaxKom7dni = posty7dni.OrderByDescending(p => db.Komentarze.Count(k => k.PostId == p.PostId)).FirstOrDefault();
            if (postMaxKom7dni != null)
            {
                Uzytkownik uzytkownik = db.Uzytkownicy.Where(u => u.UzytkownikId == postMaxKom7dni.UzytkownikId).FirstOrDefault();
                int iloscKom = db.Komentarze.Count(k => posty7dni.Any(p => p.PostId == k.PostId));
                AutorzyPostowZNajwKoment.Add($"{uzytkownik.Nazwa}, ({iloscKom})");
            }
            else
                AutorzyPostowZNajwKoment.Add("---");

            Post postMaxKom1mies = posty1mies.OrderByDescending(p => db.Komentarze.Count(k => k.PostId == p.PostId)).FirstOrDefault();
            if (postMaxKom1mies != null)
            {
                Uzytkownik uzytkownik = db.Uzytkownicy.Where(u => u.UzytkownikId == postMaxKom1mies.UzytkownikId).FirstOrDefault();
                int iloscKom = db.Komentarze.Count(k => posty1mies.Any(p => p.PostId == k.PostId));
                AutorzyPostowZNajwKoment.Add($"{uzytkownik.Nazwa}, ({iloscKom})");
            }
            else
                AutorzyPostowZNajwKoment.Add("---");

            Post postMaxKom6mies = posty6mies.OrderByDescending(p => db.Komentarze.Count(k => k.PostId == p.PostId)).FirstOrDefault();
            if (postMaxKom6mies != null)
            {
                Uzytkownik uzytkownik = db.Uzytkownicy.Where(u => u.UzytkownikId == postMaxKom6mies.UzytkownikId).FirstOrDefault();
                int iloscKom = db.Komentarze.Count(k => posty6mies.Any(p => p.PostId == k.PostId));
                AutorzyPostowZNajwKoment.Add($"{uzytkownik.Nazwa}, ({iloscKom})");
            }
            else
                AutorzyPostowZNajwKoment.Add("---");
        }

        private void PobierzAutPostowNajwPolub()
        {
            Post postMaxPolub24h = posty24h.OrderByDescending(p => db.PolubieniaPostow.Count(pp => pp.PostId == p.PostId)).FirstOrDefault();
            if (postMaxPolub24h != null)
            {
                Uzytkownik autor = db.Uzytkownicy.Where(u => u.UzytkownikId == postMaxPolub24h.UzytkownikId).FirstOrDefault();
                int iloscPolub = db.PolubieniaPostow.Count(pp => posty24h.Any(p => p.PostId == pp.PostId));
                AutorzyPostowZNajwPolubien.Add($"{autor.Nazwa}, ({iloscPolub})");
            }
            else
                AutorzyPostowZNajwPolubien.Add("---");

            Post postMaxPolub7dni = posty7dni.OrderByDescending(p => db.PolubieniaPostow.Count(pp => pp.PostId == p.PostId)).FirstOrDefault();
            if (postMaxPolub7dni != null)
            {
                Uzytkownik autor = db.Uzytkownicy.Where(u => u.UzytkownikId == postMaxPolub7dni.UzytkownikId).FirstOrDefault();
                int iloscPolub = db.PolubieniaPostow.Count(pp => posty7dni.Any(p => p.PostId == pp.PostId));
                AutorzyPostowZNajwPolubien.Add($"{autor.Nazwa}, ({iloscPolub})");
            }
            else
                AutorzyPostowZNajwPolubien.Add("---");

            Post postMaxPolub1mies = posty1mies.OrderByDescending(p => db.PolubieniaPostow.Count(pp => pp.PostId == p.PostId)).FirstOrDefault();
            if (postMaxPolub1mies != null)
            {
                Uzytkownik autor = db.Uzytkownicy.Where(u => u.UzytkownikId == postMaxPolub1mies.UzytkownikId).FirstOrDefault();
                int iloscPolub = db.PolubieniaPostow.Count(pp => posty1mies.Any(p => p.PostId == pp.PostId));
                AutorzyPostowZNajwPolubien.Add($"{autor.Nazwa}, ({iloscPolub})");
            }
            else
                AutorzyPostowZNajwPolubien.Add("---");

            Post postMaxPolub6mies = posty6mies.OrderByDescending(p => db.PolubieniaPostow.Count(pp => pp.PostId == p.PostId)).FirstOrDefault();
            if (postMaxPolub6mies != null)
            {
                Uzytkownik autor = db.Uzytkownicy.Where(u => u.UzytkownikId == postMaxPolub6mies.UzytkownikId).FirstOrDefault();
                int iloscPolub = db.PolubieniaPostow.Count(pp => posty6mies.Any(p => p.PostId == pp.PostId));
                AutorzyPostowZNajwPolubien.Add($"{autor.Nazwa}, ({iloscPolub})");
            }
            else
                AutorzyPostowZNajwPolubien.Add("---");
        }

        private void PobierzAutKomNajwPolub()
        {
            kom24h = db.Komentarze.Where(k => k.DataPublikacji > DateTime.Now.AddDays(-1));
            kom7dni = db.Komentarze.Where(k => k.DataPublikacji > DateTime.Now.AddDays(-7));
            kom1mies = db.Komentarze.Where(k => k.DataPublikacji > DateTime.Now.AddMonths(-1));
            kom6mies = db.Komentarze.Where(k => k.DataPublikacji > DateTime.Now.AddMonths(-6));

            Komentarz komMaxPolub24h = kom24h.OrderByDescending(k => db.PolubieniaKomentarzy.Count(pk => pk.KomentarzId == k.KomentarzId)).FirstOrDefault();
            if (komMaxPolub24h != null)
            {
                Uzytkownik autor = db.Uzytkownicy.Where(u => u.UzytkownikId == komMaxPolub24h.UzytkownikId).FirstOrDefault();
                int iloscPolub = db.PolubieniaKomentarzy.Count(pk => kom24h.Any(k => k.KomentarzId == pk.KomentarzId));
                AutorzyKomentZNajwPolubien.Add($"{autor.Nazwa}, ({iloscPolub})");
            }
            else
                AutorzyKomentZNajwPolubien.Add("---");

            Komentarz komMaxPolub7dni = kom7dni.OrderByDescending(k => db.PolubieniaKomentarzy.Count(pk => pk.KomentarzId == k.KomentarzId)).FirstOrDefault();
            if (komMaxPolub7dni != null)
            {
                Uzytkownik autor = db.Uzytkownicy.Where(u => u.UzytkownikId == komMaxPolub7dni.UzytkownikId).FirstOrDefault();
                int iloscPolub = db.PolubieniaKomentarzy.Count(pk => kom7dni.Any(k => k.KomentarzId == pk.KomentarzId));
                AutorzyKomentZNajwPolubien.Add($"{autor.Nazwa}, ({iloscPolub})");
            }
            else
                AutorzyKomentZNajwPolubien.Add("---");

            Komentarz komMaxPolub1mies = kom1mies.OrderByDescending(k => db.PolubieniaKomentarzy.Count(pk => pk.KomentarzId == k.KomentarzId)).FirstOrDefault();
            if (komMaxPolub1mies != null)
            {
                Uzytkownik autor = db.Uzytkownicy.Where(u => u.UzytkownikId == komMaxPolub1mies.UzytkownikId).FirstOrDefault();
                int iloscPolub = db.PolubieniaKomentarzy.Count(pk => kom1mies.Any(k => k.KomentarzId == pk.KomentarzId));
                AutorzyKomentZNajwPolubien.Add($"{autor.Nazwa}, ({iloscPolub})");
            }
            else
                AutorzyKomentZNajwPolubien.Add("---");

            Komentarz komMaxPolub6mies = kom6mies.OrderByDescending(k => db.PolubieniaKomentarzy.Count(pk => pk.KomentarzId == k.KomentarzId)).FirstOrDefault();
            if (komMaxPolub6mies != null)
            {
                Uzytkownik autor = db.Uzytkownicy.Where(u => u.UzytkownikId == komMaxPolub6mies.UzytkownikId).FirstOrDefault();
                int iloscPolub = db.PolubieniaKomentarzy.Count(pk => kom6mies.Any(k => k.KomentarzId == pk.KomentarzId));
                AutorzyKomentZNajwPolubien.Add($"{autor.Nazwa}, ({iloscPolub})");
            }
            else
                AutorzyKomentZNajwPolubien.Add("---");
        }

        private void PobierzUzytkNajwKom()
        {
            var autorMax24h = kom24h.GroupBy(k => k.UzytkownikId).OrderByDescending(g => g.Count()).FirstOrDefault();
            if (autorMax24h != null)
            {
                Uzytkownik autor = db.Uzytkownicy.FirstOrDefault(u => u.UzytkownikId == autorMax24h.Key);
                int iloscKom = kom24h.Count(k => k.UzytkownikId == autor.UzytkownikId);
                UzytkownicyNajwKoment.Add($"{autor.Nazwa}, ({iloscKom})");
            }
            else
                UzytkownicyNajwKoment.Add("---");

        }


        public void OnPostConvertCurrentPageToPDF()
        {
            u
        }
    }
}
    

/* UWAGA
 * na tej stronie brakuje sporej iloœci statystyk
 * TASK - jak ktoœ ma czas to prosze niech siê tym zajmie
 *                                      ~Wiktor
 */
