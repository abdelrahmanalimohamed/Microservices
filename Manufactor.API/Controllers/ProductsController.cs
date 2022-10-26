using Manufactor.API.Data;
using Manufactor.API.Data.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MassTransit;
using Shared.RabbitMqModels;

namespace Manufactor.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ManufactureContext _manufactureContext;

        private readonly IPublishEndpoint _publishEndpoint;

        public ProductsController(ManufactureContext manufactureContext , IPublishEndpoint publishEndpoint)
        {
            _manufactureContext = manufactureContext;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var products = await _manufactureContext.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var product = await _manufactureContext.Products.FindAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Products newProduct)
        {
            _manufactureContext.Products.Add(newProduct);
            await _manufactureContext.SaveChangesAsync();
            await _publishEndpoint.Publish<ProductCreated>(new ProductCreated
            {
                Id = newProduct.Id,
                Name = newProduct.Name
            });

            return CreatedAtAction("Get", new { id = newProduct.Id }, newProduct);
        }
    }
}
