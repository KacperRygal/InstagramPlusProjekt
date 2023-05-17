using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace InstPlusEntityFr.Pages.MainPage
{
    public class IndexModel : PageModel
    {
		private readonly IWebHostEnvironment _environment;
		DbInstagramPlus db = new DbInstagramPlus();
		public void OnGet()
        {
		}

		[BindProperty]
		public IFormFile UploadedFile { get; set; }

		[BindProperty]
		public string FilePath { get; set; }

		public IndexModel(IWebHostEnvironment environment)
		{
			_environment = environment;
		}

		public IActionResult OnPost()
		{
			if (UploadedFile != null)
			{
				var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}

				var filePath = Path.Combine(uploadsFolder, UploadedFile.FileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					UploadedFile.CopyTo(stream);
				}

				FilePath = filePath;
				var uz = db.Uzytkownicy.Where(s => s.UzytkownikId == 1);
				foreach (Uzytkownik u in uz)
				{
					u.Zdjecie = filePath;
				}
				db.SaveChanges();
				Console.WriteLine(filePath);
			}

			return Page();
		}

	}
}
