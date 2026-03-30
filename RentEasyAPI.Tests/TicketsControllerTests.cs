using Microsoft.AspNetCore.Mvc;
using Moq;
using RentEasyAPI.Controllers;
using RentEasyAPI.Models;
using RentEasyAPI.Services;

namespace RentEasyAPI.Tests;

public class TicketsControllerTests
{
    [Fact]
    public async Task GetTickets_ShouldReturn_OkWithTickets()
    {
        // Arrange
        var mockTicketService = new Mock<ITicketService>();

        var mocktTickets = new List<Ticket>
        {
            new Ticket {TenantId = 1, TicketId = 1, Subject = "Kithen", Description = "The sink has a leak", Status = "Not Fixed"},
            new Ticket { TenantId = 1, TicketId = 3, Subject = "Lighting", Description = "Bedroom ceiling light flickering", Status = "In Progress" },
            new Ticket { TenantId = 2, TicketId = 4, Subject = "Front Door", Description = "Key sticks in the deadbolt", Status = "Fixed" },
            new Ticket { TenantId = 2, TicketId = 5, Subject = "Bathroom Tile", Description = "Loose tile near the shower base", Status = "Not Fixed" },
            new Ticket { TenantId = 3, TicketId = 6, Subject = "Heating", Description = "Radiator making knocking sounds", Status = "Fixed" }
        };

        mockTicketService.Setup(s => s.GetTickets()).ReturnsAsync(mocktTickets);

        var controller = new TicketsController(mockTicketService.Object);

        // Act
        var result = await controller.GetTickets();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);

        Assert.Equal(200, okResult.StatusCode);

        var data = Assert.IsType<List<Ticket>>(okResult.Value);

        Assert.Equal(5, data.Count);

        Assert.Equal(1, data[0].TicketId);
    }
}
