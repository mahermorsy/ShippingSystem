using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Helpers;

namespace UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class CountriesController : Controller
    {
        private readonly ICountries _ICountry;
        public CountriesController(ICountries ICountry)
        {
            _ICountry = ICountry;
        }
        public async Task<IActionResult> Index()
        {
            var CountriesLst = await _ICountry.GetAllAsync();
            return View(CountriesLst);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(DtoCounty NewCounty)
        {
     
            if (!ModelState.IsValid)
            {
                TempData["Messagetype"] = (int)Messagetype.SavedFailed;
                return View("Edit", NewCounty);
            }

            try
            {
                if (NewCounty.Id == Guid.Empty)
                {
                    await _ICountry.AddAsync(NewCounty);
                    TempData["Messagetype"] = (int)Messagetype.SavedSuccessfully;
                }
                else
                {
                    await _ICountry.UpdateAsync(NewCounty);
                    TempData["Messagetype"] = (int)Messagetype.UpdatedSuccessfully;
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Messagetype"] = (int)Messagetype.Error;
                return View("Edit", NewCounty);
            }
        }
        public IActionResult Addnew()
        {
            return View("Edit", new DtoCounty());
        }
        public async Task<IActionResult> Edit(Guid CountyId)
        {
            if (CountyId == Guid.Empty)
                return BadRequest();

            var Country = await _ICountry.GetByIdAsync(CountyId);

            if (Country == null)
                return NotFound();

            return View("Edit", Country);
        }
        public async Task<IActionResult> Delete(Guid CountyId)
        {

            if (CountyId == Guid.Empty)
            {
                TempData["Messagetype"] = (int)Messagetype.DeletedFailed;
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _ICountry.ChangeStatus(CountyId);
                TempData["Messagetype"] = (int)Messagetype.DeletedSuccessfully;
            }
            catch
            {
                TempData["Messagetype"] = (int)Messagetype.DeletedFailed;
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Error(int code)
        {
            if (code == 404)
                return View("NotFound");

            return View("Error");
        }
    }
}