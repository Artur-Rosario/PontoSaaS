public class RegistroPonto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int EmpresaId { get; set; }
    public DateTime DataHora { get; set; }
    public string Tipo { get; set; } = string.Empty;
}