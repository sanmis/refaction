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
    public class LogicTests
    {
        IProductLibrary _productLibrary;
        IProductOptionLibrary _productOptionLibrary;
        IGenericRepository<Product> _productRepository;
        IGenericRepository<ProductOption> _productOptionRepository;
        IMapper _mapper;
        IPersistanceFactory _persistanceFactory;
        List<Product> _products;
        List<ProductOption> _productOptions;

        [SetUp]
        public void Setup()
        {
            _products = SetupProducts();
            _productOptions = SetupProductOptions();
            _productRepository = SetupProductRepository();
            _productOptionRepository = SetupProductOptionsRepository();
            var mapperConfig = new MapperConfiguration(cfg =>
                         cfg.AddProfiles(new[] { "refactor-me.Logic", "refactor-me.Data" }));
            _mapper = new Mapper(mapperConfig);
            _persistanceFactory = new Mock<IPersistanceFactory>().Object;
            _productLibrary = new ProductLibrary(_persistanceFactory, _mapper, _productRepository);
            _productOptionLibrary = new ProductOptionLibrary(_persistanceFactory, _mapper, _productOptionRepository);
        }

        public List<Product> SetupProducts()
        {
            return new List<Product>()
            {
                new Product() {Id = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"), Name = "Samsung Galaxy S7", Price = new decimal(1024.99), Description = "Newest mobile product from Samsung.", DeliveryPrice = new decimal(16.99) },
                new Product() {Id = Guid.Parse("DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3"), Name = "Apple iPhone 6S", Price = new decimal(1299.99), Description = "Newest mobile product from Apple.", DeliveryPrice = new decimal(15.99) }
            };
        }

        public List<ProductOption> SetupProductOptions()
        {
            return new List<ProductOption>()
            {
                new ProductOption() {Id = Guid.Parse("0643CCF0-AB00-4862-B3C5-40E2731ABCC9"), Name = "White", ProductId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"), Description = "White Samsung Galaxy S7"},
                new ProductOption() {Id = Guid.Parse("A21D5777-A655-4020-B431-624BB331E9A2"), Name = "Black", ProductId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"), Description = "Black Samsung Galaxy S7"},
                new ProductOption() {Id = Guid.Parse("5C2996AB-54AD-4999-92D2-89245682D534"), Name = "Rose Gold", ProductId = Guid.Parse("DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3"), Description = "Gold Apple iPhone 6S"},
                new ProductOption() {Id = Guid.Parse("9AE6F477-A010-4EC9-B6A8-92A85D6C5F03"), Name = "White", ProductId = Guid.Parse("DE1287C0-4B15-4A7B-9D8A-DD21B3CAFEC3"), Description = "White Apple iPhone 6S"},
                new ProductOption() {Id = Guid.Parse("4E2BC5F2-699A-4C42-802E-CE4B4D2AC0EF"), Name = "Black", ProductId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"), Description = "Black Apple iPhone 6S"},
            };
        }

        public IGenericRepository<Product> SetupProductRepository()
        {
            var repo = new Mock<IGenericRepository<Product>>();

            repo.Setup(r => r.GetAll()).Returns(_products.AsQueryable());

            repo.Setup(r => r.Find(It.IsAny<Expression<Func<Product, bool>>>()))
                .Returns(new Func<Guid, Product>(id => _products.Find(p => p.Id.Equals(id))));

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

        public IGenericRepository<ProductOption> SetupProductOptionsRepository()
        {
            var repo = new Mock<IGenericRepository<ProductOption>>();

            repo.Setup(r => r.GetAll()).Returns(_productOptions.AsQueryable());

            return repo.Object;
        }

        [Test]
        public void GetAll()
        {
            var products = _productLibrary.GetAll();
            var countShouldBe = _products.Count;
            var countIs = products.Count;
        }

        public void GetById()
        {
            var products = _productLibrary.Get(Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"), null);
        }

        public void GetByName()
        {
            var products = _productLibrary.Get(Guid.Empty, "Samsung Galaxy S7");
        }

        public void Create()
        {
            var product = new Product() {Id = Guid.NewGuid(), Name = "Nokia", Description = "Old school", Price = new decimal(200), DeliveryPrice = new decimal(0.00)};

            _productLibrary.Create(MapProductToProductView(product));
        }

        public void Update()
        {
            var productExisting = _products.First();
            productExisting.Description = "this is updated";
            _productLibrary.Update(MapProductToProductView(productExisting));
        }

        public void Delete()
        {
            var productExisting = _products.First();
            _productLibrary.Delete(productExisting.Id);
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
