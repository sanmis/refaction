using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using refactor_me.Models;
using refactor_me.ViewModels;

namespace refactor_me.Logic
{
    public interface IProductLibrary
    {
        List<ProductView> GetAll();
        List<ProductView> Get(Guid id, string name);
        void Create(ProductView product);
        void Update(ProductView product);
        void Delete(Guid id);
    }
}
