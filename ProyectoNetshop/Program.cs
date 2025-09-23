using Microsoft.VisualBasic.Logging;
using ProyectoNetshop;
using System;
using System.Windows.Forms;
using vistaDeProyectoC;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        // Ajusta estilos y configuraci�n visual antes de crear formularios
        ApplicationConfiguration.Initialize();

        // Crea el formulario de inicio de sesi�n
        using var login = new FInicioSesion();

        // Pregunta si es usuario valid�
        if (login.ShowDialog() == DialogResult.OK)
        {
            Application.Run(new principal(
                login.IdPerfil,
                login.VendedorDni,
                login.VendedorNombre));
        }
    }
}