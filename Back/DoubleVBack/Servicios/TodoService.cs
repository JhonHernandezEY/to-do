using DoubleV.DTOs;
using DoubleV.Interfaces;
using DoubleV.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoubleV.Servicios
{
    public class TodoService : ITodoService
    {
        private readonly ApplicationDbContext _context;

        public TodoService(ApplicationDbContext context)
        {
            _context = context;
        }        

        public async Task<Todo?> GetTodoByIdAsync(string todoId)
        {
            try
            {
                return await _context.Todos.FirstOrDefaultAsync(t => t.TodoId == todoId);
            }
            catch (Exception ex)
            {                
                Console.WriteLine($"Error getting to-do with Id: {todoId}: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Detail: {ex.InnerException.Message}");
                }

                // Return new in case of error
                return null;
            }
        }

        public async Task<bool> DeleteTodoAsync(string todoId)
        {
            try
            {
                var todo = await _context.Todos.FirstOrDefaultAsync(t => t.TodoId == todoId);

                if (todo == null)
                {                    
                    return false;
                }

                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine("Database error: " + dbEx.Message);
                if (dbEx.InnerException != null)
                {
                    Console.WriteLine("Detail: " + dbEx.InnerException.Message);
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("General error: " + ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<TodoWithUserDTO>> GetTodosWithUserAsync()
        {
            try
            {                
                var todos = await _context.Todos
                    .Include(t => t.User)
                    .ToListAsync();
                                
                var todosWithUserDTO = todos.Select(t => new TodoWithUserDTO
                {
                    TodoId = t.TodoId,
                    Deadline = t.Deadline,
                    Name = t.Name,
                    // Handling possible null
                    UserId = t.User?.UserId ?? "0", 
                    UserName = t.User?.Name ?? "User not available" 
                }).ToList();

                return todosWithUserDTO;                
            }
            catch (Exception ex)
            {   
                return new List<TodoWithUserDTO>();
            }
        }

        public async Task<string> CreateTodoAsync(Todo todo)
        {
            try
            {
                _context.Todos.Add(todo);
                await _context.SaveChangesAsync();                
                return todo.TodoId;
            }
            catch (DbUpdateException dbEx)
            {   
                return "-1"; 
            }
            catch (Exception ex)
            {                
                return "-2"; 
            }
        }

        public async Task<List<Todo>> GetAllTodosAsync()
        {
            return await _context.Todos.Include(t => t.User).ToListAsync();
        }       

        //public async Task<Todo> UpdateTareaAsync(Todo todo)
        //{
        //    _context.Entry(todo).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //    return todo;
        //}

        //public async Task<bool> UpdateTodoAsync(Todo todo)
        //{
        //    try
        //    {
        //        _context.Todos.Update(todo);
        //        await _context.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (DbUpdateException dbEx)
        //    {
        //        Console.WriteLine("Database Errors: " + dbEx.Message);
        //        if (dbEx.InnerException != null)
        //        {
        //            Console.WriteLine("Detail: " + dbEx.InnerException.Message);
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("General Error: " + ex.Message);
        //        return false;
        //    }
        //}
    }
}
