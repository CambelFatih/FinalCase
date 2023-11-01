using MediatR;

namespace BSYS.Application.Features.Queries.Basket.GetBasketItems;

public class GetBasketItemsQueryRequest : IRequest<List<GetBasketItemsQueryResponse>>
{
}