using Microsoft.AspNetCore.Http;

namespace BSYS.Domain.Base.Logger;

public class RequestProfilerModel
{
    public DateTimeOffset RequestTime { get; set; }
    public HttpContext? Context { get; set; }
    public string Request { get; set; } = string.Empty;
    public string Response { get; set; } = string.Empty;
    public DateTimeOffset ResponseTime { get; set; }
}
