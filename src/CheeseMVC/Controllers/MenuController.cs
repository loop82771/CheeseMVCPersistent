using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CheeseMVC.Models;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }


        public IActionResult Index()
        {

            return View(context.Menus.ToList());

        }

        [HttpGet]
        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();

            return View(addMenuViewModel);

        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                // Add the new cheese to my existing cheeses
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name,

                };

                context.Menus.Add(newMenu);
                context.SaveChanges();

                return Redirect("/Menu/ViewMenu/" + newMenu.ID);

            };

            return View(addMenuViewModel);
        }

        [HttpGet]
        public IActionResult ViewMenu(int id)
        {
            


            List<CheeseMenu> items = context
            .CheeseMenus
            .Include(item => item.Cheese)
            .Where(cm => cm.MenuID == id)
            .ToList();

            Menu menu = context.Menus.Single(c => c.ID == id);

            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel
            {
                Menu = menu,
                Items = items

            };

            return View(viewMenuViewModel);

        }

        [HttpGet]
        public IActionResult AddItem(int id)

        { 
            Menu menu = context.Menus.Single(c => c.ID == id);

            List<Cheese> cheeses = context.Cheeses.ToList();
        
            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(menu, cheeses);

           return View(addMenuItemViewModel);

        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (ModelState.IsValid)
            {

                var cheese = addMenuItemViewModel.cheeseID;
                var menu = addMenuItemViewModel.menuID;

                IList<CheeseMenu> existingItems = context.CheeseMenus
                .Where(cm => cm.CheeseID == cheese)
                .Where(cm => cm.MenuID == menu).ToList();

                if (existingItems.Count == 0) {

                    CheeseMenu menuItem = new CheeseMenu
                    {

                        Cheese = context.Cheeses.Single(c=> c.ID == cheese),
                        Menu = context.Menus.Single(c => c.ID == menu),

                    };

                context.CheeseMenus.Add(menuItem);
                    context.SaveChanges();

                }


                return Redirect("/Menu/ViewMenu/" + menu);

            }

            return View(addMenuItemViewModel);
        }
    }
}