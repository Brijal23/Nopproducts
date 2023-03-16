
using System.Net;
using System.Web;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Nop.Plugin.Widgets.BasicPlugins.Models;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using DateTime = System.DateTime;

namespace Nop.Plugin.Widgets.BasicPlugins.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class DemoProductAttributeController : BasePluginController
    {

        [HttpGet]
        public ActionResult ProductAttributeList(string ProductAttributeName = "", string published = "All")
        {

            List<ProductAttribute> ProductAttributes = new();
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";

                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    string Query = "";
                    Query = "SELECT * FROM ProductAttributes AS C WHERE IsActive=1";
                    if (ProductAttributeName != "" && ProductAttributeName != null)
                    {
                        Query += " AND C.ProductAttributeName LIKE '%" + ProductAttributeName + "%'";
                    }
                    if (published != "All")
                    {
                        int Ipublished = Convert.ToInt32(published);
                        Query += " AND C.IsPublished =" + Ipublished + "";
                    }
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductAttributes.Add(new ProductAttribute
                                {
                                    ProductAttributeName = reader["ProductAttributeName"].ToString(),
                                    ProductAttributeID = Convert.ToInt32(reader["ProductAttributeID"].ToString()),
                                });

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/ProductAttributeList.cshtml", ProductAttributes);
            }
            ViewBag.ProductAttributenamesearch = ProductAttributeName;
            ViewBag.publishedsearch = published;
            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/ProductAttributeList.cshtml", ProductAttributes);
        }
        [HttpGet]
        public ActionResult AddProductAttribute(int id = 0)
        {
            ProductAttribute ProductAttribute = new();
            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/AddProductAttribute.cshtml", ProductAttribute);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddProductAttribute(ProductAttribute model, string Command)
        {
            ProductAttribute ProductAttribute = new();
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/AddProductAttribute.cshtml", ProductAttribute);
                }
                else
                {
                    ViewBag.SuccessMessage = "";
                    int ProductAttributeID = 0;
                    int Count = 0;
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string q = "select Count(*) as Count from [dbo].[ProductAttributes] where ProductAttributeName='" + model.ProductAttributeName + "'";
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
                            if (model.Description != null)
                                model.Description = HttpUtility.UrlEncode(model.Description);

                            string query = "INSERT INTO [ProductAttributes] (ProductAttributeName, Description, IsActive,CreatedDate,UpdatedDate) VALUES  ('" + model.ProductAttributeName.Trim() + "','" + model.Description + "','" + 1 + "','" + DateTime.Now + "','" + DateTime.Now + "')";
                            connection.Open();
                            SqlCommand cmd = new SqlCommand(query, connection);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                connection.Close();
                                string query1 = "select ProductAttributeID from [dbo].[ProductAttributes] where ProductAttributeName='" + model.ProductAttributeName + "'";
                                connection.Open();
                                using (SqlCommand command = new SqlCommand(query1, connection))
                                {
                                    using (SqlDataReader reader = command.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            ProductAttributeID = Convert.ToInt32(reader["ProductAttributeID"].ToString());
                                        }
                                    }
                                }
                                connection.Close();
                                ViewBag.SuccessMessage = "ProductAttribute Add Successfully";
                                if (Command == "save")
                                    return RedirectToAction("ProductAttributeList", "DemoProductAttribute");
                                else
                                {
                                    return RedirectToAction("EditProductAttribute", new { id = ProductAttributeID });
                                }

                            }
                        }
                        else
                        {
                            ViewBag.SuccessMessage = "ProductAttribute Name Alredy Exist.";
                            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/AddProductAttribute.cshtml", ProductAttribute);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/AddProductAttribute.cshtml", ProductAttribute);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/AddProductAttribute.cshtml", ProductAttribute);
        }
        [HttpGet]
        public ActionResult EditProductAttribute(int id)
        {
            ProductAttribute ProductAttribute = new();
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    string query = "select * from [dbo].[ProductAttributes] where IsActive=1 and ProductAttributeID='" + id + "'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductAttribute.ProductAttributeName = reader["ProductAttributeName"].ToString();
                                ProductAttribute.ProductAttributeID = Convert.ToInt32(reader["ProductAttributeID"].ToString());
                                if (reader["Description"].ToString() != "")
                                    ProductAttribute.Description = HttpUtility.UrlDecode(reader["Description"].ToString());
                                else
                                    ProductAttribute.Description = "";

                            }
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/EditProductAttribute.cshtml", ProductAttribute);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/EditProductAttribute.cshtml", ProductAttribute);
        }
        [HttpPost]
        public ActionResult EditProductAttribute(ProductAttribute ProductAttribute,string Command)
        {
            // GetAllMethods(product);
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.SuccessMessage = "Enter required fields.";
                    return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/EditProductAttribute.cshtml", ProductAttribute);
                }
                else
                {
                    int ProductAttributeID = 0;
                    int Count = 0;
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string q = "select Count(*) as Count from [dbo].[ProductAttributes] where ProductAttributeName='" + ProductAttribute.ProductAttributeName + "'and ProductAttributeID!='" + ProductAttribute.ProductAttributeID + "'";

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
                            if (ProductAttribute.Description != null)
                                ProductAttribute.Description = HttpUtility.UrlEncode(ProductAttribute.Description);

                            string query = "Update ProductAttributes set ProductAttributeName='" + ProductAttribute.ProductAttributeName + "',Description='" + ProductAttribute.Description + "',IsActive=1,UpdatedDate=GETDATE() where ProductAttributeID='" + ProductAttribute.ProductAttributeID + "'";
                            SqlCommand cmd = new SqlCommand(query, connection);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                ViewBag.SuccessMessage = "Updated ProductAttribute Successful";
                                if (Command == "save")
                                    return RedirectToAction("ProductAttributeList", "DemoProductAttribute");
                                else
                                {
                                    return RedirectToAction("EditProductAttribute", new { id = ProductAttribute.ProductAttributeID });
                                }
                            }
                        }
                        else
                        {
                            ViewBag.SuccessMessage = "ProductAttribute Name Alredy Exist.";
                            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/EditProductAttribute.cshtml", ProductAttribute);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/EditProductAttribute.cshtml", ProductAttribute);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/EditProductAttribute.cshtml", ProductAttribute);
        }
        [HttpGet]
        public ActionResult DeleteProductAttribute(string data)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                {
                    connection.Open();
                    foreach (var id in data.Split(","))
                    {
                        string query = "Update ProductAttributes set IsActive=0 where ProductAttributeID='" + id + "'";
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
                    string query = "Update ProductAttributes set IsActive=0 where ProductAttributeID='" + id + "'";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        ViewBag.SuccessMessage = "ProductAttribute Deleted Successfully.";
                        return RedirectToAction("ProductAttributeList", "DemoProductAttribute");
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return RedirectToAction("ProductAttributeList", "DemoProductAttribute");
            }

            return RedirectToAction("ProductAttributeList", "DemoProductAttribute");
        }
    }
}