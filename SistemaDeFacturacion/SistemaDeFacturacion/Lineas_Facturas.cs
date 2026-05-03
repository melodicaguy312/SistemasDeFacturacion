using Microsoft.Data.Sqlite;
namespace SistemaDeFacturacion;

public class Lineas_Factura
{
    public static void Insertar(int id_factura, int cod_producto, int cantidad)
    {
        try
        {
            using (var conexion = Conexion.Conectar())
            {
                int id_linea = 1;
                string sqlIdLinea = "SELECT COALESCE(MAX(id_linea), 0) + 1 FROM lineas_factura WHERE id_factura = @id_factura";
                using (var comando = new SqliteCommand(sqlIdLinea, conexion))
                {
                    comando.Parameters.AddWithValue("@id_factura", id_factura);
                    id_linea = Convert.ToInt32(comando.ExecuteScalar());
                }

                double precio_unitario = 0;
                string sqlPrecio = "SELECT precio_unitario FROM productos WHERE codigo_producto = @cod_prod";
                using (var comando = new SqliteCommand(sqlPrecio, conexion))
                {
                    comando.Parameters.AddWithValue("@cod_prod", cod_producto);
                    var resultado = comando.ExecuteScalar();
                    if (resultado != null)
                        precio_unitario = Convert.ToDouble(resultado);
                }

                double precio_total = precio_unitario * cantidad;

                string cadenaSQL = @"INSERT INTO lineas_factura (id_factura, id_linea, codigo_producto, precio_unitario, cantidad, precio_total) VALUES (@id_factura, @id_linea, @cod_prod, @precio_u, @cant, @precio_t)";

                using (var comando = new SqliteCommand(cadenaSQL, conexion))
                {
                    comando.Parameters.AddWithValue("@id_factura", id_factura);
                    comando.Parameters.AddWithValue("@id_linea", id_linea);
                    comando.Parameters.AddWithValue("@cod_prod", cod_producto);
                    comando.Parameters.AddWithValue("@precio_u", precio_unitario);
                    comando.Parameters.AddWithValue("@cant", cantidad);
                    comando.Parameters.AddWithValue("@precio_t", precio_total);

                    comando.ExecuteNonQuery();
                }
                Console.WriteLine();
                Console.WriteLine($"Línea {id_linea} añadida correctamente.");
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Error en SQLite: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error de argumento: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        Console.ReadKey();
    }

    public static void MostrarLineasPorIdFactura(int id_factura)
    {
        try
        {
            using (var conexion = Conexion.Conectar())
            {
                string cadenaSQL = @"SELECT id_linea, id_factura, codigo_producto, precio_unitario, cantidad, precio_total FROM lineas_factura WHERE id_factura = @id_factura";

                using (var comando = new SqliteCommand(cadenaSQL, conexion))
                {
                    comando.Parameters.AddWithValue("@id_factura", id_factura);

                    using (var reader = comando.ExecuteReader())
                    {
                        bool hayLineas = false;

                        while (reader.Read())
                        {
                            hayLineas = true;

                            Console.WriteLine();
                            Console.WriteLine("--- LÍNEA DE FACTURA ---");
                            Console.WriteLine($"{"ID Línea:",-16} {reader.GetInt32(0)}");
                            Console.WriteLine($"{"ID Factura:",-16} {id_factura}");
                            Console.WriteLine($"{"Cód. Producto:",-16} {reader.GetInt32(2)}");
                            Console.WriteLine($"{"Precio Unit.:",-16} {reader.GetDouble(3)} €");
                            Console.WriteLine($"{"Cantidad:",-16} {reader.GetInt32(4)}");
                            Console.WriteLine($"{"Precio Total:",-16} {reader.GetDouble(5)} €");
                            Console.WriteLine("------------------------");
                        }

                        if (!hayLineas)
                        {
                            Console.WriteLine();
                            Console.WriteLine($"No se han encontrado líneas para la factura con ID: {id_factura}");
                        }
                    }
                }
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Error en SQLite: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        Console.ReadKey();
    }

    public static void EliminarLinea(int id_factura, int id_linea)
    {
        try
        {
            using (var conexion = Conexion.Conectar())
            {
                string cadenaExiste = "SELECT COUNT(*) FROM lineas_factura WHERE id_factura = @id_fact AND id_linea = @id_lin";
                using (var comando = new SqliteCommand(cadenaExiste, conexion))
                {
                    comando.Parameters.AddWithValue("@id_fact", id_factura);
                    comando.Parameters.AddWithValue("@id_lin", id_linea);
                    if (Convert.ToInt32(comando.ExecuteScalar()) == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"No existe la línea {id_linea} en la factura {id_factura}.");
                        Console.ReadKey();
                        return;
                    }
                }

                Console.WriteLine();
                Console.Write($"¿Seguro que desea eliminar la línea {id_linea} de la factura {id_factura}? (s/n): ");
                string respuesta = Console.ReadLine().ToLower();

                if (respuesta == "s")
                {
                    string cadenaSQL = "DELETE FROM lineas_factura WHERE id_factura = @id_fact AND id_linea = @id_lin";
                    using (var comando = new SqliteCommand(cadenaSQL, conexion))
                    {
                        comando.Parameters.AddWithValue("@id_fact", id_factura);
                        comando.Parameters.AddWithValue("@id_lin", id_linea);
                        comando.ExecuteNonQuery();
                        Console.WriteLine();
                        Console.WriteLine("Línea eliminada correctamente.");
                    }
                }
                else
                {
                    Console.WriteLine("Operación cancelada.");
                }
            }
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"Error en SQLite: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        Console.ReadKey();
    }
}