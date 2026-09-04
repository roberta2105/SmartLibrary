using AutoMapper;
using CleanArchMvc_V2.Domain.Validation;
using SmartLibrary.Application.DTOs.Livro;
using SmartLibrary.Application.Interfaces;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Interfaces;

namespace SmartLibrary.Application.Services;

public class LivroService : ILivroService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILivroRepository _livroRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    public LivroService(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ILivroRepository livroRepository,
        ICategoriaRepository categoriaRepository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _livroRepository = livroRepository;
        _categoriaRepository = categoriaRepository;
    }


    public async Task<IEnumerable<LivroDTO>> GetAll(
        string? titulo = null,
        string? descricao = null,
        string? autor = null,
        DateTime? dataPublicacao = null,
        string? isbn = null,
        int? categoriaId = null,
        string? categoriaNome = null)
    {
        var livros = await _livroRepository.GetAllAsync(
            titulo,
            descricao,
            autor,
            dataPublicacao,
            isbn,
            categoriaId,
            categoriaNome
        );
        return _mapper.Map<IEnumerable<LivroDTO>>(livros);
    }

    public async Task<LivroDTO> GetById(int id)
    {
        var livro = await _livroRepository.GetByIdAsync(id);

        if (livro == null)
            throw new DomainExceptionValidation(
                "Livro inválido.");

        return _mapper.Map<LivroDTO>(livro);
    }

    public async Task Add(CreateLivroDTO livroDto)
    {
        var isbnExistente = await _livroRepository.GetByIsbnAsync(livroDto.Isbn);

        if (isbnExistente != null)
            throw new DomainExceptionValidation(
                "Já existe um livro cadastrado com este ISBN.");

        var categoriaExistente = await _categoriaRepository.GetByIdAsync(livroDto.CategoriaId);

        if (categoriaExistente == null)
            throw new DomainExceptionValidation(
                "A categoria informada é inválida.");

        var livro = _mapper.Map<Livro>(livroDto);

        await _livroRepository.CreateAsync(livro);
        await _unitOfWork.CommitAsync();
    }

    public async Task Update(UpdateLivroDTO livroDto)
    {
        var livro = await _livroRepository.GetByIdAsync(livroDto.Id);

        if (livro == null)
            throw new DomainExceptionValidation(
                "O livro informado é inválido.");

        var isbnExistente = await _livroRepository.GetByIsbnAsync(livroDto.Isbn);

        if (isbnExistente != null && isbnExistente.Id != livroDto.Id)
            throw new DomainExceptionValidation(
                "Já existe um livro cadastrado com este ISBN.");

        var categoriaExistente = await _categoriaRepository.GetByIdAsync(livroDto.CategoriaId);

        if (categoriaExistente == null)
            throw new DomainExceptionValidation(
                "A categoria informada é inválida.");

        livro.Update(
            livroDto.Titulo,
            livroDto.Descricao,
            livroDto.Autor,
            livroDto.DataPublicacao,
            livroDto.Isbn,
            livroDto.CategoriaId,
            livroDto.QuantidadeTotal
        );

        await _livroRepository.UpdateAsync(livro);
        await _unitOfWork.CommitAsync();
    }

    public async Task<LivroDTO?> Deactivate(int id)
    {
        var livro = await _livroRepository.GetByIdAsync(id);

        if (livro == null)
            throw new DomainExceptionValidation(
                "Livro inválido.");

        var possuiEmprestimoAtivo = await _livroRepository.HasActiveEmprestimoAsync(id);

        if (possuiEmprestimoAtivo)
            throw new DomainExceptionValidation(
                "Não é possível inativar o livro, pois ele possui um empréstimo ativo.");

        livro.Deactivate();
        await _livroRepository.UpdateAsync(livro);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<LivroDTO>(livro);
    }
}
