using Microsoft.AspNetCore.Identity;

namespace SmartLibrary.Infra.Data.Identity;

public class ApplicationUser : IdentityUser
{
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public DateTime? DataExclusao { get; set; }

}
