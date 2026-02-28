namespace PontoSaaS.Models
{
    public class Usuario
    {
            public Guid Id { get; set; } = Guid.NewGuid();
    public Guid EmpresaId { get; set; }
    public Empresa Empresa { get; set; }

  public string Nome { get; set; } = string.Empty;
    public string Email { get; set; }
    public string SenhaHash { get; set; }
    public string Role { get; set; }
    }
}