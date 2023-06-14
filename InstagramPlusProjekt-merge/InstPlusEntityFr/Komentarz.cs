using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstPlusEntityFr
{
    [Table("Komentarze")]
    public class Komentarz
    {
        [Key]
        public int KomentarzId { get; set; }

        [MaxLength(200)]
        public string Tresc { get; set; }

        public int UzytkownikId { get; set; } //autor komentarza

        public int PostId { get; set; } //post komentarza

        public DateTime DataPublikacji { get; private set; }

        public Komentarz()
        {
            DataPublikacji = DateTime.Now;
        }
    }
}
