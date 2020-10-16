using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Herokufy.Models.Interfaces
{
    public interface IInventoryManager
    {
        Task<List<Product>> GetProducts();
        Task<Product> CreateProduct(Product product);
    }
}
