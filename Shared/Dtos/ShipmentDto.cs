using System;
using System.Collections.Generic;

namespace Shared.Dtos;

public partial class ShipmentDto : BaseDto
{
    public DateTime ShippingDate { get; set; } = DateTime.Now;

    public Guid SenderId { get; set; }
    public UserSenderDto UserSender { get; set; }
    public Guid ReceiverId { get; set; }
    public UserReceiverDto UserReceiver { get; set; }
    public Guid ShippingTypeId { get; set; }
    public Guid? ShipingPackgingId { get; set; }
    public int CurrentState { get; set; }

    public double Width { get; set; }
    public DateTime DelivryDate { get; set; }
    public double Height { get; set; }

    public double Weight { get; set; }

    public double Length { get; set; }

    public decimal PackageValue { get; set; }

    public decimal ShippingRate { get; set; }

    public Guid? PaymentMethodId { get; set; }

    public Guid? UserSubscriptionId { get; set; }

    public double? TrackingNumber { get; set; }

    public Guid? ReferenceId { get; set; }
}