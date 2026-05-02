using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace SistemaDeFacturacion;

public class Conexion
{
    
    public static SqliteConnection Conectar()
    {
        string rutaCompleta = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "bbdd/tienda.db");
        

        string connString = $"Data Source={rutaCompleta}";

        SqliteConnection conn = new SqliteConnection(connString);
        conn.Open();
        return conn;
    }
}