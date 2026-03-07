public class RegistroPonto
{
    public int Id { get; set; }

    public Guid UsuarioId { get; set; }  
    public Guid EmpresaId { get; set; }    

    public DateTime DataHora { get; set; }
    public string Tipo { get; set; } = string.Empty;
}