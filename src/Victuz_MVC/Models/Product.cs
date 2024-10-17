﻿using System.ComponentModel.DataAnnotations;

namespace Victuz_MVC.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public List<ProductCategoryLine>? ProductCategoryLines { get; set; }
    }
}
