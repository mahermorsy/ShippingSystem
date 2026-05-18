using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;


namespace BusinessLayer.Services.ShipmentFile.ManageStatus
{
    public class ShippedService : IShipmentStatusHandler
    {
        IShipmentCommand _Ishipment;
        IShipmentStatus _IshipmentStatus;
        IUserService _UserService;
        public ShippedService(IShipmentCommand Ishipment, IShipmentStatus IshipmentStatus, IUserService UserService)
        {
            _Ishipment = Ishipment;
            _IshipmentStatus = IshipmentStatus;
            _UserService = UserService; 
        }
        public ShipmentStatusEnum statusEnum => ShipmentStatusEnum.Shipped;
        public async Task StatusHandler(DtoShipment Shipment)
        {
            await _IshipmentStatus.AddShipmentStatus(Shipment.Id, statusEnum, "Shipment shipped.");
            await _Ishipment.EditFieldsAsync(
             Shipment.Id,
              E =>
              {
                  E.DeliveryDate = Shipment.DeliveryDate;
                  E.CurrentState = (int)ShipmentStatusEnum.Shipped;
                  E.UpdatedBy = _UserService.GetLoggedInUser();
                  E.UpdatedDate = DateTime.UtcNow;
              });
        }
    }
}
