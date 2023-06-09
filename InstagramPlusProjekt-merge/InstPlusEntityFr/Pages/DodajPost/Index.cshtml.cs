using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InstPlusEntityFr;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;


namespace InstPlusEntityFr.Pages.DodajPost
{
    public class IndexModel : PageModel
    {
        DbInstagramPlus db = new DbInstagramPlus();
        public string DodajTagTxt { get; set; }

        public string UzytkownikTworzacy { get; set; }

        public string DodajOpisTxt { get; set; }

        public String errorMessage = "";

        [BindProperty]
        public IFormFile UploadedImage { get; set; }

        //niby lista ale dam set ¿eby nie powtarza³
        public HashSet<string> listaDodTagow = new HashSet<string>();

        public void OnGet()
        {


            Console.WriteLine(HttpContext.Session.GetString("OpisPostu"));
            if (HttpContext.Session.GetString("OpisPostu") != null)
            {
                DodajOpisTxt = HttpContext.Session.GetString("OpisPostu");
            }
            
            var zalogowanyUsr = db.Uzytkownicy.Where(u => u.UzytkownikId == HttpContext.Session.GetInt32("UzytkownikId")).First();
            UzytkownikTworzacy = $"u¿ytkownik: {zalogowanyUsr.Nazwa}";
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //DODAWANIE TAGÓW

        public IActionResult OnPostAppendTag(string dodajTagTxt, string opisTxt2)
        {
            if (opisTxt2 != null) HttpContext.Session.SetString("OpisPostu", opisTxt2.ToString()); else HttpContext.Session.SetString("OpisPostu", "");

            if (dodajTagTxt == null)
            {
                errorMessage = "brak tagu!";
                //dodaæ przywracanie wartoœci w labelu!
            }
            else if (!string.IsNullOrEmpty(DodajOpisTxt))
            {
                return null;
                //nie dzia³a jak bym chcia³ - nie wykonuje siê else
                //a przy pustym polu txt crashuje - poprawiæ bo da³em !
            }
            else
            {
                //zapisujemy wartoœæ pola opis ¿eby nie znika³o po return Page()
                //crashuje - sprawdziæ czemu i jak naprawiæ
                //HttpContext.Session.SetString("OpisPostu", DodajOpisTxt);

                //zczytuje z sesji zserializowane tagi
                string? listaTagowJSON = HttpContext.Session.GetString("ListaTagow");
                if (listaTagowJSON == null)
                {
                    listaTagowJSON = JsonConvert.SerializeObject(listaDodTagow);
                    HttpContext.Session.SetString("ListaTagow", listaTagowJSON);
                }
                else
                    listaDodTagow = JsonConvert.DeserializeObject<HashSet<string>>((string)listaTagowJSON);

                listaDodTagow.Add(dodajTagTxt.ToLower());
                DodajTagTxt = "tagi postu:";
                foreach (string t in listaDodTagow)
                {
                    DodajTagTxt += " #" + t;
                }

                //zapisuje do sesji zserializowane tagi z nowym
                listaTagowJSON = JsonConvert.SerializeObject(listaDodTagow);
                HttpContext.Session.SetString("ListaTagow", listaTagowJSON);

            }
            this.OnGet();

            return Page();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //PUBLIKOWANIE POSTU
        public async Task<IActionResult> OnPostOpublikujBtn(string OpisTxt)
        {
            Console.WriteLine(OpisTxt);
            string? listaTagowJSON = HttpContext.Session.GetString("ListaTagow");
            listaDodTagow = JsonConvert.DeserializeObject<HashSet<string>>((string)listaTagowJSON);

            //trzeba zrobiæ blokade ¿eby niezalogowany u¿ytkownik nie móg³ wejœæ na tê stronê
            if (HttpContext.Session.GetString("OpisPostu") != null && OpisTxt!="" && HttpContext.Session.GetString("OpisPostu")!="") OpisTxt = HttpContext.Session.GetString("OpisPostu");
            
            if (OpisTxt == null)
            {
                errorMessage = "Post musi zawieraæ opis!";
        
            }
            else if (listaDodTagow == null) //nie dzia³a
            {
                errorMessage = "Post musi zawieraæ przynajmniej 1 tag!";
            }
            else if (UploadedImage == null)
            {
                errorMessage = "Post musi zawieraæ zdjêcie!";
            }
            else
            {
                Post nowyPost = new Post();
                nowyPost.UzytkownikId = (int)HttpContext.Session.GetInt32("UzytkownikId");
                Console.WriteLine("T"+OpisTxt);

                nowyPost.Opis = OpisTxt;

                //trzeba dodawaæ te¿ te tagi u¿ytkownikowi

                foreach (string t in listaDodTagow)
                {
                    if (db.TagiPostow.Where(tag => tag.Nazwa == t).IsNullOrEmpty())
                    {
                        TagPostu nowyTag = new TagPostu();
                        nowyTag.Nazwa = (string)t;
                        nowyTag.Posty.Add(nowyPost);
                        nowyPost.Tagi.Add(nowyTag);
                        db.TagiPostow.Add(nowyTag);
                    }
                    else
                    {
                        TagPostu szukany = db.TagiPostow.Where(tag => tag.Nazwa == t).First();
                        nowyPost.Tagi.Add(szukany);
                        szukany.Posty.Add(nowyPost);
                    }
                }

                //dodawanie zdjêcia do folderu na serwerze

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "imgUploads", UploadedImage.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadedImage.CopyToAsync(stream);
                }

                nowyPost.Zdjecie = "~/ImgUploads/"+UploadedImage.FileName;

                db.Posty.Add(nowyPost);
                db.SaveChanges();

                HttpContext.Session.SetString("INFO", "Poprawnie dodano nowy post!"); //do wyœwietlwnia na profilu/g³ównej
                return RedirectToPage("/ProfilPrywatny/Index");

            }
            return Page();
        }
    }
}

/*
 * || DO POPRAWY W OKNIE: ||
 * przekierowania i blokada tylko dla zalogowanych
 * uzupe³nianie siê pola z opisem, labela tagów, zdjêcia po jakiejœ akcji - dokoñczyæ
 * dodawanie nie tylko tagów dla postu a te¿ dla u¿ytkownika
 * css i strona wizualna
 * wyœwietlanie zdjêcia profilowego oprócz loginu u¿ytkownika tworz¹cego post
*/