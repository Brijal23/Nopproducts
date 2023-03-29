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
    public class ShoppingCart
    {
        public int? CartID { get; set; }
        public int? ProductID { get; set; }
        public string? Size { get; set; }
        public string? ProductName { get; set; }
        public string? ImageData { get; set; }
        public decimal? Price { get; set; }
        public float? FPrice { get; set; }
        public float? FEPrice { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? Quantity { get; set; }
        public bool Ischecked { get; set; }
        public List<UploadFileModel>? UploadFileModel { get; set; }
        //public bool? IsActive { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public DateTime? UpdatedDate { get; set; }
    }
    public class ListShoppingCart
    {
        public List<ShoppingCart> ShoppingCartList { get; set; }
    }
}
