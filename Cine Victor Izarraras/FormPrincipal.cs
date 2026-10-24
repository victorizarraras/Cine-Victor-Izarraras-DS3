#nullable disable
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Cine
{
    public class FormPrincipal : Form
    {
        private Label lblTitulo = new Label();
        private Button btnClasificacion = new Button();
        private Button btnGenero = new Button();
        private Button btnPelicula = new Button();
        private Button btnSucursal = new Button();
        private Button btnSala = new Button();
        private Button btnTipoBoleto = new Button();
        private Button btnCiudadEstado = new Button();
        private Button btnFuncion = new Button();
        private Button btnCliente = new Button();
        private Button btnVenta = new Button();
        private Button btnSalir = new Button();

        public FormPrincipal()
        {
            CrearControles();
        }

        private void CrearControles()
        {
            this.Text = "Sistema de Cine";
            this.ClientSize = new Size(480, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblTitulo.Text = "Sistema de Cine";
            lblTitulo.Font = new Font("Segoe UI", 16);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            lblTitulo.Location = new Point(0, 15);
            lblTitulo.Size = new Size(480, 35);
            this.Controls.Add(lblTitulo);

            btnClasificacion.Text = "Clasificación";
            btnClasificacion.Location = new Point(30, 70);
            btnClasificacion.Size = new Size(200, 40);
            btnClasificacion.Click += btnClasificacion_Click;
            this.Controls.Add(btnClasificacion);

            btnGenero.Text = "Género";
            btnGenero.Location = new Point(30, 125);
            btnGenero.Size = new Size(200, 40);
            btnGenero.Click += btnGenero_Click;
            this.Controls.Add(btnGenero);

            btnPelicula.Text = "Película";
            btnPelicula.Location = new Point(30, 180);
            btnPelicula.Size = new Size(200, 40);
            btnPelicula.Click += btnPelicula_Click;
            this.Controls.Add(btnPelicula);

            btnSucursal.Text = "Sucursal";
            btnSucursal.Location = new Point(30, 235);
            btnSucursal.Size = new Size(200, 40);
            btnSucursal.Click += btnSucursal_Click;
            this.Controls.Add(btnSucursal);

            btnSala.Text = "Sala";
            btnSala.Location = new Point(30, 290);
            btnSala.Size = new Size(200, 40);
            btnSala.Click += btnSala_Click;
            this.Controls.Add(btnSala);

            btnTipoBoleto.Text = "Tipo de boleto";
            btnTipoBoleto.Location = new Point(260, 70);
            btnTipoBoleto.Size = new Size(200, 40);
            btnTipoBoleto.Click += btnTipoBoleto_Click;
            this.Controls.Add(btnTipoBoleto);

            btnCiudadEstado.Text = "Ciudad y Estado";
            btnCiudadEstado.Location = new Point(260, 125);
            btnCiudadEstado.Size = new Size(200, 40);
            btnCiudadEstado.Click += btnCiudadEstado_Click;
            this.Controls.Add(btnCiudadEstado);

            btnFuncion.Text = "Función";
            btnFuncion.Location = new Point(260, 180);
            btnFuncion.Size = new Size(200, 40);
            btnFuncion.Click += btnFuncion_Click;
            this.Controls.Add(btnFuncion);

            btnCliente.Text = "Cliente";
            btnCliente.Location = new Point(260, 235);
            btnCliente.Size = new Size(200, 40);
            btnCliente.Click += btnCliente_Click;
            this.Controls.Add(btnCliente);

            btnVenta.Text = "Venta";
            btnVenta.Location = new Point(260, 290);
            btnVenta.Size = new Size(200, 40);
            btnVenta.Click += btnVenta_Click;
            this.Controls.Add(btnVenta);

            btnSalir.Text = "Salir";
            btnSalir.Location = new Point(140, 350);
            btnSalir.Size = new Size(200, 35);
            btnSalir.Click += btnSalir_Click;
            this.Controls.Add(btnSalir);
        }

        private void btnClasificacion_Click(object sender, EventArgs e)
        {
            FormClasificacion pantalla = new FormClasificacion();
            pantalla.ShowDialog();
        }

        private void btnGenero_Click(object sender, EventArgs e)
        {
            FormGenero pantalla = new FormGenero();
            pantalla.ShowDialog();
        }

        private void btnPelicula_Click(object sender, EventArgs e)
        {
            FormPelicula pantalla = new FormPelicula();
            pantalla.ShowDialog();
        }

        private void btnSucursal_Click(object sender, EventArgs e)
        {
            FormSucursal pantalla = new FormSucursal();
            pantalla.ShowDialog();
        }

        private void btnSala_Click(object sender, EventArgs e)
        {
            FormSala pantalla = new FormSala();
            pantalla.ShowDialog();
        }

        private void btnTipoBoleto_Click(object sender, EventArgs e)
        {
            FormTipoBoleto pantalla = new FormTipoBoleto();
            pantalla.ShowDialog();
        }

        private void btnCiudadEstado_Click(object sender, EventArgs e)
        {
            FormCiudadEstado pantalla = new FormCiudadEstado();
            pantalla.ShowDialog();
        }

        private void btnFuncion_Click(object sender, EventArgs e)
        {
            FormFuncion pantalla = new FormFuncion();
            pantalla.ShowDialog();
        }

        private void btnCliente_Click(object sender, EventArgs e)
        {
            FormCliente pantalla = new FormCliente();
            pantalla.ShowDialog();
        }

        private void btnVenta_Click(object sender, EventArgs e)
        {
            FormVenta pantalla = new FormVenta();
            pantalla.ShowDialog();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
