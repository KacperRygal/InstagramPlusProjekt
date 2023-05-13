using System.ComponentModel.DataAnnotations.Schema;

namespace InstPlusEntityFr
{
    [Table("Obserwujacy")]
    public class Obserwujacy : Obserwowany
    {
        public Obserwujacy(int obserwowanyId, int obserwatorId) :base(obserwowanyId, obserwatorId) { }
    }

    //ObserwowanyId --- id użytkownika x
    //ObserwatorId --- id osoby obserwującej użytkownika x
}
