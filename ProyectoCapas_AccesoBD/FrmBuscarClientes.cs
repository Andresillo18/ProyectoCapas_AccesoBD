using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using CapaLogicaNegocio;
using CapaEntidades;

namespace InterfazEscritorio
{
    public partial class FrmBuscarClientes : Form
    {
        //Crear un nuevo evento para el formulario
        public event EventHandler Aceptar;

        int id_Cliente;

        public FrmBuscarClientes()
        {
            InitializeComponent();
        }

        #region Método CargarListaArray

        private void CargarListaArray(string condicion = "")
        { //carga el datagridview con la informacion de la lista
            BLCliente logica = new BLCliente(Configuracion.getConnectionString);
            List<EntidadCliente> clientes;

            try
            {
                clientes = logica.ListarClientes(condicion);
                if (clientes.Count > 0) //si la lista tiene algo entonces...
                {
                    grdLista.DataSource = clientes;//cargue en el datagridview lo que tiene la lista
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }

        }

        #endregion


        #region Método Seleccionar el ID de una fila 

        private void Seleccionar()
        {
            if (grdLista.SelectedRows.Count > 0) //si ha seleccionado una fila
            {
                id_Cliente = (int)grdLista.SelectedRows[0].Cells[0].Value;
                Aceptar(id_Cliente, null);//le manda el id al evento aceptar que esta en el otro form
                Close();
            }
        }

        #endregion

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Seleccionar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            string condicion = string.Empty;
            //condicion que se usara para realizar el filtrado en los datos para recuperar el cliente deseado
            try
            {
                if (!string.IsNullOrEmpty(txtNombre.Text))//si no esta vacio
                {
                    condicion = string.Format("Nombre like '%{0}%'", txtNombre.Text.Trim());
                    //donde en el nombre sea algo como lo que se escriba en el txtNombre el trim lo usa para quitar espacios
                }



                CargarListaArray(condicion);



            }
            catch (Exception ex)
            {



                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Aceptar(-1, null);
            Close();
        }
    }
}