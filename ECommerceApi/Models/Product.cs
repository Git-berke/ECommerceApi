using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApi.Models
{
       public class Product
       {

        [Key]
        public int Id { get; set; }


        [Required]
        [StringLength(100)]
        public string Name { get; set; }


        [StringLength(1000)]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int Stock { get; set; }
        public string ImageUrl { get; set; }

        //foreign key 
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        // Bir ürün birden fazla sepet öğesinde veya sipariş öğesinde bulunabilir.
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }



    }



}