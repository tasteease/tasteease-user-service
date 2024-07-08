using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Fiap.TasteEase.Application.Helpers;
using Fiap.TasteEase.Application.Ports;
using Fiap.TasteEase.Domain.DTOs;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fiap.TasteEase.Application.UseCases.ClientUseCase.Login
{
    public class LoginClientHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly ILogger<LoginClientHandler> _logger;
        private readonly IAmazonCognitoIdentityProvider _identityProvider;
        private readonly AwsSettings _awsSettings;
        private readonly CognitoUserPool _userPool;
        private readonly IClientRepository _clientRepository;

        public LoginClientHandler(
            ILogger<LoginClientHandler> logger,
            IAmazonCognitoIdentityProvider identityProvider,
            CognitoUserPool userPool,
            IClientRepository clientRepository,
            IOptions<AwsSettings> awsSettings)
        {
            _logger = logger;
            _identityProvider = identityProvider;
            _userPool = userPool;
            _clientRepository = clientRepository;
            _awsSettings = awsSettings.Value;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand loginCommand, CancellationToken cancellationToken)
        {
            try
            {
                var userDbResult = await _clientRepository.Get(x => x.TaxpayerNumber == loginCommand.TaxpayerNumber);
                if (userDbResult.IsFailed)
                    return userDbResult.ToResult();

                var userDb = userDbResult.Value.FirstOrDefault();
                if (userDb is null)
                    return Result.Fail("Usuário inválido");

                if (userDb.IsDeleted)
                    return Result.Fail("Usuário deletado");

                var user = new CognitoUser(
                    loginCommand.TaxpayerNumber,
                    _awsSettings.UserPoolClientId,
                    _userPool,
                    _identityProvider);

                var authRequest = new InitiateCustomAuthRequest
                {
                    AuthParameters = new Dictionary<string, string>
                    {
                        { "CHALLENGE_NAME", "CUSTOM_CHALLENGE" },
                        { "USERNAME", loginCommand.TaxpayerNumber },
                        { "SECRET_HASH", CognitoHash.GetSecretHash(loginCommand.TaxpayerNumber, _awsSettings.UserPoolClientId,_awsSettings.UserPoolClientSecret) }
                },
                    ClientMetadata = new Dictionary<string, string>()
                };

                var authResponse = await user.StartWithCustomAuthAsync(authRequest);

                var loginResponse = new LoginResponse(
                    authResponse.AuthenticationResult.RefreshToken, 
                    authResponse.AuthenticationResult.AccessToken, 
                    authResponse.AuthenticationResult.ExpiresIn);

                return Result.Ok(loginResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante login");
                return Result.Fail("Erro! Não foi possivel realizar o login");
            }
        }
    }
}