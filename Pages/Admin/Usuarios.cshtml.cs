using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PontoSaaS.Models;

[Authorize(Roles="Admin")]
public class UsuariosModel : PageModel
{
    private readonly AppDbContext _context;

    public UsuariosModel(AppDbContext context)
    {
        _context = context;
    }

    public List<Usuario> Usuarios { get; set; } = new();
    public List<Empresa> Empresas { get; set; } = new();

    [BindProperty]
    public Usuario NovoUsuario { get; set; }

    [BindProperty]
    public string Senha { get; set; }

    public async Task OnGetAsync()
    {
        Usuarios = await _context.Usuarios
            .Include(u => u.Empresa)
            .ToListAsync();

        Empresas = await _context.Empresas.ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        NovoUsuario.Id = Guid.NewGuid();
        NovoUsuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(Senha);
        NovoUsuario.Role = "Funcionario";

        _context.Usuarios.Add(NovoUsuario);

        await _context.SaveChangesAsync();

        return RedirectToPage();
    }
}