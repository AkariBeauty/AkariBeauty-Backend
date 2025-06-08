using AkariBeauty.Controllers.Dtos;

namespace AkariBeauty.Services.Interfaces
{
    public interface IGenericLogin
    {
        Task<string> Login(RequestLoginDTO request);
    }
}