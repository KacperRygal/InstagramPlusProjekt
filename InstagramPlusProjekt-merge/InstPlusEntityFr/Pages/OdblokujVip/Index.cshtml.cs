using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InstPlusEntityFr.Pages.OdblokujVip
{
    public class IndexModel : PageModel
    {
        public string OpisSubskrypcji1 { get; set; }
        public string OpisSubskrypcji2 { get; set; }
        public string OpisSubskrypcji3 { get; set; }
        public string OpisSubskrypcji4 { get; set; }

        public void OnGet()
        {
            OpisSubskrypcji1 = "Masz doœæ reklam? zakup subskrypcjê VIP w dostêpnych planach:";
            OpisSubskrypcji2 = "  => tydzieñ: 10$";
            OpisSubskrypcji3 = "  => miesi¹c: 30$";
            OpisSubskrypcji4 = "  => 6 miesiêcy: 100$";
            
            Page();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //WPISANIE KODU
        public IActionResult OnPostPotwierdzKodBtn(string wpiszKodTxt)
        {
            //wyszukanie zalogowanego u¿ytkownika
            DbInstagramPlus db = new DbInstagramPlus();
            int zalogowanyId = (int)HttpContext.Session.GetInt32("UzytkownikId");
            Uzytkownik zalogowany = db.Uzytkownicy.Where(u => u.UzytkownikId == zalogowanyId).FirstOrDefault();

            //HttpContext.Session.SetString("INFO", "Subskrypcja VIP aktywna!");

            //sprawdzamy typ subskrypcji - kod na sztywno bo to nie bankowoœæ
            switch (wpiszKodTxt)
            {
                case "XXX-XXX-XXX":
                    zalogowany.DataVipDo = DateTime.Now.AddDays(7);
                    HttpContext.Session.SetString("INFO", $"Data subskrypcji VIP aktywna do" +
                        $"{zalogowany.DataVipDo}.\nDziêkujemy za dokonanie zakupu!");
                    db.SaveChanges();
                    break;
                case "YYY-YYY-YYY":
                    zalogowany.DataVipDo = DateTime.Now.AddMonths(1);
                    HttpContext.Session.SetString("INFO", $"Data subskrypcji VIP aktywna do" +
                        $"{zalogowany.DataVipDo}.\nDziêkujemy za dokonanie zakupu!");
                    db.SaveChanges();
                    break;
                case "ZZZ-ZZZ-ZZZ":
                    zalogowany.DataVipDo = DateTime.Now.AddMonths(6);
                    db.SaveChanges();
                    HttpContext.Session.SetString("INFO", $"Data subskrypcji VIP aktywna do" +
                        $"{zalogowany.DataVipDo}.\nDziêkujemy za dokonanie zakupu!");
                    break;
                default:
                    HttpContext.Session.SetString("INFO", "Nieprawid³owy Kod Produktu!");
                    break;
            }
            return RedirectToPage("/ProfilPrywatny/Index");
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //ANULOWANIE I POWRÓT DO PROFILU
        public IActionResult OnPostAnulujBtn()
        {
            return RedirectToPage("/ProfilPrywatny/Index");
        }
    }
}
