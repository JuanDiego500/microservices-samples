using InvoiceMicroservice;
using MassTransit;

var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host("localhost");
    cfg.ReceiveEndpoint("invoice-service", e =>
    {
        e.UseInMemoryOutbox();
        e.Consumer<EventConsumer>(c =>
          c.UseMessageRetry(m => m.Interval(5, new TimeSpan(0, 0, 10))));
    });
});

var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));
await busControl.StartAsync(source.Token);

Console.WriteLine("Invoice Microservice Listening");

try
{
    while (true)
    {
        //listening for messages in while loop
        await Task.Delay(100);
    }
}
finally
{
    await busControl.StopAsync();
}
