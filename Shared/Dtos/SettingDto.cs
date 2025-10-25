using System;
using System.Collections.Generic;

namespace Shared.Dtos;

public partial class SettingDto : BaseDto
{
    public double? KiloMeterRate { get; set; }

    public double? KilooGramRate { get; set; }
}
