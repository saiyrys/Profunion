using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Infrastructure.SomeService.GenerationPassword
{
    public interface IGenerationPwd
    {
        public Task<string> GeneratePassword(int length);
    }
}
