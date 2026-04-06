using AutoMapper;
using SignalR.DtoLayer.BookingDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Mapping
{
    public class DiscountMapping : Profile
    {
        public DiscountMapping()
        {
            CreateMap<Discount, ResultBookingDto>().ReverseMap();
            CreateMap<Discount, CreateBookingDto>().ReverseMap();
            CreateMap<Discount, UpdateBookingDto>().ReverseMap();
            CreateMap<Discount, GetBookingDto>().ReverseMap();
        }   
    }
}
