using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace InstPlusEntityFr.Pages.ProfilPubliczny
{
    public class IndexModel : PageModel
    {
        DbInstagramPlus db = new DbInstagramPlus();
        public string ZdjecieProfilowe { get; set; }
        public string LoginUzytkownika { get; set; }

        public int IdUzytkownika { get; set; }
        [BindProperty]
        public string OpisUzytkownika { get; set; }

        [BindProperty]
        public bool CzyAdministrator { get; set; } //flagi dla przycisków
        public bool CzyVip { get; set; }

        public String errorMessage = "";

        public int IloscObserwowanych { get; set; } = 0;
        public int IloscObserwujacych { get; set; } = 0;
        public int IloscPostowZalogow { get; set; } = 0;
        public int IloscPolubZalogow { get; set; } = 0;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //LAUNCH

        public Uzytkownik profilowy;
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


            //wyszukanie profilu danego u¿ytkownika
            string dataValue = HttpContext.Request.Query["data"];
            var data = dataValue;
            if (data == null) 
            {
                var temp = db.Uzytkownicy.Where(u => u.UzytkownikId == HttpContext.Session.GetInt32("PowrotneID")).FirstOrDefault();
                data = temp.Nazwa;
                HttpContext.Session.Remove("PowrotneID");
            }

            profilowy = db.Uzytkownicy.Where(u => u.Nazwa == data).FirstOrDefault();
            
            if(profilowy == null) RedirectToPage("/MainPage/Index");


            //wyœwietlenie zdjêcia profilowego
            if (profilowy.Zdjecie == null)
            {
                ZdjecieProfilowe = "~/ImgUploads/userTmpImg.jpg";
            }
            else
            {
                string sciezka = "~/ImgUploads/" + Path.GetFileName(profilowy.Zdjecie);
                ZdjecieProfilowe = sciezka;
            }

            //zapis id
            IdUzytkownika = profilowy.UzytkownikId;

            //wyœwietlenie opisu i loginu
            OpisUzytkownika = profilowy.Opis;
            LoginUzytkownika = $"Profil u¿ytkownika {profilowy.Nazwa}";
            Console.WriteLine(LoginUzytkownika);

            //sprawdzenie czy obecny profil ma admina
            if (profilowy.Moderator == null || profilowy.Moderator == false)
                CzyAdministrator = false;
            else
                CzyAdministrator = true;

			//sprawdzenie czy obecny profil ma aktualn¹ subskrypcjê
			if (profilowy.DataVipDo == null || profilowy.DataVipDo < DateTime.Now)
                CzyVip = false;
            else
                CzyVip = true;

           //wyœwietlenie statystyk
            IloscObserwowanych = db.Obserwowani.Where(o=>o.ObserwatorId == profilowy.UzytkownikId).Count();
            IloscObserwujacych = db.Obserwujacy.Where(o=>o.ObserwowanyId == profilowy.UzytkownikId).Count();
            IloscPolubZalogow = db.PolubieniaKomentarzy.Where(p => p.UzytkownikId == profilowy.UzytkownikId).Count()+db.PolubieniaPostow.Where(p => p.UzytkownikId == profilowy.UzytkownikId).Count();
            IloscPostowZalogow = db.Posty.Where(p=>p.UzytkownikId==profilowy.UzytkownikId).Count();

            //prze³adowanie strony
            Page();
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //WYŒWIETLANIE OBSERWOWANYCH
        public IActionResult OnPostWyswObserwowanychBtn(int id)
        {
			HttpContext.Session.SetInt32("SzukaneID", id);
			return RedirectToPage("/WyswObserwowanych/Index");
        }


		////////////////////////////////////////////////////////////////////////////////////////////////////
		//WYŒWIETLANIE OBSERWOWUJACYCH
		public IActionResult OnPostWyswObserwujacychBtn(int id)
		{
			HttpContext.Session.SetInt32("SzukaneID", id);
			return RedirectToPage("/WyswObserwujacych/Index");
		}

		////////////////////////////////////////////////////////////////////////////////////////////////////
		//WYŒWIETLANIE POSTOW

		public IActionResult OnPostWyswMojePostyBtn(int id)
        {
			HttpContext.Session.SetInt32("SzukaneID", id);
			return RedirectToPage("/MainPage/Index");
			//var temp = db.Uzytkownicy.Where(u => u.Nazwa == nazwa).FirstOrDefault();
		}
    }
}
