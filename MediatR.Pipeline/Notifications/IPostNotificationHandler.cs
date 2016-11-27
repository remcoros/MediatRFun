namespace MediatR.Pipeline
{
    public interface IPostNotificationHandler<in TNotification>
    {
        void Handle(TNotification notification);
    }
}