using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using BusinessLayer.Services;
using DataAccessLayer;
using Domains.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UI.Helpers;

namespace UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class CitiesController : Controller
    {
        private readonly ICities _ICities;
        private readonly ICountries _ICountry;
        private readonly IVwCityService _vwCityService; 
        public CitiesController(ICities Icities, ICountries iCountry, IVwCityService vwCityService  )
        {
            _ICities = Icities;
            _ICountry = iCountry;
            _vwCityService = vwCityService;
        }
        public async Task<IActionResult> Index()
        {
            var VwCitiesLst = await _vwCityService.GetAllAsync();       
            var CitiesLst = await _ICities.GetAllAsync();
            return View(CitiesLst);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(DtoCity NewCity)
        {

            foreach (var error in ModelState)
            {
                foreach (var subError in error.Value.Errors)
                {
                    Console.WriteLine($"Field: {error.Key} - Error: {subError.ErrorMessage}");
                }
            }

            if (!ModelState.IsValid)
            {
                TempData["Messagetype"] = (int)Messagetype.SavedFailed;
                ViewBag.Countries = new SelectList(await _ICountry.GetAllAsync(), "Id", "CountryEname");
                return View("Edit", NewCity);
            }

            try
            {
                if (NewCity.Id == Guid.Empty)
                {
                    await _ICities.AddAsync(NewCity);
                    TempData["Messagetype"] = (int)Messagetype.SavedSuccessfully;
                }
                else
                {
                    await _ICities.UpdateAsync(NewCity);
                    TempData["Messagetype"] = (int)Messagetype.UpdatedSuccessfully;
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Messagetype"] = (int)Messagetype.Error;
                ViewBag.Countries = new SelectList(await _ICountry.GetAllAsync(), "Id", "CountryEname");
                return View("Edit", NewCity);
            }
        }
        public async Task<IActionResult> Addnew()
        {
            ViewBag.Countries = new SelectList(await _ICountry.GetAllAsync(), "Id", "CountryEname");
            return View("Edit", new DtoCity());
        }
        public async Task<IActionResult> Edit(Guid CityId)
        {

            if (CityId == Guid.Empty)
                return BadRequest();

            var City = await _ICities.GetByIdAsync(CityId);

            if (City == null)
                return NotFound();

            ViewBag.Countries = new SelectList(await _ICountry.GetAllAsync(), "Id", "CountryEname");
            return View("Edit", City);
        }
        public async Task<IActionResult> Delete(Guid CityId)
        {

            if (CityId == Guid.Empty)
            {
                TempData["Messagetype"] = (int)Messagetype.DeletedFailed;
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _ICities.ChangeStatus(CityId);
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