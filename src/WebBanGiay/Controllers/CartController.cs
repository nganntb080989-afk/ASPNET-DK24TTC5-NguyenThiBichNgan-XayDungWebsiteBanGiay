using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebBanGiay.Models;
using System.Data.Entity;

namespace WebBanGiay.Controllers
{
    public class CartController : Controller
    {
        private WebBanGiayDbContext db = new WebBanGiayDbContext();

        // 1. Hàm phụ trợ: Lấy giỏ hàng từ Session
        private List<CartItem> GetCart()
        {
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null)
            {
                cart = new List<CartItem>();
                Session["Cart"] = cart;
            }
            return cart;
        }

        // 2. GET: Hiển thị giỏ hàng
        public ActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        // 3. POST: Thêm sản phẩm vào giỏ (được gọi từ nút "Thêm vào giỏ" ở trang Details)
        [HttpPost]
        public ActionResult Add(int variantId, int quantity)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.VariantId == variantId);

            if (item != null)
            {
                // Nếu đã có trong giỏ -> tăng số lượng
                item.Quantity += quantity;
            }
            else
            {
                // Truy vấn DB để lấy thông tin (Giá, Tên, Size...) dựa vào VariantId
                // KHÔNG LẤY GIÁ TỪ HTML ĐỂ BẢO MẬT
                var variant = db.ProductVariants
                                .Include(v => v.Product)
                                .Include(v => v.Product.ProductImages)
                                .SingleOrDefault(v => v.VariantId == variantId);

                if (variant != null)
                {
                    // Tìm ảnh mặc định
                    var mainImage = variant.Product.ProductImages.FirstOrDefault(i => i.IsDefault)?.ImageUrl
                                    ?? "/Images/default-shoe.png";

                    cart.Add(new CartItem
                    {
                        VariantId = variant.VariantId,
                        ProductName = variant.Product.ProductName,
                        ImageUrl = mainImage,
                        Size = variant.Size,
                        Color = variant.Color,
                        Price = variant.Price,
                        Quantity = quantity
                    });
                }
            }

            // Quay về trang xem giỏ hàng
            return RedirectToAction("Index");
        }

        // 4. GET: Xóa 1 món khỏi giỏ
        public ActionResult Remove(int variantId)
        {
            var cart = GetCart();
            var item = cart.SingleOrDefault(c => c.VariantId == variantId);
            if (item != null)
            {
                cart.Remove(item);
            }
            return RedirectToAction("Index");
        }
    }
}