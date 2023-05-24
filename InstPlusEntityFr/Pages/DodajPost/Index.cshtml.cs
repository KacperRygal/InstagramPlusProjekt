using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InstPlusEntityFr;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace InstPlusEntityFr.Pages.DodajPost
{
    public class IndexModel : PageModel
    {
        DbInstagramPlus db = new DbInstagramPlus();
        public string DodajTagTxt { get; set; }

        public string DodajOpisTxt { get; set; }

        private readonly ILogger<IndexModel> _logger; //to te¿ nie wiem
        private readonly IWebHostEnvironment _environment; //co to - nie wiem xd

        [BindProperty]
        public string ImagePath { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment environment) 
        {
            _logger = logger;
            _environment = environment;
        }

        //niby lista ale dam set ¿eby nie powtarza³
        public HashSet<string> listaDodTagow = new HashSet<string>();

        public void OnGet()
        {
            if (HttpContext.Session.GetString("OpisPostu") != null)
            {
                DodajOpisTxt = HttpContext.Session.GetString("OpisPostu");
            }
            else
            {
                DodajOpisTxt = "";
            }
        }

        public IActionResult OnPostAppendTag(string dodajTagTxt)
        {
            if (!string.IsNullOrEmpty(DodajOpisTxt))
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

                return Page();
            }
        }

        public IActionResult OnPostOpublikujBtn(string dodajOpisTxt, IFormFile image)
        {
            Post nowyPost = new Post();
            //nowyPost.UzytkownikId = (int)HttpContext.Session.GetInt32("UzytkownikId"); //OK
            //nowyPost.Opis = dodajOpisTxt; //OK

            string? listaTagowJSON = HttpContext.Session.GetString("ListaTagow");
            listaDodTagow = JsonConvert.DeserializeObject<HashSet<string>>((string)listaTagowJSON);

            //trzeba dodawaæ te¿ te tagi u¿ytkownikowi
            /*
            foreach(string t in listaDodTagow) //NIE DZIA£A - zmiana w bazie potrzebna
            {
                TagPostu nowyTag = new TagPostu();
                nowyTag.Nazwa = t;
                nowyPost.Tagi.Add(nowyTag);
            }*/

            //dodawanie zdjêcia do folderu na serwerze
            /*
            if (image != null) //NIE DZIA£A
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                };
                
                Console.WriteLine(filePath);
            }*/

            //przyda³by siê jakiœ komunikat/ nowa strona z tekstem ¿e dodano nowy post !!!
            return RedirectToPage("/MainPage/Index");
        }
    }
}