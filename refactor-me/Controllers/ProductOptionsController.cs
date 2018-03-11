using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using refactor_me.Logic.Interface;
using refactor_me.ViewModels;

namespace refactor_me.Controllers
{
    [RoutePrefix("productOptions")]
    public class ProductOptionsController : ApiController
    {
        private readonly IProductOptionLibrary _productOptionLibrary;

        public ProductOptionsController(IProductOptionLibrary productOptionLibrary)
        {
            _productOptionLibrary = productOptionLibrary;
        }

        [Route("{productId}/options")]
        [HttpGet]
        public IHttpActionResult GetOptions(Guid productId)
        {
            try
            {
                var result = _productOptionLibrary.GetOptions(productId, Guid.Empty);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException?.Message + ex.InnerException?.StackTrace);
            }
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public IHttpActionResult GetOption(Guid productId, Guid id)
        {
            try
            {
                var result = _productOptionLibrary.GetOptions(productId, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException?.Message + ex.InnerException?.StackTrace);
            }
        }

        [Route("create")]
        [HttpPost]
        public IHttpActionResult CreateOption(ProductOptionView option)
        {
            try
            {
                _productOptionLibrary.Create(option);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException?.Message + ex.InnerException?.StackTrace);
            }
        }

        [Route("update")]
        [HttpPut]
        public IHttpActionResult UpdateOption(ProductOptionView option)
        {
            try
            {
                _productOptionLibrary.Update(option);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException?.Message + ex.InnerException?.StackTrace);
            }
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteOption(Guid id)
        {
            try
            {
                _productOptionLibrary.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException?.Message + ex.InnerException?.StackTrace);
            }
        }
    }
}
