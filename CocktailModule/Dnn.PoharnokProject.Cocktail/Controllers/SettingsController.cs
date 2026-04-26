/*
' Copyright (c) 2026 KortyKommando
'  All rights reserved.
*/

using DotNetNuke.Web.Mvc.Framework.Controllers;
using DotNetNuke.Collections;
using System.Web.Mvc;
using DotNetNuke.Security;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using System;
using System.Linq;
using DotNetNuke.Entities.Modules;
using Hotcakes.Commerce;
using Hotcakes.Commerce.Dnn.Utils; // Ez kell a Hotcakes App eléréséhez

namespace PoharnokProject.Dnn.Dnn.PoharnokProject.Cocktail.Controllers
{
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
    [DnnHandleError]
    public class SettingsController : DnnController
    {
        [HttpGet]
        public ActionResult Settings()
        {
            // 1. Hotcakes inicializálása
            var hccApp = new Hotcakes.Commerce.HotcakesApplication(Hotcakes.Commerce.HccRequestContext.Current);

            // 2. Kategóriák lekérése a Hotcakes-ből
            var allCategories = hccApp.CatalogServices.Categories.FindAll()
                                      .Select(c => new SelectListItem
                                      {
                                          Text = c.Name,
                                          Value = c.Bvin // A Bvin a Hotcakes egyedi azonosítója
                                      }).ToList();

            // Üres opció hozzáadása az elejére
            allCategories.Insert(0, new SelectListItem { Text = "-- Válassz kategóriát --", Value = "" });

            // Átadjuk a listát a nézetnek
            ViewBag.Categories = allCategories;

            // 3. Korábban elmentett beállítások betöltése a DNN-ből
            ViewBag.Cat1 = ModuleContext.Configuration.ModuleSettings.GetValueOrDefault("Cocktail_Cat1", "");
            ViewBag.Cat2 = ModuleContext.Configuration.ModuleSettings.GetValueOrDefault("Cocktail_Cat2", "");
            ViewBag.Cat3 = ModuleContext.Configuration.ModuleSettings.GetValueOrDefault("Cocktail_Cat3", "");
            ViewBag.Cat4 = ModuleContext.Configuration.ModuleSettings.GetValueOrDefault("Cocktail_Cat4", "");
            ViewBag.Cat5 = ModuleContext.Configuration.ModuleSettings.GetValueOrDefault("Cocktail_Cat5", "");

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Settings(string cat1, string cat2, string cat3, string cat4, string cat5)
        {
            // 4. A kiválasztott Kategória ID-k (Bvin) mentése a DNN ModuleSettings-be
            ModuleController.Instance.UpdateModuleSetting(ModuleContext.ModuleId, "Cocktail_Cat1", cat1);
            ModuleController.Instance.UpdateModuleSetting(ModuleContext.ModuleId, "Cocktail_Cat2", cat2);
            ModuleController.Instance.UpdateModuleSetting(ModuleContext.ModuleId, "Cocktail_Cat3", cat3);
            ModuleController.Instance.UpdateModuleSetting(ModuleContext.ModuleId, "Cocktail_Cat4", cat4);
            ModuleController.Instance.UpdateModuleSetting(ModuleContext.ModuleId, "Cocktail_Cat5", cat5);

            return RedirectToDefaultRoute();
        }
    }
}