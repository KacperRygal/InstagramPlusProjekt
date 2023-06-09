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

        public bool CzyAdministrator { get; set; } //flagi dla przycisków
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
            //wyœwietlenie informacji
            if (HttpContext.Session.Keys.Contains("INFO"))
            {
                errorMessage = HttpContext.Session.GetString("INFO");
                HttpContext.Session.Remove("INFO");
            }
           
            HttpContext.Session.Remove("OpisPostu");
            HttpContext.Session.Remove("ListaTagow");
            //wyszukanie zalogowanego u¿ytkownika
            string dataValue = HttpContext.Request.Query["data"];
            var zalogowany2 = dataValue;
            zalogowany = db.Uzytkownicy.Where(u => u.Nazwa == zalogowany2).FirstOrDefault();
            if(zalogowany==null) RedirectToPage("/MainPage/Index");
            //wyœwietlenie zdjêcia profilowego

            if (zalogowany.Zdjecie == null)
            {
                ZdjecieProfilowe = "~/ImgUploads/userTmpImg.jpg";
            }
            else
            {
                string sciezka = "~/ImgUploads/" + Path.GetFileName(zalogowany.Zdjecie);
                ZdjecieProfilowe = sciezka;
            }

            //wyœwietlenie opisu i loginu
            OpisUzytkownika = zalogowany.Opis;

            LoginUzytkownika = $"Profil u¿ytkownika {zalogowany.Nazwa}";

            //sprawdzenie czy zalogowany jest adminem
            if (zalogowany.Moderator == null || zalogowany.Moderator == false)
                CzyAdministrator = false;
            else
                CzyAdministrator = true;

            //sprawdzenie czy zalogowany ma aktualn¹ subskrypcjê
            if (zalogowany.DataVipDo == null || zalogowany.DataVipDo < DateTime.Now)
                CzyVip = false;
            else
                CzyVip = true;

           // //wyœwietlenie statystyk
            IloscObserwowanych = db.Obserwowani.Where(o=>o.ObserwatorId == zalogowany.UzytkownikId).Count();
           IloscObserwujacych = db.Obserwujacy.Where(o=>o.ObserwowanyId == zalogowany.UzytkownikId).Count();
            IloscPolubZalogow = db.PolubieniaKomentarzy.Where(p => p.UzytkownikId == zalogowany.UzytkownikId).Count() +
                db.PolubieniaPostow.Where(p => p.UzytkownikId == zalogowany.UzytkownikId).Count();
          //  IloscPostowZalogow = db.Posty.Where(p=>p.UzytkownikId==zalogowanyId).Count();

            //prze³adowanie strony
            Page();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //ZMIANA ZDJÊCIA PROFILOWEGO
        public IActionResult OnPostZmienZdjecie()
        {
            return RedirectToPage("/ZmianaZdjecia/Index");
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //ZMIANA OPISU POSTA

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //CHÊÆ ZAKUPU SUBSKRYPCJI

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //WYŒWIETLANIE OBSERWOWANYCH
        public IActionResult OnPostWyswObserwowanychBtn()
        {
            return RedirectToPage("/WyswObserwowanych/Index");
        }

		////////////////////////////////////////////////////////////////////////////////////////////////////
		//WYŒWIETLANIE OBSERWOWANYCH
		public IActionResult OnPostWyswObserwujacychBtn()
		{
			return RedirectToPage("/WyswObserwujacych/Index");
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		//WYŒWIETLANIE OBSERWOWANYCH
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
