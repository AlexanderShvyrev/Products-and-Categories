using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace ProductsCategories.Models
{
    public class Product
    {
        [Key]
        public int ProductId {get; set;}
        [Required]
        public string PName{get; set;}
        [Required]
        public string PDescription{get; set;}
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price{get; set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;

        public DateTime UpdatedAt {get;set;} = DateTime.Now;

        public List<Association> Categories{get; set;}
    }
}