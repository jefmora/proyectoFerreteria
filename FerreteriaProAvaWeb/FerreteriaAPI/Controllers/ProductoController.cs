using Dapper;
using FerreteriaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FerreteriaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ProductoController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("ConsultarProductos")]
        public IActionResult ConsultarProductos()
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var response = context.Query<ProductoModel>(
                "spConsultarProductos",
                commandType: CommandType.StoredProcedure);

            return Ok(response);
        }
    }
}