
using System.Net;
using System.Web;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
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
    public class DemoSpecificationAttributeController : BasePluginController
    {

        [HttpGet]
        public ActionResult SpecificationAttributeList()
        {

            List<SpecificationAttribute> SpecificationAttributes = new();
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();
                    string Query = "";
                    Query = "SELECT * FROM SpecificationAttributes AS C WHERE IsActive=1";
                    
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SpecificationAttributes.Add(new SpecificationAttribute
                                {
                                    SpecificationAttributeName = reader["SpecificationAttributeName"].ToString(),
                                    SpecificationAttributeID = Convert.ToInt32(reader["SpecificationAttributeID"].ToString()),
                                    Displayorder = Convert.ToInt32(reader["Displayorder"].ToString()),
                                });

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/SpecificationAttributeList.cshtml", SpecificationAttributes);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/SpecificationAttributeList.cshtml", SpecificationAttributes);
        }
        //[HttpGet]
        //public ActionResult Search(string SpecificationAttributeName = "", string published = "All")
        //{
        //    List<SpecificationAttribute> SpecificationAttributes = new();
        //    try
        //    {
        //        string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";

        //        using (SqlConnection connection = new SqlConnection(constr))
        //        {
        //            connection.Open();
        //            string Query = "";
        //            Query = "SELECT * FROM SpecificationAttributes AS C WHERE IsActive=1";
        //            if (SpecificationAttributeName != "" && SpecificationAttributeName != null)
        //            {
        //                Query += " AND C.SpecificationAttributeName LIKE '%" + SpecificationAttributeName + "%'";
        //            }
        //            if (published != "All")
        //            {
        //                int Ipublished = Convert.ToInt32(published);
        //                Query += " AND C.IsPublished =" + Ipublished + "";
        //            }
        //            using (SqlCommand command = new SqlCommand(Query, connection))
        //            {
        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        SpecificationAttributes.Add(new SpecificationAttribute
        //                        {
        //                            SpecificationAttributeName = reader["SpecificationAttributeName"].ToString(),
        //                            SpecificationAttributeID = Convert.ToInt32(reader["SpecificationAttributeID"].ToString()),
        //                        });

        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.SuccessMessage = ex.Message.ToString();
        //        return Json(new { data = SpecificationAttributes, status = "error" });
        //    }
        //    return Json(new { data = SpecificationAttributes, status = "success" });
        //}
        [HttpGet]
        public ActionResult AddSpecificationAttribute(int id = 0)
        {
            SpecificationAttribute SpecificationAttribute = new();
            return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/AddSpecificationAttribute.cshtml", SpecificationAttribute);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddSpecificationAttribute(SpecificationAttribute model, string Command)
        {
            SpecificationAttribute SpecificationAttribute = new();
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/AddSpecificationAttribute.cshtml", SpecificationAttribute);
                }
                else
                {
                    ViewBag.SuccessMessage = "";
                    int SpecificationAttributeID = 0;
                    int Count = 0;
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string q = "select Count(*) as Count from [dbo].[SpecificationAttributes] where IsActive=1 and SpecificationAttributeName='" + model.SpecificationAttributeName.Trim() + "'";
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
                            
                            string query = "INSERT INTO [SpecificationAttributes] (SpecificationAttributeName, Displayorder, IsActive,CreatedDate,UpdatedDate) VALUES  ('" + model.SpecificationAttributeName.Trim() + "','" + model.Displayorder + "','" + 1 + "','" + DateTime.Now + "','" + DateTime.Now + "')";
                            connection.Open();
                            SqlCommand cmd = new SqlCommand(query, connection);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                connection.Close();
                                string query1 = "select SpecificationAttributeID from [dbo].[SpecificationAttributes] where IsActive=1 and SpecificationAttributeName='" + model.SpecificationAttributeName.Trim() + "'";
                                connection.Open();
                                using (SqlCommand command = new SqlCommand(query1, connection))
                                {
                                    using (SqlDataReader reader = command.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            SpecificationAttributeID = Convert.ToInt32(reader["SpecificationAttributeID"].ToString());
                                        }
                                    }
                                }
                                connection.Close();
                                ViewBag.SuccessMessage = "SpecificationAttribute Add Successfully";
                                if (Command == "save")
                                    return RedirectToAction("SpecificationAttributeList", "DemoSpecificationAttribute");
                                else
                                {
                                    return RedirectToAction("EditSpecificationAttribute", new { id = SpecificationAttributeID });
                                }

                            }
                        }
                        else
                        {
                            ViewBag.SuccessMessage = "Name already exist.";
                            return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/AddSpecificationAttribute.cshtml", SpecificationAttribute);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/AddSpecificationAttribute.cshtml", SpecificationAttribute);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/AddSpecificationAttribute.cshtml", SpecificationAttribute);
        }
        [HttpGet]
        public ActionResult EditSpecificationAttribute(int id)
        {
            SpecificationAttribute SpecificationAttribute = new();
            try
            {
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    string query = "select * from [dbo].[SpecificationAttributes] where IsActive=1 and SpecificationAttributeID='" + id + "'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SpecificationAttribute.SpecificationAttributeName = reader["SpecificationAttributeName"].ToString();
                                SpecificationAttribute.SpecificationAttributeID = Convert.ToInt32(reader["SpecificationAttributeID"].ToString());
                                SpecificationAttribute.Displayorder = Convert.ToInt32(reader["Displayorder"].ToString());

                            }
                            connection.Close();
                        }
                    }
                    string Iquery = "select * from [dbo].[Options] where IsActive=1 and SpecificationAttributeID='" + id + "'";
                    using (SqlCommand commands = new SqlCommand(Iquery, connection))
                    {
                        connection.Open();
                        List<Options> List = new();
                        using (SqlDataReader readers = commands.ExecuteReader())
                        {
                            while (readers.Read())
                            {
                                List.Add(new Options()
                                {
                                    OptionID = Convert.ToInt32(readers["OptionID"].ToString()),
                                    OptionName = readers["OptionName"].ToString(),
                                    RGBcolor = readers["RGBcolor"].ToString(),
                                    Displayorder = Convert.ToInt32(readers["DisplayOrder"].ToString()),
                                    IsSpecifycolor = Convert.ToBoolean(readers["IsSpecifycolor"].ToString()) == true ? true : false,
                                }); 

                            }
                            SpecificationAttribute.Options = List;
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/EditSpecificationAttribute.cshtml", SpecificationAttribute);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/EditSpecificationAttribute.cshtml", SpecificationAttribute);
        }
        [HttpPost]
        public ActionResult EditSpecificationAttribute(SpecificationAttribute SpecificationAttribute, string Command)
        {
            // GetAllMethods(product);
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.SuccessMessage = "Enter required fields.";
                    return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/EditSpecificationAttribute.cshtml", SpecificationAttribute);
                }
                else
                {
                    int SpecificationAttributeID = 0;
                    int Count = 0;
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string q = "select Count(*) as Count from [dbo].[SpecificationAttributes] where IsActive=1 and SpecificationAttributeName='" + SpecificationAttribute.SpecificationAttributeName.Trim() + "'and SpecificationAttributeID!='" + SpecificationAttribute.SpecificationAttributeID + "'";

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
                           
                            string query = "Update SpecificationAttributes set SpecificationAttributeName='" + SpecificationAttribute.SpecificationAttributeName.Trim() + "',Displayorder='" + SpecificationAttribute.Displayorder + "',IsActive=1,UpdatedDate=GETDATE() where SpecificationAttributeID='" + SpecificationAttribute.SpecificationAttributeID + "'";
                            SqlCommand cmd = new SqlCommand(query, connection);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                ViewBag.SuccessMessage = "Updated SpecificationAttribute Successful";
                                if (Command == "save")
                                    return RedirectToAction("SpecificationAttributeList", "DemoSpecificationAttribute");
                                else
                                {
                                    return RedirectToAction("EditSpecificationAttribute", new { id = SpecificationAttribute.SpecificationAttributeID });
                                }
                            }
                        }
                        else
                        {
                            ViewBag.SuccessMessage = "Name already exist.";
                            return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/EditSpecificationAttribute.cshtml", SpecificationAttribute);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/EditSpecificationAttribute.cshtml", SpecificationAttribute);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/EditSpecificationAttribute.cshtml", SpecificationAttribute);
        }
        [HttpGet]
        public ActionResult DeleteSpecificationAttribute(string data)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                {
                    connection.Open();
                    foreach (var id in data.Split(","))
                    {
                        string query = "Update SpecificationAttributes set IsActive=0 where SpecificationAttributeID='" + id + "'";
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
                    string query = "Update SpecificationAttributes set IsActive=0 where SpecificationAttributeID='" + id + "'";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        ViewBag.SuccessMessage = "SpecificationAttribute Deleted Successfully.";
                        return RedirectToAction("SpecificationAttributeList", "DemoSpecificationAttribute");
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return RedirectToAction("SpecificationAttributeList", "DemoSpecificationAttribute");
            }

            return RedirectToAction("SpecificationAttributeList", "DemoSpecificationAttribute");
        }

        [HttpGet]
        public ActionResult AddOption(int SpecificationAttributeId)
        {
            Options Options = new();
            Options.SpecificationAttributeID = SpecificationAttributeId;
            return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/AddOption.cshtml", Options);
        }

        [HttpPost]
        public ActionResult AddOption(Options model)
        {
            Options Options = new();
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/AddOption.cshtml", Options);
                }
                else
                {
                    ViewBag.SuccessMessage = "";
                    int Count = 0;
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string q = "select Count(*) as Count from [dbo].[Options] where IsActive=1 and OptionName  ='" + model.OptionName.Trim() + "'";
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
                            string query = "INSERT INTO [Options] (SpecificationAttributeID, OptionName, IsSpecifycolor,RGBcolor,Displayorder,IsActive,CreatedDate,UpdatedDate) VALUES  ('" + model.SpecificationAttributeID + "','" + model.OptionName.Trim() + "','" + model.IsSpecifycolor + "','" + model.RGBcolor + "','" + model.Displayorder + "','" + 1 + "','" + DateTime.Now + "','" + DateTime.Now + "')";
                            connection.Open();
                            SqlCommand cmd = new SqlCommand(query, connection);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                return RedirectToAction("EditSpecificationAttribute", new { id = model.SpecificationAttributeID });
                            }
                            connection.Close();
                        }
                        else
                        {
                            ViewBag.SuccessMessage = "Name already exist.";
                            return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/AddOption.cshtml", Options);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/AddOption.cshtml", Options);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/AddOption.cshtml", Options);
        }
        [HttpGet]
        public ActionResult EditOption(int SpecificationAttributeId, int OptionID)
        {
            Options Options = new();
            try
            {
                Options.SpecificationAttributeID = SpecificationAttributeId;
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    string query = "select * from [dbo].[Options] where IsActive=1 and OptionID='" + OptionID + "'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader readers = command.ExecuteReader())
                        {
                            while (readers.Read())
                            {
                                Options.OptionID = Convert.ToInt32(readers["OptionID"].ToString());
                                Options.OptionName = readers["OptionName"].ToString();
                                Options.Displayorder = Convert.ToInt32(readers["DisplayOrder"].ToString());
                                Options.RGBcolor = readers["RGBcolor"].ToString();
                                Options.IsSpecifycolor = Convert.ToBoolean(readers["IsSpecifycolor"].ToString()) == true ? true : false;
                            }
                            connection.Close();
                        }
                    }
                }
                return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/EditOption.cshtml", Options);
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/EditOption.cshtml", Options);
            }
        }
        [HttpPost]
        public ActionResult EditOption(Options model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.SuccessMessage = "Enter required fields.";
                    return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/EditOption.cshtml", model);
                }
                else
                {
                   
                    int Count = 0;
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string q = "select Count(*) as Count from [dbo].[Options] where IsActive=1 and OptionName='" + model.OptionName.Trim() + "'and OptionID!='" + model.OptionID + "'";

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
                            
                            string query = "Update Options set OptionName='" + model.OptionName.Trim() + "',IsSpecifycolor='" + model.IsSpecifycolor + "',RGBcolor='" + model.RGBcolor + "',Displayorder='" + model.Displayorder + "',IsActive=1,UpdatedDate=GETDATE() where OptionID='" + model.OptionID + "'";
                            SqlCommand cmd = new SqlCommand(query, connection);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                    return RedirectToAction("EditSpecificationAttribute", new { id = model.SpecificationAttributeID });
                            }
                        }
                        else
                        {
                            ViewBag.SuccessMessage = "Name already exist.";
                            return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/EditOption.cshtml", model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/EditOption.cshtml", model);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/SpecificationAttribute/EditOption.cshtml", model);
        }
        [HttpGet]
        public ActionResult DeleteOption(int id,int ids)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                {
                    connection.Open();
                    string query = "Update Options set IsActive=0 where OptionID='" + id + "'";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        return RedirectToAction("EditSpecificationAttribute", new { id = ids });
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return RedirectToAction("EditSpecificationAttribute", new { id = ids });
            }
            return RedirectToAction("EditSpecificationAttribute", new { id = ids });
        }
    }
}