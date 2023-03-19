
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
    public class DemoProductAttributeController : BasePluginController
    {

        [HttpGet]
        public ActionResult ProductAttributeList()
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
            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/ProductAttributeList.cshtml", ProductAttributes);
        }
        [HttpGet]
        public ActionResult Search(string ProductAttributeName = "", string published = "All")
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
                return Json(new { data = ProductAttributes, status = "error" });
            }
            return Json(new { data = ProductAttributes, status = "success" });
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
                            ViewBag.SuccessMessage = "ProductAttribute Name Already Exist.";
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
                    string Iquery = "select * from [dbo].[PredefineValues] where IsActive=1 and ProductAttributeID='" + id + "'";
                    using (SqlCommand commands = new SqlCommand(Iquery, connection))
                    {
                        connection.Open();
                        List<PredefineValues> List = new();
                        using (SqlDataReader readers = commands.ExecuteReader())
                        {
                            while (readers.Read())
                            {
                                List.Add(new PredefineValues()
                                {
                                    PredefinevalueID = Convert.ToInt32(readers["PredefinevalueID"].ToString()),
                                    ValueName = readers["ValueName"].ToString(),
                                    sPriceadjustment = readers["Priceadjustment"].ToString().Replace(".0000", ""),
                                    sWeightadjustment = readers["Weightadjustment"].ToString().Replace(".0000", ""),
                                    Cost = readers["Cost"].ToString() == "" ? 0 : Convert.ToDecimal(readers["Cost"].ToString()),
                                    Displayorder = Convert.ToInt32(readers["DisplayOrder"].ToString()),
                                    IsUsepercentage = Convert.ToBoolean(readers["IsUsepercentage"].ToString()) == true ? true : false,
                                    Ispreselected = Convert.ToBoolean(readers["Ispreselected"].ToString()) == true ? true : false

                                }); 

                            }
                            ProductAttribute.PredefineValues = List;
                        }
                    }
                    connection.Close();
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
        public ActionResult EditProductAttribute(ProductAttribute ProductAttribute, string Command)
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
                            ViewBag.SuccessMessage = "Name already exist.";
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

        [HttpGet]
        public ActionResult AddPredefineValue(int ProductAttributeId)
        {
            PredefineValues predefineValues = new();
            predefineValues.ProductAttributeID = ProductAttributeId;
            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/AddPredefineValue.cshtml", predefineValues);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddPredefineValue(PredefineValues model)
        {
            PredefineValues predefineValues = new();
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/AddPredefineValue.cshtml", predefineValues);
                }
                else
                {
                    ViewBag.SuccessMessage = "";
                    int Count = 0;
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string q = "select Count(*) as Count from [dbo].[PredefineValues] where ValueName='" + model.ValueName + "'";
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
                            string query = "INSERT INTO [PredefineValues] (ProductAttributeID, ValueName, Priceadjustment,IsUsepercentage,Weightadjustment,Cost,Ispreselected,Displayorder,IsActive,CreatedDate,UpdatedDate) VALUES  ('" + model.ProductAttributeID + "','" + model.ValueName.Trim() + "','" + model.Priceadjustment + "','" + model.IsUsepercentage + "','" + model.Weightadjustment + "','" + model.Cost + "','" + model.Ispreselected + "','" + model.Displayorder + "','" + 1 + "','" + DateTime.Now + "','" + DateTime.Now + "')";
                            connection.Open();
                            SqlCommand cmd = new SqlCommand(query, connection);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                return RedirectToAction("EditProductAttribute", new { id = model.ProductAttributeID });
                            }
                            connection.Close();
                        }
                        else
                        {
                            ViewBag.SuccessMessage = "Name already exist.";
                            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/AddPredefineValue.cshtml", predefineValues);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/AddPredefineValue.cshtml", predefineValues);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/AddPredefineValue.cshtml", predefineValues);
        }
        [HttpGet]
        public ActionResult EditPredefineValue(int ProductAttributeId, int PredefinevalueID)
        {
            PredefineValues predefineValues = new();
            try
            {
                predefineValues.ProductAttributeID = ProductAttributeId;
                string constr = @"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True";
                using (SqlConnection connection = new SqlConnection(constr))
                {
                    connection.Open();

                    string query = "select * from [dbo].[PredefineValues] where IsActive=1 and PredefinevalueID='" + PredefinevalueID + "'";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader readers = command.ExecuteReader())
                        {
                            while (readers.Read())
                            {
                                predefineValues.PredefinevalueID = Convert.ToInt32(readers["PredefinevalueID"].ToString());
                                predefineValues.ValueName = readers["ValueName"].ToString();
                                predefineValues.Priceadjustment = readers["Priceadjustment"].ToString() == "" ? 0 : Convert.ToDecimal(readers["Priceadjustment"].ToString());
                                predefineValues.Weightadjustment = readers["Weightadjustment"].ToString() == "" ? 0 : Convert.ToDecimal(readers["Weightadjustment"].ToString());
                                predefineValues.Cost = readers["Cost"].ToString() == "" ? 0 : Convert.ToDecimal(readers["Cost"].ToString());
                                predefineValues.Displayorder = Convert.ToInt32(readers["DisplayOrder"].ToString());
                                predefineValues.IsUsepercentage = Convert.ToBoolean(readers["IsUsepercentage"].ToString()) == true ? true : false;
                                predefineValues.Ispreselected = Convert.ToBoolean(readers["Ispreselected"].ToString()) == true ? true : false;
                            }
                            connection.Close();
                        }
                    }
                }
                return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/EditPredefineValue.cshtml", predefineValues);
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/EditPredefineValue.cshtml", predefineValues);
            }
        }
        [HttpPost]
        public ActionResult EditPredefineValue(PredefineValues model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.SuccessMessage = "Enter required fields.";
                    return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/EditPredefineValue.cshtml", model);
                }
                else
                {
                   
                    int Count = 0;
                    using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                    {
                        connection.Open();
                        string q = "select Count(*) as Count from [dbo].[PredefineValues] where ValueName='" + model.ValueName + "'and PredefinevalueID!='" + model.PredefinevalueID + "'";

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
                            
                            string query = "Update PredefineValues set ValueName='" + model.ValueName + "',Priceadjustment='" + model.Priceadjustment + "',Weightadjustment='" + model.Weightadjustment + "',Ispreselected='" + model.Ispreselected + "',Cost='" + model.Cost + "',IsUsepercentage='" + model.IsUsepercentage + "',Displayorder='" + model.Displayorder + "',IsActive=1,UpdatedDate=GETDATE() where PredefinevalueID='" + model.PredefinevalueID + "'";
                            SqlCommand cmd = new SqlCommand(query, connection);
                            if (cmd.ExecuteNonQuery() == 1)
                            {
                                    return RedirectToAction("EditProductAttribute", new { id = model.ProductAttributeID });
                            }
                        }
                        else
                        {
                            ViewBag.SuccessMessage = "Name already exist.";
                            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/EditPredefineValue.cshtml", model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/EditPredefineValue.cshtml", model);
            }
            return View("~/Plugins/Widgets.BasicPlugins/Views/ProductAttribute/EditPredefineValue.cshtml", model);
        }
        [HttpGet]
        public ActionResult DeletePredefinevalue(int id,int ids)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-9N1RJHQ\SQLEXPRESS;Initial Catalog=NopProduct;Integrated Security=true;Persist Security Info=False;Trust Server Certificate=True"))
                {
                    connection.Open();
                    string query = "Update PredefineValues set IsActive=0 where PredefinevalueID='" + id + "'";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        return RedirectToAction("EditProductAttribute", new { id = ids });
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                ViewBag.SuccessMessage = ex.Message.ToString();
                return RedirectToAction("EditProductAttribute", new { id = ids });
            }
            return RedirectToAction("EditProductAttribute", new { id = ids });
        }
    }
}