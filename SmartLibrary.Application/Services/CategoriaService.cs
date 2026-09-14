using AutoMapper;
using CleanArchMvc_V2.Domain.Validation;
using SmartLibrary.Application.DTOs.Categoria;
using SmartLibrary.Application.Interfaces;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Interfaces;

namespace SmartLibrary.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaService(
         IMapper mapper,
         IUnitOfWork unitOfWork,
         ICategoriaRepository categoriaRepository
        )
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _categoriaRepository = categoriaRepository;
    }

    public async Task<IEnumerable<CategoriaDTO>> GetAll()
    {
        var categorias = await _categoriaRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
    }

    public async Task<CategoriaDTO> GetById(int id)
    {
        var categoria = await _categoriaRepository.GetByIdAsync(id);

        return _mapper.Map<CategoriaDTO>(categoria);
    }


    public async Task Add(CreateCategoriaDTO categoriaDto)
    {
        var categoriaExists = await _categoriaRepository.HasActiveCategoryWithSameNameAsync(categoriaDto.Nome);

        if (categoriaExists)
            throw new DomainExceptionValidation("Já existe uma categoria ativa com o mesmo nome.");

        var categoria = new Categoria(categoriaDto.Nome);

        await _categoriaRepository.CreateAsync(categoria); 
        await _unitOfWork.CommitAsync();
    }

    public async Task Update(UpdateCategoriaDTO categoriaDto)
    { 
        var categoria = await _categoriaRepository.GetByIdAsync(categoriaDto.Id);

        if (categoria == null)
            throw new DomainExceptionValidation(
                "A categoria informada é inválida.");

        var categoriaComMesmoNome = await _categoriaRepository.HasActiveCategoryWithSameNameAsync(categoriaDto.Nome, categoriaDto.Id);

        if (categoriaComMesmoNome)
            throw new DomainExceptionValidation("Já existe uma categoria ativa com o mesmo nome.");

        categoria.Update(categoriaDto.Nome);

        await _categoriaRepository.UpdateAsync(categoria);  
        await _unitOfWork.CommitAsync();
    }

    public async Task Remove(int id)
    {
        var categoria = await _categoriaRepository.GetByIdAsync(id);

        if (categoria == null)
            throw new DomainExceptionValidation(
                 "A categoria informada é inválida.");

        var categoriaComLivros = await _categoriaRepository.HasActiveLivrosAsync(id);

        if(categoriaComLivros)
            throw new DomainExceptionValidation(
             "Essa categoria não pode ser excluída, pois está vinculada à livros ativos");

        await _categoriaRepository.RemoveAsync(categoria);
        await _unitOfWork.CommitAsync();
    }
}
