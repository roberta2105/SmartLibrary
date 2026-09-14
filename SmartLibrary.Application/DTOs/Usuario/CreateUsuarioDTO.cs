using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Application.DTOs.Usuario;

public class CreateUsuarioDTO
{
    [Required(ErrorMessage = "O nome é obrigatório!")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;


    [Required(ErrorMessage = "O email é obrigatório!")]
    public string Email { get; set; } = string.Empty;


    [Required(ErrorMessage = "O CPF é obrigatório!")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter exatamente 11 dígitos.")]
    public string Cpf { get; set; } = string.Empty;


    [Required(ErrorMessage = "O telefone é obrigatório!")]
    [MaxLength(20)]
    public string Telefone { get; set; } = string.Empty;


    [Required(ErrorMessage = "A senha é obrigatória!")]
    [MinLength(6)]
    public string Senha { get; set; } = string.Empty;

}
