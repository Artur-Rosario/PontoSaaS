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
    public class PontosDiaModel : PageModel
    {
        private readonly AppDbContext _context;

        public PontosDiaModel(AppDbContext context)
        {
            _context = context;
        }

        public Guid EmpresaId { get; set; }
        public Guid? UsuarioId { get; set; }
        public DateTime DataSelecionada { get; set; } = DateTime.UtcNow.Date;
        public string NomeEmpresa { get; set; } = string.Empty;
        public string? NomeFuncionario { get; set; }

        public List<RegistroPontoViewModel> Registros { get; set; } = new();

        public async Task OnGetAsync(Guid empresaId, Guid? usuarioId = null, string? data = null)
        {
            EmpresaId = empresaId;
            UsuarioId = usuarioId;

            var empresa = await _context.Empresas.FindAsync(empresaId);
            if (empresa == null)
            {
                // Tratar erro (pode redirecionar ou mostrar mensagem)
                NomeEmpresa = "Empresa não encontrada";
                return;
            }
            NomeEmpresa = empresa.Nome;

            if (data != null && DateTime.TryParse(data, out var parsedData))
            {
                DataSelecionada = parsedData.Date;
            }

            if (usuarioId.HasValue)
            {
                var usuario = await _context.Usuarios.FindAsync(usuarioId);
                if (usuario != null)
                    NomeFuncionario = usuario.Nome;
            }

            // Busca dos registros
         // Substitua a query por isso:

var query = from r in _context.RegistrosPonto
            join u in _context.Usuarios on r.UsuarioId equals u.Id
            where r.EmpresaId == empresaId 
               && r.DataHora.Date == DataSelecionada
            orderby u.Nome, r.DataHora
            select new { Registro = r, Usuario = u };

if (usuarioId.HasValue)
{
    query = query.Where(x => x.Registro.UsuarioId == usuarioId.Value);
}

var resultados = await query.ToListAsync();

Registros = resultados.Select(x => new RegistroPontoViewModel
{
    Id = x.Registro.Id,
    FuncionarioNome = x.Usuario.Nome,
    DataHora = x.Registro.DataHora,
    Tipo = x.Registro.Tipo,
    HoraFormatada = x.Registro.DataHora.ToString("HH:mm"),
    DataFormatada = x.Registro.DataHora.ToString("dd/MM/yyyy")
}).ToList();
}
        // Helper para view
        public class RegistroPontoViewModel
        {
            public int Id { get; set; }
            public string FuncionarioNome { get; set; } = string.Empty;
            public DateTime DataHora { get; set; }
            public string Tipo { get; set; } = string.Empty;
            public string HoraFormatada { get; set; } = string.Empty;
            public string DataFormatada { get; set; } = string.Empty;
        }
    }
}