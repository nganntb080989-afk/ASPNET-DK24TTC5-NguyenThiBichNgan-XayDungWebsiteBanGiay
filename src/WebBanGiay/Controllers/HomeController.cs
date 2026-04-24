using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using WebBanGiay.Models;

namespace WebBanGiay.Controllers
{
    public class HomeController : Controller
    {
        // Khởi tạo kết nối DB
        private WebBanGiayDbContext db = new WebBanGiayDbContext();

        public ActionResult Index()
        {
            // Lấy 8 sản phẩm mới nhất (sắp xếp ID giảm dần) kèm theo thông tin Ảnh, Giá, Thương hiệu
            var featuredProducts = db.Products
                .Include(p => p.Brand)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariants)
                .OrderByDescending(p => p.ProductId)
                .Take(8)
                .ToList();

            return View(featuredProducts);
        }

        // Đừng quên hàm giải phóng bộ nhớ
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}