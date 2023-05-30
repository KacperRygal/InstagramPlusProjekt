using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstPlusEntityFr
{
    [Table("Uzytkownicy")]
    public class Uzytkownik
    {
        [Key]
        public int UzytkownikId { get; set; }

        [MaxLength(40)]
        public string Nazwa { get; set; }

        [MaxLength(40)]
        public string Haslo { get; set; }

        public string? Zdjecie { get; set; } //ścieżka do zdjęcia

        public DateTime? DataVipDo { get; set; } //data do której trwa vip, nullable

        public bool Moderator { get; set; } //flaga czy użytkownik jest moderatorem

        [MaxLength(200)]
        public string Opis { get; set; } = "";

        public List<TagUzytkownika> Tagi { get; set; }

        public bool testLoginu()
        {
            if (Nazwa.Length <= 40) return true; else return false;
        }
        public bool testHasla()
        {
            if (Haslo.Length <= 40 && Haslo.Length >= 8 && CzyMalaLitera(Haslo) && CzyDuzaLitera(Haslo) && CzyLiczba(Haslo)) return true;
            else return false;
        }
        private bool CzyMalaLitera(String t)
        {
            foreach (char c in t)
                if (char.IsUpper(c)) return true;
            return false;
        }
        private bool CzyDuzaLitera(String t)
        {
            foreach (char c in t)
                if (char.IsLower(c)) return true;
            return false;
        }
        private bool CzyLiczba(String t)
        {
            foreach (char c in t)
                if (char.IsDigit(c)) return true;
            return false;
        }

        public Uzytkownik() { }
        public Uzytkownik(int uzytkownikId, string nazwa, string haslo, bool moderator, List<TagUzytkownika> tagi)
        {
            UzytkownikId = uzytkownikId;
            Nazwa = nazwa;
            Haslo = haslo;
            DataVipDo = null;
            Moderator = moderator;
            Tagi = tagi;
        }
    }

    [Table("TagiUzytkownikow")]
    public class TagUzytkownika
    {
        [Key]
        public int TagId { get; set; }
        private string nazwa;
        public string Nazwa
        {
            get => nazwa;
            set => value.ToLower();
        }
        public int Counter { get; set; }
    }
}
