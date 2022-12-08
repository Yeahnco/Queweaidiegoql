using Controladores;
using MahApps.Metro.Controls;
using PersistenciaBD;
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
using System.Windows.Shapes;

namespace Vistas
{
    /// <summary>
    /// Lógica de interacción para VentanaNuevaCapacitacion.xaml
    /// </summary>
    public partial class VentanaNuevaCapacitacion : MetroWindow
    {
        public VentanaNuevaCapacitacion()
        {
            InitializeComponent();
            datePickerFechaCapacitacion.DisplayDateStart = DateTime.Now.AddDays(14);
            timePickerHoraCapacitacion.SourceHours = horas;
            timePickerHoraCapacitacion.SourceMinutes = minutos;
            FormatoCalendario();
        }
        public int idCliente;
        public int idProfesional;
        int[] horas = { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
        int[] minutos = { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55 };
        public void FormatoCalendario()
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-CL");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-CL");
            datePickerFechaCapacitacion.Language = XmlLanguage.GetLanguage("es-CL");
        }
        public bool ValidarCampos()
        {
            if (txtBoxNombreCapacitacion.Text == ""
                || txtBoxCantidadAsistentes.Text == ""
                || txtBoxDescripcion.Text == ""
                || txtBoxMateriales.Text == ""
                || datePickerFechaCapacitacion.SelectedDate == null
                || timePickerHoraCapacitacion.SelectedDateTime == null
                && txtBoxNombreCapacitacion.Text == ""
                && txtBoxCantidadAsistentes.Text == ""
                && txtBoxDescripcion.Text == ""
                && txtBoxMateriales.Text == ""
                && datePickerFechaCapacitacion.SelectedDate == null
                && timePickerHoraCapacitacion.SelectedDateTime == null)
            {
                MessageBox.Show("Se encontraron campos vacíos en la ventana, favor de rellenar todos los campos.", "Validación de campos");
                return false;
            }
            else
            {
                return true;
            }
        }
        private void txtBoxCantidadAsistentes_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtBoxCantidadAsistentes.Text, "[^0-9]"))
            {
                MessageBox.Show("Cantidad asistentes debe ser un valor númerico");
                txtBoxCantidadAsistentes.Text = txtBoxCantidadAsistentes.Text.Remove(txtBoxCantidadAsistentes.Text.Length - 1);
            }
            else if (txtBoxCantidadAsistentes.Text == "0")
            {
                MessageBox.Show("Cantidad asistentes no puede ser 0.");
                txtBoxCantidadAsistentes.Clear();
            }
        }

        public static int Ultimo_idActividad()
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
        public void CrearActividad(int idProfesional, int idCliente)
        {
            using BD_NMAEntities contextActividad = new();
            contextActividad.CREATE_ACTIVIDAD
                (
                    fecha_act: datePickerFechaCapacitacion.SelectedDate,
                    hora_act: timePickerHoraCapacitacion.SelectedDateTime,
                    contador: 1,
                    prof_id_profe: idProfesional,
                    cliente_id_emp: idCliente,
                    tipo_actividad: "Capacitación",
                    estado: 0,
                    retraso: 0
                );
            contextActividad.SaveChanges();
        }
        public void CrearCapacitacion(int idActividad)
        {
            using BD_NMAEntities contextCapacitacion = new();
            contextCapacitacion.CREATE_CAPACITACION
            (
                nombre_cap: txtBoxNombreCapacitacion.Text,
                asistentes: int.Parse(txtBoxCantidadAsistentes.Text),
                descripcion: txtBoxDescripcion.Text,
                material: txtBoxMateriales.Text,
                capacitacionActividad_id_act: idActividad
            );
            contextCapacitacion.SaveChanges();

        }
        private void TileGuardar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("La información a continuación será ingresada. \n¿Esta seguro(a) que la información ingresada es correcta?", "Pregunta de confirmación", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (ValidarCampos() == true)
                {
                    CrearActividad(idProfesional, idCliente);
                    CrearCapacitacion(Ultimo_idActividad());
                    MessageBox.Show("Capacitación creada correctamente.");
                    this.Close();
                }
                else if (ValidarCampos() == false)
                {
                    MessageBox.Show("No se pudo crear la capacitación.");
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
