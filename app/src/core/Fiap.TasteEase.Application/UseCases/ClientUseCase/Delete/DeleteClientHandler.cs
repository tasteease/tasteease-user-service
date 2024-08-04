using Fiap.TasteEase.Application.Ports;
using FluentResults;
using MediatR;

namespace Fiap.TasteEase.Application.UseCases.ClientUseCase.Delete
{
    public class DeleteClientHandler : IRequestHandler<Delete, Result<bool>>
    {
        private readonly IClientRepository _clientRepository;

        public DeleteClientHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Result<bool>> Handle(Delete request, CancellationToken cancellationToken)
        {
            var clientResult = await _clientRepository.Get(w => w.TaxpayerNumber == request.TaxpayerNumber);
            if (clientResult.IsFailed || !clientResult.ValueOrDefault.Any()) return Result.Fail<bool>("Cliente não encontrado");

            var client = clientResult.Value.FirstOrDefault();
            if (client is null)
                return Result.Fail("Cliente não encontrado");

            client.Delete();

            _clientRepository.Update(client);

            await _clientRepository.SaveChanges();

            return Result.Ok(true);
        }
    }
}