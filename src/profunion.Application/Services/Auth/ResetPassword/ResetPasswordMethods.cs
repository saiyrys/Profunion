using profunion.Applications.Interface.IAuth;
using profunion.Applications.Interface.IEmailService;
using profunion.Domain.Models.UserModels;
using profunion.Domain.Persistance;
using profunion.Infrastructure.SomeService.HashPassword;
using profunion.Shared.Dto.Auth.PWD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Services.Auth.ResetPassword
{
    public class ResetPasswordMethods : IResetPasswordMethods
    {
        private readonly IControl<User> _control;
        private readonly IEmailResetSender _resetSender;
        private readonly IUserRepository _userRepository;
        private readonly IHashingPassword _hashingPassword;

        public ResetPasswordMethods(IControl<User> control, IEmailResetSender resetSender, IUserRepository userRepository, IHashingPassword hashingPassword)
        {
            _control = control;
            _resetSender = resetSender;
            _userRepository = userRepository;
            _hashingPassword = hashingPassword;
        }

        private async Task<bool> ValidatePassword(string loginPassword, string dbPassword, string salt)
            => await _hashingPassword.VerifyPassword(loginPassword, dbPassword, salt);

        public async Task<bool> ChangePassword(ChangePasswordDto dto)
        {
            string token = await _control.VerifyByTokenAsync();
            var currentUser = await _control.FindByTokenAsync(token);

            var user = await _userRepository.GetByIdAsync(currentUser.userId);

            // Проверяем старый пароль
            if (!await ValidatePassword(dto.currentPassword, user.password, user.salt))
                throw new UnauthorizedAccessException();

            // Хешируем новый пароль
            var (newPass, salt) = await _hashingPassword.HashPassword(dto.newPassword);
            user.password = newPass;
            user.salt = Convert.ToBase64String(salt);

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> ResetPasswordByToken(string token, string newPassword)
        {
            if (!CacheResetToken.TryGetEmail(token, out string email))
            {
                throw new ArgumentException("Недействительный или просроченный токен.");
            }

            var user = await _control.FindByEmailAsync(email);

            if (user == null)
            {
                throw new ArgumentException("Пользователь не найден.");
            }

            var (hashedPassword, salt) = await _hashingPassword.HashPassword(newPassword);

            user.password = hashedPassword;
            user.salt = Convert.ToBase64String(salt);
            user.updatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);

            CacheResetToken.RemoveToken(token); // Удаляем использованный токен

            return true;
        }

        public async Task RequestPasswordReset(string email)
        {
            var user = await _control.FindByEmailAsync(email);

            if (user == null)
                throw new ArgumentException("Пользователь с таким email не найден");

            string token = GenerateSecureToken();

            CacheResetToken.UpdateCache(token, email, TimeSpan.FromMinutes(15)); // Сохраняем токен

            string resetLink = $"https://profunions.ru/reset-password?token={token}";

            string emailBody = LoadHtmlTemplate("ResetPasswordEmail.html", new Dictionary<string, string>
            {
                { "{{resetLink}}", resetLink }
            });

            await _resetSender.SendEmailResetPasswordAsync(email, emailBody);
        }

        private string GenerateSecureToken()
        {
            using var hmac = new HMACSHA256();
            return Convert.ToBase64String(hmac.ComputeHash(Guid.NewGuid().ToByteArray()))
                .TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

        private string LoadHtmlTemplate(string templateName, Dictionary<string, string> replacements)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Templates", templateName);

            if (!File.Exists(path))
                throw new FileNotFoundException($"Шаблон {templateName} не найден.");

            var template = File.ReadAllText(path);

            foreach (var item in replacements)
            {
                template = template.Replace(item.Key, item.Value);
            }

            return template;
        }

    }


}
