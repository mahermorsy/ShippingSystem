using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Helpers;

namespace UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class SubscriptionPackagesController : Controller
    {
        private readonly ISubscriptionPackage _ISubscriptionPackage;
        public SubscriptionPackagesController(ISubscriptionPackage SubscriptionPackage)
        {
            _ISubscriptionPackage = SubscriptionPackage;
        }
        public async Task<IActionResult> Index()
        {
            var SubscriptionPackagesLst = await _ISubscriptionPackage.GetAllAsync();
            return View(SubscriptionPackagesLst);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(DtoSubscriptionPackage NewSubscriptionPackage)
        {

            if (!ModelState.IsValid)
            {
                TempData["Messagetype"] = (int)Messagetype.SavedFailed;
                return View("Edit", NewSubscriptionPackage);
            }

            try
            {
                if (NewSubscriptionPackage.Id == Guid.Empty)
                {
                    await _ISubscriptionPackage.AddAsync(NewSubscriptionPackage);
                    TempData["Messagetype"] = (int)Messagetype.SavedSuccessfully;
                }
                else
                {
                    await _ISubscriptionPackage.UpdateAsync(NewSubscriptionPackage);
                    TempData["Messagetype"] = (int)Messagetype.UpdatedSuccessfully;
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Messagetype"] = (int)Messagetype.Error;
                return View("Edit", NewSubscriptionPackage);
            }
        }
        public IActionResult Addnew()
        {
            return View("Edit", new DtoSubscriptionPackage());
        }
        public async Task<IActionResult> Edit(Guid SubscriptionPackageId)
        {
            if (SubscriptionPackageId == Guid.Empty)
                return BadRequest();

            var Country = await _ISubscriptionPackage.GetByIdAsync(SubscriptionPackageId);

            if (Country == null)
                return NotFound();

            return View("Edit", Country);
        }
        public async Task<IActionResult> Delete(Guid SubscriptionPackageId)
        {

            if (SubscriptionPackageId == Guid.Empty)
            {
                TempData["Messagetype"] = (int)Messagetype.DeletedFailed;
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _ISubscriptionPackage.ChangeStatus(SubscriptionPackageId);
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