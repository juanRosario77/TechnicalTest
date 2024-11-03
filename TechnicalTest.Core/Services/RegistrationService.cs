using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using TechnicalTest.Core.DTOs;
using TechnicalTest.Core.Interfaces;
using TechnicalTest.Data.Interfaces;
using TechnicalTest.Data.Models;

namespace TechnicalTest.Core.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhoneRepository _phoneRepository;
        private readonly IAccountService _accountService;
        private readonly ValidationConfig _validations;

        public RegistrationService(IAccountService accountService,
            IUserRepository userRepository, IPhoneRepository phoneRepository,
               IOptions<ValidationConfig> validations)
        {
            _accountService = accountService;
            _userRepository = userRepository;
            _phoneRepository = phoneRepository;
            _validations = validations.Value;
        }

        public async Task<(bool IsSuccess, BaseResponse<UserResponseBasic> Response)> LoginUserAsync(UserRequestLogin userRequest)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByEmailAsync(userRequest.Email);

                if (existingUser == null)
                {
                    return (false, new BaseResponse<UserResponseBasic>()
                    {
                        Message = "Account does not exist"
                    });
                }

                existingUser = null;
                existingUser = await _userRepository.GetUserByEmailandPasswordAsync(userRequest.Email,
                     _accountService.EncryptPassword(userRequest.Password));

                if (existingUser == null)
                {
                    return (false, new BaseResponse<UserResponseBasic>()
                    {
                        Message = "Email and Password do not match"
                    });
                }
                else if (!existingUser.IsActive)
                {
                    return (false, new BaseResponse<UserResponseBasic>()
                    {
                        Message = "Account is not active"
                    });
                }
                else
                {
                    existingUser.LastLogin = DateTime.Now;
                    existingUser.Token = _accountService.GenerateJwtToken(existingUser);

                    var userResponse = new UserResponseBasic
                    {
                        Name = existingUser.Name,
                        Email = existingUser.Email,
                        Token = existingUser.Token
                    };

                    await _userRepository.UpdateUserAsync(existingUser);

                    return (true, new BaseResponse<UserResponseBasic>()
                    {
                        Content = userResponse
                    });
                }
            }
            catch (Exception ex)
            {
                return (false, new BaseResponse<UserResponseBasic>()
                {
                    Message = ex.ToString()
                });
            }
        }

        public async Task<(bool IsSuccess, BaseResponse<UserResponse> Response)> RegisterUserAsync(UserRequest userRequest)
        {
            try
            {
                // Validations
                if (!IsValidEmail(userRequest.Email ?? string.Empty))
                {
                    return (false, new BaseResponse<UserResponse>()
                    {
                        Message = "Invalid email format"
                    });
                }

                if (!IsValidPassword(userRequest.Password ?? string.Empty))
                {
                    return (false, new BaseResponse<UserResponse>()
                    {
                        Message = "Invalid password format"
                    });
                }

                var existingUser = await _userRepository.GetUserByEmailAsync(userRequest.Email ?? string.Empty);

                if (existingUser != null)
                {
                    return (false, new BaseResponse<UserResponse>()
                    {
                        Message = "The email is already registered"
                    });
                }
                // Validations

                var newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Name = userRequest.Name,
                    Email = userRequest.Email,
                    Password = _accountService.EncryptPassword(userRequest.Password ?? string.Empty),
                    Created = DateTime.UtcNow,
                    Modified = DateTime.UtcNow,
                    LastLogin = DateTime.UtcNow,
                    IsActive = true
                };

                newUser.Token = _accountService.GenerateJwtToken(newUser);

                await _userRepository.AddUserAsync(newUser);

                if (userRequest.Phones != null && userRequest.Phones.Any())
                {
                    foreach (var phoneRequest in userRequest.Phones)
                    {
                        var phone = new Phone
                        {
                            UserId = newUser.Id,
                            Number = phoneRequest.Number,
                            CityCode = phoneRequest.CityCode,
                            CountryCode = phoneRequest.CountryCode
                        };

                        await _phoneRepository.AddPhoneAsync(phone);
                    }
                }

                var userResponse = new UserResponse
                {
                    Id = newUser.Id,
                    Name = newUser.Name,
                    Email = newUser.Email,
                    Created = newUser.Created,
                    Modified = newUser.Modified,
                    LastLogin = newUser.LastLogin,
                    Token = newUser.Token,
                    IsActive = newUser.IsActive
                };

                return (true, new BaseResponse<UserResponse>()
                {
                    Content = userResponse
                });
            }
            catch (Exception ex)
            {
                return (false, new BaseResponse<UserResponse>()
                {
                    Message = ex.ToString()
                });
            }
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, _validations.EmailRegex ?? string.Empty);
        }

        private bool IsValidPassword(string password)
        {
            if (password.Length < _validations.PasswordMinLengh
                || password.Length > _validations.PasswordMaxLengh)
            {
                return false;
            }

            return Regex.IsMatch(password, _validations.PasswordRegex ?? string.Empty);
        }
    }

}
