using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;            
        }

        [HttpGet]
        public async Task<ActionResult> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            if (basket == null)
            {
                return NotFound();
            }

            var newCustomerBasketId = new CustomerBasket(id);

            return Ok(newCustomerBasketId);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket([FromBody] CustomerBasket customerBasket)
        {
            var updatedBasket = await _basketRepository.UpdateBasketAsync(customerBasket);

            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasketAsync(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);
        }
    }
}