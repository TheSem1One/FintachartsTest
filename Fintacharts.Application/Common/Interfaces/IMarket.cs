using Fintacharts.Application.DTO.Assets;

namespace Fintacharts.Application.Common.Interfaces
{
    public interface IMarket
    {
        Task<AssetPriceDto> GetPrice(int id);
    }
}
