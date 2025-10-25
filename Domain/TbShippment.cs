using System;
using System.Collections.Generic;

namespace Domains;

public partial class TbShippment : BaseTable
{

    public DateTime ShippingDate { get; set; }
    public DateTime DelivryDate { get; set; }

    public Guid SenderId { get; set; }
    
    public Guid ReceiverId { get; set; }

    public Guid ShippingTypeId { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }

    public double Weight { get; set; }

    public double Length { get; set; }

    public decimal PackageValue { get; set; }

    public decimal ShippingRate { get; set; }

    public Guid? PaymentMethodId { get; set; }

    public Guid? UserSubscriptionId { get; set; }

    public double? TrackingNumber { get; set; }

    public Guid? ReferenceId { get; set; }
    public Guid? ShipingPackgingId { get; set; }
    public virtual TbShipingPackging ShipingPackging { get; set; } = null!;


    public virtual TbPaymentMethod? PaymentMethod { get; set; }

    public virtual TbUserReceiver Receiver { get; set; } = null!;

    public virtual TbUserSebder Sender { get; set; } = null!;

    public virtual TbShippingType ShippingType { get; set; } = null!;

    public virtual ICollection<TbShippmentStatus> TbShippmentStatuses { get; set; } = new List<TbShippmentStatus>();
}
