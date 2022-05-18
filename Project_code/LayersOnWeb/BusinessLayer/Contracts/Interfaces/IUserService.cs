using DataAccess.Contracts.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts
{
    public interface IUserService
    {
        Task<UserModel> Register(RegisterUserModel user);
        Task<UserModel> Login(LoginUserModel user);
        Task<UserModel> Update(Guid id, String email, String password);
        Task<ApplicationUser> Delete(Guid id);
        Task<List<UserModel>> GetUsers();
        bool ExistsUser(Guid id);
        Task<UserModel> GetUserAsync(Guid id);
    }
}
