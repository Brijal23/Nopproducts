using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using MaxMind.GeoIP2.Model;
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
using Path = System.IO.Path;

namespace Nop.Plugin.Widgets.BasicPlugins.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class DemoProductController : BasePluginController
    {

        [HttpGet]
        public ActionResult ProductList(string productName = "", string published = "All", string category = "0")
        {
            List<Product> Products = new();
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";

                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    string Query = "SELECT * FROM [dbo].[Product] AS P ";
                    if (category != "0")
                    {

                        Query += " INNER JOIN [dbo].[CategoryDetail] as C on C.ProductID = P.ProductID";
                    }
                    Query += " WHERE P.IsActive = 1";
                    if (category != "0")
                    {
                        int Icategory = Convert.ToInt32(category);
                        Query += " AND C.CategoryID =" + Icategory + "";
                    }
                    if (productName != "")
                    {
                        Query += " AND P.ProductName LIKE '%" + productName + "%'";
                    }
                    if (published != "All")
                    {
                        int Ipublished = Convert.ToInt32(published);
                        Query += " AND P.IsPublished =" + Ipublished + "";
                    }
                    Query += " ORDER BY CreatedDate DESC";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Products.Add(new Product
                                {
                                    ProductName = reader["ProductName"].ToString(),
                                    ProductID = Convert.ToInt32(reader["ProductID"].ToString()),
                                    SKU = reader["SKU"].ToString(),
                                    SPrice = reader["Price"].ToString().Replace(".00", ""),
                                    IsPublished = Convert.ToBoolean(reader["IsPublished"].ToString()) == true ? true : false,
                                    StockQuantity = Convert.ToInt32(reader["StockQuantity"].ToString()),
                                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"].ToString())
                                });

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();


                return View("~/Plugins/Widgets.BasicPlugins/Views/ProductList.cshtml", Products);

            }
            ViewBag.productnamesearch = productName;
            ViewBag.publishedsearch = published;
            ViewBag.categorysearch = category;
            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductList.cshtml", Products);
        }
        [HttpGet]
        public ActionResult AddProduct(int id = 0)
        {
            Product product = new();
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
            return View("~/Plugins/Widgets.BasicPlugins/Views/AddProduct.cshtml", product);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(Product model)
        {
            Product product = new();
            product.Categories = new[]
            {
                    new SelectListItem { Value = "1", Text = "Lunch & Dinner" },
                    new SelectListItem { Value = "2", Text = "Breakfast & Brunch" },
                    new SelectListItem { Value = "3", Text = "Baked Goods & Desserts" },
                    new SelectListItem { Value = "4", Text = "Drinks" },
                         };
            // model.Categories = new MultiSelectList(categories, "Value", "Text");
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
            try
            {
                if (!ModelState.IsValid)
                {
                    //ViewBag.SuccessMessage = "Enter required fields.";
                    return View("~/Plugins/Widgets.BasicPlugins/Views/AddProduct.cshtml", product);
                }
                else
                {
                    ViewBag.SuccessMessage = "";
                    int ProductID = 0;
                    int Count = 0;
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string q = "select Count(*) as Count from [dbo].[Product] where ProductName='" + model.ProductName + "'";

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
                            string query = "INSERT INTO [Product] (ProductName, ShortDescription, FullDescription,SKU,IsPublished,Price,IsTextexempt,Taxcategory,IsShippingEnabled,InventoryMethod,StockQuantity,IsActive,CreatedDate,UpdatedDate) VALUES  ('" + model.ProductName + "','" + model.ShortDescription + "','" + model.FullDescription + "','" + model.SKU + "','" + model.IsPublished + "','" + model.Price + "','" + model.IsTextexempt + "','" + model.Taxcategory + "','" + model.IsShippingEnable + "','" + model.InventoryMethod + "','" + model.StockQuantity + "','" + 1 + "','" + DateTime.Now + "','" + DateTime.Now + "')";
                            connection.Open();
                            SqlCommand cmd = new SqlCommand(query, connection);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                connection.Close();
                                string query1 = "select ProductID from [dbo].[Product] where ProductName='" + model.ProductName + "'";
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
                                                connection.Close();

                                                ViewBag.SuccessMessage = "Added Product Successful";
                                                return RedirectToAction("ProductList", "DemoProduct");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            ViewBag.SuccessMessage = "Product Name Alredy Exist.";
                            return View("~/Plugins/Widgets.BasicPlugins/Views/AddProduct.cshtml", product);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/AddProduct.cshtml", product);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/AddProduct.cshtml", product);
        }
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            Product product = new();
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

                            string query1 = "select STUFF((SELECT  ', ' + CAST(t1.CategoryID AS varchar) from CategoryDetail t1 where t.ProductID = t1.ProductID FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)') ,1,2,'') as CategoryID from Products t where t.ProductID=" + id + "";
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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/EditProduct.cshtml", product);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/EditProduct.cshtml", product);
        }
        [HttpPost]
        public ActionResult EditProduct(Product product)
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
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.SuccessMessage = "Enter required fields.";
                    return View("~/Plugins/Widgets.BasicPlugins/Views/EditProduct.cshtml", product);
                }
                else
                {
                    int ProductID = 0;
                    int Count = 0;
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string q = "select Count(*) as Count from [dbo].[Product] where ProductName='" + product.ProductName + "'and ProductID!='" + product.ProductID + "'";

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
                            string query = "Update Product set ProductName='" + product.ProductName + "',ShortDescription='" + product.ShortDescription + "',FullDescription='" + product.FullDescription + "',SKU='" + product.SKU + "',IsPublished='" + product.IsPublished + "',Price='" + product.Price + "',IsTextexempt='" + product.IsTextexempt + "',Taxcategory='" + product.Taxcategory + "',IsShippingEnabled='" + product.IsShippingEnable + "',InventoryMethod='" + product.InventoryMethod + "',StockQuantity='" + product.StockQuantity + "',IsActive=1,UpdatedDate=GETDATE() where ProductID='" + product.ProductID + "'";
                            SqlCommand cmd = new SqlCommand(query, connection);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                string query1 = "select ProductID from [dbo].[Product] where ProductName='" + product.ProductName + "'";

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
                                                if (product.CategoryID != null)
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
                                                }
                                                connection.Close();

                                                ViewBag.SuccessMessage = "Added Product Successful";
                                                return RedirectToAction("ProductList", "DemoProduct");

                                            }

                                        }
                                    }
                                }

                            }
                        }
                        else
                        {

                            ViewBag.SuccessMessage = "Product Name Alredy Exist.";
                            return View("~/Plugins/Widgets.BasicPlugins/Views/AddProduct.cshtml", product);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/EditProduct.cshtml", product);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/EditProduct.cshtml", product);
        }
        [HttpGet]
        public ActionResult DeleteProduct(int id)
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
        //[HttpPost]
        //public IActionResult UploadImage(IFormFile file, string name)
        //{
        //    // Handle the form data
        //    return Ok();
        //}

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
                                            AltImage = reader["Alt"].ToString(),
                                            ProductID = reader["ProductID"].ToString(),
                                            Title = reader["Title"].ToString(),
                                            Displayorder = reader["DisplayOrder"].ToString(),
                                            sFile = RetrieveImage((byte[])reader["ImageData"]),

                                        });
                                        ;

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