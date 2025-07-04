using Fintacharts.Application.Common.Interfaces;
using Fintacharts.Application.DTO.Assets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Fintacharts.Application.DTO.Market;
using Fintacharts.Infrastructure.Options;
using Fintacharts.Infrastructure.Persistence;

namespace Fintacharts.Infrastructure.Services;
public class MarketService(IServiceScopeFactory scopeFactory,
    IOptions<FintaOptions> options, DatabaseContext dbContext) : IMarket
{
    private readonly IOptions<FintaOptions> _options = options;
    public async Task<AssetPriceDto> GetPrice(int id)
    {
        var wss = _options.Value.Wss;
        var token = _options.Value.Token;
        using var ws = new ClientWebSocket();
        var uri = new Uri($"{wss}{token}");

        await ws.ConnectAsync(uri, CancellationToken.None);

        var obj = new MarketRequestDto()
        {
            Id = id.ToString(),
            InstrumentId = "ad9e5345-4c3b-41fc-9437-1d253f62db52",
            Kinds = new List<string> { "bid" }
        };
        var request1 = JsonSerializer.Serialize(obj, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });
        var bytesToSend = Encoding.UTF8.GetBytes(request1);
        await ws.SendAsync(new ArraySegment<byte>(bytesToSend), WebSocketMessageType.Text, true, CancellationToken.None);

        var buffer = new byte[1024 * 4];
        string? lastMessage = null;

        for (int i = 0; i < 4; i++)
        {
            var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            lastMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
        }
        var price = JsonSerializer.Deserialize<AssetPriceDto>(lastMessage, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return price;
    }
}
