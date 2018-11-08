using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Models
{
    public class Login
    {
        [Required(ErrorMessage = "Informe seu email")]
        public string email { get; set; }

        [Required(ErrorMessage = "Informe sua senha")]
        public string password { get; set; }
    }
}