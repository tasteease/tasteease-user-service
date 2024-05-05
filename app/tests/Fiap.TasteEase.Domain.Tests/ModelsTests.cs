using Fiap.TasteEase.Domain.Models;
using Fiap.TasteEase.Domain.Ports;
using FluentAssertions;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fiap.TasteEase.Domain.Tests
{
    public class ModelsTests
    {
        [Fact]
        [Trait("Models", "EntityModel")]
        public void EntityModel_Id_Should_Be_Guid()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var entity = new EntityModel()
            {
                Id = id
            };

            // Act
            // Assert
            entity.Id.Should().NotBeEmpty();
            entity.Id.Should().Be(id);
        }

        [Fact]
        [Trait("Models", "Client")]
        public void ClientModel_Properties_Should_Have_Correct_Attributes()
        {
            // Arrange
            var id = Guid.NewGuid();
            var name = "John Doe";
            var taxpayerNumber = "123456789";
            var createdAt = DateTime.UtcNow;
            var updatedAt = DateTime.UtcNow;

            var client = new ClientModel
            {
                Id = id,
                Name = name,
                TaxpayerNumber = taxpayerNumber,
                CreatedAt = createdAt,
                UpdatedAt = updatedAt
            };

            // Act

            // Assert
            client.Id.Should().Be(id);
            client.Name.Should().Be(name);
            client.TaxpayerNumber.Should().Be(taxpayerNumber);
            client.CreatedAt.Should().Be(createdAt);
            client.UpdatedAt.Should().Be(updatedAt);

            client.GetType().GetProperty(nameof(ClientModel.Id))
                .Should().BeDecoratedWith<KeyAttribute>()
                .And.BeDecoratedWith<DatabaseGeneratedAttribute>(attr => attr.DatabaseGeneratedOption == DatabaseGeneratedOption.Identity)
                .And.BeDecoratedWith<ColumnAttribute>(attr => attr.Name == "id");

            client.GetType().GetProperty(nameof(ClientModel.Name))
                .Should().BeDecoratedWith<ColumnAttribute>(attr => attr.Name == "name")
                .And.BeDecoratedWith<MaxLengthAttribute>(attr => attr.Length == 512);

            client.GetType().GetProperty(nameof(ClientModel.TaxpayerNumber))
                .Should().BeDecoratedWith<ColumnAttribute>(attr => attr.Name == "taxpayer_number")
                .And.BeDecoratedWith<MaxLengthAttribute>(attr => attr.Length == 256);

            client.GetType().GetProperty(nameof(ClientModel.CreatedAt))
                .Should().BeDecoratedWith<ColumnAttribute>(attr => attr.Name == "created_at" && attr.TypeName == "timestamp without time zone");

            client.GetType().GetProperty(nameof(ClientModel.UpdatedAt))
                .Should().BeDecoratedWith<ColumnAttribute>(attr => attr.Name == "updated_at" && attr.TypeName == "timestamp without time zone");
        }
    }
}