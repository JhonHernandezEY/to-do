using DoubleV.DTOs;
using DoubleV.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using DoubleV.Helpers;
using DoubleV.Models;

namespace DoubleV.Controllers
{
    [ApiController]
    [EnableCors("AllowOrigins")]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public TodosController(ITodoService todoService, IUserService userService, IMapper mapper)
        {
            _todoService = todoService;
            _userService = userService;
            _mapper = mapper;
        }

        //[HttpPut("UpdateTodo/{id}")]
        //[AuthorizeRoles("Administrator", "Supervisor", "Employee")]
        //public async Task<ActionResult<ApiResponse>> UpdateTodo(int id, [FromBody] TodoWithUserDTO todoWithUserDTO)
        //{
        //    if (id <= 0)
        //    {
        //        return BadRequest(new ApiResponse { Message = "Todo Id is required", Data = null });
        //    }

        //    if (todoWithUserDTO == null)
        //    {
        //        return BadRequest(new ApiResponse { Message = "To-do data is required", Data = null });
        //    }

        //    try
        //    {
        //        var todoFound = await _todoService.GetTodoByIdAsync(id);

        //        if (todoFound == null)
        //        {
        //            return NotFound(new ApiResponse { Message = "Todo not found", Data = null });
        //        }

        //        todoFound.Name = todoWithUserDTO.Name;
        //        todoFound.User = todoWithUserDTO.UserName;
        //        todoFound.Us.UsuarioId = todoWithUserDTO.UsuarioId;
        //        todoFound.TareaId = todoWithUserDTO.TareaId;

        //        var resultado = await _todoService.UpdateTodoAsync(todoFound);

        //        if (resultado)
        //        {
        //            return Ok(new ApiResponse
        //            {
        //                Message = "To-do updated",
        //                Data = null
        //            });
        //        }

        //        return BadRequest(new ApiResponse { Message = "Error al actualizar la tarea.", Data = null });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new ApiResponse { Message = "Error al actualizar la tarea.", Error = ex.Message });
        //    }
        //}

        [HttpDelete("DeleteTodo/{id}")]
        [AuthorizeRoles("Administrator")]
        public async Task<ActionResult<ApiResponse>> DeleteTodo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ApiResponse { Message = "To-do Id is required", Data = null });
            }
            try
            {
                var result = await _todoService.DeleteTodoAsync(id);

                if (result)
                {
                    return Ok(new ApiResponse
                    {
                        Message = "To-do deleted",
                        Data = null
                    });
                }
                return NotFound(new ApiResponse { Message = "To-do not found", Data = null });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse { Message = "Error when deleting the to-do", Error = ex.Message });
            }
        }

        [HttpGet("GetTodosWithUsers")]
        [AuthorizeRoles("Administrator", "Supervisor", "Employee")]
        public async Task<ActionResult<ApiResponse>> GetTodosWithUsers()
        {
            try
            {
                var todosWithUsers = await _todoService.GetTodosWithUserAsync();
                var todosWithUsersDTO = _mapper.Map<IEnumerable<TodoWithUserDTO>>(todosWithUsers);

                return Ok(new ApiResponse
                {
                    Message = "To-do obtained",
                    Data = todosWithUsersDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Message = "Error when getting the to-dos",
                    Error = ex.Message
                });
            }
        }

        [HttpPost("CreateTodo")]
        [AuthorizeRoles("Administrator")]
        public async Task<ActionResult<ApiResponse>> CreateTodo([FromBody] TodoWithoutIdDTO todo)
        {
            if (todo == null)
            {
                return BadRequest(new ApiResponse { Message = "To-do data is required", Data = null });
            }

            try
            {
                var mappedTodo = _mapper.Map<Todo>(todo);

                string todoId = await _todoService.CreateTodoAsync(mappedTodo);
                if (todoId != "-1" || todoId != "-2") // If created correctly, Id will be > 0
                {
                    return CreatedAtAction(
                        nameof(GetTodoById), 
                        new { id = todoId }, 
                        new ApiResponse { Message = "To-do created", Data = todoId }
                    );
                }
                return BadRequest(new ApiResponse { Message = "Error when creating the to-do", Data = null });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse { Message = "Error when creating the to-do", Error = ex.Message });
            }
        }

        [HttpGet("GetTodoById/{id}")]
        [AuthorizeRoles("Administrator")]
        public async Task<ActionResult<Todo>> GetTodoById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(new ApiResponse { Message = "Invalid id for the to-do", Data = null });
            }
            try
            {
                var todoFound = await _todoService.GetTodoByIdAsync(id);

                if (todoFound == null)
                {
                    return NotFound(new ApiResponse { Message = $"To-do with id {id} not found", Data = null });
                }
                return Ok(new ApiResponse { Message = "To-do found", Data = todoFound });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse { Message = "Error when getting to-do by id", Error = ex.Message });
            }
        }                           
        
    }
}
