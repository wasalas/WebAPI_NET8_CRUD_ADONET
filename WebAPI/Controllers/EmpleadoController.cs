using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Data;
using Modelo;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly EmpleadoData _empleadoData;
        public EmpleadoController(EmpleadoData empleadoData)
        {
            _empleadoData = empleadoData;
        }
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            List<Empleado> lista = await _empleadoData.Listar();
            return StatusCode(StatusCodes.Status200OK, lista);
        }
        
        [HttpPost]
        public async Task<IActionResult> Insertar([FromBody]Empleado objeto)
        {
            bool respuesta =  await _empleadoData.Insertar(objeto);
            return StatusCode(StatusCodes.Status200OK, new {isSuccess = respuesta });
        }

        [HttpPut]
        public async Task<IActionResult> Editar([FromBody] Empleado objeto)
        {
            bool respuesta = await _empleadoData.Editar(objeto);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            bool respuesta = await _empleadoData.Eliminar(id);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = respuesta });
        }
    }
}
