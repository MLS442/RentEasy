using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentEasyAPI.Data;
using RentEasyAPI.Models;

namespace RentEasyAPI.Controllers
{
    [ApiController]
    [Route ("[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly RentEasyContext _context;

        public PropertiesController(RentEasyContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Property>>> GetProperties()
        {
            return await _context.Properties
                        .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Property>> PostProperty(Property newProperty)
        {
            _context.Add(newProperty);
            await _context.SaveChangesAsync();

            return newProperty;
        }
    }
}
