using AutoMapper;
using CleanArchMvc_V2.Domain.Validation;
using SmartLibrary.Application.DTOs;
using SmartLibrary.Application.Interfaces;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;
using SmartLibrary.Domain.Interfaces;

namespace SmartLibrary.Application.Services;

public class EmprestimoService : IEmprestimoService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmprestimoRepository _emprestimoRepository;
    private readonly ILivroRepository _livroRepository;
    private readonly IIdentityService _identityService;

    public EmprestimoService(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IEmprestimoRepository emprestimoRepository,
        ILivroRepository livroRepository,
        IIdentityService identityService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _emprestimoRepository = emprestimoRepository;
        _livroRepository = livroRepository;
        _identityService = identityService;
    }

    public async Task<IEnumerable<EmprestimoDTO>> GetAll(
        string? usuarioId = null,
        string? nomeUsuario = null,
        int? livroId = null,
        string? nomeLivro = null,
        int? quantidadeRenovacoes = null,
        bool? estaAtrasado = null,
        StatusEmprestimo? statusEmprestimo = null,
        DateTime? dataDevolucaoPrevista = null,
        DateTime? dataDevolucaoEfetiva = null)
    {
        var emprestimos = await _emprestimoRepository.GetAllAsync(
            usuarioId,
            nomeUsuario,
            livroId,
            nomeLivro,
            quantidadeRenovacoes,
            estaAtrasado,
            statusEmprestimo,
            dataDevolucaoPrevista,
            dataDevolucaoEfetiva);

        return _mapper.Map<IEnumerable<EmprestimoDTO>>(emprestimos);
    }

    public async Task<EmprestimoDTO> GetById(int id)
    {
        var emprestimo = await _emprestimoRepository.GetByIdAsync(id);

        if (emprestimo == null)
        {
            throw new DomainExceptionValidation(
                "Empréstimo inválido");
        }

        return _mapper.Map<EmprestimoDTO>(emprestimo);
    }

    public async Task Add(CreateEmprestimoDTO emprestimoDto)
    {
        var livro = await _livroRepository.GetByIdAsync(
            emprestimoDto.LivroId);

        if (livro == null)
        {
            throw new DomainExceptionValidation(
                "Livro inválido");
        }

        var usuario = await _identityService.GetById(
            emprestimoDto.UsuarioId);

        if (usuario == null || usuario.DataExclusao.HasValue)
        {
            throw new DomainExceptionValidation(
                "Usuário inválido");
        }

        var possuiMesmoLivroEmprestado =
            await _emprestimoRepository.HasEmprestimoUsuarioAsync(
                emprestimoDto.UsuarioId,
                emprestimoDto.LivroId,
                StatusEmprestimo.Emprestado);

        if (possuiMesmoLivroEmprestado)
        {
            throw new DomainExceptionValidation(
                "Não foi possível criar o empréstimo, pois o usuário já possui este livro emprestado.");
        }

        var possuiEmprestimoAtrasado =
            await _emprestimoRepository.HasEmprestimoUsuarioAsync(
                emprestimoDto.UsuarioId,
                null,
                StatusEmprestimo.Emprestado,
                true);

        if (possuiEmprestimoAtrasado)
        {
            throw new DomainExceptionValidation(
                "Não foi possível criar o empréstimo, pois o usuário possui pendências de empréstimos atrasadas. " +
                "");
        }

        var possuiLimiteEmprestimos =
            await _emprestimoRepository.HasLimiteEmprestimoUsuarioAsync(
                emprestimoDto.UsuarioId);

        if (possuiLimiteEmprestimos)
        {
            throw new DomainExceptionValidation(
                "Não foi possível criar o empréstimo, pois o usuário atingiu o limite de 5 empréstimos simultâneos.");
        }

        if (livro.QuantidadeDisponivel <= 0)
        {
            throw new DomainExceptionValidation(
                "Não foi possível criar o empréstimo, pois o livro não possui saldo disponível .");
        }
         
        livro.Emprestar();

        var emprestimo = new Emprestimo(
            emprestimoDto.LivroId,
            emprestimoDto.UsuarioId);

        await _livroRepository.UpdateAsync(livro);
        await _emprestimoRepository.CreateAsync(emprestimo);
        await _unitOfWork.CommitAsync();
    }

    public async Task Renovar(int id)
    {
        var emprestimo = await _emprestimoRepository.GetByIdAsync(id);

        if (emprestimo == null)
        {
            throw new DomainExceptionValidation(
                "Empréstimo inválido ");
        }

        emprestimo.Renovar();

        await _emprestimoRepository.UpdateAsync(emprestimo);
        await _unitOfWork.CommitAsync();
    }

    public async Task Devolver(int id)
    {
        var emprestimo = await _emprestimoRepository.GetByIdAsync(id);

        if (emprestimo == null)
        {
            throw new DomainExceptionValidation(
                "Empréstimo inválido");
        }

        var livro = await _livroRepository.GetByIdAsync(
            emprestimo.LivroId);

        if (livro == null)
        {
            throw new DomainExceptionValidation(
                "Livro inválido");
        }

        livro.Devolver();
        emprestimo.Devolver();

        await _livroRepository.UpdateAsync(livro);
        await _emprestimoRepository.UpdateAsync(emprestimo);
        await _unitOfWork.CommitAsync();
    }
}