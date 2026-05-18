using BusinessLayer.Dtos;
using DataAccessLayer.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.UI_Services.ShipmentServices;

namespace UI.Controllers
{
    public class ShipmentsController : Controller
    {
        private readonly IShipmentApiService shipmentApiService_;

        public ShipmentsController(IShipmentApiService shipmentService)
        {
            shipmentApiService_ = shipmentService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreateShipment()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateShipment([FromBody] DtoShipment dtoShipment)
        {
            if (dtoShipment == null)
            {
                return BadRequest(new { success = false, message = "Shipment data is required." });
            }

            try
            {
                var result = await shipmentApiService_.CreateShipmentAsync(dtoShipment);

                if (result.Success)
                {
                    return Ok(new { success = true, message = "Shipment created successfully." });
                }

                return StatusCode(500, new { success = false, message = "Failed to create shipment." });
            }
            catch (DataAccessException ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(Guid ShId)
        {
            if (ShId == Guid.Empty)
            {
                return RedirectToAction(nameof(List));
            }

            return View(new DtoShipment());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit([FromBody] DtoShipment dtoShipment)
        {
            if (dtoShipment == null || dtoShipment.Id == Guid.Empty)
            {
                return BadRequest(new { success = false, message = "Shipment data is required." });
            }

            try
            {
                var result = await shipmentApiService_.EditShipmentAsync(dtoShipment);

                if (result.Success)
                {
                    return Ok(new { success = true, message = "Shipment updated successfully." });
                }

                return StatusCode(500, new { success = false, message = "Failed to update shipment." });
            }
            catch (DataAccessException ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [Authorize]
        public IActionResult List(int Pagenumber = 1)
        {
            return View(new List<DtoShipment>());
        }

        [Authorize]
        public IActionResult Show(Guid ShId)
        {
            if (ShId == Guid.Empty)
            {
                return RedirectToAction(nameof(List));
            }

            return View(new DtoShipment());
        }

        public async Task<IActionResult> Delete(Guid ShId)
        {
            if (ShId == Guid.Empty)
            {
                return RedirectToAction(nameof(List));
            }
           // await shipmentApiService_.DeleteShipmentAsync(ShId);        
            return RedirectToAction(nameof(List));
        }
    }
}
