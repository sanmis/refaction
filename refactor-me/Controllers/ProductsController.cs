using System;
using System.Web.Http;
using refactor_me.Logic;
using refactor_me.ViewModels;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private readonly IProductLibrary _productLibrary;

        public ProductsController(IProductLibrary productLibrary)
        {
            _productLibrary = productLibrary;
        }

        [Route("all")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                var result = _productLibrary.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException?.Message + ex.InnerException?.StackTrace);
            }
        }

        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetProduct(Guid id)
        {
            try
            {
                var result = _productLibrary.Get(id, null);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException?.Message + ex.InnerException?.StackTrace);
            }
        }

        [Route("search/{name}")]
        [HttpGet]
        public IHttpActionResult SearchProduct(string name)
        {
            try
            {
                var result = _productLibrary.Get(Guid.Empty, name);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException?.Message + ex.InnerException?.StackTrace);
            }
        }

        [Route("create")]
        [HttpPost]
        public IHttpActionResult Create(ProductView product)
        {
            try
            {
                _productLibrary.Create(product);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException?.Message + ex.InnerException?.StackTrace);
            }
        }

        [Route("update")]
        [HttpPut]
        public IHttpActionResult Update(ProductView product)
        {
            try
            {
                _productLibrary.Update(product);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException?.Message + ex.InnerException?.StackTrace);
            }
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            try
            {
                _productLibrary.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message + ex.InnerException?.Message + ex.InnerException?.StackTrace);
            }
        }
    }
}
