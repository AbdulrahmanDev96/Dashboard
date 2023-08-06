using API.Entities;

namespace API.Services
{
    public interface IAuthServices 
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
    }
}