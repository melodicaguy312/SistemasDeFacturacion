using Microsoft.Data.Sqlite;
namespace SistemaDeFacturacion;

public class Clientes
{
    
    // INSERTAR
    public static void Insertar(string nombre, string apellidos, string direccion, int telefono, string mail)
    {
        using (var conexion = Conexion.Conectar())
        {
            string cadenaSQL = @"INSERT INTO clientes(nombre, apellidos, direccion, telefono, mail) VALUES(@nombre, @apellidos, @direccion, @telefono, @mail);";
            
            using (var comando = new SqliteCommand(cadenaSQL, conexion))
            {
                comando.Parameters.AddWithValue("@nombre", nombre);
                comando.Parameters.AddWithValue("@apellidos", apellidos);
                comando.Parameters.AddWithValue("@direccion", direccion);
                if (string.IsNullOrWhiteSpace(telefono.ToString()))
                {
                    comando.Parameters.AddWithValue("@telefono", DBNull.Value);
                }
                else
                {
                    comando.Parameters.AddWithValue("@telefono", telefono);
                }
                comando.Parameters.AddWithValue("@mail", mail);
                
                comando.ExecuteNonQuery();
            }
            Console.WriteLine();
            Console.WriteLine("Cliente añadido correctamente.");
        }
    }

    public static void Modificar(string columna, string valor, int id)
    {
        string[] columnasPermitidas = { "nombre", "apellidos", "direccion", "telefono", "mail" };
        
            if (!columnasPermitidas.Contains(columna.ToLower()))
            {
                Console.WriteLine("Columna inválida.");
                return;
            }
            
        using (var conexion = Conexion.Conectar())
        {
            string cadenaSQL = $"UPDATE clientes SET {columna} = @valor WHERE id = @id;";
            using (var comando = new SqliteCommand(cadenaSQL, conexion))
            {
                comando.Parameters.AddWithValue("@valor", valor);
                comando.Parameters.AddWithValue("@id", id);
                
                
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

    // MOSTRAR TODOS
    public static void MostrarTodos()
    {
        using (var conexion = Conexion.Conectar())
        {
            string cadenaSQL = "SELECT * FROM clientes";
            string cabecera = $"{"ID",-4} | {"Nombre",-20} | {"Apellidos",-20} | {"Dirección",-30} | {"Teléfono",-20} | {"Mail",-20} \n";
            int contador = 0;
            
            Console.WriteLine(cabecera);

            int id;
            string nombre, apellidos, direccion, telefono, mail;
            
            using (SqliteCommand comando = new SqliteCommand(cadenaSQL, conexion))
            using (var reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                    nombre = reader.GetString(1);
                    apellidos = reader.GetString(2);
                    direccion = reader.GetString(3);
                    telefono = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    mail = reader.GetString(5);

                    Console.WriteLine($"{id,-4} | {nombre,-20} | {apellidos,-20} | {direccion,-30} | {telefono,-20} | {mail,-20}");
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

    public static void Eliminar(int id)
    {
        using (var conexion = Conexion.Conectar())
        {
            string sqlContar = "SELECT COUNT(*) FROM facturas WHERE id_cliente = @id;";
            int totalFacturas = 0;

            using (var comandoContar = new SqliteCommand(sqlContar, conexion))
            {
                comandoContar.Parameters.AddWithValue("@id", id);
                totalFacturas = Convert.ToInt32(comandoContar.ExecuteScalar());
            }

            Console.WriteLine();
            Console.WriteLine($"El cliente con ID {id} tiene {totalFacturas} facturas.");
            Console.Write("¿Estas seguro de que quieres eliminar al cliente y todas sus facturas? (s/n): ");
            string respuesta = Console.ReadLine().ToLower();

            if (respuesta != "s")
            {
                Console.WriteLine("Cancelado.");
                return;
            }

            string cadenaSQL = "DELETE FROM clientes WHERE id = @id;";
            using (var comando = new SqliteCommand(cadenaSQL, conexion))
            {
                comando.Parameters.AddWithValue("@id", id);
                
                int filasActualizadas = comando.ExecuteNonQuery();
                if (filasActualizadas > 0)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Filas Eliminadas {filasActualizadas}.");
                    Console.WriteLine($"Se eliminaron {totalFacturas} facturas del cliente.");
                }
                else
                {
                    Console.WriteLine("No se han eliminado las filas.");
                }
            }
        }
    }
    
    public static void BuscarClientePorId(int id)
    {
        using (var conexion = Conexion.Conectar())
        {
            string cadenaSQL = "SELECT * FROM clientes WHERE id = @id";
            int idCliente;
            string nombre, apellidos, direccion, telefono, mail;
            using (var comando = new SqliteCommand(cadenaSQL, conexion))
            {
                comando.Parameters.AddWithValue("@id", id);

                using (var reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        idCliente = reader.GetInt32(0);
                        nombre = reader.GetString(1);
                        apellidos = reader.GetString(2);
                        direccion = reader.GetString(3);
                        telefono = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        mail = reader.GetString(5);
                        
                        Console.WriteLine();
                        Console.WriteLine("--- CLIENTE ENCONTRADO ---");
                        Console.WriteLine($"{"ID:",-12} {idCliente}");
                        Console.WriteLine($"{"Nombre:",-12} {nombre}");
                        Console.WriteLine($"{"Apellidos:",-12} {apellidos}");
                        Console.WriteLine($"{"Dirección:",-12} {direccion}");
                        Console.WriteLine($"{"Teléfono:",-12} {telefono}");
                        Console.WriteLine($"{"Mail:",-12} {mail}");
                        Console.WriteLine("--------------------------");
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"No se ha encontrado ningun cliente con el ID: {id}");
                    }
                }
            }
        }
    }
    
}