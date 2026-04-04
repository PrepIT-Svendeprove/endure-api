using Microsoft.Extensions.Primitives;

namespace Endure.Middleware
{
    public class TraceMiddleware(IRequestContext requestContext) : IMiddleware
    {
        private readonly IRequestContext _requestContext = requestContext;
        private const string TRACE_HEADER_NAME = "TraceId";

        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string traceId = context.TraceIdentifier;

            if (context.Request.Headers.TryGetValue(TRACE_HEADER_NAME, out StringValues value) && !string.IsNullOrEmpty(value))
                traceId = value.ToString();

            context.TraceIdentifier = traceId;
            context.Response.Headers[TRACE_HEADER_NAME] = traceId;

            _requestContext.TraceId = traceId;

            return next(context);
        }
    }
}
