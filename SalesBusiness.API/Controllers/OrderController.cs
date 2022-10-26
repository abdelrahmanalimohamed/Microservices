using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesBusiness.API.Data;
using SalesBusiness.API.Data.Entities;
using SalesBusiness.API.DTO;


namespace SalesBusiness.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly SalesBusinessContext _salesBusinessContext;

        public OrderController(SalesBusinessContext salesBusinessContext)
        {
            _salesBusinessContext = salesBusinessContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            // var orders = await _salesBusinessContext.Orders.ToListAsync();

            var orders = await _salesBusinessContext.Orders
    .Join(
        _salesBusinessContext.Products,
        order => order.ProductId,
        product => product.Id,
        (order, product) => new { Order = order, Product = product }
            )
    .Select(_ => new OrderDTO
    {
        Id = _.Order.Id,
        OrderDate = _.Order.OrderDate,
        UserId = _.Order.UserId,
        ProductInfo = new ProductDto
        {
            Id = _.Product.Id,
            Name = _.Product.Name
        }
    }).ToListAsync();
            return Ok(orders);
          
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var order = await _salesBusinessContext.Orders.FindAsync(id);
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Orders newOrder)
        {
            _salesBusinessContext.Orders.Add(newOrder);
            await _salesBusinessContext.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = newOrder.Id }, newOrder);
        }


    }
}
