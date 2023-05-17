using System.ComponentModel.DataAnnotations.Schema;

namespace InstPlusEntityFr
{
    [Table("Obserwujacy")]
    public class Obserwujacy
    {
        public int ObserwowanyId { get; set; } //id użytkownika x

        public int ObserwatorId { get; set; } //id osoby obserwującej użytkownika x

        public Obserwujacy(int obserwowanyId, int obserwatorId)
        {
            ObserwatorId = obserwatorId;
            ObserwowanyId = obserwowanyId;
        }
    }
}
