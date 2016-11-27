using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Pipeline
{
    public interface IPostRequestHandler<in TRequest, in TResponse>
    {
        void Handle(TRequest request, TResponse response);
    }
}