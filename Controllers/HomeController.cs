using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductsCategories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ProductsCategories.Controllers
{
    public class HomeController : Controller
    
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext=context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            

            return View();
        }

        [HttpGet("product/new")]
        public IActionResult AddProduct()
        {
            ViewBag.AllProducts=dbContext.Products.ToList();
            return View();
        }
        [HttpPost("create")]
        public IActionResult Create(Product newProduct)
        {
            if(ModelState.IsValid)
            {
                dbContext.Products.Add(newProduct);
                dbContext.SaveChanges();
                return Redirect("product/new");
            }
            else
            {
                ViewBag.AllProducts=dbContext.Products.ToList();
                return View("AddProduct");
            }
        }

        [HttpGet("categories")]
        public IActionResult Categories()
        {
            ViewBag.AllCategories=dbContext.Categories.ToList();
            return View();
        }
        [HttpPost("process")]
        public IActionResult Process(Category newCategory)
        {
            if(ModelState.IsValid)
            {
                dbContext.Categories.Add(newCategory);
                dbContext.SaveChanges();
                return Redirect("categories");
            }
            else
            {
                ViewBag.AllCategories=dbContext.Categories.ToList();
                return View("Categories");
            }
        }
        [HttpGet("product/{productId}")]
        public IActionResult DisplayProduct(int productId)
        {
            List<Product> AllProducts=dbContext.Products.Include(c=>c.Categories).ThenInclude(c=>c.NavProduct).ToList();
            
            ViewBag.AllCategories=dbContext.Categories.ToList();
            ViewBag.NotOnProduct = dbContext.Categories.Include( c => c.Products ).Where( c => c.Products.All( a => a.ProductId != productId )).ToList();
            Product PrId=dbContext.Products.Include(p=>p.Categories).ThenInclude(a=>a.NavCategory).FirstOrDefault(p=>p.ProductId==productId);

            ViewBag.productId = productId;
            return View(PrId);
        }
        [HttpPost("product/{productId}")]
        public IActionResult AddCategory(int productId, int categoryId)
        {
            Association categorized=new Association();
            categorized.ProductId=productId;
            categorized.CategoryId=categoryId;
            dbContext.Associations.Add(categorized);
            dbContext.SaveChanges();
            return Redirect($"/product/{productId}");

        }
        [HttpPost("category/{categoryId}")]
        public IActionResult AddProduct(int productId, int categoryId)
        {
            Association LoadProduct=new Association();
            LoadProduct.ProductId=productId;
            LoadProduct.CategoryId=categoryId;
            dbContext.Associations.Add(LoadProduct);
            dbContext.SaveChanges();
            return Redirect($"/category/{categoryId}");

        }
        [HttpGet("category/{categoryId}")]
        public IActionResult DisplayCategory(int categoryId)
        {
            List<Category> AllCategories=dbContext.Categories.Include(c=>c.Products).ThenInclude(c=>c.NavCategory).ToList();
            
            ViewBag.AllProducts=dbContext.Products.ToList();
            ViewBag.NotOnCategory = dbContext.Products.Include( c => c.Categories ).Where( c => c.Categories.All( a => a.CategoryId != categoryId)).ToList();
            Category CtrId=dbContext.Categories.Include(p=>p.Products).ThenInclude(a=>a.NavProduct).FirstOrDefault(p=>p.CategoryId==categoryId);

            ViewBag.categoryId = categoryId;
            return View(CtrId);
        }
        [HttpPost("remove/{associationId}")]
        public IActionResult Remove(int associationId)
        {
            Association ToBeRemoved = dbContext.Associations.FirstOrDefault(t => t.AssociationId == associationId);
            dbContext.Associations.Remove(ToBeRemoved);
            dbContext.SaveChanges();
            return Redirect($"/category/{ToBeRemoved.CategoryId}");
        }
        [HttpPost("delete/{associationId}")]
        public IActionResult Delete(int associationId)
        {
            Association ToBeRemoved = dbContext.Associations.FirstOrDefault(t => t.AssociationId == associationId);
            dbContext.Associations.Remove(ToBeRemoved);
            dbContext.SaveChanges();
            return Redirect($"/product/{ToBeRemoved.ProductId}");
        }
        


















        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
