using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IronPdf;

namespace InstPlusEntityFr.Pages.StatystykiAdmin
{
    public class IndexModel : PageModel
    {
        //elementy: [0] -> 24h [1] -> tydzień [2] -> miesiąc [3] -> 6 miesięcy

        //lista do zrobienia:
        public List<int> IlosciDodPostow { get; set; } = new List<int>();
        public List<int> IlosciDodPolubien { get; set; } = new List<int>();
        public List<int> IlosciDodKomentarzy { get; set; } = new List<int>();
        public List<String> AutorzyPostowZNajwKoment { get; set; } = new List<String>();
        public List<String> AutorzyPostowZNajwPolubien { get; set; } = new List<String>();
        public List<String> AutorzyKomentZNajwPolubien { get; set; } = new List<String>();
        public List<String> UzytkownicyNajwKoment { get; set; } = new List<String>();
        public List<String> UzytkownicyNajwPostow { get; set; } = new List<String>();
        public List<String> NajczestszeTagi { get; set; } = new List<String>();
        public List<int> PostyUzytkownikowVip { get; set; } = new List<int>();

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
            PobierzUzytkNajwKom(); // -- nie dzia³a!!! (crashuje)

            //użytkownicy o największej ilości dodanych postów
            PobierzUzytkNajwPostow();

            //najczęściej występujące tagi
            PobierzNajczestszeTagi();

            //posty użytkowników VIP
            PobierzPostyUzytkownikowVip();
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
            //24h
            var uzytkownikZMaxLiczbaKomentarzy24h = kom24h
                .GroupBy(k => k.UzytkownikId)
                .Select(g => new
                {
                    UzytkownikId = g.Key,
                    LiczbaKomentarzy = g.Count()
                })
                .OrderByDescending(g => g.LiczbaKomentarzy)
            .FirstOrDefault();

            if (uzytkownikZMaxLiczbaKomentarzy24h != null)
            {
                var uzytkownik = db.Uzytkownicy
                        .FirstOrDefault(u => u.UzytkownikId == uzytkownikZMaxLiczbaKomentarzy24h.UzytkownikId);

                if (uzytkownik != null)
                {
                    var iloscKom = kom24h.Count(k => k.UzytkownikId == uzytkownik.UzytkownikId);
                    UzytkownicyNajwKoment.Add($"{uzytkownik.Nazwa}, ({iloscKom})");
                }
                else
                    UzytkownicyNajwKoment.Add("---");
            }
            else
                UzytkownicyNajwKoment.Add("---");

            //7dni
            var uzytkownikZMaxLiczbaKomentarzy7dni = kom7dni
                .GroupBy(k => k.UzytkownikId)
                .Select(g => new
                {
                    UzytkownikId = g.Key,
                    LiczbaKomentarzy = g.Count()
                })
                .OrderByDescending(g => g.LiczbaKomentarzy)
            .FirstOrDefault();

            if (uzytkownikZMaxLiczbaKomentarzy7dni != null)
            {
                var uzytkownik = db.Uzytkownicy
                        .FirstOrDefault(u => u.UzytkownikId == uzytkownikZMaxLiczbaKomentarzy7dni.UzytkownikId);

                if (uzytkownik != null)
                {
                    var iloscKom = kom7dni.Count(k => k.UzytkownikId == uzytkownik.UzytkownikId);
                    UzytkownicyNajwKoment.Add($"{uzytkownik.Nazwa}, ({iloscKom})");
                }
                else
                    UzytkownicyNajwKoment.Add("---");
            }
            else
                UzytkownicyNajwKoment.Add("---");

            //1miesiąc
            var uzytkownikZMaxLiczbaKomentarzy1mies = kom1mies
                .GroupBy(k => k.UzytkownikId)
                .Select(g => new
                {
                    UzytkownikId = g.Key,
                    LiczbaKomentarzy = g.Count()
                })
                .OrderByDescending(g => g.LiczbaKomentarzy)
            .FirstOrDefault();

            if (uzytkownikZMaxLiczbaKomentarzy1mies != null)
            {
                var uzytkownik = db.Uzytkownicy
                        .FirstOrDefault(u => u.UzytkownikId == uzytkownikZMaxLiczbaKomentarzy1mies.UzytkownikId);

                if (uzytkownik != null)
                {
                    var iloscKom = kom1mies.Count(k => k.UzytkownikId == uzytkownik.UzytkownikId);
                    UzytkownicyNajwKoment.Add($"{uzytkownik.Nazwa}, ({iloscKom})");
                }
                else
                    UzytkownicyNajwKoment.Add("---");
            }
            else
                UzytkownicyNajwKoment.Add("---");

            //6miesięcy
            var uzytkownikZMaxLiczbaKomentarzy6mies = kom6mies
                .GroupBy(k => k.UzytkownikId)
                .Select(g => new
                {
                    UzytkownikId = g.Key,
                    LiczbaKomentarzy = g.Count()
                })
                .OrderByDescending(g => g.LiczbaKomentarzy)
            .FirstOrDefault();

            if (uzytkownikZMaxLiczbaKomentarzy6mies != null)
            {
                var uzytkownik = db.Uzytkownicy
                        .FirstOrDefault(u => u.UzytkownikId == uzytkownikZMaxLiczbaKomentarzy6mies.UzytkownikId);

                if (uzytkownik != null)
                {
                    var iloscKom = kom6mies.Count(k => k.UzytkownikId == uzytkownik.UzytkownikId);
                    UzytkownicyNajwKoment.Add($"{uzytkownik.Nazwa}, ({iloscKom})");
                }
                else
                    UzytkownicyNajwKoment.Add("---");
            }
            else
                UzytkownicyNajwKoment.Add("---");
        }

        private void PobierzUzytkNajwPostow()
        {
            //24h
            var uzytkownikZMaxLiczbaPostow24h = posty24h
                .GroupBy(p => p.UzytkownikId)
                .Select(g => new
                {
                    UzytkownikId = g.Key,
                    LiczbaPostow = g.Count()
                })
                .OrderByDescending(g => g.LiczbaPostow)
            .FirstOrDefault();

            if (uzytkownikZMaxLiczbaPostow24h != null)
            {
                var uzytkownik = db.Uzytkownicy
                        .FirstOrDefault(u => u.UzytkownikId == uzytkownikZMaxLiczbaPostow24h.UzytkownikId);

                if (uzytkownik != null)
                {
                    var iloscPostow = posty24h.Count(p => p.UzytkownikId == uzytkownik.UzytkownikId);
                    UzytkownicyNajwPostow.Add($"{uzytkownik.Nazwa}, ({iloscPostow})");
                }
                else
                    UzytkownicyNajwPostow.Add("---");
            }
            else
                UzytkownicyNajwPostow.Add("---");

            //7dni
            var uzytkownikZMaxLiczbaPostow7dni = posty7dni
                .GroupBy(p => p.UzytkownikId)
                .Select(g => new
                {
                    UzytkownikId = g.Key,
                    LiczbaPostow = g.Count()
                })
                .OrderByDescending(g => g.LiczbaPostow)
            .FirstOrDefault();

            if (uzytkownikZMaxLiczbaPostow7dni != null)
            {
                var uzytkownik = db.Uzytkownicy
                        .FirstOrDefault(u => u.UzytkownikId == uzytkownikZMaxLiczbaPostow7dni.UzytkownikId);

                if (uzytkownik != null)
                {
                    var iloscPostow = posty7dni.Count(p => p.UzytkownikId == uzytkownik.UzytkownikId);
                    UzytkownicyNajwPostow.Add($"{uzytkownik.Nazwa}, ({iloscPostow})");
                }
                else
                    UzytkownicyNajwPostow.Add("---");
            }
            else
                UzytkownicyNajwPostow.Add("---");

            //1miesiąc
            var uzytkownikZMaxLiczbaPostow1mies = posty1mies
                .GroupBy(p => p.UzytkownikId)
                .Select(g => new
                {
                    UzytkownikId = g.Key,
                    LiczbaPostow = g.Count()
                })
                .OrderByDescending(g => g.LiczbaPostow)
            .FirstOrDefault();

            if (uzytkownikZMaxLiczbaPostow1mies != null)
            {
                var uzytkownik = db.Uzytkownicy
                        .FirstOrDefault(u => u.UzytkownikId == uzytkownikZMaxLiczbaPostow1mies.UzytkownikId);

                if (uzytkownik != null)
                {
                    var iloscPostow = posty1mies.Count(p => p.UzytkownikId == uzytkownik.UzytkownikId);
                    UzytkownicyNajwPostow.Add($"{uzytkownik.Nazwa}, ({iloscPostow})");
                }
                else
                    UzytkownicyNajwPostow.Add("---");
            }
            else
                UzytkownicyNajwPostow.Add("---");

            //6miesięcy
            var uzytkownikZMaxLiczbaPostow6mies = posty6mies
                .GroupBy(p => p.UzytkownikId)
                .Select(g => new
                {
                    UzytkownikId = g.Key,
                    LiczbaPostow = g.Count()
                })
                .OrderByDescending(g => g.LiczbaPostow)
            .FirstOrDefault();

            if (uzytkownikZMaxLiczbaPostow6mies != null)
            {
                var uzytkownik = db.Uzytkownicy
                        .FirstOrDefault(u => u.UzytkownikId == uzytkownikZMaxLiczbaPostow6mies.UzytkownikId);

                if (uzytkownik != null)
                {
                    var iloscPostow = posty6mies.Count(p => p.UzytkownikId == uzytkownik.UzytkownikId);
                    UzytkownicyNajwPostow.Add($"{uzytkownik.Nazwa}, ({iloscPostow})");
                }
                else
                    UzytkownicyNajwPostow.Add("---");
            }
            else
                UzytkownicyNajwPostow.Add("---");
        }

        private void PobierzNajczestszeTagi()
        {
            //24h
            var najczestszyTag24h = posty24h
                .SelectMany(p => p.Tagi)
                .GroupBy(t => t.Nazwa)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            if (najczestszyTag24h != null)
                NajczestszeTagi.Add(najczestszyTag24h);
            else
                NajczestszeTagi.Add("---");

            //7dni
            var najczestszyTag7dni = posty7dni
                .SelectMany(p => p.Tagi)
                .GroupBy(t => t.Nazwa)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            if (najczestszyTag7dni != null)
                NajczestszeTagi.Add(najczestszyTag7dni);
            else
                NajczestszeTagi.Add("---");

            //1miesiąc
            var najczestszyTag1mies = posty1mies
                .SelectMany(p => p.Tagi)
                .GroupBy(t => t.Nazwa)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            if (najczestszyTag1mies != null)
                NajczestszeTagi.Add(najczestszyTag1mies);
            else
                NajczestszeTagi.Add("---");

            //6miesięcy
            var najczestszyTag6mies = posty6mies
                .SelectMany(p => p.Tagi)
                .GroupBy(t => t.Nazwa)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            if (najczestszyTag6mies != null)
                NajczestszeTagi.Add(najczestszyTag6mies);
            else
                NajczestszeTagi.Add("---");
        }

        private void PobierzPostyUzytkownikowVip()
        {
            //24h
            DateTime czasTeraz = DateTime.Now;

            var iloscPostowVip24h = posty24h
                .Join(db.Uzytkownicy.Where(u => u.DataVipDo != null && u.DataVipDo > czasTeraz),
                    post => post.UzytkownikId,
                    uzytkownik => uzytkownik.UzytkownikId,
                    (post, uzytkownik) => post)
                .Count();
            PostyUzytkownikowVip.Add(iloscPostowVip24h);

            //7dni
            var iloscPostowVip7dni = posty7dni
                .Join(db.Uzytkownicy.Where(u => u.DataVipDo != null && u.DataVipDo > czasTeraz),
                    post => post.UzytkownikId,
                    uzytkownik => uzytkownik.UzytkownikId,
                    (post, uzytkownik) => post)
                .Count();
            PostyUzytkownikowVip.Add(iloscPostowVip7dni);

            //1miesiąc
            var iloscPostowVip1mies = posty1mies
                .Join(db.Uzytkownicy.Where(u => u.DataVipDo != null && u.DataVipDo > czasTeraz),
                    post => post.UzytkownikId,
                    uzytkownik => uzytkownik.UzytkownikId,
                    (post, uzytkownik) => post)
                .Count();
            PostyUzytkownikowVip.Add(iloscPostowVip1mies);

            //6miesięcy
            var iloscPostowVip6mies = posty6mies
                .Join(db.Uzytkownicy.Where(u => u.DataVipDo != null && u.DataVipDo > czasTeraz),
                    post => post.UzytkownikId,
                    uzytkownik => uzytkownik.UzytkownikId,
                    (post, uzytkownik) => post)
                .Count();
            PostyUzytkownikowVip.Add(iloscPostowVip6mies);
        }

        public IActionResult OnPostConvertCurrentPageToPDF()
        {
  
            var renderer = new ChromePdfRenderer();
            var host = Request.Host;
            var path = Request.Path;
            var scheme = Request.Scheme;
            var currentPageUrl = $"{scheme}://{host}{path}";
            var pdf = renderer.RenderUrlAsPdf(currentPageUrl);
            pdf.SaveAs("T.pdf");
            byte[] pdfBytes = pdf.BinaryData;
            return File(pdfBytes, "application/pdf", "T.pdf");
        }
    }
}
    

/* UWAGA
 * na tej stronie brakuje sporej iloœci statystyk
 * TASK - jak ktoœ ma czas to prosze niech siê tym zajmie
 *                                      ~Wiktor
 */
