using MassTransit;
using MessageContracts;

namespace InvoiceMicroservice;

public sealed class EventConsumer : IConsumer<IInvoiceToCreate>
{
    public async Task Consume(ConsumeContext<IInvoiceToCreate> context)
    {
        var newInvoiceNumber = new Random().Next(10000, 99999);

        Console.WriteLine($"Creating invoice {newInvoiceNumber} for customer: {context.Message.CustomerNumber}");

        context.Message.InvoiceItems.ForEach(i =>
        {
            Console.WriteLine($"Items: Price: {i.Price}, Desc: {i.Description}");
            Console.WriteLine($"Actual distance in miles: {i.ActualMileage}, Base Rate: {i.BaseRate}");
            Console.WriteLine($"Oversized: {i.IsOversized}, Refrigerated: {i.IsRefrigerated}, Haz Mat: {i.IsHazardousMaterial}");
        });

        await context.Publish<IInvoiceCreated>(new
        {
            newInvoiceNumber = newInvoiceNumber,
            InvoiceData = new
            {
                context.Message.CustomerNumber,
                context.Message.InvoiceItems
            }
        });
    }
}
