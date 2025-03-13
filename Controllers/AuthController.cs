using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NS.ApiCore.Controllers;
using NS.Core.Messages.Integration;
using NS.Identidade.API.Models;
using NS.Identidade.API.Services;
using NS.MessageBus;

namespace NS.Identidade.API.Controllers
{
    [Route("api/auth")]
    public class AuthController : MainController
    {
        private readonly AuthenticationService _authenticationService;
        private readonly IMessageBus _bus;

        public AuthController(AuthenticationService authenticationService, IMessageBus bus)
        {
            _authenticationService = authenticationService;
            _bus = bus;
        }

        [HttpPost("new-account")]
        public async Task<IActionResult> Register(UserRegister userRegister)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = userRegister.Email,
                Email = userRegister.Email,
                EmailConfirmed = true
            };

            var result = await _authenticationService.UserManager.CreateAsync(user, userRegister.Password);

            if (result.Succeeded)
            {
                var clienteResult = await RegisterClient(userRegister);

                if (!clienteResult.ValidationResult.IsValid)
                {
                    await _authenticationService.UserManager.DeleteAsync(user);
                    return CustomResponse(clienteResult.ValidationResult);
                }

                return CustomResponse(await _authenticationService.GenerateJwt(userRegister.Email));
            }

            foreach (var error in result.Errors)
            {
                AddErrorProcessing(error.Description);
            }

            return CustomResponse();
        }

        [HttpPost("authentication")]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _authenticationService.SignInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password,
                false, true);

            if (result.Succeeded)
            {
                return CustomResponse(await _authenticationService.GenerateJwt(userLogin.Email));
            }

            if (result.IsLockedOut)
            {
                AddErrorProcessing("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }

            AddErrorProcessing("Usuário ou Senha incorretos");
            return CustomResponse();
        }

        private async Task<ResponseMessage> RegisterClient(UserRegister useRegister)
        {
            var user = await _authenticationService.UserManager.FindByEmailAsync(useRegister.Email);

            var usuarioRegistrado = new UserRegisterIntegrationEvent(
                Guid.Parse(user.Id), useRegister.Name, useRegister.Email, useRegister.Cpf);

            try
            {
                return await _bus.RequestAsync<UserRegisterIntegrationEvent, ResponseMessage>(usuarioRegistrado);
            }
            catch
            {
                await _authenticationService.UserManager.DeleteAsync(user);
                throw;
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                AddErrorProcessing("Refresh Token inválido");
                return CustomResponse();
            }

            var token = await _authenticationService.GiveRefreshToken(Guid.Parse(refreshToken));

            if (token is null)
            {
                AddErrorProcessing("Refresh Token expirado");
                return CustomResponse();
            }

            return CustomResponse(await _authenticationService.GenerateJwt(token.Username));
        }
    }
}
