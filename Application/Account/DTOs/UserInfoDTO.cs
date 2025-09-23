using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Account.DTOs
{
    namespace Application.Account
    {
        public record UserInfoDto(
            string Id,
            string UserName,
            string Email,
            string FirstName,
            string LastName,
            string Address,
            IEnumerable<string> Roles
        );
    }
}
