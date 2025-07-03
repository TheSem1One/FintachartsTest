using Fintacharts.Application.DTO.Assets;
using Fintacharts.Domain.Entity;

namespace Fintacharts.Application.Common.Interfaces
{
    public interface IForex
    {
        Task<IEnumerable<Asset>> GetAll();
        Task<bool> AddAsset(AssetDto dto);
        Task<bool> DeleteAsset(int id);
    }
}
