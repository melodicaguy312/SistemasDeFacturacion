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

    public static void Modificar(string valor, string nombre, string apellidos, string direccion, int telefono, string mail)
    {
        using (var conexion = Conexion.Conectar())
        {
            string cadenaSQL = @"UPDATE clientes SET valor = @valor WHERE id = @id;";
            using (var comando = new SqliteCommand(cadenaSQL, conexion))
            {
                
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
                }
            }
        }
    }
}