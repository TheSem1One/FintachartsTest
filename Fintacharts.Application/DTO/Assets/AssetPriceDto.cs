using System.Security.Cryptography;

namespace Fintacharts.Application.DTO.Assets
{
    public class AssetPriceDto
    {
        public string Type { get; set; }
        public string InstrumentId { get; set; }
        public string Provider { get; set; }
        public Bid Bid { get; set; }
    }
    public class Bid
    {
        public DateTime Timestamp { get; set; }
        public double Price { get; set; }
        public double Volume { get; set; }
    }
}
