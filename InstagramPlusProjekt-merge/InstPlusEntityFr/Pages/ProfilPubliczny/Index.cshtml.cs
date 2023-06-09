using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InstPlusEntityFr.Pages.ProfilPubliczny
{
    public class IndexModel : PageModel
    {
        DbInstagramPlus db = new DbInstagramPlus();
        public string ZdjecieProfilowe { get; set; }
        public string LoginUzytkownika { get; set; }

        [BindProperty]
        public string OpisUzytkownika { get; set; }

        [BindProperty]
        public IFormFile UploadedImage { get; set; }

        public bool CzyAdministrator { get; set; } //flagi dla przycisk�w
        public bool CzyVip { get; set; }

        public String errorMessage = "";

        public int IloscObserwowanych { get; set; } = 0;
        public int IloscObserwujacych { get; set; } = 0;
        public int IloscPostowZalogow { get; set; } = 0;
        public int IloscPolubZalogow { get; set; } = 0;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //LAUNCH

        Uzytkownik zalogowany;
        public void OnGet(string Nazwa)
        {
            //wy�wietlenie informacji
            if (HttpContext.Session.Keys.Contains("INFO"))
            {
                errorMessage = HttpContext.Session.GetString("INFO");
                HttpContext.Session.Remove("INFO");
            }
           
            HttpContext.Session.Remove("OpisPostu");
            HttpContext.Session.Remove("ListaTagow");
            //wyszukanie zalogowanego u�ytkownika
            string dataValue = HttpContext.Request.Query["data"];
            var zalogowany2 = dataValue;
            zalogowany = db.Uzytkownicy.Where(u => u.Nazwa == zalogowany2).FirstOrDefault();
            if(zalogowany==null) RedirectToPage("/MainPage/Index");
            //wy�wietlenie zdj�cia profilowego

            if (zalogowany.Zdjecie == null)
            {
                ZdjecieProfilowe = "~/ImgUploads/userTmpImg.jpg";
            }
            else
            {
                string sciezka = "~/ImgUploads/" + Path.GetFileName(zalogowany.Zdjecie);
                ZdjecieProfilowe = sciezka;
            }

            //wy�wietlenie opisu i loginu
            OpisUzytkownika = zalogowany.Opis;

            LoginUzytkownika = $"Profil u�ytkownika {zalogowany.Nazwa}";

            //sprawdzenie czy zalogowany jest adminem
            if (zalogowany.Moderator == null || zalogowany.Moderator == false)
                CzyAdministrator = false;
            else
                CzyAdministrator = true;

            //sprawdzenie czy zalogowany ma aktualn� subskrypcj�
            if (zalogowany.DataVipDo == null || zalogowany.DataVipDo < DateTime.Now)
                CzyVip = false;
            else
                CzyVip = true;

           // //wy�wietlenie statystyk
            IloscObserwowanych = db.Obserwowani.Where(o=>o.ObserwatorId == zalogowany.UzytkownikId).Count();
           IloscObserwujacych = db.Obserwujacy.Where(o=>o.ObserwowanyId == zalogowany.UzytkownikId).Count();
            IloscPolubZalogow = db.PolubieniaKomentarzy.Where(p => p.UzytkownikId == zalogowany.UzytkownikId).Count() +
                db.PolubieniaPostow.Where(p => p.UzytkownikId == zalogowany.UzytkownikId).Count();
          //  IloscPostowZalogow = db.Posty.Where(p=>p.UzytkownikId==zalogowanyId).Count();

            //prze�adowanie strony
            Page();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //ZMIANA ZDJ�CIA PROFILOWEGO
        public IActionResult OnPostZmienZdjecie()
        {
            return RedirectToPage("/ZmianaZdjecia/Index");
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //ZMIANA OPISU POSTA

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //CH�� ZAKUPU SUBSKRYPCJI

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //WY�WIETLANIE OBSERWOWANYCH
        public IActionResult OnPostWyswObserwowanychBtn()
        {
            return RedirectToPage("/WyswObserwowanych/Index");
        }

		////////////////////////////////////////////////////////////////////////////////////////////////////
		//WY�WIETLANIE OBSERWOWANYCH
		public IActionResult OnPostWyswObserwujacychBtn()
		{
			return RedirectToPage("/WyswObserwujacych/Index");
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		//WY�WIETLANIE OBSERWOWANYCH
		public IActionResult OnPostWyswStatystykiBtn()
        {
            return RedirectToPage("/StatystykiAdmin/Index");
        }
        public IActionResult OnPostWyswMojePostyBtn()
        {
            var zalogowany2 = LoginUzytkownika;
            Console.WriteLine("T@" + zalogowany2);
            zalogowany = db.Uzytkownicy.Where(u => u.Nazwa == zalogowany2).FirstOrDefault();
            HttpContext.Session.SetInt32("SzukaneID", (int)zalogowany.UzytkownikId);
            return RedirectToPage("/MainPage/Index");
        }
    }
}
