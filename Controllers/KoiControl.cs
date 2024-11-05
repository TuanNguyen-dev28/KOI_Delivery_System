using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KoiShippingApp.Controllers
{
    public class KoiControl : Controller
    {
        private readonly KoiDeliveryContext_context;

        public KoiControl(KoiDeliveryContext_context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(koiList);
        }
        public async Task<IActionResult> Index()
        {
            var koiList = await _context.Kois.ToListAsync();
            return View(koiList);
        }
        public IActionResult Order(int id)
        {
            var koi = _context.Kois.Find(id);
            if (koi == null)
            {
                return NotFound();
            }
            return View(koi);
        }
        [HttpPost]
        public async Task<IActionResult> Order(int koiId, Customer customer)
        {
            if (ModelState.IsValid)
            {
                var koi = await _context.Kois.FindAsync(koiId);
                if (koi == null)
                {
                    return NotFound();
                }

                // Lưu khách hàng và đơn hàng
                _context.Customers.Add(customer);
                var order = new ShippingOrder
                {
                    Koi = koi,
                    Customer = customer,
                    OrderDate = DateTime.Now
                };

                _context.ShippingOrders.Add(order);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"Đặt hàng thành công cá Koi: {koi.Name}";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}