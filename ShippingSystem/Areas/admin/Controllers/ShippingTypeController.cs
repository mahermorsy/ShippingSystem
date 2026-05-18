using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Helpers;

namespace UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class ShippingTypeController : Controller
    {
        private readonly IShippingTypes _ShTypes;

        public ShippingTypeController(IShippingTypes shTypes)
        {
            _ShTypes = shTypes;
        }

        public async Task<IActionResult> Index()
        {
            var shTypeList = await _ShTypes.GetAllAsync();
            return View(shTypeList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(DtoShippingType newShiptype)
        {

            if (!ModelState.IsValid)
            {
                TempData["Messagetype"] = (int)Messagetype.SavedFailed;
                return View("Edit", newShiptype);
            }

            try
            {
                if (newShiptype.Id == Guid.Empty)
                {
                    await _ShTypes.AddAsync(newShiptype);
                    TempData["Messagetype"] = (int)Messagetype.SavedSuccessfully;
                }
                else
                {
                    await _ShTypes.UpdateAsync(newShiptype);
                    TempData["Messagetype"] = (int)Messagetype.UpdatedSuccessfully;
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Messagetype"] = (int)Messagetype.Error;
                return View("Edit", newShiptype);
            }
        }

        public IActionResult Addnew()
        {
            return View("Edit", new DtoShippingType());
        }

        public async Task<IActionResult> Edit(Guid ShipTypeId)
        {
            if (ShipTypeId == Guid.Empty)
                return BadRequest();

            var shipType = await _ShTypes.GetByIdAsync(ShipTypeId);

            if (shipType == null)
                return NotFound();

            return View("Edit", shipType);
        }

        public async Task<IActionResult> Delete(Guid ShipTypeId)
        {

            if (ShipTypeId == Guid.Empty)
            {
                TempData["Messagetype"] = (int)Messagetype.DeletedFailed;
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _ShTypes.ChangeStatus(ShipTypeId);
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