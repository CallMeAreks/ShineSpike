using ShineSpike.ViewModels;

namespace ShineSpike.Services
{
    public interface ILoginService
    {
        bool ValidateLogin(LoginViewModel loginViewModel);
    }
}
