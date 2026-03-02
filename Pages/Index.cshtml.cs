using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BCrypt.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

public class LoginModel : PageModel
{
    private readonly AppDbContext _context;

    public LoginModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string Senha { get; set; }

    public string Erro { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        var usuario = _context.Usuarios
            .FirstOrDefault(u => u.Email == Email);

        if (usuario == null ||
            !BCrypt.Net.BCrypt.Verify(Senha, usuario.SenhaHash))
        {
            Erro = "Credenciais inválidas";
            return Page();
        }

        // HttpContext.Session.SetString("UserId", usuario.Id.ToString());
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
        new Claim(ClaimTypes.Name, usuario.Nome),
        new Claim("EmpresaId", usuario.EmpresaId.ToString()),
        new Claim(ClaimTypes.Role, usuario.Role)
    };

    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

    await HttpContext.SignInAsync(
    CookieAuthenticationDefaults.AuthenticationScheme,
    new ClaimsPrincipal(claimsIdentity));

    return RedirectToPage("/BaterPonto");


    }

}