using DoubleV.DTOs;
using DoubleV.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoubleV.Interfaces
{
    public interface IUserService
    {        
        Task<string?> GetRoleByEmailAsync(string email);
        Task<(string Message, bool IsValid)> ValidateUserCredentialsAsync(User user);
        Task<User?> GetUserByIdAsync(string id);        
        Task<User> GetUserWithRoleAndTodosByIdAsync(string id);        
        Task<string> CreateUserAsync(User user);
        //Task<List<UserWithRoleDTO>> GetAllUsersAsync();
        //Task<bool> DeleteUserAsync(int userId);
        //Task<bool> UpdateUserAsync(User user);
    }
}
