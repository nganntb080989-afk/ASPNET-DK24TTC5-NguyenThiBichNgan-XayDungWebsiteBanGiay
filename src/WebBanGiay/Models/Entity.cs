using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanGiay.Models
{
    public class AdminUser
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(255)]
        public string Password { get; set; }

        public string Role { get; set; } // Ví dụ: "Admin", "Manager"
    }
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [StringLength(100)]
        public string CategoryName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }

    public class Brand
    {
        [Key]
        public int BrandId { get; set; }

        [Required]
        [StringLength(100)]
        public string BrandName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }

    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200)]
        public string ProductName { get; set; }

        public string Description { get; set; }

        // Khóa ngoại
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

        // Navigation properties
        public virtual ICollection<ProductVariant> ProductVariants { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }

    // Bảng này dùng để quản lý Kho hàng & Giá bán thực tế
    public class ProductVariant
    {
        [Key]
        public int VariantId { get; set; }

        public int Size { get; set; } // Ví dụ: 39, 40, 41...

        [StringLength(50)]
        public string Color { get; set; } // Ví dụ: Đen, Trắng...

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; } // Số lượng tồn kho cho size/màu này

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }

    // Bảng lưu trữ nhiều ảnh cho 1 sản phẩm
    public class ProductImage
    {
        [Key]
        public int ImageId { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public bool IsDefault { get; set; } // Đánh dấu ảnh nào là ảnh bìa chính

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }

    public class CartItem
    {
        public int VariantId { get; set; } // Khóa chính để xác định chính xác đôi giày (kèm size/màu)
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }
        public int Size { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // Thuộc tính tính toán tổng tiền của món này
        public decimal TotalPrice => Price * Quantity;
    }

    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên người nhận")]
        [StringLength(100)]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [StringLength(20)]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [StringLength(500)]
        public string CustomerAddress { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        // Trạng thái đơn hàng: Chờ xử lý, Đang giao, Hoàn thành, Đã hủy...
        public string Status { get; set; }

        // Navigation property
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }

    public class OrderDetail
    {
        [Key]
        public int DetailId { get; set; }

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }

        public int VariantId { get; set; }
        [ForeignKey("VariantId")]
        public virtual ProductVariant ProductVariant { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}