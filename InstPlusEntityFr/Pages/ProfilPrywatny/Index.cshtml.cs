using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InstPlusEntityFr.Pages.ProfilPrywatny
{
    public class IndexModel : PageModel
    {
        DbInstagramPlus db = new DbInstagramPlus();
        public String ZdjecieProfilowe { get; set; }
        public string LoginUzytkownika { get; set; }

        [BindProperty]
        public IFormFile UploadedImage { get; set; }

        public String errorMessage = "";

        Uzytkownik zalogowany;
        public void OnGet()
        {
            if (HttpContext.Session.Keys.Contains("INFO"))
            {
                errorMessage = HttpContext.Session.GetString("INFO");
                HttpContext.Session.Remove("INFO");
            }

            int zalogowanyId = (int)HttpContext.Session.GetInt32("UzytkownikId");
            zalogowany = db.Uzytkownicy.Where(u => u.UzytkownikId == zalogowanyId).FirstOrDefault();

            if (zalogowany.Zdjecie == null)
            {
                ZdjecieProfilowe = "~/ImgUploads/userTmpImg.jpg";
            }
            else
            {
                string sciezka = "~/ImgUploads/" + Path.GetFileName(zalogowany.Zdjecie);
                ZdjecieProfilowe = sciezka;
            }


            LoginUzytkownika = $"Profil u¿ytkownika {zalogowany.Nazwa}";
            Page();
        }

        public IActionResult OnPostZmienZdjecie()
        {
            Console.WriteLine(UploadedImage.ToString());
            return Page();
        }
    }
}
