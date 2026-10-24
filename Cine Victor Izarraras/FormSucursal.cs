#nullable disable
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Cine
{
    public class FormSucursal : Form
    {
        private bool editar = false;
        private GroupBox gbDatos = new GroupBox();
        private Button btnNuevo = new Button();
        private Button btnEditar = new Button();
        private Button btnEliminar = new Button();
        private Button btnGuardar = new Button();
        private Button btnCancelar = new Button();
        private DataGridView dgv = new DataGridView();
        private TextBox txtId = new TextBox();
        private TextBox txtNombre = new TextBox();
        private TextBox txtDireccion = new TextBox();
        private ComboBox cbCiudad = new ComboBox();

        private Button btnSalir = new Button();

        public FormSucursal()
        {
            CrearControles();
            try
            {
                CargarCombos();
                CargarDatos();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No se pudo leer la base de datos: " + ex.Message);
            }
            ModoEdicion(false);
        }

        // crea y acomoda todos los controles del formulario
        private void CrearControles()
        {
            this.Text = "Sucursal";
            this.ClientSize = new Size(780, 433);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // grupo de Sucursal
            gbDatos.Text = "Sucursal";
            gbDatos.Location = new Point(10, 10);
            gbDatos.Size = new Size(760, 375);
            this.Controls.Add(gbDatos);

            btnNuevo.Text = "Nuevo";
            btnNuevo.Location = new Point(10, 20);
            btnNuevo.Size = new Size(70, 28);
            btnNuevo.Click += btnNuevo_Click;
            gbDatos.Controls.Add(btnNuevo);

            btnEditar.Text = "Editar";
            btnEditar.Location = new Point(85, 20);
            btnEditar.Size = new Size(70, 28);
            btnEditar.Click += btnEditar_Click;
            gbDatos.Controls.Add(btnEditar);

            btnEliminar.Text = "Eliminar";
            btnEliminar.Location = new Point(160, 20);
            btnEliminar.Size = new Size(70, 28);
            btnEliminar.Click += btnEliminar_Click;
            gbDatos.Controls.Add(btnEliminar);

            btnGuardar.Text = "Guardar";
            btnGuardar.Location = new Point(235, 20);
            btnGuardar.Size = new Size(70, 28);
            btnGuardar.Click += btnGuardar_Click;
            gbDatos.Controls.Add(btnGuardar);

            btnCancelar.Text = "Cancelar";
            btnCancelar.Location = new Point(310, 20);
            btnCancelar.Size = new Size(70, 28);
            btnCancelar.Click += btnCancelar_Click;
            gbDatos.Controls.Add(btnCancelar);

            dgv.Location = new Point(10, 55);
            dgv.Size = new Size(740, 180);
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionChanged += dgv_SelectionChanged;
            gbDatos.Controls.Add(dgv);

            Label lblId = new Label();
            lblId.Text = "Id:";
            lblId.AutoSize = true;
            lblId.Location = new Point(10, 253);
            gbDatos.Controls.Add(lblId);
            txtId.ReadOnly = true;
            txtId.Location = new Point(130, 250);
            txtId.Size = new Size(100, 23);
            gbDatos.Controls.Add(txtId);

            Label lblNombre = new Label();
            lblNombre.Text = "Nombre:";
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(10, 283);
            gbDatos.Controls.Add(lblNombre);
            txtNombre.Location = new Point(130, 280);
            txtNombre.Size = new Size(300, 23);
            txtNombre.MaxLength = 50;
            gbDatos.Controls.Add(txtNombre);

            Label lblDireccion = new Label();
            lblDireccion.Text = "Dirección:";
            lblDireccion.AutoSize = true;
            lblDireccion.Location = new Point(10, 313);
            gbDatos.Controls.Add(lblDireccion);
            txtDireccion.Location = new Point(130, 310);
            txtDireccion.Size = new Size(300, 23);
            txtDireccion.MaxLength = 100;
            gbDatos.Controls.Add(txtDireccion);

            Label lblCiudad = new Label();
            lblCiudad.Text = "Ciudad:";
            lblCiudad.AutoSize = true;
            lblCiudad.Location = new Point(10, 343);
            gbDatos.Controls.Add(lblCiudad);
            cbCiudad.Location = new Point(130, 340);
            cbCiudad.Size = new Size(300, 23);
            cbCiudad.DropDownStyle = ComboBoxStyle.DropDownList;
            gbDatos.Controls.Add(cbCiudad);

            btnSalir.Text = "Salir";
            btnSalir.Location = new Point(690, 395);
            btnSalir.Size = new Size(80, 28);
            btnSalir.Click += btnSalir_Click;
            this.Controls.Add(btnSalir);
        }

        // llena los combos con datos de otras tablas
        private void CargarCombos()
        {
            using (SqlConnection con = new SqlConnection(Conexion.Cadena))
            {
                SqlDataAdapter daCiudad = new SqlDataAdapter("SELECT c.IdCiudad, c.Nombre + ', ' + e.Nombre AS Nombre FROM Ciudad c INNER JOIN Estado e ON c.IdEstado = e.IdEstado ORDER BY c.Nombre", con);
                DataTable dtCiudad = new DataTable();
                daCiudad.Fill(dtCiudad);
                cbCiudad.DisplayMember = "Nombre";
                cbCiudad.ValueMember = "IdCiudad";
                cbCiudad.DataSource = dtCiudad;
            }
        }

        // llena la tabla con los datos de la base de datos
        private void CargarDatos()
        {
            using (SqlConnection con = new SqlConnection(Conexion.Cadena))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT s.IdSucursal, s.Nombre, s.Direccion, s.IdCiudad, c.Nombre AS Ciudad, e.Nombre AS Estado FROM Sucursal s INNER JOIN Ciudad c ON s.IdCiudad = c.IdCiudad INNER JOIN Estado e ON c.IdEstado = e.IdEstado ORDER BY s.IdSucursal", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgv.DataSource = dt;
            }
            dgv.Columns["IdCiudad"].Visible = false;
            dgv.Columns["IdSucursal"].HeaderText = "Id";
            dgv.Columns["Direccion"].HeaderText = "Dirección";
        }

        // activa o desactiva los campos y los botones
        private void ModoEdicion(bool activo)
        {
            txtNombre.ReadOnly = !activo;
            txtDireccion.ReadOnly = !activo;
            cbCiudad.Enabled = activo;
            btnGuardar.Enabled = activo;
            btnCancelar.Enabled = activo;
            btnNuevo.Enabled = !activo;
            btnEditar.Enabled = !activo;
            btnEliminar.Enabled = !activo;
            dgv.Enabled = !activo;
        }

        private void LimpiarCampos()
        {
            txtId.Text = "";
            txtNombre.Text = "";
            txtDireccion.Text = "";
            cbCiudad.SelectedIndex = -1;
        }

        // muestra en los campos el registro seleccionado de la tabla
        private void MostrarRegistro()
        {
            if (dgv.CurrentRow != null)
            {
                DataGridViewRow fila = dgv.CurrentRow;
                txtId.Text = fila.Cells["IdSucursal"].Value.ToString();
                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                txtDireccion.Text = fila.Cells["Direccion"].Value.ToString();
                cbCiudad.SelectedValue = fila.Cells["IdCiudad"].Value;
            }
        }

        private void dgv_SelectionChanged(object sender, EventArgs e)
        {
            MostrarRegistro();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            editar = false;
            LimpiarCampos();
            ModoEdicion(true);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un registro.");
                return;
            }
            editar = true;
            ModoEdicion(true);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text.Trim() == "")
            {
                MessageBox.Show("Escribe el nombre de la sucursal.");
                return;
            }
            if (txtDireccion.Text.Trim() == "")
            {
                MessageBox.Show("Escribe la dirección.");
                return;
            }
            if (cbCiudad.SelectedValue == null)
            {
                MessageBox.Show("Elige la ciudad.");
                return;
            }
            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.Cadena))
                {
                    con.Open();
                    SqlCommand cmd;
                    if (editar == false)
                    {
                        cmd = new SqlCommand("INSERT INTO Sucursal (Nombre, Direccion, IdCiudad) VALUES (@Nombre, @Direccion, @IdCiudad)", con);
                    }
                    else
                    {
                        cmd = new SqlCommand("UPDATE Sucursal SET Nombre = @Nombre, Direccion = @Direccion, IdCiudad = @IdCiudad WHERE IdSucursal = @Id", con);
                        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));
                    }
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@Direccion", txtDireccion.Text.Trim());
                    cmd.Parameters.AddWithValue("@IdCiudad", cbCiudad.SelectedValue);
                    cmd.ExecuteNonQuery();
                }
                ModoEdicion(false);
                CargarDatos();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No se pudo guardar: " + ex.Message);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un registro.");
                return;
            }
            DialogResult respuesta = MessageBox.Show("¿Seguro que deseas eliminar este registro?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (respuesta == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(Conexion.Cadena))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("DELETE FROM Sucursal WHERE IdSucursal = @Id", con);
                        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));
                        cmd.ExecuteNonQuery();
                    }
                    LimpiarCampos();
                    CargarDatos();
                }
                catch (SqlException ex)
                {
                    // el error 547 es cuando el registro se usa en otra tabla
                    if (ex.Number == 547)
                    {
                        MessageBox.Show("No se puede eliminar porque se está usando en otra tabla.");
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar: " + ex.Message);
                    }
                }
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            ModoEdicion(false);
            LimpiarCampos();
            MostrarRegistro();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
