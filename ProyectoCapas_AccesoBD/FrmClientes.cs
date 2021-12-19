using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CapaEntidades;
using CapaLogicaNegocio;
//Andrés
namespace InterfazEscritorio
//namespace ProyectoCapas_AccesoBD

// Aparecía con este nombre (no con InterfazEscritorio), por eso no servía


/*
Nota sobre los NameSpaces
En caso que no se reconozca FrmBuscarClientes verificar que en FrmClientes y FrmBuscarClientes,
en ambos el namespace sea: InterfazEscritorio


Pero al cambiar en esta clase el namespace de ProyectoCapas_AccesoBD a InterfazEscritorio
- Funciona la línea: FrmBuscarClientes formBuscar;
- pero deja de funcionar: InitializeComponent();

Solución:
Proyecto - Propiedades de InterfazEscritorio
Aplicación
Revisar que en "Nombre del ensamblado" y "Espacio de nombres predeterminado"
en ambos el namespace sea InterfazEscritorio


Compilar - Limpiar Solución
Compilar - Recompilar solución
*/
{
    public partial class FrmClientes : Form
    {
        EntidadCliente clienteRegistrado;
        // Carga en los campos del formulario los datos de una persona

        FrmBuscarClientes formularioBuscar;

        #region Constructor
        public FrmClientes()
        {
            InitializeComponent();
        }
        #endregion

        #region Método para limpiar el formulario

        private void limpiar()
        {
            txtIdCliente.Clear();
            txtNombre.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtDireccion.Clear();
            txtNombre.Focus();
        }

        #endregion

        #region Método para generar una entidad

        private EntidadCliente GenerarEntidad()
        {
            EntidadCliente cliente;
            if (!string.IsNullOrEmpty(txtIdCliente.Text))
            // Si en el cuadro de ID hay algo
            {
                cliente = clienteRegistrado;
                // Como el cliente ya existe en la BD o hay un ID cargado entonces solo cambiará los datos de nuevo y lo guardará proximamente
            }
            else
            {
                cliente = new EntidadCliente();
            }

            cliente.Nombre = txtNombre.Text;
            cliente.Telefono = txtTelefono.Text;
            cliente.Direccion = txtDireccion.Text;

            return cliente;
        }

        #endregion

        #region Método para cargar el DataGridView con una lista

        private void CargarListaArray(string condicion = "")
        {
            BLCliente logica = new BLCliente(Configuracion.getConnectionString);

            List<EntidadCliente> clientes;

            try
            {
                clientes = logica.ListarClientes(condicion);
                //if (clientes.Count > 0)
                //{
                grdLista.DataSource = clientes;
                //Cargue en el DataGridView lo que contenga la lista

                //Con el IF cuando sólo queda una persona y se elimina, sigue apareciendo en DataGridView
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error:" + ex.Message);
                //throw;
            }
        }

        #endregion

        #region Método para cargar el DataGridView con un Dataset

        private void CargarListaDataSet(string condicion = "", string orden = "")
        {
            BLCliente logica = new BLCliente(Configuracion.getConnectionString);
            DataSet DSCliente;

            try
            {
                DSCliente = logica.ListarClientes(condicion, orden);
                //Ingresar el dataset en el datagridview
                grdLista.DataSource = DSCliente;

                /*Debemos indicar  CÚAL TABLA de ese DataSet es la deseamos mostrar;
                 ya que un DataSet puede conrtener VARIAS tablas*/
                grdLista.DataMember = DSCliente.Tables["Clientes"].TableName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido un error:" + ex.Message);
                //throw;
            }
        }


        #endregion

        #region Metodo cargar un cliente del DataGridView a los TextBox

        private void CargarCliente(int id)
        {
            EntidadCliente cliente;
            BLCliente logica = new BLCliente(Configuracion.getConnectionString);

            try
            {
                cliente = logica.ObtenerCliente(id);
                if (cliente != null)
                {
                    txtIdCliente.Text = cliente.Id_cliente.ToString();
                    txtNombre.Text = cliente.Nombre;
                    txtTelefono.Text = cliente.Telefono;
                    txtDireccion.Text = cliente.Direccion;

                    clienteRegistrado = cliente;
                }
                else
                {
                    MessageBox.Show("El cliente no se encuentra en la Base de Datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    CargarListaDataSet();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
        }

        #endregion

        #region Evetos
        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            BLCliente logica = new BLCliente(Configuracion.getConnectionString); //***
            EntidadCliente cliente;
            int resultado;

            try
            {
                if (!string.IsNullOrEmpty(txtNombre.Text) && !string.IsNullOrEmpty(txtTelefono.Text) && !string.IsNullOrEmpty(txtDireccion.Text))
                {
                    cliente = GenerarEntidad();

                    if (!cliente.Existe)
                    //Si el cliente NO existe
                    {
                        resultado = logica.Insertar(cliente);
                    }
                    else
                    {
                        resultado = logica.Modificar(cliente);
                    }

                    if (resultado > 0)
                    {
                        limpiar();

                        MessageBox.Show("Operación realizada con éxito", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        CargarListaDataSet();
                    }
                    else
                    {
                        MessageBox.Show("No se realizó ninguna modificación", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    MessageBox.Show("Los datos son obligatorios", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
        }

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            try
            {
                CargarListaDataSet();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                throw;
            }
        }

        private void grdLista_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = 0;
            try
            {

                //Tomamos el ID de la fila a la cual le dieron doble clic
                id = (int)grdLista.SelectedRows[0].Cells[0].Value;

                CargarCliente(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            EntidadCliente cliente;
            int resultado;
            BLCliente logica = new BLCliente(Configuracion.getConnectionString);

            try
            {
                if (!string.IsNullOrEmpty(txtIdCliente.Text))
                {
                    cliente = logica.ObtenerCliente(int.Parse(txtIdCliente.Text));
                    //busca primero el cliente antes de borrarlo para ver si existe
                    if (cliente != null)
                    {
                        resultado = logica.EliminarRegistroConSP(cliente);
                        //si el cliente no es nulo puede borrarlo


                        MessageBox.Show(logica.Mensaje, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //imprime el mensaje que el SP mandó
                        limpiar();
                        CargarListaDataSet();
                    }
                    else
                    {
                        MessageBox.Show("El cliente no existe", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        limpiar();
                        CargarListaDataSet();
                    }
                }
                else
                //Si el textBox del ID estaba vacío
                {
                    MessageBox.Show("Debe Seleccionar un cliente antes de eliminar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {



                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            formularioBuscar = new FrmBuscarClientes();
            formularioBuscar.Aceptar += new EventHandler(Aceptar);
            //Especificamos que deseamos utilizar el evento Aceptar

            formularioBuscar.ShowDialog();

        }

        #endregion

        #region Evento Aceptar

        /* Esto es una forma de pasar el ID desde un formulario a otro*/

        private void Aceptar(object id, EventArgs e)
        //implementa el evento aceptar y recibe un id el cual se manda desde el formulario que se abre y aqui se carga el cliente
        {
            try
            {
                int idCliente = (int)id;
                if (idCliente != -1)
                {
                    CargarCliente(idCliente);
                }
                else
                {
                    limpiar();
                }
            }
            catch (Exception ex)
            {



                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

    }
}
