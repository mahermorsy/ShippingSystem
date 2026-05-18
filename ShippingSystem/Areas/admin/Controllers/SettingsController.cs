using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Helpers;

namespace UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {

        private readonly ISettings _ISetting;
        public SettingsController(ISettings ISetting)
        {
            _ISetting = ISetting;
        }
        public async Task<IActionResult> Index()
        {
            var SettingsLst = await _ISetting.GetAllAsync();
            return View(SettingsLst);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(DtoSetting NewSetting)
        {

            if (!ModelState.IsValid)
            {
                TempData["Messagetype"] = (int)Messagetype.SavedFailed;
                return View("Edit", NewSetting);
            }

            try
            {
                if (NewSetting.Id == Guid.Empty)
                {
                    await _ISetting.AddAsync(NewSetting);
                    TempData["Messagetype"] = (int)Messagetype.SavedSuccessfully;
                }
                else
                {
                    await _ISetting.UpdateAsync(NewSetting);
                    TempData["Messagetype"] = (int)Messagetype.UpdatedSuccessfully;
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Messagetype"] = (int)Messagetype.Error;
                return View("Edit", NewSetting);
            }
        }
        public IActionResult Addnew()
        {
            return View("Edit", new DtoSetting());
        }
        public async Task<IActionResult> Edit(Guid SettingId)
        {
            if (SettingId == Guid.Empty)
                return BadRequest();

            var Country = await _ISetting.GetByIdAsync(SettingId);

            if (Country == null)
                return NotFound();

            return View("Edit", Country);
        }
        public async Task<IActionResult> Delete(Guid SettingId)
        {

            if (SettingId == Guid.Empty)
            {
                TempData["Messagetype"] = (int)Messagetype.DeletedFailed;
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _ISetting.ChangeStatus(SettingId);
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