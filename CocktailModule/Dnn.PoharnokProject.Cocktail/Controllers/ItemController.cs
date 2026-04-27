using System;
using System.Linq;
using System.Web.Mvc;
using PoharnokProject.Dnn.Dnn.PoharnokProject.Cocktail.Components;
using PoharnokProject.Dnn.Dnn.PoharnokProject.Cocktail.Models;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Collections;
using System.Collections.Generic;
using Hotcakes.Commerce.Catalog;
using Hotcakes.Commerce;

namespace PoharnokProject.Dnn.Dnn.PoharnokProject.Cocktail.Controllers
{
    [DnnHandleError]
    public class ItemController : DnnController
    {
        public ActionResult Delete(int itemId)
        {
            ItemManager.Instance.DeleteItem(itemId, ModuleContext.ModuleId);
            return RedirectToDefaultRoute();
        }

        public ActionResult Edit(int itemId = -1)
        {
            DotNetNuke.Framework.JavaScriptLibraries.JavaScript.RequestRegistration(CommonJs.DnnPlugins);
            var userlist = UserController.GetUsers(PortalSettings.PortalId);
            var users = from user in userlist.Cast<UserInfo>().ToList()
                        select new SelectListItem { Text = user.DisplayName, Value = user.UserID.ToString() };
            ViewBag.Users = users;
            var item = (itemId == -1)
                 ? new Item { ModuleId = ModuleContext.ModuleId }
                 : ItemManager.Instance.GetItem(itemId, ModuleContext.ModuleId);
            return View(item);
        }

        [HttpPost]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            if (item.ItemId == -1)
            {
                item.CreatedByUserId = User.UserID;
                item.CreatedOnDate = DateTime.UtcNow;
                item.LastModifiedByUserId = User.UserID;
                item.LastModifiedOnDate = DateTime.UtcNow;
                ItemManager.Instance.CreateItem(item);
            }
            else
            {
                var existingItem = ItemManager.Instance.GetItem(item.ItemId, item.ModuleId);
                existingItem.LastModifiedByUserId = User.UserID;
                existingItem.LastModifiedOnDate = DateTime.UtcNow;
                existingItem.ItemName = item.ItemName;
                existingItem.ItemDescription = item.ItemDescription;
                existingItem.AssignedUserId = item.AssignedUserId;
                ItemManager.Instance.UpdateItem(existingItem);
            }
            return RedirectToDefaultRoute();
        }

        [ModuleAction(ControlKey = "Edit", TitleKey = "AddItem")]
        public ActionResult Index()
        {
            DotNetNuke.Framework.JavaScriptLibraries.JavaScript.RequestRegistration(CommonJs.DnnPlugins);
            var hccApp = new Hotcakes.Commerce.HotcakesApplication(Hotcakes.Commerce.HccRequestContext.Current);
            var model = new List<IngredientCategoryViewModel>();

            for (int i = 1; i <= 5; i++)
            {
                var catId = ModuleContext.Configuration.ModuleSettings.GetValueOrDefault("Cocktail_Cat" + i, "NONE");
                if (!string.IsNullOrEmpty(catId) && catId != "NONE")
                {
                    var category = hccApp.CatalogServices.Categories.Find(catId);
                    if (category != null)
                    {
                        var crossRefs = hccApp.CatalogServices.CategoriesXProducts.FindForCategory(catId, 1, 100);
                        var products = new List<Product>();
                        if (crossRefs != null)
                        {
                            foreach (var xref in crossRefs)
                            {
                                var p = hccApp.CatalogServices.Products.Find(xref.ProductId);
                                if (p != null) products.Add(p);
                            }
                        }
                        if (products.Any())
                        {
                            model.Add(new IngredientCategoryViewModel
                            {
                                CategoryName = category.Name,
                                Products = products
                            });
                        }
                    }
                }
            }
            return View(model);
        }

        // A leglazább, legsimább form feldolgozó.
        [HttpPost]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult AddToCart() // Nincs paraméter, nem bízzuk a DNN-re
        {
            // 1. Direktben kinyerjük az adatot a beküldött űrlapból
            string productIdsString = Request.Form["productIdsString"];

            if (string.IsNullOrEmpty(productIdsString))
            {
                // Ha valamiért üres, csak simán visszatöltjük az oldalt hiba nélkül
                return RedirectToDefaultRoute();
            }

            var hccContext = Hotcakes.Commerce.HccRequestContext.Current;
            var hccApp = new Hotcakes.Commerce.HotcakesApplication(hccContext);
            var cart = hccApp.OrderServices.CurrentShoppingCart();

            var productIds = productIdsString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var bvin in productIds)
            {
                var product = hccApp.CatalogServices.Products.Find(bvin);
                if (product != null)
                {
                    var li = new Hotcakes.Commerce.Orders.LineItem
                    {
                        ProductId = product.Bvin,
                        Quantity = 1,
                        BasePricePerItem = product.SitePrice,
                        AdjustedPricePerItem = product.SitePrice
                    };
                    hccApp.OrderServices.AddItemToOrder(cart, li);
                }
            }
            hccApp.OrderServices.Orders.Update(cart);

            // 2. Sima, tiszta átirányítás a kosárra! Nincs JSON, nincs HTML szemetelés!
            return Redirect("/HotcakesStore/Cart");
        }
    }
}