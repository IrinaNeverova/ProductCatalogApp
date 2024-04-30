using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using WebApplication_IN.Enums;
using WebApplication_IN.Models;

namespace WebApplication_IN.Service
{
    
    public class ProductService : IProductService
    {
        private readonly ProductContext _context;

        public ProductService(ProductContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetCollectionV1Async(ProductCollectionModel filter)
        {
            var listOfProducts = await _context.Product.ToListAsync();

            var filteredProducts = listOfProducts
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();

            return filteredProducts.Count() > 0 ? filteredProducts : Enumerable.Empty<Product>();
        }

        public async Task<IEnumerable<Product>> GetCollectionV2Asyncstring (string categoryName, int page = 1, int pageSize = 10)
        {
            var filteredProducts = new List<Product>();

            var listOfProducts = await _context.Product.ToListAsync();

            if (!string.IsNullOrEmpty(categoryName))
            {
                filteredProducts = listOfProducts.Where(x => x.Category == categoryName).ToList();
                filteredProducts = filteredProducts.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            }
            else
            {
                filteredProducts = listOfProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            }

            return filteredProducts.Count() > 0 ? filteredProducts : Enumerable.Empty<Product>();
        }

        public async Task<ActionResult<Product>> GetProductByIdAsync(int id)
        {
            var product = await _context.Product.FindAsync(id);
            return product;
        }

        public async Task UpdateProductAsync(Product product)
        {
            var productDB = GetProductByEan(product.Ean);
            if (productDB != null)
            {
                _context.Entry(product).State = EntityState.Modified;

                try
                {
                    if (productDB.ProductId != product.ProductId && product.ProductId != 0)
                    {
                        throw new HttpRequestException("Kombinace Id produktu a EANu je nevalidni.", null, System.Net.HttpStatusCode.BadRequest);
                    }

                    if (product.UpdatedPrice != null && product.Price != productDB?.Price)
                    {
                        //Pouzivam Parse, protoze podminkou if jiz vynechavam pripad kdy UpdatedPrice je null
                        DateTime lastPriceChangeTime = DateTime.Parse(productDB.UpdatedPrice.ToString());
                        DateTime currentTime = DateTime.Now;

                        TimeSpan timeSinceLastChange = currentTime - lastPriceChangeTime;

                        if (timeSinceLastChange.TotalHours < 12)
                        {
                            throw new HttpRequestException("Cena může být změněna pouze jednou za 12 hodin.", null, System.Net.HttpStatusCode.BadRequest);
                        }
                    }

                    var hasTransaction = _context.Database.CurrentTransaction != null;
                    //_context.Entry(product).State = EntityState.Modified;
                    bool hasChanges = _context.ChangeTracker.HasChanges();

                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException) 
                {
                    throw;
                }                
            }
            if (productDB == null) //else?
            {
                if (!ProductExists(product.Ean))
                {
                    throw new HttpRequestException("Produkt s eanem " + product.Ean + " neexistuje v DB.", null, System.Net.HttpStatusCode.BadRequest);
                }
            }
        }

        public async Task<ActionResult<Product>> CreateProductAsync(Product product)
        {
            if (ProductExists(product.Ean))
            {
                throw new HttpRequestException("Nelze zalozit produkt s duplicitnim EANem.", null, System.Net.HttpStatusCode.BadRequest);
            }

            try
            {
                _context.Product.Add(product);

                await _context.SaveChangesAsync();

                return GetProductByEan(product.Ean);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }                        
        }

        public async Task DeleteProductAsync(int id)
        {
            var productDB = await _context.Product.FindAsync(id);

            if (productDB == null)
            {
                throw new HttpRequestException("Produkt s Id " + id + " neexistuje v DB. Nelze smazat neexistujici produkt.", null, System.Net.HttpStatusCode.BadRequest);
            }

            try
            {
                _context.Product.Remove(productDB);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        private Product GetProductByEan(int ean)
        {
            var product = _context.Product.AsNoTracking().FirstOrDefault(p => p.Ean == ean);
            
            return product;
        }

        private bool ProductExists(int ean)
        {
            return _context.Product.Any(e => e.Ean == ean);
        }
    }
}
