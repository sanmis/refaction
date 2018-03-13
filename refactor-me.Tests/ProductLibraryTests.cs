using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using Moq;
using NUnit.Framework;
using refactor_me.Data;
using refactor_me.Data.Interface;
using refactor_me.Logic;
using refactor_me.Logic.Interface;
using refactor_me.Models;
using refactor_me.ViewModels;

namespace refactor_me.Tests
{
    [TestFixture]
    public class ProductLibraryTests
    {
        IProductLibrary _productLibrary;
        IGenericRepository<Product> _productRepository;
        IMapper _mapper;
        IPersistanceFactory _persistanceFactory;
        List<Product> _products;
        

        [SetUp]
        public void Setup()
        {
            _products = SetupProducts();
            _productRepository = SetupProductRepository();
            var mapperConfig = new MapperConfiguration(cfg =>
                         cfg.AddProfiles(new[] { "refactor-me.Logic", "refactor-me.Data" }));
            _mapper = new Mapper(mapperConfig);
            _persistanceFactory = new Mock<IPersistanceFactory>().Object;
            _productLibrary = new ProductLibrary(_persistanceFactory, _mapper, _productRepository);
        }

        public List<Product> SetupProducts()
        {
            return new List<Product>()
            {
                new Product() {Id = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"), Name = "Samsung Galaxy S7", Price = new decimal(1024.99), Description = "Newest mobile product from Samsung.", DeliveryPrice = new decimal(16.99) },
                new Product() {Id = Guid.Parse("DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3"), Name = "Apple iPhone 6S", Price = new decimal(1299.99), Description = "Newest mobile product from Apple.", DeliveryPrice = new decimal(15.99) }
            };
        }

        public IGenericRepository<Product> SetupProductRepository()
        {
            var repo = new Mock<IGenericRepository<Product>>();

            repo.Setup(r => r.GetAll()).Returns(_products.AsQueryable());

            repo.Setup(r => r.Find(It.IsAny<Expression<Func<Product, bool>>>()))
                .Returns<Expression<Func<Product, bool>>>(e => _products.Where(e.Compile()).AsQueryable());

            repo.Setup(r => r.Add(It.IsAny<Product>())).Callback(new Action<Product>(
                newProduct =>
                {
                    newProduct.Id = Guid.NewGuid();
                    newProduct.DeliveryPrice = new decimal(13.99);
                    newProduct.Price = new decimal(1311.99);
                    newProduct.Name = "Moto";
                    newProduct.Description = "Very costly phone!!";
                    _products.Add(newProduct);
                }
                ));

            repo.Setup(r => r.Update(It.IsAny<Product>())).Callback(new Action<Product>(x =>
            {
                var oldProduct = _products.Find(a => a.Id == x.Id);
                oldProduct.DeliveryPrice = new decimal(7.99);
                oldProduct = x;
            }));

            repo.Setup(r => r.Delete(It.IsAny<Product>())).Callback(new Action<Product>(x =>
            {
                var productToRemove = _products.Find(a => a.Id == x.Id);
                if (productToRemove != null)
                    _products.Remove(productToRemove);
            }));

            return repo.Object;
        }
        

        [Test]
        public void GetAll()
        {
            var products = _productLibrary.GetAll();
            var countShouldBe = _products.Count;
            var countIs = products.Count;
            Assert.AreEqual(countShouldBe, countIs);
        }

        [Test]
        public void GetById()
        {
            var products = _productLibrary.Get(Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"), null);
            var product = _products.Find(x => x.Id == Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"));
            Assert.AreEqual(products.First().Id, product.Id);
            Assert.AreEqual(products.First().Name, product.Name);
            Assert.AreEqual(products.First().Description, product.Description);
            Assert.AreEqual(products.First().DeliveryPrice, product.DeliveryPrice);
            Assert.AreEqual(products.First().Price, product.Price);
        }

        [Test]
        public void GetByName()
        {
            var products = _productLibrary.Get(Guid.Empty, "Samsung Galaxy S7");
            var product =  _products.Find(x => x.Name == "Samsung Galaxy S7");
            Assert.AreEqual(products.First().Id, product.Id);
            Assert.AreEqual(products.First().Name, product.Name);
            Assert.AreEqual(products.First().Description, product.Description);
            Assert.AreEqual(products.First().DeliveryPrice, product.DeliveryPrice);
            Assert.AreEqual(products.First().Price, product.Price);
        }

        [Test]
        public void Create()
        {
            var countShouldBe = _products.Count + 1;

            var product = new Product() {Id = Guid.NewGuid(), Name = "Nokia", Description = "Old school", Price = new decimal(200), DeliveryPrice = new decimal(0.00)};

            _productLibrary.Create(MapProductToProductView(product));
            var countIs = _products.Count;

            Assert.AreEqual(countIs, countShouldBe);
        }

        [Test]
        public void Update()
        {
            var productExisting = _products.First();
            productExisting.Description = "this is updated";
            _productLibrary.Update(MapProductToProductView(productExisting));

            Assert.That(_products.First().Description, Is.EqualTo(productExisting.Description));
        }

        [Test]
        public void Delete()
        {
            var countShouldBe = _products.Count - 1;
            var productExisting = _products.First();
            _productLibrary.Delete(productExisting.Id);
            var countIs = _products.Count;

            Assert.AreEqual(countIs, countShouldBe);
        }

        #region privateMethods

        private ProductView MapProductToProductView(Product product)
        {
            return new ProductView()
            {
                Id = product.Id,
                Description = product.Description,
                DeliveryPrice = product.DeliveryPrice,
                Price = product.Price,
                Name = product.Name
            };
        }

        #endregion

    }
}
