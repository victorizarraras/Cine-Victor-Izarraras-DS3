#nullable disable
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Cine
{
    public class FormPelicula : Form
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
        private TextBox txtTitulo = new TextBox();
        private TextBox txtDuracion = new TextBox();
        private DateTimePicker dtpFechaEstreno = new DateTimePicker();
        private ComboBox cbClasificacion = new ComboBox();
        private ComboBox cbGenero = new ComboBox();

        private Button btnSalir = new Button();

        public FormPelicula()
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
            this.Text = "Película";
            this.ClientSize = new Size(780, 493);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // grupo de Película
            gbDatos.Text = "Película";
            gbDatos.Location = new Point(10, 10);
            gbDatos.Size = new Size(760, 435);
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

            Label lblTitulo = new Label();
            lblTitulo.Text = "Título:";
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(10, 283);
            gbDatos.Controls.Add(lblTitulo);
            txtTitulo.Location = new Point(130, 280);
            txtTitulo.Size = new Size(300, 23);
            txtTitulo.MaxLength = 100;
            gbDatos.Controls.Add(txtTitulo);

            Label lblDuracion = new Label();
            lblDuracion.Text = "Duración (min):";
            lblDuracion.AutoSize = true;
            lblDuracion.Location = new Point(10, 313);
            gbDatos.Controls.Add(lblDuracion);
            txtDuracion.Location = new Point(130, 310);
            txtDuracion.Size = new Size(300, 23);
            gbDatos.Controls.Add(txtDuracion);

            Label lblFechaEstreno = new Label();
            lblFechaEstreno.Text = "Fecha de estreno:";
            lblFechaEstreno.AutoSize = true;
            lblFechaEstreno.Location = new Point(10, 343);
            gbDatos.Controls.Add(lblFechaEstreno);
            dtpFechaEstreno.Location = new Point(130, 340);
            dtpFechaEstreno.Size = new Size(200, 23);
            dtpFechaEstreno.Format = DateTimePickerFormat.Short;
            gbDatos.Controls.Add(dtpFechaEstreno);

            Label lblClasificacion = new Label();
            lblClasificacion.Text = "Clasificación:";
            lblClasificacion.AutoSize = true;
            lblClasificacion.Location = new Point(10, 373);
            gbDatos.Controls.Add(lblClasificacion);
            cbClasificacion.Location = new Point(130, 370);
            cbClasificacion.Size = new Size(300, 23);
            cbClasificacion.DropDownStyle = ComboBoxStyle.DropDownList;
            gbDatos.Controls.Add(cbClasificacion);

            Label lblGenero = new Label();
            lblGenero.Text = "Género:";
            lblGenero.AutoSize = true;
            lblGenero.Location = new Point(10, 403);
            gbDatos.Controls.Add(lblGenero);
            cbGenero.Location = new Point(130, 400);
            cbGenero.Size = new Size(300, 23);
            cbGenero.DropDownStyle = ComboBoxStyle.DropDownList;
            gbDatos.Controls.Add(cbGenero);

            btnSalir.Text = "Salir";
            btnSalir.Location = new Point(690, 455);
            btnSalir.Size = new Size(80, 28);
            btnSalir.Click += btnSalir_Click;
            this.Controls.Add(btnSalir);
        }

        // llena los combos con datos de otras tablas
        private void CargarCombos()
        {
            using (SqlConnection con = new SqlConnection(Conexion.Cadena))
            {
                SqlDataAdapter daClasificacion = new SqlDataAdapter("SELECT IdClasificacion, Nombre FROM Clasificacion ORDER BY Nombre", con);
                DataTable dtClasificacion = new DataTable();
                daClasificacion.Fill(dtClasificacion);
                cbClasificacion.DisplayMember = "Nombre";
                cbClasificacion.ValueMember = "IdClasificacion";
                cbClasificacion.DataSource = dtClasificacion;

                SqlDataAdapter daGenero = new SqlDataAdapter("SELECT IdGenero, Nombre FROM Genero ORDER BY Nombre", con);
                DataTable dtGenero = new DataTable();
                daGenero.Fill(dtGenero);
                cbGenero.DisplayMember = "Nombre";
                cbGenero.ValueMember = "IdGenero";
                cbGenero.DataSource = dtGenero;
            }
        }

        // llena la tabla con los datos de la base de datos
        private void CargarDatos()
        {
            using (SqlConnection con = new SqlConnection(Conexion.Cadena))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT p.IdPelicula, p.Titulo, p.Duracion, p.FechaEstreno, p.IdClasificacion, p.IdGenero, c.Nombre AS Clasificacion, g.Nombre AS Genero FROM Pelicula p INNER JOIN Clasificacion c ON p.IdClasificacion = c.IdClasificacion INNER JOIN Genero g ON p.IdGenero = g.IdGenero ORDER BY p.IdPelicula", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgv.DataSource = dt;
            }
            dgv.Columns["IdClasificacion"].Visible = false;
            dgv.Columns["IdGenero"].Visible = false;
            dgv.Columns["IdPelicula"].HeaderText = "Id";
            dgv.Columns["Titulo"].HeaderText = "Título";
            dgv.Columns["Duracion"].HeaderText = "Duración (min)";
            dgv.Columns["FechaEstreno"].HeaderText = "Fecha de estreno";
            dgv.Columns["FechaEstreno"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgv.Columns["Clasificacion"].HeaderText = "Clasificación";
            dgv.Columns["Genero"].HeaderText = "Género";
        }

        // activa o desactiva los campos y los botones
        private void ModoEdicion(bool activo)
        {
            txtTitulo.ReadOnly = !activo;
            txtDuracion.ReadOnly = !activo;
            dtpFechaEstreno.Enabled = activo;
            cbClasificacion.Enabled = activo;
            cbGenero.Enabled = activo;
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
            txtTitulo.Text = "";
            txtDuracion.Text = "";
            dtpFechaEstreno.Value = DateTime.Now;
            cbClasificacion.SelectedIndex = -1;
            cbGenero.SelectedIndex = -1;
        }

        // muestra en los campos el registro seleccionado de la tabla
        private void MostrarRegistro()
        {
            if (dgv.CurrentRow != null)
            {
                DataGridViewRow fila = dgv.CurrentRow;
                txtId.Text = fila.Cells["IdPelicula"].Value.ToString();
                txtTitulo.Text = fila.Cells["Titulo"].Value.ToString();
                txtDuracion.Text = fila.Cells["Duracion"].Value.ToString();
                dtpFechaEstreno.Value = Convert.ToDateTime(fila.Cells["FechaEstreno"].Value);
                cbClasificacion.SelectedValue = fila.Cells["IdClasificacion"].Value;
                cbGenero.SelectedValue = fila.Cells["IdGenero"].Value;
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
            if (txtTitulo.Text.Trim() == "")
            {
                MessageBox.Show("Escribe el título de la película.");
                return;
            }
            int duracion;
            if (int.TryParse(txtDuracion.Text, out duracion) == false || duracion <= 0)
            {
                MessageBox.Show("La duración debe ser un número entero mayor a 0.");
                return;
            }
            if (cbClasificacion.SelectedValue == null)
            {
                MessageBox.Show("Elige la clasificación.");
                return;
            }
            if (cbGenero.SelectedValue == null)
            {
                MessageBox.Show("Elige el género.");
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
                        cmd = new SqlCommand("INSERT INTO Pelicula (Titulo, Duracion, FechaEstreno, IdClasificacion, IdGenero) VALUES (@Titulo, @Duracion, @FechaEstreno, @IdClasificacion, @IdGenero)", con);
                    }
                    else
                    {
                        cmd = new SqlCommand("UPDATE Pelicula SET Titulo = @Titulo, Duracion = @Duracion, FechaEstreno = @FechaEstreno, IdClasificacion = @IdClasificacion, IdGenero = @IdGenero WHERE IdPelicula = @Id", con);
                        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));
                    }
                    cmd.Parameters.AddWithValue("@Titulo", txtTitulo.Text.Trim());
                    cmd.Parameters.AddWithValue("@Duracion", duracion);
                    cmd.Parameters.AddWithValue("@FechaEstreno", dtpFechaEstreno.Value.Date);
                    cmd.Parameters.AddWithValue("@IdClasificacion", cbClasificacion.SelectedValue);
                    cmd.Parameters.AddWithValue("@IdGenero", cbGenero.SelectedValue);
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
                        SqlCommand cmd = new SqlCommand("DELETE FROM Pelicula WHERE IdPelicula = @Id", con);
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
