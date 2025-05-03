using DoubleV.DTOs;
using DoubleV.Interfaces;
using DoubleV.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoubleV.Servicios
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }   
                
        public async Task<string?> GetRoleByEmailAsync(string email)
        {
            try
            {                
                var user = await _context.Users
                                            .Include(u => u.Rol) 
                                            .FirstOrDefaultAsync(u => u.Email == email);
               
                if (user == null || user.Rol == null)
                    return null;
                               
                return user.Rol.Name;
            }
            catch (Exception ex) 
            {                
                Console.WriteLine($"Error getting the role: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Detail: {ex.InnerException.Message}");
                }
               
                return null;
            }
        }

        public async Task<string> CreateUserAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user.UserId;
            }
            catch (DbUpdateException dbEx)
            {   
                return "Error";
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.UserId == id)
                    .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting user by id: " + ex.Message);
                return null;
            }
        }        

        public async Task<User> GetUserWithRoleAndTodosByIdAsync(string id)
        {
            return await _context.Users.Include(u => u.Rol).Include(u => u.Todos)
                .FirstOrDefaultAsync(u => u.UserId == id);
        }        

        public async Task<(string Message, bool IsValid)> ValidateUserCredentialsAsync(User user)
        {
            try
            {
                var userFound = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);

                if (userFound == null)
                {
                    return ("Invalid credentials", false);
                }

                return ("Valid credencials", true);
            }
            catch (Exception ex)
            {
                return ("Error on User validation", false);
            }
        }

        //public async Task<List<UserWithRoleDTO>> GetAllUsersAsync()
        //{
        //    try
        //    {
        //        // Get users with roles and mapp 
        //        var usuarios = await _context.Users
        //            .Include(u => u.Rol)
        //            .Select(u => new UserWithRoleDTO
        //            {
        //                UsuarioId = u.UsuarioId,
        //                Nombre = u.Nombre,
        //                Email = u.Email,
        //                Password = u.Password,
        //                RolId = u.RolId,
        //                RolNombre = u.Rol != null ? u.Rol.Nombre : string.Empty
        //            })
        //            .ToListAsync();

        //        return usuarios;
        //    }
        //    catch (DbUpdateException dbEx)
        //    {
        //        return new List<UserWithRoleDTO>();
        //    }
        //    catch (Exception ex)
        //    {
        //        return new List<UserWithRoleDTO>();
        //    }
        //}

        //public async Task<bool> ActualizarUsuarioAsync(User usuario)
        //{
        //    try
        //    {
        //        _context.Usuarios.Update(usuario);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (DbUpdateException dbEx)
        //    {
        //        Console.WriteLine("Error de base de datos: " + dbEx.Message);
        //        if (dbEx.InnerException != null)
        //        {
        //            Console.WriteLine("Detalle: " + dbEx.InnerException.Message);
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error general: " + ex.Message);
        //        return false;
        //    }
        //}

        //public async Task<bool> BorrarUsuarioAsync(int usuarioId)
        //{
        //    try
        //    {
        //        var usuario = await _context.Usuarios.Include(u => u.Tareas)
        //                                             .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

        //        if (usuario == null)
        //        {
        //            // Usuario no encontrado
        //            return false;
        //        }

        //        // Al eliminar el usuario, también se eliminan sus tareas debido a la cascada configurada en OnModelCreating
        //        _context.Usuarios.Remove(usuario);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (DbUpdateException dbEx)
        //    {
        //        Console.WriteLine("Error de base de datos: " + dbEx.Message);
        //        if (dbEx.InnerException != null)
        //        {
        //            Console.WriteLine("Detalle: " + dbEx.InnerException.Message);
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error general: " + ex.Message);
        //        return false;
        //    }
        //}
    }
}