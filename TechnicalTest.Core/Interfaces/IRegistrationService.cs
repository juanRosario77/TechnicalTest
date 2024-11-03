using TechnicalTest.Core.DTOs;

namespace TechnicalTest.Core.Interfaces
{
    public interface IRegistrationService
    {
        Task<(bool IsSuccess, BaseResponse<UserResponseBasic> Response)> LoginUserAsync(UserRequestLogin userRequest);
        Task<(bool IsSuccess, BaseResponse<UserResponse> Response)> RegisterUserAsync(UserRequest userRequest);
    }

}
