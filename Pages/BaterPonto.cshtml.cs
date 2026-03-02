using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using PontoSaaS.Models;
using Microsoft.EntityFrameworkCore;

namespace PontoSaaS.Pages
{
    [Authorize]
    public class BaterPontoModel : PageModel
    {
        private readonly AppDbContext _context;

        public BaterPontoModel(AppDbContext context)
        {
            _context = context;
        }

        public List<RegistroPonto> Registros { get; set; } = new();
        public string? Mensagem { get; set; }

        public async Task OnGetAsync()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            Registros = await _context.RegistrosPonto
                .Where(r => r.UsuarioId == usuarioId)
                .OrderByDescending(r => r.DataHora)
                .Take(20)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(string tipo)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var empresaId = int.Parse(User.FindFirst("EmpresaId")!.Value);

            var registro = new RegistroPonto
            {
                UsuarioId = usuarioId,
                EmpresaId = empresaId,
                DataHora = DateTime.UtcNow,
                Tipo = tipo
            };

            _context.RegistrosPonto.Add(registro);
            await _context.SaveChangesAsync();

            Mensagem = "Ponto registrado com sucesso.";

            await OnGetAsync();

            return Page();
        }
    }
}