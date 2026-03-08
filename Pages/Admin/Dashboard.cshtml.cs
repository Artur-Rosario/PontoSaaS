using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PontoSaaS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PontoSaaS.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _context;

        public DashboardModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Empresa> Empresas { get; set; } = new();
        public List<Usuario> FuncionariosSelecionados { get; set; } = new(); // para AJAX ou reload
        public Guid? EmpresaSelecionadaId { get; set; }

        public async Task OnGetAsync(Guid? empresaId = null)
        {
            Empresas = await _context.Empresas
                .OrderBy(e => e.Nome)
                .ToListAsync();

            if (empresaId.HasValue)
            {
                EmpresaSelecionadaId = empresaId;
                FuncionariosSelecionados = await _context.Usuarios
                    .Where(u => u.EmpresaId == empresaId && u.Role != "Admin") // ou filtre como quiser
                    .OrderBy(u => u.Nome)
                    .ToListAsync();
            }
        }
    }
}