using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTODO.Models;

namespace WebTODO.ILoginRegisterServices
{
    public interface ILoginRegister
    {
        Task<ValidationMessage> RegisterAsync(Register register);

        Task<ValidationMessage> LoginAsync(Login login);
    }
}
