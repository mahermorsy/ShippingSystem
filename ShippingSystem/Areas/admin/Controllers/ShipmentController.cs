using BusinessLayer.Dtos;
using BusinessLayer.Services.ShipmentFile.ManageStatus;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.UI_Services.ShipmentServices;

namespace UI.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin,Reviewer,Operation,Operation Manager")]
    public class ShipmentController : Controller
    {
        private readonly IShipmentApiService _shipmentApiService;

        public ShipmentController(IShipmentApiService shipmentApiService)
        {
            _shipmentApiService = shipmentApiService;
        }

        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            ShipmentStatusEnum? status = null; // You can set this to a specific status if needed

            if(User.IsInRole("Reviewer"))
            {
                status = ShipmentStatusEnum.Created; // Example: set status for operation users
            }
            else if (User.IsInRole("Operation"))
            {
                status = ShipmentStatusEnum.Approved; // Admin can see all shipments regardless of status
            }
            else if (User.IsInRole("Operation Manager"))
            {
                status = ShipmentStatusEnum.ReadyForShipment; // Admin can see all shipments regardless of status
            }

            var response = await _shipmentApiService.GetShipmentsList(pageNumber, 10, false, status);

            if (response.Success && response.Data != null)
            {
                return View(response.Data);
            }

            return View(new PagedResult<DtoShipment>
            {
                Items = new List<DtoShipment>(),
                PageNumber = pageNumber,
                PageSize = 10,
                TotalCount = 0
            });
        }

        public IActionResult Edit(Guid shId)
        {
            ViewBag.ShipmentId = shId;
            ViewBag.UserRole = User.IsInRole("Admin") ? "Admin" :
                               User.IsInRole("Reviewer") ? "Reviewer" :
                               User.IsInRole("Operation Manager") ? "Operation Manager" :
                               User.IsInRole("Operation") ? "Operation" : "";

            return View(new DtoShipment());
        }
        public async Task<IActionResult> ChangeStatusAsync(DtoShipment shipment)
        {
            await _shipmentApiService.ChangeStatusAsync(shipment);
            return RedirectToAction("Index");
        }
    }
}
