using Microsoft.EntityFrameworkCore;
using SmartLibrary.Domain.Entities;
using SmartLibrary.Domain.Enums;
using SmartLibrary.Domain.Interfaces;
using SmartLibrary.Infra.Data.Context;

namespace SmartLibrary.Infra.Data.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    private readonly ApplicationDbContext _context;

    public UsuarioRepository(ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync(
        string? nome = null, 
        string? email = null, 
        string? cpf = null, 
        string? telefone = null,
        PerfilUsuario? perfil = null)
    {
        var query = _context.Usuarios
           .AsNoTracking()
           .AsQueryable();

        if (!string.IsNullOrWhiteSpace(nome))
            query = query.Where(x => x.Nome.Contains(nome));

        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(x => x.Email.Contains(email));

        if (!string.IsNullOrWhiteSpace(cpf))
            query = query.Where(x => x.Cpf.Contains(cpf));

        if (!string.IsNullOrWhiteSpace(telefone))
            query = query.Where(x => x.Telefone.Contains(telefone));

        if (perfil.HasValue)
            query = query.Where(x => x.Perfil == perfil.Value);

        return await query.ToListAsync();
    }

    public async Task<Usuario?> GetByCpfAsync(string cpf)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Cpf == cpf);
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }
}

