
using Microsoft.EntityFrameworkCore;
using WebApplication_IN.Controllers;
using WebApplication_IN.Models;
using WebApplication_IN.Service;

namespace WebApplication_IN.ImportXmlFeed
{
    public class ProductImport : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        //private readonly ProductContext _context;
        //private readonly IProductService _productService;

        public ProductImport(IServiceProvider serviceProvider)//, ProductContext context, ProductService productService)
        {
            _serviceProvider = serviceProvider;
            //_context = context;
            //_productService = productService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        // All you code goes here
                        List<Product> listXml =  DownloadAndParseXml.Parse_XML();                        
                        
                        // getting DbContext instance
                        var _context = scope.ServiceProvider.GetRequiredService<ProductContext>();
                        var _productService = scope.ServiceProvider.GetRequiredService<ProductService>();
                        // IProductService vytahovat ZDE (bez konstruktoru )


                        foreach (var productXml in listXml)
                        {
                            List<Product> listDb = new(_context.Product.AsNoTracking().ToList());

                            foreach (var productDb in listDb)
                            {
                                if (productDb.Ean == productXml.Ean)
                                {
                                    productXml.ProductId = productDb.ProductId;
                                    await _productService.UpdateProductAsync(productXml);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString() + "Nepodarilo se nacist XML feed.");
                    //throw new Exception("Nepodarilo se nacist XML feed.");
                }
                finally
                {
                    // this sets the timer
                    await Task.Delay(new TimeSpan(0, 0, 30));
                }
            }
        }
    }
}
