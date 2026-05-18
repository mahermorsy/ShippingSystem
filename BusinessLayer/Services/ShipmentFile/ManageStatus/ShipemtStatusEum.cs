using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services.ShipmentFile.ManageStatus
{
    public enum ShipmentStatusEnum
    {
        Created = 0,
        Deleted = 1,
        Approved = 2, // Can Edit or Delete
        ReadyForShipment = 3, // Cannot Change City Or Country Can Delete
        Shipped = 4, // Cannot Change City Or Country Cannot Delete
        Delivered = 5,
        Returned = 6,
        Cancelled = 7,
    }
}
