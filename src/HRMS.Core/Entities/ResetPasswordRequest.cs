using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Core.Entities
{
    public class ResetPasswordRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }

        [Required, MinLength(6)]
        public string NewPassword { get; set; }
    }
}
