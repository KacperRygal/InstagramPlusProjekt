using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstPlusEntityFr
{
    [Table("PolubieniaKomentarzy")]
    public class PolubienieKomentarza
    {
        public int KomentarzId { get; set; }

        public int UzytkownikId { get; set; }

        public PolubienieKomentarza(int komentarzId, int uzytkownikId)
        {
            KomentarzId = komentarzId;
            UzytkownikId = uzytkownikId;
        }
    }
}
