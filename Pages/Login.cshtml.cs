using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BCrypt.Net;

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

        HttpContext.Session.SetString("UserId", usuario.Id.ToString());

        return RedirectToPage("/Index");
    }
}