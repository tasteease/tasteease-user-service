using Fiap.TasteEase.Domain.Aggregates.ClientAggregate.ValueObjects;
using Fiap.TasteEase.Domain.Aggregates.Common;
using Fiap.TasteEase.Domain.Models;
using FluentResults;

namespace Fiap.TasteEase.Domain.Aggregates.ClientAggregate
{
    public class Client : Entity<ClientId, ClientProps>, IClientAggregate
    {
        public Client(ClientProps props, ClientId? id = default) : base(props, id)
        {
        }

        public string Name => Props.Name;
        public string TaxpayerNumber => Props.TaxpayerNumber;
        public string FullAddress => Props.FullAddress;
        public long? CellPhoneNumber => Props.CellPhoneNumber;
        public DateTime CreatedAt => Props.CreatedAt;
        public DateTime UpdatedAt => Props.UpdatedAt;

        public static Result<Client> Create(CreateClientProps props)
        {
            var date = DateTime.UtcNow;
            var clientProps = new ClientProps(
                props.Name,
                props.TaxpayerNumber,
                props.FullAddress,
                props.CellPhoneNumber,
                date,
                date
            );

            var order = new Client(clientProps, new ClientId(Guid.NewGuid()));
            return Result.Ok(order);
        }

        public static Result<Client> Rehydrate(ClientProps props, ClientId id)
        {
            return Result.Ok(new Client(props, id));
        }

        public static Result<Client> Rehydrate(ClientModel model)
        {
            var order = new Client(
                new ClientProps(
                    model.Name,
                    model.TaxpayerNumber,
                    model.FullAddress,
                    model.CellPhoneNumber,
                    model.CreatedAt,
                    model.UpdatedAt
                ),
                new ClientId(model.Id)
            );

            return Result.Ok(order);
        }
    }
}

public record ClientProps(
    string Name,
    string TaxpayerNumber,
    string FullAddress,
    long? CellPhoneNumber,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateClientProps(
    string Name,
    string TaxpayerNumber,
    string FullAddress,
    long? CellPhoneNumber
);