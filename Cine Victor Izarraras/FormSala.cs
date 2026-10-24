#nullable disable
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Cine
{
    public class FormSala : Form
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
        private TextBox txtCapacidad = new TextBox();
        private ComboBox cbSucursal = new ComboBox();

        private Button btnSalir = new Button();

        public FormSala()
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
            this.Text = "Sala";
            this.ClientSize = new Size(780, 433);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // grupo de Sala
            gbDatos.Text = "Sala";
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
            lblNombre.Text = "Nombre o número:";
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(10, 283);
            gbDatos.Controls.Add(lblNombre);
            txtNombre.Location = new Point(130, 280);
            txtNombre.Size = new Size(300, 23);
            txtNombre.MaxLength = 30;
            gbDatos.Controls.Add(txtNombre);

            Label lblCapacidad = new Label();
            lblCapacidad.Text = "Capacidad:";
            lblCapacidad.AutoSize = true;
            lblCapacidad.Location = new Point(10, 313);
            gbDatos.Controls.Add(lblCapacidad);
            txtCapacidad.Location = new Point(130, 310);
            txtCapacidad.Size = new Size(300, 23);
            gbDatos.Controls.Add(txtCapacidad);

            Label lblSucursal = new Label();
            lblSucursal.Text = "Sucursal:";
            lblSucursal.AutoSize = true;
            lblSucursal.Location = new Point(10, 343);
            gbDatos.Controls.Add(lblSucursal);
            cbSucursal.Location = new Point(130, 340);
            cbSucursal.Size = new Size(300, 23);
            cbSucursal.DropDownStyle = ComboBoxStyle.DropDownList;
            gbDatos.Controls.Add(cbSucursal);

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
                SqlDataAdapter daSucursal = new SqlDataAdapter("SELECT IdSucursal, Nombre FROM Sucursal ORDER BY Nombre", con);
                DataTable dtSucursal = new DataTable();
                daSucursal.Fill(dtSucursal);
                cbSucursal.DisplayMember = "Nombre";
                cbSucursal.ValueMember = "IdSucursal";
                cbSucursal.DataSource = dtSucursal;
            }
        }

        // llena la tabla con los datos de la base de datos
        private void CargarDatos()
        {
            using (SqlConnection con = new SqlConnection(Conexion.Cadena))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT s.IdSala, s.Nombre, s.Capacidad, s.IdSucursal, su.Nombre AS Sucursal FROM Sala s INNER JOIN Sucursal su ON s.IdSucursal = su.IdSucursal ORDER BY s.IdSala", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgv.DataSource = dt;
            }
            dgv.Columns["IdSucursal"].Visible = false;
            dgv.Columns["IdSala"].HeaderText = "Id";
            dgv.Columns["Nombre"].HeaderText = "Sala";
        }

        // activa o desactiva los campos y los botones
        private void ModoEdicion(bool activo)
        {
            txtNombre.ReadOnly = !activo;
            txtCapacidad.ReadOnly = !activo;
            cbSucursal.Enabled = activo;
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
            txtCapacidad.Text = "";
            cbSucursal.SelectedIndex = -1;
        }

        // muestra en los campos el registro seleccionado de la tabla
        private void MostrarRegistro()
        {
            if (dgv.CurrentRow != null)
            {
                DataGridViewRow fila = dgv.CurrentRow;
                txtId.Text = fila.Cells["IdSala"].Value.ToString();
                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                txtCapacidad.Text = fila.Cells["Capacidad"].Value.ToString();
                cbSucursal.SelectedValue = fila.Cells["IdSucursal"].Value;
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
                MessageBox.Show("Escribe el nombre o número de la sala.");
                return;
            }
            int capacidad;
            if (int.TryParse(txtCapacidad.Text, out capacidad) == false || capacidad <= 0)
            {
                MessageBox.Show("La capacidad debe ser un número entero mayor a 0.");
                return;
            }
            if (cbSucursal.SelectedValue == null)
            {
                MessageBox.Show("Elige la sucursal.");
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
                        cmd = new SqlCommand("INSERT INTO Sala (Nombre, Capacidad, IdSucursal) VALUES (@Nombre, @Capacidad, @IdSucursal)", con);
                    }
                    else
                    {
                        cmd = new SqlCommand("UPDATE Sala SET Nombre = @Nombre, Capacidad = @Capacidad, IdSucursal = @IdSucursal WHERE IdSala = @Id", con);
                        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));
                    }
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@Capacidad", capacidad);
                    cmd.Parameters.AddWithValue("@IdSucursal", cbSucursal.SelectedValue);
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
                        SqlCommand cmd = new SqlCommand("DELETE FROM Sala WHERE IdSala = @Id", con);
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
