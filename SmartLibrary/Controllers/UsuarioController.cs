using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.DTOs.Usuario;
using SmartLibrary.Application.Interfaces;
using System.Security.Claims;

namespace SmartLibrary.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Create(
        [FromBody] CreateUsuarioDTO usuarioDto)
    {
        try
        {
            await _usuarioService.Add(usuarioDto);

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    message = "Usuário criado com sucesso."
                });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? nome = null,
        [FromQuery] string? email = null,
        [FromQuery] string? cpf = null,
        [FromQuery] string? telefone = null,
        [FromQuery] string? perfil = null)
    {
        var usuarios = await _usuarioService.GetAll(
            nome,
            email,
            cpf,
            telefone,
            perfil);

        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public async Task<IActionResult> GetById(string id)
    {
        var usuario = await _usuarioService.GetById(id);

        if (usuario == null)
        {
            return NotFound(new
            {
                message = "Usuário não encontrado."
            });
        }

        return Ok(usuario);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(usuarioId))
        {
            return Unauthorized();
        }
        var usuario = await _usuarioService.GetById(usuarioId);
        if (usuario == null)
        {
            return NotFound(new { message = "Usuário não encontrado." });
        }
        return Ok(usuario);
    }

    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateUsuarioDTO usuarioDto)
    {
        var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(usuarioId))
        {
            return Unauthorized();
        }

        try
        {
            await _usuarioService.Update(usuarioId, usuarioDto);
            return Ok(new 
            {
                message = "Usuário atualizado com sucesso." 
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new 
            { 
                message = ex.Message 
            });
        }
    }

    [HttpPut]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateUsuarioDTO usuarioDto)
    {
        try
        {
            await _usuarioService.Update(id, usuarioDto);
            return Ok(new
            {
                message = "Usuário atualizado com sucesso."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPatch("{id}/perfil")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> UpdatePerfil(
    string id,
    [FromBody] UpdatePerfilUsuarioDTO dto)
    {
        try
        {
            await _usuarioService.UpdatePerfil(id, dto.Perfil);

            return Ok(new
            {
                message = "Perfil atualizado com sucesso."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpDelete("{id}/desativar")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Deactivate(string id)
    {
        try
        {
            var usuario = await _usuarioService.Deactivate(id);

            return Ok(usuario);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}