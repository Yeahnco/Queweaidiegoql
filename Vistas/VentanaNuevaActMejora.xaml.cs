using Controladores;
using MahApps.Metro.Controls;
using PersistenciaBD;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Shapes;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para VentanaNuevaActMejora.xaml
    /// </summary>
    public partial class VentanaNuevaActMejora : MetroWindow
    {
        public VentanaNuevaActMejora()
        {
            InitializeComponent();
            datePickerFechaActMejora.DisplayDateStart = DateTime.Now.AddDays(2);
            timePickerHoraActMejora.SourceHours = horas;
            timePickerHoraActMejora.SourceMinutes = minutos;
            FormatoCalendario();
        }
        public int idCliente;
        public int idProfesional;
        int[] horas = { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
        int[] minutos = { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };
        private void FormatoCalendario()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-CL");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-CL");
            datePickerFechaActMejora.Language = XmlLanguage.GetLanguage("es-CL");
        }
        private bool ValidarCampos()
        {
            if (txtboxNomActMejora.Text == ""
                || txtboxDescripcionActMejora.Text == ""
                || datePickerFechaActMejora.SelectedDate == null
                || timePickerHoraActMejora.SelectedDateTime == null
                && txtboxNomActMejora.Text == ""
                && txtboxDescripcionActMejora.Text == ""
                && datePickerFechaActMejora.SelectedDate == null
                && timePickerHoraActMejora.SelectedDateTime == null)
            {
                MessageBox.Show("Se encontraron campos vacíos en la ventana, favor de rellenar todos los campos.", "Validación de campos");
                return false;
            }
            else
            {
                return true;
            }
        }
        private static int Ultimo_idActividad()
        {
            ServiceActividad serviceActividad = new();
            int ultimoID = 0;
            foreach (Actividad _actividad in serviceActividad.GetEntities())
            {
                if (ultimoID < _actividad.id_act)
                {
                    ultimoID = _actividad.id_act;
                }
            }
            return ultimoID;
        }
        private void CrearActividad(int idProfesional, int idCliente)
        {
            using BD_NMAEntities contextActividad = new();
            contextActividad.CREATE_ACTIVIDAD
                (
                    fecha_act: datePickerFechaActMejora.SelectedDate,
                    hora_act: timePickerHoraActMejora.SelectedDateTime,
                    contador: 1,
                    prof_id_profe: idProfesional,
                    cliente_id_emp: idCliente,
                    tipo_actividad: "Actividad de mejora",
                    estado: 0,
                    retraso: 0
                );
            contextActividad.SaveChanges();
        }
        private void CrearActDeMejora(int idActividad, int idProfesional)
        {
            using BD_NMAEntities contextActMejora = new();
            contextActMejora.CREATE_ACT_DE_MEJORA
            (
                nombre_act_mejora: txtboxNomActMejora.Text,
                descripcion_act_mejora: txtboxDescripcionActMejora.Text,
                actividad_id_act: idActividad,
                revision_profesional: "Revisión pendiente",
                prof_emisor_id: idProfesional,
                prof_remitente_id: null,
                estado_actividad: "Sin asignación",
                estado_asignacion: "Pendiente"
            );
            contextActMejora.SaveChanges();
        }

        private void TileGuardar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("La información a continuación será ingresada. \n¿Esta seguro(a) que la información ingresada es correcta?", "Pregunta de confirmación", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (ValidarCampos() == true)
                {
                    CrearActividad(idProfesional, idCliente);
                    CrearActDeMejora(Ultimo_idActividad(), idProfesional);
                    MessageBox.Show("Actividad de mejora creada correctamente.");
                    this.Close();
                }
                else if (ValidarCampos() == false)
                {
                    MessageBox.Show("No se pudo crear la actividad de mejora.");
                }
            }
            else if (messageBoxResult == MessageBoxResult.No)
            {
            }

        }
        private void TileAtras_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
