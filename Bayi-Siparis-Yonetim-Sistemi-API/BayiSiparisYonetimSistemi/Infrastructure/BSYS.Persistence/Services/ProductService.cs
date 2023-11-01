using BSYS.Application.Abstractions.Services;
using BSYS.Application.Abstractions.UoW;
using BSYS.Application.Repositories.ProductRepository;
using BSYS.Domain.Entities;
using System.Text.Json;

namespace BSYS.Persistence.Services;

public class ProductService : IProductService
{
    readonly IUnitofWork _uow;
    readonly IQRCodeService _qrCodeService;

    public ProductService(IUnitofWork uow, IQRCodeService qrCodeService)
    {
        _uow = uow;
        _qrCodeService = qrCodeService;
    }

    public async Task<byte[]> QrCodeToProductAsync(string productId)
    {
        Product product = await _uow.ProductReadRepository.GetByIdAsync(productId);
        if (product == null)
            throw new Exception("Product not found");

        var plainObject = new
        {
            product.Id,
            product.Name,
            product.Price,
            product.Stock,
            product.CreatedDate
        };
        string plainText = JsonSerializer.Serialize(plainObject);

        return _qrCodeService.GenerateQRCode(plainText);
    }

    public async Task StockUpdateToProductAsync(string productId, int stock)
    {
        Product product = await _uow.ProductReadRepository.GetByIdAsync(productId);
        if (product == null)
            throw new Exception("Product not found");

        product.Stock = stock;
        await _uow.ProductWriteRepository.SaveAsync();
    }
}
