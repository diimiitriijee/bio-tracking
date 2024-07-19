using Application.Biljke;
using Application.Komentari;
using Application.Obilasci;
using AutoMapper;
using Domain;
using Domain.src;
using Microsoft.AspNetCore.Http;

namespace Application.Core;

public class MappingProfiles : Profile
{
    private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

    public MappingProfiles()
    {   
        string currentUsername = null;
        CreateMap<Obilazak, Obilazak>();
        CreateMap<Biljka, Biljka>();
        CreateMap<Podrucje, Podrucje>();
        CreateMap<Ruta, Ruta>();
        // ako zeza nesto obrisi ove ispod
        CreateMap<Korisnik, Korisnik>();
        CreateMap<Vodic, Vodic>();
        CreateMap<Profile, Profile>();
        
        CreateMap<Obilazak, ObilazakDto>();

        
        // CreateMap<Biljka, BiljkaDto>()
        //     .ForMember(dest => dest.Slika, opt => opt.Ignore());

        CreateMap<BiljkaDto, Biljka>()
            .ForMember(dest => dest.Slika, opt => opt.MapFrom(src => SaveFile(src.Slika)));


        CreateMap<PrijavljeniObilazak, Profiles.Profile>()
            .ForMember(d => d.Username, o => o.MapFrom(s => s.Korisnik.UserName))
            .ForMember(d => d.Ime, o => o.MapFrom(s => s.Korisnik.Ime))
            .ForMember(d => d.Prezime, o => o.MapFrom(s => s.Korisnik.Prezime))
            .ForMember(d => d.SlikaProfila, o => o.MapFrom(s => s.Korisnik.Slike.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(d => d.Telefon, o => o.MapFrom(s => s.Korisnik.Telefon))
            .ForMember(d => d.DatumRodjenja, o => o.MapFrom(s => s.Korisnik.DatumRodjenja));
        CreateMap<PrijavljeniObilazak, UcesnikDto>()
            .ForMember(d => d.Username, o => o.MapFrom(s => s.Korisnik.UserName))
            .ForMember(d => d.Ime, o => o.MapFrom(s => s.Korisnik.Ime))
            .ForMember(d => d.Prezime, o => o.MapFrom(s => s.Korisnik.Prezime))
            .ForMember(d => d.SlikaProfila, o => o.MapFrom(s => s.Korisnik.Slike.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(d => d.Telefon, o => o.MapFrom(s => s.Korisnik.Telefon))
            .ForMember(d => d.DatumRodjenja, o => o.MapFrom(s => s.Korisnik.DatumRodjenja));//ako nesto diku jebe kako da nabavi vodica iz ucesnikDTO ovde treba da se mapira  d.HostUsername na UcesnikDTO.IsHost sto ne postoji jos u tu klasu
        CreateMap<PrijavljeniObilazak, Profiles.KorisnikObilazakDto>()
            .ForMember(d => d.ID, o => o.MapFrom(s => s.ObilazakID))
            .ForMember(d => d.Naziv, o => o.MapFrom(s => s.Obilazak.Naziv))
            .ForMember(d => d.Opis, o => o.MapFrom(s => s.Obilazak.Opis))
            .ForMember(d => d.DatumOdrzavanja, o => o.MapFrom(s => s.Obilazak.DatumOdrzavanja))
            .ForMember(d => d.VodicUsername, o => o.MapFrom(s => s.Obilazak.Vodic.UserName));
        CreateMap<Obilazak, Profiles.KorisnikObilazakDto>()
            .ForMember(d => d.ID, o => o.MapFrom(s => s.ID))
            .ForMember(d => d.Naziv, o => o.MapFrom(s => s.Naziv))
            .ForMember(d => d.Opis, o => o.MapFrom(s => s.Opis))
            .ForMember(d => d.DatumOdrzavanja, o => o.MapFrom(s => s.DatumOdrzavanja))
            .ForMember(d => d.VodicUsername, o => o.MapFrom(s => s.Vodic.UserName));
        CreateMap<Korisnik, Profiles.Profile>()
            .ForMember(p => p.Ime, s => s.MapFrom(k => k.Ime))
            .ForMember(p => p.Prezime, s => s.MapFrom(k => k.Prezime))
            .ForMember(p => p.Username, s => s.MapFrom(k => k.UserName))
            .ForMember(p => p.Telefon, s => s.MapFrom(k => k.Telefon))
            .ForMember(p => p.DatumRodjenja, s => s.MapFrom(k => k.DatumRodjenja))
            .ForMember(p => p.SlikaProfila, s => s.MapFrom(k => k.Slike.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(p => p.Slike, s => s.MapFrom(k => k.Slike))
            .ForMember(p => p.BrojPrijavljenihObilazaka, s => s.MapFrom(k => k.PrijavljeniObilasci.Count()))
            .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count));//on ima samo broj koliko vodica prati
            
            
        CreateMap<Vodic, Profiles.ProfileVodic>()
            .ForMember(p => p.Ime, s => s.MapFrom(k => k.Ime))
            .ForMember(p => p.Prezime, s => s.MapFrom(k => k.Prezime))
            .ForMember(p => p.Username, s => s.MapFrom(k => k.UserName))
            .ForMember(p => p.Telefon, s => s.MapFrom(k => k.Telefon))
            .ForMember(p => p.DatumRodjenja, s => s.MapFrom(k => k.DatumRodjenja))
            .ForMember(p => p.SlikaProfila, s => s.MapFrom(k => k.Slike.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(p => p.Slike, s => s.MapFrom(k => k.Slike))
            .ForMember(p => p.StrucnaSprema, s => s.MapFrom(k => k.StrucnaSprema))
            .ForMember(p => p.BrojOdrzanihObilazaka, s => s.MapFrom(k => k.BrojOdrzanihObilazaka))
            .ForMember(p => p.Ocene, s => s.MapFrom(k => k.Ocene))// ako bude potrebno dodaj klasu OcenaDto
            .ForMember(p => p.ProsecnaOcena, s => s.MapFrom(k => k.Ocene != null && k.Ocene.Count > 0 ? k.Ocene.Average(o => o.VrednostOcene) : 0))
            .ForMember(p => p.BrojPrijavljenihObilazaka, s => s.MapFrom(k => k.PrijavljeniObilasci.Count()))
            .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
            .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count))
            .ForMember(d => d.Following, o => o.MapFrom(s => s.Followers.Any(x => x.Observer.UserName == currentUsername)));
            
        CreateMap<Profiles.Profile, Profiles.ProfileVodic>()
            .ForMember(p => p.Ime, s => s.MapFrom(k => k.Ime))
            .ForMember(p => p.Prezime, s => s.MapFrom(k => k.Prezime))
            .ForMember(p => p.Username, s => s.MapFrom(k => k.Username))
            .ForMember(p => p.Telefon, s => s.MapFrom(k => k.Telefon))
            .ForMember(p => p.DatumRodjenja, s => s.MapFrom(k => k.DatumRodjenja))
            .ForMember(p => p.SlikaProfila, s => s.MapFrom(k => k.Slike.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(p => p.Slike, s => s.MapFrom(k => k.Slike))
            .ForMember(p => p.BrojPrijavljenihObilazaka, s => s.MapFrom(k => k.BrojPrijavljenihObilazaka))
            .ForMember(p => p.FollowingCount, s => s.MapFrom(k=>k.FollowingCount));

        CreateMap<Komentar, KomentarDto>()
            .ForMember(d => d.Username, o => o.MapFrom(s => s.Korisnik.UserName))
            .ForMember(d => d.Ime, o => o.MapFrom(s => s.Korisnik.Ime))
            .ForMember(d => d.Prezime, o => o.MapFrom(s => s.Korisnik.Prezime))
            .ForMember(d => d.Tekst, o => o.MapFrom(s => s.Tekst))
            .ForMember(d => d.DatumKreiranja, o => o.MapFrom(s => s.DatumKreiranja))
            .ForMember(d => d.SlikaProfila, o => o.MapFrom(s => s.Korisnik.Slike.FirstOrDefault(x => x.IsMain).Url));

        CreateMap<VodicZahtev, Vodic>()
            .ForMember(p => p.Ime, s => s.MapFrom(k => k.Ime))
            .ForMember(p => p.Prezime, s => s.MapFrom(k => k.Prezime))
            .ForMember(p => p.UserName, s => s.MapFrom(k => k.Username))
            .ForMember(p => p.Telefon, s => s.MapFrom(k => k.Telefon))
            .ForMember(p => p.DatumRodjenja, s => s.MapFrom(k => k.DatumRodjenja))
            .ForMember(p => p.StrucnaSprema, s => s.MapFrom(k => k.StrucnaSprema))
            .ForMember(p => p.BrojOdrzanihObilazaka, s => s.MapFrom(k => k.BrojOdrzanihObilazaka));
            
    }
    private string SaveFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return null;

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(_uploadPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return fileName;
    }
}