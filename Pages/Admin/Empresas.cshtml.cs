using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PontoSaaS.Models;

[Authorize(Roles = "Admin")]
public class EmpresasModel : PageModel
{
    private readonly AppDbContext _context;

    public EmpresasModel(AppDbContext context)
    {
        _context = context;
    }

    public List<Empresa> Empresas { get; set; } = new();

    [BindProperty]
    public Empresa NovaEmpresa { get; set; }

    public async Task OnGetAsync()
    {
        Empresas = await _context.Empresas.ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        NovaEmpresa.Id = Guid.NewGuid();

        _context.Empresas.Add(NovaEmpresa);
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }
}