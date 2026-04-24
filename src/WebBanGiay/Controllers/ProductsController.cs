using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using WebBanGiay.Models;
using System.Net;

namespace WebBanGiay.Controllers
{
    public class ProductsController : Controller
    {
        // Khởi tạo DbContext
        private WebBanGiayDbContext db = new WebBanGiayDbContext();

        // 1. GET: Products (Trang danh sách sản phẩm)
        public ActionResult Index()
        {
            // Lấy toàn bộ sản phẩm cùng với thông tin Hãng, Ảnh và Biến thể
            var products = db.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariants)
                .ToList();

            return View(products);
        }

        // 2. GET: Products/Details/5 (Trang chi tiết một đôi giày)
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Truy vấn lấy duy nhất 1 sản phẩm theo ID, kèm theo tất cả dữ liệu liên quan
            var product = db.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariants)
                .SingleOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        // Giải phóng kết nối database khi request kết thúc
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