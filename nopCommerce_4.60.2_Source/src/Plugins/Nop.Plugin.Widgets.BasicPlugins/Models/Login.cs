using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Humanizer;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Nop.Plugin.Widgets.BasicPlugins.Models
{
    public class Login
    {
        //[EmailAddress]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter valid email")]
        [Required(ErrorMessage = "Please enter email.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter password.")]
        public string Password { get; set; } = string.Empty;
        [DisplayName("Remember me?")]
        public bool IsRememberme { get; set; }
    }

}
