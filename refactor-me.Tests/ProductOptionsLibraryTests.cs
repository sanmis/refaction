using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NUnit.Framework;
using refactor_me.Data.Interface;
using refactor_me.Logic;
using refactor_me.Logic.Interface;
using refactor_me.Models;
using refactor_me.ViewModels;

namespace refactor_me.Tests
{
    [TestFixture]
    public class ProductOptionsLibraryTests
    {
        IGenericRepository<ProductOption> _productOptionRepository;
        IProductOptionLibrary _productOptionLibrary;
        IMapper _mapper;
        IPersistanceFactory _persistanceFactory;
        List<ProductOption> _productOptions;

        [SetUp]
        public void Setup()
        {
            _productOptions = SetupProductOptions();
            _productOptionRepository = SetupProductOptionsRepository();
            var mapperConfig = new MapperConfiguration(cfg =>
                         cfg.AddProfiles(new[] { "refactor-me.Logic", "refactor-me.Data" }));
            _mapper = new Mapper(mapperConfig);
            _persistanceFactory = new Mock<IPersistanceFactory>().Object;
            _productOptionLibrary = new ProductOptionLibrary(_persistanceFactory, _mapper, _productOptionRepository);
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

        public IGenericRepository<ProductOption> SetupProductOptionsRepository()
        {
            var repo = new Mock<IGenericRepository<ProductOption>>();

            repo.Setup(r => r.GetAll()).Returns(_productOptions.AsQueryable());

            repo.Setup(r => r.Find(It.IsAny<Expression<Func<ProductOption, bool>>>()))
                .Returns<Expression<Func<ProductOption, bool>>>(e => _productOptions.Where(e.Compile()).AsQueryable());

            repo.Setup(r => r.Add(It.IsAny<ProductOption>())).Callback(new Action<ProductOption>(
                newProduct =>
                {
                    newProduct.Id = Guid.NewGuid();
                    newProduct.ProductId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3");
                    newProduct.Name = "Blackish";
                    newProduct.Description = "Very blackish!!";
                    _productOptions.Add(newProduct);
                }
                ));

            repo.Setup(r => r.Update(It.IsAny<ProductOption>())).Callback(new Action<ProductOption>(x =>
            {
                var oldProduct = _productOptions.Find(a => a.Id == x.Id);
                oldProduct.Description = "this is updated";
                oldProduct = x;
            }));

            repo.Setup(r => r.Delete(It.IsAny<ProductOption>())).Callback(new Action<ProductOption>(x =>
            {
                var productToRemove = _productOptions.Find(a => a.Id == x.Id);
                if (productToRemove != null)
                    _productOptions.Remove(productToRemove);
            }));

            return repo.Object;
        }

        [Test]
        public void GetById()
        {
            var products = _productOptionLibrary.GetOptions(Guid.Empty, Guid.Parse("A21D5777-A655-4020-B431-624BB331E9A2"));
            var product = _productOptions.Find(x => x.Id == Guid.Parse("A21D5777-A655-4020-B431-624BB331E9A2"));
            Assert.AreEqual(products.First().Id, product.Id);
            Assert.AreEqual(products.First().Name, product.Name);
            Assert.AreEqual(products.First().Description, product.Description);
            Assert.AreEqual(products.First().ProductId, product.ProductId);
        }

        [Test]
        public void GetByProductId()
        {
            var products = _productOptionLibrary.GetOptions(Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"), Guid.Empty);
            var product = _productOptions.Find(x => x.ProductId == Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3"));
            Assert.AreEqual(products.First().Id, product.Id);
            Assert.AreEqual(products.First().Name, product.Name);
            Assert.AreEqual(products.First().Description, product.Description);
            Assert.AreEqual(products.First().ProductId, product.ProductId);
        }

        [Test]
        public void Create()
        {
            var countShouldBe = _productOptions.Count + 1;

            var product = new ProductOption() { Id = Guid.NewGuid(), Name = "Whitish", Description = "Very whitish!!", ProductId = Guid.Parse("8F2E9176-35EE-4F0A-AE55-83023D2DB1A3") };

            _productOptionLibrary.Create(MapProductOptionToProductOptionView(product));
            var countIs = _productOptions.Count;

            Assert.AreEqual(countIs, countShouldBe);
        }

        [Test]
        public void Update()
        {
            var productExisting = _productOptions.First();
            productExisting.Description = "this is updated";
            _productOptionLibrary.Update(MapProductOptionToProductOptionView(productExisting));

            Assert.That(_productOptions.First().Description, Is.EqualTo(productExisting.Description));
        }

        [Test]
        public void Delete()
        {
            var countShouldBe = _productOptions.Count - 1;
            var productExisting = _productOptions.First();
            _productOptionLibrary.Delete(productExisting.Id);
            var countIs = _productOptions.Count;

            Assert.AreEqual(countIs, countShouldBe);
        }

        #region privateMethods

        private ProductOptionView MapProductOptionToProductOptionView(ProductOption product)
        {
            return new ProductOptionView()
            {
                Id = product.Id,
                Description = product.Description,
                ProductId = product.ProductId,
                Name = product.Name
            };
        }

        #endregion
    }
}
