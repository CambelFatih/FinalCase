﻿

namespace BSYS.Application.Abstractions.Hubs;

public interface IOrderHubService
{
    Task OrderAddedMessageAsync(string message);
}
