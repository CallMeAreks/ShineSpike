using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using ShineSpike.Utils;
using ShineSpike.ViewModels;
using System;
using System.Text;

namespace ShineSpike.Services
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration Config;

        public LoginService(IConfiguration config) => Config = config;

        public bool ValidateLogin(LoginViewModel model)
        {
            return model.UserName == Config[SettingKeys.User.UserName] && VerifyHashedPassword(model.Password);
        }

        private bool VerifyHashedPassword(string password)
        {
            var saltBytes = Encoding.UTF8.GetBytes(Config[SettingKeys.User.Salt]);

            var hashBytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8
            );

            var hashText = BitConverter.ToString(hashBytes).Replace(Constants.Dash, string.Empty, StringComparison.OrdinalIgnoreCase);
            return hashText == Config[SettingKeys.User.Password];
        }
    }
}
