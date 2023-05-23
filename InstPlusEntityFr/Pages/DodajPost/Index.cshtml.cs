using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using InstPlusEntityFr;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;

namespace InstPlusEntityFr.Pages.DodajPost
{
    public class IndexModel : PageModel
    {
        DbInstagramPlus db = new DbInstagramPlus();
        public string DodajTagTxt { get; set; }

        public string DodajOpisTxt { get; set; }

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
    }
}