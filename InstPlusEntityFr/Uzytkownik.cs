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
    }
}
