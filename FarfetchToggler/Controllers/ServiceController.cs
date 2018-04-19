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
using System.Web.Http.Description;

namespace FarfetchToggler.Controllers
{
    
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


        /// <summary>
        /// Gets a collection of Services
        /// </summary>
        /// <returns></returns>
        // GET api/services
        [AuthorizationRequired]
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<ServiceEntity>))]
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

        /// <summary>
        /// Gets a specific of Service
        /// </summary>
        /// <returns></returns>
        // GET api/services/5
        [AuthorizationRequired]
        [HttpGet]
        [Route("{serviceId}")]
        [ResponseType(typeof(ServiceEntity))]
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

        /// <summary>
        /// Creates a service
        /// </summary>
        /// <returns></returns>
        // POST api/services
        [AuthorizationRequired]
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(ServiceEntity))]
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

        /// <summary>
        /// Updates a service
        /// </summary>
        /// <returns></returns>
        // PUT api/services/5
        [AuthorizationRequired]
        [HttpPut]
        [Route("{serviceId}")]
        [ResponseType(typeof(ServiceEntity))]
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

 

    }
}
