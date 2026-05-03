using Microsoft.Data.Sqlite;
namespace SistemaDeFacturacion;

public class Facturas
{
    public static void Insertar(string codigo_factura, string fecha)
    {
        using (var conexion = Conexion.Conectar())
        {
            string cadenaSQL = "INSERT into facturas(codigo_factura, fecha) VALUES(@codigo_factura, @fecha)";
            using (var comando = new SqliteCommand(cadenaSQL, conexion))
            {
                if (string.IsNullOrWhiteSpace(codigo_factura))
                {
                    comando.Parameters.AddWithValue("@codigo_factura", DBNull.Value);
                }
                else
                {
                    comando.Parameters.AddWithValue("@codigo_factura", codigo_factura);
                }

                if (string.IsNullOrWhiteSpace(fecha))
                {
                    comando.Parameters.AddWithValue("@fecha", DBNull.Value);
                }
                else
                {
                    comando.Parameters.AddWithValue("@fecha", fecha);
                }
                
                comando.ExecuteNonQuery();
            }
            Console.WriteLine();
            Console.WriteLine("Factura añadida correctamente.");
            Console.ReadKey();
        }
    }

    public static void ModificarCabeceraFactura()
    {
        Console.Write("Introduce ID de factura: ");
        if (!int.TryParse(Console.ReadLine(), out int id_factura))
        {
            Console.WriteLine("ID no válido.");
            Console.ReadKey();
            return;
        }
 
        using (var conexion = Conexion.Conectar())
        {
            int id_cliente_actual = 0;
            string fecha_actual = "";
            string sqlActual = "SELECT id_cliente, fecha FROM facturas WHERE id_factura = @id";
            using (var comando = new SqliteCommand(sqlActual, conexion))
            {
                comando.Parameters.AddWithValue("@id", id_factura);
                using (var reader = comando.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        Console.WriteLine($"No existe ninguna factura con ID {id_factura}.");
                        Console.ReadKey();
                        return;
                    }
                    id_cliente_actual = reader.GetInt32(0);
                    fecha_actual = reader.GetString(1);
                }
            }
            
            string nombre_cliente = "";
            string sqlCliente = "SELECT nombre, apellidos FROM clientes WHERE id = @id";
            using (var comando = new SqliteCommand(sqlCliente, conexion))
            {
                comando.Parameters.AddWithValue("@id", id_cliente_actual);
                using (var reader = comando.ExecuteReader())
                {
                    if (reader.Read()) nombre_cliente = $"{reader.GetString(0)}";
                }
            }
 
            Console.WriteLine("Si no introduce datos, se deja el valor actual.");
 
            Console.Write($"ID Cliente (ID={id_cliente_actual} {nombre_cliente}): ");
            string entradaCliente = Console.ReadLine();
            int nuevo_id_cliente;
            if (string.IsNullOrWhiteSpace(entradaCliente))
            {
                nuevo_id_cliente = id_cliente_actual;
            }
            else
            {
                nuevo_id_cliente = int.Parse(entradaCliente);
                
            }
 
            Console.Write($"Fecha factura ({fecha_actual}): ");
            string entradaFecha = Console.ReadLine();
            string nueva_fecha;
            if (string.IsNullOrWhiteSpace(entradaFecha))
            {
                nueva_fecha = fecha_actual;
            }
            else
            {
                nueva_fecha = entradaFecha;
            }
            DateTime fecha = DateTime.Parse(nueva_fecha);
            string nuevo_codigo = $"{id_factura}/{fecha.Year}";
 
            string cadenaSQL = @"UPDATE facturas SET codigo_factura = @cod, fecha = @fecha, id_cliente = @idCliente WHERE id_factura = @idFactura";
            using (var comando = new SqliteCommand(cadenaSQL, conexion))
            {
                comando.Parameters.AddWithValue("@cod", nuevo_codigo);
                comando.Parameters.AddWithValue("@fecha", nueva_fecha);
                comando.Parameters.AddWithValue("@idCliente", nuevo_id_cliente);
                comando.Parameters.AddWithValue("@idFactura", id_factura);
                int filas = comando.ExecuteNonQuery();
                Console.WriteLine();
                if (filas > 0)
                {
                    Console.WriteLine($"Factura modificada. El nuevo código de factura es {nuevo_codigo}");
                }
                else
                {
                    Console.WriteLine("No han habido cambios.");
                }
            }
            Console.ReadKey();
        }
    }

    public static void ModificarLineaFactura()
    {
        Console.Write("Introduce ID de factura: ");
        if (!int.TryParse(Console.ReadLine(), out int id_factura))
        {
            Console.WriteLine("ID no válido.");
            Console.ReadKey();
            return;
        }
 
        Console.Write("Introduce número de línea: ");
        if (!int.TryParse(Console.ReadLine(), out int id_linea))
        {
            Console.WriteLine("Número de línea no válido.");
            Console.ReadKey();
            return;
        }
 
        using (var conexion = Conexion.Conectar())
        {
            int cod_prod_actual = 0;
            int cantidad_actual = 0;
            string sqlActual = "SELECT codigo_producto, cantidad FROM lineas_factura WHERE id_factura = @id_fact AND id_linea = @id_linea";
            using (var comando = new SqliteCommand(sqlActual, conexion))
            {
                comando.Parameters.AddWithValue("@id_fact", id_factura);
                comando.Parameters.AddWithValue("@id_linea", id_linea);
                using (var reader = comando.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        Console.WriteLine($"No existe la línea {id_linea} en la factura {id_factura}.");
                        Console.ReadKey();
                        return;
                    }
                    cod_prod_actual = reader.GetInt32(0);
                    cantidad_actual = reader.GetInt32(1);
                }
            }
            
            string nombre_prod = "";
            string sqlProd = "SELECT nombre_producto FROM productos WHERE codigo_producto = @id";
            using (var comando = new SqliteCommand(sqlProd, conexion))
            {
                comando.Parameters.AddWithValue("@id", cod_prod_actual);
                var scalar = comando.ExecuteScalar();
                if (scalar != null) nombre_prod = scalar.ToString();
            }
 
            Console.WriteLine("Si no introduce datos, se deja el valor actual.");
 
            Console.Write($"ID Artículo (ID={cod_prod_actual} {nombre_prod}): ");
            string entradaCod = Console.ReadLine();
            int nuevo_cod;
            if (string.IsNullOrWhiteSpace(entradaCod))
            {
                nuevo_cod = cod_prod_actual;
            }
            else
            {
                nuevo_cod = int.Parse(entradaCod);
            }
 
            Console.Write($"Cantidad ({cantidad_actual}): ");
            string entradaCant = Console.ReadLine();
            int nueva_cantidad;
            if (string.IsNullOrWhiteSpace(entradaCant))
            {
                nueva_cantidad =  cantidad_actual;
            }
            else
            {
                nueva_cantidad = int.Parse(entradaCant);
            }
            
            double nuevo_precio_u = 0;
            string sqlPrecio = "SELECT precio_unitario FROM productos WHERE codigo_producto = @id";
            using (var comando = new SqliteCommand(sqlPrecio, conexion))
            {
                comando.Parameters.AddWithValue("@id", nuevo_cod);
                var scalar = comando.ExecuteScalar();
                if (scalar != null) nuevo_precio_u = Convert.ToDouble(scalar);
            }
            double nuevo_precio_t = nuevo_precio_u * nueva_cantidad;
 
            string sqlUpdate = @"UPDATE lineas_factura SET codigo_producto = @cod, cantidad = @cant, precio_unitario = @pu, precio_total = @pt WHERE id_factura = @id_fact AND id_linea = @id_linea";
            using (var comando = new SqliteCommand(sqlUpdate, conexion))
            {
                comando.Parameters.AddWithValue("@cod", nuevo_cod);
                comando.Parameters.AddWithValue("@cant", nueva_cantidad);
                comando.Parameters.AddWithValue("@pu", nuevo_precio_u);
                comando.Parameters.AddWithValue("@pt", nuevo_precio_t);
                comando.Parameters.AddWithValue("@id_fact", id_factura);
                comando.Parameters.AddWithValue("@id_linea", id_linea);
                int filas = comando.ExecuteNonQuery();
                Console.WriteLine();
                Console.WriteLine(filas > 0 ? "Línea modificada correctamente." : "No se pudo modificar la línea.");
            }
            Console.ReadKey();
        }
    }

   public static void VisualizarFactura(int id_factura)
{
    using (var conexion = Conexion.Conectar())
    {
        // === CABECERA + CLIENTE ===
        string sqlFactura = @"SELECT f.id_factura, f.codigo_factura, f.fecha, 
                                     c.nombre, c.apellidos, c.direccion, c.telefono, c.mail 
                              FROM facturas f 
                              LEFT JOIN clientes c ON f.id_cliente = c.id 
                              WHERE f.id_factura = @id";

        string codigo_factura = "", fecha = "", nombre = "", apellidos = "", direccion = "", telefono = "", mail = "";

        using (var comando = new SqliteCommand(sqlFactura, conexion))
        {
            comando.Parameters.AddWithValue("@id", id_factura);
            using (var reader = comando.ExecuteReader())
            {
                if (!reader.Read())
                {
                    Console.WriteLine($"No se encontró la factura con ID {id_factura}.");
                    Console.ReadKey();
                    return;
                }
                codigo_factura = reader.GetString(1);
                fecha          = reader.IsDBNull(2) ? "" : reader.GetString(2);
                nombre         = reader.GetString(3);
                apellidos      = reader.GetString(4);
                direccion      = reader.GetString(5);
                telefono       = reader.IsDBNull(6) ? "" : reader.GetString(6);
                mail           = reader.GetString(7);
            }
        }

        // === MOSTRAR CABECERA ===
        Console.WriteLine();
        Console.WriteLine($"Empresa: {Configuracion.Ultima.NombreEmpresa}");
        Console.WriteLine($"CIF:     {Configuracion.Ultima.CIF}");
        Console.WriteLine();
        Console.WriteLine($"Código Factura: {codigo_factura}");
        Console.WriteLine($"Fecha Factura:  {fecha}");
        Console.WriteLine($"Cliente:        {nombre} {apellidos}");
        Console.WriteLine($"Dirección:      {direccion}");
        Console.WriteLine($"Teléfono:       {telefono}");
        Console.WriteLine($"Mail:           {mail}");
        Console.WriteLine();

        // === LÍNEAS DE FACTURA ===
        string sqlLineas = @"SELECT lf.id_linea, p.codigo_producto, p.nombre_producto, 
                                    lf.precio_unitario, lf.cantidad, lf.precio_total 
                             FROM lineas_factura lf 
                             JOIN productos p ON lf.codigo_producto = p.codigo_producto 
                             WHERE lf.id_factura = @id 
                             ORDER BY lf.id_linea";

        double baseImponible = 0;
        bool hayLineas = false;

        Console.WriteLine($"{"Nº",-4} {"Cód.",-6} {"Artículo",-25} {"P. Unit.",-10} {"Cant.",-6} {"Precio",10}");
        Console.WriteLine(new string('-', 65));

        using (var comando = new SqliteCommand(sqlLineas, conexion))
        {
            comando.Parameters.AddWithValue("@id", id_factura);
            using (var reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    hayLineas = true;
                    int id_linea   = reader.GetInt32(0);
                    int cod_prod   = reader.GetInt32(1);
                    string nom_prod = reader.GetString(2);
                    double p_unit  = reader.GetDouble(3);
                    int cant       = reader.GetInt32(4);
                    double p_total = reader.GetDouble(5);

                    baseImponible += p_total;

                    Console.WriteLine($"{id_linea,-4} {cod_prod,-6} {nom_prod,-25} {p_unit,-10:F2} {cant,-6} {p_total,10:F2}");
                }
            }
        }

        if (!hayLineas)
        {
            Console.WriteLine("Esta factura todavía no tiene líneas de factura.");
        }
        else
        {
            double iva   = baseImponible * (Configuracion.Ultima.IVA / 100.0);
            double total = baseImponible + iva;

            Console.WriteLine(new string('-', 65));
            Console.WriteLine();
            Console.WriteLine($"{"Base Imponible:",-51} {baseImponible,14:F2} €");
            Console.WriteLine($"{"IVA (" + Configuracion.Ultima.IVA + "%):",-51} {iva,14:F2} €");
            Console.WriteLine($"{"Total:",-51} {total,14:F2} €");
        }

        Console.ReadKey();
    }
}

    public static void Eliminar(int id_factura)
    {
        using (var conexion = Conexion.Conectar())
        {
            string sqlExiste = "SELECT COUNT(*) FROM facturas WHERE id_factura = @id";
            using (var comando = new SqliteCommand(sqlExiste, conexion))
            {
                comando.Parameters.AddWithValue("@id", id_factura);
                if (Convert.ToInt32(comando.ExecuteScalar()) == 0)
                {
                    Console.WriteLine($"No existe ninguna factura con ID {id_factura}.");
                    Console.ReadKey();
                    return;
                }
            }
            
            int totalLineas = 0;
            string sqlLineas = "SELECT COUNT(*) FROM lineas_factura WHERE id_factura = @id";
            using (var comando = new SqliteCommand(sqlLineas, conexion))
            {
                comando.Parameters.AddWithValue("@id", id_factura);
                totalLineas = Convert.ToInt32(comando.ExecuteScalar());
            }
 
            Console.WriteLine();
            Console.WriteLine($"La factura {id_factura} tiene {totalLineas} líneas asociadas.");
            Console.Write("¿Está seguro de que desea eliminarla junto con todas sus líneas? (s/n): ");
            string respuesta = Console.ReadLine().ToLower();
 
            if (respuesta != "s")
            {
                Console.WriteLine("Cancelado.");
                Console.ReadKey();
                return;
            }
            
            string sqlBorrarLineas = "DELETE FROM lineas_factura WHERE id_factura = @id";
            using (var comando = new SqliteCommand(sqlBorrarLineas, conexion))
            {
                comando.Parameters.AddWithValue("@id", id_factura);
                comando.ExecuteNonQuery();
            }
            
            string sqlBorrarFactura = "DELETE FROM facturas WHERE id_factura = @id";
            using (var comando = new SqliteCommand(sqlBorrarFactura, conexion))
            {
                comando.Parameters.AddWithValue("@id", id_factura);
                int filas = comando.ExecuteNonQuery();
                Console.WriteLine();
                Console.WriteLine(filas > 0 ? $"Factura eliminada. Se eliminaron {totalLineas} líneas asociadas." : "No se pudo eliminar la factura.");
            }
            Console.ReadKey();
        }
    }
}