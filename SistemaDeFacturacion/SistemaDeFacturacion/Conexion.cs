using Microsoft.Data.Sqlite;

namespace SistemaDeFacturacion;

public class Conexion
{
    public static SqliteConnection Conectar()
    {
        string ruta = @"C:\Users\ismat\Documents\SistemasDeFacturacion\SistemaDeFacturacion\SistemaDeFacturacion\bbdd\tienda.db";
        string connString = $"Data Source={ruta}";
        SqliteConnection conn = new SqliteConnection(connString);
        
        conn.Open();
        Console.WriteLine("Conexión abierta en: " + ruta);
        return conn;
    }
}