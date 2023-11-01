using BSYS.Application.Repositories.BasketItemRepository;
using BSYS.Application.Repositories.BasketRepository;
using BSYS.Application.Repositories.CompletedOrderRepository;
using BSYS.Application.Repositories.CustomerRepository;
using BSYS.Application.Repositories.EndpointRepository;
using BSYS.Application.Repositories.FileRepository;
using BSYS.Application.Repositories.InvoiceFileRepository;
using BSYS.Application.Repositories.MenuRepository;
using BSYS.Application.Repositories.OrderRepository;
using BSYS.Application.Repositories.ProductImageFileRepository;
using BSYS.Application.Repositories.ProductRepository;
using BSYS.Persistence.Contexts;
using BSYS.Application.Abstractions.UoW;
using BSYS.Persistence.Repositories.CustomerRepository;
using BSYS.Persistence.Repositories.OrderRepository;
using BSYS.Persistence.Repositories.ProductRepository;
using BSYS.Persistence.Repositories.FileRepository;
using BSYS.Persistence.Repositories.ProductImageFileRepository;
using BSYS.Persistence.Repositories.InvoiceFileRepository;
using BSYS.Persistence.Repositories.BasketItemRepository;
using BSYS.Persistence.Repositories.BasketRepository;
using BSYS.Persistence.Repositories.CompletedOrderRepository;
using BSYS.Persistence.Repositories.EndpointRepository;
using BSYS.Persistence.Repositories.MenuRepository;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace BSYS.Persistence.UoW;

public class UnitofWork : IUnitofWork
{
    private IDbContextTransaction _transaction;
    private readonly BSYSDbContext _dbContext;
    public UnitofWork(BSYSDbContext dbContext)
    {
        _dbContext = dbContext;
        CustomerReadRepository = new CustomerReadRepository(dbContext);
        CustomerWriteRepository = new CustomerWriteRepository(dbContext);
        OrderReadRepository = new OrderReadRepository(dbContext);
        OrderWriteRepository = new OrderWriteRepository(dbContext);
        ProductReadRepository = new ProductReadRepository(dbContext);
        ProductWriteRepository = new ProductWriteRepository(dbContext);
        FileReadRepository = new FileReadRepository(dbContext);
        FileWriteRepository = new FileWriteRepository(dbContext);
        ProductImageFileReadRepository = new ProductImageFileReadRepository(dbContext);
        ProductImageFileWriteRepository = new ProductImageFileWriteRepository(dbContext);
        InvoiceFileReadRepository = new InvoiceFileReadRepository(dbContext);
        InvoiceFileWriteRepository = new InvoiceFileWriteRepository(dbContext);
        BasketItemReadRepository = new BasketItemReadRepository(dbContext);
        BasketItemWriteRepository = new BasketItemWriteRepository(dbContext);
        BasketReadRepository = new BasketReadRepository(dbContext);
        BasketWriteRepository = new BasketWriteRepository(dbContext);
        CompletedOrderReadRepository = new CompletedOrderReadRepository(dbContext);
        CompletedOrderWriteRepository = new CompletedOrderWriteRepository(dbContext);
        EndpointReadRepository = new EndpointReadRepository(dbContext);
        EndpointWriteRepository = new EndpointWriteRepository(dbContext);
        MenuReadRepository = new MenuReadRepository(dbContext);
        MenuWriteRepository = new MenuWriteRepository(dbContext);
    }
    public void BeginTransaction()
    {
        _transaction = _dbContext.Database.BeginTransaction();
    }

    public void Commit()
    {
        if (_transaction != null)
        {
            _transaction.Commit();
        }
    }

    public void Rollback()
    {
        if (_transaction != null)
        {
            _transaction.Rollback();
        }
    }
    public ICustomerReadRepository CustomerReadRepository { get; private set; }
    public ICustomerWriteRepository CustomerWriteRepository { get; private set; }
    public IOrderReadRepository OrderReadRepository { get; private set; }
    public IOrderWriteRepository OrderWriteRepository { get; private set; }
    public IProductReadRepository ProductReadRepository { get; private set; }
    public IProductWriteRepository ProductWriteRepository { get; private set; }
    public IFileReadRepository FileReadRepository { get; private set; }
    public IFileWriteRepository FileWriteRepository { get; private set; }
    public IProductImageFileReadRepository ProductImageFileReadRepository { get; private set; }
    public IProductImageFileWriteRepository ProductImageFileWriteRepository { get; private set; }
    public IInvoiceFileReadRepository InvoiceFileReadRepository { get; private set; }
    public IInvoiceFileWriteRepository InvoiceFileWriteRepository { get; private set; }
    public IBasketItemReadRepository BasketItemReadRepository { get; private set; }
    public IBasketItemWriteRepository BasketItemWriteRepository { get; private set; }
    public IBasketReadRepository BasketReadRepository { get; private set; }
    public IBasketWriteRepository BasketWriteRepository { get; private set; }
    public ICompletedOrderReadRepository CompletedOrderReadRepository { get; private set; }
    public ICompletedOrderWriteRepository CompletedOrderWriteRepository { get; private set; }
    public IEndpointReadRepository EndpointReadRepository { get; private set; }
    public IEndpointWriteRepository EndpointWriteRepository { get; private set; }
    public IMenuReadRepository MenuReadRepository { get; private set; }
    public IMenuWriteRepository MenuWriteRepository { get; private set; }
}
