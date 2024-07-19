using Domain;
using Application.Korisnici;//sve iz foldera korisnici (create, delete... ce da se koristi pozivanjem iz mediatora)
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.src;
using Persistence;
using Application.Interfaces;
using AutoMapper;
using Application.Core;
using MediatR;
using System.Text;
using Application.Administrator;
using Microsoft.AspNetCore.WebUtilities;

namespace API.Controllers;

public class RandomStringGenerator
{
    private static readonly Random _random = new Random();
    public static string GenerateRandomString(int length, string charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!?.,:;|*-+/")
    {
        StringBuilder result = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            result.Append(charset[_random.Next(charset.Length)]);
        }
        return result.ToString();
    }
}

//[AllowAnonymous]
public class AdministratorController : BaseApiController
{
    private readonly UserManager<Korisnik> _userManager;
    private readonly DataContext _context;
    private readonly IEmailService _emailService; // Dodato za slanje emaila
    private readonly IMapper _mapper;

    public AdministratorController(UserManager<Korisnik> userManager, DataContext context, IEmailService emailService, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
        _userManager = userManager;
        _emailService = emailService; // Inicijalizacija servisa za email
    }


    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpGet("VratiSveKorisnike")]
    public async Task<IActionResult> VratiSveKorisnike()
    {
        return HandleResult(await Mediator.Send(new ListOfUsers.Query()));
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpPost("BanujKorisnika/{idKorisnika}")]
    public async Task<IActionResult> BanujKorisnika(Guid idKorisnika)
    {
        return HandleResult(await Mediator.Send(new BanujKorisnika.Command {KorisnikId = idKorisnika} ));
    }

    //PAR STVARI OVDE KOJE SE TESKO POSTIZU ALI MOGU DA BUDU BOLJE
    //da provalim kako sa mediator da ga odradim u klasu ListWithRoles ali je problem povratni tip jer ne ume da konveruje to u List<Korisnik> kad mu dodamo role
    //Da li da koristim Ok ili da nadjem nesto drugo
    //[Authorize(Policy = "RequireAdministratorRole")]
    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpGet("VratiSveKorisnikeSaUlogama")]
    public async Task<IActionResult> VratiSveKorisnikeSaUlogama()
    {
        var korisnici = await _userManager.Users.OrderBy(korisnik => korisnik.UserName).Select(korisnik => new  {
                Id = korisnik.Id,
                ime = korisnik.Ime,
                prezime = korisnik.Prezime,
                userName = korisnik.UserName,
                slika = korisnik.Slike.FirstOrDefault(s => s.IsMain).Url,
                roles = korisnik.KorisnikRoles.Select(role => role.Role).ToList()
        }).ToListAsync();
        return Ok(korisnici);
    }
    
    // [Authorize(Policy = "RequireAdministratorRole")]//nije jos spremna ali da znam da ce da bude tu
    // [HttpGet("ObrisiKorisnika")]
    // public async Task<IActionResult> ObrisiObilazak(Guid id)
    // {
    //     return HandleResult(await Mediator.Send(new Delete.Command {Id = id}));
    // }

    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpGet("zahtevi-vodica")]
    public async Task<ActionResult<List<VodicZahtev>>> GetVodicZahtevi()
    {
        var zahtevi = await _context.VodicZahtevi.ToListAsync();
        return Ok(zahtevi);
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpPost("odobri-vodica/{userId}")]
    public async Task<IActionResult> ApproveVodicRequest(Guid userId)
    {
        var user = await _context.VodicZahtevi.FindAsync(userId);
        if (user == null)
        {
            return NotFound();
        }
        var vodic = _mapper.Map<Vodic>(user);

        var tempPassword = RandomStringGenerator.GenerateRandomString(16); // Generisanje privremene šifre

        var result = await _userManager.CreateAsync(vodic, tempPassword);// kreiran vodic
        await _userManager.AddToRoleAsync(vodic, "TuristickiVodic");

        vodic.EmailConfirmed = true;// vodic odobren
        vodic.AccessFailedCount = 1;// OVAJ PARAMETAR ISPITUJ ZA PRVO LOGOVANJE JE 1-MORA DA MENJA SIFRU, kad promeni smanji ga na 0

        if (!result.Succeeded)
            return BadRequest(result.Errors);
        _context.VodicZahtevi.Remove(user);

        var origin = Request.Headers["origin"];
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(vodic);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var verifyUrl = $"{origin}/account/verifyEmail?token={token}&email={vodic.Email}";
        var message = $"<p>Vaš zahtev za vodiča je odobren. Vaša privremena šifra je: \" {tempPassword} \". Molimo vas da je promenite pri prvom logovanju.</p><p><a href='{verifyUrl}'>Kliknite za verifikaciju email-a</a></p>";

        await _emailService.SendGridEmailAsync(vodic.Email, "Zahtev odobren", message);

        await _userManager.UpdateAsync(vodic);

        return Ok("Zahtev odobren i email poslat.");
    }

    

}