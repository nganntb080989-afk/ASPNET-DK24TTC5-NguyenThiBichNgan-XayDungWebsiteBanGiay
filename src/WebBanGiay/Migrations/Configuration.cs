namespace WebBanGiay.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebBanGiay.Models; // Import thư mục Models của bạn

    internal sealed class Configuration : DbMigrationsConfiguration<WebBanGiay.Models.WebBanGiayDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebBanGiay.Models.WebBanGiayDbContext context)
        {
            // Kiểm tra nếu bảng Category trống thì mới chạy Seed để tránh lặp dữ liệu
            if (!context.Categories.Any())
            {
                // 1. Tạo Danh mục
                var catSneaker = new Category { CategoryName = "Giày Thể Thao (Sneaker)" };
                var catRunning = new Category { CategoryName = "Giày Chạy Bộ (Running)" };
                context.Categories.AddRange(new[] { catSneaker, catRunning });

                // 2. Tạo Thương hiệu
                var brandNike = new Brand { BrandName = "Nike" };
                var brandAdidas = new Brand { BrandName = "Adidas" };
                context.Brands.AddRange(new[] { brandNike, brandAdidas });

                // 3. Tạo Sản phẩm chung
                var prodAF1 = new Product
                {
                    ProductName = "Nike Air Force 1 '07",
                    Description = "Mẫu giày huyền thoại với thiết kế cổ điển, màu trắng tinh tế dễ phối đồ.",
                    Category = catSneaker,
                    Brand = brandNike
                };

                var prodUltra = new Product
                {
                    ProductName = "Adidas Ultraboost 22",
                    Description = "Giày chạy bộ đỉnh cao với đế Boost hoàn trả năng lượng tuyệt vời.",
                    Category = catRunning,
                    Brand = brandAdidas
                };
                context.Products.AddRange(new[] { prodAF1, prodUltra });

                // 4. Tạo Biến thể (Kho & Giá)
                var var1 = new ProductVariant { Size = 39, Color = "Trắng", Price = 2600000, StockQuantity = 15, Product = prodAF1 };
                var var2 = new ProductVariant { Size = 40, Color = "Trắng", Price = 2600000, StockQuantity = 20, Product = prodAF1 };
                var var3 = new ProductVariant { Size = 41, Color = "Đen/Trắng", Price = 2500000, StockQuantity = 5, Product = prodAF1 };

                var var4 = new ProductVariant { Size = 40, Color = "Đen", Price = 3200000, StockQuantity = 12, Product = prodUltra };
                var var5 = new ProductVariant { Size = 42, Color = "Trắng", Price = 3500000, StockQuantity = 0, Product = prodUltra }; // Hết hàng
                context.ProductVariants.AddRange(new[] { var1, var2, var3, var4, var5 });

                // 5. Tạo Hình ảnh (Tôi dùng link ảnh online mẫu để giao diện bạn có ảnh hiển thị luôn)
                var img1 = new ProductImage { ImageUrl = "https://images.unsplash.com/photo-1595950653106-6c9ebd614d3a?auto=format&fit=crop&w=500&q=80", IsDefault = true, Product = prodAF1 };
                var img2 = new ProductImage { ImageUrl = "https://images.unsplash.com/photo-1600185365483-26d7a4cc7519?auto=format&fit=crop&w=500&q=80", IsDefault = false, Product = prodAF1 };

                var img3 = new ProductImage { ImageUrl = "https://images.unsplash.com/photo-1608231387042-66d1773070a5?auto=format&fit=crop&w=500&q=80", IsDefault = true, Product = prodUltra };
                context.ProductImages.AddRange(new[] { img1, img2, img3 });

                context.AdminUsers.AddOrUpdate(u => u.Username,
                    new AdminUser
                    {
                        Username = "admin",
                        Password = "123456", // Trong thực tế nên mã hóa mật khẩu
                        Role = "Admin"
                    }
                );

                // 6. Lưu toàn bộ xuống DB
                context.SaveChanges();
            }
        }
    }
}