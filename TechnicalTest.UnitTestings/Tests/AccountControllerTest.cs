using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TechnicalTest.API.Controllers;
using TechnicalTest.Core.DTOs;
using TechnicalTest.Core.Services;
using TechnicalTest.Data.Repositories;
using TechnicalTest.UnitTestings.Data;

namespace TechnicalTest.UnitTestings.Tests
{
    public class AccountControllerTest : BaseTest
    {
        private readonly AccountController _accountController;

        public AccountControllerTest()
        {
            var memoryContext = new TechnicalTestContextUnit().GetMemoryContext();

            // Repositories
            var userRepository = new UserRepository(memoryContext);
            var phoneRepository = new PhoneRepository(memoryContext);

            // IOptions
            var validationConfig = new OptionsWrapper<ValidationConfig>(
                GetConfiguration().GetSection("Validations").Get<ValidationConfig>() ??
                new ValidationConfig()
            );

            var authenticationConfig = new OptionsWrapper<AuthenticationConfig>(
                GetConfiguration().GetSection("Authentication").Get<AuthenticationConfig>() ??
                new AuthenticationConfig()
            );

            // Services
            var accountService = new AccountService(authenticationConfig);
            var registrationService = new RegistrationService(accountService, userRepository,
                phoneRepository, validationConfig);

            // Controller
            _accountController = new AccountController(registrationService);
        }

        private UserRequest DefaultUserRequest { get; set; }
            = new UserRequest
            {
                Name = "Juan Rosario",
                Email = "JuanRosario@gmail.com",
                Password = "123456789123456789AB",
                Phones = new List<PhoneRequest>
                {
                    new PhoneRequest()
                    {
                        Number = 8275782,
                        CountryCode = 1,
                        CityCode = 809
                    }
                }
            };

        [Fact]
        // Verificar que se pueda agregar un usuario correctamente.
        public async Task RegisterUserCorrect()
        {
            DefaultUserRequest.Email = "JuanRosario1@gmail.com";
            var response = await _accountController.Register(DefaultUserRequest);

            var okResult = Assert.IsType<OkObjectResult>(response);
            var resultResponse = Assert.IsType<BaseResponse<UserResponse>>(okResult.Value);
            var resultUserResponse = Assert.IsType<UserResponse>(resultResponse.Content);

            Assert.Equal(DefaultUserRequest.Name, resultUserResponse.Name);
            Assert.Equal(DefaultUserRequest.Email, resultUserResponse.Email);
            Assert.True(resultUserResponse.IsActive);
        }

        [Fact]
        // Verificar que las validaciones del correo, se apliquen en el registro.
        public async Task RegisterUserEmailIncorrect()
        {
            DefaultUserRequest.Email = "JuanRosario@@@@.gmail,.com";
            var response = await _accountController.Register(DefaultUserRequest);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);
            var resultResponse = Assert.IsType<BaseResponse<UserResponse>>(badRequestResult.Value);

            Assert.Equal("Invalid email format", resultResponse.Message);
        }

        [Fact]
        // Verificar que las validaciones no permitan un correo duplicado.
        public async Task RegisterUserEmailDuplicatedIncorrect()
        {
            DefaultUserRequest.Email = "JuanRosario2@gmail.com";
            await _accountController.Register(DefaultUserRequest);

            DefaultUserRequest.Email = "JuanRosario2@gmail.com";
            var response = await _accountController.Register(DefaultUserRequest);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);
            var resultResponse = Assert.IsType<BaseResponse<UserResponse>>(badRequestResult.Value);

            Assert.Equal("The email is already registered", resultResponse.Message);
        }

        [Fact]
        // Verificar que las validaciones del password funcionen.
        // Password
        // Debe tener al menos 8 caracteres y maximo 20,
        // Debe tener al menos una letra, y al menos un dígito.
        // Password
        public async Task RegisterUserPasswordIncorrect()
        {
            DefaultUserRequest.Email = "JuanRosario1@gmail.com";
            DefaultUserRequest.Password = "1234";
            var response = await _accountController.Register(DefaultUserRequest);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(response);
            var resultResponse = Assert.IsType<BaseResponse<UserResponse>>(badRequestResult.Value);

            Assert.Equal("Invalid password format", resultResponse.Message);
        }
    }
}