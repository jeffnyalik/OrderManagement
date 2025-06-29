using AutoMapper;
using OrderManagement.Dtos;
using OrderManagement.Models;

namespace OrderManagement.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<CustomerCreateDto, Customer>();
            CreateMap<CustomerUpdateDto, Customer>();

        }
    }
}
