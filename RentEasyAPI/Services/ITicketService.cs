using RentEasyAPI.Models;

namespace RentEasyAPI.Services
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetTickets();
        Task<Ticket> PostTicket(Ticket newTicket);
        Task<bool> PutTicket(int id, Ticket updatedTicket);
        Task<bool> DeleteTicket(int id);
    }
}
