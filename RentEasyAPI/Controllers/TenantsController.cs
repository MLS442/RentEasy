using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentEasyAPI.Data;
using RentEasyAPI.Models;
using System.Runtime.Intrinsics.X86;

namespace RentEasyAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TenantsController : ControllerBase
    {
        private readonly RentEasyContext _context;

        public TenantsController(RentEasyContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Tenant>>> GetTenants()
        {
            return await _context.Tenants
                .ToListAsync();
        }

        [HttpPost]

        public async Task<ActionResult<Tenant>> PostTenant(Tenant newTenant)
        {
            _context.Add(newTenant);
            await _context.SaveChangesAsync();

            return newTenant;
        }
    }
}

