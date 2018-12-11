using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CheeseMVC.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
           
            return View(context.Categories.ToList());
        }

        public IActionResult Add()
        {
            AddCategoryViewModel addCategoryViewModel = new AddCategoryViewModel();

            return View(addCategoryViewModel);

        }

        [HttpPost]
        public IActionResult Add(AddCategoryViewModel addCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                // Add the new cheese to my existing cheeses
                CheeseCategory newCategory = new CheeseCategory
                {
                    Name = addCategoryViewModel.Name,

                };

                context.Categories.Add(newCategory);
                context.SaveChanges();

                return Redirect("/Category");

            }

            return View(addCategoryViewModel);
        }

        private readonly CheeseDbContext context;

        public CategoryController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }
    }
}