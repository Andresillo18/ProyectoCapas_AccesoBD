using System;

namespace CapaEntidades
{
    public class EntidadCliente
    {
        #region Atributos

        private int id_cliente;
        private string nombre;
        private string telefono;
        private string direccion;
        private bool existe;

        #endregion


        #region Propiedades

        public int Id_cliente { get => id_cliente; set => id_cliente = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public bool Existe { get => existe; set => existe = value; }

        #endregion


        #region Constructores

        public EntidadCliente()
        {
            id_cliente = 0;
            nombre = string.Empty;
            telefono = string.Empty;
            direccion = string.Empty;
            existe = false;
        }

        public EntidadCliente(int id, string nombreC, string telefonoC, string direccionC)
        {
            id_cliente = id;
            nombre = nombreC;
            telefono = telefonoC;
            direccion = direccionC;
            existe = true;
        }

        #endregion


        #region Metodos

        public override string ToString()
        {
            return string.Format("{0} - {1}", id_cliente, nombre);
        }

        /*
         El método ToString va a retornar TODOS los atributos
        Lo sobreescribimos en este campo para que retorne solo 2 atributos
        **Tampoco es necesario que las entidades tengan métodos porque solo sirven para mover información   
         */

        #endregion

    }
}



