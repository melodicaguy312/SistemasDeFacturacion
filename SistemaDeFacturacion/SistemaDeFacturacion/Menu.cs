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
        Console.WriteLine("Opción: ");
        Console.WriteLine("###############################");
    }

    public static void Menu_Productos()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("########");
        Console.WriteLine("Productos");
        Console.WriteLine("########");
        Console.ResetColor();
        Console.WriteLine("1 - Alta de artículo");
        Console.WriteLine("2 - Modificar artículo");
        Console.WriteLine("3 - Eliminar artículo");
        Console.WriteLine("4 - Listado de todos los artículos");
        Console.WriteLine("5 - Buscar artículo por ID");
        Console.WriteLine("6 - Número total de artículos vendidos por ID de artículo");
        Console.WriteLine("0 - Volver al menú principal");
        Console.WriteLine("##############################");
        Console.WriteLine("Opción: ");
        Console.WriteLine("###############################");
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
        Console.WriteLine("Opción: ");
        Console.WriteLine("###############################");
    }
    
    /* ##################################################
    # SISTEMA DE FACTURACIÓN #
    ##################################################
    Empresa: LOS CLAVOS DE CRISTO
    CIF: B00121109
    1) Clientes
    2) Artículos
    3) Facturas
    0) Salir
    Opción:
    ###################################################
*/
    
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
        Console.WriteLine("2 - Artículos");
        Console.WriteLine("3 - Facturas");
        Console.WriteLine("0 - Salir");
        Console.WriteLine("##############################");
        Console.WriteLine("Opción: ");
        Console.WriteLine("###############################");
    }
}