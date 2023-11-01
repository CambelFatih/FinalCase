using BSYS.Application.Consts;
using BSYS.Application.Enums;
using BSYS.Application.Features.Commands.Order.CompleteOrder;
using BSYS.Application.Features.Commands.Order.CreateOrder;
using BSYS.Application.Features.Queries.Order.GetAllOrders;
using BSYS.Application.Features.Queries.Order.GetOrderById;
using BSYS.Application.Features.Queries.Order.GetOrdersByUserId;
using BSYS.Application.CustomAttributes;
using BSYS.Application.Features.CQRS; 
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BSYS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    readonly IMediator _mediator;
    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{Id}")]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Reading, Definition = "Get Order By Id")]
    public async Task<ActionResult> GetOrderById([FromRoute] GetOrderByIdQueryRequest getOrderByIdQueryRequest)
    {
        GetOrderByIdQueryResponse response = await _mediator.Send(getOrderByIdQueryRequest);
        return Ok(response);
    }

    [HttpGet]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Reading, Definition = "Get All Orders")]
    public async Task<ActionResult> GetAllOrders([FromQuery] GetAllOrdersQueryRequest getAllOrdersQueryRequest)
    {
        GetAllOrdersQueryResponse response = await _mediator.Send(getAllOrdersQueryRequest);
        return Ok(response);
    }

    [HttpGet("ByUserId")]
    [Authorize]
    public async Task<ActionResult> GetOrdersByUserId([FromQuery] int page, int size)
    {
        var id = (User.Identity as ClaimsIdentity).FindFirst("Id").Value;
        //var operations = new GetOrdersByUserIdQuery(getOrdersByUserIdQueryRequest, id);
        GetOrdersByUserIdQueryRequest getOrdersByUserIdQueryRequest = new GetOrdersByUserIdQueryRequest
        {
            UserId = id,
            Page = page,
            Size = size
        };
        GetOrdersByUserIdQueryResponse response = await _mediator.Send(getOrdersByUserIdQueryRequest);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Writing, Definition = "Create Order")]
    public async Task<ActionResult> CreateOrder(CreateOrderCommandRequest createOrderCommandRequest)
    {
        CreateOrderCommandResponse response = await _mediator.Send(createOrderCommandRequest);
        return Ok(response);
    }

    [HttpGet("complete-order/{Id}")]
    [Authorize(AuthenticationSchemes = "Admin")]
    [AuthorizeDefinition(Menu = AuthorizeDefinitionConstants.Orders, ActionType = ActionType.Updating, Definition = "Complete Order")]
    public async Task<ActionResult> CompleteOrder([FromRoute] CompleteOrderCommandRequest completeOrderCommandRequest)
    {
        CompleteOrderCommandResponse response = await _mediator.Send(completeOrderCommandRequest);
        return Ok(response);
    }
}
