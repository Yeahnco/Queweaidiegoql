using Controladores;
using PersistenciaBD;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Lógica de interacción para TarjetaRevision.xaml
    /// </summary>
    public partial class TarjetaRevision : UserControl
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public TarjetaRevision()
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        {
            InitializeComponent();
            DataContext = this;
        }
        public int idProf_Emisor { get; set; }
        public int idActividadMejora { get; set; }
        public string DisplayNombreEmpresa { get; set; }
        public string DisplayRutEmpresa { get; set; }
        public string DisplayNombreGerente { get; set; }
        public string DisplayNombreProfesional { get; set; }
        public string DisplayNombreActMejora { get; set; }
        public string DisplayFechaActMejora { get; set; }
        public string DisplayHoraActMejora { get; set; }
        public string DisplayDescripcionActMejora { get; set; }
        public string CreateInformacionDeImportancia { get; set; }
        public string DisplayProfAsignado { get; set; }
        private void GridPrincipal_Initialized(object sender, EventArgs e)
        {
            dockPanelMedio.Visibility = Visibility.Collapsed;
            dockPanelInferior.Visibility = Visibility.Collapsed;
            ucTarjetaRevision.Height.Equals(89.775);
        }
        private bool ValidarCampos()
        {
            if (txtBoxInfoExtra.Text == "")
            {
                MessageBox.Show("Se encontraron campos vacíos en la ventana, favor de rellenar todos los campos.", "Validación de campos");
                return false;
            }
            else
            {
                return true;
            }
            return false;
        }
        private void BtnUpdDown_Click(object sender, RoutedEventArgs e)
        {
            if (dockPanelMedio.Visibility.Equals(Visibility.Collapsed)
                && dockPanelInferior.Visibility.Equals(Visibility.Collapsed))
            {
                ucTarjetaRevision.Height.Equals(400);
                dockPanelMedio.Visibility = Visibility.Visible;
                dockPanelInferior.Visibility = Visibility.Visible;
            }
            else if (dockPanelMedio.Visibility.Equals(Visibility.Visible)
                && dockPanelInferior.Visibility.Equals(Visibility.Visible))
            {
                ucTarjetaRevision.Height.Equals(89.775);
                dockPanelMedio.Visibility = Visibility.Collapsed;
                dockPanelInferior.Visibility = Visibility.Collapsed;
            }
        }
        public void BloqueoTileTomar(int idPerfil, int idProf)
        {
            if (idPerfil == idProf)
            {
                TileTomar.IsEnabled = false;
                TileCumplida.IsEnabled = false;
                TileNoCumplida.IsEnabled = false;
                txtBoxInfoExtra.IsEnabled = false;
            }
        }
        public void ActividadTomada(string estado)
        {
            if (estado == "Tomada")
            {
                TileTomar.Content = "Tomada";
                TileTomar.IsEnabled = false;
            }
        }
        public void ActualizarActMejora(int idProfesionalRemitente, int idActividadSeleccionada)

        {
            ServiceProfesional serviceProfesional = new();
            string nombreprof = serviceProfesional.GetEntity(idProfesionalRemitente).Nombre_prof;
            string apeprof = serviceProfesional.GetEntity(idProfesionalRemitente).Apellido_prof;
            using BD_NMAEntities contextActividadMejoraRemitente = new();
            contextActividadMejoraRemitente.crudUpdate
                (
                    nombreTabla: "Act_de_mejora",
                    nombreColumna: "Remitente",
                    nuevoDato: idProfesionalRemitente.ToString(),
                    id: idActividadSeleccionada
                );
            contextActividadMejoraRemitente.SaveChanges();

            using BD_NMAEntities contextActividadMejoraEstadoActividad = new();
            contextActividadMejoraEstadoActividad.crudUpdate
                (
                    nombreTabla: "Act_de_mejora",
                    nombreColumna: "Estado_actividad",
                    nuevoDato: "Tomada",
                    id: idActividadSeleccionada
                );
            contextActividadMejoraEstadoActividad.SaveChanges();

            using BD_NMAEntities contextActividadMejoraEstadoAsignacion = new();
            contextActividadMejoraEstadoAsignacion.crudUpdate
                (
                    nombreTabla: "Act_de_mejora",
                    nombreColumna: "Estado_asignacion",
                    nuevoDato: "Asignada a: " + nombreprof + " " + apeprof,
                    id: idActividadSeleccionada
                );
            contextActividadMejoraEstadoAsignacion.SaveChanges();
        }
        public void GenerarReporte(string revision, int idActividadSeleccionada)
        {
            using BD_NMAEntities contextRevisionProfesional = new();
            contextRevisionProfesional.crudUpdate
                (
                    nombreTabla: "Act_de_mejora",
                    nombreColumna: "Revision_profesional",
                    nuevoDato: revision,
                    id: idActividadSeleccionada
                );
            contextRevisionProfesional.SaveChanges();
        }
        public void ActualizarPorReporte(int idActividadSeleccionada)
        {
            using BD_NMAEntities contextActividadMejoraEstadoActividad = new();
            contextActividadMejoraEstadoActividad.crudUpdate
                (
                    nombreTabla: "Act_de_mejora",
                    nombreColumna: "Estado_actividad",
                    nuevoDato: "Revisada",
                    id: idActividadSeleccionada
                );
            contextActividadMejoraEstadoActividad.SaveChanges();
        }
        private void TileTomar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("La actividad a continuación será tomada. \n¿Esta seguro(a) que desea tomar la actividad seleccionada?", "Pregunta de confirmación", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                ActualizarActMejora(MainWindow.IdProfesional, idActividadMejora);
            }
            else if (messageBoxResult == MessageBoxResult.No)
            {
            }
        }

        private void TileCumplida_Click(object sender, RoutedEventArgs e)
        {
            string reporte = "Cumplida: " + txtBoxInfoExtra.Text;
            MessageBoxResult messageBoxResult = MessageBox.Show("La información del reporte a continuación será ingresada. \n¿Esta seguro(a) que la información ingresada es correcta?", "Pregunta de confirmación", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (ValidarCampos() == true)
                {
                    GenerarReporte(reporte, idActividadMejora);
                    ActualizarPorReporte(idActividadMejora);
                }
                else if (ValidarCampos() == false)
                {
                    MessageBox.Show("No se pudo crear el reporte.");
                }
            }
            else if (messageBoxResult == MessageBoxResult.No)
            {
            }
        }
        private void TileNoCumplida_Click(object sender, RoutedEventArgs e)
        {
            string reporte = "No Cumplida: " + txtBoxInfoExtra.Text;
            MessageBoxResult messageBoxResult = MessageBox.Show("La información del reporte a continuación será ingresada. \n¿Esta seguro(a) que la información ingresada es correcta?", "Pregunta de confirmación", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (ValidarCampos() == true)
                {
                    GenerarReporte(reporte, idActividadMejora);
                    ActualizarPorReporte(idActividadMejora);
                }
                else if (ValidarCampos() == false)
                {
                    MessageBox.Show("No se pudo crear el reporte.");
                }
            }
            else if (messageBoxResult == MessageBoxResult.No)
            {
            }
        }
    }
}
