using DotNetNuke.Web.Mvc.Framework.Controllers;
using System.Web.Mvc;
using DotNetNuke.Security;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using System.Linq;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Collections;
using System;

namespace PoharnokProject.Dnn.Dnn.PoharnokProject.Cocktail.Controllers
{
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
    [DnnHandleError]
    public class SettingsController : DnnController
    {
        [HttpGet]
        public ActionResult Settings()
        {
            var hccApp = new Hotcakes.Commerce.HotcakesApplication(Hotcakes.Commerce.HccRequestContext.Current);
            var allCategories = hccApp.CatalogServices.Categories.FindAll()
                                      .Select(c => new SelectListItem
                                      {
                                          Text = c.Name,
                                          Value = c.Bvin
                                      }).ToList();

            // AZ ÜRES OPCIÓ ÉRTÉKE MOSTANTÓL "-1"
            allCategories.Insert(0, new SelectListItem { Text = "-- Válassz kategóriát --", Value = "-1" });
            ViewBag.Categories = allCategories;

            // Betöltéskor is a "-1" az alapértelmezett
            ViewBag.Cat1 = ModuleContext.Configuration.TabModuleSettings.GetValueOrDefault("Cocktail_Cat1", "-1");
            ViewBag.Cat3 = ModuleContext.Configuration.TabModuleSettings.GetValueOrDefault("Cocktail_Cat3", "-1");
            ViewBag.Cat4 = ModuleContext.Configuration.TabModuleSettings.GetValueOrDefault("Cocktail_Cat4", "-1");
            ViewBag.Cat5 = ModuleContext.Configuration.TabModuleSettings.GetValueOrDefault("Cocktail_Cat5", "-1");
            ViewBag.Cat2 = ModuleContext.Configuration.TabModuleSettings.GetValueOrDefault("Cocktail_Cat2", "-1");

            return View();
        }

        [HttpPost]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Settings(FormCollection collection)
        {

            // 1. LÉPÉS: Ellenőrizzük, hogy egyáltalán jött-e adat
            if (collection == null) return RedirectToDefaultRoute();

            var mc = ModuleController.Instance;

            for (int i = 1; i <= 5; i++)
            {
                // Keressük a sima "cat1", "cat2"... neveket
                string value = collection["cat" + i];
                string key = "Cocktail_Cat" + i;

                // Ha üres vagy "-1", akkor töröljük a beállítást
                if (string.IsNullOrWhiteSpace(value) || value == "-1")
                {
                    mc.DeleteTabModuleSetting(ModuleContext.TabModuleId, key);
                }
                else
                {
                    mc.UpdateTabModuleSetting(ModuleContext.TabModuleId, key, value);
                }
            }


            // 2. LÉPÉS: Kényszerített gyorstár ürítés
            DotNetNuke.Common.Utilities.DataCache.ClearModuleCache(ModuleContext.TabId);

            return RedirectToDefaultRoute();
        }
    }
}