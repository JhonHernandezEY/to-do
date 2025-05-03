
using DoubleV.DTOs;
using DoubleV.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DoubleV.Interfaces
{
    public interface IRoleService
    {
        Task<List<Role>> GetAllTheRolesAsync();
    }
}
