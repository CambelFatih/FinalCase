using BSYS.Application.Repositories.CompletedOrderRepository;
using BSYS.Application.Repositories.ProductRepository;
using BSYS.Application.Repositories.CustomerRepository;
using BSYS.Application.Repositories.MenuRepository;
using BSYS.Application.Repositories.EndpointRepository;
using BSYS.Application.Repositories.BasketRepository;
using BSYS.Application.Repositories.BasketItemRepository;
using BSYS.Application.Repositories.OrderRepository;
using BSYS.Application.Repositories.FileRepository;
using BSYS.Application.Repositories.ProductImageFileRepository;
using BSYS.Application.Repositories.InvoiceFileRepository;

namespace BSYS.Application.Abstractions.UoW;

public interface IUnitofWork 
{
    ICustomerReadRepository CustomerReadRepository { get; }
    ICustomerWriteRepository CustomerWriteRepository { get; }
    IOrderReadRepository OrderReadRepository { get; }
    IOrderWriteRepository OrderWriteRepository { get; }
    IProductReadRepository ProductReadRepository { get; }
    IProductWriteRepository ProductWriteRepository { get;}
    IFileReadRepository FileReadRepository { get; }
    IFileWriteRepository FileWriteRepository { get;}
    IProductImageFileReadRepository ProductImageFileReadRepository { get; }
    IProductImageFileWriteRepository ProductImageFileWriteRepository { get; }
    IInvoiceFileReadRepository InvoiceFileReadRepository { get; }
    IInvoiceFileWriteRepository InvoiceFileWriteRepository { get; }
    IBasketItemReadRepository BasketItemReadRepository { get; }
    IBasketItemWriteRepository BasketItemWriteRepository { get; }
    IBasketReadRepository BasketReadRepository { get; }
    IBasketWriteRepository BasketWriteRepository { get; }
    ICompletedOrderReadRepository CompletedOrderReadRepository { get; }
    ICompletedOrderWriteRepository CompletedOrderWriteRepository { get; }
    IEndpointReadRepository EndpointReadRepository { get; }
    IEndpointWriteRepository EndpointWriteRepository { get; }
    IMenuReadRepository MenuReadRepository { get; }
    IMenuWriteRepository MenuWriteRepository { get; }
}
