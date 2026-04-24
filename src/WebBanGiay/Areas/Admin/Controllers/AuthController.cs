using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebBanGiay.Models;

namespace WebBanGiay.Areas.Admin.Controllers
{
    // Cho phép tất cả mọi người truy cập trang này để đăng nhập
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private WebBanGiayDbContext db = new WebBanGiayDbContext();

        // GET: Hiển thị form đăng nhập
        public ActionResult Login()
        {
            return View();
        }

        // POST: Xử lý khi bấm nút Đăng nhập
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tài khoản trong database
                var user = db.AdminUsers.SingleOrDefault(u => u.Username == username && u.Password == password);

                if (user != null)
                {
                    // Phát thẻ "qua cửa" (Cookie) cho trình duyệt
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    return RedirectToAction("Index", "Products"); // Đăng nhập xong nhảy vào trang quản lý Sản phẩm
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View();
        }

        // Xử lý Đăng xuất
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Auth");
        }
    }
}