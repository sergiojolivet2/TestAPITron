using WebApi.Models;

namespace WebApi.Services
{
    public class ProductService : IProductService
    {

        private readonly List<Product> _products;
        private int _nextId = 1;

        public ProductService()
        {
            _products = new List<Product>();
        }
        public Product CreateProduct(Product product)
        {
            product.Id = ++_nextId;
            _products.Add(product);
            return product;
        }

        public bool DeleteProduct(int id)
        {
           var product = _products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return false;
            }
            _products.Remove(product);
            return true;
        }

        public List<Product> GetAllProducts()
        {
            return _products;
        }

        public Product GetProductById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public Product UpdateProduct(Product product)
        {
            var exisitingProduct = _products.FirstOrDefault(p => p.Id == product.Id);
            if (exisitingProduct == null) { return null; }

            exisitingProduct.Name = product.Name;
            exisitingProduct.Priced = product.Priced;

            return exisitingProduct;
        }
    }
}
