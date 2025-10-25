using AppResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Dtos;

public partial class CountryDto : BaseDto
{
    [Required(ErrorMessageResourceName = "NameArRequired", ErrorMessageResourceType = typeof(Messages), AllowEmptyStrings = false)]
    [StringLength(100, MinimumLength = 5, ErrorMessageResourceName = "NameLenght", ErrorMessageResourceType = typeof(Messages))]
    public string? CountryAname { get; set; }
    [Required(ErrorMessageResourceName = "NameEnRequired", ErrorMessageResourceType = typeof(Messages), AllowEmptyStrings = false)]
    [StringLength(100, MinimumLength = 5, ErrorMessageResourceName = "NameLenght", ErrorMessageResourceType = typeof(Messages))]
    public string? CountryEname { get; set; }
}
