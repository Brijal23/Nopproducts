//using System;
//using System.Formats.Tar;
//using System.Globalization;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web;
//using System.Xml.Linq;
//using DocumentFormat.OpenXml.EMMA;
//using DocumentFormat.OpenXml.Spreadsheet;
//using DocumentFormat.OpenXml.Vml;
//using MaxMind.GeoIP2.Model;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.Data.SqlClient;
//using Nop.Core;
//using Nop.Plugin.Widgets.BasicPlugins.Models;
//using Nop.Web.Framework;
//using Nop.Web.Framework.Controllers;
//using Nop.Web.Framework.Mvc.Filters;
//using static ClosedXML.Excel.XLPredefinedFormat;
//using DateTime = System.DateTime;
//using Path = System.IO.Path;

//namespace Nop.Plugin.Widgets.BasicPlugins.Controllers
//{
//    [AuthorizeAdmin]
//    [Area(AreaNames.Admin)]
//    [AutoValidateAntiforgeryToken]
//    public class DemoProductController : BasePluginController
//    {

//        [HttpGet]
//        public ActionResult ProductList(string productName = "", string published = "All", string category = "0")
//        {
//            if (string.IsNullOrWhiteSpace(productName))
//                productName = null;
//            List<Product> Products = new();
//            try
//            {
//                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";

//                using (SqlConnection connection = new SqlConnection(constr))
//                {
//                    connection.Open();
//                    //string Query = "SELECT * FROM [dbo].[Product] AS P LEFT JOIN [dbo].[ImageDetail] as I on I.ProductID = P.ProductID ";
//                    string Query = "SELECT * FROM [dbo].[Product] AS P ";

//                    if (category != "0")
//                    {

//                        Query += " INNER JOIN [dbo].[CategoryDetail] as C on C.ProductID = P.ProductID";
//                    }
//                    Query += " WHERE P.IsActive = 1";
//                    if (category != "0")
//                    {
//                        int Icategory = Convert.ToInt32(category);
//                        Query += " AND C.CategoryID =" + Icategory + "";
//                    }
//                    if (productName != "" && productName != null)
//                    {
//                        Query += " AND P.ProductName LIKE '%" + productName + "%'";
//                    }
//                    if (published != "All")
//                    {
//                        int Ipublished = Convert.ToInt32(published);
//                        Query += " AND P.IsPublished =" + Ipublished + "";
//                    }
//                    // Query += " ORDER BY CreatedDate DESC";
//                    using (SqlCommand command = new SqlCommand(Query, connection))
//                    {
//                        using (SqlDataReader reader = command.ExecuteReader())
//                        {
//                            while (reader.Read())
//                            {
//                                Products.Add(new Product
//                                {
//                                    //SFile = reader["ImageData"].ToString() != "" ? RetrieveImage((byte[])reader["ImageData"]) : null,
//                                    ProductName = reader["ProductName"].ToString(),
//                                    ProductID = Convert.ToInt32(reader["ProductID"].ToString()),
//                                    SKU = reader["SKU"].ToString(),
//                                    SPrice =  reader["Price"].ToString().Replace(".00", "") + " USD" ,
//                                    IsPublished = Convert.ToBoolean(reader["IsPublished"].ToString()) == true ? true : false,
//                                    StockQuantity = Convert.ToInt32(reader["StockQuantity"].ToString()),
//                                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
//                                });

//                            }
//                        }
//                    }
//                }

//            }
//            catch (Exception ex)
//            {
//                ViewBag.SuccessMessage = ex.Message.ToString();
//                return View("~/Plugins/Widgets.BasicPlugins/Views/ProductList.cshtml", Products);

//            }
//            ViewBag.productnamesearch = productName;
//            ViewBag.publishedsearch = published;
//            ViewBag.categorysearch = category;
//            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductList.cshtml", Products);
//        }
//        [HttpGet]
//        public ActionResult AddProduct(int id = 0)
//        {
//            Product product = new();
//            GetAllMethods(product);
           
//            return View("~/Plugins/Widgets.BasicPlugins/Views/AddProduct.cshtml", product);
//        }

//        private void GetAllMethods(Product product)
//        {
//            product.Categories = new[]
//          {
//        new SelectListItem { Value = "1", Text = "Lunch & Dinner" },
//        new SelectListItem { Value = "2", Text = "Breakfast & Brunch" },
//        new SelectListItem { Value = "3", Text = "Baked Goods & Desserts" },
//        new SelectListItem { Value = "4", Text = "Drinks" },
//             };
//            product.TaxCategories = new[]
//            {
//        new SelectListItem { Value = "0", Text = "[None]" },
//        new SelectListItem { Value = "1", Text = "Food" },
//        new SelectListItem { Value = "2", Text = "Bevarage" },

//             };
//            product.InventoryMethods = new[]
//            {
//        new SelectListItem { Value = "0", Text = "Don't track inventory" },
//        new SelectListItem { Value = "1", Text = "Track inventory" },
//        new SelectListItem { Value = "2", Text = "Track inventory by product attributes" },

//             };
//            product.ProductTypes = new[]
//            {
//        new SelectListItem { Value = "1", Text = "Simple" },
//        new SelectListItem { Value = "2", Text = "Grouped(product with variants)" }

//             };
//            product.ProductTemplates = new[]
//           {
//        new SelectListItem { Value = "1", Text = "Simple product" },
//        new SelectListItem { Value = "2", Text = "Product Weight" }

//             };
//            product.Unitofproducts = new[]
//           {
//        new SelectListItem { Value = "1", Text = "ounce(s)" },
//        new SelectListItem { Value = "2", Text = "lb(s)" },
//        new SelectListItem { Value = "3", Text = "kg(s)" },
//        new SelectListItem { Value = "4", Text = "gram(s)" }

//             };
//            product.Referenceunits = new[]
//           {
//        new SelectListItem { Value = "1", Text = "ounce(s)" },
//        new SelectListItem { Value = "2", Text = "lb(s)" },
//        new SelectListItem { Value = "3", Text = "kg(s)" },
//        new SelectListItem { Value = "4", Text = "gram(s)" }

//             };
//            product.Discounts = new[]
//          {
//        new SelectListItem { Value = "1", Text = "Simple" },
//        new SelectListItem { Value = "2", Text = "Sample discount with coupon code" }

//             };
//            product.Deliverydates = new[]
//          {
//        new SelectListItem { Value = "0", Text = "None" },
//        new SelectListItem { Value = "1", Text = "1-2 days" },
//        new SelectListItem { Value = "2", Text = "3-5 days" },
//        new SelectListItem { Value = "3", Text = "1 week" }

//             };
//            product.Warehouses = new[]
//         {
//        new SelectListItem { Value = "0", Text = "None" }

//             };
//            product.Lowstockactivities = new[]
//          {
//        new SelectListItem { Value = "0", Text = "Nothing" },
//        new SelectListItem { Value = "1", Text = "Disable buy button" },
//        new SelectListItem { Value = "2", Text = "Unpublish" }

//             };
//            product.BackordersList = new[]
//         {
//        new SelectListItem { Value = "0", Text = "No backorders" },
//        new SelectListItem { Value = "1", Text = "Allow qty below 0" },
//        new SelectListItem { Value = "2", Text = "Allow qty below 0 and notify user" }

//             };
//            product.GiftcardtypeList = new[]
//        {
//        new SelectListItem { Value = "1", Text = "Virtual" },
//        new SelectListItem { Value = "2", Text = "Physical" }

//             };
//            product.RentalperiodList = new[]
//           {
//                    new SelectListItem { Value = "0", Text = "Days" },
//                    new SelectListItem { Value = "1", Text = "Weeks" },
//                    new SelectListItem { Value = "2", Text = "Months" },
//                    new SelectListItem { Value = "3", Text = "Years" },
//            };
//            product.CycleperiodList = new[]
//          {
//                    new SelectListItem { Value = "0", Text = "Days" },
//                    new SelectListItem { Value = "1", Text = "Weeks" },
//                    new SelectListItem { Value = "2", Text = "Months" },
//                    new SelectListItem { Value = "3", Text = "Years" },
//            };
//        }

//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public ActionResult AddProduct(Product model)
//        {
//            Product product = new();
//            GetAllMethods(product);

//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    //ViewBag.SuccessMessage = "Enter required fields.";
//                    return View("~/Plugins/Widgets.BasicPlugins/Views/AddProduct.cshtml", product);
//                }
//                else
//                {
//                    ViewBag.SuccessMessage = "";
//                    int ProductID = 0;
//                    int Count = 0;
//                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
//                    {
//                        connection.Open();
//                        string q = "select Count(*) as Count from [dbo].[Product] where ProductName='" + model.ProductName + "'";
//                        using (SqlCommand command = new SqlCommand(q, connection))
//                        {
//                            using (SqlDataReader r = command.ExecuteReader())
//                            {
//                                while (r.Read())
//                                {
//                                    Count = Convert.ToInt32(r["Count"].ToString());
//                                }
//                            }
//                        }
//                        connection.Close();
//                        if (Count == 0)
//                        {
//                            if (model.FullDescription != null)
//                                model.FullDescription = HttpUtility.UrlEncode(model.FullDescription);
//                            if (product.ShortDescription != null)
//                                product.ShortDescription = HttpUtility.UrlEncode(product.ShortDescription);
//                            //CultureInfo provider = CultureInfo.InvariantCulture;
//                            if (model.sAvailablestartdate != null)
//                            {
//                                model.Availablestartdate = DateTime.ParseExact(model.sAvailablestartdate, "MM/dd/yyyy HH:mm:ss tt", CultureInfo.InvariantCulture);
//                                //DateTime.Parse(model.sAvailablestartdate).ToString("MM/dd/yyyy HH:mm");
//                            }
//                            if (model.sAvailableenddate != null)
//                            {
//                                model.Availableenddate = DateTime.Parse(model.sAvailableenddate).ToString("MM/dd/yyyy HH:mm");
//                            }
//                            if (model.sMarkasnewStartdate != null)
//                            {
//                                model.MarkasnewStartdate = DateTime.Parse(model.sMarkasnewStartdate).ToString("MM/dd/yyyy HH:mm");
//                            }
//                            if (model.sMarkasnewEnddate != null)
//                            {
//                                model.MarkasnewEnddate = DateTime.Parse(model.sMarkasnewEnddate).ToString("MM/dd/yyyy HH:mm");
//                            }
//                            if (model.sPreorderavailabilitystartdate != null)
//                            {
//                                model.Preorderavailabilitystartdate = DateTime.Parse(model.sPreorderavailabilitystartdate).ToString("MM/dd/yyyy HH:mm");
//                            }
//                            string query = "INSERT INTO [Product] (ProductName, ShortDescription, FullDescription,SKU,IsPublished,Price,IsTextexempt,Taxcategory,IsShippingEnabled,InventoryMethod,StockQuantity,IsActive,CreatedDate,UpdatedDate,[Producttags],[GTIN],[ManufacturerPartnumber],[IsShowOnHomepage],[DisplayOrder],[ProductType],[ProductTemplate],[IsVisibleIndividually],[VendorId],[IsAllowuserreviews],[Availablestartdate],[Availableenddate],[IsMarkasnew],[MarkasnewStartdate],[MarkasnewEnddate],[Admincomment],[OldPrice],[Productcost],[IsDisablebuybutton],[IsDisablewishlistbutton] ,[IsAvailableforpreorder],[Preorderavailabilitystartdate],[IsCallforprice],[IsUserentersprice],[Minimumamount],[Maximumamount],[IsBasepriceenabled],[AmountInProduct],[UnitOfProduct],[ReferenceAmount],[ReferenceUnit],[IsServices],[Weight],[Length],[Width],[Height],[IsFreeshipping],[IsShipseparately],[Additionalshippingcharge],[Deliverydate],[IsMultiplewarehouses],[IsDisplayavailability],[Displaystockquantity],[Minimumstockqty],[Lowstockactivity],[Notifyforqtybelow],[Backorders],[IsAllowbackinstocksubscriptions],[Minimumcartqty],[Maximumcartqty],[IsNotreturnable],[Allowedquantities],[Isgiftcard],[Giftcardtype],[Giftcardamount],[Isrental],[Rentalperiodlength],[Rentalperiod],[IsRecurringproduct],[Cyclelength],[Cycleperiod],[Totalcycles],[Searchenginefriendlypagename],[Metatitle],[Metadescription],[Metakeywords]) VALUES  ('" + model.ProductName.Trim() + "','" + model.ShortDescription + "','" + model.FullDescription + "','" + model.SKU + "','" + model.IsPublished + "','" + model.Price + "','" + model.IsTextexempt + "','" + model.Taxcategory + "','" + model.IsShippingEnable + "','" + model.InventoryMethod + "','" + model.StockQuantity + "','" + 1 + "','" + DateTime.Now + "','" + DateTime.Now + "','" + model.Producttags + "','" + model.GTIN + "','" + model.ManufacturerPartnumber + "','" + model.IsShowOnHomepage + "','" + model.Displayorder + "','" + model.ProductType + "','" + model.ProductTemplate + "','" + model.IsVisibleindividually + "','" + model.VendorID + "','" + model.IsAllowuserreviews + "','" + model.Availablestartdate + "','" + model.Availableenddate + "','" + model.IsMarkasnew + "','" + model.MarkasnewStartdate + "','" + model.MarkasnewEnddate + "','" + model.Admincomment + "','" + model.OldPrice + "','" + model.Productcost + "','" + model.IsDisablebuybutton + "','" + model.IsDisablewishlistbutton + "','" + model.IsAvailableforpreorder + "','" + model.Preorderavailabilitystartdate + "','" + model.IsCallforprice + "','" + model.IsUserentersprice + "','" + model.Minimumamount + "','" + model.Maximumamount + "','" + model.IsBasepriceenabled + "','" + model.Amountinproduct + "','" + model.Unitofproduct + "','" + model.Referenceamount + "','" + model.Referenceunit + "','" + model.IsServices + "','" + model.Weight + "','" + model.Length + "','" + model.Width + "','" + model.Height + "','" + model.IsFreeshipping + "','" + model.IsShipseparately + "','" + model.Additionalshippingcharge + "','" + model.DeliverydateID + "','" + model.IsMultiplewarehouses + "','" + model.IsDisplayavailability + "','" + model.Displaystockquantity + "','" + model.Minimumstockqty + "','" + model.Lowstockactivity + "','" + model.Notifyforqtybelow + "','" + model.Backorders + "','" + model.IsAllowbackinstocksubscriptions + "','" + model.Minimumcartqty + "','" + model.Maximumcartqty + "','" + model.IsNotreturnable + "','" + model.Allowedquantities + "','" + model.Isgiftcard + "','" + model.Giftcardtype + "','" + model.Giftcardamount + "','" + model.Isrental + "','" + model.Rentalperiodlength + "','" + model.Rentalperiod + "','" + model.IsRecurringproduct + "','" + model.Cyclelength + "','" + model.Cycleperiod + "','" + model.Totalcycles + "','" + model.Searchenginefriendlypagename + "','" + model.Metatitle + "','" + model.Metadescription + "','" + model.Metakeywords + "')";
//                            connection.Open();
//                            SqlCommand cmd = new SqlCommand(query, connection);
//                            if (cmd.ExecuteNonQuery() == 1)
//                            {
//                                connection.Close();
//                                string query1 = "select ProductID from [dbo].[Product] where ProductName='" + model.ProductName + "'";
//                                connection.Open();
//                                using (SqlCommand command = new SqlCommand(query1, connection))
//                                {
//                                    using (SqlDataReader reader = command.ExecuteReader())
//                                    {
//                                        while (reader.Read())
//                                        {
//                                            ProductID = Convert.ToInt32(reader["ProductID"].ToString());
//                                            connection.Close();
//                                            connection.Open();
//                                            if (ProductID > 0)
//                                            {
//                                                if (model.CategoryID != null)
//                                                {
//                                                    foreach (int CategoryID in model.CategoryID)
//                                                    {
//                                                        string query3 = "INSERT INTO [dbo].[CategoryDetail](ProductID, CategoryID)VALUES(" + ProductID + "," + CategoryID + ")";
//                                                        SqlCommand command1 = new SqlCommand(query3, connection);
//                                                        command1.ExecuteNonQuery();
//                                                    }
//                                                }
//                                                connection.Close();

//                                                ViewBag.SuccessMessage = "Added Product Successful";
//                                                return RedirectToAction("ProductList", "DemoProduct");
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                        else
//                        {
//                            ViewBag.SuccessMessage = "Product Name Already Exist.";
//                            return View("~/Plugins/Widgets.BasicPlugins/Views/AddProduct.cshtml", product);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ViewBag.SuccessMessage = ex.Message.ToString();
//                return View("~/Plugins/Widgets.BasicPlugins/Views/AddProduct.cshtml", product);
//            }
//            return View("~/Plugins/Widgets.BasicPlugins/Views/AddProduct.cshtml", product);
//        }
//        [HttpGet]
//        public ActionResult EditProduct(int id)
//        {
//            Product product = new();
//            GetAllMethods(product);
//            try
//            {
//                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";
//                using (SqlConnection connection = new SqlConnection(constr))
//                {
//                    connection.Open();

//                    string query = "select * from [dbo].[Product] where IsActive=1 and ProductID='" + id + "'";
//                    using (SqlCommand command = new SqlCommand(query, connection))
//                    {
//                        using (SqlDataReader reader = command.ExecuteReader())
//                        {
//                            while (reader.Read())
//                            {
//                                product.ProductName = reader["ProductName"].ToString();
//                                product.ProductID = Convert.ToInt32(reader["ProductID"].ToString());
//                                product.SKU = reader["SKU"].ToString();
//                                product.Price = reader["Price"].ToString() == "" ? 0 : Convert.ToDecimal(reader["Price"].ToString());
//                                product.IsPublished = Convert.ToBoolean(reader["IsPublished"].ToString()) == true ? true : false;
//                                product.IsTextexempt = Convert.ToBoolean(reader["IsTextexempt"].ToString()) == true ? true : false;
//                                product.Taxcategory = reader["Taxcategory"].ToString();
//                                product.IsShippingEnable = Convert.ToBoolean(reader["IsShippingEnabled"].ToString()) == true ? true : false;
//                                product.InventoryMethod = reader["InventoryMethod"].ToString();
//                                product.StockQuantity = Convert.ToInt32(reader["StockQuantity"].ToString());
//                                if (reader["FullDescription"].ToString() != "")
//                                    product.FullDescription = HttpUtility.UrlDecode(reader["FullDescription"].ToString());
//                                else
//                                    product.FullDescription = "";
//                                if (reader["ShortDescription"].ToString() != "")
//                                    product.ShortDescription = HttpUtility.UrlDecode(reader["ShortDescription"].ToString());
//                                else
//                                    product.ShortDescription = "";
//                            }
//                            connection.Close();

//                            string query1 = "select STUFF((SELECT  ', ' + CAST(t1.CategoryID AS varchar) from CategoryDetail t1 where t.ProductID = t1.ProductID FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)') ,1,2,'') as CategoryID from Product t where t.ProductID=" + id + "";
//                            connection.Open();
//                            using (SqlCommand command1 = new SqlCommand(query1, connection))
//                            {
//                                using (SqlDataReader reader1 = command1.ExecuteReader())
//                                {
//                                    while (reader1.Read())
//                                    {
//                                        var s = reader1["CategoryID"].ToString();
//                                        if (s != "")
//                                        {
//                                            var split = s.Split(", ");
//                                            product.CategoryID = Array.ConvertAll(split, int.Parse);
//                                        }
//                                    }
//                                }
//                            }
//                            connection.Close();
//                            string Iquery = "select * from [dbo].[ImageDetail] where ProductID='" + id + "'";
//                            using (SqlCommand commands = new SqlCommand(Iquery, connection))
//                            {
//                                connection.Open();
//                                List<UploadFileModel> UploadFiles = new List<UploadFileModel>();
//                                using (SqlDataReader readers = commands.ExecuteReader())
//                                {
//                                    while (readers.Read())
//                                    {
//                                        UploadFiles.Add(new UploadFileModel()
//                                        {
//                                            ImageID = Convert.ToInt32(readers["ImageID"].ToString()),
//                                            AltImage = readers["Alt"].ToString(),
//                                            ProductID = readers["ProductID"].ToString(),
//                                            Title = readers["Title"].ToString(),
//                                            Displayorder = readers["DisplayOrder"].ToString(),
//                                            sFile = RetrieveImage((byte[])readers["ImageData"])

//                                        });


//                                    }
//                                    product.UploadFileModel = UploadFiles;
//                                }
//                            }
//                            connection.Close();
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ViewBag.SuccessMessage = ex.Message.ToString();
//                return View("~/Plugins/Widgets.BasicPlugins/Views/EditProduct.cshtml", product);
//            }
//            return View("~/Plugins/Widgets.BasicPlugins/Views/EditProduct.cshtml", product);
//        }
//        [HttpPost]
//        public ActionResult EditProduct(Product product)
//        {
//            GetAllMethods(product);
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    ViewBag.SuccessMessage = "Enter required fields.";
//                    return View("~/Plugins/Widgets.BasicPlugins/Views/EditProduct.cshtml", product);
//                }
//                else
//                {
//                    int ProductID = 0;
//                    int Count = 0;
//                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
//                    {
//                        connection.Open();
//                        string q = "select Count(*) as Count from [dbo].[Product] where ProductName='" + product.ProductName + "'and ProductID!='" + product.ProductID + "'";

//                        using (SqlCommand command = new SqlCommand(q, connection))
//                        {
//                            using (SqlDataReader r = command.ExecuteReader())
//                            {
//                                while (r.Read())
//                                {
//                                    Count = Convert.ToInt32(r["Count"].ToString());
//                                }
//                            }
//                        }
//                        if (Count == 0)
//                        {
//                            if (product.FullDescription != null)
//                                product.FullDescription = HttpUtility.UrlEncode(product.FullDescription);
//                            if (product.ShortDescription != null)
//                                product.ShortDescription = HttpUtility.UrlEncode(product.ShortDescription);
//                            string query = "Update Product set ProductName='" + product.ProductName + "',ShortDescription='" + product.ShortDescription + "',FullDescription='" + product.FullDescription + "',SKU='" + product.SKU + "',IsPublished='" + product.IsPublished + "',Price='" + product.Price + "',IsTextexempt='" + product.IsTextexempt + "',Taxcategory='" + product.Taxcategory + "',IsShippingEnabled='" + product.IsShippingEnable + "',InventoryMethod='" + product.InventoryMethod + "',StockQuantity='" + product.StockQuantity + "',IsActive=1,UpdatedDate=GETDATE() where ProductID='" + product.ProductID + "'";
//                            SqlCommand cmd = new SqlCommand(query, connection);
//                            if (cmd.ExecuteNonQuery() == 1)
//                            {
//                                string query1 = "select ProductID from [dbo].[Product] where ProductName='" + product.ProductName + "'";

//                                using (SqlCommand command = new SqlCommand(query1, connection))
//                                {
//                                    using (SqlDataReader reader = command.ExecuteReader())
//                                    {
//                                        while (reader.Read())
//                                        {
//                                            ProductID = Convert.ToInt32(reader["ProductID"].ToString());
//                                            connection.Close();
//                                            connection.Open();
//                                            if (ProductID > 0)
//                                            {

//                                                string query4 = "Delete from CategoryDetail where ProductID='" + ProductID + "'";
//                                                SqlCommand command4 = new SqlCommand(query4, connection);
//                                                command4.ExecuteNonQuery();
//                                                connection.Close();
//                                                connection.Open();
//                                                foreach (int CategoryID in product.CategoryID)
//                                                {
//                                                    string query3 = "INSERT INTO [dbo].[CategoryDetail](ProductID, CategoryID)VALUES(" + ProductID + "," + CategoryID + ")";
//                                                    SqlCommand command1 = new SqlCommand(query3, connection);
//                                                    command1.ExecuteNonQuery();
//                                                }

//                                                connection.Close();

//                                                ViewBag.SuccessMessage = "Added Product Successful";
//                                                return RedirectToAction("ProductList", "DemoProduct");

//                                            }

//                                        }
//                                    }
//                                }


//                            }
//                        }
//                        else
//                        {

//                            ViewBag.SuccessMessage = "Product Name Already Exist.";
//                            return View("~/Plugins/Widgets.BasicPlugins/Views/AddProduct.cshtml", product);
//                        }

//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                ViewBag.SuccessMessage = ex.Message.ToString();
//                return View("~/Plugins/Widgets.BasicPlugins/Views/EditProduct.cshtml", product);
//            }
//            return View("~/Plugins/Widgets.BasicPlugins/Views/EditProduct.cshtml", product);
//        }
//        [HttpGet]
//        public ActionResult DeleteProduct(int id)
//        {
//            try
//            {
//                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
//                {
//                    connection.Open();
//                    string query = "Update Product set IsActive=0 where ProductID='" + id + "'";
//                    SqlCommand cmd = new SqlCommand(query, connection);
//                    if (cmd.ExecuteNonQuery() == 1)
//                    {
//                        ViewBag.SuccessMessage = "Product Deleted Successfully.";
//                        return RedirectToAction("ProductList", "DemoProduct");
//                    }
//                    connection.Close();
//                }
//            }
//            catch (Exception ex)
//            {
//                ViewBag.SuccessMessage = ex.Message.ToString();
//                return RedirectToAction("ProductList", "DemoProduct");
//            }

//            return RedirectToAction("ProductList", "DemoProduct");
//        }
//        [HttpGet]
//        public ActionResult DeleteImage(int id, int ProductID)
//        {
//            List<UploadFileModel> imageList = new();
//            using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
//            {

//                string query = "DELETE FROM [dbo].[ImageDetail] WHERE ImageID='" + id + "'";
//                connection.Open();
//                SqlCommand cmd = new SqlCommand(query, connection);
//                if (cmd.ExecuteNonQuery() == 1)
//                {
//                    connection.Close();
//                    string Iquery = "select * from [dbo].[ImageDetail] where ProductID='" + ProductID + "'";
//                    using (SqlCommand command = new SqlCommand(Iquery, connection))
//                    {
//                        connection.Open();
//                        using (SqlDataReader reader = command.ExecuteReader())
//                        {
//                            while (reader.Read())
//                            {
//                                imageList.Add(new UploadFileModel()
//                                {
//                                    ImageID = Convert.ToInt32(reader["ImageID"].ToString()),
//                                    AltImage = reader["Alt"].ToString(),
//                                    ProductID = reader["ProductID"].ToString(),
//                                    Title = reader["Title"].ToString(),
//                                    Displayorder = reader["DisplayOrder"].ToString(),
//                                    sFile = RetrieveImage((byte[])reader["ImageData"]),

//                                });


//                            }
//                        }
//                        connection.Close();
//                    }
//                    return Json(new { data = imageList, status = "success" });

//                }
//            }
//            return Json(new { data = imageList, status = "error" });
//        }

//        [HttpPost]
//        public ActionResult UploadImage(UploadFileModel model)
//        {
//            try
//            {
//                string status = "";
//                List<UploadFileModel> imageList = new();
//                if (model.File != null)
//                {
//                    MemoryStream ms = new MemoryStream();
//                    model.File.CopyTo(ms);
//                    var ImageData = ms.ToArray();
//                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
//                    {
//                        if (model.AltImage == null)
//                            model.AltImage = "";
//                        if (model.Title == null)
//                            model.Title = "";
//                        if (model.Displayorder == null)
//                            model.Displayorder = "";
//                        //string query = "INSERT INTO [dbo].[ImageDetail] (ProductID, ImageData, Alt,Title,DisplayOrder) VALUES  ('" + model.ProductID + "','" + ImageData + "','" + model.AltImage + "','" + model.Title + "','" + model.Displayorder + "')";
//                        string query = "INSERT INTO [dbo].[ImageDetail] (ProductID, ImageData, Alt,Title,DisplayOrder) VALUES  (@ProductID,@ImageData,@Alt,@Title,@DisplayOrder)";
//                        connection.Open();
//                        SqlCommand cmd = new SqlCommand(query, connection);
//                        cmd.Parameters.AddWithValue("@ProductID", model.ProductID);
//                        cmd.Parameters.AddWithValue("@Alt", model.AltImage);
//                        cmd.Parameters.AddWithValue("@Title", model.Title);
//                        cmd.Parameters.AddWithValue("@DisplayOrder", model.Displayorder);
//                        cmd.Parameters.AddWithValue("@ImageData", ImageData);
//                        if (cmd.ExecuteNonQuery() == 1)
//                        {
//                            connection.Close();
//                            string Iquery = "select * from [dbo].[ImageDetail] where ProductID='" + model.ProductID + "'";
//                            using (SqlCommand command = new SqlCommand(Iquery, connection))
//                            {
//                                connection.Open();
//                                using (SqlDataReader reader = command.ExecuteReader())
//                                {
//                                    while (reader.Read())
//                                    {
//                                        imageList.Add(new UploadFileModel()
//                                        {
//                                            ImageID = Convert.ToInt32(reader["ImageID"].ToString()),
//                                            AltImage = reader["Alt"].ToString(),
//                                            ProductID = reader["ProductID"].ToString(),
//                                            Title = reader["Title"].ToString(),
//                                            Displayorder = reader["DisplayOrder"].ToString(),
//                                            sFile = RetrieveImage((byte[])reader["ImageData"]),

//                                        });
                                        
//                                    }
//                                }
//                                connection.Close();
//                            }
//                            return Json(new { data = imageList, status = "success" });

//                        }
//                    }
//                }


//            }
//            catch (WebException ex)
//            {
//                return Json("");

//            }
//            return Json("");
//        }
//        public string RetrieveImage(byte[] ImageData)
//        {
//            string imageBase64Data =
//        Convert.ToBase64String(ImageData);
//            string imageDataURL =
//        string.Format("data:image/jpg;base64,{0}",
//        imageBase64Data);
//            ViewBag.ImageDataUrl = imageDataURL;
//            return imageDataURL;
//        }
//    }
//}