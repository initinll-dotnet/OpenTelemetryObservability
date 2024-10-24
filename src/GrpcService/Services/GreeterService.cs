using Grpc.Core;

using GrpcService;

using OpenTelemetry;
using OpenTelemetry.Trace;

using System.Diagnostics;

namespace GrpcService.Services;
public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        var requestId = Baggage.Current.GetBaggage("request.id");

        Activity.Current?.SetTag("request.id", requestId);

        try
        {
            var selectedValue = Random.Shared.GetItems([0 ,1, 2, 3], 1).First();
            var res = 1 / selectedValue;

            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }
        catch (Exception ex)
        {
            Activity.Current?.SetStatus(ActivityStatusCode.Error, ex.Message);

            Activity.Current?.RecordException(ex);

            throw;
        }       
    }
}
