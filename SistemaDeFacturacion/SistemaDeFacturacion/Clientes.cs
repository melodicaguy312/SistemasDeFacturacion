using Microsoft.Data.Sqlite;

namespace SistemaDeFacturacion;

public class Clientes
{
    public static void Insertar(string nombre, string apellidos, string direccion, int? telefono, string mail)
    {
        try
        {
            using (var conexion = Conexion.Conectar())
            {
                string cadenaSQL = "INSERT INTO clientes(nombre, apellidos, direccion, telefono, mail) VALUES(@nombre, @apellidos, @direccion, @telefono, @mail);";

                using (var comando = new SqliteCommand(cadenaSQL, conexion))
                {
                    comando.Parameters.AddWithValue("@nombre", nombre);
                    comando.Parameters.AddWithValue("@apellidos", apellidos);
                    comando.Parameters.AddWithValue("@direccion", direccion);
                    if (string.IsNullOrWhiteSpace(telefono.ToString()))
                        comando.Parameters.AddWithValue("@telefono", DBNull.Value);
                    else
                        comando.Parameters.AddWithValue("@telefono", telefono);
                    comando.Parameters.AddWithValue("@mail", mail);

                    comando.ExecuteNonQuery();
                }
                Console.WriteLine("\nCliente añadido correctamente.");
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

    public static void Modificar(string columna, string valor, int id)
    {
        string[] columnasPermitidas = { "nombre", "apellidos", "direccion", "telefono", "mail" };

        if (!columnasPermitidas.Contains(columna.ToLower()))
        {
            Console.WriteLine("Columna inválida.");
            Console.ReadKey();
            return;
        }

        try
        {
            using (var conexion = Conexion.Conectar())
            {
                string cadenaSQL = $"UPDATE clientes SET {columna} = @valor WHERE id = @id;";
                using (var comando = new SqliteCommand(cadenaSQL, conexion))
                {
                    comando.Parameters.AddWithValue("@valor", valor);
                    comando.Parameters.AddWithValue("@id", id);

                    int filasActualizadas = comando.ExecuteNonQuery();
                    if (filasActualizadas > 0)
                        Console.WriteLine($"Cliente modificado correctamente. Filas afectadas: {filasActualizadas}.");
                    else
                        Console.WriteLine("No se encontró ningún cliente con ese ID.");
                }
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

    public static void MostrarTodos()
    {
        try
        {
            using (var conexion = Conexion.Conectar())
            {
                string cadenaSQL = "SELECT * FROM clientes";
                string cabecera = $"{"ID",-4} | {"Nombre",-20} | {"Apellidos",-20} | {"Dirección",-30} | {"Teléfono",-20} | {"Mail",-20} \n";
                int contador = 0;

                Console.WriteLine(cabecera);

                using (SqliteCommand comando = new SqliteCommand(cadenaSQL, conexion))
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string nombre = reader.GetString(1);
                        string apellidos = reader.GetString(2);
                        string direccion = reader.GetString(3);
                        string telefono = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        string mail = reader.GetString(5);

                        Console.WriteLine($"{id,-4} | {nombre,-20} | {apellidos,-20} | {direccion,-30} | {telefono,-20} | {mail,-20}");
                        contador++;

                        if (contador % 20 == 0)
                        {
                            Console.WriteLine("\nIntroduzca cualquier letra para continuar o 's' para salir del listado.");
                            string opcion = Console.ReadLine()?.ToLower();
                            if (opcion == "s") return;
                        }
                    }
                }
                Console.WriteLine("\nNo hay más registros.");
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

    public static void Eliminar(int id)
    {
        try
        {
            using (var conexion = Conexion.Conectar())
            {
                // Verificar si existe el cliente
                string sqlExiste = "SELECT COUNT(*) FROM clientes WHERE id = @id;";
                using (var comandoExiste = new SqliteCommand(sqlExiste, conexion))
                {
                    comandoExiste.Parameters.AddWithValue("@id", id);
                    if (Convert.ToInt32(comandoExiste.ExecuteScalar()) == 0)
                    {
                        Console.WriteLine($"No existe ningún cliente con ID {id}.");
                        Console.ReadKey(true);
                        return;
                    }
                }

                int totalFacturas = 0;
                string sqlContar = "SELECT COUNT(*) FROM facturas WHERE id_cliente = @id;";
                using (var comandoContar = new SqliteCommand(sqlContar, conexion))
                {
                    comandoContar.Parameters.AddWithValue("@id", id);
                    totalFacturas = Convert.ToInt32(comandoContar.ExecuteScalar());
                }

                Console.WriteLine($"\nEl cliente con ID {id} tiene {totalFacturas} facturas asociadas.");
                Console.Write("¿Estás seguro de que quieres eliminar al cliente y todas sus facturas? (s/n): ");
                string respuesta = Console.ReadLine()?.ToLower();

                if (respuesta != "s")
                {
                    Console.WriteLine("Operación cancelada.");
                    Console.ReadKey(true);
                    return;
                }

                // Borrar líneas de factura
                string sqlBorrarLineas = @"DELETE FROM lineas_factura 
                                           WHERE id_factura IN (SELECT id_factura FROM facturas WHERE id_cliente = @id);";
                using (var comando = new SqliteCommand(sqlBorrarLineas, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    comando.ExecuteNonQuery();
                }

                // Borrar facturas
                string sqlBorrarFacturas = "DELETE FROM facturas WHERE id_cliente = @id;";
                using (var comando = new SqliteCommand(sqlBorrarFacturas, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    comando.ExecuteNonQuery();
                }

                // Borrar cliente
                string sqlBorrarCliente = "DELETE FROM clientes WHERE id = @id;";
                using (var comando = new SqliteCommand(sqlBorrarCliente, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);
                    int filas = comando.ExecuteNonQuery();
                    if (filas > 0)
                    {
                        Console.WriteLine("\nCliente eliminado correctamente.");
                        Console.WriteLine($"Se eliminaron {totalFacturas} facturas y sus líneas asociadas.");
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
        Console.ReadKey(true);
    }

    public static void BuscarClientePorId(int id)
    {
        try
        {
            using (var conexion = Conexion.Conectar())
            {
                string cadenaSQL = "SELECT * FROM clientes WHERE id = @id";

                using (var comando = new SqliteCommand(cadenaSQL, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Console.WriteLine("\n--- CLIENTE ENCONTRADO ---");
                            Console.WriteLine($"{"ID:",-12} {reader.GetInt32(0)}");
                            Console.WriteLine($"{"Nombre:",-12} {reader.GetString(1)}");
                            Console.WriteLine($"{"Apellidos:",-12} {reader.GetString(2)}");
                            Console.WriteLine($"{"Dirección:",-12} {reader.GetString(3)}");
                            Console.WriteLine($"{"Teléfono:",-12} {(reader.IsDBNull(4) ? "" : reader.GetString(4))}");
                            Console.WriteLine($"{"Mail:",-12} {reader.GetString(5)}");
                            Console.WriteLine("--------------------------");
                        }
                        else
                        {
                            Console.WriteLine($"\nNo se ha encontrado ningún cliente con el ID: {id}");
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

    public static void FacturasAsociadasAClientePorId(int id_cliente)
    {
        try
        {
            using (var conexion = Conexion.Conectar())
            {
                string cadenaSQL = "SELECT id_factura, codigo_factura, fecha FROM facturas WHERE id_cliente = @id_cliente";

                using (var comando = new SqliteCommand(cadenaSQL, conexion))
                {
                    comando.Parameters.AddWithValue("@id_cliente", id_cliente);

                    using (var reader = comando.ExecuteReader())
                    {
                        bool hayFacturas = false;

                        while (reader.Read())
                        {
                            hayFacturas = true;
                            Console.WriteLine("\n--- FACTURA ASOCIADA ---");
                            Console.WriteLine($"{"ID Factura:",-16} {reader.GetInt32(0)}");
                            Console.WriteLine($"{"Código Factura:",-16} {reader.GetString(1)}");
                            Console.WriteLine($"{"Fecha:",-16} {reader.GetString(2)}");
                            Console.WriteLine("------------------------");
                        }

                        if (!hayFacturas)
                        {
                            Console.WriteLine($"\nNo se han encontrado facturas para el cliente con ID: {id_cliente}");
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
}