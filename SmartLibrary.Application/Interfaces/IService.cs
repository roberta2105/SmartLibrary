namespace SmartLibrary.Application.Interfaces;

public interface IService<TEntityDTO, TCreateDTO, TUpdateDTO>
{
    Task<TEntityDTO> GetById(int id);
    Task Add(TCreateDTO dto);
    Task Update(TUpdateDTO dto);
}
