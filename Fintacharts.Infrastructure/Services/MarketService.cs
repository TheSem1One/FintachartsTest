using Fintacharts.Application.Common.Interfaces;
using Fintacharts.Application.DTO.Assets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
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

        var request = $@"{{
      ""type"": ""l1-subscription"",
      ""id"": ""{id}"",
      ""instrumentId"": ""ad9e5345-4c3b-41fc-9437-1d253f62db52"",
      ""provider"": ""simulation"",
      ""subscribe"": true,
      ""kinds"": [""bid""]
    }}";

        var bytesToSend = Encoding.UTF8.GetBytes(request);
        await ws.SendAsync(new ArraySegment<byte>(bytesToSend), WebSocketMessageType.Text, true, CancellationToken.None);

        var buffer = new byte[1024 * 4];
        List<string> messages = new();

        for (int i = 0; i < 4; i++)
        {
            var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            messages.Add(message);
        }
        var lastMessage = messages.LastOrDefault();
        var price = JsonSerializer.Deserialize<AssetPriceDto>(lastMessage, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        return price; 
        
 

    }
}
