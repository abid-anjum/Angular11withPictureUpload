using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AngularTestBackend.Controllers
{
    [RoutePrefix("Api/Country")]
    public class CountryController : ApiController
    {

        Angular11TestEntities objEntity = new Angular11TestEntities();

        [HttpGet]
        [Route("AllCountries")]
        public IHttpActionResult GetCountries()
        {
            try
            {
                var result = (from tblcountry in objEntity.Countries
                             
                              select new
                              {
                                  CountryId = tblcountry.CountryId,
                                  CountryName = tblcountry.CountryName,
                                  CountryCode = tblcountry.Code

                              }).ToList();

                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetCountryById/{countryId}")]
        public IHttpActionResult getCountryById(string countryId)
        {
            try
            {
                Country objEmp = new Country();
                int ID = Convert.ToInt32(countryId);
                try
                {
                    objEmp = objEntity.Countries.Find(ID);
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

        [HttpPost]
        [Route("InsertCountryDetails")]
        public IHttpActionResult PostCountry(Country data)
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
                    objEntity.Countries.Add(data);
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

        [HttpPut]
        [Route("UpdateCountryDetails")]
        public IHttpActionResult PutCountryMaster(Country country)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                try
                {
                    Country objCountry = new Country();
                    objCountry = objEntity.Countries.Find(objCountry.CountryId);
                    if (objCountry != null)
                    {
                        objCountry.CountryName = country.CountryName;
                        objCountry.Code = country.Code;

                    }
                    this.objEntity.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
                return Ok(country);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpDelete]
        [Route("DeleteCountryDetails")]
        public IHttpActionResult DeleteCountry(int id)
        {
            try
            {
                Country country = objEntity.Countries.Find(id);
                if (country == null)
                {
                    return NotFound();
                }
                objEntity.Countries.Remove(country);
                objEntity.SaveChanges();
                return Ok(country);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}