using Microsoft.Data.Sqlite;
namespace SistemaDeFacturacion;

public class Facturas
{
    public static void Insertar(string codigo_factura, string fecha)
    {
        using (var conexion = Conexion.Conectar())
        {
            string cadenaSQL = "INSERT into facturas(codigo_factura, fecha) VALUES(@codigo_factura, @fecha)";
            using (var comando = new SqliteCommand(cadenaSQL, conexion))
            {
                if (string.IsNullOrWhiteSpace(codigo_factura))
                {
                    comando.Parameters.AddWithValue("@codigo_factura", DBNull.Value);
                }
                else
                {
                    comando.Parameters.AddWithValue("@codigo_factura", codigo_factura);
                }

                if (string.IsNullOrWhiteSpace(fecha))
                {
                    comando.Parameters.AddWithValue("@fecha", DBNull.Value);
                }
                else
                {
                    comando.Parameters.AddWithValue("@fecha", fecha);
                }
                
                comando.ExecuteNonQuery();
            }
            Console.WriteLine();
            Console.WriteLine("Factura añadida correctamente.");
            Console.ReadKey();
        }
    }

    public static void ModificarCabeceraFactura()
    {
        
    }

    public static void ModificarLineaFactura(int id_factura, int id_linea, string columna, string valor)
    {
        string[] columnasPermitidas = { "codigo_producto", "precio_unitario", "cantidad", "precio_total" };
        
        if (!columnasPermitidas.Contains(columna.ToLower()))
        {
            Console.WriteLine("Columna inválida.");
            Console.ReadKey();
            return;
        }
            
        using (var conexion = Conexion.Conectar())
        {
            string cadenaSQL = $"UPDATE lineas_factura SET {columna} = @valor WHERE id_factura = @id_factura AND num_linea = @id_linea;";
            using (var comando = new SqliteCommand(cadenaSQL, conexion))
            {
                comando.Parameters.AddWithValue("@valor", valor);
                comando.Parameters.AddWithValue("@id_factura", id_factura);
                comando.Parameters.AddWithValue("@id_linea", id_linea);
                
                
                int filasActualizadas = comando.ExecuteNonQuery();
                if (filasActualizadas > 0)
                {
                    Console.WriteLine($"Filas modificadas {filasActualizadas}.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine($"No se han modificado las filas.");
                    Console.ReadKey();
                }
            }
            Console.ReadKey();
        }
    }

    public static void VisualizarFactura()
    {
        // hacer
    }
    public static void MostrarTodos()
    {
        using (var conexion = Conexion.Conectar())
        {
            string cadenaSQL = "SELECT * FROM facturas";
            string cabecera = $"{"ID Factura",-4} | {"Codigo Factura",-20} | {"Fecha",-20} | {"ID Cliente"} \n";
            int contador = 0;
            
            Console.WriteLine(cabecera);

            int id_factura;
            string codigo_factura; string fecha; int id_cliente;
            
            using (SqliteCommand comando = new SqliteCommand(cadenaSQL, conexion))
            using (var reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    id_factura = reader.GetInt32(0);
                    codigo_factura = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    fecha = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    id_cliente = reader.GetInt32(3);

                    Console.WriteLine($"{id_factura,-4} | {codigo_factura,-20} | {fecha,-20} | {id_cliente,-20}");
                    contador++;

                    
                    if (contador % 20 == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Introduzca cualquier letra para continuar o 's' para salir del listado.");
                        string opcion = Console.ReadLine().ToLower();
                        if (opcion == "s") break;
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Introduzca cualquier letra para salir del listado.");
                Console.ReadKey(true);
            }
            Console.ReadKey();
        }
    }

    public static void Eliminar(int id)
    {
        using (var conexion = Conexion.Conectar())
        {
            string sqlContar = "SELECT COUNT(*) FROM lineas_factura WHERE cod_producto = @id;";;
            int vecesAsociado = 0;

            using (var comandoContar = new SqliteCommand(sqlContar, conexion))
            {
                comandoContar.Parameters.AddWithValue("@id", id);
                vecesAsociado = Convert.ToInt32(comandoContar.ExecuteScalar());
            }
            
            if (vecesAsociado > 0)
            {
                Console.WriteLine();
                Console.WriteLine($"ERROR: El producto no se puede eliminar porque está en {vecesAsociado} lineas de la factura.");
                return;
            }
            
            Console.WriteLine();
            Console.WriteLine($"¿Estás seguro de que deseas eliminar el producto con ID {id}? (s/n): ");
            string respuesta = Console.ReadLine().ToLower();

            if (respuesta != "s")
            {
                Console.WriteLine("Cancelado.");
                return;
            }


            string cadenaSQL = "DELETE FROM productos WHERE codigo_producto = @id;";
            using (var comando = new SqliteCommand(cadenaSQL, conexion))
            {
                comando.Parameters.AddWithValue("@id", id);
            
                int filasActualizadas = comando.ExecuteNonQuery();
                if (filasActualizadas > 0)
                {
                    Console.WriteLine($"Producto eliminado. Filas afectadas: {filasActualizadas}.");
                }
                else
                {
                    Console.WriteLine("No se encontró ningún producto con ese ID.");
                }
            }
            Console.ReadKey();
        }
    }
    
    public static void ModificarCabecera(int idFactura, string nuevaFecha, int nuevoIdCliente)
    {
        using (var conexion = Conexion.Conectar())
        {
            // 1. Extraer el año de la nueva fecha
            DateTime fecha = DateTime.Parse(nuevaFecha);
            string nuevoAño = fecha.Year.ToString();
        
            string nuevoCodigo = $"{idFactura}/{nuevoAño}";

            string cadenaSQL = "UPDATE facturas SET codigo_factura = @cod, fecha = @fecha, id_cliente = @idCliente WHERE id_factura = @idFactura";
            using (var comando = new SqliteCommand(cadenaSQL, conexion))
            {
                comando.Parameters.AddWithValue("@cod", nuevoCodigo);
                comando.Parameters.AddWithValue("@fecha", nuevaFecha);
                comando.Parameters.AddWithValue("@idCliente", nuevoIdCliente);
                comando.Parameters.AddWithValue("@idFactura", idFactura);
                comando.ExecuteNonQuery();
            }
            Console.ReadKey();
        }
    }
}