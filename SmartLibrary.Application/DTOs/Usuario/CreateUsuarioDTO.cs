using System.ComponentModel.DataAnnotations;

namespace SmartLibrary.Application.DTOs.Usuario;

public class CreateUsuarioDTO
{
    [Required(ErrorMessage = "O nome é obrigatório!")]
    [MaxLength(100, ErrorMessage = "O nome deve conter até 100 caracteres")]
    public string Nome { get; set; } = string.Empty;


    [Required(ErrorMessage = "O email é obrigatório!")]
    [EmailAddress(ErrorMessage = "O email deve ser um endereço de email válido.")]
    public string Email { get; set; } = string.Empty;


    [Required(ErrorMessage = "O CPF é obrigatório!")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter exatamente 11 dígitos.")]
    public string Cpf { get; set; } = string.Empty;


    [Required(ErrorMessage = "O telefone é obrigatório!")]
    [MaxLength(20, ErrorMessage = "O telefone deve conter até 20 caracteres")]
    public string Telefone { get; set; } = string.Empty;


    [Required(ErrorMessage = "A senha é obrigatória!")]
    [MinLength(6, ErrorMessage = "A senha deve conter no mínimo 6 caracteres")]
    public string Senha { get; set; } = string.Empty;

}
