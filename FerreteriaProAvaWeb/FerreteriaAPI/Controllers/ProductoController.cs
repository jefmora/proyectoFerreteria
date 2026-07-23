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

        [HttpGet("ConsultarProducto")]
        public IActionResult ConsultarProducto(int idProducto)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var response = context.QueryFirstOrDefault<ProductoModel>(
                "spConsultarProducto",
                new
                {
                    IdProducto = idProducto
                },
                commandType: CommandType.StoredProcedure);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPut("ActualizarProducto")]
        public IActionResult ActualizarProducto([FromBody] ProductoModel producto)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parametros = new
            {
                producto.IdProducto,
                producto.SKU,
                producto.Nombre,
                producto.Precio,
                producto.StockActual,
                producto.StockMinimo,
                producto.Estado,
                producto.IdCategoria
            };

            context.Execute(
                "spActualizarProducto",
                parametros,
                commandType: CommandType.StoredProcedure);

            return Ok(new
            {
                mensaje = "Producto actualizado correctamente."
            });
        }

        [HttpDelete("EliminarProducto")]
        public IActionResult EliminarProducto(int idProducto)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            context.Execute(
                "spEliminarProducto",
                new
                {
                    IdProducto = idProducto
                },
                commandType: CommandType.StoredProcedure);

            return Ok(new
            {
                mensaje = "Producto eliminado correctamente."
            });
        }

        [HttpPost("InsertarProducto")]
        public IActionResult InsertarProducto([FromBody] ProductoModel producto)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parametros = new
            {
                producto.SKU,
                producto.Nombre,
                producto.Precio,
                producto.StockActual,
                producto.StockMinimo,
                producto.Estado,
                producto.IdCategoria
            };

            context.Execute(
                "spInsertarProducto",
                parametros,
                commandType: CommandType.StoredProcedure);

            return Ok(new
            {
                mensaje = "Producto registrado correctamente."
            });
        }


    }
}