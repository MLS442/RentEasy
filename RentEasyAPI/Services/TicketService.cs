using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using RentEasyAPI.Data;
using RentEasyAPI.Models;
using RentEasyAPI.Responses;

namespace RentEasyAPI.Services
{
    public class TicketService : ITicketService
    {
        private readonly RentEasyContext _context;
        private readonly IMapper _mapper;

        public TicketService(RentEasyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TicketResponse>> GetTickets(string role, int? tenantId)
        {
            var query = _context.Tickets.AsQueryable();

            if (role != "Landlord")
            {
                query = query.Where(t => t.TenantId == tenantId);
            }

            // ProjectTo automatically handles the Include/mapping at the database level
            return await query
                .ProjectTo<TicketResponse>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<TicketResponse> PostTicket(Ticket newTicket)
        {
            _context.Add(newTicket);
            await _context.SaveChangesAsync();

            return await _context.Tickets.
                Where(t => t.TicketId == newTicket.TicketId)
                .ProjectTo<TicketResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> PutTicket(int id, Ticket updatedTicket)
        {
            var existingTicket = await _context.Tickets.FindAsync(id);

            if (existingTicket == null) 
            { 
                return false;
            }

            existingTicket.Status = updatedTicket.Status;

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
