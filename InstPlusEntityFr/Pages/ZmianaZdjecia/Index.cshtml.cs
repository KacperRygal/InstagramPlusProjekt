using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InstPlusEntityFr.Pages.ZmianaZdjecia
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public IFormFile UploadedImage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostZmienZdjecieBtn()
        {
            DbInstagramPlus db = new DbInstagramPlus();
            if (UploadedImage != null)
            {
                int zalogowanyId = (int)HttpContext.Session.GetInt32("UzytkownikId");
                var zalogowany = db.Uzytkownicy.Where(u => u.UzytkownikId == zalogowanyId).FirstOrDefault();

                if (zalogowany != null)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "imgUploads", UploadedImage.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await UploadedImage.CopyToAsync(stream);
                    }

                    zalogowany.Zdjecie = "~/ImgUploads/" + UploadedImage.FileName;
                    db.SaveChanges();
                    HttpContext.Session.SetString("INFO", "Poprawnie zmieniono zdj�cie profilowe!");
                }

                return RedirectToPage("/ProfilPrywatny/Index");
            }
            else
            {
                HttpContext.Session.SetString("INFO", "Wyst�pi� b��d przy dodawaniu zdj�cia profilowego!");
                return RedirectToPage("/ProfilPrywatny/Index");
            }
        }

        public IActionResult OnPostAnulujBtn()
        {
            return RedirectToPage("/ProfilPrywatny/Index");
        }
    }
}
