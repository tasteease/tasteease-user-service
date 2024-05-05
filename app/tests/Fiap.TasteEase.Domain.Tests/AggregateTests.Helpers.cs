using Fiap.TasteEase.Domain.Aggregates.ClientAggregate;

namespace Fiap.TasteEase.Domain.Tests
{
    public partial class AggregateTests
    {
        public class TestClientId
        {
            public Guid Value { get; }

            public TestClientId(Guid value)
            {
                Value = value;
            }
        }

        public class TestClientProps
        {
            public string Name { get; set; }
            public string TaxpayerNumber { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
        }
    }
}