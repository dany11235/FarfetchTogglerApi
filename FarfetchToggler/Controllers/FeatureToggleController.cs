using FarfetchBusinessEntities;
using FarfetchBusinessServices.ServicesInterfaces;
using FarfetchToggler.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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


        
        [HttpGet]
        [Route("")]
        // GET api/FeatureToggles
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


        [HttpPost]
        [Route("")]
        // POST api/FeatureToggles
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

        [AuthorizationRequired]
        [HttpGet]
        [Route("{name}")]
        // GET api/FeatureToggles/isbuttonRed
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

        [HttpPut]
        [Route("{name}")]
        // PUT api/FeatureToggles/isbuttonRed
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


        [HttpDelete]
        [Route("{name}")]
        // DELETE api/FeatureToggles/isbuttonRed
        public IHttpActionResult Delete(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest();

            bool result = _featureToggleServices.DeleteFeatureToggle(name);

            if (result)
                return Ok();
            else
                return StatusCode(HttpStatusCode.NoContent);
        }



    }
}
