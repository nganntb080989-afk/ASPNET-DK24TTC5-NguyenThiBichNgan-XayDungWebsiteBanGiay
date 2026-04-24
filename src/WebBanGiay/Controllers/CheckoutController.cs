using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebBanGiay.Models;

namespace WebBanGiay.Controllers
{
    public class CheckoutController : Controller
    {
        private WebBanGiayDbContext db = new WebBanGiayDbContext();

        // 1. GET: Hiển thị form nhập thông tin giao hàng
        public ActionResult Index()
        {
            var cart = Session["Cart"] as List<CartItem>;
            // Nếu giỏ hàng trống, đuổi về trang sản phẩm
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Products");
            }

            return View(cart); // Truyền giỏ hàng sang View để hiển thị tóm tắt
        }

        // 2. POST: Xử lý lưu đơn hàng khi khách bấm xác nhận
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessOrder(Order orderInfo)
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Products");
            }

            if (ModelState.IsValid)
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Lưu thông tin Order chung
                        orderInfo.OrderDate = DateTime.Now;
                        orderInfo.TotalAmount = cart.Sum(c => c.TotalPrice);
                        orderInfo.Status = "Chờ xử lý";

                        db.Orders.Add(orderInfo);
                        db.SaveChanges(); // Lưu để lấy ra OrderId mới sinh

                        // 2. Lưu từng OrderDetail & Trừ tồn kho
                        foreach (var item in cart)
                        {
                            var orderDetail = new OrderDetail
                            {
                                OrderId = orderInfo.OrderId,
                                VariantId = item.VariantId,
                                Quantity = item.Quantity,
                                UnitPrice = item.Price
                            };
                            db.OrderDetails.Add(orderDetail);

                            // TRỪ TỒN KHO TRONG DB
                            var variantInfo = db.ProductVariants.Find(item.VariantId);
                            if (variantInfo != null)
                            {
                                variantInfo.StockQuantity -= item.Quantity;
                            }
                        }

                        db.SaveChanges();
                        transaction.Commit(); // Xác nhận giao dịch thành công

                        // 3. Xóa giỏ hàng khỏi Session
                        Session["Cart"] = null;

                        // 4. Chuyển hướng tới trang Thành công
                        return RedirectToAction("Success", new { orderId = orderInfo.OrderId });
                    }
                    catch (Exception)
                    {
                        transaction.Rollback(); // Nếu có lỗi (ví dụ rớt mạng), hoàn tác toàn bộ dữ liệu
                        ModelState.AddModelError("", "Có lỗi xảy ra trong quá trình đặt hàng. Vui lòng thử lại.");
                    }
                }
            }

            // Nếu dữ liệu nhập bị thiếu, trả lại trang Index kèm thông báo lỗi
            return View("Index", cart);
        }

        // 3. GET: Trang báo đặt hàng thành công
        public ActionResult Success(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
    }
}