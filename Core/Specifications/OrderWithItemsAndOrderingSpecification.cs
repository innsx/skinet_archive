using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrderWithItemsAndOrderingSpecification : BaseSpecification<Order>
    {
        // CONSTRUCTOR #1: get ALL orders by userEmail
        public OrderWithItemsAndOrderingSpecification(string userEmail) : base(o => o.BuyerEmail == userEmail) // using base to Specifiy the criteria
        {
            // EAGER LOADING OrderItems & DeliveryMethod entities
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);

            // SORT the Order Entity BY ORDERDATE IN DESCENDING order
            AddOrderByDescending(o => o.OrderDate);

        }

        // CONSTRUCTOR #2: using base to Specifiy the criteria with OrderId & user Email
        public OrderWithItemsAndOrderingSpecification(int id, string userEmail) : base(o => o.Id == id && o.BuyerEmail == userEmail)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}