using Dapper;
using FerreteriaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;


namespace FerreteriaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CategoriaController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("ConsultarCategorias")]
        public IActionResult ConsultarCategorias()
        {
            using var context = new SqlConnection(_config["ConnectionStrings:DefaultConnection"]);

            var response = context.Query<CategoriaModel>(
                "spConsultarCategorias",
                commandType: CommandType.StoredProcedure);

            return Ok(response);
        }
    }
}