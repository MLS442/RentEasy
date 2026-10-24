using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentEasyAPI.Data;
using RentEasyAPI.Models;
using RentEasyAPI.Responses;
using RentEasyAPI.Services;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Claims;

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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketResponse>>> GetTickets()
        {
            var role = User.FindFirstValue(ClaimTypes.Role);
            int? tenantId = null;
            
            if(role == "Tenant")
            {
                tenantId = int.Parse(User.FindFirstValue("TenantId"));
            }

            var ticktets = await _ticketService.GetTickets(role, tenantId);

            return Ok(ticktets);

            //var ticktets = await _ticketService.GetTickets();
            //return Ok(ticktets);
        }

       [Authorize(Roles = "Tenant")]
       [HttpPost]

        public async Task<ActionResult<TicketResponse>> PostTicket(Ticket newTicket)
        {
            newTicket.TenantId = int.Parse(User.FindFirstValue("TenantId"));

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
