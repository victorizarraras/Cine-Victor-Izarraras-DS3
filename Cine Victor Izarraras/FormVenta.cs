#nullable disable
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Cine
{
    public class FormVenta : Form
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
        private ComboBox cbCliente = new ComboBox();
        private ComboBox cbFuncion = new ComboBox();
        private ComboBox cbTipoBoleto = new ComboBox();
        private TextBox txtCantidad = new TextBox();
        private TextBox txtTotal = new TextBox();

        private Button btnSalir = new Button();

        public FormVenta()
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
            this.Text = "Venta";
            this.ClientSize = new Size(780, 493);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // grupo de Venta
            gbDatos.Text = "Venta";
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

            Label lblCliente = new Label();
            lblCliente.Text = "Cliente:";
            lblCliente.AutoSize = true;
            lblCliente.Location = new Point(10, 283);
            gbDatos.Controls.Add(lblCliente);
            cbCliente.Location = new Point(130, 280);
            cbCliente.Size = new Size(300, 23);
            cbCliente.DropDownStyle = ComboBoxStyle.DropDownList;
            gbDatos.Controls.Add(cbCliente);

            Label lblFuncion = new Label();
            lblFuncion.Text = "Función:";
            lblFuncion.AutoSize = true;
            lblFuncion.Location = new Point(10, 313);
            gbDatos.Controls.Add(lblFuncion);
            cbFuncion.Location = new Point(130, 310);
            cbFuncion.Size = new Size(300, 23);
            cbFuncion.DropDownStyle = ComboBoxStyle.DropDownList;
            gbDatos.Controls.Add(cbFuncion);

            Label lblTipoBoleto = new Label();
            lblTipoBoleto.Text = "Tipo de boleto:";
            lblTipoBoleto.AutoSize = true;
            lblTipoBoleto.Location = new Point(10, 343);
            gbDatos.Controls.Add(lblTipoBoleto);
            cbTipoBoleto.Location = new Point(130, 340);
            cbTipoBoleto.Size = new Size(300, 23);
            cbTipoBoleto.DropDownStyle = ComboBoxStyle.DropDownList;
            gbDatos.Controls.Add(cbTipoBoleto);

            Label lblCantidad = new Label();
            lblCantidad.Text = "Cantidad:";
            lblCantidad.AutoSize = true;
            lblCantidad.Location = new Point(10, 373);
            gbDatos.Controls.Add(lblCantidad);
            txtCantidad.Location = new Point(130, 370);
            txtCantidad.Size = new Size(300, 23);
            gbDatos.Controls.Add(txtCantidad);

            Label lblTotal = new Label();
            lblTotal.Text = "Total:";
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(10, 403);
            gbDatos.Controls.Add(lblTotal);
            txtTotal.Location = new Point(130, 400);
            txtTotal.Size = new Size(300, 23);
            txtTotal.ReadOnly = true;
            gbDatos.Controls.Add(txtTotal);

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
                SqlDataAdapter daCliente = new SqlDataAdapter("SELECT IdCliente, Nombre FROM Cliente ORDER BY Nombre", con);
                DataTable dtCliente = new DataTable();
                daCliente.Fill(dtCliente);
                cbCliente.DisplayMember = "Nombre";
                cbCliente.ValueMember = "IdCliente";
                cbCliente.DataSource = dtCliente;

                SqlDataAdapter daFuncion = new SqlDataAdapter("SELECT f.IdFuncion, p.Titulo + ' - ' + CONVERT(VARCHAR(16), f.FechaHora, 120) AS Nombre FROM Funcion f INNER JOIN Pelicula p ON f.IdPelicula = p.IdPelicula ORDER BY f.FechaHora", con);
                DataTable dtFuncion = new DataTable();
                daFuncion.Fill(dtFuncion);
                cbFuncion.DisplayMember = "Nombre";
                cbFuncion.ValueMember = "IdFuncion";
                cbFuncion.DataSource = dtFuncion;

                SqlDataAdapter daTipoBoleto = new SqlDataAdapter("SELECT IdTipoBoleto, Nombre, Precio FROM TipoBoleto ORDER BY Nombre", con);
                DataTable dtTipoBoleto = new DataTable();
                daTipoBoleto.Fill(dtTipoBoleto);
                cbTipoBoleto.DisplayMember = "Nombre";
                cbTipoBoleto.ValueMember = "IdTipoBoleto";
                cbTipoBoleto.DataSource = dtTipoBoleto;
            }
        }

        // llena la tabla con los datos de la base de datos
        private void CargarDatos()
        {
            using (SqlConnection con = new SqlConnection(Conexion.Cadena))
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT v.IdVenta, v.IdCliente, v.IdFuncion, v.IdTipoBoleto, c.Nombre AS Cliente, p.Titulo AS Pelicula, f.FechaHora AS FechaFuncion, t.Nombre AS TipoBoleto, v.Cantidad, v.Total, v.FechaVenta FROM Venta v INNER JOIN Cliente c ON v.IdCliente = c.IdCliente INNER JOIN Funcion f ON v.IdFuncion = f.IdFuncion INNER JOIN Pelicula p ON f.IdPelicula = p.IdPelicula INNER JOIN TipoBoleto t ON v.IdTipoBoleto = t.IdTipoBoleto ORDER BY v.IdVenta", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgv.DataSource = dt;
            }
            dgv.Columns["IdCliente"].Visible = false;
            dgv.Columns["IdFuncion"].Visible = false;
            dgv.Columns["IdTipoBoleto"].Visible = false;
            dgv.Columns["IdVenta"].HeaderText = "Id";
            dgv.Columns["Pelicula"].HeaderText = "Película";
            dgv.Columns["FechaFuncion"].HeaderText = "Función";
            dgv.Columns["FechaFuncion"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            dgv.Columns["TipoBoleto"].HeaderText = "Tipo de boleto";
            dgv.Columns["Total"].DefaultCellStyle.Format = "C2";
            dgv.Columns["FechaVenta"].HeaderText = "Fecha de venta";
            dgv.Columns["FechaVenta"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        }

        // activa o desactiva los campos y los botones
        private void ModoEdicion(bool activo)
        {
            cbCliente.Enabled = activo;
            cbFuncion.Enabled = activo;
            cbTipoBoleto.Enabled = activo;
            txtCantidad.ReadOnly = !activo;
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
            cbCliente.SelectedIndex = -1;
            cbFuncion.SelectedIndex = -1;
            cbTipoBoleto.SelectedIndex = -1;
            txtCantidad.Text = "";
            txtTotal.Text = "";
        }

        // muestra en los campos el registro seleccionado de la tabla
        private void MostrarRegistro()
        {
            if (dgv.CurrentRow != null)
            {
                DataGridViewRow fila = dgv.CurrentRow;
                txtId.Text = fila.Cells["IdVenta"].Value.ToString();
                cbCliente.SelectedValue = fila.Cells["IdCliente"].Value;
                cbFuncion.SelectedValue = fila.Cells["IdFuncion"].Value;
                cbTipoBoleto.SelectedValue = fila.Cells["IdTipoBoleto"].Value;
                txtCantidad.Text = fila.Cells["Cantidad"].Value.ToString();
                txtTotal.Text = fila.Cells["Total"].Value.ToString();
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
            if (cbCliente.SelectedValue == null)
            {
                MessageBox.Show("Elige el cliente.");
                return;
            }
            if (cbFuncion.SelectedValue == null)
            {
                MessageBox.Show("Elige la función.");
                return;
            }
            if (cbTipoBoleto.SelectedValue == null)
            {
                MessageBox.Show("Elige el tipo de boleto.");
                return;
            }
            int cantidad;
            if (int.TryParse(txtCantidad.Text, out cantidad) == false || cantidad <= 0)
            {
                MessageBox.Show("La cantidad debe ser un número entero mayor a 0.");
                return;
            }
            // el total es el precio del boleto por la cantidad
            DataRowView filaBoleto = (DataRowView)cbTipoBoleto.SelectedItem;
            decimal precio = Convert.ToDecimal(filaBoleto["Precio"]);
            decimal total = precio * cantidad;
            txtTotal.Text = total.ToString();
            try
            {
                using (SqlConnection con = new SqlConnection(Conexion.Cadena))
                {
                    con.Open();
                    SqlCommand cmd;
                    if (editar == false)
                    {
                        cmd = new SqlCommand("INSERT INTO Venta (IdCliente, IdFuncion, IdTipoBoleto, Cantidad, Total) VALUES (@IdCliente, @IdFuncion, @IdTipoBoleto, @Cantidad, @Total)", con);
                    }
                    else
                    {
                        cmd = new SqlCommand("UPDATE Venta SET IdCliente = @IdCliente, IdFuncion = @IdFuncion, IdTipoBoleto = @IdTipoBoleto, Cantidad = @Cantidad, Total = @Total WHERE IdVenta = @Id", con);
                        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(txtId.Text));
                    }
                    cmd.Parameters.AddWithValue("@IdCliente", cbCliente.SelectedValue);
                    cmd.Parameters.AddWithValue("@IdFuncion", cbFuncion.SelectedValue);
                    cmd.Parameters.AddWithValue("@IdTipoBoleto", cbTipoBoleto.SelectedValue);
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@Total", total);
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
                        SqlCommand cmd = new SqlCommand("DELETE FROM Venta WHERE IdVenta = @Id", con);
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
