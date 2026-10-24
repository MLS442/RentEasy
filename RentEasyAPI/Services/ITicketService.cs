using RentEasyAPI.Models;
using RentEasyAPI.Responses;

namespace RentEasyAPI.Services
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketResponse>> GetTickets(string role, int? tenantId);
        Task<TicketResponse> PostTicket(Ticket newTicket);
        Task<bool> PutTicket(int id, Ticket updatedTicket);
        Task<bool> DeleteTicket(int id);
    }
}
