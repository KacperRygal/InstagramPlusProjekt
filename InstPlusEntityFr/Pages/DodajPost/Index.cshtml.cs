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

        //niby lista ale dam set �eby nie powtarza�
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
                //nie dzia�a jak bym chcia� - nie wykonuje si� else
                //a przy pustym polu txt crashuje - poprawi� bo da�em !
            }
            else
            {
                //zapisujemy warto�� pola opis �eby nie znika�o po return Page()
                //crashuje - sprawdzi� czemu i jak naprawi�
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