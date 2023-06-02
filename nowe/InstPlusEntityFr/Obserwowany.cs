using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstPlusEntityFr
{
    [Table("Obserwowani")]
    public class Obserwowany
    {
        public int ObserwowanyId { get; set; } //id osoby którą obserwuje użytkownik x

        public int ObserwatorId { get; set; } //id użytkownika x

        public Obserwowany(int obserwowanyId, int obserwatorId)
        {
            ObserwatorId = obserwatorId;
            ObserwowanyId = obserwowanyId;
        }
    }
}
