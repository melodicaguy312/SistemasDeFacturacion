using Microsoft.Data.Sqlite;
namespace SistemaDeFacturacion;


public class Productos
{
    public static void MostrarTodos()
    {
        using (var conexion = Conexion.Conectar())
        {
            string cadenaSQL = "SELECT * FROM productos";
            string cabecera = $"{"CODIGO_PRODUCTO",-4} | {"Nombre_Producto",-20} | {"Precio_Unitario",-20} |\n";
            int contador = 0;
            Console.WriteLine(cabecera);


            int codigo_producto;
            string nombre_producto;
            double precio_unitario;
          
            using (SqliteCommand comando = new SqliteCommand(cadenaSQL, conexion))
            using (var reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    codigo_producto = reader.GetInt32(0);
                    nombre_producto = reader.GetString(1);
                    precio_unitario = reader.GetDouble(2);
                    Console.WriteLine($"{codigo_producto,-4} | {nombre_producto,-20} | {precio_unitario,-20} |");
                    contador++;

                    
                    if (contador % 20 == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Introduzca cualquier letra para continuar o 's' para salir del listado.");
                        string opcion = Console.ReadLine().ToLower();
                        if (opcion == "s") break;
                    }
                }
            }
        }
    }
  
    public static void Insertar(string nombre_producto, double precio_unitario)
    {
        using (var conexion = Conexion.Conectar())
        {
            string cadenaSQL = @"INSERT INTO productos(nombre_producto, precio_unitario) VALUES(@nombre_producto, @precio_unitario)";
            using (var comando = new SqliteCommand(cadenaSQL, conexion))
            {
                comando.Parameters.AddWithValue("@nombre_producto", nombre_producto);
                comando.Parameters.AddWithValue("@precio_unitario", precio_unitario);
              
                comando.ExecuteNonQuery();
            }
            Console.WriteLine();
            Console.WriteLine("Producto añadido correctamente.");
        }
    }

    public static void Modificar(string columna, string valor, int codigo_producto)
    {
        string[] columnasPermitidas = { "codigo_producto", "nombre_producto", "precio_unitario" };

        if (!columnasPermitidas.Contains(columna.ToLower()))
        {
            Console.WriteLine("Columna inválida.");
            return;
        }

        using (var conexion = Conexion.Conectar())
        {
            string cadenaSQL = $"UPDATE productos SET {columna} = @valor WHERE codigo_producto = @codigo_producto;";
            using (var comando = new SqliteCommand(cadenaSQL, conexion))
            {
                comando.Parameters.AddWithValue("@valor", valor);
                comando.Parameters.AddWithValue("@codigo_producto", codigo_producto);


                int filasActualizadas = comando.ExecuteNonQuery();
                if (filasActualizadas > 0)
                {
                    Console.WriteLine($"Filas modificadas {filasActualizadas}.");
                }
                else
                {
                    Console.WriteLine($"No se han modificado las filas.");
                }
            }
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
                Console.WriteLine($"ERROR: El producto no se puede eliminado porque está en {vecesAsociado} lineas de la factura.");
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
        }
    }
    
    public static void BuscarProductoPorId(int codigo_producto)
    {
        using (var conexion = Conexion.Conectar())
        {
            string cadenaSQL = "SELECT * FROM productos WHERE codigo_producto = @codigo_producto";
            int cod_producto;
            double precio_unitario;
            string nombre_producto;
            using (var comando = new SqliteCommand(cadenaSQL, conexion))
            {
                comando.Parameters.AddWithValue("@codigo_producto", codigo_producto);

                using (var reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        cod_producto = reader.GetInt32(0);
                        nombre_producto = reader.GetString(1);
                        precio_unitario = reader.GetDouble(2);
                        
                        
                        Console.WriteLine();
                        Console.WriteLine("--- PRODUCTO ENCONTRADO ---");
                        Console.WriteLine($"{"ID:",-12} {cod_producto}");
                        Console.WriteLine($"{"Nombre:",-12} {nombre_producto}");
                        Console.WriteLine($"{"Precio Unitario:",-12} {precio_unitario}");
                        Console.WriteLine("--------------------------");
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"No se ha encontrado ningun producto con el codigo de producto: {codigo_producto}");
                    }
                }
            }
        }
    }
    
    public static void TotalArticulosVendidosPorCodigo(int codigo_producto)
    {
        using (var conexion = Conexion.Conectar())
        {
            string cadenaSQL = "SELECT SUM(cantidad) FROM lineas_factura WHERE cod_producto = @codigo_producto;";
        
            using (var comando = new SqliteCommand(cadenaSQL, conexion))
            {
                comando.Parameters.AddWithValue("@codigo_producto", codigo_producto);
            
                var suma = comando.ExecuteScalar();
            
                int totalVendidos = 0;

                if (suma != DBNull.Value && suma != null)
                {
                    totalVendidos = Convert.ToInt32(suma);
                }
                Console.WriteLine($"El artículo con codigo_producto {codigo_producto} ha vendido un total de: {totalVendidos} unidades.");
            }
        }
    }
}