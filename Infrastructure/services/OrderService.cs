using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRespo;  
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRespo, IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
            _basketRespo = basketRespo;            
        }
        
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            // get the basket item(s) from basketRepo and check the order price and quantity and compared the price & quantity from database
            var basket = await _basketRespo.GetBasketAsync(basketId);

            // get the items from the ProductRepo
            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);

                items.Add(orderItem);
            }
            
            // get the delivery method from DeliveryMethod Repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // calculate subtotal
            var subtotal = items.Sum(item => item.Price * item.Quantity);

            // check to see if order exists
            var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
            var existingOrders = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if (existingOrders != null) 
            {
                _unitOfWork.Repository<Order>().Delete(existingOrders);
                await _paymentService.CreateOrderUpdatePaymentIntent(basket.PaymentIntentId);
            }

            // create order
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, subtotal, items, basket.PaymentIntentId);
            
            _unitOfWork.Repository<Order>().Add(order);

            // save the order to database
            var result = await _unitOfWork.Complete();

            if (result <= 0)
            {
                return null;
            }


            // return the order
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var listOfDeliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();

            return listOfDeliveryMethods;
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {

            var spec = new OrderWithItemsAndOrderingSpecification(id, buyerEmail);

            var orderById = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            return orderById;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderWithItemsAndOrderingSpecification(buyerEmail);

            var listOfUserOrders = await _unitOfWork.Repository<Order>().ListAsync(spec);

            return listOfUserOrders;
        }
    }
}
