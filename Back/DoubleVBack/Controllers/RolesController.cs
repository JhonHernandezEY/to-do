using DoubleV.DTOs;
using DoubleV.Helpers;
using DoubleV.Interfaces;
using DoubleV.Models;
using DoubleV.Servicios;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoubleV.Controllers
{
    [ApiController]
    [EnableCors("AllowOrigins")]
    [Route("api/[controller]")]
    public class RolesController : Controller
    {
        private readonly IRoleService _rolService;

        public RolesController(IRoleService rolService)
        {
            _rolService = rolService;
        }

        [HttpGet("GetAllTheRolesAsync")]
        [AuthorizeRoles("Administrator")]
        public async Task<ActionResult<RolesResponse>> GetAllTheRolesAsync()
        {
            try
            {
                var roles = await _rolService.GetAllTheRolesAsync();

                if (roles == null || !roles.Any())
                {
                    return Ok(new RolesResponse { Message = "Roles not found", Roles = new List<Role>() });
                }

                return Ok(new RolesResponse { Message = "Roles found", Roles = roles });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with GetAllTheRolesAsync: " + ex.Message);
                return StatusCode(500, new RolesResponse { Message = "Internal Server Error" });
            }
        }

    }
}
