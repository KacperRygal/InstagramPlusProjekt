using InstPlusEntityFr.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace InstPlusEntityFr.Pages.MainPage
{

    public class IndexModel : PageModel
    {
		private readonly IWebHostEnvironment _environment;

		public List<String> autorzy_postow = new List<String>();

		public List<Post> posty = new List<Post>();

		
		public String getPostZdjecie(int id)
		{
			return posty.ElementAt(id).Zdjecie;
		}
		public String getPostOpis(int id)
		{
			return posty.ElementAt(id).Opis;
		}

		public List<String> getZdjecia()
		{ 
			List<String> result = new List<String>();
			foreach(Post p in posty)
			{
				result.Add(@Url.Content(p.Zdjecie));
			}
			return result;
		}

		public List<String> getOpisy()
		{
			List<String> result = new List<String>();
			foreach (Post p in posty)
			{
				result.Add(p.Opis);
			}
			return result;
			
		}

		public List<String> getZdjecieUzytkownik()
		{
			List<String> result = new List<String>();
			foreach (Post p in posty)
			{
				var xd = db.Uzytkownicy.Where(u => u.UzytkownikId == p.UzytkownikId);
				foreach(var item in xd)
				{
					result.Add(@Url.Content(item.Zdjecie));
				}
			}
			return result;
		}

		public List<String> getNazwaUzytkownik()
		{
			List<String> result = new List<String>();
			foreach (Post p in posty)
			{
				var xd = db.Uzytkownicy.Where(u => u.UzytkownikId == p.UzytkownikId);
				foreach (var item in xd)
				{
					result.Add(item.Nazwa);
				}
			}
			return result;
		}

		public List<int> getIdPostow()
		{
			List<int> result = new List<int>();
			foreach (Post p in posty)
			{
				result.Add(p.PostId);
			}
			return result;
		}

		public List<String> getKomentarze(int idPost)
		{
			List<String> result = new List<String>();
			var xd = db.Komentarze.Where(u => u.PostId == idPost);
			foreach (var item in xd)
			{
				result.Add(item.Tresc);
			}
			return result;
		}

		DbInstagramPlus db = new DbInstagramPlus();
		public void OnGet()
		{

			foreach (Post p in db.Posty)
			{
				posty.Add(p);
			}




			/*if(HttpContext.Session.IsAvailable)
			{
				//sprawdzenie ktory uzytkownik
			zalogowany = db.Uzytkownicy.Where(u => u.UzytkownikId == (int)HttpContext.Session.GetInt32("UzytkownikId")).FirstOrDefault();
				//algorytm do tagow
			}
			else
			{
				//wyswietlanie po koleji


			}*/

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
