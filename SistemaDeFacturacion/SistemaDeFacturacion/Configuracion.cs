using System.Text.Json;
using System.IO;

namespace SistemaDeFacturacion;

public class Configuracion
{
    public string NombreEmpresa  { get; set; } = "";
    public string CIF            { get; set; } = "";
    public double IVA            { get; set; } = 21;
    public string CadenaConexion { get; set; } = "";
    
    public static Configuracion Ultima = new Configuracion();

    public static Configuracion Cargar(string ruta)
    {
        if (!File.Exists(ruta))
        {
            Console.WriteLine("El archivo de configuración no existe.");
            return new Configuracion();
        }

        try
        {
            string contenido = File.ReadAllText(ruta);
            Configuracion config = JsonSerializer.Deserialize<Configuracion>(contenido);
            Ultima = config;
            Console.WriteLine("Configuración cargada correctamente.");
            return config;
        }
        catch (JsonException)
        {
            Console.WriteLine("El archivo no tiene formato JSON válido.");
            return new Configuracion();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error general: {ex.Message}");
            return new Configuracion();
        }
    }
}