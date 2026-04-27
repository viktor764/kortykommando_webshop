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
            // 1. Hotcakes inicializálása
            var hccApp = new Hotcakes.Commerce.HotcakesApplication(Hotcakes.Commerce.HccRequestContext.Current);
            var model = new List<IngredientCategoryViewModel>();

            for (int i = 1; i <= 5; i++)
            {
                // Kiolvassuk a mentett kategória ID-t
                var catId = ModuleContext.Configuration.ModuleSettings.GetValueOrDefault("Cocktail_Cat" + i, "");

                if (!string.IsNullOrEmpty(catId))
                {
                    var category = hccApp.CatalogServices.Categories.Find(catId);
                    if (category != null)
                    {
                        int totalCount = 0;

                        // 1. Lépés: Kategória-Termék összerendelések lekérése (A "B-Terv" varázslata)
                        var crossRefs = hccApp.CatalogServices.CategoriesXProducts.FindForCategory(catId, 1, 100);

                        var products = new List<Product>();

                        if (crossRefs != null)
                        {
                            // 2. Lépés: Végigmegyünk az összerendeléseken, és betöltjük az igazi termékeket
                            foreach (var xref in crossRefs)
                            {
                                var p = hccApp.CatalogServices.Products.Find(xref.ProductId);

                                // Csak a készleten lévő, elérhető termékeket adjuk hozzá
                                if (p != null)
                                {
                                    products.Add(p);
                                }
                            }
                        }

                        // Ha van legalább egy elérhető termék, hozzáadjuk a képernyőre küldendő csomaghoz
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

        [HttpPost]
        public JsonResult AddToCart(List<string> productIds)
        {
            var debug = new List<string>();

            try
            {
                debug.Add("AddToCart elindult.");

                var hccApp = new HotcakesApplication(HccRequestContext.Current);
                var cart = hccApp.OrderServices.EnsureShoppingCart();

                if (cart == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Az EnsureShoppingCart() után is null a cart.",
                        debug = debug
                    });
                }

                debug.Add("Cart bvin: " + cart.bvin);
                debug.Add("Cart items before: " + cart.Items.Count);

                if (productIds == null || !productIds.Any())
                {
                    return Json(new
                    {
                        success = false,
                        message = "Nincs kiválasztott termék.",
                        debug = debug
                    });
                }

                debug.Add("Kapott productIds count: " + productIds.Count);

                foreach (var bvin in productIds)
                {
                    debug.Add("Keresett product bvin: " + bvin);

                    var product = hccApp.CatalogServices.Products.Find(bvin);

                    if (product == null)
                    {
                        debug.Add("NEM található termék: " + bvin);
                        continue;
                    }

                    debug.Add("Talált termék: " + product.ProductName + " | SKU: " + product.Sku + " | Price: " + product.SitePrice);

                    var li = new Hotcakes.Commerce.Orders.LineItem
                    {
                        ProductId = product.Bvin,
                        ProductName = product.ProductName,
                        ProductShortDescription = product.ShortDescription,
                        ProductSku = product.Sku,
                        Quantity = 1,
                        BasePricePerItem = product.SitePrice,
                        AdjustedPricePerItem = product.SitePrice,
                        LineTotal = product.SitePrice
                    };

                    hccApp.OrderServices.AddItemToOrder(cart, li);

                    debug.Add("AddItemToOrder meghívva: " + product.ProductName);
                }

                debug.Add("Cart items after AddItemToOrder before Update: " + cart.Items.Count);

                hccApp.OrderServices.Orders.Update(cart);

                var cartAfter = hccApp.OrderServices.CurrentShoppingCart();

                debug.Add("Cart items after Update: " + (cartAfter == null ? -1 : cartAfter.Items.Count));

                return Json(new
                {
                    success = true,
                    message = "Diagnosztikai futás kész.",
                    cartBvin = cart.bvin,
                    itemCountBefore = cart.Items.Count,
                    itemCountAfter = cartAfter == null ? -1 : cartAfter.Items.Count,
                    debug = debug
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.ToString(),
                    debug = debug
                });
            }
        }

    }
}