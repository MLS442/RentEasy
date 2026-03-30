using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentEasyAPI.Data;
using RentEasyAPI.Models;
using RentEasyAPI.Services;
using System.Collections;
using System.Runtime.InteropServices;

namespace RentEasyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            var ticktets = await _ticketService.GetTickets();
            return Ok(ticktets);
        }

       [HttpPost]

        public async Task<ActionResult<Ticket>> PostTicket(Ticket newTicket)
        {
            var createdTicket = await _ticketService.PostTicket(newTicket);
            return Ok(createdTicket);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> PutTicket(int id, Ticket updatedTicket)
        {
            if (updatedTicket.TicketId != id)
            {
                return BadRequest();
            }

            bool success = await _ticketService.PutTicket(id, updatedTicket);

            if (success)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteTicket(int id)
        {
            var success = await _ticketService.DeleteTicket(id);

            if(success)
            {
                return NoContent();
            }

            return NotFound();
            
        }
    }
}
