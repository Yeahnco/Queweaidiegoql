using PersistenciaBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para TarjetaSolicitud.xaml
    /// </summary>
    public partial class TarjetaSolicitud : UserControl
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public TarjetaSolicitud()
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        {
            InitializeComponent();
            DataContext = this;
        }
        public int idSolicitud { get; set; }
        public string DisplayNombreEmpresa { get; set; }
        public string DisplayRutEmpresa { get; set; }
        public string DisplayNombreGerente { get; set; }
        public string DisplayNombreProfesional { get; set; }
        public string DisplayRazonSolicitud { get; set; }
        public string DisplayFechaSolicitud { get; set; }
        public string DisplayHoraSolicitud { get; set; }
        public string DisplayDescripcionSolicitud { get; set; }
        public string DisplayNombreSolicitud { get; set; }

        private void GridPrincipal_Initialized(object sender, EventArgs e)
        {
            dockPanelCentral.Visibility = Visibility.Collapsed;
            ucTarjetaSolicitud.Height.Equals(107.837837837838);
        }
        private void BtnUpdDown_Click(object sender, RoutedEventArgs e)
        {
            if (dockPanelCentral.Visibility.Equals(Visibility.Collapsed))
            {
                ucTarjetaSolicitud.Height.Equals(400);
                dockPanelCentral.Visibility = Visibility.Visible;
            }
            else if (dockPanelCentral.Visibility.Equals(Visibility.Visible))
            {
                ucTarjetaSolicitud.Height.Equals(107.837837837838);
                dockPanelCentral.Visibility = Visibility.Collapsed;
            }
        }
        public void ActualizarAsesoria(int idSolicitud, string EstadoNuevo_Solicitud)
        {
            using BD_NMAEntities contextActualizar = new();
            contextActualizar.crudUpdate
                (
                    nombreTabla: "Solicitud",
                    nombreColumna: "Estado_solicitud",
                    nuevoDato: EstadoNuevo_Solicitud,
                    id: idSolicitud

                );
            contextActualizar.SaveChanges();
        }

        private void tileAceptarSolicitud_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Está seguro de que desea aceptar esta solicitud? \nDe ser así se asignará a su perfil", "Pregunta de confirmación", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                ActualizarAsesoria(idSolicitud, "Aceptada");
                MessageBox.Show("Solicitud aceptada.");
            }
            else if (messageBoxResult == MessageBoxResult.No)
            {
            }
        }
        private void tileRechazarSolicitud_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Está seguro de que desea rechazar esta solicitud? \nDe ser así no podrá visualizarla nuevamente.", "Pregunta de confirmación", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                ActualizarAsesoria(idSolicitud, "Rechazada");
                MessageBox.Show("Solicitud rechazada.");
            }
            else if (messageBoxResult == MessageBoxResult.No)
            {
            }

        }
    }
}
