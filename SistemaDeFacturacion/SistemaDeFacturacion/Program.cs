namespace SistemaDeFacturacion;

class Program
{
    static void Main(string[] args)
    {
        string entrada;
        bool salir_principal = false;

        while (!salir_principal)
        {
            Menu.Menu_Principal();
            Console.SetCursorPosition(8, 10);
            entrada = Console.ReadLine();

            if (int.TryParse(entrada, out int opcion))
            {
                switch (opcion)
                {
                    case 1:
                        Console.Clear();
                        string entrada_cliente;
                        bool salir_cliente = false;

                        while (!salir_cliente)
                        {
                            Menu.Menu_Cliente();
                            Console.SetCursorPosition(8, 11);
                            entrada_cliente = Console.ReadLine();

                            if (int.TryParse(entrada_cliente, out int opcion_cliente))
                            {
                                switch (opcion_cliente)
                                {
                                    case 1:
                                        Console.Clear();
                                        Clientes clienteInsertar = new Clientes();

                                        string nombre_insertar;
                                        string apellidos_insertar;
                                        string direccion_insertar;
                                        int? telefono_insertar;
                                        string mail_insertar;

                                        Console.Write("Nombre del cliente: ");
                                        nombre_insertar = Console.ReadLine();
                                        Console.Write("Apellidos del cliente: ");
                                        apellidos_insertar = Console.ReadLine();
                                        Console.Write("Direccion del cliente: ");
                                        direccion_insertar = Console.ReadLine();
                                        Console.Write("Telefono del cliente: ");
                                        string telefono_texto = Console.ReadLine();
                                        telefono_insertar = string.IsNullOrWhiteSpace(telefono_texto) ? null : int.Parse(telefono_texto);
                                        Console.Write("Mail del cliente: ");
                                        mail_insertar = Console.ReadLine();

                                        Clientes.Insertar(nombre_insertar, apellidos_insertar, direccion_insertar, telefono_insertar, mail_insertar);
                                        break;
                                    case 2:
                                        Console.Clear();
                                        Clientes clienteModificar = new Clientes();

                                        string columna_modificar, valor_modificar;
                                        int id_modificar;

                                        Console.Write("Columna (nombre, apellidos, direccion, telefono, mail): ");
                                        columna_modificar = Console.ReadLine();
                                        Console.Write("Valor nuevo: ");
                                        valor_modificar = Console.ReadLine();
                                        Console.Write("Donde la ID sea: ");
                                        id_modificar = Convert.ToInt32(Console.ReadLine());

                                        Clientes.Modificar(columna_modificar, valor_modificar, id_modificar);
                                        break;
                                    case 3:
                                        Console.Clear();
                                        Clientes clienteEliminar = new Clientes();
                                        int id_Eliminar;
                                        
                                        Console.Write("ID del cliente que desea eliminar: ");
                                        id_Eliminar = Convert.ToInt32(Console.ReadLine());
                                        
                                        Clientes.Eliminar(id_Eliminar);
                                        break;
                                    case 4:
                                        Console.Clear();
                                        Clientes.MostrarTodos();
                                        break;
                                    case 5:
                                        
                                        break;
                                    case 6:
                                        
                                        break;
                                    case 0:
                                        salir_cliente = true;
                                        break;
                                }
                            }
                            Console.Clear();
                        }
                        break;
                    case 2:
                        Console.Clear();
                        string entrada_productos;
                        bool salir_productos = false;
                        while (!salir_productos)
                        {
                            Menu.Menu_Productos();
                            Console.SetCursorPosition(8, 11);
                            entrada_productos = Console.ReadLine();
                            if (int.TryParse(entrada_productos, out int opcion_producto))
                            {
                                switch (opcion_producto)
                                {
                                    case 1:
                                        break;
                                    case 2:
                                        break;
                                    case 3:
                                        break;
                                    case 4:
                                        break;
                                    case 5:
                                        break;
                                    case 6:
                                        break;
                                    case 0:
                                        salir_productos = true;
                                        break;
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Opción inválida. Por favor, escriba un número válido del menú. \n");
                                Console.WriteLine("Presione cualquier tecla para volver a intentar.");
                                Console.ReadKey(true);
                            }
                        }
                        break;
                    case 3:
                        break;
                    case 0:
                        salir_principal = true;
                        break;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Opción inválida. Por favor, escriba un número válido del menú. \n");
                Console.WriteLine("Presione cualquier tecla para volver a intentar.");
                Console.ReadKey(true);
            }
            Console.Clear();
        }
    }
}