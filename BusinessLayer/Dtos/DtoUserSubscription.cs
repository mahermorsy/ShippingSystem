using Domains.Models;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Dtos;

public partial class DtoUserSubscription : BaseDto
{
    public Guid UserId { get; set; }
    public Guid PackageId { get; set; }
    public DateTime SubscriptionDate { get; set; }
    public virtual DtoSubscriptionPackage Package { get; set; } = null!;
}
