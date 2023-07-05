using InstPlusEntityFr.Migrations;
using iText.Forms.Form.Element;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Linq;
using System.Text;

namespace InstPlusEntityFr.Pages.MainPage
{
    public class PostWithComments
    {
        public int Id { get; set; }
        public bool CzyPolubionyPost { get; set; }
        public string Image { get; set; }
        public string Opis { get; set; }
        public string ImageAvatar { get; set; }
        public string Nazwa { get; set; }
        public List<bool> CzyPolubionyKomentarz { get; set; }
        public List<int> KomentarzeId { get; set; }
        public List<Komentarz> Komentarze { get; set; }
        public List<String> KomentarzeTresc { get; set; }
        public List<String> KomentarzeZDJ { get; set; }
        public List<String> KomentarzeAutor { get; set; }
        public List<String> KomentarzeData { get; set; }
        public List<String> Tagi { get; set; }
        public DateTime Data { get; set; }
        public int IloscPolubien { get; set; }
        public List<int> KomentarzePolubienia { get; set; }
    }
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        DbInstagramPlus db = new DbInstagramPlus();

        //do stopki reklamowej
        public bool CzyVip { get; set; }
        public bool CzyZalogowany { get; set; }
        public string ZdjecieZalogowanego { get; set; }
        public string LoginZalogowanego { get; set; }
        public String WyswReklama { get; set; }


        public string przechTresc { get; set; }
        public int przechPId { get; set; }

        public void OnGetGuzik(int fun, int id, string tresc)
        {
            var zalo = db.Uzytkownicy.Where(u => u.UzytkownikId == (int?)HttpContext.Session.GetInt32("UzytkownikId")).FirstOrDefault();
            if (zalo != null)
            {
                var postsListBytes = HttpContext.Session.Get("PostsList");
                if (postsListBytes != null)
                {
                    postsWithComments = JsonConvert.DeserializeObject<List<PostWithComments>>(Encoding.UTF8.GetString(postsListBytes));
                    // Wykonaj operacje na liœcie postów
                }

                switch (fun)
                {
                    case 1:
                        Komentarz kom = new Komentarz();

                        kom.UzytkownikId = zalo.UzytkownikId;
                        kom.PostId = id;
                        kom.Tresc = tresc;
                        db.Komentarze.Add(kom);
                        db.SaveChanges();
                        Reload();
                        break;

                    case 2:
                        bool czyBylPolubiony = false;
                        PolubieniePosta p = new PolubieniePosta(id, zalo.UzytkownikId);
                        foreach (PolubieniePosta pol in db.PolubieniaPostow)
                        {
                            if (pol.UzytkownikId == p.UzytkownikId && pol.PostId == p.PostId)
                            {
                                db.PolubieniaPostow.Remove(pol);
                                czyBylPolubiony = true;
                                break;
                            }
                        }
                        if (!czyBylPolubiony) db.PolubieniaPostow.Add(p);
                        db.SaveChanges();
                        Reload();
                        break;

                    case 3:
                        bool czyBylPolubionyK = false;
                        PolubienieKomentarza k = new PolubienieKomentarza(id, zalo.UzytkownikId);
                        foreach (PolubienieKomentarza pol in db.PolubieniaKomentarzy)
                        {
                            if (pol.KomentarzId==k.KomentarzId && pol.UzytkownikId==k.UzytkownikId)
                            {
                                db.PolubieniaKomentarzy.Remove(pol);
                                czyBylPolubionyK = true;
                                break;
                            }
                        }
                        if (!czyBylPolubionyK) db.PolubieniaKomentarzy.Add(k);
                        db.SaveChanges();
                        Reload();
                        break;
                }
                
            }
            HttpContext.Session.SetInt32("CZYODCZYT",1);
            // Console.WriteLine(fun+" "+id+" "+tresc);
        }

        private void Reload()
        {
            

            foreach (PostWithComments pk in postsWithComments)
            {
                foreach (Post post in db.Posty)
                {
                    if (post.PostId==pk.Id)
                    {
                        pk.IloscPolubien = db.PolubieniaPostow.Count(u => u.PostId == post.PostId);
                        pk.Komentarze.Clear();
                        pk.KomentarzeTresc.Clear();
                        pk.KomentarzePolubienia.Clear();
                        pk.KomentarzeZDJ.Clear();
                        pk.KomentarzeAutor.Clear();
                        pk.KomentarzeId.Clear();
                        pk.CzyPolubionyKomentarz.Clear();

                        var zalog = db.Uzytkownicy.Where(u => u.UzytkownikId == (int?)HttpContext.Session.GetInt32("UzytkownikId")).FirstOrDefault();
                        bool PolubionyPost = db.PolubieniaPostow.Any(l => l.UzytkownikId == zalog.UzytkownikId && l.PostId == post.PostId);
                        pk.CzyPolubionyPost = PolubionyPost;

                        var tempKom = db.Komentarze.Where(u => u.PostId == post.PostId);
                        foreach (var kom in tempKom)
                        {
                            pk.Komentarze.Add(kom);
                            pk.KomentarzeId.Add(kom.KomentarzId);
                            pk.KomentarzeTresc.Add(kom.Tresc);
                            pk.KomentarzePolubienia.Add(db.PolubieniaKomentarzy.Count(u => u.KomentarzId == kom.KomentarzId));

                            pk.CzyPolubionyKomentarz.Add(db.PolubieniaKomentarzy.Any(l => l.UzytkownikId == zalog.UzytkownikId && l.KomentarzId == kom.KomentarzId));
                           // 
                            var zdj = db.Uzytkownicy.Where(u => u.UzytkownikId == kom.UzytkownikId).Select(u => u.Zdjecie).FirstOrDefault();
                            if (zdj == null) zdj = "/ImgUploads/userTmpImg.jpg";
                            pk.KomentarzeZDJ.Add(@Url.Content(zdj));
                            //if (zdj) post.KomentarzeZDJ.Add(@Url.Content("/ImgUploads/userTmpImg.jpg"));


                            var autor = db.Uzytkownicy.Where(u => u.UzytkownikId == kom.UzytkownikId).Select(u => u.Nazwa).FirstOrDefault();
                            pk.KomentarzeAutor.Add(autor);
                            pk.KomentarzeData.Add(kom.DataPublikacji.ToString());
                        }
                        pk.Tagi.Clear();
                        
                        var tempTagi = db.Posty.Where(u => u.PostId == post.PostId).Select(s => s.Tagi);
                        foreach (var tag in tempTagi)
                        {
                            foreach (var x in tag)
                            {
                                pk.Tagi.Add(x.Nazwa);
                            }
                        }
                    }
                    
                }
            }
            HttpContext.Session.Set("PostsList", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postsWithComments)));
        }

        public String getPosty()
        {
            var postsListBytes = HttpContext.Session.Get("PostsList");
            if (postsListBytes != null)
            {
                postsWithComments = JsonConvert.DeserializeObject<List<PostWithComments>>(Encoding.UTF8.GetString(postsListBytes));
                // Wykonaj operacje na liœcie postów
            }
            return JsonConvert.SerializeObject(postsWithComments);
        }

        private List<PostWithComments> postsWithComments = new List<PostWithComments>();
        private Dictionary<String, int> mapaTagowSesji = new Dictionary<String, int>();

        private String zal = null;
        public String getZalogowany()
        {
            return JsonConvert.SerializeObject(zal);
        }
        public void OnGet(string inputValue = null)
        {

            //sprawdzenie czy jest ktoœ zalogowany
            var zalogowany = db.Uzytkownicy.Where(u => u.UzytkownikId == (int?)HttpContext.Session.GetInt32("UzytkownikId")).FirstOrDefault();
            if (zalogowany != null) zal = zalogowany.Nazwa;
            var obecny = db.Uzytkownicy.Where(u => u.UzytkownikId == (int?)HttpContext.Session.GetInt32("SzukaneID")).FirstOrDefault();

            

            if (zalogowany != null)
            {
                CzyZalogowany = true;

                //sprawdzenie czy ma siê wyœwietliæ reklama
                if (zalogowany.DataVipDo == null || zalogowany.DataVipDo < DateAndTime.Now)
                    CzyVip = false;
                else
                    CzyVip = true;

              

                if (HttpContext.Session.Keys.Contains("CZYODCZYT"))
                {
                    var postsListBytes = HttpContext.Session.Get("PostsList");
                    if (postsListBytes != null)
                    {
                        postsWithComments = JsonConvert.DeserializeObject<List<PostWithComments>>(Encoding.UTF8.GetString(postsListBytes));
                        // Wykonaj operacje na liœcie postów
                    }
                    HttpContext.Session.Remove("CZYODCZYT");
                }

                else
                { 
                //zliczenie tagów
                foreach (var pol in db.PolubieniaPostow.Where(u => u.UzytkownikId == zalogowany.UzytkownikId))
                {
                    var post = db.Posty.Where(u => u.PostId == pol.PostId).Select(u => u.Tagi);
                    foreach (var p in post)
                    {
                        foreach (var v in p)
                        {
                            if (mapaTagowSesji.ContainsKey(v.Nazwa))
                            {
                                mapaTagowSesji[v.Nazwa]++;
                            }
                            else
                            {
                                mapaTagowSesji.Add(v.Nazwa, 1);
                            }
                        }
                    }
                }
                //pomocnicze do testów zliczania tagow
                /*foreach (var m in mapaTagowSesji)
				{
					Console.WriteLine($"{m.Key}: {m.Value}");
				}*/

                //dodawanie postu do listy (startowe)

                //Tutaj Modu³ wyszukiwania tagu
                var testowa = db.Posty.ToList();
                if (inputValue != null) testowa = db.TagiPostow.Include(tp => tp.Posty).Where(tp => tp.Nazwa == inputValue).SelectMany(tp => tp.Posty).ToList();
                if (HttpContext.Session.GetInt32("SzukaneID") != null)
                {
                    testowa = db.Posty.Where(p => p.UzytkownikId == HttpContext.Session.GetInt32("SzukaneID")).ToList();
                    HttpContext.Session.Remove("SzukaneID");
                }
                //Tutaj koniec modu³u wyszukiwania tagu

                foreach (var idpost in testowa)
                {
                    var post = new PostWithComments();
                    post.Tagi = new List<String>();
                    post.KomentarzeId = new List<int>();
                    post.KomentarzePolubienia = new List<int>();
                    post.Komentarze = new List<Komentarz>();
                    post.KomentarzeTresc = new List<String>();
                    post.KomentarzeZDJ = new List<String>();
                    post.KomentarzeAutor = new List<String>();
                    post.KomentarzeData = new List<String>();
                    post.CzyPolubionyKomentarz = new List<bool>();
                    post.Id = idpost.PostId;
                    post.Image = @Url.Content(idpost.Zdjecie);
                    post.Opis = idpost.Opis;
                    post.ImageAvatar = @Url.Content(db.Uzytkownicy.Where(u => u.UzytkownikId == idpost.UzytkownikId).FirstOrDefault().Zdjecie);
                    if (post.ImageAvatar == null) post.ImageAvatar = "/ImgUploads/userTmpImg.jpg";
                    post.Nazwa = db.Uzytkownicy.Where(u => u.UzytkownikId == idpost.UzytkownikId).FirstOrDefault().Nazwa;
                    post.IloscPolubien = db.PolubieniaPostow.Count(u => u.PostId == idpost.PostId);
                    var tempKom = db.Komentarze.Where(u => u.PostId == idpost.PostId);


                        var zalog = db.Uzytkownicy.Where(u => u.UzytkownikId == (int?)HttpContext.Session.GetInt32("UzytkownikId")).FirstOrDefault();
                        bool PolubionyPost = db.PolubieniaPostow.Any(l => l.UzytkownikId == zalog.UzytkownikId && l.PostId == idpost.PostId);
                        post.CzyPolubionyPost = PolubionyPost;

                       // Console.WriteLine(zalog.UzytkownikId + " " + post.Id + " " + PolubionyPost);

                        foreach (var kom in tempKom)
                    {
                        post.Komentarze.Add(kom);
                        post.KomentarzeId.Add(kom.KomentarzId);
                        post.KomentarzeTresc.Add(kom.Tresc);
                        post.KomentarzePolubienia.Add(db.PolubieniaKomentarzy.Count(u => u.KomentarzId == kom.KomentarzId));
                        post.CzyPolubionyKomentarz.Add(db.PolubieniaKomentarzy.Any(l => l.UzytkownikId == zalog.UzytkownikId && l.KomentarzId == kom.KomentarzId));

                            var zdj = db.Uzytkownicy.Where(u => u.UzytkownikId == kom.UzytkownikId).Select(u => u.Zdjecie).FirstOrDefault();
                        if (zdj == null) zdj = "/ImgUploads/userTmpImg.jpg";
                        post.KomentarzeZDJ.Add(@Url.Content(zdj));
                        //if (zdj) post.KomentarzeZDJ.Add(@Url.Content("/ImgUploads/userTmpImg.jpg"));


                        var autor = db.Uzytkownicy.Where(u => u.UzytkownikId == kom.UzytkownikId).Select(u => u.Nazwa).FirstOrDefault();
                        post.KomentarzeAutor.Add(autor);
                        post.KomentarzeData.Add(kom.DataPublikacji.ToString());
                    }
                    var tempTagi = db.Posty.Where(u => u.PostId == idpost.PostId).Select(s => s.Tagi);
                    foreach (var tag in tempTagi)
                    {
                        foreach (var x in tag)
                        {
                            post.Tagi.Add(x.Nazwa);
                        }
                    }
                    postsWithComments.Add(post);
                }
                //stworzenie mieszanej listy
                var mieszanaLista = new List<PostWithComments>();
                var random = new Random();
                //przepisanie losow do mieszanej listy
                while (postsWithComments.Count > 0)
                {
                    var losowyIndex = random.Next(postsWithComments.Count);
                    var post = postsWithComments[losowyIndex];
                    mieszanaLista.Add(post);
                    postsWithComments.RemoveAt(losowyIndex);
                    //Console.WriteLine(post.Nazwa);
                    //Console.WriteLine(mieszanaLista[mieszanaLista.Count-1].Nazwa);
                }
                //przepisanie do koncowej listy (algorytm)
                foreach (var post in mieszanaLista)
                {
                    foreach (var s in post.Tagi)
                    {
                        if (mapaTagowSesji.ContainsKey(s))
                        {
                            postsWithComments.Insert(0, post);
                            break;
                        }
                        else
                        {
                            postsWithComments.Insert(postsWithComments.Count / 2, post);
                            break;
                        }
                    }
                    // Console.WriteLine(post.Nazwa);
                }
                }
            }
            //to kiedy niezalogowany uzytkownik
            else
            {
                //nie ma VIP z automatu - reklamy
                CzyZalogowany = false;
                CzyVip = false;

                //Tutaj Modu³ wyszukiwania tagu
                var testowa = db.Posty.ToList();
                if (inputValue != null) testowa = db.TagiPostow.Include(tp => tp.Posty).Where(tp => tp.Nazwa == inputValue).SelectMany(tp => tp.Posty).ToList();
                if (HttpContext.Session.GetInt32("SzukaneID") != null && obecny != null)
                {
                    testowa = db.Posty.Where(p => p.UzytkownikId == HttpContext.Session.GetInt32("SzukaneID")).ToList();
                    HttpContext.Session.Remove("SzukaneID");
                }

                //Tutaj koniec modu³u wyszukiwania tagu

                foreach (var idpost in testowa)
                {
                    var post = new PostWithComments();
                    post.Tagi = new List<String>();
                    post.KomentarzeId = new List<int>();
                    post.KomentarzePolubienia = new List<int>();
                    post.Komentarze = new List<Komentarz>();
                    post.KomentarzeTresc = new List<String>();
                    post.KomentarzeZDJ = new List<String>();
                    post.KomentarzeAutor = new List<String>();
                    post.KomentarzeData = new List<String>();
                    post.CzyPolubionyKomentarz = new List<bool>();
                    post.Id = idpost.PostId;
                    post.Image = @Url.Content(idpost.Zdjecie);
                    post.Opis = idpost.Opis;
                    post.ImageAvatar = @Url.Content(db.Uzytkownicy.Where(u => u.UzytkownikId == idpost.UzytkownikId).FirstOrDefault().Zdjecie);
                    if (post.ImageAvatar == null) post.ImageAvatar = "/ImgUploads/userTmpImg.jpg";
                    post.Nazwa = db.Uzytkownicy.Where(u => u.UzytkownikId == idpost.UzytkownikId).FirstOrDefault().Nazwa;
                    post.IloscPolubien = db.PolubieniaPostow.Count(u => u.PostId == idpost.PostId);
                    var tempKom = db.Komentarze.Where(u => u.PostId == idpost.PostId);

                    // Console.WriteLine(zalog.UzytkownikId + " " + post.Id + " " + PolubionyPost);

                    foreach (var kom in tempKom)
                    {
                        post.Komentarze.Add(kom);
                        post.KomentarzeId.Add(kom.KomentarzId);
                        post.KomentarzeTresc.Add(kom.Tresc);
                        post.KomentarzePolubienia.Add(db.PolubieniaKomentarzy.Count(u => u.KomentarzId == kom.KomentarzId));
                      

                        var zdj = db.Uzytkownicy.Where(u => u.UzytkownikId == kom.UzytkownikId).Select(u => u.Zdjecie).FirstOrDefault();
                        if (zdj == null) zdj = "/ImgUploads/userTmpImg.jpg";
                        post.KomentarzeZDJ.Add(@Url.Content(zdj));
                        //if (zdj) post.KomentarzeZDJ.Add(@Url.Content("/ImgUploads/userTmpImg.jpg"));


                        var autor = db.Uzytkownicy.Where(u => u.UzytkownikId == kom.UzytkownikId).Select(u => u.Nazwa).FirstOrDefault();
                        post.KomentarzeAutor.Add(autor);
                        post.KomentarzeData.Add(kom.DataPublikacji.ToString());
                    }
                    var tempTagi = db.Posty.Where(u => u.PostId == idpost.PostId).Select(s => s.Tagi);
                    foreach (var tag in tempTagi)
                    {
                        foreach (var x in tag)
                        {
                            post.Tagi.Add(x.Nazwa);
                        }
                    }
                    postsWithComments.Add(post);
                }
                postsWithComments = postsWithComments.OrderByDescending(x => x.Data).ToList();
            }

            //sprawdzamy czy jest sens ³adowaæ reklamê
            if (CzyVip == false)
            {
                Random rnd = new Random();
                int wybranaReklama = rnd.Next(3);

                switch (wybranaReklama)
                {
                    default: //zabezpieczenie
                        WyswReklama = "~/ImgUploads/_REKLAMA1.png";
                        break;
                    case 0:
                        WyswReklama = "~/ImgUploads/_REKLAMA1.png";
                        break;
                    case 1:
                        WyswReklama = "~/ImgUploads/_REKLAMA2.png";
                        break;
                    case 2:
                        WyswReklama = "~/ImgUploads/_REKLAMA3.png";
                        break;
                }
            }

            //wyœwietlamy jak ktoœ jest zalogowany kim jest - trzeba dodaæ w cshtml!!!!!!!
            if (CzyZalogowany)
            {
                LoginZalogowanego = zalogowany.Nazwa;
                ZdjecieZalogowanego = zalogowany.Zdjecie;
            }

            HttpContext.Session.Set("PostsList", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(postsWithComments)));
        }

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        [BindProperty]
        public string FilePath { get; set; }

        public IndexModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult OnPost()
        {
            if (UploadedFile != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, UploadedFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    UploadedFile.CopyTo(stream);
                }

                FilePath = filePath;
                var uz = db.Uzytkownicy.Where(s => s.UzytkownikId == 1);
                foreach (Uzytkownik u in uz)
                {
                    u.Zdjecie = filePath;
                }
                db.SaveChanges();
                //Console.WriteLine(filePath);
            }

            return Page();
        }

    }

}
