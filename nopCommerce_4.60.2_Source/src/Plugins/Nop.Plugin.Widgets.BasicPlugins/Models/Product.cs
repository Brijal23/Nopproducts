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
    public class Product
    {
        public int ProductID { get; set; }
        [DisplayName("Name")]
        [RegularExpression(@"^[a-zA-Z0-9\s&]*$", ErrorMessage = "Please enter valid name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter name.")]
        [StringLength(80, MinimumLength = 0, ErrorMessage = "Name length must be between 0 and 80 characters.")]
        public string ProductName { get; set; }
        [DisplayName("Short description")]
        [MinLength(1)]
        public string? ShortDescription { get; set; }
        [DisplayName("Full description")]
        public string? FullDescription { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Enter valid SKU.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter SKU.")]
        public string? SKU { get; set; }
        [RegularExpression(@"^(0|\d{0,8}(\.\d{0,4})?)$", ErrorMessage = "Enter valid price.")]
        [Range(0, 99999999.9999, ErrorMessage = "Please enter price between 0 to 99999999.9999")]
        [Required(ErrorMessage = "Enter price.")]
        //[MaxLength(11, ErrorMessage = "Enter valid price.")]
        public decimal Price { get; set; }
        [DisplayName("Published")]
        public bool IsPublished { get; set; }
        [DisplayName("Inventory method")]
        public string? InventoryMethod { get; set; }
        public IEnumerable<SelectListItem>? InventoryMethods { get; set; }
        [DisplayName("Stock quantity")]
        [Range(0, 99999999)]
        public int StockQuantity { get; set; }
        public IEnumerable<SelectListItem>? TaxCategories { get; set; }

        [DisplayName("Shipping enabled")]
        public bool IsShippingEnable { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? SFile { get; set; }
        public List<UploadFileModel>? UploadFileModel { get; set; }
        public List<ShoppingCart>? ShoppingCartList { get; set; }
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Enter valid GTIN.")]
        [MaxLength(13)]
        [DisplayName("GTIN (Global Trade Item Number)")]
        public string? GTIN { get; set; }
        [DisplayName("Manufacturer Part Number")]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Please enter valid manufacturer part number")]
        public string? ManufacturerPartnumber { get; set; }
       
        [DisplayName("Product tags")]
        public string? Producttags { get; set; }
        [DisplayName("Show on home page")]
        public bool IsShowOnHomepage { get; set; }
        [DisplayName("Categories")]
        [Required(ErrorMessage = "Please select category.")]
        public int[]? CategoryID { get; set; }
        [DisplayName("Display order")]
        public int? Displayorder { get; set; }
        [DisplayName("Product type")]
        public int? ProductType { get; set; }
        public IEnumerable<SelectListItem>? ProductTypes { get; set; }
        [DisplayName("Product template")]
        public int? ProductTemplate { get; set; }
        public IEnumerable<SelectListItem>? ProductTemplates { get; set; }
        [DisplayName("Visible individually")]
        public bool IsVisibleindividually { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }
        [DisplayName("Customer roles")]
        public int[]? CustomerroleID { get; set; }
        [DisplayName("Limited to stores")]
        public int[]? LimitedstoresID { get; set; }
        [DisplayName("Vendor")]
        public int[]? VendorID { get; set; }
        [DisplayName("Allow user reviews")]
        public bool IsAllowuserreviews { get; set; }
        //[DisplayName("Require other products")]
        //public bool IsRequireotherproducts { get; set; }
        [DisplayName("Available start date")]
        public DateTime? Availablestartdate { get; set; }
        [DisplayName("Available start date")]
        public string? sAvailablestartdate { get; set; }
        [DisplayName("Available end date")]
        public DateTime? Availableenddate { get; set; }
        [DisplayName("Available end date")]
        public string? sAvailableenddate { get; set; }
        [DisplayName("Mark as new")]
        public bool IsMarkasnew { get; set; }
        [DisplayName("Mark as new. Start date")]
        public DateTime? MarkasnewStartdate { get; set; }
        public string? sMarkasnewStartdate { get; set; }
        [DisplayName("Mark as new. End date")]
        public DateTime? MarkasnewEnddate { get; set; }
        public string? sMarkasnewEnddate { get; set; }
        [DisplayName("Admin comment")]
        public string? Admincomment { get; set; }
        public float? FPrice { get; set; }
        public string? SPrice { get; set; }
        [RegularExpression(@"^(0|\d{0,8}(\.\d{0,4})?)$", ErrorMessage = "Enter valid price.")]
        [Range(0, 99999999.99, ErrorMessage = "Please enter price between 0 to 99999999.99")]
        [DisplayName("Old price")]
        public decimal OldPrice { get; set; }
        [RegularExpression(@"^(0|\d{0,8}(\.\d{0,4})?)$", ErrorMessage = "Enter valid cost.")]
        [Range(0, 99999999.99, ErrorMessage = "Please enter price between 0 to 99999999.99")]
        [DisplayName("Product cost")]
        public decimal Productcost { get; set; }
        [DisplayName("Disable buy button")]
        public bool IsDisablebuybutton { get; set; }
        [DisplayName("Disable wishlist button")]
        public bool IsDisablewishlistbutton { get; set; }
        [DisplayName("Available for pre-order")]
        public bool IsAvailableforpreorder { get; set; }
        [DisplayName("Pre-order availability start date")]
        public DateTime? Preorderavailabilitystartdate { get; set; }
        public string? sPreorderavailabilitystartdate { get; set; }
        [DisplayName("Call for price")]
        public bool IsCallforprice { get; set; }
        [DisplayName("User enters price")]
        public bool IsUserentersprice { get; set; }
        [RegularExpression(@"^(0|\d{0,8}(\.\d{0,4})?)$", ErrorMessage = "Enter valid amount.")]
        [Range(0, 99999999.99)]
        [DisplayName("Minimum amount")]
        public decimal Minimumamount { get; set; }
        [RegularExpression(@"^(0|\d{0,8}(\.\d{0,4})?)$", ErrorMessage = "Enter valid amount.")]
        [Range(0, 99999999.99)]
        [DisplayName("Maximum amount")]
        public decimal Maximumamount { get; set; }
        [DisplayName("Text exempt")]
        public bool IsTextexempt { get; set; }
        [DisplayName("PAngV (base price) enabled")]
        public bool IsBasepriceenabled { get; set; }
        [RegularExpression(@"^(0|\d{0,8}(\.\d{0,4})?)$", ErrorMessage = "Enter valid amount.")]
        [Range(0, 99999999.99)]
        [DisplayName("Amount in product")]
        public decimal Amountinproduct { get; set; }
        [DisplayName("Tax category")]
        public string? Taxcategory { get; set; }
        [DisplayName("Unit of product")]
        public string? Unitofproduct { get; set; }
        public IEnumerable<SelectListItem>? Unitofproducts { get; set; }
        [RegularExpression(@"^(0|\d{0,8}(\.\d{0,4})?)$", ErrorMessage = "Enter valid amount.")]
        [Range(0, 99999999.99)]
        [DisplayName("Reference amount")]
        public decimal Referenceamount { get; set; }
        [DisplayName("Reference unit")]
        public string? Referenceunit { get; set; }
        public IEnumerable<SelectListItem>? Referenceunits { get; set; }
        [DisplayName("Discounts")]
        public int[]? DiscountID { get; set; }
        public IEnumerable<SelectListItem>? Discounts { get; set; }
        [DisplayName("Telecommunications, broadcasting and electronic services")]
        public bool IsServices { get; set; }
       
        [DisplayName("Weight")]
        public decimal Weight { get; set; }
        [DisplayName("Length")]
        public decimal Length { get; set; }
        [DisplayName("Width")]
        public decimal Width { get; set; }
        [DisplayName("Height")]
        public decimal Height { get; set; }
        [DisplayName("Free shipping")]
        public bool IsFreeshipping { get; set; }
        [DisplayName("Ship separately")]
        public bool IsShipseparately { get; set; }
        [RegularExpression(@"^(0|\d{0,8}(\.\d{0,4})?)$", ErrorMessage = "Enter valid charge.")]
        [Range(0, 99999999.99)]
        [DisplayName("Additional shipping charge")]
        public decimal Additionalshippingcharge { get; set; }
        [DisplayName("Delivery date")]
        public int DeliverydateID { get; set; }
        public IEnumerable<SelectListItem>? Deliverydates { get; set; }
       

        [DisplayName("Warehouse")]
        public string? WarehouseID { get; set; }
        public IEnumerable<SelectListItem>? Warehouses { get; set; }
        [DisplayName("Multiple warehouses")]
        public bool IsMultiplewarehouses { get; set; }
        [DisplayName("Display availability")]
        public bool IsDisplayavailability { get; set; }
        [DisplayName("Minimum stock qty")]
        public int? Minimumstockqty { get; set; }
        [DisplayName("Display stock quantity")]
        public int? Displaystockquantity { get; set; }
        [DisplayName("Low stock activity")]
        public string? Lowstockactivity { get; set; }
        public IEnumerable<SelectListItem>? Lowstockactivities { get; set; }
        [DisplayName("Notify for qty below")]
        public int? Notifyforqtybelow { get; set; }
        [DisplayName("Backorders")]
        public string? Backorders { get; set; }
        public IEnumerable<SelectListItem>? BackordersList { get; set; }
        [DisplayName("Allow back in stock subscriptions")]
        public bool IsAllowbackinstocksubscriptions { get; set; }
        [DisplayName("Minimum cart qty")]
        public int? Minimumcartqty { get; set; }
        [DisplayName("Maximum cart qty")]
        public int? Maximumcartqty { get; set; }
        [DisplayName("Not returnable")]
        public bool IsNotreturnable { get; set; }
        [DisplayName("Allowed quantities")]
        public int? Allowedquantities { get; set; }
        [DisplayName("Is gift card")]
        public bool Isgiftcard { get; set; }
        public IEnumerable<SelectListItem>? GiftcardtypeList { get; set; }
        [DisplayName("Gift card type")]
        public string? Giftcardtype { get; set; }
        [RegularExpression(@"^(0|\d{0,8}(\.\d{0,4})?)$", ErrorMessage = "Enter valid charge.")]
        [Range(0, 99999999.99)]
        [DisplayName("Overridden gift card amount")]
        public decimal Giftcardamount { get; set; }
        [DisplayName("Is rental")]
        public bool Isrental { get; set; }
        [DisplayName("Rental period length")]
        public int? Rentalperiodlength { get; set; }
        [DisplayName("Rental period")]
        public string? Rentalperiod { get; set; }
        public IEnumerable<SelectListItem>? RentalperiodList { get; set; }
        [DisplayName("Recurring product")]
        public bool IsRecurringproduct { get; set; }
        [DisplayName("Cycle period")]
        public string? Cycleperiod { get; set; }
        public IEnumerable<SelectListItem>? CycleperiodList { get; set; }
        [DisplayName("Cycle length")]
        public int? Cyclelength { get; set; }
        [DisplayName("Total cycles")]
        public int? Totalcycles { get; set; }
        [DisplayName("Search engine friendly page name")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Please enter valid name")]
        public string? Searchenginefriendlypagename { get; set; }
        [DisplayName("Meta title")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Please enter valid title")]
        public string? Metatitle { get; set; }
        [DisplayName("Meta keywords")]
        [RegularExpression(@"^[a-zA-Z0-9\s]*$", ErrorMessage = "Please enter valid keywords")]
        public string? Metakeywords { get; set; }
        [DisplayName("Meta description")]
        public string? Metadescription { get; set; }
        
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
