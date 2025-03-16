using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Services.Auth.ResetPassword
{
    public class CacheResetToken
    {
        private static readonly ConcurrentDictionary<string, (string Email, DateTime Expiration)> _resetTokens = new();

        public static void UpdateCache(string token, string email, TimeSpan lifetime)
        {
            var expirationTime = DateTime.UtcNow.Add(lifetime);
            _resetTokens[token] = (email, expirationTime);
        }

        public static bool TryGetEmail(string token, out string email)
        {
            if (_resetTokens.TryGetValue(token, out var data))
            {
                if (DateTime.UtcNow <= data.Expiration)
                {
                    email = data.Email;
                    return true;
                }
                else
                {
                    _resetTokens.TryRemove(token, out _); // Удаляем протухший токен
                }
            }

            email = null;
            return false;
        }

        public static void RemoveToken(string token)
        {
            _resetTokens.TryRemove(token, out _);
        }

    }
}
