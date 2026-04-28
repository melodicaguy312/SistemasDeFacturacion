using Microsoft.Data.Sqlite;
namespace SistemaDeFacturacion;

public class Clientes
{
    
    // INSERTAR
    public static void Insertar(string nombre, string apellidos, string direccion, int? telefono, string mail)
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
            Console.ReadKey();
        }
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
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine($"No se han modificado las filas.");
                    Console.ReadKey();
                }
            }
        }
    }
    
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
                Console.WriteLine();
                Console.WriteLine("Introduzca cualquier letra para salir del listado.");
                Console.ReadKey(true);
            }
        }
    }

    public static void Eliminar(int id)
    {
        using (var conexion = Conexion.Conectar())
        {
            string sqlExiste = "SELECT COUNT(*) FROM clientes WHERE id = @id;";
            using (var comandoExiste = new SqliteCommand(sqlExiste, conexion))
            {
                comandoExiste.Parameters.AddWithValue("@id", id);
                int existe = Convert.ToInt32(comandoExiste.ExecuteScalar());

                if (existe == 0)
                {
                    Console.WriteLine($"No existe ningún cliente con ID {id}.");
                    Console.ReadKey(true);
                    return;
                }
            }
            string sqlContar = "SELECT COUNT(*) FROM facturas WHERE id_cliente = @id;";
            int totalFacturas = 0;

            using (var comandoContar = new SqliteCommand(sqlContar, conexion))
            {
                comandoContar.Parameters.AddWithValue("@id", id);
                totalFacturas = Convert.ToInt32(comandoContar.ExecuteScalar());
            }

            Console.WriteLine();
            Console.WriteLine($"El cliente con ID {id} tiene {totalFacturas} facturas.");
            Console.WriteLine("¿Estas seguro de que quieres eliminar al cliente y todas sus facturas? (s/n): ");
            string respuesta;
            respuesta = Console.ReadLine().ToLower();
            
            if (respuesta == "n")
            {
                Console.WriteLine("Cancelado.");
                Console.ReadKey(true);
                return;
            }
            
            if (respuesta == "s")
            {
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
                        Console.ReadKey(true);
                    }
                    else
                    {
                        Console.WriteLine("No se han eliminado las filas.");
                        Console.ReadKey(true);
                    }
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
    
    public static void FacturasAsociadasAClientePorId(int id_cliente)
    {
        using (var conexion = Conexion.Conectar())
        {
            string cadenaSQL = "SELECT id_factura,codigo_factura,fecha FROM facturas WHERE id_cliente = @id_cliente";
            int id_factura;
            string codigo_factura, fecha;
            using (var comando = new SqliteCommand(cadenaSQL, conexion))
            {
                comando.Parameters.AddWithValue("@id_cliente", id_cliente);

                using (var reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        id_factura = reader.GetInt32(0);
                        codigo_factura = reader.GetString(1);
                        fecha = reader.GetString(2);
                        
                        
                        Console.WriteLine();
                        Console.WriteLine("--- FACTURAS ASOCIADAS DE CLIENTE ENCONTRADAS ---");
                        Console.WriteLine($"{"ID:",-12} {id_factura}");
                        Console.WriteLine($"{"Codigo Factura:",-12} {codigo_factura}");
                        Console.WriteLine($"{"Fecha:",-12} {fecha}");
                        Console.WriteLine("--------------------------");
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine($"No se ha encontrado ninguna factura asociada con el cliente con el ID: {id_cliente}");
                    }
                }
            }
        }
    }
}