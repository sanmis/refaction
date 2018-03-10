using System;
using System.Net;
using System.Web.Http;
using refactor_me.Logic;
using refactor_me.Models;
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

        //[Route("all")]
        //[HttpGet]
        //public Products GetAll()
        //{
        //    return new Products();
        //}

        [Route("search")]
        [HttpGet]
        public Products SearchByName(string name)
        {
            return new Products(name);
        }

        //[Route("{id}")]
        //[HttpGet]
        //public Product GetProduct(Guid id)
        //{
        //    var product = new Product(id);
        //    if (product.IsNew)
        //        throw new HttpResponseException(HttpStatusCode.NotFound);

        //    return product;
        //}

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

        //[Route]
        //[HttpPost]
        //public void Create(Product product)
        //{
        //    product.Save();
        //}

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

        //[Route("{id}")]
        //[HttpPut]
        //public void Update(Guid id, Product product)
        //{
        //    var orig = new Product(id)
        //    {
        //        Name = product.Name,
        //        Description = product.Description,
        //        Price = product.Price,
        //        DeliveryPrice = product.DeliveryPrice
        //    };

        //    if (!orig.IsNew)
        //        orig.Save();
        //}

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

        //[Route("{id}")]
        //[HttpDelete]
        //public void Delete(Guid id)
        //{
        //    var product = new Product(id);
        //    product.Delete();
        //}

        [Route("{productId}/options")]
        [HttpGet]
        public ProductOptions GetOptions(Guid productId)
        {
            return new ProductOptions(productId);
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var option = new ProductOption(id);
            if (option.IsNew)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return option;
        }

        [Route("{productId}/options")]
        [HttpPost]
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;
            option.Save();
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public void UpdateOption(Guid id, ProductOption option)
        {
            var orig = new ProductOption(id)
            {
                Name = option.Name,
                Description = option.Description
            };

            if (!orig.IsNew)
                orig.Save();
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
            var opt = new ProductOption(id);
            opt.Delete();
        }
    }
}
