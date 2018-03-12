using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using refactor_me.Data.Interface;
using refactor_me.Models;
using refactor_me.ViewModels;

namespace refactor_me.Logic
{
    public class ProductLibrary : AbstractLogic, IProductLibrary
    {
        private readonly IGenericRepository<Product> _productRepository;

        public ProductLibrary(IPersistanceFactory persistanceFactory, IMapper mapper) : base(mapper)
        {
            Mapper = mapper;
            _productRepository = persistanceFactory.BuildRefactorMeRepository<Product>();
        }

        public List<ProductView> GetAll()
        {
            var products = _productRepository.GetAll().ToList();
            return Mapper.Map<List<ProductView>>(products);
        }

        public List<ProductView> Get(Guid id, string name)
        {
            if(!string.IsNullOrEmpty(name))
                return Mapper.Map<List<ProductView>>(_productRepository.Find(x => x.Name.Contains(name)).ToList());

            return Mapper.Map<List<ProductView>>(_productRepository.Find(x => x.Id == id).ToList());
        }

        public void Create(ProductView product)
        {
            var productToAdd = product;
            productToAdd.Id = Guid.NewGuid();
            _productRepository.Add(Mapper.Map<Product>(productToAdd));
            _productRepository.SaveChanges();
        }

        public void Update(ProductView product)
        {
            _productRepository.Update(Mapper.Map<Product>(product));
            _productRepository.SaveChanges();
        }

        public void Delete(Guid id)
        {
            _productRepository.Delete(_productRepository.Find(x => x.Id == id).First());
            _productRepository.SaveChanges();
        }
    }
}
