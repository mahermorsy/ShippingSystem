using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using Domains.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services.ShipmentFile.ManageStatus
{
    public class ShipmentStatusHandlerFactory : IShipmentStatusHandlerFactory
    {
        IServiceProvider _serviceProvider;
        public ShipmentStatusHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IShipmentStatusHandler GetStatusHandler(ShipmentStatusEnum statusEnum)
        {
            return statusEnum switch
            {
                ShipmentStatusEnum.Approved => _serviceProvider.GetRequiredService<ApproveService>(),
                ShipmentStatusEnum.ReadyForShipment => _serviceProvider.GetRequiredService<ReadyForShipmentService>(),
                ShipmentStatusEnum.Shipped => _serviceProvider.GetRequiredService<ShippedService>(),
                ShipmentStatusEnum.Delivered => _serviceProvider.GetRequiredService<DeliveredService>(),
                ShipmentStatusEnum.Returned => _serviceProvider.GetRequiredService<ReturnedService>(),
                _ => throw new NotImplementedException()

            };
        }
    }
}
