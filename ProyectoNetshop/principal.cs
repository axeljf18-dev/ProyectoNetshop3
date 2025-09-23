using vistaDeProyectoC; // Importa el namespace con el formulario de login
using ProyectoNetshop.formularios; // Importa los formularios de la aplicaci�n

namespace ProyectoNetshop
{
    public partial class principal : Form
    {
        // Datos del usuario autenticado
        private int idPerfil;
        private int vendedorDni;
        private string vendedorNombre;

        // Constructor
        public principal(int p_idPerfil, int p_dni, string p_nombre)
        {
            InitializeComponent();

            // Llama a principal_Load al mostrarse
            this.Load += principal_Load;

            // Guardamos internamente los par�metros pasados desde el login
            idPerfil = p_idPerfil;
            vendedorDni = p_dni;
            vendedorNombre = p_nombre;

            // Validamos el acceso seg�n el perfil del usuario
            AplicarPermisos();
        }

        // Evento del bot�n �Cerrar sesi�n�
        private void iconButton6_Click(object sender, EventArgs e)
        {
            // Se muestra un mensaje
            var respuesta = MessageBox.Show("�Est�s seguro que deseas cerrar sesi�n y volver al login?", "Confirmar salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Si el usuario dice S�, reinicio la app
            if (respuesta == DialogResult.Yes)
            {
                Application.Restart();
            }
        }

        // M�todo para mostrar un Form dentro de un Panel
        private void AbrirFormularioEnPanel(object formulario)
        {
            // Si el panel ya tiene contenido, entonces se quita
            if (this.panelContenedor.Controls.Count > 0) this.panelContenedor.Controls.RemoveAt(0);

            // Convertimos el objeto a Form para tratarlo como control hijo
            Form fh = formulario as Form;

            // Indicamos que no sea ventana independiente
            fh.TopLevel = false;

            // Establecemos que ocupa todo el espacio disponible dentro del panel
            fh.Dock = DockStyle.Fill;

            // Agrega el formulario al panel y almacena una referencia en tag
            this.panelContenedor.Controls.Add(fh);
            this.panelContenedor.Tag = fh;

            // Se muestra
            fh.Show();
        }

        // Al hacer clic en el boton Usuarios, carga ese Form en el panel
        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new formularios.usuarios());
        }

        // Al hacer clic en �Reportes�, se pregunta qu� formulario de reportes mostrar seg�n el perfil de usuario
        private void btnReportes_Click(object sender, EventArgs e)
        {
            Form reporteARenderizar;

            switch (idPerfil)   // idPerfil indica rol: 1=Admin, 2=Vendedor, 3=Gerente�
            {
                case 1: // Administrador
                    reporteARenderizar = new FReporteUsuario();
                    break;

                case 2: // Vendedor
                    reporteARenderizar = new ReportesV(vendedorDni, vendedorNombre);
                    break;

                case 3: // Gerente
                    reporteARenderizar = new Reportes(idPerfil);
                    break;

                default:
                    MessageBox.Show("Perfil de usuario no reconocido.", "Error de permisos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            // Se agrega el formulario de reportes elegido en el panel principal
            AbrirFormularioEnPanel(reporteARenderizar);
        }

        // Al hacer clic en �Ventas�, abre la vista de ventas
        private void btnVentas_Click_1(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new formularios.ventas(vendedorDni, vendedorNombre));
        }

        // Al hacer clic en �Backup�, muestra el formulario de respaldo de la base de datos
        private void btnBackup_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new vistaDeProyectoC.FBackUp());
        }

        // Al hacer clic en �Clientes�, abre el formulario para registrar nuevos clientes
        private void btnClientes_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new vistaDeProyectoC.FRegistrarCliente());
        }

        // Al hacer clic en �Productos�, abre el formulario para registrar nuevos productos
        private void btnProductos_Click(object sender, EventArgs e)
        {
            AbrirFormularioEnPanel(new formularios.CrudProductos());
        }

        private void panelContenedor_Paint(object sender, PaintEventArgs e)
        {

        }

        private void principal_Load(object sender, EventArgs e)
        {
            
        }

        // Este metodo determina que secciones el usuario puede acceder dependiendo el rol del usuario
        private void AplicarPermisos()
        {
            // Se lista de todos los botones
            var botones = new[]
            {
                btnIconUsuarios, btnIconBackup, btnIconVentas, btnIconClientes, btnIconProductos, btnIconReportes
            };

            // Se deshabilita todo al inicio
            foreach (var btn in botones)
                btn.Enabled = false;

            // Habilitar seg�n idPerfil (1=Admin, 2=Vendedor, 3=Gerente)
            switch (idPerfil)
            {
                case 1: // Administrador
                    btnIconUsuarios.Enabled = true;
                    btnIconBackup.Enabled = true;
                    btnIconReportes.Enabled = true;
                    break;

                case 2: // Vendedor
                    btnIconVentas.Enabled = true;
                    btnIconClientes.Enabled = true;
                    btnIconReportes.Enabled = true;
                    break;

                case 3: // Gerente
                    btnIconVentas.Enabled = true;
                    btnIconProductos.Enabled = true;
                    btnIconReportes.Enabled = true;
                    break;

                default:
                    break;
            }
        }
    }
}
