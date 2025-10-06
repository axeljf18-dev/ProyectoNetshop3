using Microsoft.Data.SqlClient;
using ProyectoNetshop.Cruds;
using ProyectoNetshop.BD;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vistaDeProyectoC
{
    public partial class FRegistrarCliente : Form
    {
        public FRegistrarCliente()
        {
            InitializeComponent();

            this.Load += FRegistrarCliente_Load;

            tbTelefono.KeyPress -= tbTelefono_KeyPress;
            tbDNI.KeyPress -= tbDNI_KeyPress;
            tbNombre.KeyPress -= tbNombre_KeyPress;
            tbApellido.KeyPress -= tbApellido_KeyPress;

            tbTelefono.KeyPress += tbTelefono_KeyPress;
            tbDNI.KeyPress += tbDNI_KeyPress;
            tbNombre.KeyPress += tbNombre_KeyPress;
            tbApellido.KeyPress += tbApellido_KeyPress;

            tbNombre.TextChanged += InputFields_Changed;
            tbApellido.TextChanged += InputFields_Changed;
            tbDNI.TextChanged += InputFields_Changed;
            tbEmail.TextChanged += InputFields_Changed;
            tbTelefono.TextChanged += InputFields_Changed;
            rbMasculino.CheckedChanged += InputFields_Changed;
            rbFemenino.CheckedChanged += InputFields_Changed;
            rbOtros.CheckedChanged += InputFields_Changed;
            fechaNacimiento.ValueChanged += InputFields_Changed;
            fechaNacimiento.MouseClick += InputFields_Changed;

            tbBusquedaDniCliente.KeyPress -= tbBusquedaDniCliente_KeyPress;
            tbBusquedaNombreCliente.KeyPress -= tbBusquedaNombreCliente_KeyPress;

            tbBusquedaDniCliente.KeyPress += tbBusquedaDniCliente_KeyPress;
            tbBusquedaNombreCliente.KeyPress += tbBusquedaNombreCliente_KeyPress;
        }

        private void tbNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsLetter(e.KeyChar)
                && e.KeyChar != ' ')
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten letras.", "Carácter no permitido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tbApellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                 && !char.IsLetter(e.KeyChar)
                 && e.KeyChar != ' ')
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten letras.", "Carácter no permitido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tbTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten números.", "Carácter no permitido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tbDNI_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Solo se permiten números.", "Carácter no permitido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FRegistrarCliente_Load(object sender, EventArgs e)
        {
            btnGuardar.Enabled = false;

            // Máximo de dígitos
            tbDNI.MaxLength = 8;
            tbTelefono.MaxLength = 10;

            fechaNacimiento.ShowCheckBox = true;
            fechaNacimiento.Checked = false;
            fechaNacimiento.Value = DateTime.Today;

            // Agregamos una fila de ejemplo:
            dgvClientes.DataSource = ObtenerClientes();
        }

        private void InputFields_Changed(object sender, EventArgs e)
        {
            validarCampos();
        }

        private void validarCampos()
        {
            btnGuardar.Enabled =
                !string.IsNullOrWhiteSpace(tbNombre.Text) &&
                !string.IsNullOrWhiteSpace(tbApellido.Text) &&
                ValidarDNI() &&
                ValidarEmail() &&
                ValidarTelefono() &&
                (rbMasculino.Checked || rbFemenino.Checked || rbOtros.Checked) &&
                ValidarFechaNacimiento();
        }

        private bool ValidarDNI()
        {
            return tbDNI.Text.Length >= 8;
        }

        private bool ValidarEmail()
        {
            if (string.IsNullOrWhiteSpace(tbEmail.Text))
                return false;

            return Regex.IsMatch(
                tbEmail.Text,
                @"^[^@\s]+@[^@\s]+\.com$",
                RegexOptions.IgnoreCase);
        }

        private bool ValidarTelefono()
        {
            return string.IsNullOrWhiteSpace(tbTelefono.Text) || tbTelefono.Text.Length >= 10;
        }

        private bool ValidarFechaNacimiento()
        {
            if (!fechaNacimiento.Checked)
                return true;

            DateTime hoy = DateTime.Today;
            DateTime fn = fechaNacimiento.Value.Date;
            if (fn > hoy)
                return false;

            int edad = hoy.Year - fn.Year;
            if (fn > hoy.AddYears(-edad))
                edad--;

            return edad >= 18 && edad <= 100;
        }

        private void tbNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbApellido_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbDNI_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbTelefono_TextChanged(object sender, EventArgs e)
        {

        }

        private void fechaNacimiento_ValueChanged(object sender, EventArgs e)
        {

        }

        private void rbMasculino_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbFemenino_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbOtros_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            var cliente = new Cliente_model
            {
                nombre = tbNombre.Text,
                apellido = tbApellido.Text,
                email = tbEmail.Text,
                sexo = rbMasculino.Checked ? "Masculino" : rbFemenino.Checked ? "Femenino" : "Otros",
                fecha_nacimiento = fechaNacimiento.Checked ? fechaNacimiento.Value.Date : (DateTime?)null,
                telefono = string.IsNullOrWhiteSpace(tbTelefono.Text) ? null : long.Parse(tbTelefono.Text),
                dni = int.Parse(tbDNI.Text)
            };

            int resultado = Cliente_controller.agregarCliente(cliente);
            if (resultado > 0)
                MessageBox.Show("Cliente registrado con éxito.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Error al registrar el cliente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            FiltrarYRefrescar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            vaciarCampos();
            FiltrarYRefrescar();
        }

        private void FiltrarYRefrescar()
        {
            var clientes = ObtenerClientes();

            // Se aplica un filtro por activo/inactivo
            var filtrados = clientes.Where(u => (cbActivos.Checked && u.activo == 1) || (cbInactivos.Checked && u.activo == 0)).ToList();

            // Si hay texto en el DNI, filtrar por prefijo
            var textoDni = tbBusquedaDniCliente.Text.Trim();
            if (textoDni.Length > 0 && textoDni.All(char.IsDigit))
            {
                filtrados = filtrados.Where(u => u.dni.ToString().StartsWith(textoDni)).ToList();
            }

            // Si hay texto en el Nombre, filtrar por contenido
            var textoNom = tbBusquedaNombreCliente.Text.Trim();
            if (textoNom.Length > 0)
            {
                filtrados = filtrados.Where(u => u.nombre.IndexOf(textoNom, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            // Se refresca el DataGridView con la lista filtrada
            dgvClientes.DataSource = null;
            dgvClientes.DataSource = filtrados;
        }

        private List<Cliente_model> ObtenerClientes()
        {
            var lista = new List<Cliente_model>();
            using var con = ProyectoNetshop.BD.BaseDeDatos.obtenerConexion();
            using var cmd = con.CreateCommand();
            cmd.CommandText = @"SELECT id_cliente, nombre, apellido, email, activo, sexo, fecha_nacimiento, telefono, dni FROM cliente";
            using var rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                DateTime? fn = rd.IsDBNull(6) ? (DateTime?)null : rd.GetDateTime(6);
                long? tel = rd.IsDBNull(7) ? (long?)null : rd.GetInt32(7);

                var cliente = new Cliente_model
                {
                    id_cliente = rd.GetInt32(0),
                    nombre = rd.GetString(1),
                    apellido = rd.GetString(2),
                    email = rd.GetString(3),
                    activo = rd.GetInt32(4),
                    sexo = rd.GetString(5),
                    fecha_nacimiento = fn,
                    telefono = tel,
                    dni = rd.GetInt32(8),
                };

                lista.Add(cliente);
            }

            return lista;
        }

        private void vaciarCampos()
        {
            tbNombre.Text = "";
            tbApellido.Text = "";
            tbDNI.Text = "";
            tbEmail.Text = "";
            tbTelefono.Text = "";
            rbMasculino.Checked = false;
            rbFemenino.Checked = false;
            rbOtros.Checked = true;
            fechaNacimiento.Value = DateTime.Today;

            btnGuardar.Enabled = false;
        }

        private void tbBusquedaDniCliente_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbBusquedaDniCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                MessageBox.Show("Sólo se permiten números.", "Carácter no permitido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tbBusquedaNombreCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsLetter(e.KeyChar)
                && e.KeyChar != ' ')
            {
                e.Handled = true;
                MessageBox.Show("Sólo se permiten letras.", "Carácter no permitido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbActivos_CheckedChanged(object sender, EventArgs e)
        {
            FiltrarYRefrescar();
        }

        private void cbInactivos_CheckedChanged(object sender, EventArgs e)
        {
            FiltrarYRefrescar();
        }
    }
}
