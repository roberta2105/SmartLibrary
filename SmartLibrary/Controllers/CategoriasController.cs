using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.DTOs.Categoria;
using SmartLibrary.Application.Interfaces;

namespace SmartLibrary.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
    {
        var categorias = await _categoriaService.GetAll();

        if (!categorias.Any())
        {
            return NotFound("Categorias não encontradas");
        }
        return Ok(categorias);
    }

    [HttpGet("{id:int}", Name = "GetCategoria")]
    public async Task<ActionResult<CategoriaDTO>> Get(int id)
    {
        var categoria = await _categoriaService.GetById(id);

        if (categoria == null)
        {
            return NotFound("Categoria não encontrada");
        }
        return Ok(categoria);
    }


    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateCategoriaDTO categoriaDto)
    {
        if(categoriaDto == null)
        {
            return BadRequest("Dados inválidos");
        }
        await _categoriaService.Add(categoriaDto);
        return Ok(categoriaDto);
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(int id, [FromBody] UpdateCategoriaDTO categoriaDto)
    {
        if (categoriaDto == null || id != categoriaDto.Id)
        {
            return BadRequest("Categoria inválida");
        }
        await _categoriaService.Update(categoriaDto);
        return Ok(categoriaDto);
    }


    [HttpDelete("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _categoriaService.Remove(id);
        return NoContent();
    }
}
