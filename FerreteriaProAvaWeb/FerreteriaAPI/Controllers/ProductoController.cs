using Dapper;
using FerreteriaAPI.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet("ConsultarProductosAdminAPI")]
        public IActionResult ConsultarProductosAdminAPI()
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var response = context.Query<ProductoModel>(
                "spConsultarProductosAdmin",
                commandType: CommandType.StoredProcedure);

            return Ok(response);
        }

        [Authorize]
        [HttpGet("ConsultarProductoPorIdAPI")]
        public IActionResult ConsultarProductoPorIdAPI(int idProducto)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@IdProducto", idProducto);

            var response = context.QueryFirstOrDefault<ProductoModel>(
                "spConsultarProductoPorId",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (response != null)
                return Ok(response);

            return NotFound("Producto no encontrado");
        }

        [Authorize]
        [HttpPost("RegistrarProductoAPI")]
        public IActionResult RegistrarProductoAPI(ProductoModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@SKU", model.SKU);
            parameters.Add("@Nombre", model.Nombre);
            parameters.Add("@Precio", model.Precio);
            parameters.Add("@StockActual", model.StockActual);
            parameters.Add("@StockMinimo", model.StockMinimo);
            parameters.Add("@IdCategoria", model.IdCategoria <= 0 ? 1 : model.IdCategoria);

            var response = context.Execute("spRegistrarProducto", parameters, commandType: CommandType.StoredProcedure);

            if (response > 0)
                return Ok("Producto registrado correctamente");

            return BadRequest("Error al registrar el producto");
        }

        [Authorize]
        [HttpPut("ActualizarProductoAPI")]
        public IActionResult ActualizarProductoAPI(ProductoModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@IdProducto", model.IdProducto);
            parameters.Add("@SKU", model.SKU);
            parameters.Add("@Nombre", model.Nombre);
            parameters.Add("@Precio", model.Precio);
            parameters.Add("@StockActual", model.StockActual);
            parameters.Add("@StockMinimo", model.StockMinimo);
            parameters.Add("@IdCategoria", model.IdCategoria <= 0 ? 1 : model.IdCategoria);

            var response = context.Execute("spActualizarProducto", parameters, commandType: CommandType.StoredProcedure);

            if (response > 0)
                return Ok("Producto actualizado correctamente");

            return BadRequest("Error al actualizar el producto");
        }

        [Authorize]
        [HttpPut("CambiarEstadoProductoAPI")]
        public IActionResult CambiarEstadoProductoAPI(int idProducto)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@IdProducto", idProducto);

            var response = context.Execute("spCambiarEstadoProducto", parameters, commandType: CommandType.StoredProcedure);

            if (response > 0)
                return Ok("Estado del producto cambiado correctamente");

            return BadRequest("Error al cambiar el estado del producto");
        }

        [HttpGet("ConsultarCategoriasAPI")]
        public IActionResult ConsultarCategoriasAPI()
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var response = context.Query<dynamic>("spConsultarCategorias", commandType: CommandType.StoredProcedure);
            return Ok(response);
        }
    }
}