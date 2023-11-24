using Consumer.DAL;
using Consumer.DAL.Entities;
using DataContracts.Events;
using MassTransit;

namespace Consumer.Consumers;

public class GetResponseByUrlEventConsumer : IConsumer<IGetResponseByUrlEvent>
{
    public GetResponseByUrlEventConsumer(IHttpClientFactory httpClientFactory, DataContext context)
    {
        _httpClientFactory = httpClientFactory;
        _context = context;
    }
    
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly DataContext _context;

    // Можно оптимизировать с помощью Batch, уменьшится как нагрузка на память, так и нагрузка на БД
    public async Task Consume(ConsumeContext<IGetResponseByUrlEvent> context)
    {
        var url = context.Message.Url;
        using var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(url, context.CancellationToken);
        _context.Responses.Add(new ResponseLog()
        {
            Date = DateTime.UtcNow,
            Url = url,
            Response = await response.Content.ReadAsStringAsync(context.CancellationToken)
        });
        await _context.SaveChangesAsync(context.CancellationToken);
    }
}