#nullable disable
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Cine
{
    public class FormFuncion : Form
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
        private ComboBox cbPelicula = new ComboBox();
        private ComboBox cbSala = new ComboBox();
        private DateTimePicker dtpFechaHora = new DateTimePicker();

        private Button btnSalir = new Button();

        public FormFuncion()
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
            this.Text = "Función";
            this.ClientSize = new Size(780, 433);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // grupo de Función
            gbDatos.Text = "Función";
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

            Label lblPelicula = new Label();
            lblPelicula.Text = "Película:";
            lblPelicula.AutoSize = true;
            lblPelicula.Location = new Point(10, 283);
            gbDatos.Controls.Add(lblPelicula);
            cbPelicula.Location = new Point(130, 280);
            cbPelicula.Size = new Size(300, 23);
            cbPelicula.DropDownStyle = ComboBoxStyle.DropDownList;
            gbDatos.Controls.Add(cbPelicula);

            Label lblSala = new Label();
            lblSala.Text = "Sala:";
            lblSala.AutoSize = true;
            lblSala.Location = new Point(10, 313);
            gbDatos.Controls.Add(lblSala);
            cbSala.Location = new Point(130, 310);
            cbSala.Size = new Size(300, 23);
            cbSala.DropDownStyle = ComboBoxStyle.DropDownList;
            gbDatos.Controls.Add(cbSala);

            Label lblFechaHora = new Label();
            lblFechaHora.Text = "Fecha y hora:";
            lblFechaHora.AutoSize = true;
            lblFechaHora.Location = new Point(10, 343);
            gbDatos.Controls.Add(lblFechaHora);
            dtpFechaHora.Location = new Point(130, 340);
            dtpFechaHora.Size = new Size(200, 23);
            dtpFechaHora.Format = DateTimePickerFormat.Custom;
            dtpFechaHora.CustomFormat = "dd/MM/yyyy HH:mm";
            gbDatos.Controls.Add(dtpFechaHora);

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
                SqlDataAdapter daPelicula = new SqlDataAdapter("SELECT IdPelicula, Titulo AS Nombre FROM Pelicula ORDER BY Titulo", con);
                DataTable dtPelicula = new DataTable();
                daPelicula.Fill(dtPelicula);
                cbPelicula.DisplayMember = "Nombre";
                cbPelicula.ValueMember = "IdPelicula";
                cbPelicula.DataSource = dtPelicula;

                SqlDataAdapter daSala = new SqlDataAdapter("SELECT s.IdSala, su.Nombre + ' - ' + s.Nombre AS Nombre FROM Sala s INNER JOIN Sucursal su ON s.IdSucursal = su.IdSucursal ORDER BY su.Nombre, s.Nombre", con);
                DataTable dtSala = new DataTable();
                daSala.Fill(dtSala);
                cbSala.DisplayMember = "Nombre";
                cbSala.ValueMember = "IdSala";
                cbSala.DataSource = dtSala;
            }
        }

        // llena la tabla con los datos de la base de datos
        private void CargarDatos()
        {
            using (SqlConnection con = new SqlConnection(Conexion.Cadena))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT f.IdFuncion, f.IdPelicula, f.IdSala, p.Titulo AS Pelicula, su.Nombre AS Sucursal, s.Nombre AS Sala, f.FechaHora FROM Funcion f INNER JOIN Pelicula p ON f.IdPelicula = p.IdPelicula INNER JOIN Sala s ON f.IdSala = s.IdSala INNER JOIN Sucursal su ON s.IdSucursal = su.IdSucursal ORDER BY f.FechaHora", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgv.DataSource = dt;
            }
            dgv.Columns["IdPelicula"].Visible = false;
            dgv.Columns["IdSala"].Visible = false;
            dgv.Columns["IdFuncion"].HeaderText = "Id";
            dgv.Columns["Pelicula"].HeaderText = "Película";
            dgv.Columns["FechaHora"].HeaderText = "Fecha y hora";
            dgv.Columns["FechaHora"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        }

        // activa o desactiva los campos y los botones
        private void ModoEdicion(bool activo)
        {
            cbPelicula.Enabled = activo;
            cbSala.Enabled = activo;
            dtpFechaHora.Enabled = activo;
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
            cbPelicula.SelectedIndex = -1;
            cbSala.SelectedIndex = -1;
            dtpFechaHora.Value = DateTime.Now;
        }

        // muestra en los campos el registro seleccionado de la tabla
        private void MostrarRegistro()
        {
            if (dgv.CurrentRow != null)
            {
                DataGridViewRow fila = dgv.CurrentRow;
                txtId.Text = fila.Cells["IdFuncion"].Value.ToString();
                cbPelicula.SelectedValue = fila.Cells["IdPelicula"].Value;
                cbSala.SelectedValue = fila.Cells["IdSala"].Value;
                dtpFechaHora.Value = Convert.ToDateTime(fila.Cells["FechaHora"].Value);
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
            if (cbPelicula.SelectedValue == null)
            {
                MessageBox.Show("Elige la película.");
                return;
            }
            if (cbSala.SelectedValue == null)
            {
                MessageBox.Show("Elige la sala.");
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
                        cmd = new SqlCommand("INSERT INTO Funcion (IdPelicula, IdSala, FechaHora) VALUES (@IdPelicula, @IdSala, @FechaHora)", con);
                    }
                    else
                    {
                        cmd = new SqlCommand("UPDATE Funcion SET IdPelicula = @IdPelicula, IdSala = @IdSala, FechaHora = @FechaHora WHERE IdFuncion = @Id", con);
                        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));
                    }
                    cmd.Parameters.AddWithValue("@IdPelicula", cbPelicula.SelectedValue);
                    cmd.Parameters.AddWithValue("@IdSala", cbSala.SelectedValue);
                    cmd.Parameters.AddWithValue("@FechaHora", dtpFechaHora.Value);
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
                        SqlCommand cmd = new SqlCommand("DELETE FROM Funcion WHERE IdFuncion = @Id", con);
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
