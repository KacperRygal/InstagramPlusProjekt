using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstPlusEntityFr
{
    [Table("PolubieniaPostow")]
    public class PolubieniePosta
    {
        public int PostId { get; set; }

        public int UzytkownikId { get; set; }

        public PolubieniePosta(int postId, int uzytkownikId)
        {
            PostId = postId;
            UzytkownikId = uzytkownikId;
        }
    }
}
