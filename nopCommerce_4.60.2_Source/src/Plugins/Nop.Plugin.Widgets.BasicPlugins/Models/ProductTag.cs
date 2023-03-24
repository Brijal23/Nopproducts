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
    public class ProductTag
    {
        public int ProductTagID { get; set; }
        [DisplayName("Name")]
        [RegularExpression(@"^[a-zA-Z0-9\s&]*$", ErrorMessage = "Please enter valid name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter name.")]
        [StringLength(80, MinimumLength = 0, ErrorMessage = "Name length must be between 0 and 80 characters.")]
        public string ProductTagName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    
}
