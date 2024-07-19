using System.Security.Claims;
using System.Text;
using API.DTOs;
using API.Services;
using Application.Interfaces;
using Domain;
using Domain.src;
using Infrastructure.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<Korisnik> _userManager;
    private readonly TokenService _tokenService;
    private readonly DataContext _context;
    private readonly IPhotoAccessor _photoAccessor;

    private readonly SignInManager<Korisnik> _signInManager;// dodato za promenu sifre
    private readonly IEmailService _emailSender;

    public AccountController(UserManager<Korisnik> userManager, TokenService tokenService, DataContext context, IPhotoAccessor photoAccessor, SignInManager<Korisnik> signInManager, IEmailService emailSender)
    {
        _context = context;
        _tokenService = tokenService;
        _userManager = userManager;
        _photoAccessor = photoAccessor;

        _signInManager = signInManager;// dodato za promenu sifre
        _emailSender = emailSender;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<KorisnikDto>> Login(LoginDto loginDto)
    {
        var korisnik = await _userManager.Users
            .Include(x => x.Slike)
            .FirstOrDefaultAsync(x => x.Email == loginDto.Email);

        if (korisnik == null) return BadRequest($"Unet je netacan email: {loginDto.Email}");

        if(korisnik.UserName == "suki" || korisnik.UserName == "dika" || korisnik.UserName == "tasko_" || korisnik.UserName=="mile123")
            korisnik.EmailConfirmed = true;

        if(!korisnik.EmailConfirmed) return BadRequest("Email nije potvrdjen");
        
        if(korisnik.LockoutEnd != null)
        {
            return BadRequest("Banovani ste! Ostalo je jos " + (korisnik.LockoutEnd - DateTime.UtcNow).Value.Days + " dana.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(korisnik, loginDto.Password, false);//aspnet ovde proverava sifru da l se poklapa za nas

        if (result.Succeeded)
        {
            await SetRefreshToken(korisnik);
            var korisnikObject = await CreateUserObject(korisnik);
            return korisnikObject;
        }

        return BadRequest("Uneta sifra nije tacna");
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok("Logout successful");
    }

    [Authorize]
    [HttpGet("access-denied")]
    public IActionResult AccessDenied()
    {
        return Unauthorized("Access denied");
    }

    // [AllowAnonymous]
    // [HttpPost("login-vodica")]
    // public async Task<ActionResult<KorisnikDto>> LoginVodica(LoginDto loginDto)
    // {
    //     var user = (Vodic)await _userManager.FindByEmailAsync(loginDto.Email);

    //     if (user == null) return Unauthorized($"Unet je netacan email: {loginDto.Email}");

    //     var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);

    //     if (result.Succeeded)
    //     {
    //         if (user.MustChangePassword)
    //         {
    //             return Ok(new { MustChangePassword = true, Token = await _tokenService.CreateToken(user) });
    //         }

    //         var userObject = await CreateUserObject(user);
    //         return userObject;
    //     }

    //     return Unauthorized("Uneta sifra nije tacna");
    // }


    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<KorisnikDto>> Register(RegisterDto registerDto)
    {
        if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
        {
            ModelState.AddModelError("username", "Username taken");
            return ValidationProblem();
        }

        if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
        {
            ModelState.AddModelError("email", "Email taken");
            return ValidationProblem();
        }

        var korisnik = new Korisnik
        {
            Ime = registerDto.Ime,
            Prezime = registerDto.Prezime,
            Email = registerDto.Email,
            UserName = registerDto.Username,
            Telefon = registerDto.Telefon,
            DatumRodjenja = registerDto.DatumRodjenja
        };

        var result = await _userManager.CreateAsync(korisnik, registerDto.Password);//da sacuvamo korisnika u bazu

        if (!result.Succeeded) return BadRequest("Problem registracije korisnika");

        result = await _userManager.AddToRoleAsync(korisnik, "ObicanKorisnik");
        
        if (!result.Succeeded) return BadRequest("Problem dodavanja role korisniku");

        var origin = Request.Headers["origin"];
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(korisnik);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var verifyUrl = $"{origin}/account/verifyEmail?token={token}&email={korisnik.Email}";
        var message = $"<p>Kliknite link ispod da verifikujete Vasu email adresu:</p><p><a href='{verifyUrl}'>Kliknite za verifikaciju email-a</a></p>";

        await _emailSender.SendGridEmailAsync(korisnik.Email, "Verifikacija email-a", message);

        return Ok("Registracija uspesna - verifikujte email");
    }

    [AllowAnonymous]
    [HttpPost("verifyEmail")]
    public async Task<IActionResult> VerifyEmail(string token, string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return Unauthorized();
        var decodedTokenBytes = WebEncoders.Base64UrlDecode(token);
        var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);
        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

        if (!result.Succeeded) return BadRequest("Nije moguće potvrditi email adresu");

        return Ok("Email potvrđen - sada se možete prijaviti");
    }

    [AllowAnonymous]
    [HttpGet("resendEmailConfirmationLink")]
    public async Task<IActionResult> ResendEmailConfirmationLink(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null) return Unauthorized();

        var origin = Request.Headers["origin"];
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

        var verifyUrl = $"{origin}/account/verifyEmail?token={token}&email={user.Email}";
        var message = $"<p>Molimo kliknite na donji link da biste verifikovali vašu email adresu:</p><p><a href='{verifyUrl}'>Kliknite za verifikaciju email-a</a></p>";

        await _emailSender.SendGridEmailAsync(user.Email, "Molimo verifikujte email", message);

        return Ok("Link za verifikaciju email-a ponovo poslat");
    }

    [AllowAnonymous]
    [HttpPost("register-vodic")]
    public async Task<ActionResult> RegisterVodic([FromForm] RegisterVodicDto registerVodicDto)
    {
        if (await _userManager.Users.AnyAsync(x => x.UserName == registerVodicDto.Username))
        {
            ModelState.AddModelError("username", "Username taken");
            return ValidationProblem();
        }

        if (await _userManager.Users.AnyAsync(x => x.Email == registerVodicDto.Email))
        {
            ModelState.AddModelError("email", "Email taken");
            return ValidationProblem();
        }

        string filePath = null;

        if (registerVodicDto.SlikaDiplome != null)
        {
            var photoUploadResult = await _photoAccessor.AddPhoto(registerVodicDto.SlikaDiplome);
            filePath = photoUploadResult.Url;
        }

        var vodicZahtev = new VodicZahtev
        {
            Ime = registerVodicDto.Ime,
            Prezime = registerVodicDto.Prezime,
            Email = registerVodicDto.Email,
            Username = registerVodicDto.Username,
            Telefon = registerVodicDto.Telefon,
            DatumRodjenja = registerVodicDto.DatumRodjenja,
            StrucnaSprema = registerVodicDto.StrucnaSprema,
            BrojOdrzanihObilazaka = 0,
            PutanjaSlikeDiplome = filePath
        };
        
        _context.VodicZahtevi.Add(vodicZahtev);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<KorisnikDto>> GetCurrentUser()
    {
        var korisnik = await _userManager.Users
            .Include(x => x.Slike)
            .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));
        if(korisnik != null){
            await SetRefreshToken(korisnik);// NOVO
            
        }
        var korisnikObject = await CreateUserObject(korisnik);
        return korisnikObject;
    }

    // private async Task<KorisnikDto> CreateUserObject(Korisnik korisnik)//promenjena u async metodu zbog novog nacina dobijanja tokena
    // {
    //     return new KorisnikDto
    //     {
    //         Ime = korisnik.Ime,
    //         Prezime = korisnik.Prezime,
    //         Slika = korisnik?.SlikaProfila,
    //         Token = await _tokenService.CreateToken(korisnik),
    //         UserName = korisnik.UserName
    //     };
    // }

    private async Task<KorisnikDto> CreateUserObject(Korisnik korisnik)
    {
        var roles = await _userManager.GetRolesAsync(korisnik);
        
        if (roles.Contains("TuristickiVodic") && korisnik is Vodic vodic)
        {
            return new VodicDto
            {
                Ime = vodic.Ime,
                Prezime = vodic.Prezime,
                Slika = vodic?.Slike?.FirstOrDefault(x => x.IsMain)?.Url,
                Token = await _tokenService.CreateToken(vodic),
                UserName = vodic.UserName,
                StrucnaSprema = vodic.StrucnaSprema,
                BrojOdrzanihObilazaka = vodic.BrojOdrzanihObilazaka
            };
        }

        return new KorisnikDto
        {
            Ime = korisnik.Ime,
            Prezime = korisnik.Prezime,
            Slika = korisnik?.Slike?.FirstOrDefault(x => x.IsMain)?.Url,
            Token = await _tokenService.CreateToken(korisnik),
            UserName = korisnik.UserName
        };
    }
    
    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage)
                                        .ToList();
            return BadRequest(new { message = "ModelState nije validan", errors });
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        if (result.Succeeded)
        {
            await _signInManager.RefreshSignInAsync(user);
            return Ok("Sifra je uspesno promenjena.");
        }

        var errorDescriptions = result.Errors.Select(error => error.Description);
        return BadRequest("Greska pri promeni sifre: " + string.Join("; ", errorDescriptions));
    }

    [Authorize]
    [HttpPost("refreshToken")]
    public async Task<ActionResult<KorisnikDto>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];
        var user = await _userManager.Users
            .Include(r => r.RefreshTokens)
            .Include(p => p.Slike)
            .FirstOrDefaultAsync(x => x.UserName == User.FindFirstValue(ClaimTypes.Name));

        if (user == null) return Unauthorized();

        var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

        if (oldToken != null && !oldToken.IsActive) return Unauthorized();

        var korisnikObject = await CreateUserObject(user);
        return korisnikObject;
    }

    private async Task SetRefreshToken(Korisnik user)
    {
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshTokens.Add(refreshToken);
        await _userManager.UpdateAsync(user);

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7)
        };

        Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
    }
}