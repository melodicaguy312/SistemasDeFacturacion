namespace SistemaDeFacturacion;

public class Menu
{
    public static void Menu_Cliente()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("########");
        Console.WriteLine("Clientes");
        Console.WriteLine("########");
        Console.ResetColor();
        Console.WriteLine("1 - Alta de cliente");
        Console.WriteLine("2 - Modificar cliente");
        Console.WriteLine("3 - Eliminar Cliente");
        Console.WriteLine("4 - Listado de todos los clientes");
        Console.WriteLine("5 - Buscar cliente por ID");
        Console.WriteLine("6 - Facturas de cliente por ID de cliente");
        Console.WriteLine("0 - Volver al menú principal");
        Console.WriteLine("##############################");
        Console.Write("Opción: ");
    }

    public static void Menu_Productos()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("########");
        Console.WriteLine("Productos");
        Console.WriteLine("########");
        Console.ResetColor();
        Console.WriteLine("1 - Alta de producto");
        Console.WriteLine("2 - Modificar producto");
        Console.WriteLine("3 - Eliminar producto");
        Console.WriteLine("4 - Listado de todos los productos");
        Console.WriteLine("5 - Buscar producto por ID");
        Console.WriteLine("6 - Número total de productos vendidos por ID de producto");
        Console.WriteLine("0 - Volver al menú principal");
        Console.WriteLine("##############################");
        Console.Write("Opción: ");
    }
    
    public static void Menu_Facturas()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("########");
        Console.WriteLine("Facturas");
        Console.WriteLine("########");
        Console.ResetColor();
        Console.WriteLine("1 - Alta de factura");
        Console.WriteLine("2 - Modificar factura");
        Console.WriteLine("3 - Eliminar factura");
        Console.WriteLine("4 - Visualizar factura");
        Console.WriteLine("0 - Volver al menú principal");
        Console.WriteLine("##############################");
        Console.Write("Opción: ");

    }
    
    public static void Menu_Principal()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("##############################");
        Console.WriteLine("# SISTEMA DE FACTURACIÓN #");
        Console.WriteLine("##############################");
        Console.ResetColor();
        Console.WriteLine("Empresa: TECH X");
        Console.WriteLine("CIF: K223784609");
        Console.WriteLine("1 - Clientes");
        Console.WriteLine("2 - Productos");
        Console.WriteLine("3 - Facturas");
        Console.WriteLine("0 - Salir");
        Console.WriteLine("##############################");
        Console.Write("Opción: ");
    }
}