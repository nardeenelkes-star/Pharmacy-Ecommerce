
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
            var driverIdValue =
                User.FindFirst("DriverId")?.Value ??
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(driverIdValue, out var driverId))
            {
                return Unauthorized();
            }

            var orders = await _context.DriverOrders
                .AsNoTracking()
                .Where(o => o.DriverId == driverId)
                .OrderByDescending(o => o.OrderId)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Details(long id)
        {
            var driverIdValue =
                User.FindFirst("DriverId")?.Value ??
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(driverIdValue, out var driverId))
            {
                return Unauthorized();
            }

            var order = await _context.DriverOrders
                .AsNoTracking()
                .FirstOrDefaultAsync(o =>
                    o.OrderId == id && o.DriverId == driverId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
