
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
    public class DemoManufacturerController : BasePluginController
    {

        [HttpGet]
        public ActionResult ManufacturerList(/*string ManufacturerName = "", string published = "All"*/)
        {

            List<Manufacturer> Manufacturers = new();
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";

                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    string Query = "";
                    Query = "SELECT * FROM Manufacturers AS C WHERE IsActive=1";
                    //if (ManufacturerName != "" && ManufacturerName != null)
                    //{
                    //    Query += " AND C.ManufacturerName LIKE '%" + ManufacturerName + "%'";
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
                                Manufacturers.Add(new Manufacturer
                                {
                                    ManufacturerName = reader["ManufacturerName"].ToString(),
                                    ManufacturerID = Convert.ToInt32(reader["ManufacturerID"].ToString()),
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
                return View("~/Plugins/Widgets.BasicPlugins/Views/Manufacturer/ManufacturerList.cshtml", Manufacturers);
            }
            //ViewBag.Manufacturernamesearch = ManufacturerName;
            //ViewBag.publishedsearch = published;
            return View("~/Plugins/Widgets.BasicPlugins/Views/Manufacturer/ManufacturerList.cshtml", Manufacturers);
        }
        [HttpGet]
        public ActionResult Search(string ManufacturerName, string published)
        {
            List<Manufacturer> Manufacturers = new();
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";

                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    string Query = "";
                    Query = "SELECT * FROM Manufacturers AS C WHERE IsActive=1";
                    if (ManufacturerName != "" && ManufacturerName != null)
                    {
                        Query += " AND C.ManufacturerName LIKE '%" + ManufacturerName + "%'";
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
                                Manufacturers.Add(new Manufacturer
                                {
                                    ManufacturerName = reader["ManufacturerName"].ToString(),
                                    ManufacturerID = Convert.ToInt32(reader["ManufacturerID"].ToString()),
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
                return Json(new { data = Manufacturers, status = "error" });
            }
            return Json(new { data = Manufacturers, status = "success" });
        }
        [HttpGet]
        public ActionResult AddManufacturer(int id = 0)
        {
            Manufacturer Manufacturer = new();
            return View("~/Plugins/Widgets.BasicPlugins/Views/Manufacturer/AddManufacturer.cshtml", Manufacturer);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddManufacturer(Manufacturer model, string Command)
        {
            Manufacturer Manufacturer = new();
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Plugins/Widgets.BasicPlugins/Views/Manufacturer/AddManufacturer.cshtml", Manufacturer);
                }
                else
                {
                    ViewBag.SuccessMessage = "";
                    int ManufacturerID = 0;
                    int Count = 0;
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string q = "select Count(*) as Count from [dbo].[Manufacturers] where ManufacturerName='" + model.ManufacturerName + "'";
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

                            string query = "INSERT INTO [Manufacturers] (ManufacturerName, Description, IsPublished,Displayorder,IsActive,CreatedDate,UpdatedDate) VALUES  ('" + model.ManufacturerName.Trim() + "','" + model.Description + "','" + model.IsPublished + "','" + model.Displayorder + "','" + 1 + "','" + DateTime.Now + "','" + DateTime.Now + "')";
                            connection.Open();
                            SqlCommand cmd = new SqlCommand(query, connection);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                connection.Close();
                                string query1 = "select ManufacturerID from [dbo].[Manufacturers] where ManufacturerName='" + model.ManufacturerName + "'";
                                connection.Open();
                                using (SqlCommand command = new SqlCommand(query1, connection))
                                {
                                    using (SqlDataReader reader = command.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            ManufacturerID = Convert.ToInt32(reader["ManufacturerID"].ToString());
                                        }
                                    }
                                }
                                connection.Close();
                                ViewBag.SuccessMessage = "Manufacturer Add Successfully";
                                if (Command == "save")
                                    return RedirectToAction("ManufacturerList", "DemoManufacturer");
                                else
                                {
                                    return RedirectToAction("EditManufacturer", new { id = ManufacturerID });
                                }

                            }
                        }
                        else
                        {
                            ViewBag.SuccessMessage = "Manufacturer Name Already Exist.";
                            return View("~/Plugins/Widgets.BasicPlugins/Views/Manufacturer/AddManufacturer.cshtml", Manufacturer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/Manufacturer/AddManufacturer.cshtml", Manufacturer);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/Manufacturer/AddManufacturer.cshtml", Manufacturer);
        }
        [HttpGet]
        public ActionResult EditManufacturer(int id)
        {
            Manufacturer Manufacturer = new();
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    string query = "select * from [dbo].[Manufacturers] where IsActive=1 and ManufacturerID='" + id + "'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Manufacturer.ManufacturerName = reader["ManufacturerName"].ToString();
                                Manufacturer.ManufacturerID = Convert.ToInt32(reader["ManufacturerID"].ToString());
                                Manufacturer.IsPublished = Convert.ToBoolean(reader["IsPublished"].ToString()) == true ? true : false;
                                Manufacturer.Displayorder = Convert.ToInt32(reader["Displayorder"].ToString());
                                if (reader["Description"].ToString() != "")
                                    Manufacturer.Description = HttpUtility.UrlDecode(reader["Description"].ToString());
                                else
                                    Manufacturer.Description = "";

                            }
                            connection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/Manufacturer/EditManufacturer.cshtml", Manufacturer);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/Manufacturer/EditManufacturer.cshtml", Manufacturer);
        }
        [HttpPost]
        public ActionResult EditManufacturer(Manufacturer Manufacturer, string Command)
        {
            // GetAllMethods(product);
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.SuccessMessage = "Enter required fields.";
                    return View("~/Plugins/Widgets.BasicPlugins/Views/Manufacturer/EditManufacturer.cshtml", Manufacturer);
                }
                else
                {
                    int ManufacturerID = 0;
                    int Count = 0;
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string q = "select Count(*) as Count from [dbo].[Manufacturers] where ManufacturerName='" + Manufacturer.ManufacturerName + "'and ManufacturerID!='" + Manufacturer.ManufacturerID + "'";

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
                            if (Manufacturer.Description != null)
                                Manufacturer.Description = HttpUtility.UrlEncode(Manufacturer.Description);

                            string query = "Update Manufacturers set ManufacturerName='" + Manufacturer.ManufacturerName + "',Description='" + Manufacturer.Description + "',IsPublished='" + Manufacturer.IsPublished + "',Displayorder='" + Manufacturer.Displayorder + "',IsActive=1,UpdatedDate=GETDATE() where ManufacturerID='" + Manufacturer.ManufacturerID + "'";
                            SqlCommand cmd = new SqlCommand(query, connection);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                ViewBag.SuccessMessage = "Updated Manufacturer Successful";
                                if (Command == "save")
                                    return RedirectToAction("ManufacturerList", "DemoManufacturer");
                                else
                                {
                                    return RedirectToAction("EditManufacturer", new { id = Manufacturer.ManufacturerID });
                                }
                            }
                        }
                        else
                        {
                            ViewBag.SuccessMessage = "Manufacturer Name Already Exist.";
                            return View("~/Plugins/Widgets.BasicPlugins/Views/Manufacturer/EditManufacturer.cshtml", Manufacturer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/Manufacturer/EditManufacturer.cshtml", Manufacturer);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/Manufacturer/EditManufacturer.cshtml", Manufacturer);
        }
        [HttpGet]
        public ActionResult DeleteManufacturer(string data)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                {
                    connection.Open();
                    foreach (var id in data.Split(","))
                    {
                        string query = "Update Manufacturers set IsActive=0 where ManufacturerID='" + id + "'";
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
                    string query = "Update Manufacturers set IsActive=0 where ManufacturerID='" + id + "'";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        ViewBag.SuccessMessage = "Manufacturer Deleted Successfully.";
                        return RedirectToAction("ManufacturerList", "DemoManufacturer");
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return RedirectToAction("ManufacturerList", "DemoManufacturer");
            }

            return RedirectToAction("ManufacturerList", "DemoManufacturer");
        }
    }
}