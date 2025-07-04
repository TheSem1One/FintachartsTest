namespace Fintacharts.Application.DTO.Market
{
    public class MarketRequestDto
    {

        public string Type { get; set; } = "l1-subscription";
        public string Id { get; set; }
        public string InstrumentId { get; set; }
        public string Provider { get; set; } = "simulation";
        public bool Subscribe { get; set; } = true;
        public List<string> Kinds { get; set; }
    }
}
