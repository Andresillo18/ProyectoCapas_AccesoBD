using CapaEntidades;

using System;
using System.Data.SqlClient; // UTILIZAR ADO .NET
using System.Data; // Se utiliza para el acceso a datos
using System.Linq;
using System.Collections.Generic;

namespace CapaAccesoDatos
{
    public class DACliente
    {
        #region Atributos

        private string _cadenaConexion;
        private string _mensaje;

        #endregion

        #region Propiedades

        public string CadenaConexion { set => _cadenaConexion = value; }
        public string Mensaje { get => _mensaje; }

        #endregion

        #region Constructor

        public DACliente()
        {
            _cadenaConexion = string.Empty;
            _mensaje = string.Empty;
        }

        public DACliente(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
            _mensaje = string.Empty;
        }
        #endregion

        #region Metodo insertar
        // Insertar un cliente en la base de datos
        //devuelve un int, que es el ID generado por el IDENTITY
        public int Insertar(EntidadCliente cliente)
        {
            // Se usa la conexión usada para la base de datos en la instancia
            SqlConnection conexion = new SqlConnection(_cadenaConexion);

            SqlCommand comando = new SqlCommand();

            int id = 0; // retornará el IDENTITY de la BD del registro insertado

            //Se le debe asignar lo que será añadido o modificado en la base de datos
            //Se debe hacer como si fuera en la misma, se puede hacer todo junto sin concatenar
            string sentencia =
               "INSERT INTO CLIENTES(NOMBRE, TELEFONO, DIRECCION) " +
               "VALUES (@nombre, @telefono, @direccion) " +
               "SELECT @@IDENTITY";

            // Al atributo del objeto 'comando' se le establece lo que devolvio la instancia de conexion
            comando.Connection = conexion;

            // El segundo parámetro recibe los datos ingresado y lo guarda en el campo @...
            comando.Parameters.AddWithValue("@nombre", cliente.Nombre);
            comando.Parameters.AddWithValue("@telefono", cliente.Telefono);
            comando.Parameters.AddWithValue("@direccion", cliente.Direccion);

            // La sentencia a alterar se guarda en el CommandText porque es la sentencia dado por el provedor
            comando.CommandText = sentencia;

            //Hasta este punto el objeto SqlCommnado (comando) ya está prepadado para ejecutar la instrucción SQL

            // Ejecutar la consulta SQL:
            try
            {
                conexion.Open();
                id = Convert.ToInt32(comando.ExecuteScalar()); // ExecuteScalar devuelve un valor fijo o el IDENTITY 
                conexion.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                // Dispose means throw away
                conexion.Dispose();
                conexion.Dispose();
            }

            return id;
        }

        #endregion

        #region Método Modificar

        public int Modificar(EntidadCliente cliente)
        {
            // Devuelve un -1 sino se pudo lograr
            int filasAfectadas = -1;

            SqlConnection conexion = new SqlConnection();

            SqlCommand comando = new SqlCommand();

            string sentencia = "UPDATE CLIENTES" +
                "SET NOMBRE = @nombre, " +
                "TELEFONO= @telefono, " +
                "DIRECCION = @direccion " +
                "WHERE ID_CLIENTE = @id_cliente";

            comando.CommandText = sentencia;
            comando.Connection = conexion;

            comando.Parameters.AddWithValue("@id_cliente", cliente.Id_cliente);
            comando.Parameters.AddWithValue("@nombre", cliente.Nombre);
            comando.Parameters.AddWithValue("@direccion", cliente.Direccion);
            comando.Parameters.AddWithValue("@telefono", cliente.Telefono);

            try
            {
                conexion.Open();
                filasAfectadas = comando.ExecuteNonQuery(); // Devuelve el número de fija afectadas
                conexion.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //Se desase de las conexiones
                conexion.Dispose();
                comando.Dispose();
            }

            return filasAfectadas;
        }

        #endregion

        #region Método ListasClientes(string condicion, string orden)

        // Devuelve un DataSet con los clientes, para mostrarlo en el DataGridView
        //DataSet: es una representación en memoria de una BD (contiene la base datos), puede contener uno o más objetos DataTable,
        //así como información sobre claves principales, externas, restricciones.
        public DataSet ListarClientes(string condicion, string orden)
        {
            DataSet tablaDataSet = new DataSet();

            /*Vamos a hacer una consulta
             -La consulta nos devolverá una tabla (la tabla Clientes)
            -La tabla la vamos a almacenar en ese DataSet, solo tiene una tabla
             */

            SqlConnection conexion = new SqlConnection(_cadenaConexion);
            SqlDataAdapter adapter;

            /*SqlDataAdapter: Representa un conjunto de comandos de datos y una conexión de base de datos que se utilizan para:
            - Rellenar un DataSet y
            - actualizar una base de datos de SQL Server.

            Esta clase no se puede heredar. SqlDataAdapter, Se utiliza como un puente entre DataSet y SQL Server para recuperar y guardar datos. SqlDataAdapter proporciona este puente mediante la asignación de Fill, que cambia los datos en DataSet para que coincidan con los datos del origen de datos; y Update, que cambia los datos en el origen de datos para que coincidan con los datos en DataSet utilizando las instrucciones de Transact-SQL en el origen de datos adecuado.*/

            string sentencia = "SELECT ID_CLIENTE, NOMBRE, TELEFONO, DIRECCION FROM CLIENTES"; // *Ó SELECT *
            //**** LA COMA***

            if (!String.IsNullOrEmpty(condicion))
            {
                sentencia = String.Format("{0} WHERE {1}", sentencia, condicion); // Remplaza los parámetros de las ""; el string.format funciona para concatener
                //sentencia = sentencia + "WHERE" + condicion;
            }

            if (!String.IsNullOrEmpty(orden))
            {
                sentencia = String.Format("{0} ORDER BY {1}", sentencia, orden);
            }

            try
            {
                //No se ocupa abrir abrir la conexión a la base de datos cuando solo se ocupa leer***
                
                adapter = new SqlDataAdapter(sentencia, conexion);

                //Llene el DataSet y además le asigne un nombre a esa tabla del DataSet
                adapter.Fill(tablaDataSet, "Clientes");
            }
            catch (Exception)
            {
                throw;
            }

            return tablaDataSet;
        }

        #endregion

        #region Método ListarClientes(String condicion)

        //Este método va a devolver una LISTA de clientes (no un DataSet como el anterior), es más dinamico que el Array

        public List<EntidadCliente> ListarClientes(String condicion)
        {
            DataSet TablaDataSet = new DataSet();

            SqlConnection conexion = new SqlConnection(_cadenaConexion);
            SqlDataAdapter adapter; // Es el puente entre el DataSet (tabla aquí) y la Base de Datos

            List<EntidadCliente> listaDeClientes;

            string sentencia = "SELECT * FROM CLIENTES";

            if (!String.IsNullOrEmpty(condicion))
            {
                sentencia = String.Format("{0} WHERE {1}", sentencia, condicion); 
            }

            try
            {
                adapter = new SqlDataAdapter(sentencia, conexion);
                adapter.Fill(TablaDataSet, "Clientes");
                //Se llena el DataSet con la de la BD

                //***Sentencia linQ para convertir el DataSet en una lista 
                listaDeClientes = (from DataRow fila in TablaDataSet.Tables["Clientes"].Rows
                                   select new EntidadCliente()
                                   {
                                       Id_cliente = (int)fila[0],
                                       Nombre = fila[1].ToString(),
                                       Telefono = fila[2].ToString(),
                                       Existe = true
                                   }).ToList();
                //Lo anterior convierte el DataSet en una lista, y cada elemento de la lista tiene Objeto EntidadCliente que 
                //representa cada fila de la tabla de la BD;
            }
            catch (Exception)
            {

                throw;
            }

            return listaDeClientes;
        }

        #endregion

        #region Método ObtenerCliente

        //Retorna un cliente (en una entidad cliente)
        public EntidadCliente ObtenerCliente(int id)
        {
            EntidadCliente cliente = null;
            SqlConnection conexion = new SqlConnection(_cadenaConexion);
            SqlCommand comando = new SqlCommand();

            SqlDataReader dataReader;
            //El dataReader no tiene constructor
            // Para llenarlo se hace mediante un EXECUTE
            /*
            Los objetos DataReader permiten obtener datos de la base de datos y cargarlos en memoria local, por lo que sólo son datos de lectura, necesitan tabajar sobre una conexión abierta y ejecutarse a traves de un comando del metodo ExecuteReader.
              */

            string sentencia = string.Format("SELECT ID_CLIENTE, NOMBRE, TELEFONO, DIRECCION FROM CLIENTES WHERE ID_CLIENTE = {0}", id);
            //si el id fuera texto se debe poner el 0 entre comillas asi '{0}' dentro del string

            // Aquí no se usa DataSet porque el DataSet solo guarda la tabla, la entidad guardar un solo registro***
            comando.Connection = conexion; // Al crearlo se le envio la conexion y comando.Connection tiene la conexion realizada

            comando.CommandText = sentencia;

            try
            {
                conexion.Open();
                // Lo que hace ese método de comando es crear el dataReader con lo que se tiene arriba
                dataReader = comando.ExecuteReader();

                if (dataReader.HasRows)
                    //si obtuvo algo de la BD (algún registro) construimos una Entidad con esos datos
                {
                    cliente = new EntidadCliente();
                    dataReader.Read();
                    //Lee fila por fila del DataReader, siempre hacia adelante

                    /* 
                    Si uno quisiera con el método .Read() hacer un CICLO y leer todas las filas
                     */
                    /*
                    while (dataReader.HasRows)
                    {
                    // instrucciones, que vaya leyendo cada fila
                    // ...
                    }
                    */

                    cliente.Id_cliente = dataReader.GetInt32(0);
                    //Esta columna es de tipo INT y está en la posición 0 (primera columna)
                    cliente.Nombre = dataReader.GetString(1);
                    cliente.Telefono= dataReader.GetString(2);
                    cliente.Direccion = dataReader.GetString(3);
                    cliente.Existe = true;
                }

                conexion.Close();
            }
            catch (Exception)
            {
                throw;
            }

            return cliente;
            //Si no existía el ID en la BD, retorna un cliente que contiene NULL
            //En caso que si existia retorna un cliente con los datos respectivos

        }

        #endregion

        #region Método EliminarRegistroConSP (procedimiento almacenado)

        public int EliminarRegistroConSP(EntidadCliente cliente)
        {
            int resultado = -1;
            SqlConnection conexion = new SqlConnection(_cadenaConexion);
            SqlCommand comando = new SqlCommand();

            comando.CommandText = "Eliminar";
            //nombre del procedimiento de la BD

            comando.CommandType = CommandType.StoredProcedure;
            //Especificar que el tipo de comando es un SP (procedimiento almancenado)

            comando.Connection = conexion;

            //El SP recibe un único parámetro que es el ID
            comando.Parameters.AddWithValue("@idcliente", cliente.Id_cliente);

            //El SP tiene un parámetro de SALIDA
            comando.Parameters.Add("@msj", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

            //El SP retorna un valor (un entero)
            comando.Parameters.Add("@retorno", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
                //Ejecuta el SP y también llena las variables de retorno del COMMAND

                // Devuelve 1 o 0 si se pudó resolver
                resultado = Convert.ToInt32(comando.Parameters["@retorno"].Value);

                //Leer el parámetro de salida del SP:
                _mensaje = comando.Parameters["@msj"].Value.ToString();
            }
            catch (Exception)
            {
                throw;
            }

            return resultado;
        }

        #endregion
    }
}
