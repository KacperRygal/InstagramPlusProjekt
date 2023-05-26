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
        public string Opis { get; set; } = "";

        public string Zdjecie { get; set; } //ścieżka do zdjęcia

        public int UzytkownikId { get; set; } //twórca posta

        public DateTime DataPublikacji { get; private set; }

        public List<TagPostu> Tagi { get; } = new();

        public Post()
        {
            DataPublikacji = DateTime.Now;
        }
    }

    [Table("TagiPostow")]
    public class TagPostu
    {
        [Key]
        public int TagId { get; set; }
        private string nazwa = "";
        public string Nazwa 
        {
            get => nazwa;
            set => nazwa = value.ToLower();
        }

        public TagPostu(string nazwa)
        {
            Nazwa = nazwa;
        }
        public TagPostu() { }

        public List<Post> Posty { get; } = new();
    }
}

//wyjaśniona lista
//https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many