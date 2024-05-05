using Fiap.TasteEase.Domain.Aggregates.Common;

namespace Fiap.TasteEase.Domain.Tests
{
    public partial class CommonTests
    {
        public record TestId(Guid Value);

        public class TestProps
        {
            public string Name { get; set; }
        }

        public class TestEntity : Entity<TestId, TestProps>
        {
            public TestEntity(TestProps props, TestId? id = default) : base(props, id)
            {
            }

            public new bool IsTransient_Public() => base.IsTransient();

            public new void AddDomainEvent_Public(IEntityEvent eventItem) => base.AddDomainEvent(eventItem);

            public new void RemoveDomainEvent_Public(IEntityEvent eventItem) => base.RemoveDomainEvent(eventItem);

            public new void ClearDomainEvents_Public() => base.ClearDomainEvents();

            public IReadOnlyCollection<IEntityEvent> DomainEvents_Public => base.DomainEvents;

        }

        public class TestDomainEvent : IEntityEvent
        {
            // Implement test event properties/methods if needed
            public string AggregateType { get; init; }
            public string AggregateEventType { get; init; }
            public string Payload { get; init; }
            public DateTime CreatedAt { get; init; }
        }
    }
}