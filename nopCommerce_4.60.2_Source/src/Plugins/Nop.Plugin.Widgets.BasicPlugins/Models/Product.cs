using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Nop.Plugin.Widgets.BasicPlugins.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        [DisplayName("Name")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Please enter valid name")]
        //[Required(AllowEmptyStrings = false)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter name.")]
        public string ProductName { get; set; }
        [DisplayName("Short description")]
        // [StringLength(288, ErrorMessage = "Maximum 288 characters allowed")]
        [MinLength(1)]
        //[RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        //[RegularExpression(@"^[.*\S.]*$", ErrorMessage = "Please Enter Valid Short description")]
        public string? ShortDescription { get; set; }
        [DisplayName("Full description")]
        //[RegularExpression(@"\S(.*)\S", ErrorMessage = "Please Enter Valid Full Description")]
        public string? FullDescription { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Enter valid SKU.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter SKU.")]
        public string? SKU { get; set; }
        [DisplayName("Published")]
        public bool IsPublished { get; set; }
        [DisplayName("Categories")]
        [Required(ErrorMessage = "Please select category.")]
        public int[] CategoryID { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }
        //[Column(TypeName = "decimal(7,2)")]
        [RegularExpression(@"^(0|\d{0,8}(\.\d{0,2})?)$", ErrorMessage = "Enter valid price.")]
        [Range(0, 99999999.99)]
        [Required(ErrorMessage = "Enter price.")]
        //[MaxLength(11, ErrorMessage = "Enter valid price.")]
        public decimal Price { get; set; }
        public string? SPrice { get; set; }
        [DisplayName("Text exempt")]
        public bool IsTextexempt { get; set; }
        [DisplayName("Tax category")]
        public string? Taxcategory { get; set; }
        public IEnumerable<SelectListItem>? TaxCategories { get; set; }
        [DisplayName("Shipping enabled")]
        public bool IsShippingEnable { get; set; }
        [DisplayName("Inventory method")]
        public string? InventoryMethod { get; set; }
        public IEnumerable<SelectListItem>? InventoryMethods { get; set; }
        [DisplayName("Stock quantity")]
        [Range(0, 99999999)]
        //[MaxLength(8, ErrorMessage = "Stock quantity can't be more than 8 digits.")]
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<UploadFileModel>? UploadFileModel { get; set; }
    }
    public class UploadFileModel
    {
        public int? ImageID { get; set; }
        public string? AltImage { get; set; }
        public IFormFile? File { get; set; }
        public string? sFile { get; set; }
        public string? Title { get; set; }
        public string? Displayorder { get; set; }
        public string? ProductID { get; set; }
    }
   
}
