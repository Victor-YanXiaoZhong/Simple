using Microsoft.AspNetCore.Mvc;

namespace Simple.WebHost.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublicController : ControllerBase
    {
        private readonly ILogger<PublicController> _logger;
        private readonly IUploadService uploadService;

        public PublicController(ILogger<PublicController> logger, IUploadService uploadService)
        {
            _logger = logger;
            this.uploadService = uploadService;
        }

        [HttpGet("verifycodeimg")]
        public IActionResult VerifyCodeImage()
        {
            var codeItem = VerifyCodeHelper.CreateVerifyCode();
            HttpContext.Session.Set("VerifyCode", codeItem.code);

            var ms = new MemoryStream();//生成内存流对象
            codeItem.img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            ms.Seek(0, SeekOrigin.Begin);//指针回归

            return File(ms, "image/gif");
        }

        [HttpGet("verifycode")]
        public ApiResult VerifyCode(string code)
        {
            if (code.IsNullOrEmpty())
                return ApiResult.Fail("请输入验证码");
            var serverCode = HttpContext.Session.Get<string>("VerifyCode");

            if (serverCode.IsNotEmpty())
                return ApiResult.Fail("验证码不存在，请重新获取");

            if (serverCode.ToLower() == code.ToLower())
                return ApiResult.Fail("验证码错误");

            return ApiResult.Success("验证码正确");
        }

        [HttpPost("upload")]
        public async Task<ApiResult> Upload(IFormFile file)
        {
            var result = await uploadService.FileUploadAsync(file);
            return ApiResult.Success(result);
        }
    }
}