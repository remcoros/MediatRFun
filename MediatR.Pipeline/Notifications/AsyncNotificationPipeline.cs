using System.Threading.Tasks;

namespace MediatR.Pipeline
{
    public class AsyncNotificationPipeline<TNotification> :
        IAsyncNotificationHandler<TNotification>
        where TNotification : IAsyncNotification
    {
        private readonly IAsyncNotificationHandler<TNotification> _inner;
        private readonly IPreNotificationHandler<TNotification>[] _preNotificationHandlers;
        private readonly IPostNotificationHandler<TNotification>[] _postNotificationHandlers;

        public AsyncNotificationPipeline(
            IAsyncNotificationHandler<TNotification> inner,
            IPreNotificationHandler<TNotification>[] preNotificationHandlers,
            IPostNotificationHandler<TNotification>[] postNotificationHandlers)
        {
            _inner = inner;
            _preNotificationHandlers = preNotificationHandlers;
            _postNotificationHandlers = postNotificationHandlers;
        }

        public async Task Handle(TNotification notification)
        {
            foreach (var preNotificationHandler in _preNotificationHandlers)
            {
                preNotificationHandler.Handle(notification);
            }

            await _inner.Handle(notification).ConfigureAwait(false);

            foreach (var postNotificationHandler in _postNotificationHandlers)
            {
                postNotificationHandler.Handle(notification);
            }
        }
    }
}