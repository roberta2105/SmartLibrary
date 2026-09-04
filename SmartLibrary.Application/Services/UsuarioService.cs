using AutoMapper;
using CleanArchMvc_V2.Domain.Validation;
using SmartLibrary.Application.DTOs.Usuario;
using SmartLibrary.Application.Interfaces;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;
using SmartLibrary.Domain.Interfaces;


namespace SmartLibrary.Application.Services;

public class UsuarioService : IUsuarioService 
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IEmprestimoRepository _emprestimoRepository;

    public UsuarioService(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IUsuarioRepository usuarioRepository,
        IEmprestimoRepository emprestimoRepository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _usuarioRepository = usuarioRepository;
        _emprestimoRepository = emprestimoRepository;
    }

    public async Task<IEnumerable<UsuarioDTO>> GetAll(
        string? nome = null,
        string? email = null,
        string? cpf = null,
        string? telefone = null,
        PerfilUsuario? perfil = null)
    {
        var usuarios = await _usuarioRepository.GetAllAsync(
            nome,
            email,
            cpf,
            telefone,
            perfil
        );
        return _mapper.Map<IEnumerable<UsuarioDTO>>(usuarios);
    }

    public async Task<UsuarioDTO> GetById(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);

        return _mapper.Map<UsuarioDTO>(usuario);

    }

    public async Task Add(CreateUsuarioDTO usuarioDto)
    {
        var cpfExistente = await _usuarioRepository.GetByCpfAsync(usuarioDto.Cpf);

        if (cpfExistente != null)
            throw new DomainExceptionValidation(
                "O CPF informado pertence à outro usuário cadastrado no sistema");

        var emailExistente = await _usuarioRepository.GetByEmailAsync(usuarioDto.Email);

        if (emailExistente != null)
            throw new DomainExceptionValidation(
                "O e-mail informado pertence à outro usuário cadastrado no sistema");

        var usuario = _mapper.Map<Usuario>(usuarioDto);

        await _usuarioRepository.CreateAsync(usuario);
        await _unitOfWork.CommitAsync();
    }

    public async Task Update(UpdateUsuarioDTO usuarioDto)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(usuarioDto.Id);

        if (usuario == null)
            throw new DomainExceptionValidation(
                "O usuário informado é inválido.");


        var cpfExistente = await _usuarioRepository.GetByCpfAsync(usuarioDto.Cpf);

        if (cpfExistente != null && cpfExistente.Id != usuarioDto.Id)
            throw new DomainExceptionValidation(
                "O CPF informado pertence à outro usuário cadastrado no sistema");


        var emailExistente = await _usuarioRepository.GetByEmailAsync(usuarioDto.Email);

        if (emailExistente != null && emailExistente.Id != usuarioDto.Id)
            throw new DomainExceptionValidation(
                "O e-mail informado pertence à outro usuário cadastrado no sistema");

        usuario.Update(
            usuarioDto.Nome,
            usuarioDto.Email,
            usuarioDto.Cpf,
            usuarioDto.Telefone,
            usuarioDto.Perfil
        );

        await _usuarioRepository.UpdateAsync(usuario);
        await _unitOfWork.CommitAsync();
    }

    public async Task<UsuarioDTO?> Deactivate(int id)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(id);

        if (usuario == null)
            throw new DomainExceptionValidation(
                "Usuário inválido.");

        var possuiEmprestimoAtivo = await _emprestimoRepository.HasEmprestimoUsuarioAsync(usuario.Id, null, StatusEmprestimo.Emprestado);

        if (possuiEmprestimoAtivo)
            throw new DomainExceptionValidation(
                "Não é possível inativar o usuário, pois ele possui um empréstimo ativo.");

        usuario.Deactivate();
        await _usuarioRepository.UpdateAsync(usuario);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<UsuarioDTO>(usuario);
    }
}
