﻿using System;
using System.Collections.Generic;

namespace Shared.Dtos;

public partial class PaymentMethodDto : BaseDto
{
    public string? MethdAname { get; set; }

    public string? MethodEname { get; set; }

    public double? Commission { get; set; }
}
