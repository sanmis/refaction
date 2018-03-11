using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using refactor_me.Data.Interface;
using refactor_me.Logic.Interface;
using refactor_me.Models;
using refactor_me.ViewModels;

namespace refactor_me.Logic
{
    public class ProductOptionLibrary : AbstractLogic, IProductOptionLibrary
    {
        private readonly IGenericRepository<ProductOption> _productOptionRepository;

        public ProductOptionLibrary(IPersistanceFactory persistanceFactory, IMapper mapper) : base(mapper)
        {
            Mapper = mapper;
            _productOptionRepository = persistanceFactory.BuildRefactorMeRepository<ProductOption>();
        }

        public List<ProductOptionView> GetOptions(Guid productId, Guid id)
        {
            if (id != Guid.Empty)
                return Mapper.Map<List<ProductOptionView>>(_productOptionRepository.Find(x => x.Id == id && x.ProductId == productId)).ToList();

            return
                Mapper.Map<List<ProductOptionView>>(_productOptionRepository.Find(x => x.ProductId == productId))
                    .ToList();
        }

        public void Create(ProductOptionView product)
        {
            var productOptionToAdd = product;
            productOptionToAdd.Id = Guid.NewGuid();
            _productOptionRepository.Add(Mapper.Map<ProductOption>(productOptionToAdd));
            _productOptionRepository.SaveChanges();
        }

        public void Update(ProductOptionView product)
        {
            _productOptionRepository.Update(Mapper.Map<ProductOption>(product));
            _productOptionRepository.SaveChanges();
        }

        public void Delete(Guid id)
        {
            _productOptionRepository.Delete(_productOptionRepository.Find(x => x.Id == id).First());
            _productOptionRepository.SaveChanges();
        }
    }
}
