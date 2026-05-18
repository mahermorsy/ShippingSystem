using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Helpers;

namespace UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class PaymentMethodsController : Controller
    {
        private readonly IPaymentMethods _IPaymentMethod;
        public PaymentMethodsController(IPaymentMethods ICountry)
        {
            _IPaymentMethod = ICountry;
        }
        public async Task<IActionResult> Index()
        {
            var PaymentMethodsLst = await _IPaymentMethod.GetAllAsync();
            return View(PaymentMethodsLst);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(DtoPaymentMethod NewPaymentMethod)
        {

            if (!ModelState.IsValid)
            {
                TempData["Messagetype"] = (int)Messagetype.SavedFailed;
                return View("Edit", NewPaymentMethod);
            }

            try
            {
                if (NewPaymentMethod.Id == Guid.Empty)
                {
                    await _IPaymentMethod.AddAsync(NewPaymentMethod);
                    TempData["Messagetype"] = (int)Messagetype.SavedSuccessfully;
                }
                else
                {
                    await _IPaymentMethod.UpdateAsync(NewPaymentMethod);
                    TempData["Messagetype"] = (int)Messagetype.UpdatedSuccessfully;
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Messagetype"] = (int)Messagetype.Error;
                return View("Edit", NewPaymentMethod);
            }
        }
        public IActionResult Addnew()
        {
            return View("Edit", new DtoPaymentMethod());
        }
        public async Task<IActionResult> Edit(Guid PaymentMethodId)
        {
            if (PaymentMethodId == Guid.Empty)
                return BadRequest();

            var PaymentMethod = await _IPaymentMethod.GetByIdAsync(PaymentMethodId);

            if (PaymentMethod == null)
                return NotFound();

            return View("Edit", PaymentMethod);
        }
        public async Task<IActionResult> Delete(Guid PaymentMethodId)
        {

            if (PaymentMethodId == Guid.Empty)
            {
                TempData["Messagetype"] = (int)Messagetype.DeletedFailed;
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _IPaymentMethod.ChangeStatus(PaymentMethodId);
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