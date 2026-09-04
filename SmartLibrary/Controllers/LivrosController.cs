using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.DTOs.Livro;
using SmartLibrary.Application.Interfaces;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class LivrosController : ControllerBase
{
    private readonly ILivroService _livroService;

    public LivrosController(ILivroService livroService)
    {
        _livroService = livroService;
    }

    [HttpGet]
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
    public async Task<ActionResult> Deactivate(int id)
    {
        await _livroService.Deactivate(id);
        return NoContent();
    }
}
