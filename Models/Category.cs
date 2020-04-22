using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;




namespace ProductsCategories.Models
{
    public class Category
    {
        [Key]
        public int CategoryId{get; set;}
        [Required]
        public string CName{get; set;}

        public List<Association> Products{get; set;}
    }
}