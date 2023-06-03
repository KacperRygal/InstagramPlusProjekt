using InstPlusEntityFr.Migrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;

namespace InstPlusEntityFr.Pages.MainPage
{
	public class PostWithComments
	{
		public string Image { get; set; }
		public string Opis { get; set; }
		public string ImageAvatar { get; set; }
		public string Nazwa { get; set; }
		public List<String> Komentarze { get; set; }
		public int IloscPolubien { get; set; } 
	}
	public class IndexModel : PageModel
	{
		private readonly IWebHostEnvironment _environment;
		DbInstagramPlus db = new DbInstagramPlus();
		public String getPosty()
		{
			return JsonConvert.SerializeObject(postsWithComments);
		}

		private List<PostWithComments> postsWithComments = new List<PostWithComments>();

		public void OnGet()
		{
			foreach (var idpost in db.Posty)
			{
				var post = new PostWithComments();
				post.Komentarze = new List<String>();
				post.Image = @Url.Content(idpost.Zdjecie);
				post.Opis = idpost.Opis;
				post.ImageAvatar = @Url.Content(db.Uzytkownicy.Where(u => u.UzytkownikId == idpost.UzytkownikId).FirstOrDefault().Zdjecie);
				post.Nazwa = db.Uzytkownicy.Where(u => u.UzytkownikId == idpost.UzytkownikId).FirstOrDefault().Nazwa;
				post.IloscPolubien = db.PolubieniaPostow.Count(u=>u.PostId==idpost.PostId);
				var tempKom = db.Komentarze.Where(u=>u.PostId == idpost.PostId).Select(u=>u.Tresc);
				foreach(var kom in tempKom)
				{
					post.Komentarze.Add(kom);
				}
				postsWithComments.Add(post);
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
