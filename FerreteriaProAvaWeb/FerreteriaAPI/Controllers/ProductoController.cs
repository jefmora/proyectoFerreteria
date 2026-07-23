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

            if (response > 0 || response == -1)
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

            if (response > 0 || response == -1)
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

            if (response > 0 || response == -1)
                return Ok("Estado del producto cambiado correctamente");

            return BadRequest("Error al cambiar el estado del producto");
        }

        [HttpGet("ConsultarCategoriasAPI")]
        public IActionResult ConsultarCategoriasAPI()
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var response = context.Query<CategoriaModel>("spConsultarCategorias", commandType: CommandType.StoredProcedure);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("RegistrarCategoriaAPI")]
        public IActionResult RegistrarCategoriaAPI(CategoriaModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@Nombre", model.Nombre);
            parameters.Add("@Descripcion", model.Descripcion);

            var response = context.Execute("spRegistrarCategoria", parameters, commandType: CommandType.StoredProcedure);

            if (response > 0 || response == -1)
                return Ok("Categoría registrada correctamente");

            return BadRequest("Error al registrar la categoría");
        }

        [Authorize]
        [HttpGet("ConsultarCategoriaPorIdAPI")]
        public IActionResult ConsultarCategoriaPorIdAPI(int idCategoria)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@IdCategoria", idCategoria);

            var response = context.QueryFirstOrDefault<CategoriaModel>(
                "spConsultarCategoriaPorId",
                parameters,
                commandType: CommandType.StoredProcedure);

            if (response != null)
                return Ok(response);

            return NotFound("Categoría no encontrada");
        }

        [Authorize]
        [HttpPut("ActualizarCategoriaAPI")]
        public IActionResult ActualizarCategoriaAPI(CategoriaModel model)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@IdCategoria", model.IdCategoria);
            parameters.Add("@Nombre", model.Nombre);
            parameters.Add("@Descripcion", model.Descripcion);

            var response = context.Execute("spActualizarCategoria", parameters, commandType: CommandType.StoredProcedure);

            if (response > 0 || response == -1)
                return Ok("Categoría actualizada correctamente");

            return BadRequest("Error al actualizar la categoría");
        }

        [Authorize]
        [HttpPut("CambiarEstadoCategoriaAPI")]
        public IActionResult CambiarEstadoCategoriaAPI(int idCategoria)
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var parameters = new DynamicParameters();
            parameters.Add("@IdCategoria", idCategoria);

            var response = context.Execute("spCambiarEstadoCategoria", parameters, commandType: CommandType.StoredProcedure);

            if (response > 0 || response == -1)
                return Ok("Estado de la categoría cambiado correctamente");

            return BadRequest("Error al cambiar el estado de la categoría");
        }
    }
}