using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace SistemaDeFacturacion;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        string ruta = Path.Combine(AppContext.BaseDirectory, "config.json");
        Configuracion config = Configuracion.Cargar(ruta);
        string entrada;
        bool salir_principal = false;

        while (!salir_principal)
        {
            Menu.Menu_Principal();
            entrada = Console.ReadLine();

            if (int.TryParse(entrada, out int opcion))
            {
                switch (opcion)
                {
                    case 1:
                        Console.Clear();
                        bool salir_cliente = false;

                        while (!salir_cliente)
                        {
                            Menu.Menu_Cliente();
                            string entrada_cliente = Console.ReadLine();

                            if (int.TryParse(entrada_cliente, out int opcion_cliente))
                            {
                                switch (opcion_cliente)
                                {
                                    case 1:
                                        Console.Clear();
                                        Console.Write("Nombre del cliente: ");
                                        string nombre_insertar = Console.ReadLine();
                                        Console.Write("Apellidos del cliente: ");
                                        string apellidos_insertar = Console.ReadLine();
                                        Console.Write("Direccion del cliente: ");
                                        string direccion_insertar = Console.ReadLine();
                                        
                                        Console.Write("Telefono del cliente: ");
                                        string telefono_texto = Console.ReadLine();
                                        int? telefono_insertar = int.TryParse(telefono_texto, out int tel_val) ? tel_val : null;
                                        
                                        Console.Write("Mail del cliente: ");
                                        string mail_insertar = Console.ReadLine();

                                        Clientes.Insertar(nombre_insertar, apellidos_insertar, direccion_insertar, telefono_insertar, mail_insertar);
                                        break;

                                    case 2:
                                        Console.Clear();
                                        Console.Write("Columna (nombre, apellidos, direccion, telefono, mail): ");
                                        string columna_modificar = Console.ReadLine();
                                        Console.Write("Valor nuevo: ");
                                        string valor_modificar = Console.ReadLine();
                                        
                                        Console.Write("Donde la ID sea: ");
                                        int id_modificar;
                                        while (!int.TryParse(Console.ReadLine(), out id_modificar))
                                        {
                                            Console.Write("ID no válido. Inténtelo de nuevo: ");
                                        }

                                        Clientes.Modificar(columna_modificar, valor_modificar, id_modificar);
                                        break;

                                    case 3:
                                        Console.Clear();
                                        Console.Write("ID del cliente que desea eliminar: ");
                                        int id_Eliminar;
                                        while (!int.TryParse(Console.ReadLine(), out id_Eliminar))
                                        {
                                            Console.Write("ID no válido. Inténtelo de nuevo: ");
                                        }
                                        
                                        Clientes.Eliminar(id_Eliminar);
                                        break;

                                    case 4:
                                        Console.Clear();
                                        Clientes.MostrarTodos();
                                        break;

                                    case 5:
                                        Console.Clear();
                                        Console.Write("ID del cliente para mostrar sus datos: ");
                                        int id_MostrarClientePorId;
                                        while (!int.TryParse(Console.ReadLine(), out id_MostrarClientePorId))
                                        {
                                            Console.Write("ID no válido. Inténtelo de nuevo: ");
                                        }
                                        
                                        Clientes.BuscarClientePorId(id_MostrarClientePorId);
                                        break;

                                    case 6:
                                        Console.Clear();
                                        Console.Write("ID del cliente para mostrar sus facturas: ");
                                        int id_MostrarFacturasCliente;
                                        while (!int.TryParse(Console.ReadLine(), out id_MostrarFacturasCliente))
                                        {
                                            Console.Write("ID no válido. Inténtelo de nuevo: ");
                                        }
                                        
                                        Clientes.FacturasAsociadasAClientePorId(id_MostrarFacturasCliente);
                                        break;

                                    case 0:
                                        salir_cliente = true;
                                        break;
                                }
                            }
                            if (!salir_cliente) Console.Clear();
                        }
                        break;

                    case 2:
                        Console.Clear();
                        bool salir_productos = false;
                        while (!salir_productos)
                        {
                            Menu.Menu_Productos();
                            string entrada_productos = Console.ReadLine();
                            if (int.TryParse(entrada_productos, out int opcion_producto))
                            {
                                switch (opcion_producto)
                                {
                                    case 1:
                                        Console.Clear();
                                        Console.Write("Nombre del producto: ");
                                        string nombre_insertar_p = Console.ReadLine();
                                        Console.Write("Precio unitario: ");
                                        double precio_insertar_p;
                                        while (!double.TryParse(Console.ReadLine(), out precio_insertar_p))
                                        {
                                            Console.Write("Precio no válido. Inténtelo de nuevo: ");
                                        }

                                        Productos.Insertar(nombre_insertar_p, precio_insertar_p);
                                        break;

                                    case 2:
                                        Console.Clear();
                                        Console.Write("Columna (nombre_producto, precio_unitario): ");
                                        string columna_modificar_p = Console.ReadLine();
                                        Console.Write("Valor nuevo: ");
                                        string valor_modificar_p = Console.ReadLine();
                                        Console.Write("ID del producto a modificar: ");
                                        int id_modificar_p;
                                        while (!int.TryParse(Console.ReadLine(), out id_modificar_p))
                                        {
                                            Console.Write("ID no válido. Inténtelo de nuevo: ");
                                        }

                                        Productos.Modificar(columna_modificar_p, valor_modificar_p, id_modificar_p);
                                        break;

                                    case 3:
                                        Console.Clear();
                                        int id_eliminar_p;
                                        Console.Write("ID del producto a eliminar: ");
                                        while (!int.TryParse(Console.ReadLine(), out id_eliminar_p))
                                        {
                                            Console.Write("ID no válido. Inténtelo de nuevo: ");
                                        }

                                        Productos.Eliminar(id_eliminar_p);
                                        break;

                                    case 4:
                                        Console.Clear();
                                        Productos.MostrarTodos();
                                        break;

                                    case 5:
                                        Console.Clear();
                                        int id_buscar_p;
                                        Console.Write("ID del producto a buscar: ");
                                        while (!int.TryParse(Console.ReadLine(), out id_buscar_p))
                                        {
                                            Console.Write("ID no válido. Inténtelo de nuevo: ");
                                        }

                                        Productos.BuscarProductoPorId(id_buscar_p);
                                        break;

                                    case 6:
                                        Console.Clear();
                                        int id_vendidos_p;
                                        Console.Write("ID del producto para ver unidades vendidas: ");
                                        while (!int.TryParse(Console.ReadLine(), out id_vendidos_p))
                                        {
                                            Console.Write("ID no válido. Inténtelo de nuevo: ");
                                        }

                                        Productos.TotalArticulosVendidosPorCodigo(id_vendidos_p);
                                        break;

                                    case 0:
                                        salir_productos = true;
                                        break;
                                }
                            }
                        }
                        break;

                    case 3:
                        Console.Clear();
                        bool salir_facturas = false;
                        while (!salir_facturas)
                        {
                            Menu.Menu_Facturas();
                            string entrada_facturas = Console.ReadLine();
                            if (int.TryParse(entrada_facturas, out int opcion_facturas))
                            {
                                switch (opcion_facturas)
                                {
                                    case 1:
                                        Console.Clear();
                                        Console.Write("ID del cliente: ");
                                        int id_cliente;
                                        while (!int.TryParse(Console.ReadLine(), out id_cliente))
                                        {
                                            Console.Write("ID de cliente no válido. Inténtelo de nuevo: ");
                                        }

                                        Console.Write("Fecha de la factura (yyyy-MM-dd) [Enter para hoy]: ");
                                        string fecha_str = Console.ReadLine().Trim();
                                        if (string.IsNullOrWhiteSpace(fecha_str))
                                            fecha_str = DateTime.Now.ToString("yyyy-MM-dd");

                                        if (!DateTime.TryParse(fecha_str, out DateTime fechaFactura))
                                            fechaFactura = DateTime.Now;

                                        try
                                        {
                                            using (var conexion = Conexion.Conectar())
                                            {
                                                string sqlInsert =
                                                    "INSERT INTO facturas (id_cliente, fecha) VALUES (@id_cliente, @fecha)";
                                                using (var cmd = new SqliteCommand(sqlInsert, conexion))
                                                {
                                                    cmd.Parameters.AddWithValue("@id_cliente", id_cliente);
                                                    cmd.Parameters.AddWithValue("@fecha",
                                                        fechaFactura.ToString("yyyy-MM-dd"));
                                                    cmd.ExecuteNonQuery();
                                                }

                                                int id_factura;
                                                using (var cmdId = new SqliteCommand("SELECT last_insert_rowid();",
                                                           conexion))
                                                {
                                                    id_factura = Convert.ToInt32(cmdId.ExecuteScalar());
                                                }

                                                string codigo_factura = $"{id_factura}/{fechaFactura.Year}";
                                                string sqlUpdate =
                                                    "UPDATE facturas SET codigo_factura = @codigo WHERE id_factura = @id_factura";
                                                using (var cmdUpdate = new SqliteCommand(sqlUpdate, conexion))
                                                {
                                                    cmdUpdate.Parameters.AddWithValue("@codigo", codigo_factura);
                                                    cmdUpdate.Parameters.AddWithValue("@id_factura", id_factura);
                                                    cmdUpdate.ExecuteNonQuery();
                                                }

                                                Console.WriteLine(
                                                    $"Factura creada correctamente. ID: {id_factura} - Código: {codigo_factura}");
                                            }
                                        }
                                        catch (SqliteException ex)
                                        {
                                            Console.WriteLine($"Error de Sqlite: {ex.Message}");
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine($"Error: {ex.Message}");
                                        }
                                        Console.ReadKey();
                                        break;

                                    case 2: 
                                        bool salirModificar = false;
                                        while (!salirModificar)
                                        {
                                            Menu.Menu_FacturasModificar();
                                            if (int.TryParse(Console.ReadLine(), out int opcionModificar))
                                            {
                                                if (opcionModificar == 1) Facturas.ModificarLineaFactura();
                                                else if (opcionModificar == 2) Facturas.ModificarCabeceraFactura();
                                                else if (opcionModificar == 0) salirModificar = true;
                                            }
                                            Console.Clear();
                                        }
                                        break;

                                    case 3:
                                        Console.Clear();
                                        Console.Write("ID de la factura a eliminar: ");
                                        int idEliminar;
                                        while (!int.TryParse(Console.ReadLine(), out idEliminar))
                                        {
                                            Console.Write("ID no válido. Inténtelo de nuevo: ");
                                        }
                                        Facturas.Eliminar(idEliminar);
                                        break;

                                    case 4:
                                        Console.Clear();
                                        Console.Write("ID de la factura a visualizar: ");
                                        int idFactVisualizar;
                                        while (!int.TryParse(Console.ReadLine(), out idFactVisualizar))
                                        {
                                            Console.Write("ID no válido. Inténtelo de nuevo: ");
                                        }
                                        Facturas.VisualizarFactura(idFactVisualizar);
                                        break;

                                    case 0:
                                        salir_facturas = true;
                                        break;
                                }
                            }
                        }
                        break;

                    case 0:
                        salir_principal = true;
                        break;
                }
            }
            Console.Clear();
        }
    }
}