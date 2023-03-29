
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Nop.Core;
using Nop.Plugin.Widgets.BasicPlugins.Models;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using static ClosedXML.Excel.XLPredefinedFormat;
using DateTime = System.DateTime;
using Path = System.IO.Path;

namespace Nop.Plugin.Widgets.BasicPlugins.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class DemoProductController : BasePluginController
    {

        [HttpGet]
        public ActionResult ProductList()
        {
            List<Product> Products = new();
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";

                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    string Query = "";
                    string Query1 = "";
                    string Query2 = "";
                    string Query3 = "";

                    string Q = "SELECT P.ProductID,P.ProductName,P.SKU,P.Price,P.StockQuantity,P.IsPublished,P.CreatedDate,I.ImageData FROM [Product] P LEFT JOIN ImageDetail I ON P.ProductID = I.ProductID" + Query + " WHERE P.IsActive=1 " + Query1 + Query2 + Query3 + " and I.ImageID IN (SELECT MAX(ImageID) FROM ImageDetail GROUP BY ProductID) Union SELECT * FROM (SELECT P.ProductID,P.ProductName,P.SKU,P.Price,P.StockQuantity,P.IsPublished,P.CreatedDate,I.ImageData FROM [Product] P LEFT JOIN ImageDetail I ON P.ProductID = I.ProductID " + Query + " WHERE P.IsActive=1 " + Query1 + Query2 + Query3 + ") as X WHERE X.ImageData is null";
                    using (SqlCommand command = new SqlCommand(Q, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Products.Add(new Product
                                {
                                    SFile = reader["ImageData"].ToString() != "" ? RetrieveImage((byte[])reader["ImageData"]) : null,
                                    ProductName = reader["ProductName"].ToString(),
                                    ProductID = Convert.ToInt32(reader["ProductID"].ToString()),
                                    SKU = reader["SKU"].ToString(),
                                    SPrice = reader["Price"].ToString().Replace(".0000", "") + " USD",
                                    FPrice = Convert.ToSingle(reader["Price"].ToString()),
                                    IsPublished = Convert.ToBoolean(reader["IsPublished"].ToString()) == true ? true : false,
                                    StockQuantity = Convert.ToInt32(reader["StockQuantity"].ToString()),
                                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                                });

                            }
                        }
                    }
                }
                return View("~/Plugins/Widgets.BasicPlugins/Views/Product/ProductList.cshtml", Products);
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/Product/ProductList.cshtml", Products);
            }
        }
        [HttpGet]
        public ActionResult Search(string productName, string SearchCategoryId, string SearchPublishedId)
        {
            List<Product> Products = new();
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";

                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    string Query = "";
                    string Query1 = "";
                    string Query2 = "";
                    string Query3 = "";
                    if (SearchCategoryId != "0" && SearchCategoryId != null && SearchCategoryId != "")
                    {
                        Query = " INNER JOIN [dbo].[CategoryDetail] as C on C.ProductID = P.ProductID";
                    }
                    if (SearchCategoryId != "0" && SearchCategoryId != null && SearchCategoryId != "")
                    {
                        int Icategory = Convert.ToInt32(SearchCategoryId);
                        Query1 = " AND C.CategoryID =" + Icategory + "";
                    }
                    if (productName != "" && productName != null)
                    {
                        Query2 = " AND P.ProductName LIKE '%" + productName + "%'";
                    }
                    if (SearchPublishedId != "All")
                    {
                        int Ipublished = Convert.ToInt32(SearchPublishedId);
                        Query3 = " AND P.IsPublished =" + Ipublished + "";
                    }
                    string Q = "SELECT P.ProductID,P.ProductName,P.SKU,P.Price,P.StockQuantity,P.IsPublished,P.CreatedDate,I.ImageData FROM [Product] P LEFT JOIN ImageDetail I ON P.ProductID = I.ProductID" + Query + " WHERE P.IsActive=1 " + Query1 + Query2 + Query3 + " and I.ImageID IN (SELECT MAX(ImageID) FROM ImageDetail GROUP BY ProductID) Union SELECT * FROM (SELECT P.ProductID,P.ProductName,P.SKU,P.Price,P.StockQuantity,P.IsPublished,P.CreatedDate,I.ImageData FROM [Product] P LEFT JOIN ImageDetail I ON P.ProductID = I.ProductID " + Query + " WHERE P.IsActive=1 " + Query1 + Query2 + Query3 + ") as X WHERE X.ImageData is null";
                    using (SqlCommand command = new SqlCommand(Q, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Products.Add(new Product
                                {
                                    SFile = reader["ImageData"].ToString() != "" ? RetrieveImage((byte[])reader["ImageData"]) : "https://cafe.shopfast.net/images/thumbs/default-image_100.png",
                                    ProductName = reader["ProductName"].ToString(),
                                    ProductID = Convert.ToInt32(reader["ProductID"].ToString()),
                                    SKU = reader["SKU"].ToString(),
                                    SPrice = reader["Price"].ToString().Replace(".0000", "") + " USD",
                                    FPrice = Convert.ToSingle(reader["Price"].ToString()),
                                    IsPublished = Convert.ToBoolean(reader["IsPublished"].ToString()) == true ? true : false,
                                    StockQuantity = Convert.ToInt32(reader["StockQuantity"].ToString()),
                                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                                });

                            }
                        }
                    }
                }

                return Json(new { data = Products, status = "success" });
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return Json(new { data = Products, status = "error" });
            }

        }
        [HttpGet]
        public ActionResult AddProduct(int id = 0)
        {
            Product product = new();
            GetAllMethods(product);
            return View("~/Plugins/Widgets.BasicPlugins/Views/Product/AddProduct.cshtml", product);
        }

        private void GetAllMethods(Product product)
        {
            product.Categories = new[]
          {
        new SelectListItem { Value = "1", Text = "Lunch & Dinner" },
        new SelectListItem { Value = "2", Text = "Breakfast & Brunch" },
        new SelectListItem { Value = "3", Text = "Baked Goods & Desserts" },
        new SelectListItem { Value = "4", Text = "Drinks" },
             };
            product.TaxCategories = new[]
            {
        new SelectListItem { Value = "0", Text = "[None]" },
        new SelectListItem { Value = "1", Text = "Food" },
        new SelectListItem { Value = "2", Text = "Bevarage" },

             };
            product.InventoryMethods = new[]
            {
        new SelectListItem { Value = "0", Text = "Don't track inventory" },
        new SelectListItem { Value = "1", Text = "Track inventory" },
        new SelectListItem { Value = "2", Text = "Track inventory by product attributes" },

             };
            product.ProductTypes = new[]
            {
        new SelectListItem { Value = "1", Text = "Simple" },
        new SelectListItem { Value = "2", Text = "Grouped(product with variants)" }

             };
            product.ProductTemplates = new[]
           {
        new SelectListItem { Value = "1", Text = "Simple product" },
        new SelectListItem { Value = "2", Text = "Product Weight" }

             };
            product.Unitofproducts = new[]
           {
        new SelectListItem { Value = "1", Text = "ounce(s)" },
        new SelectListItem { Value = "2", Text = "lb(s)" },
        new SelectListItem { Value = "3", Text = "kg(s)" },
        new SelectListItem { Value = "4", Text = "gram(s)" }

             };
            product.Referenceunits = new[]
           {
        new SelectListItem { Value = "1", Text = "ounce(s)" },
        new SelectListItem { Value = "2", Text = "lb(s)" },
        new SelectListItem { Value = "3", Text = "kg(s)" },
        new SelectListItem { Value = "4", Text = "gram(s)" }

             };
            product.Discounts = new[]
          {
        new SelectListItem { Value = "1", Text = "Simple" },
        new SelectListItem { Value = "2", Text = "Sample discount with coupon code" }

             };
            product.Deliverydates = new[]
          {
        new SelectListItem { Value = "0", Text = "None" },
        new SelectListItem { Value = "1", Text = "1-2 days" },
        new SelectListItem { Value = "2", Text = "3-5 days" },
        new SelectListItem { Value = "3", Text = "1 week" }

             };
            product.Warehouses = new[]
         {
        new SelectListItem { Value = "0", Text = "None" }

             };
            product.Lowstockactivities = new[]
          {
        new SelectListItem { Value = "0", Text = "Nothing" },
        new SelectListItem { Value = "1", Text = "Disable buy button" },
        new SelectListItem { Value = "2", Text = "Unpublish" }

             };
            product.BackordersList = new[]
         {
        new SelectListItem { Value = "0", Text = "No backorders" },
        new SelectListItem { Value = "1", Text = "Allow qty below 0" },
        new SelectListItem { Value = "2", Text = "Allow qty below 0 and notify user" }

             };
            product.GiftcardtypeList = new[]
        {
        new SelectListItem { Value = "1", Text = "Virtual" },
        new SelectListItem { Value = "2", Text = "Physical" }

             };
            product.RentalperiodList = new[]
           {
                    new SelectListItem { Value = "0", Text = "Days" },
                    new SelectListItem { Value = "1", Text = "Weeks" },
                    new SelectListItem { Value = "2", Text = "Months" },
                    new SelectListItem { Value = "3", Text = "Years" },
            };
            product.CycleperiodList = new[]
          {
                    new SelectListItem { Value = "0", Text = "Days" },
                    new SelectListItem { Value = "1", Text = "Weeks" },
                    new SelectListItem { Value = "2", Text = "Months" },
                    new SelectListItem { Value = "3", Text = "Years" },
            };
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(Product model, string Command)
        {
            Product product = new();
            GetAllMethods(product);
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Plugins/Widgets.BasicPlugins/Views/Product/AddProduct.cshtml", product);
                }
                else
                {
                    ViewBag.SuccessMessage = "";
                    int ProductID = 0;
                    int Count = 0;
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string q = "select Count(*) as Count from [dbo].[Product] where IsActive=1 and ProductName='" + model.ProductName.Trim() + "'";
                        using (SqlCommand command = new SqlCommand(q, connection))
                        {
                            using (SqlDataReader r = command.ExecuteReader())
                            {
                                while (r.Read())
                                {
                                    Count = Convert.ToInt32(r["Count"].ToString());
                                }
                            }
                        }
                        connection.Close();
                        if (Count == 0)
                        {
                            if (model.FullDescription != null)
                                model.FullDescription = HttpUtility.UrlEncode(model.FullDescription);
                            if (product.ShortDescription != null)
                                product.ShortDescription = HttpUtility.UrlEncode(product.ShortDescription);

                            string query = "INSERT INTO [Product] (ProductName, ShortDescription, FullDescription,SKU,IsPublished,Price,IsTextexempt,Taxcategory,IsShippingEnabled,InventoryMethod,StockQuantity,IsActive,CreatedDate,UpdatedDate) VALUES  ('" + model.ProductName.Trim() + "','" + model.ShortDescription + "','" + model.FullDescription + "','" + model.SKU + "','" + model.IsPublished + "','" + model.Price + "','" + model.IsTextexempt + "','" + model.Taxcategory + "','" + model.IsShippingEnable + "','" + model.InventoryMethod + "','" + model.StockQuantity + "','" + 1 + "','" + DateTime.Now + "','" + DateTime.Now + "')";
                            connection.Open();
                            SqlCommand cmd = new SqlCommand(query, connection);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                connection.Close();
                                string query1 = "select ProductID from [dbo].[Product] where IsActive=1 and ProductName='" + model.ProductName.Trim() + "'";
                                connection.Open();
                                using (SqlCommand command = new SqlCommand(query1, connection))
                                {
                                    using (SqlDataReader reader = command.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            ProductID = Convert.ToInt32(reader["ProductID"].ToString());
                                            connection.Close();
                                            connection.Open();
                                            if (ProductID > 0)
                                            {
                                                if (model.CategoryID != null)
                                                {
                                                    foreach (int CategoryID in model.CategoryID)
                                                    {
                                                        string query3 = "INSERT INTO [dbo].[CategoryDetail](ProductID, CategoryID)VALUES(" + ProductID + "," + CategoryID + ")";
                                                        SqlCommand command1 = new SqlCommand(query3, connection);
                                                        command1.ExecuteNonQuery();
                                                    }
                                                }


                                                ViewBag.SuccessMessage = "Added Product Successful";
                                                if (Command == "save")
                                                    return RedirectToAction("ProductList", "DemoProduct");
                                                else
                                                {
                                                    return RedirectToAction("EditProduct", new { id = ProductID });
                                                }
                                            }
                                            connection.Close();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            ViewBag.SuccessMessage = "Name already exist.";
                            return View("~/Plugins/Widgets.BasicPlugins/Views/Product/AddProduct.cshtml", product);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/Product/AddProduct.cshtml", product);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/Product/AddProduct.cshtml", product);
        }
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            Product product = new();
            GetAllMethods(product);
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    string query = "select * from [dbo].[Product] where IsActive=1 and ProductID='" + id + "'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                product.ProductName = reader["ProductName"].ToString();
                                product.ProductID = Convert.ToInt32(reader["ProductID"].ToString());
                                product.SKU = reader["SKU"].ToString();
                                product.Price = reader["Price"].ToString() == "" ? 0 : Convert.ToDecimal(reader["Price"].ToString());
                                product.IsPublished = Convert.ToBoolean(reader["IsPublished"].ToString()) == true ? true : false;
                                product.IsTextexempt = Convert.ToBoolean(reader["IsTextexempt"].ToString()) == true ? true : false;
                                product.Taxcategory = reader["Taxcategory"].ToString();
                                product.IsShippingEnable = Convert.ToBoolean(reader["IsShippingEnabled"].ToString()) == true ? true : false;
                                product.InventoryMethod = reader["InventoryMethod"].ToString();
                                product.StockQuantity = Convert.ToInt32(reader["StockQuantity"].ToString());
                                if (reader["FullDescription"].ToString() != "")
                                    product.FullDescription = HttpUtility.UrlDecode(reader["FullDescription"].ToString());
                                else
                                    product.FullDescription = "";
                                if (reader["ShortDescription"].ToString() != "")
                                    product.ShortDescription = HttpUtility.UrlDecode(reader["ShortDescription"].ToString());
                                else
                                    product.ShortDescription = "";
                            }
                            connection.Close();

                            string query1 = "select STUFF((SELECT  ', ' + CAST(t1.CategoryID AS varchar) from CategoryDetail t1 where t.ProductID = t1.ProductID FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)') ,1,2,'') as CategoryID from Product t where t.ProductID=" + id + "";
                            connection.Open();
                            using (SqlCommand command1 = new SqlCommand(query1, connection))
                            {
                                using (SqlDataReader reader1 = command1.ExecuteReader())
                                {
                                    while (reader1.Read())
                                    {
                                        var s = reader1["CategoryID"].ToString();
                                        if (s != "")
                                        {
                                            var split = s.Split(", ");
                                            product.CategoryID = Array.ConvertAll(split, int.Parse);
                                        }
                                    }
                                }
                            }
                            connection.Close();
                            string Iquery = "select * from [dbo].[ImageDetail] where ProductID='" + id + "'";
                            using (SqlCommand commands = new SqlCommand(Iquery, connection))
                            {
                                connection.Open();
                                List<UploadFileModel> UploadFiles = new List<UploadFileModel>();
                                using (SqlDataReader readers = commands.ExecuteReader())
                                {
                                    while (readers.Read())
                                    {
                                        UploadFiles.Add(new UploadFileModel()
                                        {
                                            ImageID = Convert.ToInt32(readers["ImageID"].ToString()),
                                            AltImage = readers["Alt"].ToString(),
                                            ProductID = readers["ProductID"].ToString(),
                                            Title = readers["Title"].ToString(),
                                            Displayorder = readers["DisplayOrder"].ToString(),
                                            sFile = RetrieveImage((byte[])readers["ImageData"])

                                        });


                                    }
                                    product.UploadFileModel = UploadFiles;
                                }
                            }
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/Product/EditProduct.cshtml", product);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/Product/EditProduct.cshtml", product);
        }
        [HttpPost]
        public ActionResult EditProduct(Product product, string Command)
        {
            if (Command == "delete")
            {
                return RedirectToAction("Delete", new { id = product.ProductID });
            }
            GetAllMethods(product);
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.SuccessMessage = "Enter required fields.";
                    return View("~/Plugins/Widgets.BasicPlugins/Views/Product/EditProduct.cshtml", product);
                }
                else
                {
                    int ProductID = 0;
                    int Count = 0;
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string q = "select Count(*) as Count from [dbo].[Product] where IsActive=1 and ProductName='" + product.ProductName.Trim() + "'and ProductID!='" + product.ProductID + "'";

                        using (SqlCommand command = new SqlCommand(q, connection))
                        {
                            using (SqlDataReader r = command.ExecuteReader())
                            {
                                while (r.Read())
                                {
                                    Count = Convert.ToInt32(r["Count"].ToString());
                                }
                            }
                        }
                        if (Count == 0)
                        {
                            if (product.FullDescription != null)
                                product.FullDescription = HttpUtility.UrlEncode(product.FullDescription);
                            if (product.ShortDescription != null)
                                product.ShortDescription = HttpUtility.UrlEncode(product.ShortDescription);
                            string query = "Update Product set ProductName='" + product.ProductName.Trim() + "',ShortDescription='" + product.ShortDescription + "',FullDescription='" + product.FullDescription + "',SKU='" + product.SKU + "',IsPublished='" + product.IsPublished + "',Price='" + product.Price + "',IsTextexempt='" + product.IsTextexempt + "',Taxcategory='" + product.Taxcategory + "',IsShippingEnabled='" + product.IsShippingEnable + "',InventoryMethod='" + product.InventoryMethod + "',StockQuantity='" + product.StockQuantity + "',IsActive=1,UpdatedDate=GETDATE() where ProductID='" + product.ProductID + "'";
                            SqlCommand cmd = new SqlCommand(query, connection);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                string query1 = "select ProductID from [dbo].[Product] where IsActive=1 and ProductName='" + product.ProductName.Trim() + "'";

                                using (SqlCommand command = new SqlCommand(query1, connection))
                                {
                                    using (SqlDataReader reader = command.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            ProductID = Convert.ToInt32(reader["ProductID"].ToString());
                                            connection.Close();
                                            connection.Open();
                                            if (ProductID > 0)
                                            {

                                                string query4 = "Delete from CategoryDetail where ProductID='" + ProductID + "'";
                                                SqlCommand command4 = new SqlCommand(query4, connection);
                                                command4.ExecuteNonQuery();
                                                connection.Close();
                                                connection.Open();
                                                foreach (int CategoryID in product.CategoryID)
                                                {
                                                    string query3 = "INSERT INTO [dbo].[CategoryDetail](ProductID, CategoryID)VALUES(" + ProductID + "," + CategoryID + ")";
                                                    SqlCommand command1 = new SqlCommand(query3, connection);
                                                    command1.ExecuteNonQuery();
                                                }

                                                connection.Close();

                                                ViewBag.SuccessMessage = "Added Product Successful";
                                                if (Command == "save")
                                                    return RedirectToAction("ProductList", "DemoProduct");
                                                else
                                                {
                                                    return RedirectToAction("EditProduct", new { id = ProductID });
                                                }

                                            }

                                        }
                                    }
                                }


                            }
                        }
                        else
                        {

                            ViewBag.SuccessMessage = "Name already exist.";
                            return View("~/Plugins/Widgets.BasicPlugins/Views/Product/AddProduct.cshtml", product);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/Product/EditProduct.cshtml", product);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/Product/EditProduct.cshtml", product);
        }
        [HttpGet]
        public ActionResult DeleteProduct(string data)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                {
                    connection.Open();
                    foreach (var id in data.Split(","))
                    {
                        string query = "Update Product set IsActive=0 where ProductID='" + id + "'";
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                    return Json(new { status = "success" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "error" });
            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                {
                    connection.Open();
                    string query = "Update Product set IsActive=0 where ProductID='" + id + "'";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        ViewBag.SuccessMessage = "Product Deleted Successfully.";
                        return RedirectToAction("ProductList", "DemoProduct");
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return RedirectToAction("ProductList", "DemoProduct");
            }

            return RedirectToAction("ProductList", "DemoProduct");
        }
        [HttpGet]
        public ActionResult DeleteImage(int id, int ProductID)
        {
            List<UploadFileModel> imageList = new();
            using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
            {

                string query = "DELETE FROM [dbo].[ImageDetail] WHERE ImageID='" + id + "'";
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                if (cmd.ExecuteNonQuery() == 1)
                {
                    connection.Close();
                    string Iquery = "select * from [dbo].[ImageDetail] where ProductID='" + ProductID + "'";
                    using (SqlCommand command = new SqlCommand(Iquery, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                imageList.Add(new UploadFileModel()
                                {
                                    ImageID = Convert.ToInt32(reader["ImageID"].ToString()),
                                    AltImage = reader["Alt"].ToString(),
                                    ProductID = reader["ProductID"].ToString(),
                                    Title = reader["Title"].ToString(),
                                    Displayorder = reader["DisplayOrder"].ToString(),
                                    sFile = RetrieveImage((byte[])reader["ImageData"]),

                                });


                            }
                        }
                        connection.Close();
                    }
                    return Json(new { data = imageList, status = "success" });

                }
            }
            return Json(new { data = imageList, status = "error" });
        }

        [HttpPost]
        public ActionResult UploadImage(UploadFileModel model)
        {
            try
            {
                string status = "";
                List<UploadFileModel> imageList = new();
                if (model.File != null)
                {
                    MemoryStream ms = new MemoryStream();
                    model.File.CopyTo(ms);
                    var ImageData = ms.ToArray();
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        if (model.AltImage == null)
                            model.AltImage = "";
                        if (model.Title == null)
                            model.Title = "";
                        if (model.Displayorder == null)
                            model.Displayorder = "";
                        //string query = "INSERT INTO [dbo].[ImageDetail] (ProductID, ImageData, Alt,Title,DisplayOrder) VALUES  ('" + model.ProductID + "','" + ImageData + "','" + model.AltImage + "','" + model.Title + "','" + model.Displayorder + "')";
                        string query = "INSERT INTO [dbo].[ImageDetail] (ProductID, ImageData, Alt,Title,DisplayOrder) VALUES  (@ProductID,@ImageData,@Alt,@Title,@DisplayOrder)";
                        connection.Open();
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@ProductID", model.ProductID);
                        cmd.Parameters.AddWithValue("@Alt", model.AltImage);
                        cmd.Parameters.AddWithValue("@Title", model.Title);
                        cmd.Parameters.AddWithValue("@DisplayOrder", model.Displayorder);
                        cmd.Parameters.AddWithValue("@ImageData", ImageData);
                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            connection.Close();
                            string Iquery = "select * from [dbo].[ImageDetail] where ProductID='" + model.ProductID + "'";
                            using (SqlCommand command = new SqlCommand(Iquery, connection))
                            {
                                connection.Open();
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        imageList.Add(new UploadFileModel()
                                        {
                                            ImageID = Convert.ToInt32(reader["ImageID"].ToString()),
                                            AltImage = reader["Alt"].ToString(),
                                            ProductID = reader["ProductID"].ToString(),
                                            Title = reader["Title"].ToString(),
                                            Displayorder = reader["DisplayOrder"].ToString(),
                                            sFile = RetrieveImage((byte[])reader["ImageData"]),

                                        });

                                    }
                                }
                                connection.Close();
                            }
                            return Json(new { data = imageList, status = "success" });

                        }
                    }
                }


            }
            catch (WebException ex)
            {
                return Json("");

            }
            return Json("");
        }
        public string RetrieveImage(byte[] ImageData)
        {
            string imageBase64Data =
        Convert.ToBase64String(ImageData);
            string imageDataURL =
        string.Format("data:image/jpg;base64,{0}",
        imageBase64Data);
            ViewBag.ImageDataUrl = imageDataURL;
            return imageDataURL;
        }
    }
}