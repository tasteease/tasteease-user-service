using FluentResults;
using MediatR;

namespace Fiap.TasteEase.Application.UseCases.ClientUseCase.Login
{
    public class LoginCommand : IRequest<Result<LoginResponse>>
    {
        public string TaxpayerNumber { get; set; }
    }

    public record LoginResponse(string RefreshToken, string AccessToken, int Expiration);
}