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
        [HttpGet("ConsultarProductosDestacados")]
        public IActionResult ConsultarProductosDestacados()
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var response = context.Query<ProductoModel>(
                "spConsultarProductosDestacados",
                commandType: CommandType.StoredProcedure);

            return Ok(response);
        }

        [HttpGet("ConsultarProducto")]
        public IActionResult ConsultarProducto(int idProducto)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var response = context.QueryFirstOrDefault<ProductoModel>(
                "spConsultarProducto",
                new { IdProducto = idProducto },
                commandType: CommandType.StoredProcedure);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

    }
}