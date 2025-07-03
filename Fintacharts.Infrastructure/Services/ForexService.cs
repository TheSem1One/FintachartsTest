using Fintacharts.Application.Common.Interfaces;
using Fintacharts.Application.DTO.Assets;
using Fintacharts.Domain.Entity;
using Fintacharts.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Fintacharts.Infrastructure.Services
{
    public class ForexService(DatabaseContext dbContext) : IForex
    {
        private readonly DatabaseContext _dbContext = dbContext;
        public async Task<IEnumerable<Asset>> GetAll()
        {
            return await _dbContext.Assets.ToListAsync();
        }

        public async Task<bool> AddAsset(AssetDto dto)
        {
            var asset = new Asset
            {
                Symbol = dto.Symbol
            };
            await _dbContext.Assets.AddAsync(asset);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsset(int id)
        {
            var asset = await _dbContext.Assets.FindAsync(id);
            _dbContext.Assets.Remove(asset);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
