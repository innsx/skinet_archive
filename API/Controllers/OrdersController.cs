using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Infrastructure.services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _mapper = mapper;
            _orderService = orderService;            
        }


        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] OrderDto orderDto)
        {
            var buyerEmail = HttpContext.User.RetrieveEmailFromClaimPrincipal();

            var validatedShipToAddress = _mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);

            var createOrder = await _orderService.CreateOrderAsync(buyerEmail, orderDto.DeliveryMethodId, orderDto.BasketId, validatedShipToAddress);

            if (createOrder == null) 
            {
                return BadRequest(new ApiResponse(400, "Problem with creating an Order"));
            }

            return Ok(createOrder);
        }

        
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderDto>>> GetOrdersForUser()
        {
            var email = HttpContext.User.RetrieveEmailFromClaimPrincipal();

            var listOfOrders = await _orderService.GetOrdersForUserAsync(email);

            var mappedWithOderDto = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(listOfOrders);

            return Ok(mappedWithOderDto);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var email = HttpContext.User.RetrieveEmailFromClaimPrincipal();

            var order = await _orderService.GetOrderByIdAsync(id, email);

            if (order == null)
            {
                return NotFound(new ApiResponse(404, "Get Order By Id Not Found"));
            }

            var mappedOrderWithOrderToReturnDtoFormat = _mapper.Map<Order, OrderToReturnDto>(order);

            return Ok(mappedOrderWithOrderToReturnDtoFormat);
        }

        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var listOfDeliveryMethods = await _orderService.GetDeliveryMethodsAsync();

            return Ok(listOfDeliveryMethods);
        }

    }
}