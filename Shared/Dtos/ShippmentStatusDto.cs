using System;
using System.Collections.Generic;

namespace Shared.Dtos;

public partial class ShippmentStatusDto : BaseDto
{
    public Guid? ShippmentId { get; set; }

    public int CurrentState { get; set; }

    public string? Notes { get; set; }

    public Guid CarrierId { get; set; }
}
