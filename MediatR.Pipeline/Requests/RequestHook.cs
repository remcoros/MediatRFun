using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Pipeline
{
    public abstract class RequestHook<TRequest, TResponse> :
        IPreRequestHandler<TRequest, TResponse>,
        IPostRequestHandler<TRequest, TResponse>
    {
        void IPreRequestHandler<TRequest, TResponse>.Handle(TRequest request) => OnPreRequest(request);

        protected virtual void OnPreRequest(TRequest request)
        {
        }

        void IPostRequestHandler<TRequest, TResponse>.Handle(TRequest request, TResponse response)
        {
            OnPostRequest(request, response);
            ProcessResponse(response);
        }

        protected virtual void OnPostRequest(TRequest request, TResponse response)
        {
        }

        protected virtual void ProcessResponse(TResponse response)
        {
        }
    }
}