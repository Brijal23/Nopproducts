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
    public class SpecificationAttribute
    {
        public int SpecificationAttributeID { get; set; }
        [DisplayName("Name")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Please enter valid name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter name.")]
        public string SpecificationAttributeName { get; set; }
        [DisplayName("Display order")]
        public int Displayorder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<Options>? Options { get; set; }
    }
    public class Options
    {
        public int OptionID { get; set; }
        public int SpecificationAttributeID { get; set; }
        [DisplayName("Name")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Please enter valid name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter name.")]
        public string OptionName { get; set; }

        [DisplayName("Display order")]
        public int Displayorder { get; set; }
       
        [DisplayName("Specify color")]
        public bool IsSpecifycolor { get; set; }
        
        [DisplayName("RGB color")]
        public string? RGBcolor { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
