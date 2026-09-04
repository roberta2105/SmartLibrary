using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartLibrary.Application.Interfaces;
using SmartLibrary.Application.Mappings;
using SmartLibrary.Application.Services;
using SmartLibrary.Domain.Interfaces;
using SmartLibrary.Infra.Data;
using SmartLibrary.Infra.Data.Context;
using SmartLibrary.Infra.Data.Repositories;

namespace SmartLibrary.Infra.Ioc;

public static class DependencyInjectionAPI
{
    public static IServiceCollection AddInfrastructureAPI(this IServiceCollection services,
       IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"
          ), b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

       

        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<ILivroRepository, LivroRepository>();
        services.AddScoped<IEmprestimoRepository, EmprestimoRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ICategoriaService, CategoriaService>();
        services.AddScoped<ILivroService, LivroService>();
        services.AddScoped<IEmprestimoService, EmprestimoService>();
        services.AddScoped<IUsuarioService, UsuarioService>();

        //services.AddScoped<IAuthenticate, AuthenticateService>();

        services.AddAutoMapper(cfg => { }, typeof(DomainToDTOMappingProfile).Assembly);

        return services;
    }
}
