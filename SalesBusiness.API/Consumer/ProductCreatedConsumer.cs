using MassTransit;
using SalesBusiness.API.Data;
using SalesBusiness.API.Data.Entities;
using Shared.RabbitMqModels;

namespace SalesBusiness.API.Consumer
{
    public class ProductCreatedConsumer : IConsumer<ProductCreated>
    {
        private readonly SalesBusinessContext _salesBusinessContext;

        public ProductCreatedConsumer(SalesBusinessContext salesBusinessContext)
        {
            _salesBusinessContext = salesBusinessContext;
        }
        public async Task Consume(ConsumeContext<ProductCreated> context)
        {
            var newProduct = new Products
            {
                Id = context.Message.Id,
                Name = context.Message.Name
            };
            _salesBusinessContext.Add(newProduct);
            await _salesBusinessContext.SaveChangesAsync();
        }
    }
}
