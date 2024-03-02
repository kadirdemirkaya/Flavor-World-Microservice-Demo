using AuthenticationService.Application.Dtos;
using AuthenticationService.Application.Features.Command.Authentication.CreateUser;
using AuthenticationService.Application.Features.Command.Authentication.LoginUser;
using AuthenticationService.Application.Features.Queries.Authentication.RefreshToken;
using AuthenticationService.Infrastructure.Attributes;
using BuildingBlock.Base.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UserRequestAttributeActionFilter]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediatr;
        private readonly IEmailService _emailService;
        public AuthenticationController(IMediator mediatr, IEmailService emailService)
        {
            _mediatr = mediatr;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("User/Register/Create-User")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            CreateUserCommandRequest createUserCommand = new(createUserDto);
            CreateUserCommandResponse? response = await _mediatr.Send(createUserCommand);
            return response.IsSuccess is true ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        [Route("User/Login/Login-User")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
        {
            LoginUserCommandRequest loginUserCommand = new(loginUserDto);
            LoginUserCommandResponse? response = await _mediatr.Send(loginUserCommand);
            return response.IsSuccess is true ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        [Route("User/Token/Refresh-Token")]
        public async Task<IActionResult> RefreshToken([FromQuery] string token)
        {
            RefreshTokenQueryRequest refreshTokenQuery = new(token);
            RefreshTokenQueryResponse response = await _mediatr.Send(refreshTokenQuery);
            return response.newToken is not null ? Ok(response) : BadRequest(response);
        }
    }
}
