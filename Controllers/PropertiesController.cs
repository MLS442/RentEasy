using Microsoft.AspNetCore.Mvc;
using RentEasyAPI.Models;

namespace RentEasyAPI.Controllers
{
    [ApiController]
    [Route ("[controller]")]
    public class PropertiesController : ControllerBase
    {
        [HttpGet]

        public ActionResult<List<Property>> GetProperties()
        {
            var list = new List<Property>
            {
                new Property {Id = 1, Address = "Anywhere", Price = 1400,  Bedrooms = 2, LeaseEndDate = new DateOnly(2034, 04, 04)},
                new Property {Id = 2, Address = "123 Maple St", Price = 1200, Bedrooms = 1, LeaseEndDate = new DateOnly(2025, 05, 12)},
                new Property {Id = 3, Address = "456 Oak Ave", Price = 2500, Bedrooms = 3, LeaseEndDate = new DateOnly(2026, 08, 23)},
                new Property {Id = 4, Address = "789 Pine Rd", Price = 1800, Bedrooms = 2, LeaseEndDate = new DateOnly(2025, 11, 30)},
                new Property {Id = 5, Address = "101 Amazon Way", Price = 3200, Bedrooms = 4, LeaseEndDate = new DateOnly(2027, 01, 15)},
                new Property {Id = 6, Address = "202 Birch Ln", Price = 1100, Bedrooms = 1, LeaseEndDate = new DateOnly(2025, 03, 22)},
                new Property {Id = 7, Address = "303 Cedar Blvd", Price = 1650, Bedrooms = 2, LeaseEndDate = new DateOnly(2026, 07, 09)},
                new Property {Id = 8, Address = "404 Elm St", Price = 2100, Bedrooms = 3, LeaseEndDate = new DateOnly(2025, 12, 14)},
                new Property {Id = 9, Address = "505 Willow Dr", Price = 1350, Bedrooms = 1, LeaseEndDate = new DateOnly(2026, 06, 18)},
                new Property {Id = 10, Address = "606 Spruce Ct", Price = 2800, Bedrooms = 3, LeaseEndDate = new DateOnly(2027, 09, 05)},
                new Property {Id = 11, Address = "707 Aspen Pl", Price = 1950, Bedrooms = 2, LeaseEndDate = new DateOnly(2025, 02, 27)},
                new Property {Id = 12, Address = "808 Cherry Cir", Price = 2200, Bedrooms = 3, LeaseEndDate = new DateOnly(2026, 07, 06)},
                new Property {Id = 13, Address = "909 Twin Peaks", Price = 1500, Bedrooms = 2, LeaseEndDate = new DateOnly(2026, 10, 10)},
                new Property {Id = 14, Address = "1725 Slough Ave", Price = 1300, Bedrooms = 1, LeaseEndDate = new DateOnly(2025, 03, 15)},
                new Property {Id = 15, Address = "111 Jazz Way", Price = 3500, Bedrooms = 4, LeaseEndDate = new DateOnly(2028, 02, 21)}
            };

            return Ok(list);
        }
    }
}
