using BusinessServices;
using FarfetchBusinessEntities;
using FarfetchBusinessServices;
using FarfetchToggler.ActionFilters;
using FarfetchToggler.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FarfetchToggler.Controllers
{
    [AuthorizationRequired]
    [RoutePrefix("api/Services")]
    public class ServiceController : ApiController
    {

        private readonly IServiceApplicationServices _serviceApplicationServices;

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize service/application service instance
        /// </summary>
        public ServiceController(IServiceApplicationServices serviceApplicationServices)
        {
            _serviceApplicationServices = serviceApplicationServices;
        }

        #endregion

        [HttpGet]
        [Route("")]
        // GET api/services
        public HttpResponseMessage GetAll()
        {
            var services = _serviceApplicationServices.GetAllServices();
            if (services != null)
            {
                var serviceEntities = services as List<ServiceEntity> ?? services.ToList();
                if (serviceEntities.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, serviceEntities);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Services not found");
        }

        [HttpGet]
        [Route("{serviceId}")]
        // GET api/services/5
        public IHttpActionResult GetById(int serviceId)
        {
            if (serviceId == 0) {
                return BadRequest();
            }

            var service = _serviceApplicationServices.GetServiceById(serviceId);
            if (service != null)
                return Ok(service);
            else
                return NotFound();


        }

        [HttpPost]
        [Route("")]
        // POST api/services
        public IHttpActionResult Create([FromBody]ServiceEntity serviceEntity)
        {
            if (serviceEntity == null)
                return BadRequest();

            int serviceId = _serviceApplicationServices.CreateService(serviceEntity);

            if (serviceId > 0)
            {
                var createdService = _serviceApplicationServices.GetServiceById(serviceId);
                return Created(Url.Link("DefaultApi", new { controller = "Services", id = createdService.ServiceId.ToString() }), createdService);
            }
            else
                return StatusCode(HttpStatusCode.NoContent);


        }

        [HttpPut]
        [Route("{serviceId}")]
        // PUT api/services/5
        public IHttpActionResult Update(int serviceId, [FromBody]ServiceEntity serviceEntity)
        {
            if (serviceId == 0 || serviceEntity == null)
                return BadRequest();

            bool result = _serviceApplicationServices.UpdateService(serviceId, serviceEntity);

            if (result)
            {
                var updatedService = _serviceApplicationServices.GetServiceById(serviceId);
                return Ok(updatedService);
            }
            else
                return StatusCode(HttpStatusCode.NoContent);

        }

        [HttpDelete]
        [Route("{serviceId}")]
        // DELETE api/services/5
        public IHttpActionResult Delete(int serviceId)
        {
            if (serviceId == 0)
                return BadRequest();

            bool result = _serviceApplicationServices.DeleteService(serviceId);

            if (result)
                return Ok();
            else
                return StatusCode(HttpStatusCode.NoContent);
        }

    }
}
