using Mottu.Fleet.Api.Models;

namespace Mottu.Fleet.Api.Repositories;

public interface IMotoRepository
{
    Task<IEnumerable<Moto>> GetAllAsync();
    Task<Moto?> GetByIdAsync(int id);
    Task<Moto> AddAsync(Moto moto);
    Task<Moto> UpdateAsync(Moto moto);
}
