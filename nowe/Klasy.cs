using System.Reflection.Metadata;

namespace Projekt_InstagramPlus
{
    public class Komentarz
    {
        private String tresc;

        public int Id_komentarz { get; set; }
        public int Id_post { get; set; }
        public String Tresc
        {
            get { return tresc; }
            set
            {
                if (value.Length <= 100)
                    tresc = value;
                else
                    Console.WriteLine("za długi komentarz"); //jak zaimplementować zgłoszenie i wyświetlenie błędu na stronie??
            }
        }
        public int Id_uzytkownik { get; set; }

        public Komentarz(int id_komentarz, int id_post, String tresc, int id_uzytkownik)
        {
            Id_komentarz = id_komentarz;
            Id_post = id_post;
            Id_uzytkownik = id_uzytkownik;
            Tresc = tresc;
        }

        public Komentarz() { }
    }

    public class Obserwowany
    {
        public int Id_Obserwowanego { get; set; }
        public int Id_Obserwatora { get; set; }
        public Obserwowany(int id_obserwowanego, int id_obserwatora)
        {
            Id_Obserwatora = id_obserwatora;
            Id_Obserwowanego = id_obserwowanego;
        }
        public Obserwowany() { }
    }

    public class Obserwujacy : Obserwowany
    { 
        Obserwujacy() :base() { }
        Obserwujacy(int id_obserwowanego, int id_obserwatora) : base(id_obserwowanego, id_obserwatora) { }
    }

    public class Post
    {
        private String opis;

        public int Id_post { get; set; }
        public String Opis
        {
            get { return opis; }
            set
            {
                if (value.Length <= 200)
                    opis = value;
                else
                    Console.WriteLine("za długi opis"); //jak zaimplementować zgłoszenie i wyświetlenie błędu na stronie??
            }
        }

        public Blob Zdjecie { get; set; } //czy typ blob czy tablica byte[] ????
        public int Id_komentarz { get; set; }
        public int Id_uzytkownik { get; set; }

        public Post(int id_post, String opis, Blob zdjecie, int id_komentarz, int id_uzytkownik)
        {
            Id_post = id_post;
            Opis = opis;
            Zdjecie = zdjecie;
            Id_komentarz = id_komentarz;
            Id_uzytkownik = id_uzytkownik;
        }
        public Post() { }
    }

    public abstract class Polubienie
    {
        public int Id_polubienie { get; set; }
    }

    public class Polubienie_Komentarz : Polubienie
    {
        
        public int Id_uzytkownik { get; set; }
        public int Id_komentarz { get; set; }

        public Polubienie_Komentarz(int id_polubienie, int id_uzytkownik, int id_komentarz)
        {
            Id_polubienie = id_polubienie;
            Id_uzytkownik = id_uzytkownik;
            Id_komentarz = id_komentarz;
        }

        public Polubienie_Komentarz() { }
    }

    public class Polubienie_Post // nie ma własnego ID???? tak miało być?
    {
        public int Id_uzytkownik { get; set; }
        public int Id_post { get; set; }

        public Polubienie_Post() { }
        public Polubienie_Post(int id_uzytkownik, int id_post)
        {
            Id_uzytkownik = id_uzytkownik;
            Id_post = id_post;
        }
    }

    public class Uzytkownik
    {
        private String nazwa;
        private String haslo;
        private DateTime vip_do;
        public int Id_uzytkownik { get; set; }
        public String Nazwa
        {
            get { return nazwa; }
            set
            {
                if (value.Length <= 30)
                    nazwa = value;
                else
                    Console.WriteLine("za długa nazwa użytkownika"); //jak zaimplementować zgłoszenie i wyświetlenie błędu na stronie??
            }
        }
        public String Haslo
        {
            get { return haslo; }
            set
            {
                //hashowanie, sprawdzenie znaków
                if (value.Length <= 30)
                    haslo = value;
                else
                    Console.WriteLine("za długa nazwa użytkownika"); //jak zaimplementować zgłoszenie i wyświetlenie błędu na stronie??
            }
        }
        public Blob ZdjecieProfilowe { get; set; }
        public DateTime Vip_do
        {
            get { return vip_do; }
            set
            {
                if (value > DateTime.Now)
                    vip_do = value;
                else
                    Console.WriteLine("subskrypcja nieaktywna"); //jak zaimplementować zgłoszenie i wyświetlenie błędu na stronie??
            }
        }
        public bool Moderator { get; set; }

        public Uzytkownik(int id_uzytkownik, String nazwa, String haslo, Blob zdjecieProfilowe, DateTime vip_do, bool moderator)
        {
            Id_uzytkownik = id_uzytkownik;
            Nazwa = nazwa;
            Haslo = haslo;
            ZdjecieProfilowe = zdjecieProfilowe;
            Vip_do = vip_do;
            Moderator = moderator;
        }

        public Uzytkownik() { }
    }
}
