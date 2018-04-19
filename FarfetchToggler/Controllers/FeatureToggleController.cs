using FarfetchBusinessEntities;
using FarfetchBusinessServices.ServicesInterfaces;
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
    [RoutePrefix("api/FeatureToggles")]
    public class FeatureToggleController : ApiController
    {

        private readonly IFeatureToggleServices _featureToggleServices;

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize FeatureToggle service instance
        /// </summary>
        public FeatureToggleController(IFeatureToggleServices featureToggleServices)
        {
            _featureToggleServices = featureToggleServices;
        }

        #endregion


        /// <summary>
        /// Gets a collection of Feature Toggles
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        // GET api/FeatureToggles
        [AuthorizationRequired]
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<FeatureToggleEntity>))]
        public HttpResponseMessage GetAll()
        {
            var featureToggles = _featureToggleServices.GetAllFeatureToggles();
            if (featureToggles != null)
            {
                var featureToggleEntities = featureToggles as List<FeatureToggleEntity> ?? featureToggles.ToList();
                if (featureToggleEntities.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, featureToggleEntities);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Feature toggles not found");
        }

        /// <summary>
        /// Creates a specific Feature Toggle
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        // POST api/FeatureToggles
        [AuthorizationRequired]
        [HttpPost]
        [Route("")]
        [ResponseType(typeof(FeatureToggleEntity))]
        public IHttpActionResult Create([FromBody]FeatureToggleEntity featureToggleEntity)
        {
            if (featureToggleEntity == null)
                return BadRequest();

            string ftName = _featureToggleServices.CreateFeatureToggle(featureToggleEntity);

            if (!string.IsNullOrEmpty(ftName))
            {
                var createdFeatureToggle = _featureToggleServices.GetFeatureToggleByName(ftName);
                return Created(Url.Link("DefaultApi", new { controller = "Services", id = createdFeatureToggle.Name }), createdFeatureToggle);
            }
            else
                return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Gets a specific Feature Toggle
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        // GET api/FeatureToggles/isbuttonRed
        [AuthorizationRequired]
        [HttpGet]
        [Route("{name}")]
        [ResponseType(typeof(FeatureToggleEntity))]
        public IHttpActionResult GetByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            var featureToggle = _featureToggleServices.GetFeatureToggleByName(name);
            if (featureToggle != null)
                return Ok(featureToggle);
            else
                return NotFound();


        }


        /// <summary>
        /// Updates a Feature Toggle
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        // PUT api/FeatureToggles/isbuttonRed
        [AuthorizationRequired]
        [HttpPut]
        [Route("{name}")]
        [ResponseType(typeof(FeatureToggleEntity))]
        public IHttpActionResult Update(string name, [FromBody]FeatureToggleEntity featureToggleEntity)
        {
            if (string.IsNullOrEmpty(name) || featureToggleEntity == null)
                return BadRequest();

            bool result = _featureToggleServices.UpdateFeatureToggle(name, featureToggleEntity.Value);

            if (result)
            {
                var updatedFeatureToggle = _featureToggleServices.GetFeatureToggleByName(name);
                return Ok(updatedFeatureToggle);
            }
            else
                return StatusCode(HttpStatusCode.NoContent);

        }



        /// <summary>
        /// Gets services configurations for a specific toggle
        /// </summary>
        /// <param name="name"> Name of the FeatureToggle</param>
        /// <returns>Collection of ServiceToggleEntity></returns>
        // GET api/FeatureToggles/isbuttonRed/services
        [AuthorizationRequired]
        [HttpGet]
        [Route("{name}/services")]
        [ResponseType(typeof(IEnumerable<ServiceToggleEntity>))]
        public IHttpActionResult GetFeatureToggleServicesConfigurations(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            if (_featureToggleServices.GetFeatureToggleByName(name) == null)
                return NotFound();

            IEnumerable<ServiceToggleEntity> result = _featureToggleServices.GetFeatureToggleServicesConfigurations(name);

            if (result !=null)
                return Ok(result);
            else
                return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Gets services configurations for a specific toggle
        /// </summary>
        /// <param name="name"> Name of the FeatureToggle</param>
        /// <returns></returns>
        // GET api/FeatureToggles/isbuttonRed/services/1
        [AuthorizationRequired]
        [HttpGet]
        [Route("{name}/services/{serviceId}")]
        [ResponseType(typeof(ServiceToggleEntity))]
        public IHttpActionResult GetFeatureToggleServiceConfiguration(string name,int serviceId)
        {
            if (string.IsNullOrEmpty(name) || serviceId <= 0)
                return BadRequest();

            if (_featureToggleServices.GetFeatureToggleByName(name) == null)
                return NotFound();

            ServiceToggleEntity result = _featureToggleServices.GetFeatureToggleServiceConfiguration(name, serviceId);

            if (result!=null)
                return Ok(result);
            else
                return NotFound();
        }


        /// <summary>
        /// Creates or updates a specific service configuration for a specific toggle
        /// </summary>
        /// <param name="name"> Name of the FeatureToggle</param>
        ///<param name="serviceId">The Id of the service</param>
        /// <returns></returns>
        // PUT api/FeatureToggles/isbuttonRed/services/{serviceId}
        [AuthorizationRequired]
        [HttpPut]
        [Route("{name}/services/{serviceId}")]
        [ResponseType(typeof(ServiceToggleEntity))]
        public IHttpActionResult UpsertFeatureToggleServiceConfiguration(string name, int serviceId, [FromBody]ServiceToggleEntity entity)
        {
            if (string.IsNullOrEmpty(name) || serviceId <= 0 || entity==null)
                return BadRequest();

            if (_featureToggleServices.GetFeatureToggleByName(name) == null)
                return NotFound();

            if (_featureToggleServices.GetFeatureToggleServiceConfiguration(name, serviceId) == null)
            {
                _featureToggleServices.InsertFeatureToggleServiceConfiguration(name, entity);
                var ServiceConfiguration = _featureToggleServices.GetFeatureToggleServiceConfiguration(name, serviceId);
                return Created(Url.Link("DefaultApi", new { controller = "FeatureToggles", id = name+"/Services/"+ serviceId }), ServiceConfiguration);
            }
            else {
                _featureToggleServices.UpdateFeatureToggleServiceConfiguration(name, serviceId, entity);
                var ServiceConfiguration = _featureToggleServices.GetFeatureToggleServiceConfiguration(name, serviceId);
                return Ok(ServiceConfiguration);
            }

           
        }

        /// <summary>
        /// Deletes a specific service configuration for a specific toggle
        /// </summary>
        /// <param name="name"> Name of the FeatureToggle</param>
        ///<param name="serviceId">The Id of the service</param>
        /// <returns></returns>
        // DELETE api/FeatureToggles/isbuttonRed/services/{serviceId}
        [AuthorizationRequired]
        [HttpDelete]
        [Route("{name}/services/{serviceId}")]
        [ResponseType(typeof(ServiceToggleEntity))]
        public IHttpActionResult DeleteFeatureToggleServiceConfiguration(string name, int serviceId)
        {
            if (string.IsNullOrEmpty(name) || serviceId <= 0)
                return BadRequest();

            if (_featureToggleServices.GetFeatureToggleServiceConfiguration(name,serviceId) == null)
                return NotFound();

            bool result = _featureToggleServices.DeleteFeatureToggleServiceConfiguration(serviceId,name);

            if (result)
            {
                return Ok();
            }
            else {
                return StatusCode(HttpStatusCode.NoContent);
            }
           
        }


        /// <summary>
        /// Gets Service Available Feature Toggles
        /// </summary>
        /// <param name="serviceId"> The Id of the service</param>
        ///<param name="serviceVersion">The version of the service</param>
        /// <returns></returns>
        // DELETE api/FeatureToggles/isbuttonRed/services/{serviceId}
        
        [HttpGet]
        [Route("{serviceId}/{serviceVersion}")]
        [ResponseType(typeof(IEnumerable<FeatureToggleEntity>))]
        public IHttpActionResult GetServiceAvailableToggles(int serviceId, string serviceVersion)
        {
            if (string.IsNullOrEmpty(serviceVersion) || serviceId <= 0)
                return BadRequest();

            var featureToggleEntities = _featureToggleServices.GetServiceAvailableToggles(serviceId, serviceVersion);
            if (featureToggleEntities!=null && featureToggleEntities.Any())
                return Ok(featureToggleEntities);


            return NotFound();

        }




    }
}
