using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using refactor_me.ViewModels;

namespace refactor_me.Logic.Interface
{
    public interface IProductOptionLibrary
    {
        List<ProductOptionView> GetOptions(Guid productId, Guid id);
        void Create(ProductOptionView product);
        void Update(ProductOptionView product);
        void Delete(Guid id);
    }
}
