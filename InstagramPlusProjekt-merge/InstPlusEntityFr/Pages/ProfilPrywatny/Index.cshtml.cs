using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InstPlusEntityFr.Pages.ProfilPrywatny
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
        public void OnGet()
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
            int zalogowanyId = (int)HttpContext.Session.GetInt32("UzytkownikId");
            zalogowany = db.Uzytkownicy.Where(u => u.UzytkownikId == zalogowanyId).FirstOrDefault();

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

            //wy�wietlenie statystyk
            IloscObserwowanych = db.Obserwowani.Where(o=>o.ObserwatorId == zalogowanyId).Count();
            IloscObserwujacych = db.Obserwujacy.Where(o=>o.ObserwowanyId == zalogowanyId).Count();
            IloscPolubZalogow = db.PolubieniaKomentarzy.Where(p => p.UzytkownikId == zalogowanyId).Count() +
                db.PolubieniaPostow.Where(p => p.UzytkownikId == zalogowanyId).Count();
            IloscPostowZalogow = db.Posty.Where(p=>p.UzytkownikId==zalogowanyId).Count();

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
        public async Task<IActionResult> OnPostZmienOpisBtn()
        {
            //powt�rzenie kodu - ale bez tego crashuje hmmm...
            int zalogowanyId = (int)HttpContext.Session.GetInt32("UzytkownikId");
            zalogowany = db.Uzytkownicy.Where(u => u.UzytkownikId == zalogowanyId).FirstOrDefault();

            zalogowany.Opis = OpisUzytkownika;
            Console.WriteLine(zalogowany.Opis);
            db.SaveChanges();

            //return Page() zwraca�o pust� stron�... why?
            return RedirectToPage("/ProfilPrywatny/Index");
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //CH�� ZAKUPU SUBSKRYPCJI
        public IActionResult OnPostZakupVipBtn()
        {
            return RedirectToPage("/OdblokujVip/Index");
        }

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
    }
}
