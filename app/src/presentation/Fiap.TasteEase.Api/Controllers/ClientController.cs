using Fiap.TasteEase.Api.ViewModels;
using Fiap.TasteEase.Api.ViewModels.Client;
using Fiap.TasteEase.Application.UseCases.ClientUseCase.Create;
using Fiap.TasteEase.Application.UseCases.ClientUseCase.Delete;
using Fiap.TasteEase.Application.UseCases.ClientUseCase.Login;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fiap.TasteEase.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientController : ControllerBase
{
    private readonly ILogger<ClientController> _logger;
    private readonly IMediator _mediator;

    public ClientController(
        ILogger<ClientController> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<ResponseViewModel<Guid?>>> Post(CreateClientRequest request)
    {
        try
        {
            var command = request.Adapt<Create>();

            var mediatorResponse = await _mediator.Send(command);

            if (mediatorResponse.IsFailed)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new ResponseViewModel<Guid?>
                    {
                        Error = true,
                        ErrorMessages = mediatorResponse.Errors.Select(x => x.Message)
                    }
                );

            return StatusCode(StatusCodes.Status201Created,
                new ResponseViewModel<Guid?>
                {
                    Data = mediatorResponse.ValueOrDefault
                }
            );
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ResponseViewModel<Guid?>
                {
                    Error = true,
                    ErrorMessages = new List<string> { ex.Message },
                }
            );
        }
    }

    [HttpDelete]
    public async Task<ActionResult<ResponseViewModel<bool>>> Delete(DeleteRequest request)
    {
        try
        {
            var command = request.Adapt<Delete>();

            var mediatorResponse = await _mediator.Send(command);

            if (mediatorResponse.IsFailed)
                return StatusCode(StatusCodes.Status400BadRequest,
                    new ResponseViewModel<bool>
                    {
                        Error = true,
                        ErrorMessages = mediatorResponse.Errors.Select(x => x.Message)
                    }
                );

            return StatusCode(StatusCodes.Status201Created,
                new ResponseViewModel<bool>
                {
                    Data = mediatorResponse.ValueOrDefault
                }
            );
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ResponseViewModel<bool?>
                {
                    Error = true,
                    ErrorMessages = new List<string> { ex.Message },
                }
            );
        }
    }

    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<ResponseViewModel<LoginResponse>>> Login(LoginRequest request)
    {
        try
        {
            var command = request.Adapt<LoginCommand>();

            var mediatorResponse = await _mediator.Send(command);
            if (mediatorResponse.IsFailed)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                new ResponseViewModel<LoginResponse>
                {
                    Error = true,
                    ErrorMessages = mediatorResponse.Errors.Select(x => x.Message)  
                });
            }

            var authResponde = mediatorResponse.ValueOrDefault;

            return StatusCode(StatusCodes.Status201Created,
                new ResponseViewModel<LoginResponse>
                {
                    Data = authResponde
                }
            );
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status400BadRequest,
                new ResponseViewModel<LoginResponse>
                {
                    Error = true,
                    ErrorMessages = new List<string> { ex.Message }
                }
            );
        }
    }
}