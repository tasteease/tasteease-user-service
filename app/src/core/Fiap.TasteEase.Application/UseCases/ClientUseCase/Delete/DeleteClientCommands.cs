using FluentResults;
using MediatR;

namespace Fiap.TasteEase.Application.UseCases.ClientUseCase.Delete
{
    public class Delete : IRequest<Result<bool>>
    {
        public string TaxpayerNumber { get; set; }
    }
}