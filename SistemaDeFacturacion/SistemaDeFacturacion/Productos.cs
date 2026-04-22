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
                }
            }
        }
    }
  
    public static void Insertar(string nombre_producto, double precio_unitario)
    {
        using (var conexion = Conexion.Conectar())
        {
            string cadenaSQL = @"INSERT INTO clientes(nombre_producto, precio_unitario) VALUES(@nombre_producto, @precio_unitario)";
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
}