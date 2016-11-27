using System.Threading;
using System.Threading.Tasks;

namespace MediatR.Pipeline
{
    public class CancellableAsyncNotificationPipeline<TNotification> :
        ICancellableAsyncNotificationHandler<TNotification>
        where TNotification : ICancellableAsyncNotification
    {
        private readonly ICancellableAsyncNotificationHandler<TNotification> _inner;
        private readonly IPreNotificationHandler<TNotification>[] _preNotificationHandlers;
        private readonly IPostNotificationHandler<TNotification>[] _postNotificationHandlers;

        public CancellableAsyncNotificationPipeline(
            ICancellableAsyncNotificationHandler<TNotification> inner,
            IPreNotificationHandler<TNotification>[] preNotificationHandlers,
            IPostNotificationHandler<TNotification>[] postNotificationHandlers)
        {
            _inner = inner;
            _preNotificationHandlers = preNotificationHandlers;
            _postNotificationHandlers = postNotificationHandlers;
        }

        public async Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            foreach (var preNotificationHandler in _preNotificationHandlers)
            {
                preNotificationHandler.Handle(notification);
            }

            await _inner.Handle(notification, cancellationToken).ConfigureAwait(false);

            foreach (var postNotificationHandler in _postNotificationHandlers)
            {
                postNotificationHandler.Handle(notification);
            }
        }
    }
}