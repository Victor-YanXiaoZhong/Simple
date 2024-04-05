using Microsoft.AspNetCore.Http;
using Simple.Contracts.Model;
using Simple.Utils.Models;

namespace Simple.Contracts
{
    public interface IUploadService
    {
        IUploadService SetConfig(UploadConfig config);
        Task<UploadSuccessResponse> FileUploadAsync(IFormFile iFormFile);
        Task<List<UploadSuccessResponse>> FileUploadAsync(IFormFileCollection iFormFiles);
    }
}