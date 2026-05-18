using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Helpers;

namespace UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class ShippingPackagingController : Controller
    {
        private readonly IShippingPackaging _IShippingPackaging;
        public ShippingPackagingController(IShippingPackaging ShippingPackaging)
        {
            _IShippingPackaging = ShippingPackaging;
        }
        public async Task<IActionResult> Index()
        {
            var ShippingPackagingLst = await _IShippingPackaging.GetAllAsync();
            return View(ShippingPackagingLst);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(DTOShippingPackaging NewShippingPackaging)
        {

            if (!ModelState.IsValid)
            {
                TempData["Messagetype"] = (int)Messagetype.SavedFailed;
                return View("Edit", NewShippingPackaging);
            }

            try
            {
                if (NewShippingPackaging.Id == Guid.Empty)
                {
                    await _IShippingPackaging.AddAsync(NewShippingPackaging);
                    TempData["Messagetype"] = (int)Messagetype.SavedSuccessfully;
                }
                else
                {
                    await _IShippingPackaging.UpdateAsync(NewShippingPackaging);
                    TempData["Messagetype"] = (int)Messagetype.UpdatedSuccessfully;
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Messagetype"] = (int)Messagetype.Error;
                return View("Edit", NewShippingPackaging);
            }   
        }
        public IActionResult Addnew()
        {
            return View("Edit", new DTOShippingPackaging());
        }
        public async Task<IActionResult> Edit(Guid ShippingPackagingId)
        {
            if (ShippingPackagingId == Guid.Empty)
                return BadRequest();

            var ShippingPackaging = await _IShippingPackaging.GetByIdAsync(ShippingPackagingId);

            if (ShippingPackaging == null)
                return NotFound();

            return View("Edit", ShippingPackaging);
        }
        public async Task<IActionResult> Delete(Guid ShippingPackagingId)
        {

            if (ShippingPackagingId == Guid.Empty)
            {
                TempData["Messagetype"] = (int)Messagetype.DeletedFailed;
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _IShippingPackaging.ChangeStatus(ShippingPackagingId);
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