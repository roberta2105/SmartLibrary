using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.DTOs.Usuario;
using SmartLibrary.Application.Interfaces;

namespace SmartLibrary.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private readonly IIdentityAuthService _identityAuthService;

    public TokenController(
        IIdentityAuthService identityAuthService)
    {
        _identityAuthService = identityAuthService;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<TokenUsuarioDTO>> Login(
        [FromBody] LoginUsuarioDTO loginDto)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var token = await _identityAuthService.AuthenticateAsync(
            loginDto.Email,
            loginDto.Senha);

        if (token == null)
        {
            return Unauthorized(
                new
                {
                    mensagem = "Email ou senha inválidos."
                });
        }

        return Ok(token);
    }
}
