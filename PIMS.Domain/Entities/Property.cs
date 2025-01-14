using System.Runtime.CompilerServices;

namespace PIMS.Domain.Entities
{
    public  class Property
    {
        public Property(string name, string address, decimal price, DateTime dateTime)
        {
            Id = Guid.NewGuid();
            Name = name;
            Address = address;
            Price = price;
            CreatedDate = dateTime;

        }

        public Property() { }

        public Guid Id { get; set; }  
        public string Name { get; set; } = string.Empty;
        public Guid? OwnerId { get; set; } 
        public string Address { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public Contact? Contact { get; set; }
    }
}
