using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartLibrary.Application.DTOs.Usuario;
using SmartLibrary.Application.Interfaces;
using SmartLibrary.Domain.Enums;
using SmartLibrary.Domain.Interfaces;
using SmartLibrary.Infra.Data.Context;

namespace SmartLibrary.Infra.Data.Identity;

public class IdentityService : IIdentityService
{
    private const string PerfilUsuario = "Usuario";
    private const string PerfilBibliotecario = "Bibliotecario";
    private const string PerfilAdministrador = "Administrador";

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IEmprestimoRepository _emprestimoRepository;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        IEmprestimoRepository emprestimoRepository)
    {
        _userManager = userManager;
        _context = context;
        _emprestimoRepository = emprestimoRepository;
    }

    private async Task<ApplicationUser?> ObterUsuarioAtivoAsync(string id)
    {
        return await _userManager.Users
            .FirstOrDefaultAsync(u =>
                u.Id == id &&
                u.DataExclusao == null);
    }

    public async Task<IEnumerable<UsuarioDTO>> GetAll(
    string? nome = null,
    string? email = null,
    string? cpf = null,
    string? telefone = null,
    string? perfil = null)
    {
        var query =
            from usuario in _context.Users
            join userRole in _context.UserRoles
                on usuario.Id equals userRole.UserId
            join role in _context.Roles
                on userRole.RoleId equals role.Id
            where usuario.DataExclusao == null
            select new
            {
                Usuario = usuario,
                Perfil = role.Name
            };

        if (!string.IsNullOrWhiteSpace(nome))
        {
            query = query.Where(x =>
                x.Usuario.Nome.Contains(nome));
        }

        if (!string.IsNullOrWhiteSpace(email))
        {
            query = query.Where(x =>
                x.Usuario.Email != null &&
                x.Usuario.Email.Contains(email));
        }

        if (!string.IsNullOrWhiteSpace(cpf))
        {
            query = query.Where(x =>
                x.Usuario.Cpf.Contains(cpf));
        }

        if (!string.IsNullOrWhiteSpace(telefone))
        {
            query = query.Where(x =>
                x.Usuario.PhoneNumber != null &&
                x.Usuario.PhoneNumber.Contains(telefone));
        }

        if (!string.IsNullOrWhiteSpace(perfil))
        {
            query = query.Where(x =>
                x.Perfil == perfil);
        }

        var usuarios = await query
            .Select(x => new UsuarioDTO
            {
                Id = x.Usuario.Id,
                Nome = x.Usuario.Nome,
                Email = x.Usuario.Email ?? string.Empty,
                Cpf = x.Usuario.Cpf,
                Telefone = x.Usuario.PhoneNumber ?? string.Empty,
                Perfil = x.Perfil ?? PerfilUsuario,
                DataCriacao = x.Usuario.DataCriacao,
                DataExclusao = x.Usuario.DataExclusao
            })
            .ToListAsync();

        return usuarios;
    }

    public async Task<UsuarioDTO?> GetById(string id)
    {
        var usuario = await ObterUsuarioAtivoAsync(id);

        if (usuario == null)
            return null;

        var roles = await _userManager.GetRolesAsync(usuario);

        return MapToDTO(usuario, roles);
    }

    public async Task Add(CreateUsuarioDTO usuarioDto)
    {
        var cpfExistente = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Cpf == usuarioDto.Cpf);

        if (cpfExistente != null)
        {
            throw new InvalidOperationException(
                "O CPF informado pertence à outro usuário cadastrado no sistema.");
        }

        var emailExistente = await _userManager.FindByEmailAsync(
            usuarioDto.Email);

        if (emailExistente != null)
        {
            throw new InvalidOperationException(
                "O e-mail informado pertence à outro usuário cadastrado no sistema.");
        }

        var usuario = new ApplicationUser
        {
            UserName = usuarioDto.Email,
            Email = usuarioDto.Email,
            Nome = usuarioDto.Nome,
            Cpf = usuarioDto.Cpf,
            PhoneNumber = usuarioDto.Telefone,
            DataCriacao = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(
            usuario,
            usuarioDto.Senha);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                ObterErros(result));
        }

        var roleResult = await _userManager.AddToRoleAsync(
            usuario,
            PerfilUsuario);

        if (!roleResult.Succeeded)
        {
            throw new InvalidOperationException(
                ObterErros(roleResult));
        }
    }

    private async Task<bool> EhUltimoAdministradorAsync(
    ApplicationUser usuario)
    {
        var roles = await _userManager.GetRolesAsync(usuario);

        if (!roles.Contains(PerfilAdministrador))
        {
            return false;
        }

        var administradores =
            await _userManager.GetUsersInRoleAsync(PerfilAdministrador);

        var administradoresAtivos = administradores
            .Where(u => u.DataExclusao == null)
            .ToList();

        return administradoresAtivos.Count == 1 &&
               administradoresAtivos[0].Id == usuario.Id;
    }

    public async Task Update(string id, UpdateUsuarioDTO usuarioDto)
    {
        var usuario = await ObterUsuarioAtivoAsync(id);

        if (usuario == null)
        {
            throw new InvalidOperationException(
                "O usuário informado é inválido.");
        }

        var cpfExistente = await _userManager.Users
            .FirstOrDefaultAsync(u =>
                u.Cpf == usuarioDto.Cpf &&
                u.Id != id);

        if (cpfExistente != null)
        {
            throw new InvalidOperationException(
                "O CPF informado pertence à outro usuário cadastrado no sistema.");
        }

        var emailExistente = await _userManager.FindByEmailAsync(
            usuarioDto.Email);

        if (emailExistente != null &&
            emailExistente.Id != id)
        {
            throw new InvalidOperationException(
                "O e-mail informado pertence à outro usuário cadastrado no sistema.");
        }

        usuario.Nome = usuarioDto.Nome;
        usuario.Email = usuarioDto.Email;
        usuario.UserName = usuarioDto.Email;
        usuario.Cpf = usuarioDto.Cpf;
        usuario.PhoneNumber = usuarioDto.Telefone;

        var result = await _userManager.UpdateAsync(usuario);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                ObterErros(result));
        }
    }

    public async Task UpdatePerfil(string id, string novoPerfil)
    {
        var usuario = await ObterUsuarioAtivoAsync(id);

        if (usuario == null)
        {
            throw new InvalidOperationException(
                "Usuário não encontrado.");
        }

        var perfisPermitidos = new[]
        {
            PerfilUsuario,
            PerfilBibliotecario,
            PerfilAdministrador
        };

        if (!perfisPermitidos.Contains(novoPerfil))
        {
            throw new InvalidOperationException(
                "Perfil informado é inválido.");
        }

        var ehAdministrador = 
            (await _userManager.GetRolesAsync(usuario))
            .Contains(PerfilAdministrador);

        if (ehAdministrador &&
            novoPerfil != PerfilAdministrador &&
            await EhUltimoAdministradorAsync(usuario))
        {
            throw new InvalidOperationException(
                "Não é possível alterar o perfil, pois o usuário é o único administrador ativo do sistema.");
        }

        await AtualizarPerfilAsync(
            usuario,
            novoPerfil);
    }

    public async Task<UsuarioDTO?> Deactivate(string id)
    {
        var usuario = await ObterUsuarioAtivoAsync(id);

        if (usuario == null)
        {
            throw new InvalidOperationException(
                "Usuário inválido.");
        }

        if (await EhUltimoAdministradorAsync(usuario))
        {
            throw new InvalidOperationException(
                "Não é possível inativar o usuário, pois ele é o único administrador ativo do sistema.");
        }

        var possuiEmprestimoAtivo =
            await _emprestimoRepository.HasEmprestimoUsuarioAsync(
                id,
                null,
                StatusEmprestimo.Emprestado);

        if (possuiEmprestimoAtivo)
        {
            throw new InvalidOperationException(
                "Não é possível inativar o usuário, pois ele possui um empréstimo ativo.");
        }

        usuario.DataExclusao = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(usuario);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                ObterErros(result));
        }

        var roles = await _userManager.GetRolesAsync(usuario);

        return MapToDTO(usuario, roles);
    }

    private async Task AtualizarPerfilAsync(
        ApplicationUser usuario,
        string novoPerfil)
    {
        var rolesAtuais = await _userManager.GetRolesAsync(usuario);

        if (rolesAtuais.Contains(novoPerfil))
            return;

        if (rolesAtuais.Any())
        {
            var removeResult = await _userManager.RemoveFromRolesAsync(
                usuario,
                rolesAtuais);

            if (!removeResult.Succeeded)
            {
                throw new InvalidOperationException(
                    ObterErros(removeResult));
            }
        }

        var addResult = await _userManager.AddToRoleAsync(
            usuario,
            novoPerfil);

        if (!addResult.Succeeded)
        {
            throw new InvalidOperationException(
                ObterErros(addResult));
        }
    }

    private static UsuarioDTO MapToDTO(
        ApplicationUser usuario,
        IList<string> roles)
    {
        return new UsuarioDTO
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email ?? string.Empty,
            Cpf = usuario.Cpf,
            Telefone = usuario.PhoneNumber ?? string.Empty,
            Perfil = roles.FirstOrDefault() ?? PerfilUsuario,
            DataCriacao = usuario.DataCriacao,
            DataExclusao = usuario.DataExclusao
        };
    }

    private static string ObterErros(IdentityResult result)
    {
        return string.Join(
            "; ",
            result.Errors.Select(e => e.Description));
    }
}