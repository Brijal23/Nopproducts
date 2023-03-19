
using System.Net;
using System.Web;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Widgets.BasicPlugins.Models;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using DateTime = System.DateTime;
using ProductTag = Nop.Plugin.Widgets.BasicPlugins.Models.ProductTag;

namespace Nop.Plugin.Widgets.BasicPlugins.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class DemoProductTagController : BasePluginController
    {

        [HttpGet]
        public ActionResult ProductTagList()
        {

            List<ProductTag> ProductTags = new();
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";

                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    string Query = "";
                    Query = "SELECT * FROM ProductTags AS C WHERE IsActive=1";
                   
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductTags.Add(new ProductTag
                                {
                                    ProductTagName = reader["ProductTagName"].ToString(),
                                    ProductTagID = Convert.ToInt32(reader["ProductTagID"].ToString()),
                                });

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/ProductTag/ProductTagList.cshtml", ProductTags);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductTag/ProductTagList.cshtml", ProductTags);
        }
        [HttpGet]
        public ActionResult Search(string ProductTagName = "")
        {
            List<ProductTag> ProductTags = new();
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";

                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    string Query = "";
                    Query = "SELECT * FROM ProductTags AS C WHERE IsActive=1";
                    if (ProductTagName != "" && ProductTagName != null)
                    {
                        Query += " AND C.ProductTagName LIKE '%" + ProductTagName + "%'";
                    }

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductTags.Add(new ProductTag
                                {
                                    ProductTagName = reader["ProductTagName"].ToString(),
                                    ProductTagID = Convert.ToInt32(reader["ProductTagID"].ToString()),
                                });

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return Json(new { data = ProductTags, status = "error" });
            }
            return Json(new { data = ProductTags, status = "success" });
        }
        [HttpGet]
        public ActionResult AddProductTag(int id = 0)
        {
            ProductTag ProductTag = new();
            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductTag/AddProductTag.cshtml", ProductTag);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddProductTag(ProductTag model, string Command)
        {
            ProductTag ProductTag = new();
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Plugins/Widgets.BasicPlugins/Views/ProductTag/AddProductTag.cshtml", ProductTag);
                }
                else
                {
                    ViewBag.SuccessMessage = "";
                    int ProductTagID = 0;
                    int Count = 0;
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string q = "select Count(*) as Count from [dbo].[ProductTags] where ProductTagName='" + model.ProductTagName + "'";
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
                           
                            string query = "INSERT INTO [ProductTags] (ProductTagName,IsActive,CreatedDate,UpdatedDate) VALUES  ('" + model.ProductTagName.Trim() + "','" + 1 + "','" + DateTime.Now + "','" + DateTime.Now + "')";
                            connection.Open();
                            SqlCommand cmd = new SqlCommand(query, connection);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                connection.Close();
                                string query1 = "select ProductTagID from [dbo].[ProductTags] where ProductTagName='" + model.ProductTagName + "'";
                                connection.Open();
                                using (SqlCommand command = new SqlCommand(query1, connection))
                                {
                                    using (SqlDataReader reader = command.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            ProductTagID = Convert.ToInt32(reader["ProductTagID"].ToString());
                                        }
                                    }
                                }
                                connection.Close();
                                ViewBag.SuccessMessage = "ProductTag Add Successfully";
                                if (Command == "save")
                                    return RedirectToAction("ProductTagList", "DemoProductTag");
                                else
                                {
                                    return RedirectToAction("EditProductTag", new { id = ProductTagID });
                                }

                            }
                        }
                        else
                        {
                            ViewBag.SuccessMessage = "Tag Name Already Exist.";
                            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductTag/AddProductTag.cshtml", ProductTag);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/ProductTag/AddProductTag.cshtml", ProductTag);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductTag/AddProductTag.cshtml", ProductTag);
        }

        [HttpGet]
        public ActionResult EditProductTag(int id)
        {
            ProductTag ProductTag = new();
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    string query = "select * from [dbo].[ProductTags] where IsActive=1 and ProductTagID='" + id + "'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ProductTag.ProductTagName = reader["ProductTagName"].ToString();
                                ProductTag.ProductTagID = Convert.ToInt32(reader["ProductTagID"].ToString());
                            }
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/ProductTag/EditProductTag.cshtml", ProductTag);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductTag/EditProductTag.cshtml", ProductTag);
        }
        [HttpPost]
        public ActionResult EditProductTag(ProductTag ProductTag,string Command)
        {
            // GetAllMethods(product);
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.SuccessMessage = "Enter required fields.";
                    return View("~/Plugins/Widgets.BasicPlugins/Views/ProductTag/EditProductTag.cshtml", ProductTag);
                }
                else
                {
                    int ProductTagID = 0;
                    int Count = 0;
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string q = "select Count(*) as Count from [dbo].[ProductTags] where ProductTagName='" + ProductTag.ProductTagName + "'and ProductTagID!='" + ProductTag.ProductTagID + "'";

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
                            
                            string query = "Update ProductTags set ProductTagName='" + ProductTag.ProductTagName + "',IsActive=1,UpdatedDate=GETDATE() where ProductTagID='" + ProductTag.ProductTagID + "'";
                            SqlCommand cmd = new SqlCommand(query, connection);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                ViewBag.SuccessMessage = "Updated ProductTag Successful";
                                if (Command == "save")
                                    return RedirectToAction("ProductTagList", "DemoProductTag");
                                else
                                {
                                    return RedirectToAction("EditProductTag", new { id = ProductTag.ProductTagID });
                                }
                            }
                        }
                        else
                        {
                            ViewBag.SuccessMessage = "Tag Name Already Exist.";
                            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductTag/EditProductTag.cshtml", ProductTag);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/ProductTag/EditProductTag.cshtml", ProductTag);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductTag/EditProductTag.cshtml", ProductTag);
        }
        [HttpGet]
        public ActionResult DeleteProductTag(string data)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                {
                    connection.Open();
                    foreach (var id in data.Split(","))
                    {
                        string query = "Update ProductTags set IsActive=0 where ProductTagID='" + id + "'";
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
                    string query = "Update ProductTags set IsActive=0 where ProductTagID='" + id + "'";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        ViewBag.SuccessMessage = "ProductTag Deleted Successfully.";
                        return RedirectToAction("ProductTagList", "DemoProductTag");
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return RedirectToAction("ProductTagList", "DemoProductTag");
            }

            return RedirectToAction("ProductTagList", "DemoProductTag");
        }
    }
}