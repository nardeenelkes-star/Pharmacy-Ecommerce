
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy.DAL.Data;
using System.Security.Claims;

namespace Pharmacy.Web.Controllers
{
    public class DriverController : Controller
    {
        private readonly PharmacyDbContext _context;

        public DriverController(PharmacyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Orders()
        {
            // Get the signed-in driver's ID from their claims
            var driverIdValue =
                User.FindFirst("DriverId")?.Value ??
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(driverIdValue, out var driverId))
            {
                return Unauthorized();
            }

            // Get only orders assigned to this driver
            var orders = await _context.DriverOrders
                .AsNoTracking()
                .Where(o => o.DriverId == driverId)
                .OrderByDescending(o => o.OrderId)
                .ToListAsync();

            return View(orders);
        }
    }
}
