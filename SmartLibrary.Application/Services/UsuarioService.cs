using SmartLibrary.Application.DTOs.Usuario;
using SmartLibrary.Application.Interfaces;

namespace SmartLibrary.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IIdentityService _identityService;

    public UsuarioService(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<IEnumerable<UsuarioDTO>> GetAll(
        string? nome = null,
        string? email = null,
        string? cpf = null,
        string? telefone = null,
        string? perfil = null)
    {
        return await _identityService.GetAll(
            nome,
            email,
            cpf,
            telefone,
            perfil);
    }

    public async Task<UsuarioDTO?> GetById(string id)
    {
        return await _identityService.GetById(id);
    }

    public async Task Add(CreateUsuarioDTO usuarioDto)
    {
        await _identityService.Add(usuarioDto);
    }

    public async Task Update(string id, UpdateUsuarioDTO usuarioDto)
    {
        await _identityService.Update(id, usuarioDto);
    }

    public async Task UpdatePerfil(string id, string novoPerfil)
    {
        await _identityService.UpdatePerfil(id, novoPerfil);
    }

    public async Task<UsuarioDTO?> Deactivate(string id)
    {
        return await _identityService.Deactivate(id);
    }
}