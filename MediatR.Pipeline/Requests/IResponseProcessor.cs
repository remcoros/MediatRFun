namespace MediatR.Pipeline
{
    public interface IResponseProcessor<in TResponse>
    {
        void Handle(TResponse response);
    }
}