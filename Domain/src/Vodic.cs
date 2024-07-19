namespace Domain
{
    public class Vodic : Korisnik
    {
        public string StrucnaSprema { get; set; }
        public int BrojOdrzanihObilazaka { get; set; }
        public ICollection<Ocena> Ocene { get; set; } = new List<Ocena>();//ideja - dodaj mu sliku diplome kao property da moze da ga koristi za registraciju 
        //i onda admin treba da ima fju koja vraca sve potencijalne vodice i moze da ih prihvati ili odbije, ako ih prihvati samo postavi neki flag na true i 
        //onda mogu da se prijave - URADJENO
        
    }
}