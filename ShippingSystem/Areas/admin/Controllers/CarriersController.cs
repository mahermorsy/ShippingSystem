using BusinessLayer.Contracts;
using BusinessLayer.Services;
using BusinessLayer.Dtos;
using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Helpers;


namespace UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class CarriersController : Controller
    {
        private readonly ICarriers _ICarrier;
        public CarriersController(ICarriers ICarrier)
        {
            _ICarrier = ICarrier;
        }
        public async Task<IActionResult> Index()
        {
            var CarriersLst
                = await _ICarrier.GetAllAsync();
            return View(CarriersLst);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(DtoCarrier NewCarrier)
        {
    

            if (!ModelState.IsValid)
            {
                TempData["Messagetype"] = (int)Messagetype.SavedFailed;
                return View("Edit", NewCarrier);
            }

            try
            {
                if (NewCarrier.Id == Guid.Empty)
                {
                    await _ICarrier.AddAsync(NewCarrier);
                    TempData["Messagetype"] = (int)Messagetype.SavedSuccessfully;
                }
                else
                {
                    await _ICarrier.UpdateAsync(NewCarrier);
                    TempData["Messagetype"] = (int)Messagetype.UpdatedSuccessfully;
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Messagetype"] = (int)Messagetype.Error;
                return View("Edit", NewCarrier);
            }
        }
        public IActionResult Addnew()
        {
            return View("Edit", new DtoCarrier());
        }
        public async Task<IActionResult> Edit(Guid CarrierId)
        {
            if (CarrierId == Guid.Empty)
                return BadRequest();

            var Carrier = await _ICarrier.GetByIdAsync(CarrierId);

            if (Carrier == null)
                return NotFound();

            return View("Edit", Carrier);
        }
        public async Task<IActionResult> Delete(Guid CarrierId)
        {

            if (CarrierId == Guid.Empty)
            {
                TempData["Messagetype"] = (int)Messagetype.DeletedFailed;
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _ICarrier.ChangeStatus(CarrierId);
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