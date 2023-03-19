
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
    public class DemoCategoryController : BasePluginController
    {

        [HttpGet]
        public ActionResult CategoryList(/*string CategoryName = "", string published = "All"*/)
        {
            List<Category> categories = new();
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";

                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    string Query = "";
                    Query = "SELECT * FROM Categories AS C WHERE IsActive=1";
                    //if (CategoryName != "" && CategoryName != null)
                    //{
                    //    Query += " AND C.CategoryName LIKE '%" + CategoryName + "%'";
                    //}
                    //if (published != "All")
                    //{
                    //    int Ipublished = Convert.ToInt32(published);
                    //    Query += " AND C.IsPublished =" + Ipublished + "";
                    //}
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
                                    Displayorder = Convert.ToInt32(reader["Displayorder"].ToString()),
                                    IsPublished = Convert.ToBoolean(reader["IsPublished"].ToString()) == true ? true : false,
                                });

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/Category/CategoryList.cshtml", categories);
            }
            //ViewBag.Categorynamesearch = CategoryName;
            //ViewBag.publishedsearch = published;
            return View("~/Plugins/Widgets.BasicPlugins/Views/Category/CategoryList.cshtml", categories);
        }
        [HttpGet]
        public ActionResult Search(string CategoryName, string published)
        {
            List<Category> categories = new();
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";

                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    string Query = "";
                    Query = "SELECT * FROM Categories AS C WHERE IsActive=1";
                    if (CategoryName != "" && CategoryName != null)
                    {
                        Query += " AND C.CategoryName LIKE '%" + CategoryName + "%'";
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
                                categories.Add(new Category
                                {
                                    CategoryName = reader["CategoryName"].ToString(),
                                    CategoryID = Convert.ToInt32(reader["CategoryID"].ToString()),
                                    Displayorder = Convert.ToInt32(reader["Displayorder"].ToString()),
                                    IsPublished = Convert.ToBoolean(reader["IsPublished"].ToString()) == true ? true : false,
                                });

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return Json(new { data = categories, status = "error" });
            }
            
            return Json(new { data = categories, status = "success" });
        }
            [HttpGet]
        public ActionResult AddCategory(int id = 0)
        {
            Category category = new();
            GetAllMethods(category);
            return View("~/Plugins/Widgets.BasicPlugins/Views/Category/AddCategory.cshtml", category);
        }

        private void GetAllMethods(Category category)
        {
            category.ParentCategories = new[]
        {new SelectListItem { Value = "0", Text = "[None]" },
        new SelectListItem { Value = "1", Text = "Lunch & Dinner" },
        new SelectListItem { Value = "2", Text = "Breakfast & Brunch" },
        new SelectListItem { Value = "3", Text = "Baked Goods & Desserts" },
        new SelectListItem { Value = "4", Text = "Drinks" },
             };
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory(Category model, string Command)
        {
            Category category = new();
            GetAllMethods(category);
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Plugins/Widgets.BasicPlugins/Views/Category/AddCategory.cshtml", category);
                }
                else
                {
                    ViewBag.SuccessMessage = "";
                    int CategoryID = 0;
                    int Count = 0;
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string q = "select Count(*) as Count from [dbo].[Categories] where CategoryName='" + model.CategoryName + "'";
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

                            string query = "INSERT INTO [Categories] (CategoryName, Description, IsPublished,ParentcategoryId,Displayorder,IsActive,CreatedDate,UpdatedDate) VALUES  ('" + model.CategoryName.Trim() + "','" + model.Description + "','" + model.IsPublished + "','" + model.ParentcategoryId + "','" + model.Displayorder + "','" + 1 + "','" + DateTime.Now + "','" + DateTime.Now + "')";
                            connection.Open();
                            SqlCommand cmd = new SqlCommand(query, connection);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                connection.Close();
                                string query1 = "select CategoryID from [dbo].[Categories] where CategoryName='" + model.CategoryName + "'";
                                connection.Open();
                                using (SqlCommand command = new SqlCommand(query1, connection))
                                {
                                    using (SqlDataReader reader = command.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            CategoryID = Convert.ToInt32(reader["CategoryID"].ToString());
                                        }
                                    }
                                }
                                connection.Close();
                                ViewBag.SuccessMessage = "Category Add Successfully";
                                if (Command == "save")
                                    return RedirectToAction("CategoryList", "DemoCategory");
                                else
                                {
                                    return RedirectToAction("EditCategory", new { id = CategoryID });
                                }

                            }
                        }
                        else
                        {
                            ViewBag.SuccessMessage = "Category Name Already Exist.";
                            return View("~/Plugins/Widgets.BasicPlugins/Views/Category/AddCategory.cshtml", category);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/Category/AddCategory.cshtml", category);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/Category/AddCategory.cshtml", category);
        }
        [HttpGet]
        public ActionResult EditCategory(int id)
        {
            Category category = new();
            GetAllMethods(category);
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    string query = "select * from [dbo].[Categories] where IsActive=1 and CategoryID='" + id + "'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                category.CategoryName = reader["CategoryName"].ToString();
                                category.CategoryID = Convert.ToInt32(reader["CategoryID"].ToString());
                                category.ParentcategoryId = reader["ParentcategoryId"].ToString();
                                category.IsPublished = Convert.ToBoolean(reader["IsPublished"].ToString()) == true ? true : false;
                                category.Displayorder = Convert.ToInt32(reader["Displayorder"].ToString());
                                if (reader["Description"].ToString() != "")
                                    category.Description = HttpUtility.UrlDecode(reader["Description"].ToString());
                                else
                                    category.Description = "";

                            }
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/Category/EditCategory.cshtml", category);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/Category/EditCategory.cshtml", category);
        }
        [HttpPost]
        public ActionResult EditCategory(Category category,string Command)
        {
            GetAllMethods(category);
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.SuccessMessage = "Enter required fields.";
                    return View("~/Plugins/Widgets.BasicPlugins/Views/Category/EditCategory.cshtml", category);
                }
                else
                {
                    int CategoryID = 0;
                    int Count = 0;
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string q = "select Count(*) as Count from [dbo].[Categories] where CategoryName='" + category.CategoryName + "'and CategoryID!='" + category.CategoryID + "'";

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
                            if (category.Description != null)
                                category.Description = HttpUtility.UrlEncode(category.Description);

                            string query = "Update Categories set CategoryName='" + category.CategoryName + "',Description='" + category.Description + "',IsPublished='" + category.IsPublished + "',ParentcategoryId='" + category.ParentcategoryId + "',Displayorder='" + category.Displayorder + "',IsActive=1,UpdatedDate=GETDATE() where CategoryID='" + category.CategoryID + "'";
                            SqlCommand cmd = new SqlCommand(query, connection);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                ViewBag.SuccessMessage = "Updated Category Successful";
                                if (Command == "save")
                                    return RedirectToAction("CategoryList", "DemoCategory");
                                else
                                {
                                    return RedirectToAction("EditCategory", new { id = category.CategoryID });
                                }
                            }
                        }
                        else
                        {
                            ViewBag.SuccessMessage = "Category Name Already Exist.";
                            return View("~/Plugins/Widgets.BasicPlugins/Views/Category/EditCategory.cshtml", category);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/Category/EditCategory.cshtml", category);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/Category/EditCategory.cshtml", category);
        }
        [HttpGet]
        public ActionResult DeleteCategory(string data)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                {
                    connection.Open();
                    foreach (var id in data.Split(","))
                    {
                        string query = "Update Categories set IsActive=0 where CategoryID='" + id + "'";
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
                    string query = "Update Categories set IsActive=0 where CategoryID='" + id + "'";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        ViewBag.SuccessMessage = "Category Deleted Successfully.";
                        return RedirectToAction("CategoryList", "DemoCategory");
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return RedirectToAction("CategoryList", "DemoCategory");
            }

            return RedirectToAction("CategoryList", "DemoCategory");
        }
    }
}