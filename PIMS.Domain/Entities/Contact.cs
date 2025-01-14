using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PIMS.Domain.Entities
{
    public class Contact
    {
        public Contact(Guid id, string firstName, string lastName, string phoneNumber, string emailAddress)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            PropertyContacts = new List<PriceOfAcquisition>();
        }

        public Contact()
        {
            PropertyContacts = new List<PriceOfAcquisition>(); 
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public ICollection<PriceOfAcquisition> PropertyContacts { get; set; }
    }
    
}
