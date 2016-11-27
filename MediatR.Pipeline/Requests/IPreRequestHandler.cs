using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Pipeline
{
    public interface IPreRequestHandler<in TRequest> : IPreRequestHandler<TRequest, object>
    {
    }

    // ReSharper disable once UnusedTypeParameter -- Used for service lookup
    public interface IPreRequestHandler<in TRequest, in TResponse>
    {
        void Handle(TRequest request);
    }
}