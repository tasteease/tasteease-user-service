using FluentAssertions;

namespace Fiap.TasteEase.Domain.Tests
{
    public partial class CommonTests
    {

        [Fact]
        [Trait("Common", "Entity Should Have Correct Props")]
        public void Entity_Should_Have_Correct_Properties_And_Methods()
        {
            // Arrange
            var id = new TestId(Guid.NewGuid());
            var props = new TestProps { Name = "Test Name" };
            var entity = new TestEntity(props, id);

            // Act

            // Assert
            entity.Id.Should().Be(id);
            entity.Props.Should().BeSameAs(props);
            entity.DomainEvents.Should().BeEmpty();

            // Test Equals method
            var sameEntity = new TestEntity(props, id);
            var nullEntity = (TestEntity)null;
            entity.Equals(null).Should().BeFalse();
            entity.Equals(entity).Should().BeTrue();
            entity.Equals(sameEntity).Should().BeTrue();
            entity.Equals(nullEntity).Should().BeFalse();

            // Test GetHashCode method
            entity.GetHashCode().Should().Be(entity.Id.GetHashCode() ^ 31);
        }

        [Fact]
        [Trait("Common", "Entity Equality Operators Should Work Correctly")]
        public void Entity_Equality_Operators_Should_Work_Correctly()
        {
            // Arrange
            var props1 = new TestProps() { Name = "Name 1" };
            var props2 = new TestProps() { Name = "Name 1" };
            var id1 = new TestId(Guid.NewGuid());
            var id2 = new TestId(Guid.NewGuid());

            var entity1 = new TestEntity(props1, id1);
            var entity2 = new TestEntity(props1, id1);
            var entity3 = new TestEntity(props2, id2);

            // Act & Assert
            (entity1 == entity2).Should().BeTrue();
            (entity1 != entity3).Should().BeTrue();
        }

        [Fact]
        [Trait("Common", "Entity Equals Should Work Correctly")]
        public void Entity_Equals_Should_Work_Correctly()
        {
            // Arrange
            var props1 = new TestProps() { Name = "Name 1" };
            var props2 = new TestProps() { Name = "Name 1" };
            var id1 = new TestId(Guid.NewGuid());
            var id2 = new TestId(Guid.NewGuid());

            var entity1 = new TestEntity(props1, id1);
            var entity2 = new TestEntity(props1, id1);
            var entity3 = new TestEntity(props2, id2);

            // Act & Assert
            entity1.Equals(entity2).Should().BeTrue();
            entity1.Equals(entity3).Should().BeFalse();
            entity1.Equals(null).Should().BeFalse();
            entity1.Equals(new object()).Should().BeFalse();
        }

        [Fact]
        [Trait("Common", "Entity GetHashCode Should Work Correctly")]
        public void Entity_GetHashCode_Should_Work_Correctly()
        {
            // Arrange
            var props1 = new TestProps() { Name = "Name 1" };
            var props2 = new TestProps() { Name = "Name 1" };
            var id1 = new TestId(Guid.NewGuid());
            var id2 = new TestId(Guid.NewGuid());

            var entity1 = new TestEntity(props1, id1);
            var entity2 = new TestEntity(props1, id1);
            var entity3 = new TestEntity(props2, id2);

            // Act & Assert
            entity1.GetHashCode().Should().Be(entity2.GetHashCode());
            entity1.GetHashCode().Should().NotBe(entity3.GetHashCode());
        }

        [Fact]
        [Trait("Common", "AddDomainEvent Should Add Event")]
        public void AddDomainEvent_Should_Add_Event()
        {
            // Arrange
            var props = new TestProps() { Name = "Name 1" };
            var id = new TestId(Guid.NewGuid());
            var entity = new TestEntity(props, id);
            var domainEvent = new TestDomainEvent();

            // Act
            entity.AddDomainEvent_Public(domainEvent);

            // Assert
            entity.DomainEvents.Should().Contain(domainEvent);
        }

        [Fact]
        [Trait("Common", "RemoveDomainEvent Should Remove Event")]
        public void RemoveDomainEvent_Should_Remove_Event()
        {
            // Arrange
            var props = new TestProps() { Name = "Name 1" };
            var id = new TestId(Guid.NewGuid());
            var entity = new TestEntity(props, id);
            var domainEvent = new TestDomainEvent();
            entity.AddDomainEvent_Public(domainEvent);

            // Act
            entity.RemoveDomainEvent_Public(domainEvent);

            // Assert
            entity.DomainEvents.Should().NotContain(domainEvent);
        }

        [Fact]
        [Trait("Common", "ClearDomainEvents Should Clear Events")]
        public void ClearDomainEvents_Should_Clear_Events()
        {
            // Arrange
            var props = new TestProps() { Name = "Name 1" };
            var id = new TestId(Guid.NewGuid());
            var entity = new TestEntity(props, id);
            var domainEvent1 = new TestDomainEvent();
            var domainEvent2 = new TestDomainEvent();
            entity.AddDomainEvent_Public(domainEvent1);
            entity.AddDomainEvent_Public(domainEvent2);

            // Act
            entity.ClearDomainEvents_Public();

            // Assert
            entity.DomainEvents.Should().BeEmpty();
        }
    }
}