using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class UserResultDto
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
        public UserDto User { get; set; }  // ⬅️ أضف هذا

    }
}
