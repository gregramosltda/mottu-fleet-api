using Microsoft.EntityFrameworkCore;
using Mottu.Fleet.Api.Data;
using Mottu.Fleet.Api.Models;

namespace Mottu.Fleet.Api.Repositories;

public class MotoRepository : IMotoRepository
{
    private readonly AppDbContext _context;

    public MotoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Moto>> GetAllAsync()
    {
        return await _context.Motos.ToListAsync();
    }

    public async Task<Moto?> GetByIdAsync(int id)
    {
        return await _context.Motos.FindAsync(id);
    }

    public async Task<Moto> AddAsync(Moto moto)
    {
        _context.Motos.Add(moto);
        await _context.SaveChangesAsync();
        return moto;
    }

    public async Task<Moto> UpdateAsync(Moto moto)
    {
        _context.Motos.Update(moto);
        await _context.SaveChangesAsync();
        return moto;
    }
}
