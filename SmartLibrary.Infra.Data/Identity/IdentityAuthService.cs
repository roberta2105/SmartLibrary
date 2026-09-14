using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartLibrary.Application.DTOs.Usuario;
using SmartLibrary.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartLibrary.Infra.Data.Identity;

public class IdentityAuthService : IIdentityAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public IdentityAuthService(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<TokenUsuarioDTO?> AuthenticateAsync(
        string email,
        string senha)
    {
        var usuario = await _userManager.FindByEmailAsync(email);

        if (usuario == null)
            return null;

        if (usuario.DataExclusao.HasValue)
            return null;

        var senhaValida = await _userManager.CheckPasswordAsync(
            usuario,
            senha);

        if (!senhaValida)
            return null;

        var roles = await _userManager.GetRolesAsync(usuario);

        var token = GenerateToken(usuario, roles);

        return new TokenUsuarioDTO
        {
            Token = token,
            UsuarioId = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email ?? string.Empty,
            Perfis = roles
        };
    }

    private string GenerateToken(
        ApplicationUser usuario,
        IList<string> roles)
    {
        var secretKey = _configuration["Jwt:SecretKey"];

        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new InvalidOperationException(
                "A chave secreta do JWT não foi configurada.");
        }

        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        if (string.IsNullOrWhiteSpace(issuer))
        {
            throw new InvalidOperationException(
                "O issuer do JWT não foi configurado.");
        }

        if (string.IsNullOrWhiteSpace(audience))
        {
            throw new InvalidOperationException(
                "O audience do JWT não foi configurado.");
        }

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id),
            new(JwtRegisteredClaimNames.Email, usuario.Email ?? string.Empty),

            new(ClaimTypes.NameIdentifier, usuario.Id),
            new(ClaimTypes.Name, usuario.Nome),
            new(ClaimTypes.Email, usuario.Email ?? string.Empty)
        };

        foreach (var role in roles)
        {
            claims.Add(
                new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}
