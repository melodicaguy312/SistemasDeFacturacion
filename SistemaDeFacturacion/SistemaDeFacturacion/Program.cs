namespace SistemaDeFacturacion;

class Program
{
    static void Main(string[] args)
    {
        string entrada;
        Menu.Menu_Principal();
        Console.SetCursorPosition(8, 10);
        entrada = Console.ReadLine();
        if (int.TryParse(entrada, out int opcion))
        {
            switch (opcion)
            {
                case 1:
                    // hacer while
                    Console.Clear();
                    string entrada_cliente;
                    Menu.Menu_Cliente();
                    Console.SetCursorPosition(8, 6);
                    entrada_cliente = Console.ReadLine();
                    if (int.TryParse(entrada_cliente, out int opcion_cliente))
                    {
                        // while
                        switch (opcion_cliente)
                        {
                            case 1:
                                Console.Clear();
                                Clientes clienteInsertar = new Clientes();
                                
                                string nombre_cliente;
                                string apellidos_cliente;
                                string direccion_cliente;
                                int telefono_cliente;
                                string mail;
                                
                                Console.Write("Nombre del cliente: ");
                                nombre_cliente = Console.ReadLine();
                                Console.Write("Apellidos del cliente: ");
                                apellidos_cliente = Console.ReadLine();
                                Console.Write("Direccion del cliente: ");
                                direccion_cliente = Console.ReadLine();
                                Console.Write("Telefono del cliente: ");
                                telefono_cliente = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Mail del cliente: ");
                                mail = Console.ReadLine();

                                Clientes.Insertar(nombre_cliente, apellidos_cliente, direccion_cliente, telefono_cliente, mail);
                                break;
                            case 2:
                                Console.Clear();
                                Clientes clienteModificar = new Clientes();

                                string columna, valor;
                                int id;
                                
                                Console.Write("Columna que quiere cambiar (nombre, apellidos, direccion, telefono, mail): ");
                                columna = Console.ReadLine();
                                Console.Write("Valor nuevo: ");
                                valor = Console.ReadLine();
                                Console.Write("Donde la ID sea: ");
                                id = Convert.ToInt32(Console.ReadLine());
                                
                                Clientes.Modificar(columna, valor, id);
                                break;
                            case 3:
                                // Eliminar
                                break;
                            case 4:
                                // Listado
                                Console.Clear();
                                Clientes.MostrarTodos();
                                break;
                            case 5:
                                // Buscar cliente por ID
                                break;
                            case 6:
                                // Facturas de cliente por ID del cliente
                                break;
                            case 0:
                                // Volver al menú principal
                                break;
                        }
                    }
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 0:
                    break;
            }
        }
    }
}