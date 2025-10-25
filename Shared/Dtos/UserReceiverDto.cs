﻿using Shared.Dtos;
using System;
using System.Collections.Generic;

namespace Shared.Dtos;

public partial class UserReceiverDto : BaseDto
{
    public Guid UserId { get; set; }

    public string ReceiverName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;
    public string PostalCode { get; set; }
    public string Contact { get; set; } = null!;
    public string OtherAddress { get; set; } = null!;
    public Guid CityId { get; set; }

    public string Address { get; set; } = null!;
}
