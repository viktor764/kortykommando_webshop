//using System;
//using System.Linq;
//using System.Web.Mvc;
//using PoharnokProject.Dnn.Dnn.PoharnokProject.Cocktail.Components;
//using PoharnokProject.Dnn.Dnn.PoharnokProject.Cocktail.Models;
//using DotNetNuke.Web.Mvc.Framework.Controllers;
//using DotNetNuke.Web.Mvc.Framework.ActionFilters;
//using DotNetNuke.Entities.Users;
//using DotNetNuke.Framework.JavaScriptLibraries;
//using DotNetNuke.Collections;
//using System.Collections.Generic;
//using Hotcakes.Commerce.Catalog;
//using Hotcakes.Commerce;

//namespace PoharnokProject.Dnn.Dnn.PoharnokProject.Cocktail.Controllers
//{
//    [DnnHandleError]
//    public class ItemController : DnnController
//    {
//        public ActionResult Delete(int itemId)
//        {
//            ItemManager.Instance.DeleteItem(itemId, ModuleContext.ModuleId);
//            return RedirectToDefaultRoute();
//        }

//        public ActionResult Edit(int itemId = -1)
//        {
//            DotNetNuke.Framework.JavaScriptLibraries.JavaScript.RequestRegistration(CommonJs.DnnPlugins);
//            var userlist = UserController.GetUsers(PortalSettings.PortalId);
//            var users = from user in userlist.Cast<UserInfo>().ToList()
//                        select new SelectListItem { Text = user.DisplayName, Value = user.UserID.ToString() };
//            ViewBag.Users = users;
//            var item = (itemId == -1)
//                 ? new Item { ModuleId = ModuleContext.ModuleId }
//                 : ItemManager.Instance.GetItem(itemId, ModuleContext.ModuleId);
//            return View(item);
//        }

//        [HttpPost]
//        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
//        public ActionResult Edit(Item item)
//        {
//            if (item.ItemId == -1)
//            {
//                item.CreatedByUserId = User.UserID;
//                item.CreatedOnDate = DateTime.UtcNow;
//                item.LastModifiedByUserId = User.UserID;
//                item.LastModifiedOnDate = DateTime.UtcNow;
//                ItemManager.Instance.CreateItem(item);
//            }
//            else
//            {
//                var existingItem = ItemManager.Instance.GetItem(item.ItemId, item.ModuleId);
//                existingItem.LastModifiedByUserId = User.UserID;
//                existingItem.LastModifiedOnDate = DateTime.UtcNow;
//                existingItem.ItemName = item.ItemName;
//                existingItem.ItemDescription = item.ItemDescription;
//                existingItem.AssignedUserId = item.AssignedUserId;
//                ItemManager.Instance.UpdateItem(existingItem);
//            }
//            return RedirectToDefaultRoute();
//        }

//        [ModuleAction(ControlKey = "Edit", TitleKey = "AddItem")]
//        public ActionResult Index()
//        {
//            DotNetNuke.Framework.JavaScriptLibraries.JavaScript.RequestRegistration(CommonJs.DnnPlugins);
//            var hccApp = new Hotcakes.Commerce.HotcakesApplication(Hotcakes.Commerce.HccRequestContext.Current);
//            var model = new List<IngredientCategoryViewModel>();

//            for (int i = 1; i <= 5; i++)
//            {
//                var catId = ModuleContext.Configuration.ModuleSettings.GetValueOrDefault("Cocktail_Cat" + i, "NONE");
//                if (!string.IsNullOrEmpty(catId) && catId != "NONE")
//                {
//                    var category = hccApp.CatalogServices.Categories.Find(catId);
//                    if (category != null)
//                    {
//                        var crossRefs = hccApp.CatalogServices.CategoriesXProducts.FindForCategory(catId, 1, 100);
//                        var products = new List<Product>();
//                        if (crossRefs != null)
//                        {
//                            foreach (var xref in crossRefs)
//                            {
//                                var p = hccApp.CatalogServices.Products.Find(xref.ProductId);
//                                if (p != null) products.Add(p);
//                            }
//                        }
//                        if (products.Any())
//                        {
//                            model.Add(new IngredientCategoryViewModel
//                            {
//                                CategoryName = category.Name,
//                                Products = products
//                            });
//                        }
//                    }
//                }
//            }
//            return View(model);
//        }

//        [HttpPost]
//        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
//        public void AddToCart(string ids) // <-- Rövid név: ids
//        {
//            object resultData;
//            try
//            {
//                // Ha a DNN elhagyta volna a paramétert, kikérjük kézzel is
//                if (string.IsNullOrEmpty(ids)) ids = Request.Params["ids"];

//                if (string.IsNullOrEmpty(ids))
//                {
//                    resultData = new { success = false, message = "A szerver nem kapott ID-kat." };
//                }
//                else
//                {
//                    var hccContext = Hotcakes.Commerce.HccRequestContext.Current;
//                    var hccApp = new Hotcakes.Commerce.HotcakesApplication(hccContext);
//                    var cart = hccApp.OrderServices.CurrentShoppingCart();

//                    var productIds = ids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

//                    foreach (var bvin in productIds)
//                    {
//                        var product = hccApp.CatalogServices.Products.Find(bvin);
//                        if (product != null)
//                        {
//                            var li = new Hotcakes.Commerce.Orders.LineItem
//                            {
//                                ProductId = product.Bvin,
//                                Quantity = 1,
//                                BasePricePerItem = product.SitePrice,
//                                AdjustedPricePerItem = product.SitePrice
//                            };
//                            hccApp.OrderServices.AddItemToOrder(cart, li);
//                        }
//                    }
//                    hccApp.OrderServices.Orders.Update(cart);
//                    resultData = new { success = true };
//                }
//            }
//            catch (Exception ex)
//            {
//                resultData = new { success = false, message = ex.Message };
//            }

//            // --- MEGÁLLÍTJUK A DNN-T, HOGY NE TEGYEN HTML-T A VÉGÉRE ---
//            var response = System.Web.HttpContext.Current.Response;
//            response.Clear();
//            response.ContentType = "application/json; charset=utf-8";
//            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
//            response.Write(serializer.Serialize(resultData));
//            response.Flush();
//            response.End(); // Ez a legfontosabb sor!
//        }
//    }
//}

﻿/*
' Copyright (c) 2026 KortyKommando
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

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
            try
            {
                var hccApp = HotcakesApplication.Current;
                var cart = hccApp.OrderServices.CurrentShoppingCart();

                if (productIds == null || !productIds.Any())
                {
                    return Json(new { success = false, message = "Nincs kiválasztott termék." });
                }
                // Minden kiválasztott elemet hozzáadunk a kosárhoz
                // Minden kiválasztott elemet hozzáadunk a kosárhoz
                foreach (var bvin in productIds)
                {
                    var product = hccApp.CatalogServices.Products.Find(bvin);
                    if (product != null)
                    {
                        // 1. Lépés: A tétel teljes körű manuális létrehozása
                        var li = new Hotcakes.Commerce.Orders.LineItem
                        {
                            ProductId = product.Bvin,
                            ProductName = product.ProductName,
                            Quantity = 1,

                            // AZ ÁRAZÁS PONTOSÍTÁSA:
                            BasePricePerItem = product.SitePrice,       // Eredeti alapár
                            AdjustedPricePerItem = product.SitePrice,   // Megjelenített (kedvezményes) ár
                            LineTotal = product.SitePrice               // Tétel teljes értéke (mivel 1 db van)
                        };

                        // 2. Lépés: Hozzáadás a kosárhoz
                        hccApp.OrderServices.AddItemToOrder(cart, li);
                    }
                }

                // Kosár frissítése a Hotcakes-ben 
                hccApp.OrderServices.Orders.Update(cart);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Hibaállapot kezelése a terv szerint
                return Json(new { success = false, message = "Szerver hiba: " + ex.Message });
            }
        }

    }
}