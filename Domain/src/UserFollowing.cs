namespace Domain
{
    public class UserFollowing
    {
        public Guid ObserverId { get; set; }
        public Guid TargetId { get; set; }
        public Korisnik Observer { get; set; }//ovo je korisnik koji prati vodica zove se Observer zato sto je nelogicno da se koristi Following Follower na svim mestima
        public Korisnik Target { get; set; }//ovo je vodic kojeg korisnik prati zove se targer jer nema smisla da se svuda zove following 
    }
}