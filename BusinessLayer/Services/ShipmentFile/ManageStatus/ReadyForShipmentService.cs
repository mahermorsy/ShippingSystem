using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services.ShipmentFile.ManageStatus
{
    public class ReadyForShipmentService : IShipmentStatusHandler
    {
        IShipmentCommand _Ishipment;
        IShipmentStatus _IshipmentStatus;
        IUserService _UserService;

        public ReadyForShipmentService(IShipmentCommand Ishipment, IShipmentStatus IshipmentStatus, IUserService UserService)
        {
            _Ishipment = Ishipment;
            _IshipmentStatus = IshipmentStatus;
                        _UserService = UserService;
        }
        public ShipmentStatusEnum statusEnum => ShipmentStatusEnum.ReadyForShipment;
        public  async Task StatusHandler(DtoShipment Shipment)
        {
            await _IshipmentStatus.AddShipmentStatus(Shipment.Id, statusEnum, "Shipment ready for shipment.");
            await _Ishipment.EditFieldsAsync(
                   Shipment.Id,
                   E =>
                   {
                       E.CarrierId = Shipment.CarrierId;
                       E.CurrentState = (int)statusEnum;
                       E.UpdatedBy = _UserService.GetLoggedInUser();
                       E.UpdatedDate = DateTime.UtcNow;
                   });
        }
    }
}
