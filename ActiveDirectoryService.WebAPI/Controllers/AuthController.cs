using ActiveDirectoryService.Application.Dtos;
using ActiveDirectoryService.Application.Interface;
using CommonDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Threading.Tasks;

namespace ActiveDirectoryService.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IPasswordService _passwordService;

        public AuthController(IPasswordService passwordService)
        {
            _passwordService=passwordService;
        }

        [HttpPost]
        public ApiResponse ChangePassword(ChangePasswordRequestDto changePasswordRequest)
        {
            if (_passwordService.ChangePasswordAsync(changePasswordRequest))
                return ApiResponse.Success();
            return ApiResponse.Fail("请检查账号密码");
        }


        [HttpPost]
        public async Task<ApiResponse> ChangePasswordByCapchaAsync(ValidateRequestDto validateRequestDto)
        {

            if (await _passwordService.ChangePasswordByCapchaAsync(validateRequestDto))
                return ApiResponse.Success("修改密码成功");
            return ApiResponse.Fail("请检查验证码是否正确");
        }



        [HttpPost]
        public ApiResponse SendCapcha(ForgotPasswordRequestDto forgotPasswordRequestDto)
        {

            _passwordService.SendCapcha(forgotPasswordRequestDto);
            return ApiResponse.Success();

        }
    }
}
