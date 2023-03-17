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
    public class ProductAttribute
    {
        public int ProductAttributeID { get; set; }
        [DisplayName("Name")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Please enter valid name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter name.")]
        public string ProductAttributeName { get; set; }
        [DisplayName("Description")]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<PredefineValues>? PredefineValues { get; set; }
    }
    public class PredefineValues
    {
        public int PredefinevalueID { get; set; }
        public int ProductAttributeID { get; set; }
        [DisplayName("Name")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Please enter valid name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter name.")]
        public string ValueName { get; set; }

        [RegularExpression(@"^(0|\d{0,8}(\.\d{0,4})?)$", ErrorMessage = "Enter valid price.")]
        [Range(0, 99999999.9999, ErrorMessage = "Please enter price between 0 to 99999999.9999")]
        [Required(ErrorMessage = "Enter price.")]
        [DisplayName("Price adjustment")]
        public decimal Priceadjustment { get; set; }
        public string? sPriceadjustment { get; set; }
        [DisplayName("Price adjustment. Use percentage")]
        public bool IsUsepercentage { get; set; }
        [RegularExpression(@"^(0|\d{0,8}(\.\d{0,4})?)$", ErrorMessage = "Enter valid weight adjustment.")]
        [Range(0, 99999999.9999, ErrorMessage = "Please enter price between 0 to 99999999.9999")]
        [Required(ErrorMessage = "Enter weight adjustment.")]
        [DisplayName("Weight adjustment")]
        public decimal Weightadjustment { get; set; }
        public string? sWeightadjustment { get; set; }
        [RegularExpression(@"^(0|\d{0,8}(\.\d{0,4})?)$", ErrorMessage = "Enter valid cost.")]
        [Range(0, 99999999.9999, ErrorMessage = "Please enter price between 0 to 99999999.9999")]
        [Required(ErrorMessage = "Enter cost.")]
        [DisplayName("Cost")]
        public decimal Cost { get; set; }
        [DisplayName("Is pre-selected")]
        public bool Ispreselected { get; set; }
        [DisplayName("Display order")]
        public int Displayorder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
