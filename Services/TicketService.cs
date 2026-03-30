using Microsoft.EntityFrameworkCore;
using RentEasyAPI.Data;
using RentEasyAPI.Models;

namespace RentEasyAPI.Services
{
    public class TicketService : ITicketService
    {
        private readonly RentEasyContext _context;

        public TicketService(RentEasyContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetTickets()
        {
            return await _context.Tickets
                   .Include(t => t.Tenant)
                   .ToListAsync();
        }
 
        public async Task<Ticket> PostTicket(Ticket newTicket)
        {
            _context.Add(newTicket);
            await _context.SaveChangesAsync();

            return newTicket;
        }

        public async Task<bool> PutTicket(int id, Ticket updatedTicket)
        {
            _context.Entry(updatedTicket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return false;
            }
            else
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(t => t.TicketId == id);
        }

    }
}
