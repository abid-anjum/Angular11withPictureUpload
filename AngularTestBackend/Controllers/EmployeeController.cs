using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace AngularTestBackend.Controllers
{
    [RoutePrefix("Api/Employee")]
    public class EmployeeController : ApiController
    {
        Angular11TestEntities objEntity = new Angular11TestEntities();

        [HttpGet]
        [Route("AllEmployee")]
        public IHttpActionResult GetEmployee()
        {
            try
            {
                var url = HttpContext.Current.Request.Url;

                var result = (from tblemp in objEntity.Employees
                              join tblcountry in objEntity.Countries on tblemp.CountryId equals tblcountry.CountryId

                              select new
                              {
                                  EmployeeId = tblemp.EmployeeId,
                                  EmployeeName = tblemp.EmployeeName,
                                  JobTitle = tblemp.JobTitle,
                                  Salary = tblemp.Salary,
                                  Email = tblemp.Email,
                                  DateOfBirth = tblemp.DateOfBirth,
                                  Picture = url.Scheme + "://" + url.Host + ":" + url.Port + "/Files/" + tblemp.Picture,
                                  CountryName = tblcountry.CountryName,
                                  PictureName = tblemp.Picture
                              }).ToList();

                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpGet]
        [Route("GetEmployeeById/{employeeId}")]
        public IHttpActionResult getEmployeeById(string employeeId)
        {
            try
            {
                Employee objEmp = new Employee();
                int ID = Convert.ToInt32(employeeId);
                try
                {
                    objEmp = objEntity.Employees.Find(ID);
                    if (objEmp == null)
                    {
                        return NotFound();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                return Ok(objEmp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetImage")]
        //download Image file api  
        public HttpResponseMessage GetImage(string image)
        {
            //Create HTTP Response.  
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            //Set the File Path.  
            string filePath = System.Web.HttpContext.Current.Server.MapPath("~/Files/") + image + ".jpg";
            //Check whether File exists.  
            if (!File.Exists(filePath))
            {
                //Throw 404 (Not Found) exception if File not found.  
                response.StatusCode = HttpStatusCode.NotFound;
                response.ReasonPhrase = string.Format("File not found: {0} .", image);
                throw new HttpResponseException(response);
            }
            //Read the File into a Byte Array.  
            byte[] bytes = File.ReadAllBytes(filePath);
            //Set the Response Content.  
            response.Content = new ByteArrayContent(bytes);
            //Set the Response Content Length.  
            response.Content.Headers.ContentLength = bytes.LongLength;
            //Set the Content Disposition Header Value and FileName.  
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = image + ".jpg";
            //Set the File Content Type.  
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(image + ".jpg"));
            return response;
        }

        [HttpPost]
        [Route("InserEmployeeDetails")]
        public IHttpActionResult PostEmaploye(Employee data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                try
                {
                    //data.DateOfBirth = data.DateOfBirth.HasValue ? data.DateOfBirth.Value.AddDays(1) : (DateTime?)null;
                    objEntity.Employees.Add(data);
                    objEntity.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        [Route("InserEmployeeDetailsPicture")]
        public IHttpActionResult PostEmaployewithPic()
        {
            string result = "";
            try
            {
                Employee emp = new Employee();
                var httpRequest = HttpContext.Current.Request;
                emp.EmployeeName = httpRequest.Form["EmployeeName"];

                emp.JobTitle = httpRequest.Form["JobTitle"];
                //emp.Salary = int.Parse(httpRequest.Form["Salary"]);
                emp.Email = httpRequest.Form["Email"];

                //DateTime tempdate = DateTime.ParseExact(httpRequest.Form["DateOfBirth"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                emp.DateOfBirth = DateTime.Now;

                var postedImage = httpRequest.Files["ImageUpload"];
                string imageName = null;

                if (postedImage != null)
                {
                    imageName = new String(Path.GetFileNameWithoutExtension(postedImage.FileName).Take(10).ToArray()).Replace(" ", "-");
                    imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedImage.FileName);
                    var filePath = HttpContext.Current.Server.MapPath("~/Files/" + imageName);
                    postedImage.SaveAs(filePath);
                }
                emp.Picture = imageName;

                emp.CountryId = int.Parse(httpRequest.Form["CountryId"]);

                objEntity.Employees.Add(emp);

                int i = objEntity.SaveChanges();
                if (i > 0)
                {
                    result = "File uploaded sucessfully";
                }
                else
                {
                    result = "File uploaded faild";
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("UpdateEmployeeDetails")]
        public IHttpActionResult PutEmaployeeMaster(Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                try
                {
                    Employee objEmp = new Employee();
                    objEmp = objEntity.Employees.Find(employee.EmployeeId);
                    if (objEmp != null)
                    {
                        objEmp.EmployeeName = employee.EmployeeName;
                        objEmp.DateOfBirth = employee.DateOfBirth;
                        objEmp.JobTitle = employee.JobTitle;
                        objEmp.Salary = employee.Salary;
                        objEmp.CountryId = employee.CountryId;

                    }
                    this.objEntity.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpDelete]
        [Route("DeleteEmployeeDetails")]
        public IHttpActionResult DeleteEmployee(int id)
        {
            try
            {
                Employee emp = objEntity.Employees.Find(id);
                if (emp == null)
                {
                    return NotFound();
                }
                objEntity.Employees.Remove(emp);
                objEntity.SaveChanges();
                return Ok(emp);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("Country")]
        public IQueryable<Country> GetCountry()
        {
            try
            {
                return objEntity.Countries;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
