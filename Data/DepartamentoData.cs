using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using Modelo;

namespace Data
{
    public class DepartamentoData
    {
        private readonly ConnectionStrings conexiones;
        public DepartamentoData(IOptions<ConnectionStrings>options)
        {
                conexiones = options.Value;
        }
        public async Task<List<Departamento>> Listar() {
            List<Departamento> lista = new List<Departamento>();
            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_DepartamentoListar", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Departamento
                        {
                            IdDepartamento = Convert.ToInt32(reader["idDepartamento"]),
                            Nombre = reader["nombre"].ToString()                            
                        });
                    }
                }
            }
            return lista;
        }
        public async Task<bool>Insertar(Departamento objeto) {
            bool respuesta = true;
            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                SqlCommand cmd = new SqlCommand("sp_DepartamentoInsertar", conexion);
                cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);                
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
        public async Task<bool>Editar(Departamento objeto){
            bool respuesta = true;
            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                SqlCommand cmd = new SqlCommand("sp_DepartamentoEditar", conexion);
                cmd.Parameters.AddWithValue("@idDepartamento", objeto.IdDepartamento);
                cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);                
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
        public async Task<bool>Eliminar(int id) {
            bool respuesta = true;
            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                SqlCommand cmd = new SqlCommand("sp_DepartamentoEliminar", conexion);
                cmd.Parameters.AddWithValue("@idDepartamento", id);
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
