namespace Core.Entities.OrderAggregate
{
    public class Address
    {
        public Address()
        {            
        }

        public Address(string firstName, string lastName, string street, string city, string state, string zipcode)
        {
            Zipcode = zipcode;
            State = state;
            City = city;
            Street = street;
            LastName = lastName;
            FirstName = firstName;
            
        }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
    }
}