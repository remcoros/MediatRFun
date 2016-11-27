namespace MediatR.DomainEvents.Internal
{
    public static class AggregateAsDynamicExtensions
    {
        public static dynamic AsDynamic(this IAggregate o)
        {
            return PrivateReflectionDynamicObject.WrapObjectIfNeeded(o);
        }
    }
}