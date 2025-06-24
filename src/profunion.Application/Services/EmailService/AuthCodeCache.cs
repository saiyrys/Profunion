using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Services.EmailService
{
    public class AuthCodeCache
    {
        private static readonly ConcurrentDictionary<string, string> _dictionary = new();

        public static void UpdateCache(string email, string code)
        {
            _dictionary[email] = code;
        }

        public static async Task<string> GetCode(string email)
        {
            return _dictionary.TryGetValue(email, out var code) ? code : null;
        }
    }
}
