
using System.Diagnostics.Eventing.Reader;
using System.Net;
using System.Web;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Nop.Plugin.Widgets.BasicPlugins.Models;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Category = Nop.Plugin.Widgets.BasicPlugins.Models.Category;
using DateTime = System.DateTime;

namespace Nop.Plugin.Widgets.BasicPlugins.Controllers
{
    //[AuthorizeAdmin]
    //[Area(AreaNames.Admin)]
    //[AutoValidateAntiforgeryToken]
    public class SystemController : BasePluginController
    {

        [HttpGet]
        public ActionResult Index()
        {
            List<ShoppingCart> carts = new();
            List<Product> Products = new();
            try
            {
                Common();
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    string Query = "";
                    string Query1 = "";
                    string Query2 = "";
                    string Query3 = "";

                    string Q = "SELECT P.ProductID,P.ProductName,P.SKU,P.Price,P.StockQuantity,P.IsPublished,P.CreatedDate,I.ImageData FROM [Product] P LEFT JOIN ImageDetail I ON P.ProductID = I.ProductID" + Query + " WHERE P.IsActive=1 " + Query1 + Query2 + Query3 + " and I.ImageID IN (SELECT MAX(ImageID) FROM ImageDetail GROUP BY ProductID) Union SELECT * FROM (SELECT P.ProductID,P.ProductName,P.SKU,P.Price,P.StockQuantity,P.IsPublished,P.CreatedDate,I.ImageData FROM [Product] P LEFT JOIN ImageDetail I ON P.ProductID = I.ProductID " + Query + " WHERE P.IsActive=1 " + Query1 + Query2 + Query3 + ") as X WHERE X.ImageData is null";
                    using (SqlCommand command = new SqlCommand(Q, conn))
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
                                    FPrice = Convert.ToSingle(reader["Price"].ToString()),
                                });
                            }
                        }
                    }
                    conn.Close();
                    conn.Open();
                    string query = "select S.*,P.ProductName from [dbo].[ShoppingCart] as S Inner join[dbo].[Product] as P on P.ProductID = S.ProductID where S.IsActive = 1";
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                carts.Add(new ShoppingCart
                                {
                                    CartID = Convert.ToInt32(reader["CartID"].ToString()),
                                    ImageData = reader["ImageData"].ToString(),
                                    ProductName = reader["ProductName"].ToString(),
                                    ProductID = Convert.ToInt32(reader["ProductID"].ToString()),
                                    Size = reader["Size"].ToString(),
                                    Quantity = Convert.ToInt32(reader["Quantity"].ToString()),
                                    FPrice = Convert.ToSingle(reader["Price"].ToString()),
                                    TotalPrice = (decimal)(Convert.ToInt32(reader["Quantity"].ToString()) * Convert.ToSingle(reader["Price"].ToString())),
                                });
                            }
                            conn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/System/Index.cshtml", Products);
            }
            ViewBag.ShoppingCart = carts;
            return View("~/Plugins/Widgets.BasicPlugins/Views/System/Index.cshtml", Products);
        }
        public void Common()
        {
            List<Category> categories = new();
            List<ShoppingCart> carts = new();
            ListShoppingCart list = new();
            ViewBag.Login = HttpContext.Session.GetString("Login");
            string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";

            using (SqlConnection connection = new SqlConnection(constr))
            {
                connection.Open();
                string Query = "";
                Query = "SELECT * FROM Categories AS C WHERE IsActive=1 AND C.ParentcategoryId=0";
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new Category
                            {
                                CategoryName = reader["CategoryName"].ToString(),
                                CategoryID = Convert.ToInt32(reader["CategoryID"].ToString()),
                            });
                        }
                    }
                }
                connection.Close();
                ViewBag.Category = categories;
                connection.Open();
                string q = "Select Sum(Quantity) as Count FROM [dbo].[ShoppingCart] WHERE IsActive=1";
                using (SqlCommand command = new SqlCommand(q, connection))
                {
                    using (SqlDataReader r = command.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            ViewBag.Count = r["Count"].ToString() != "" ? Convert.ToInt32(r["Count"].ToString()) : 0;
                        }
                    }
                }
                connection.Close();
                connection.Open();
                string query = "select S.*,P.ProductName from [dbo].[ShoppingCart] as S Inner join[dbo].[Product] as P on P.ProductID = S.ProductID where S.IsActive = 1";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            carts.Add(new ShoppingCart
                            {
                                CartID = Convert.ToInt32(reader["CartID"].ToString()),
                                ImageData = reader["ImageData"].ToString(),
                                ProductName = reader["ProductName"].ToString(),
                                ProductID = Convert.ToInt32(reader["ProductID"].ToString()),
                                Size = reader["Size"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"].ToString()),
                                FPrice = Convert.ToSingle(reader["Price"].ToString()),
                                TotalPrice = (decimal)(Convert.ToInt32(reader["Quantity"].ToString()) * Convert.ToSingle(reader["Price"].ToString())),
                            });
                        }
                        list.ShoppingCartList = carts;
                        connection.Close();
                    }
                }
                ViewBag.ShoppingCartList = carts;
                decimal? subtotal = 0;
                foreach (var item in list.ShoppingCartList)
                {
                    subtotal = subtotal + item.TotalPrice;
                }
                connection.Open();
                string querys = "Update Discount set Totalprice='" + subtotal + "'";
                SqlCommand cmd = new SqlCommand(querys, connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                connection.Open();
                string qss = "Update Discount set DiscountPrice=(Case when Isdiscount=1 then (Totalprice*0.5) else Totalprice end)";
                SqlCommand cmds = new SqlCommand(qss, connection);
                cmds.ExecuteNonQuery();
                connection.Close();
                connection.Open();
                string totalqs = "Select DiscountPrice FROM [dbo].[Discount]";
                using (SqlCommand command = new SqlCommand(totalqs, connection))
                {
                    using (SqlDataReader r = command.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            ViewBag.Subtotal = Convert.ToSingle(r["DiscountPrice"].ToString());
                        }
                    }
                }
                connection.Close();
                // ViewBag.Subtotal = subtotal;
            }
        }
        [HttpGet]
        public ActionResult Productdetail(int id)
        {
            Product Product = new();
            List<ShoppingCart> carts = new();
            try
            {
                Common();
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";

                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    string query = "select S.*,P.ProductName from [dbo].[ShoppingCart] as S Inner join[dbo].[Product] as P on P.ProductID = S.ProductID where S.IsActive = 1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                carts.Add(new ShoppingCart
                                {
                                    CartID = Convert.ToInt32(reader["CartID"].ToString()),
                                    ImageData = reader["ImageData"].ToString(),
                                    ProductName = reader["ProductName"].ToString(),
                                    ProductID = Convert.ToInt32(reader["ProductID"].ToString()),
                                    Size = reader["Size"].ToString(),
                                    Quantity = Convert.ToInt32(reader["Quantity"].ToString()),
                                    FPrice = Convert.ToSingle(reader["Price"].ToString()),
                                    TotalPrice = (decimal)(Convert.ToInt32(reader["Quantity"].ToString()) * Convert.ToSingle(reader["Price"].ToString())),
                                });
                            }
                            connection.Close();
                        }
                        ViewBag.ShoppingCart = carts;
                    }
                    connection.Open();

                    // string Q = "SELECT * FROM Product WHERE IsActive=1 and ProductName='" + name + "'";
                    string Q = "SELECT P.ProductID,P.ProductName,P.ShortDescription,P.SKU,P.Price,P.StockQuantity,P.IsPublished,P.CreatedDate,I.ImageData FROM [Product] P LEFT JOIN ImageDetail I ON P.ProductID = I.ProductID WHERE P.IsActive=1 and P.ProductID='" + id + "' and I.ImageID IN (SELECT MAX(ImageID) FROM ImageDetail GROUP BY ProductID) Union SELECT * FROM (SELECT P.ProductID,P.ProductName,P.ShortDescription,P.SKU,P.Price,P.StockQuantity,P.IsPublished,P.CreatedDate,I.ImageData FROM [Product] P LEFT JOIN ImageDetail I ON P.ProductID = I.ProductID  WHERE P.IsActive=1 and P.ProductID='" + id + "') as X WHERE X.ImageData is null";
                    using (SqlCommand command = new SqlCommand(Q, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product.SFile = reader["ImageData"].ToString() != "" ? RetrieveImage((byte[])reader["ImageData"]) : "https://cafe.shopfast.net/images/thumbs/default-image_100.png";
                                Product.ProductName = reader["ProductName"].ToString();
                                Product.SKU = reader["SKU"].ToString();
                                Product.ProductID = Convert.ToInt32(reader["ProductID"].ToString());
                                Product.FPrice = Convert.ToSingle(reader["Price"].ToString());
                                if (reader["ShortDescription"].ToString() != "")
                                    Product.ShortDescription = HttpUtility.UrlDecode(reader["ShortDescription"].ToString());
                                else
                                    Product.ShortDescription = "";
                            }
                        }
                    }
                    connection.Close();

                    string Iquery = "select * from [dbo].[ImageDetail] where ProductID='" + Product.ProductID + "'order by ImageID desc";
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
                            Product.UploadFileModel = UploadFiles;
                        }
                        connection.Close();
                    }
                    connection.Open();
                    string q = "Select Sum(Quantity) as Count FROM [dbo].[ShoppingCart] WHERE IsActive=1";
                    using (SqlCommand command = new SqlCommand(q, connection))
                    {
                        using (SqlDataReader r = command.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                ViewBag.Count = Convert.ToInt32(r["Count"].ToString());
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/System/Productdetail.cshtml", Product);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/System/Productdetail.cshtml", Product);
        }
        [HttpGet]
        public ActionResult Cart()
        {
            List<ShoppingCart> carts = new();
            ListShoppingCart list = new();
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";

                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    string query = "select S.*,P.ProductName from [dbo].[ShoppingCart] as S Inner join[dbo].[Product] as P on P.ProductID = S.ProductID where S.IsActive = 1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                carts.Add(new ShoppingCart
                                {
                                    CartID = Convert.ToInt32(reader["CartID"].ToString()),
                                    ImageData = reader["ImageData"].ToString(),
                                    ProductName = reader["ProductName"].ToString(),
                                    ProductID = Convert.ToInt32(reader["ProductID"].ToString()),
                                    Size = reader["Size"].ToString(),
                                    Quantity = Convert.ToInt32(reader["Quantity"].ToString()),
                                    FPrice = Convert.ToSingle(reader["Price"].ToString()),
                                    TotalPrice = (decimal)(Convert.ToInt32(reader["Quantity"].ToString()) * Convert.ToSingle(reader["Price"].ToString())),
                                });
                            }
                            list.ShoppingCartList = carts;
                            connection.Close();
                        }
                    }
                }
                Common();
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/System/Cart.cshtml", list);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/System/Cart.cshtml", list);
        }
        [HttpPost]
        public ActionResult Addtocart(ShoppingCart model)
        {
            int Count = 0;
            int CountProduct = 0;
            ShoppingCart cart = new();
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                {
                    connection.Open();
                    string q = "select Count(*) as Count from [dbo].[ShoppingCart] where IsActive=1 and ProductID='" + model.ProductID + "'";
                    using (SqlCommand command = new SqlCommand(q, connection))
                    {
                        using (SqlDataReader r = command.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                CountProduct = Convert.ToInt32(r["Count"].ToString());
                            }
                        }
                    }
                    connection.Close();
                    if (CountProduct != 0)
                    {
                        connection.Open();
                        string query = "select * from [dbo].[ShoppingCart] where IsActive=1 and ProductID='" + model.ProductID + "'";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    cart.ProductID = Convert.ToInt32(reader["ProductID"].ToString());
                                    cart.Quantity = Convert.ToInt32(reader["Quantity"].ToString());
                                    cart.Price = reader["Price"].ToString() == "" ? 0 : Convert.ToDecimal(reader["Price"].ToString());
                                }
                            }
                        }
                        connection.Close();
                        connection.Open();
                        var Price = model.Price;
                        var Quantity = model.Quantity + cart.Quantity;
                        var TotalPrice = Price * Quantity;
                        string qss = "Update ShoppingCart set Size='" + model.Size + "',Price='" + Price + "',Quantity='" + Quantity + "',TotalPrice='" + TotalPrice + "',IsActive=1,UpdatedDate=GETDATE() where ProductID='" + model.ProductID + "'";
                        SqlCommand cmd = new SqlCommand(qss, connection);
                        if (cmd.ExecuteNonQuery() == 1)
                        { return Json(new { status = "success" }); }
                        connection.Close();
                    }
                    else
                    {
                        string query = "INSERT INTO [dbo].[ShoppingCart] (ProductID, Size,Price,Quantity,ImageData, TotalPrice,IsActive,CreatedDate,UpdatedDate) VALUES  (@ProductID,@Size,@Price,@Quantity,@ImageData,@TotalPrice,@IsActive,@CreatedDate,@UpdatedDate)";
                        connection.Open();
                        SqlCommand cmd = new SqlCommand(query, connection);
                        model.TotalPrice = model.Price * model.Quantity;
                        cmd.Parameters.AddWithValue("@ProductID", model.ProductID);
                        cmd.Parameters.AddWithValue("@Size", model.Size);
                        cmd.Parameters.AddWithValue("@Price", model.Price);
                        cmd.Parameters.AddWithValue("@Quantity", model.Quantity);
                        cmd.Parameters.AddWithValue("@ImageData", model.ImageData);
                        cmd.Parameters.AddWithValue("@TotalPrice", model.TotalPrice);
                        cmd.Parameters.AddWithValue("@IsActive", 1);
                        cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                        if (cmd.ExecuteNonQuery() == 1)
                        {
                            connection.Close();
                            connection.Open();
                            string qs = "Select Sum(Quantity) as Count FROM [dbo].[ShoppingCart] WHERE IsActive=1";
                            using (SqlCommand command = new SqlCommand(qs, connection))
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
                            return Json(new { status = "success" });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "error" });
            }
            return Json(new { status = "error" });
        }
        [HttpGet]
        public ActionResult UpdateProductdetail(int id)
        {
            List<ShoppingCart> carts = new();
            ShoppingCart cart = new();
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    // string Q = "SELECT * FROM Product WHERE IsActive=1 and ProductName='" + name + "'";
                    //string Q = "SELECT P.ProductID,q.ProductName,P.Size,P.Quantity,P.Price,I.ImageData FROM ShoppingCart as P RIGHT JOIN ImageDetail I ON P.ProductID = I.ProductID INNER JOIN Product as q on q.ProductID=p.ProductID WHERE P.IsActive=1 and  P.ProductID='" + id + "'and I.ImageID IN (SELECT MAX(ImageID) FROM ImageDetail GROUP BY ProductID) Union SELECT * FROM (SELECT P.ProductID,q.ProductName,P.Size,P.Quantity,P.Price,I.ImageData FROM ShoppingCart as P RIGHT JOIN ImageDetail I ON P.ProductID = I.ProductID INNER JOIN Product as q on q.ProductID=p.ProductID WHERE P.IsActive=1 and  P.ProductID='" + id + "') as X WHERE X.ImageData is null";
                    string Q = "SELECT P.ProductID,q.ProductName,P.Size,P.Quantity,P.Price as FPrice,q.Price,P.ImageData FROM ShoppingCart as P RIGHT JOIN Product as q on q.ProductID=P.ProductID WHERE P.IsActive=1 and  P.ProductID='" + id + "'";
                    using (SqlCommand command = new SqlCommand(Q, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cart.ImageData = reader["ImageData"].ToString();
                                cart.ProductName = reader["ProductName"].ToString();
                                cart.Size = reader["Size"].ToString();
                                cart.ProductID = Convert.ToInt32(reader["ProductID"].ToString());
                                cart.FPrice = Convert.ToSingle(reader["FPrice"].ToString());
                                cart.FEPrice = Convert.ToSingle(reader["Price"].ToString());
                                cart.Quantity = Convert.ToInt32(reader["Quantity"].ToString());
                            }
                        }
                    }
                    connection.Close();

                    string Iquery = "select * from [dbo].[ImageDetail] where ProductID='" + cart.ProductID + "'order by ImageID desc";
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
                            cart.UploadFileModel = UploadFiles;
                        }
                        connection.Close();
                    }
                    connection.Open();
                    string query = "select S.*,P.ProductName from [dbo].[ShoppingCart] as S Inner join[dbo].[Product] as P on P.ProductID = S.ProductID where S.IsActive = 1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                carts.Add(new ShoppingCart
                                {
                                    CartID = Convert.ToInt32(reader["CartID"].ToString()),
                                    ImageData = reader["ImageData"].ToString(),
                                    ProductName = reader["ProductName"].ToString(),
                                    ProductID = Convert.ToInt32(reader["ProductID"].ToString()),
                                    Size = reader["Size"].ToString(),
                                    Quantity = Convert.ToInt32(reader["Quantity"].ToString()),
                                    FPrice = Convert.ToSingle(reader["Price"].ToString()),
                                    TotalPrice = (decimal)(Convert.ToInt32(reader["Quantity"].ToString()) * Convert.ToSingle(reader["Price"].ToString())),
                                });
                            }
                            connection.Close();
                        }
                        ViewBag.ShoppingCart = carts;
                    }
                    Common();
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/System/UpdateProductdetail.cshtml", cart);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/System/UpdateProductdetail.cshtml", cart);
        }

        [HttpPost]
        public ActionResult Updatetocart(ShoppingCart model)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                {
                    connection.Open();
                    var totalPrice = model.Price * model.Quantity;
                    string qss = "Update ShoppingCart set Size='" + model.Size + "',Price='" + model.Price + "',Quantity='" + model.Quantity + "',TotalPrice='" + totalPrice + "',IsActive=1,UpdatedDate=GETDATE() where ProductID='" + model.ProductID + "'";
                    SqlCommand cmd = new SqlCommand(qss, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    return Json(new { status = "success" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = ex.Message.ToString() });
            }
        }
        [HttpPost]
        public ActionResult AddShoppingcartvalue(ListShoppingCart model)
        {

            List<ShoppingCart> carts = new();
            ListShoppingCart list = new();
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                {
                    foreach (var item in model.ShoppingCartList)
                    {
                        if (item.Ischecked == true || item.Quantity == 0)
                        {
                            connection.Open();
                            var TotalPrice = item.Quantity * item.FPrice;
                            string qss = "DELETE From ShoppingCart WHERE CartID='" + item.CartID + "'";
                            SqlCommand cmd = new SqlCommand(qss, connection);
                            cmd.ExecuteNonQuery();
                            connection.Close();
                        }
                        else
                        {
                            connection.Open();
                            var TotalPrice = item.Quantity * item.FPrice;
                            string qss = "Update ShoppingCart set Quantity='" + item.Quantity + "',TotalPrice='" + TotalPrice + "',IsActive=1,UpdatedDate=GETDATE() where CartID='" + item.CartID + "'";
                            SqlCommand cmd = new SqlCommand(qss, connection);
                            cmd.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                    connection.Open();
                    string query = "select S.*,P.ProductName from [dbo].[ShoppingCart] as S Inner join[dbo].[Product] as P on P.ProductID = S.ProductID where S.IsActive = 1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                carts.Add(new ShoppingCart
                                {
                                    CartID = Convert.ToInt32(reader["CartID"].ToString()),
                                    ImageData = reader["ImageData"].ToString(),
                                    ProductName = reader["ProductName"].ToString(),
                                    ProductID = Convert.ToInt32(reader["ProductID"].ToString()),
                                    Size = reader["Size"].ToString(),
                                    Quantity = Convert.ToInt32(reader["Quantity"].ToString()),
                                    FPrice = Convert.ToSingle(reader["Price"].ToString()),
                                    TotalPrice = (decimal)(Convert.ToInt32(reader["Quantity"].ToString()) * Convert.ToSingle(reader["Price"].ToString())),
                                });
                            }
                            list.ShoppingCartList = carts;
                            connection.Close();
                        }
                    }
                    Common();
                }
            }
            catch (Exception ex)
            {
                return View("~/Plugins/Widgets.BasicPlugins/Views/System/Cart.cshtml", list);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/System/Cart.cshtml", list);
        }
        [HttpGet]
        public ActionResult Applydiscount(string discount, decimal Total)
        {
            int Count = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                {
                    connection.Open();
                    string q = "select COUNT(*) as Count from Discount where Isdiscount=1";
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
                        connection.Open();
                        decimal discountprice = Total / 2;
                        string qss = "Update Discount set Totalprice='" + Total + "',DiscountPrice='" + discountprice + "',Isdiscount='" + 1 + "',Discountcode='" + discount.Trim() + "'";
                        SqlCommand cmd = new SqlCommand(qss, connection);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                        return Json(new { status = "success" });
                    }
                    else
                    {
                        return Json(new { status = "error", message = "Coupon code already applied." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = ex.Message.ToString() });
            }
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
        [AllowAnonymous]
        public ActionResult Login()
        {
            Common();
            return View("~/Plugins/Widgets.BasicPlugins/Views/System/Login.cshtml");
        }

        [HttpGet]
        public ActionResult Checkout()
        {

            List<ShoppingCart> carts = new();
            ListShoppingCart list = new();
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                {
                    connection.Open();
                   
                    string qss = "Update ShoppingCart set IsActive=0";
                    SqlCommand cmd = new SqlCommand(qss, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                    return Json(new { status = "success" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "error" });
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Login model)
        {
            Common();
            if (ModelState.IsValid)
            {
                string email = "cafe@olb.com";
                string password = "cafe";
                if (model.Email == email && model.Password == password)
                {
                    ViewBag.LoginSuccess = "Login Successful";
                    HttpContext.Session.SetString("Login", "1");
                    return RedirectToAction("Index", "System");
                }
                else
                {
                    ViewBag.Validation = "Email or Password does not match!";
                }
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/System/Login.cshtml", model);
        }

        public ActionResult LogOut()
        {
            HttpContext.Session.Remove("Login");
            return RedirectToAction("Login", "System");
        }

        public ActionResult Payment()
        {
            Common();
            if (ViewBag.Login == "1")
            {
                return View("~/Plugins/Widgets.BasicPlugins/Views/System/Payment.cshtml");
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/System/Login.cshtml");
        }
    }
}