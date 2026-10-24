#nullable disable
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Cine
{
    public class FormCiudadEstado : Form
    {
        private bool editarEstado = false;
        private GroupBox gbDatosEstado = new GroupBox();
        private Button btnNuevoEstado = new Button();
        private Button btnEditarEstado = new Button();
        private Button btnEliminarEstado = new Button();
        private Button btnGuardarEstado = new Button();
        private Button btnCancelarEstado = new Button();
        private DataGridView dgvEstado = new DataGridView();
        private TextBox txtIdEstado = new TextBox();
        private TextBox txtNombreEstado = new TextBox();

        private bool editarCiudad = false;
        private GroupBox gbDatosCiudad = new GroupBox();
        private Button btnNuevoCiudad = new Button();
        private Button btnEditarCiudad = new Button();
        private Button btnEliminarCiudad = new Button();
        private Button btnGuardarCiudad = new Button();
        private Button btnCancelarCiudad = new Button();
        private DataGridView dgvCiudad = new DataGridView();
        private TextBox txtIdCiudad = new TextBox();
        private TextBox txtNombreCiudad = new TextBox();
        private ComboBox cbEstadoCiudad = new ComboBox();

        private Button btnSalir = new Button();

        public FormCiudadEstado()
        {
            CrearControles();
            try
            {
                CargarDatosEstado();
                CargarCombosCiudad();
                CargarDatosCiudad();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No se pudo leer la base de datos: " + ex.Message);
            }
            ModoEdicionEstado(false);
            ModoEdicionCiudad(false);
        }

        // crea y acomoda todos los controles del formulario
        private void CrearControles()
        {
            this.Text = "Ciudad y Estado";
            this.ClientSize = new Size(930, 403);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // grupo de Estado
            gbDatosEstado.Text = "Estado";
            gbDatosEstado.Location = new Point(10, 10);
            gbDatosEstado.Size = new Size(400, 315);
            this.Controls.Add(gbDatosEstado);

            btnNuevoEstado.Text = "Nuevo";
            btnNuevoEstado.Location = new Point(10, 20);
            btnNuevoEstado.Size = new Size(70, 28);
            btnNuevoEstado.Click += btnNuevoEstado_Click;
            gbDatosEstado.Controls.Add(btnNuevoEstado);

            btnEditarEstado.Text = "Editar";
            btnEditarEstado.Location = new Point(85, 20);
            btnEditarEstado.Size = new Size(70, 28);
            btnEditarEstado.Click += btnEditarEstado_Click;
            gbDatosEstado.Controls.Add(btnEditarEstado);

            btnEliminarEstado.Text = "Eliminar";
            btnEliminarEstado.Location = new Point(160, 20);
            btnEliminarEstado.Size = new Size(70, 28);
            btnEliminarEstado.Click += btnEliminarEstado_Click;
            gbDatosEstado.Controls.Add(btnEliminarEstado);

            btnGuardarEstado.Text = "Guardar";
            btnGuardarEstado.Location = new Point(235, 20);
            btnGuardarEstado.Size = new Size(70, 28);
            btnGuardarEstado.Click += btnGuardarEstado_Click;
            gbDatosEstado.Controls.Add(btnGuardarEstado);

            btnCancelarEstado.Text = "Cancelar";
            btnCancelarEstado.Location = new Point(310, 20);
            btnCancelarEstado.Size = new Size(70, 28);
            btnCancelarEstado.Click += btnCancelarEstado_Click;
            gbDatosEstado.Controls.Add(btnCancelarEstado);

            dgvEstado.Location = new Point(10, 55);
            dgvEstado.Size = new Size(380, 180);
            dgvEstado.ReadOnly = true;
            dgvEstado.AllowUserToAddRows = false;
            dgvEstado.AllowUserToDeleteRows = false;
            dgvEstado.MultiSelect = false;
            dgvEstado.RowHeadersVisible = false;
            dgvEstado.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEstado.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEstado.SelectionChanged += dgvEstado_SelectionChanged;
            gbDatosEstado.Controls.Add(dgvEstado);

            Label lblIdEstado = new Label();
            lblIdEstado.Text = "Id:";
            lblIdEstado.AutoSize = true;
            lblIdEstado.Location = new Point(10, 253);
            gbDatosEstado.Controls.Add(lblIdEstado);
            txtIdEstado.ReadOnly = true;
            txtIdEstado.Location = new Point(130, 250);
            txtIdEstado.Size = new Size(100, 23);
            gbDatosEstado.Controls.Add(txtIdEstado);

            Label lblNombreEstado = new Label();
            lblNombreEstado.Text = "Nombre:";
            lblNombreEstado.AutoSize = true;
            lblNombreEstado.Location = new Point(10, 283);
            gbDatosEstado.Controls.Add(lblNombreEstado);
            txtNombreEstado.Location = new Point(130, 280);
            txtNombreEstado.Size = new Size(250, 23);
            txtNombreEstado.MaxLength = 50;
            gbDatosEstado.Controls.Add(txtNombreEstado);

            // grupo de Ciudad
            gbDatosCiudad.Text = "Ciudad";
            gbDatosCiudad.Location = new Point(420, 10);
            gbDatosCiudad.Size = new Size(500, 345);
            this.Controls.Add(gbDatosCiudad);

            btnNuevoCiudad.Text = "Nuevo";
            btnNuevoCiudad.Location = new Point(10, 20);
            btnNuevoCiudad.Size = new Size(70, 28);
            btnNuevoCiudad.Click += btnNuevoCiudad_Click;
            gbDatosCiudad.Controls.Add(btnNuevoCiudad);

            btnEditarCiudad.Text = "Editar";
            btnEditarCiudad.Location = new Point(85, 20);
            btnEditarCiudad.Size = new Size(70, 28);
            btnEditarCiudad.Click += btnEditarCiudad_Click;
            gbDatosCiudad.Controls.Add(btnEditarCiudad);

            btnEliminarCiudad.Text = "Eliminar";
            btnEliminarCiudad.Location = new Point(160, 20);
            btnEliminarCiudad.Size = new Size(70, 28);
            btnEliminarCiudad.Click += btnEliminarCiudad_Click;
            gbDatosCiudad.Controls.Add(btnEliminarCiudad);

            btnGuardarCiudad.Text = "Guardar";
            btnGuardarCiudad.Location = new Point(235, 20);
            btnGuardarCiudad.Size = new Size(70, 28);
            btnGuardarCiudad.Click += btnGuardarCiudad_Click;
            gbDatosCiudad.Controls.Add(btnGuardarCiudad);

            btnCancelarCiudad.Text = "Cancelar";
            btnCancelarCiudad.Location = new Point(310, 20);
            btnCancelarCiudad.Size = new Size(70, 28);
            btnCancelarCiudad.Click += btnCancelarCiudad_Click;
            gbDatosCiudad.Controls.Add(btnCancelarCiudad);

            dgvCiudad.Location = new Point(10, 55);
            dgvCiudad.Size = new Size(480, 180);
            dgvCiudad.ReadOnly = true;
            dgvCiudad.AllowUserToAddRows = false;
            dgvCiudad.AllowUserToDeleteRows = false;
            dgvCiudad.MultiSelect = false;
            dgvCiudad.RowHeadersVisible = false;
            dgvCiudad.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCiudad.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCiudad.SelectionChanged += dgvCiudad_SelectionChanged;
            gbDatosCiudad.Controls.Add(dgvCiudad);

            Label lblIdCiudad = new Label();
            lblIdCiudad.Text = "Id:";
            lblIdCiudad.AutoSize = true;
            lblIdCiudad.Location = new Point(10, 253);
            gbDatosCiudad.Controls.Add(lblIdCiudad);
            txtIdCiudad.ReadOnly = true;
            txtIdCiudad.Location = new Point(130, 250);
            txtIdCiudad.Size = new Size(100, 23);
            gbDatosCiudad.Controls.Add(txtIdCiudad);

            Label lblNombreCiudad = new Label();
            lblNombreCiudad.Text = "Nombre:";
            lblNombreCiudad.AutoSize = true;
            lblNombreCiudad.Location = new Point(10, 283);
            gbDatosCiudad.Controls.Add(lblNombreCiudad);
            txtNombreCiudad.Location = new Point(130, 280);
            txtNombreCiudad.Size = new Size(300, 23);
            txtNombreCiudad.MaxLength = 50;
            gbDatosCiudad.Controls.Add(txtNombreCiudad);

            Label lblEstadoCiudad = new Label();
            lblEstadoCiudad.Text = "Estado:";
            lblEstadoCiudad.AutoSize = true;
            lblEstadoCiudad.Location = new Point(10, 313);
            gbDatosCiudad.Controls.Add(lblEstadoCiudad);
            cbEstadoCiudad.Location = new Point(130, 310);
            cbEstadoCiudad.Size = new Size(300, 23);
            cbEstadoCiudad.DropDownStyle = ComboBoxStyle.DropDownList;
            gbDatosCiudad.Controls.Add(cbEstadoCiudad);

            btnSalir.Text = "Salir";
            btnSalir.Location = new Point(840, 365);
            btnSalir.Size = new Size(80, 28);
            btnSalir.Click += btnSalir_Click;
            this.Controls.Add(btnSalir);
        }

        // llena la tabla con los datos de la base de datos
        private void CargarDatosEstado()
        {
            using (SqlConnection con = new SqlConnection(Conexion.Cadena))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT IdEstado, Nombre FROM Estado ORDER BY IdEstado", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvEstado.DataSource = dt;
            }
            dgvEstado.Columns["IdEstado"].HeaderText = "Id";
        }

        // activa o desactiva los campos y los botones
        private void ModoEdicionEstado(bool activo)
        {
            txtNombreEstado.ReadOnly = !activo;
            btnGuardarEstado.Enabled = activo;
            btnCancelarEstado.Enabled = activo;
            btnNuevoEstado.Enabled = !activo;
            btnEditarEstado.Enabled = !activo;
            btnEliminarEstado.Enabled = !activo;
            dgvEstado.Enabled = !activo;
        }

        private void LimpiarCamposEstado()
        {
            txtIdEstado.Text = "";
            txtNombreEstado.Text = "";
        }

        // muestra en los campos el registro seleccionado de la tabla
        private void MostrarRegistroEstado()
        {
            if (dgvEstado.CurrentRow != null)
            {
                DataGridViewRow fila = dgvEstado.CurrentRow;
                txtIdEstado.Text = fila.Cells["IdEstado"].Value.ToString();
                txtNombreEstado.Text = fila.Cells["Nombre"].Value.ToString();
            }
        }

        private void dgvEstado_SelectionChanged(object sender, EventArgs e)
        {
            MostrarRegistroEstado();
        }

        private void btnNuevoEstado_Click(object sender, EventArgs e)
        {
            editarEstado = false;
            LimpiarCamposEstado();
            ModoEdicionEstado(true);
        }

        private void btnEditarEstado_Click(object sender, EventArgs e)
        {
            if (dgvEstado.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un registro.");
                return;
            }
            editarEstado = true;
            ModoEdicionEstado(true);
        }

        private void btnGuardarEstado_Click(object sender, EventArgs e)
        {
            if (txtNombreEstado.Text.Trim() == "")
            {
                MessageBox.Show("Escribe el nombre del estado.");
                return;
            }
            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.Cadena))
                {
                    con.Open();
                    SqlCommand cmd;
                    if (editarEstado == false)
                    {
                        cmd = new SqlCommand("INSERT INTO Estado (Nombre) VALUES (@Nombre)", con);
                    }
                    else
                    {
                        cmd = new SqlCommand("UPDATE Estado SET Nombre = @Nombre WHERE IdEstado = @Id", con);
                        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(txtIdEstado.Text));
                    }
                    cmd.Parameters.AddWithValue("@Nombre", txtNombreEstado.Text.Trim());
                    cmd.ExecuteNonQuery();
                }
                ModoEdicionEstado(false);
                CargarDatosEstado();
                CargarCombosCiudad();
                CargarDatosCiudad();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No se pudo guardar: " + ex.Message);
            }
        }

        private void btnEliminarEstado_Click(object sender, EventArgs e)
        {
            if (dgvEstado.CurrentRow == null)
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
                        SqlCommand cmd = new SqlCommand("DELETE FROM Estado WHERE IdEstado = @Id", con);
                        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(txtIdEstado.Text));
                        cmd.ExecuteNonQuery();
                    }
                    LimpiarCamposEstado();
                    CargarDatosEstado();
                    CargarCombosCiudad();
                    CargarDatosCiudad();
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

        private void btnCancelarEstado_Click(object sender, EventArgs e)
        {
            ModoEdicionEstado(false);
            LimpiarCamposEstado();
            MostrarRegistroEstado();
        }

        // llena los combos con datos de otras tablas
        private void CargarCombosCiudad()
        {
            using (SqlConnection con = new SqlConnection(Conexion.Cadena))
            {
                SqlDataAdapter daEstadoCiudad = new SqlDataAdapter("SELECT IdEstado, Nombre FROM Estado ORDER BY Nombre", con);
                DataTable dtEstadoCiudad = new DataTable();
                daEstadoCiudad.Fill(dtEstadoCiudad);
                cbEstadoCiudad.DisplayMember = "Nombre";
                cbEstadoCiudad.ValueMember = "IdEstado";
                cbEstadoCiudad.DataSource = dtEstadoCiudad;
            }
        }

        // llena la tabla con los datos de la base de datos
        private void CargarDatosCiudad()
        {
            using (SqlConnection con = new SqlConnection(Conexion.Cadena))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT c.IdCiudad, c.Nombre, c.IdEstado, e.Nombre AS Estado FROM Ciudad c INNER JOIN Estado e ON c.IdEstado = e.IdEstado ORDER BY c.IdCiudad", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvCiudad.DataSource = dt;
            }
            dgvCiudad.Columns["IdEstado"].Visible = false;
            dgvCiudad.Columns["IdCiudad"].HeaderText = "Id";
            dgvCiudad.Columns["Nombre"].HeaderText = "Ciudad";
        }

        // activa o desactiva los campos y los botones
        private void ModoEdicionCiudad(bool activo)
        {
            txtNombreCiudad.ReadOnly = !activo;
            cbEstadoCiudad.Enabled = activo;
            btnGuardarCiudad.Enabled = activo;
            btnCancelarCiudad.Enabled = activo;
            btnNuevoCiudad.Enabled = !activo;
            btnEditarCiudad.Enabled = !activo;
            btnEliminarCiudad.Enabled = !activo;
            dgvCiudad.Enabled = !activo;
        }

        private void LimpiarCamposCiudad()
        {
            txtIdCiudad.Text = "";
            txtNombreCiudad.Text = "";
            cbEstadoCiudad.SelectedIndex = -1;
        }

        // muestra en los campos el registro seleccionado de la tabla
        private void MostrarRegistroCiudad()
        {
            if (dgvCiudad.CurrentRow != null)
            {
                DataGridViewRow fila = dgvCiudad.CurrentRow;
                txtIdCiudad.Text = fila.Cells["IdCiudad"].Value.ToString();
                txtNombreCiudad.Text = fila.Cells["Nombre"].Value.ToString();
                cbEstadoCiudad.SelectedValue = fila.Cells["IdEstado"].Value;
            }
        }

        private void dgvCiudad_SelectionChanged(object sender, EventArgs e)
        {
            MostrarRegistroCiudad();
        }

        private void btnNuevoCiudad_Click(object sender, EventArgs e)
        {
            editarCiudad = false;
            LimpiarCamposCiudad();
            ModoEdicionCiudad(true);
        }

        private void btnEditarCiudad_Click(object sender, EventArgs e)
        {
            if (dgvCiudad.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un registro.");
                return;
            }
            editarCiudad = true;
            ModoEdicionCiudad(true);
        }

        private void btnGuardarCiudad_Click(object sender, EventArgs e)
        {
            if (txtNombreCiudad.Text.Trim() == "")
            {
                MessageBox.Show("Escribe el nombre de la ciudad.");
                return;
            }
            if (cbEstadoCiudad.SelectedValue == null)
            {
                MessageBox.Show("Elige el estado.");
                return;
            }
            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.Cadena))
                {
                    con.Open();
                    SqlCommand cmd;
                    if (editarCiudad == false)
                    {
                        cmd = new SqlCommand("INSERT INTO Ciudad (Nombre, IdEstado) VALUES (@Nombre, @IdEstado)", con);
                    }
                    else
                    {
                        cmd = new SqlCommand("UPDATE Ciudad SET Nombre = @Nombre, IdEstado = @IdEstado WHERE IdCiudad = @Id", con);
                        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(txtIdCiudad.Text));
                    }
                    cmd.Parameters.AddWithValue("@Nombre", txtNombreCiudad.Text.Trim());
                    cmd.Parameters.AddWithValue("@IdEstado", cbEstadoCiudad.SelectedValue);
                    cmd.ExecuteNonQuery();
                }
                ModoEdicionCiudad(false);
                CargarDatosCiudad();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("No se pudo guardar: " + ex.Message);
            }
        }

        private void btnEliminarCiudad_Click(object sender, EventArgs e)
        {
            if (dgvCiudad.CurrentRow == null)
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
                        SqlCommand cmd = new SqlCommand("DELETE FROM Ciudad WHERE IdCiudad = @Id", con);
                        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(txtIdCiudad.Text));
                        cmd.ExecuteNonQuery();
                    }
                    LimpiarCamposCiudad();
                    CargarDatosCiudad();
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

        private void btnCancelarCiudad_Click(object sender, EventArgs e)
        {
            ModoEdicionCiudad(false);
            LimpiarCamposCiudad();
            MostrarRegistroCiudad();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
