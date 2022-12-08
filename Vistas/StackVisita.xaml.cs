using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para StackVisita.xaml
    /// </summary>
    public partial class StackVisita : UserControl
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        public StackVisita()
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de declararlo como que admite un valor NULL.
        {
            InitializeComponent();
            DataContext = this;
            FormatoCalendario();
        }
        public static VistaProfesional objetoVistaProfesionalExistente { get; set; }
        public int idCliente;
        public int idProfesional;
        public int idVisita_Actividad;
        public string DisplayFechaVisita { get; set; }
        public string DisplayHoraVisita { get; set; }
        public void FormatoCalendario()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-CL");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-CL");
            datePickerFechaVisita.Language = XmlLanguage.GetLanguage("es-CL");
        }
        public void ToggleBtnVisita_Checked(object sender, RoutedEventArgs e)
        {
            objetoVistaProfesionalExistente.idClienteSeleccionado = idCliente;
            objetoVistaProfesionalExistente.idProfesionalPerfilActual = idProfesional;
            objetoVistaProfesionalExistente.idActividadSeleccionada = idVisita_Actividad;
            objetoVistaProfesionalExistente.ActivarChecklist();
        }
        private void ToggleBtnVisita_Unchecked(object sender, RoutedEventArgs e)
        {
            objetoVistaProfesionalExistente.DesactivarChecklist();
        }
    }
}
