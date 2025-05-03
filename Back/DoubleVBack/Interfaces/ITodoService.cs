using DoubleV.DTOs;
using DoubleV.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoubleV.Interfaces
{
    public interface ITodoService
    {        
        Task<Todo?> GetTodoByIdAsync(string todoId);
        Task<IEnumerable<TodoWithUserDTO>> GetTodosWithUserAsync();
        Task<List<Todo>> GetAllTodosAsync();        
        Task<string> CreateTodoAsync(Models.Todo todo);        
        Task<bool> DeleteTodoAsync(string todoId);
        //Task<bool> UpdateTodoAsync(Todo todo);
    }
}
