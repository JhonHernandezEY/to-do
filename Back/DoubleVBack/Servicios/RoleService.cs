using DoubleV.DTOs;
using DoubleV.Interfaces;
using DoubleV.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoubleV.Servicios 
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;

        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetAllTheRolesAsync()
        {
            try
            {                
                var roles = await _context.Roles.ToListAsync();
                return roles;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine("Database error " + dbEx.Message);
                if (dbEx.InnerException != null)
                {
                    Console.WriteLine("Detail: " + dbEx.InnerException.Message);
                }
                return new List<Role>(); // Return an empty list on error case
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error: " + ex.Message);
                return new List<Role>(); 
            }
        }

    }
}
