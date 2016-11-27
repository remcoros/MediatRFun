namespace MediatR.Pipeline
{
    public class NotificationPipeline<TNotification> : INotificationHandler<TNotification>
        where TNotification : INotification
    {
        private readonly INotificationHandler<TNotification> _inner;
        private readonly IPreNotificationHandler<TNotification>[] _preNotificationHandlers;
        private readonly IPostNotificationHandler<TNotification>[] _postNotificationHandlers;

        public NotificationPipeline(
            INotificationHandler<TNotification> inner,
            IPreNotificationHandler<TNotification>[] preNotificationHandlers,
            IPostNotificationHandler<TNotification>[] postNotificationHandlers)
        {
            _inner = inner;
            _preNotificationHandlers = preNotificationHandlers;
            _postNotificationHandlers = postNotificationHandlers;
        }

        public void Handle(TNotification notification)
        {
            foreach (var preNotificationHandler in _preNotificationHandlers)
            {
                preNotificationHandler.Handle(notification);
            }

            _inner.Handle(notification);

            foreach (var postNotificationHandler in _postNotificationHandlers)
            {
                postNotificationHandler.Handle(notification);
            }
        }
    }
}