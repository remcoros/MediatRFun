namespace MediatR.Pipeline
{
    public interface IPreNotificationHandler<in TNotification>
    {
        void Handle(TNotification notification);
    }
}