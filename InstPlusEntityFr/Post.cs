using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstPlusEntityFr
{
    [Table("Posty")]
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [MaxLength(200)]
        public string Opis { get; set; }

        public string Zdjecie { get; set; } //ścieżka do zdjęcia

        public int UzytkownikId { get; set; } //twórca posta
    }
}
