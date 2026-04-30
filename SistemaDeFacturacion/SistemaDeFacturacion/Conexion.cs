using Microsoft.Data.Sqlite;

namespace SistemaDeFacturacion;

public class Conexion
{
    public static string CadenaConexion { get; set; } = "";

    public static SqliteConnection Conectar()
    {
        SqliteConnection conn = new SqliteConnection(CadenaConexion);
        conn.Open();
        return conn;
    }
}