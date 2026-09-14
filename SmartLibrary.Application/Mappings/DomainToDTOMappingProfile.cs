using AutoMapper;
using SmartLibrary.Application.DTOs;
using SmartLibrary.Application.DTOs.Categoria;
using SmartLibrary.Application.DTOs.Livro;
using SmartLibrary.Domain.Entities;

namespace SmartLibrary.Application.Mappings;

public class DomainToDTOMappingProfile : Profile
{
    public DomainToDTOMappingProfile()
    {
        CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        CreateMap<Categoria, CreateCategoriaDTO>().ReverseMap();
        CreateMap<Categoria, UpdateCategoriaDTO>().ReverseMap();

        CreateMap<Livro, LivroDTO>().ReverseMap();
        CreateMap<Livro, CreateLivroDTO>().ReverseMap();
        CreateMap<Livro, UpdateLivroDTO>().ReverseMap();

        CreateMap<Emprestimo, EmprestimoDTO>()
            .ForMember(
                dest => dest.StatusEmprestimoLabel,
                opt => opt.MapFrom(src => src.StatusEmprestimo.ToString())
            );

        CreateMap<Emprestimo, CreateEmprestimoDTO>().ReverseMap();
    }
}