using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.DTOs.Usuario;
using SmartLibrary.Application.Interfaces;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDTO>>> Get(
         [FromQuery] string? nome = null,
         [FromQuery] string? email = null,
         [FromQuery] string? cpf = null,
         [FromQuery] string? telefone = null,
         [FromQuery] PerfilUsuario? perfil = null)
    {
        var usuarios = await _usuarioService.GetAll(
            nome,
            email,
            cpf,
            telefone,
            perfil
        );

        if (!usuarios.Any())
            return NotFound("Nenhum usuário encontrado");

        return Ok(usuarios);
    }

    [HttpGet("{id:int}", Name = "GetUsuario")]
    public async Task<ActionResult<UsuarioDTO>> Get(int id)
    {
        var usuario = await _usuarioService.GetById(id);

        if (usuario == null)
        {
            return NotFound("Usuario não encontrado");
        }
        return Ok(usuario);
    }


    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateUsuarioDTO usuarioDto)
    {
        if(usuarioDto == null)
        {
            return BadRequest("Dados inválidos");
        }
        await _usuarioService.Add(usuarioDto);
        return Ok(usuarioDto);
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromBody] UpdateUsuarioDTO usuarioDto)
    {
        if (usuarioDto == null || id != usuarioDto.Id)
        {
            return BadRequest("Usuario inválido");
        }
        await _usuarioService.Update(usuarioDto);
        return Ok(usuarioDto);
    }


    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Deactivate(int id)
    {
        await _usuarioService.Deactivate(id);
        return NoContent();
    }
}
