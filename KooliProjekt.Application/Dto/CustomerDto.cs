using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    // NB! PasswordHash ei tohi kunagi API kaudu välja minna
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IList<AddressDto> Addresses { get; set; }
        public IList<CustomerOrderDto> Orders { get; set; }

        public CustomerDto()
        {
            Addresses = new List<AddressDto>();
            Orders = new List<CustomerOrderDto>();
        }
    }
}
