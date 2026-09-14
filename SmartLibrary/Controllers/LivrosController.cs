using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.DTOs.Livro;
using SmartLibrary.Application.Interfaces;

namespace SmartLibrary.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LivrosController : ControllerBase
{
    private readonly ILivroService _livroService;

    public LivrosController(ILivroService livroService)
    {
        _livroService = livroService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<LivroDTO>>> Get(
          [FromQuery] string? titulo = null,
          [FromQuery] string? descricao = null,
          [FromQuery] string? autor = null,
          [FromQuery] DateTime? dataPublicacao = null,
          [FromQuery] string? isbn = null,
          [FromQuery] int? categoriaId = null,
          [FromQuery] string? categoriaNome = null)
    {
        var livros = await _livroService.GetAll(
             titulo,
             descricao,
             autor,
             dataPublicacao,
             isbn,
             categoriaId,
             categoriaNome
        );

        if (!livros.Any())
        return NotFound("Nenhum livro encontrado");
            
        return Ok(livros);
    }


    [HttpGet("{id:int}", Name = "GetLivro")]
    [Authorize]
    public async Task<ActionResult<LivroDTO>> Get(int id)
    {
        var livro = await _livroService.GetById(id);

        if (livro == null)
        {
            return NotFound("Livro não encontrado");
        }
        return Ok(livro);
    }


    [HttpPost]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public async Task<ActionResult> Post([FromBody] CreateLivroDTO livroDto)
    {
        if(livroDto == null)
        {
            return BadRequest("Dados inválidos");
        }
        await _livroService.Add(livroDto);
        return Ok(livroDto);
    }


    [HttpPut("{id:int}")]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public async Task<ActionResult> Put(int id, [FromBody] UpdateLivroDTO livroDto)
    {
        if (livroDto == null || id != livroDto.Id)
        {
            return BadRequest("Livro inválido");
        }
        await _livroService.Update(livroDto);
        return Ok(livroDto);
    }


    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Administrador,Bibliotecario")]
    public async Task<ActionResult> Deactivate(int id)
    {
        await _livroService.Deactivate(id);
        return NoContent();
    }
}
