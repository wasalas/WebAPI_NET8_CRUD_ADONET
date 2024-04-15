using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using Modelo;

namespace Data
{
    public class EmpleadoData
    {
        private readonly ConnectionStrings conexiones;
        public EmpleadoData(IOptions<ConnectionStrings> options)
        {
            conexiones = options.Value;
        }
        public async Task<List<Empleado>> Listar()
        {
            List<Empleado> lista = new List<Empleado>();
            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_EmpleadoListar", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Empleado
                        {
                            IdEmpleado = Convert.ToInt32(reader["idEmpleado"]),
                            NombreCompleto = reader["nombreCompleto"].ToString(),
                            Sueldo = Convert.ToDecimal(reader["sueldo"]),
                            FechaContrato = reader["fechaContrato"].ToString(),
                            Departamento = new Departamento
                            {
                                IdDepartamento = Convert.ToInt32(reader["idDepartamento"]),
                                Nombre = reader["nombre"].ToString()
                            }
                        });
                    }
                }
            }
            return lista;
        }
        public async Task<bool> Insertar(Empleado objeto)
        {
            bool respuesta = true;
            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                SqlCommand cmd = new SqlCommand("sp_EmpleadoInsertar", conexion);
                cmd.Parameters.AddWithValue("@nombreCompleto", objeto.NombreCompleto);
                cmd.Parameters.AddWithValue("@idDepartamento", objeto.Departamento!.IdDepartamento);
                cmd.Parameters.AddWithValue("@sueldo", objeto.Sueldo);
                cmd.Parameters.AddWithValue("@fechaContrato", objeto.FechaContrato);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await conexion.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
        public async Task<bool> Editar(Empleado objeto)
        {
            bool respuesta = true;
            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                SqlCommand cmd = new SqlCommand("sp_EmpleadoEditar", conexion);
                cmd.Parameters.AddWithValue("@idEmpleado", objeto.IdEmpleado);
                cmd.Parameters.AddWithValue("@nombreCompleto", objeto.NombreCompleto);
                cmd.Parameters.AddWithValue("@idDepartamento", objeto.Departamento!.IdDepartamento);
                cmd.Parameters.AddWithValue("@sueldo", objeto.Sueldo);
                cmd.Parameters.AddWithValue("@fechaContrato", objeto.FechaContrato);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await conexion.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
        public async Task<bool> Eliminar(int id)
        {
            bool respuesta = true;
            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                SqlCommand cmd = new SqlCommand("sp_EmpleadoEliminar", conexion);
                cmd.Parameters.AddWithValue("@idEmpleado", id);
                
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await conexion.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;
                }
                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
    }
}
