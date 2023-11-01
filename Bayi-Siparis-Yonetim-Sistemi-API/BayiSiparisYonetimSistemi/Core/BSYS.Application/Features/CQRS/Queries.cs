
using BSYS.Application.Features.Queries.Order.GetOrdersByUserId;
using MediatR;

namespace BSYS.Application.Features.CQRS;

public record GetOrdersByUserIdQuery(GetOrdersByUserIdQueryRequest model, string UserId) : IRequest<GetOrdersByUserIdQueryResponse>;