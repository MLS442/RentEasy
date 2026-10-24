using AutoMapper;
using RentEasyAPI.Models;
using RentEasyAPI.Responses;

namespace RentEasyAPI.Mappings
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, TicketResponse>();
        }
    }
}
