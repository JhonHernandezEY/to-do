using DoubleV.DTOs;
using DoubleV.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cors;
using DoubleV.Helpers;
using DoubleV.Models;

namespace DoubleV.Controllers
{
    [ApiController]
    [EnableCors("AllowOrigins")]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public IConfiguration _configuration;        

        public UsersController(IUserService userService, IMapper mapper, IConfiguration configuration)
        {
            _userService = userService;
            _mapper = mapper;
            _configuration = configuration;
        }
                
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            try
            {
                if (login == null)
                {
                    return BadRequest(new { message = "The user doesn't exist" });
                }

                if (string.IsNullOrWhiteSpace(login.Email) ||
                    string.IsNullOrWhiteSpace(login.Password))
                {
                    return BadRequest(new { message = "E-mail or password can't be empty" });
                }

                var user = _mapper.Map<User>(login);
                var (message, isValid) = await _userService.ValidEmployeeCredentialsAsync(user);

                if (!isValid)
                {
                    return Unauthorized(new { Message = message, IsValid = isValid });
                }

                var roleName = await _userService.GetRoleByEmailAsync(login.Correo);

                if (roleName == null)
                {
                    return NotFound(new { message = "Role not found for this user" });
                }

                var token = GenerateJwtToken(login, roleName);

                return Ok(new
                {
                    token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error on login: {ex.Message}" });
            }
        }                

        [HttpPost("CreateUser")]
        [AuthorizeRoles("Administrator", "Supervisor")]
        public async Task<ActionResult<ApiResponse>> CreateUser([FromBody] UserWithoutIdDTO userWithoutIdDto) 
        {            
            if (userWithoutIdDto == null) 
            {
                return BadRequest(new ApiResponse { Message = "User data required", Data = null }); 
            }

            try
            {
                var mappedUser = _mapper.Map<User>(userWithoutIdDto); 

                int userId = await _userService.CreateUserAsync(mappedUser);

                // If it is created id will be > 0
                if (userId > 0) 
                {
                    return Ok(new ApiResponse
                    {
                        Message = "User created",
                        Data = userId
                    });

                }
                return BadRequest(new ApiResponse { Message = "Error when creating the user", Data = null }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse { Message = "Error when creating the user", Error = ex.Message }); 
            }
        }

        //[HttpGet("GetUserByIdAsync/{id}")]
        //[AuthorizeRoles("Administrator")]
        //public async Task<ActionResult<UserResponse>> GetUserByIdAsync(int id)
        //{
        //    try
        //    {
        //        var usuarioEncontrado = await _userService.ObtenerUsuarioPorIdAsync(id);

        //        if (usuarioEncontrado == null)
        //        {
        //            return Ok(new UserResponse
        //            {
        //                Message = $"No se encontró el usuario con ID {id}",
        //                Usuarios = new List<User>()
        //            });
        //        }
               
        //        var usuario = new User
        //        {
        //            UsuarioId = usuarioEncontrado.UsuarioId,
        //            Nombre = usuarioEncontrado.Nombre,
        //            Email = usuarioEncontrado.Email,
        //            Password = usuarioEncontrado.Password,
        //            RolId = usuarioEncontrado.RolId                    
        //        };

        //        return Ok(new UserResponse
        //        {
        //            Message = "Usuario encontrado",
        //            Usuarios = new List<User> { usuario }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error en ObtenerUsuarioPorIdAsync: " + ex.Message);
        //        return StatusCode(500, new UserResponse
        //        {
        //            Message = "Error interno del servidor",
        //            Usuarios = new List<User>()
        //        });
        //    }
        //}

        //[HttpGet("GetAllTheUsersAsync")]
        //[AuthorizeRoles("Administrator")]
        //public async Task<ActionResult<UsersWithRoleResponse>> GetAllTheUsersAsync()
        //{
        //    try
        //    {
        //        var usuarios = await _userService.ObtenerTodosLosUsuariosAsync();

        //        if (usuarios == null || !usuarios.Any())
        //        {
        //            return Ok(new UsersWithRoleResponse { Message = "No se encontraron usuarios", Usuarios = new List<UserWithRoleDTO>() });
        //        }

        //        return Ok(new UsersWithRoleResponse { Usuarios = usuarios });
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error en GetUsuarios: " + ex.Message);
        //        return StatusCode(500, new UsersWithRoleResponse { Message = "Error interno del servidor" });
        //    }
        //}

        //[HttpDelete("DeleteUser/{id}")]
        //[AuthorizeRoles("Administrator")]
        //public async Task<ActionResult<ApiResponse>> DeleteUser(int id)
        //{
        //    if (id <= 0)
        //    {
        //        return BadRequest(new ApiResponse { Message = "Los datos del usuario son requeridos.", Data = null });
        //    }
        //    try
        //    {
        //        var resultado = await _userService.BorrarUsuarioAsync(id);

        //        if (resultado)
        //        {
        //            return Ok(new ApiResponse
        //            {
        //                Message = "Usuario y sus tareas asociadas eliminados exitosamente.",
        //                Data = null
        //            });
        //        }
        //        return NotFound(new ApiResponse { Message = "Usuario no encontrado.", Data = null });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new ApiResponse { Message = "Error al borrar el usuario.", Error = ex.Message });
        //    }
        //}

        //private string GenerateJwtToken(LoginDTO login, string rolName)
        //{
        //    var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

        //    var claims = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
        //        new Claim("correo", login.Correo),
        //        new Claim("password", login.Password),
        //        new Claim(ClaimTypes.Role, nombreRol)
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
        //    var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        jwt.Issuer,
        //        jwt.Audience,
        //        claims,
        //        expires: DateTime.Now.AddMinutes(40),
        //        signingCredentials: signingCredentials
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}       

        //[HttpPut("UpdateUser/{id}")]
        //[AuthorizeRoles("Administrator")]
        //public async Task<ActionResult<ApiResponse>> UpdateUser(int id, [FromBody] UserWithRoleDTO userWithRolDTO)
        //{
        //    if (id <= 0)
        //    {
        //        return BadRequest(new ApiResponse { Message = "El ID del usuario es requerido.", Data = null });
        //    }

        //    if (usuarioDTO == null)
        //    {
        //        return BadRequest(new ApiResponse { Message = "Los datos del usuario son requeridos.", Data = null });
        //    }

        //    try
        //    {
        //        var usuarioExistente = await _userService.ObtenerUsuarioPorIdAsync(id);
        //        if (usuarioExistente == null)
        //        {
        //            return NotFound(new ApiResponse { Message = "Usuario no encontrado.", Data = null });
        //        }

        //        // cambio: mapeo manual de propiedades
        //        usuarioExistente.UsuarioId = usuarioDTO.UsuarioId;
        //        usuarioExistente.Nombre = usuarioDTO.Nombre;
        //        usuarioExistente.Email = usuarioDTO.Email;
        //        usuarioExistente.Password = usuarioDTO.Password;
        //        usuarioExistente.RolId = usuarioDTO.RolId;

        //        var resultado = await _userService.ActualizarUsuarioAsync(usuarioExistente);

        //        if (resultado)
        //        {
        //            return Ok(new ApiResponse
        //            {
        //                Message = "Usuario actualizado exitosamente.",
        //                Data = null
        //            });
        //        }

        //        return BadRequest(new ApiResponse { Message = "Error al actualizar el usuario.", Data = null });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new ApiResponse { Message = "Error al actualizar el usuario.", Error = ex.Message });
        //    }
        //}       
    }
}
