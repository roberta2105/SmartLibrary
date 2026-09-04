using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Application.DTOs;
using SmartLibrary.Application.Interfaces;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;

namespace SmartLibrary.API.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class EmprestimosController : ControllerBase
{
    private readonly IEmprestimoService _emprestimoService;

    public EmprestimosController(IEmprestimoService emprestimoService)
    {
        _emprestimoService = emprestimoService;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmprestimoDTO>>> Get(
    [FromQuery] int? usuarioId = null,
    [FromQuery] int? livroId = null,
    [FromQuery] string? nomeLivro = null,
    [FromQuery] string? nomeUsuario = null,
    [FromQuery] int? quantidadeRenovacoes = null,
    [FromQuery] bool? estaAtrasado = null,
    [FromQuery] StatusEmprestimo? statusEmprestimo = null,
    [FromQuery] DateTime? dataDevolucaoPrevista = null,
    [FromQuery] DateTime? dataDevolucaoEfetiva = null)
    {
        var emprestimos = await _emprestimoService.GetAll(
            usuarioId,
            nomeUsuario,
            livroId,
            nomeLivro,
            quantidadeRenovacoes,
            estaAtrasado,
            statusEmprestimo,
            dataDevolucaoPrevista,
            dataDevolucaoEfetiva);

        if (!emprestimos.Any() || emprestimos == null)
        {
            return NotFound("Nenhum emprestimo encontrado");
        }

        return Ok(emprestimos);
    }

    [HttpGet("{id:int}", Name = "GetEmprestimo")]
    public async Task<ActionResult<EmprestimoDTO>> Get(int id)
    {
        var emprestimo = await _emprestimoService.GetById(id);

        if (emprestimo == null)
        {
            return NotFound("Emprestimo não encontrado");
        }
        return Ok(emprestimo);
    }


    [HttpPost]
    public async Task<ActionResult> Post([FromBody] CreateEmprestimoDTO emprestimoDto)
    {
        if (emprestimoDto == null)
        {
            return BadRequest("Dados inválidos");
        }
        await _emprestimoService.Add(emprestimoDto);
        return Ok(emprestimoDto);
    }

    [HttpPut("{id:int}/renovar")]
    public async Task<ActionResult> Renovar(int id)
    {
        var emprestimo = await _emprestimoService.Renovar(id);

        if (emprestimo == null)
            return BadRequest("Empréstimo inválido");

        return Ok(emprestimo);
    }

    [HttpPut("{id:int}/devolver")]
    public async Task<ActionResult> Devolver(int id)
    {
        var emprestimo = await _emprestimoService.Devolver(id);

        if (emprestimo == null)
            return BadRequest("Empréstimo inválido");

        return Ok(emprestimo);
    }
}
