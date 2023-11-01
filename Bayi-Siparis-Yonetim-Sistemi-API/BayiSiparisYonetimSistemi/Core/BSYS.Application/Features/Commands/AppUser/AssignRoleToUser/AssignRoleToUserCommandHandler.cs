using BSYS.Application.Abstractions.Services;
using MediatR;

namespace BSYS.Application.Features.Commands.AppUser.AssignRoleToUser;

public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommandRequest, AssignRoleToUserCommandResponse>
{
    readonly IUserService _userService;
    public AssignRoleToUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<AssignRoleToUserCommandResponse> Handle(AssignRoleToUserCommandRequest request, CancellationToken cancellationToken)
    {
        await _userService.AssignRoleToUserAsnyc(request.UserId, request.Roles);
        return new();
    }
}
