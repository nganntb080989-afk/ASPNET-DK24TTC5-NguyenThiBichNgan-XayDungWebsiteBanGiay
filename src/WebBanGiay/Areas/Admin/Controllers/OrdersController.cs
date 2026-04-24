using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using WebBanGiay.Models;
using System.Net;

namespace WebBanGiay.Areas.Admin.Controllers
{
    [Authorize] // Chỉ Admin mới được vào
    public class OrdersController : Controller
    {
        private WebBanGiayDbContext db = new WebBanGiayDbContext();

        // 1. Danh sách đơn hàng (Sắp xếp đơn mới nhất lên đầu)
        public ActionResult Index()
        {
            var orders = db.Orders.OrderByDescending(o => o.OrderDate).ToList();
            return View(orders);
        }

        // 2. Chi tiết đơn hàng
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var order = db.Orders
                .Include(o => o.OrderDetails.Select(d => d.ProductVariant.Product))
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null) return HttpNotFound();
            return View(order);
        }

        // 3. Cập nhật trạng thái đơn hàng (Dùng POST để bảo mật)
        [HttpPost]
        public ActionResult UpdateStatus(int orderId, string status)
        {
            var order = db.Orders.Find(orderId);
            if (order != null)
            {
                order.Status = status;
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = orderId });
        }
    }
}