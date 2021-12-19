using System;

using CapaAccesoDatos;
using CapaEntidades;
using System.Data;
using System.Collections.Generic;

namespace CapaLogicaNegocio
{
    public class BLCliente
    {
        #region Atributos

        private String _cadenaConexion;
        private String _mensaje;

        #endregion

        #region Propiedades

        public string CadenaConexion { set => _cadenaConexion = value; }
        public string Mensaje { get => _mensaje; }

        #endregion

        #region Constructores

        public BLCliente()
        {
            _cadenaConexion = string.Empty;
            _mensaje = string.Empty;
        }

        public BLCliente(String cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
            _mensaje = string.Empty;
        }
        #endregion

        #region Método Insertar

        // Los métodos de la capa de BL son un PUENTE a la capa de DA
        //Y en la capa de BL se aplicarían las comprobaciones y REGLAS DEL NEGOCIO
        public int Insertar(EntidadCliente cliente)
        {
            int id_cliente = 0;
            /*
             La capa de BL hace uso de la capa de DA
            Significa que en esta capa creamos INSTANCIAS de DA
             */

            DACliente accesoDatos = new DACliente(_cadenaConexion);

            //Por aquí se puede hacer COMPROBACIONES u otros trbajos correspondientes a BL
            try
            {
                id_cliente = accesoDatos.Insertar(cliente);
            }
            catch (Exception)
            {

                throw;
            }

            return id_cliente;
        }

        #endregion

        #region Modificar (Actualizar)

        public int Modificar(EntidadCliente cliente)
        {
            int filasAfectadas = 0;
            DACliente accesoDatos = new DACliente(_cadenaConexion);

            try
            {
                filasAfectadas = accesoDatos.Modificar(cliente);
            }
            catch (Exception)
            {
                throw;
            }

            return filasAfectadas;
        }

        #endregion

        #region ListarClientes(string condicion, string orden)

        public DataSet ListarClientes(string condicion, string orden)
        {
            DataSet DS;
            DACliente accesoDatos = new DACliente(_cadenaConexion);

            try
            {
                DS = accesoDatos.ListarClientes(condicion, orden);
            }
            catch (Exception)
            {
                throw;
            }

            return DS;
        }

        #endregion

        #region ListarClientes(String condicion)

        //Esto es una firma
        public List<EntidadCliente> ListarClientes(String condicion)
        {
            //Se crea el objeto que se devolverá
            List<EntidadCliente> listaClientes;
            DACliente accesoDatos = new DACliente(_cadenaConexion);

            try
            {
                listaClientes = accesoDatos.ListarClientes(condicion);
            }
            catch (Exception)
            {
                throw;
            }

            return listaClientes;
        }

        #endregion

        #region ObtenerCliente

        public EntidadCliente ObtenerCliente(int id)
        {
            EntidadCliente cliente;
            DACliente accesoDatos = new DACliente(_cadenaConexion);

            try
            {
                cliente = accesoDatos.ObtenerCliente(id);
            }
            catch (Exception)
            {
                throw;
            }
            return cliente;
        }

        #endregion

        #region EliminarRegistroConSP()

        public int EliminarRegistroConSP(EntidadCliente cliente)
        {
            int resultado;
            DACliente accesoDatos = new DACliente(_cadenaConexion);

            try
            {
                resultado = accesoDatos.EliminarRegistroConSP(cliente);
                _mensaje = accesoDatos.Mensaje; // Se le debe avisar al usuario que sucedio en el proceso almacenado por eso se usa este atributo
                // Se esta usando una propiedad (get)
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