using System;
using System.Collections.Generic;

namespace Domains;

public partial class TbCarrier : BaseTable
{

    public string CarrierName { get; set; } = null!;

    public virtual ICollection<TbShippmentStatus> TbShippmentStatuses { get; set; } = new List<TbShippmentStatus>();
}
