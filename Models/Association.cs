using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace ProductsCategories.Models
{
    public class Association
    {
        [Key]
        public int AssociationId{get; set;}
        public int ProductId{get; set;}
        public int CategoryId{get; set;}
        public Product NavProduct{get; set;}
        public Category NavCategory{get; set;}

    }
}